namespace Paraclete.Menu.Calculator;

public class CalculatorMenu(IServiceProvider services)
    : MenuBase(services, new Type[]
    {
        typeof(CalculateExpressionCommand),
        typeof(ClearHistoryCommand),
        typeof(ConvertRadixCommand),
    })
{
}
