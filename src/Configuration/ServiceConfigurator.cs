namespace Paraclete.Configuration;

using Paraclete.IO;
using Paraclete.Menu;
using Paraclete.Modules.Chess.Scenarios;
using Paraclete.Painting;
using Paraclete.Screens;
using Paraclete.Screens.Showroom;
using Paraclete.Screens.Unicode;

public static class ServiceConfigurator
{
    public static async Task<IServiceProvider> Configure()
    {
        var services = new ServiceProviderBuilder();

        services
            .AddScoped<BusyIndicator>()
            .AddScoped<DataInputPainter>()
            .AddScoped<DataInputter>()
            .AddScoped<ExhibitionSelector>()
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

            .AddCalculatorModule()
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
}
