namespace Paraclete.Menu.Calculator;

using Paraclete.Calculator;
using Paraclete.IO;

public class CalculateExpressionCommand : IInputCommand<Expression>
{
    private readonly DataInputter _dataInputter;
    private readonly CalculatorHistory _calculatorHistory;
    private readonly ScreenInvalidator _screenInvalidator;

    public CalculateExpressionCommand(DataInputter dataInputter, CalculatorHistory calculatorHistory, ScreenInvalidator screenInvalidator)
    {
        _dataInputter = dataInputter;
        _calculatorHistory = calculatorHistory;
        _screenInvalidator = screenInvalidator;
    }

    public ConsoleKey Shortcut => ConsoleKey.E;
    public string Description => "Calculate [E]xpression";

    public Task Execute()
    {
        _dataInputter.StartInput(this, "Enter expression:");
        return Task.CompletedTask;
    }

    public Task CompleteInput(Expression data)
    {
        _calculatorHistory.AddEntry(data);
        _screenInvalidator.InvalidatePane(0);
        return Task.CompletedTask;
    }
}