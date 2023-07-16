namespace Paraclete.Menu.Calculator;

public class CalculatorMenu : MenuBase
{
    public CalculatorMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(CalculateExpressionCommand),
        typeof(ClearHistoryCommand),
        typeof(ConvertRadixCommand),
    })
    {
    }
}
