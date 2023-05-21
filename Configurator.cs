using Microsoft.Extensions.DependencyInjection;

namespace Time;

public static class Configurator
{
    public static IServiceProvider Configure()
    {
        var services = new ServiceCollection();

        services.AddScoped<MainLoop>();
        services.AddScoped<Visualizer>();
        services.AddScoped<StopWatch>();

        return services.BuildServiceProvider();
    }
}
