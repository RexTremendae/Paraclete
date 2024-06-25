namespace Paraclete.Menu.Calculator;

using System.Numerics;
using Paraclete.IO;
using Paraclete.Modules.Calculator;
using Paraclete.Screens;

public class ConvertRadixCommand(DataInputter dataInputter, CalculatorHistory calculatorHistory, ScreenInvalidator screenInvalidator)
    : IInputCommand<BigInteger>
{
    private readonly DataInputter _dataInputter = dataInputter;
    private readonly CalculatorHistory _calculatorHistory = calculatorHistory;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.X;
    public string Description => "Convert radi[X]";

    public Task Execute()
    {
        _dataInputter.StartInput(
            this,
            $"Enter number to convert (prefix with {RadixInputDefinition.ValidPrefixesDescription}):",
            inputDefinition: new RadixInputDefinition());

        return Task.CompletedTask;
    }

    public Task CompleteInput(BigInteger data)
    {
        _calculatorHistory.RadixConversion = data;
        _screenInvalidator.InvalidatePane(CalculatorScreen.Panes.Radix);
        return Task.CompletedTask;
    }
}
