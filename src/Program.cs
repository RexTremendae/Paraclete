using Microsoft.Extensions.DependencyInjection;
using Paraclete;

ConsoleConfigurator.Configure();

var services = await ServiceConfigurator.Configure();
if (services.GetRequiredService<Settings>().EnableLogging)
{
    LoggerConfigurator.ConfigureLogging();
}

await services.GetRequiredService<MainLoop>().Run();
