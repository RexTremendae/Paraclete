namespace Paraclete.Configuration;

using Paraclete.IO;
using Paraclete.Menu;
using Paraclete.Modules.Calculator;
using Paraclete.Modules.Chess;
using Paraclete.Modules.Chess.Scenarios;
using Paraclete.Modules.GitNavigator;
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
            .AddScoped<DataInputPainter>()
            .AddScoped<DataInputter>()
            .AddScoped<ExhibitionSelector>()
            .AddScoped<Expression>()
            .AddScoped<FpsCounter>()
            .AddScoped<MainLoop>()
            .AddScoped<MenuPainter>()
            .AddScoped<NotebookPainter>()
            .AddScoped<Notebook>()
            .AddScoped<Painter>()
            .AddScoped<ScreenInvalidator>()
            .AddScoped<ScreenSaver>()
            .AddScoped<ScreenSelector>()
            .AddScoped<Settings>()
            .AddScoped<Stopwatch>()
            .AddScoped<Terminator>()
            .AddScoped<ToDoList>()
            .AddScoped<ToDoListPainter>()
            .AddScoped<UnicodeControl>()

            .AddChessModule()
            .AddGitNavigatorModule()

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

    public static ServiceConfigurator AddGitNavigatorModule(this ServiceConfigurator services)
    {
        services
            .AddScoped<RepositorySelector>()
            .AddScoped<GitRepositorySelectorPainter>()
            .AddScoped<GitLogPainter>()
            .AddScoped<LogStore>()
        ;

        return services;
    }

    public static ServiceConfigurator AddChessModule(this ServiceConfigurator services)
    {
        services
            .AddScoped<ChessBoard>()
            .AddScoped<MoveHistory>()
            .AddScoped<PieceSelectionService>()
            .AddScoped<PossibleMovesTracker>()
            .AddScoped<ScenarioSelector>()
            .AddScoped<SpecialMovesCalculator>()
        ;

        return services;
    }

    public static void ConfigureLogging()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}
