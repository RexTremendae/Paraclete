namespace Paraclete.Menu.Calculator;

public class CalculatorMenu(IServiceProvider services)
    : MenuBase(services, [
        typeof(CalculateExpressionCommand),
        typeof(ClearHistoryCommand),
        typeof(ConvertRadixCommand),
    ])
{
}
