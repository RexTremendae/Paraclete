namespace Paraclete.Menu.Calculator;

using Paraclete.Modules.Calculator;
using Paraclete.IO;

public class CalculateExpressionCommand(DataInputter dataInputter, CalculatorHistory calculatorHistory, ScreenInvalidator screenInvalidator)
    : IInputCommand<Expression>
{
    private readonly DataInputter _dataInputter = dataInputter;
    private readonly CalculatorHistory _calculatorHistory = calculatorHistory;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

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