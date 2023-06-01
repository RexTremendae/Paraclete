using Microsoft.Extensions.DependencyInjection;
using Time.Menu;
using Time.Screens;

namespace Time;

public static class Configurator
{
    public static IServiceProvider Configure()
    {
        var services = new ServiceCollection();

        services
            .AddScoped<FrameInvalidator>()
            .AddScoped<MainLoop>()
            .AddScoped<MainMenu>()
            .AddScoped<ScreenSaver>()
            .AddScoped<Stopwatch>()
            .AddScoped<Visualizer>()

            .AddImplementationsOf<ICommand>()
            .AddImplementationsOf<ScreenBase>()
        ;

        return services.BuildServiceProvider();
    }

    private static IServiceCollection AddImplementationsOf<T>(this IServiceCollection services)
    {
        var implementationTypes = typeof(ICommand).Assembly.GetTypes()
            .Where(_ => _.IsAssignableTo(typeof(T)) && !_.IsAbstract);
        foreach (var menuItemType in implementationTypes)
        {
            services.AddScoped(menuItemType);
        }

        return services;
    }
}
