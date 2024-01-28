namespace Paraclete.Menu.Calculator;

using Paraclete.Modules.Calculator;

public class ClearHistoryCommand(CalculatorHistory calculatorHistory, ScreenInvalidator screenInvalidator)
    : ICommand
{
    private readonly CalculatorHistory _calculatorHistory = calculatorHistory;
    private readonly ScreenInvalidator _screenInvalidator = screenInvalidator;

    public ConsoleKey Shortcut => ConsoleKey.C;

    public string Description => "[C]lear history";

    public Task Execute()
    {
        _calculatorHistory.Clear();
        _screenInvalidator.InvalidateAll();
        return Task.CompletedTask;
    }
}
