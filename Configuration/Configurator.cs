using Paraclete.Layouts;
using Paraclete.Menu;
using Paraclete.Painting;
using Paraclete.Screens;
using Serilog;

namespace Paraclete.Configuration;

public static class Configurator
{
    public static async Task<IServiceProvider> Configure()
    {
        var services = new ServiceConfigurator();

        services
            .AddScoped<DataInputPainter>()
            .AddScoped<DataInputter>()
            .AddScoped<ExhibitionSelector>()
            .AddScoped<FpsCounter>()
            .AddScoped<MainLoop>()
            .AddScoped<MenuPainter>()
            .AddScoped<ScreenInvalidator>()
            .AddScoped<ScreenSaver>()
            .AddScoped<ScreenSelector>()
            .AddScoped<Settings>()
            .AddScoped<Stopwatch>()
            .AddScoped<ToDoList>()
            .AddScoped<ToDoListPainter>()
            .AddScoped<Painter>()

            .AddImplementationsOf<ICommand>()
            .AddImplementationsOf<IExhibition>()
            .AddImplementationsOf<ILayout>()
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
