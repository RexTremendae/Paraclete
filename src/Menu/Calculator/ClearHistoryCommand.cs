namespace Paraclete.Menu.Calculator;

using Paraclete.Calculator;

public class ClearHistoryCommand : ICommand
{
    private readonly CalculatorHistory _calculatorHistory;
    private readonly ScreenInvalidator _screenInvalidator;

    public ClearHistoryCommand(CalculatorHistory calculatorHistory, ScreenInvalidator screenInvalidator)
    {
        _calculatorHistory = calculatorHistory;
        _screenInvalidator = screenInvalidator;
    }

    public ConsoleKey Shortcut => ConsoleKey.C;

    public string Description => "[C]lear history";

    public Task Execute()
    {
        _calculatorHistory.Clear();
        _screenInvalidator.InvalidateAll();
        return Task.CompletedTask;
    }
}
