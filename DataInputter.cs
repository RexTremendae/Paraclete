using System.Reflection;
using System.Text;
using Paraclete.Menu;

namespace Paraclete;

public class DataInputter
{
    private IInputCommand? _command;
    private readonly ScreenInvalidator _screenInvalidator;
    private StringBuilder _input;
    private Type _inputType;
    private string _alphabet;

    public bool IsActive { get; private set; }
    public string CurrentInput { get; private set; }
    public string Label { get; private set; }

    public DataInputter(ScreenInvalidator screenInvalidator)
    {
        _screenInvalidator = screenInvalidator;
        _input = new();
        CurrentInput = string.Empty;
        Label = string.Empty;
        _alphabet = string.Empty;
        _inputType = typeof(object);
    }

    public void StartInput<T>(IInputCommand<T> inputCommand, string? label, NullableGeneric<T>? valueToEdit = null)
    {
        _input.Clear();
        CurrentInput = string.Empty;

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

        _inputType = typeof(T);

        if (_inputType == typeof(string))
        {
            _alphabet =
                "abcdefghijklmnopqrstuvwxyzåäö" +
                "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ" +
                "0123456789" +
                ".,:;/\\@#$%& !?_-+=()[]{}<>";
        }
        else if (_inputType == typeof(int))
        {
            _alphabet = "0123456789";
        }
        else
        {
            throw new NotSupportedException($"Input of type {typeof(T)} is not supported.");
        }

        IsActive = true;
        _screenInvalidator.Invalidate();
    }

    public async Task Input(ConsoleKeyInfo keyInfo)
    {
        var key = keyInfo.Key;

        if (key == ConsoleKey.Enter)
        {
            IsActive = false;
            await CompleteInput();
            _command = null;
            return;
        }

        if (key == ConsoleKey.Escape)
        {
            IsActive = false;
            return;
        }

        if (key == ConsoleKey.Backspace)
        {
            if (_input.Length > 0)
            {
                _input.Remove(_input.Length-1, 1);
                CurrentInput = _input.ToString();
            }
            return;
        }

        var keyChar = keyInfo.KeyChar;
        if (_alphabet.Contains(keyChar))
        {
            _input.Append(keyChar);
            CurrentInput = _input.ToString();
        }
    }

    private async Task CompleteInput()
    {
        const BindingFlags publicInstanceFlags = BindingFlags.Instance | BindingFlags.Public;
        var methodInfo = _command
            ?.GetType()
            ?.GetMethod(nameof(IInputCommand<int>.CompleteInput), publicInstanceFlags)
            ?? throw new InvalidOperationException("Could not find the CompleteInput method.");

        var input = _input.ToString();

        object convertedInput = new();

        if (_inputType == typeof(int))
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                convertedInput = 0;
            }
            else
            {
                convertedInput = int.Parse(input);
            }
        }
        else if(_inputType == typeof(string))
        {
            convertedInput = input;
        }
        else
        {
            throw new NotSupportedException($"Input of type '{_inputType.Name}' not supported.");
        }

        await (Task)(methodInfo!.Invoke(_command, new object[] { convertedInput }))!;
    }
}
