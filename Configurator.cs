using Microsoft.Extensions.DependencyInjection;
using Time.Menu;

namespace Time;

public static class Configurator
{
    public static IServiceProvider Configure()
    {
        var services = new ServiceCollection();

        services.AddScoped<MainLoop>();
        services.AddScoped<MainMenu>();
        services.AddScoped<Visualizer>();
        services.AddScoped<Stopwatch>();
        services.AddScoped<FrameInvalidator>();

        foreach (var menuItemType in typeof(ICommand).Assembly.GetTypes().Where(_ => _.IsAssignableTo(typeof(ICommand)) && !_.IsAbstract))
        {
            services.AddScoped(menuItemType);
        }

        return services.BuildServiceProvider();
    }
}
