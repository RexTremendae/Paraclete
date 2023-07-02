namespace Paraclete.IO;

using System.Reflection;
using System.Text;
using Paraclete.Menu;

public class DataInputter
{
    private readonly ScreenInvalidator _screenInvalidator;

    private IInputCommand? _command;
    private StringBuilder _input = new ();
    private Dictionary<Type, IInputDefinition> _availableInputters = new ();
    private IInputDefinition _selectedInputter;

    public DataInputter(ScreenInvalidator screenInvalidator, IServiceProvider services)
    {
        _screenInvalidator = screenInvalidator;
        _selectedInputter = IInputDefinition.NoInputter;

        foreach (var dataInputter in TypeUtility.EnumerateImplementatingInstancesOf<IInputDefinition>(services))
        {
            _availableInputters.Add(dataInputter.DataType, dataInputter);
        }
    }

    public bool IsActive { get; private set; }
    public string CurrentInput { get; private set; } = string.Empty;
    public string ErrorMessage { get; private set; } = string.Empty;
    public AnsiString Label { get; private set; } = AnsiString.Empty;

    public Task StartInput<T>(IInputCommand<T> inputCommand, AnsiString? label, T valueToEdit)
    {
        return StartInput<T>(inputCommand, label, new NullableGeneric<T>(valueToEdit));
    }

    public Task StartInput<T>(IInputCommand<T> inputCommand, AnsiString? label, NullableGeneric<T>? valueToEdit = null)
    {
        _input.Clear();
        ErrorMessage = string.Empty;
        CurrentInput = string.Empty;
        _selectedInputter = IInputDefinition.NoInputter;

        if (valueToEdit != null)
        {
            _input.Append(valueToEdit?.Value.ToString());
            CurrentInput = _input.ToString();
        }

        if (label != null)
        {
            Label = label;
        }

        _command = inputCommand;
        if (!_availableInputters.TryGetValue(typeof(T), out var inputter))
        {
            throw new NotSupportedException($"Input of type {typeof(T)} is not supported.");
        }

        _selectedInputter = inputter;

        IsActive = true;
        _screenInvalidator.Invalidate();

        return Task.CompletedTask;
    }

    public async Task Input(ConsoleKeyInfo keyInfo)
    {
        var key = keyInfo.Key;

        if (key == ConsoleKey.Enter)
        {
            if (_selectedInputter.MinLength > _input.Length)
            {
                ErrorMessage = "Too short input, min length: " + _selectedInputter.MinLength;
                return;
            }

            if (_selectedInputter.MaxLength < _input.Length)
            {
                ErrorMessage = "Too long input, max length: " + _selectedInputter.MaxLength;
                return;
            }

            if (!_selectedInputter.TryCompleteInput(_input.ToString(), out var result, out var errorMessage))
            {
                ErrorMessage = errorMessage;
                return;
            }

            IsActive = false;
            await CompleteInput(result);
            _command = IInputCommand.NoInputCommand;
            return;
        }

        ErrorMessage = string.Empty;

        if (key == ConsoleKey.Escape)
        {
            IsActive = false;
            return;
        }

        if (key == ConsoleKey.Backspace)
        {
            if (_input.Length > 0)
            {
                _input.Remove(_input.Length - 1, 1);
                CurrentInput = _input.ToString();
            }

            return;
        }

        if (key == ConsoleKey.Delete)
        {
            _input.Clear();
            CurrentInput = string.Empty;
            return;
        }

        if (_selectedInputter.MaxLength <= _input.Length)
        {
            return;
        }

        var keyChar = keyInfo.KeyChar;
        if (_selectedInputter.Alphabet.Contains(keyChar))
        {
            _input.Append(keyChar);
            CurrentInput = _input.ToString();
        }
    }

    private async Task CompleteInput(object data)
    {
        const BindingFlags publicInstanceFlags = BindingFlags.Instance | BindingFlags.Public;
        var methodInfo = _command
            ?.GetType()
            ?.GetMethod(nameof(IInputCommand<int>.CompleteInput), publicInstanceFlags)
            ?? throw new InvalidOperationException("Could not find the CompleteInput method.");

        await (Task)(methodInfo!.Invoke(_command, new object[] { data }))!;
    }
}
