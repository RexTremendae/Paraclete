namespace Paraclete.Menu.Calculator;

using System.Numerics;
using Paraclete.Calculator;
using Paraclete.IO;

public class ConvertRadixCommand : IInputCommand<BigInteger>
{
    private readonly DataInputter _dataInputter;
    private readonly CalculatorHistory _calculatorHistory;

    public ConvertRadixCommand(DataInputter dataInputter, CalculatorHistory calculatorHistory)
    {
        _dataInputter = dataInputter;
        _calculatorHistory = calculatorHistory;
    }

    public ConsoleKey Shortcut => ConsoleKey.X;
    public string Description => "Convert radi[X]";

    public Task Execute()
    {
        _dataInputter.StartInput(this, $"Enter number to convert (prefix with {RadixInputDefinition.ValidPrefixesDescription}):", inputDefinition: new RadixInputDefinition());
        return Task.CompletedTask;
    }

    public Task CompleteInput(BigInteger data)
    {
        _calculatorHistory.RadixConversion = data;
        return Task.CompletedTask;
    }
}
