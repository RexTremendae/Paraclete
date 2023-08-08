namespace Paraclete.Configuration;

using Paraclete.Calculator;
using Paraclete.Chess;
using Paraclete.Chess.Scenarios;
using Paraclete.IO;
using Paraclete.Menu;
using Paraclete.Painting;
using Paraclete.Screens;
using Paraclete.Screens.Chess;
using Paraclete.Screens.Showroom;
using Paraclete.Screens.Unicode;

public static class Configurator
{
    public static async Task<IServiceProvider> Configure()
    {
        var services = new ServiceConfigurator();

        services
            .AddScoped<CalculatorHistory>()
            .AddScoped<ChessBoard>()
            .AddScoped<DataInputPainter>()
            .AddScoped<DataInputter>()
            .AddScoped<ExhibitionSelector>()
            .AddScoped<Expression>()
            .AddScoped<FpsCounter>()
            .AddScoped<MainLoop>()
            .AddScoped<MenuPainter>()
            .AddScoped<Painter>()
            .AddScoped<PieceSelectionService>()
            .AddScoped<ScenarioSelector>()
            .AddScoped<ScreenInvalidator>()
            .AddScoped<ScreenSaver>()
            .AddScoped<ScreenSelector>()
            .AddScoped<Settings>()
            .AddScoped<Stopwatch>()
            .AddScoped<Terminator>()
            .AddScoped<ToDoList>()
            .AddScoped<ToDoListPainter>()
            .AddScoped<UnicodeControl>()

            .AddImplementationsOf<ICommand>()
            .AddImplementationsOf<IExhibition>()
            .AddImplementationsOf<IInputDefinition>()
            .AddImplementationsOf<IScenario>()
            .AddImplementationsOf<IScreen>()
            .AddImplementationsOf<MenuBase>()
        ;

        var serviceProvider = services.BuildServiceProvider();
        await services.InvokeInitializers(serviceProvider);

        return serviceProvider;
    }

    public static void ConfigureLogging()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}
