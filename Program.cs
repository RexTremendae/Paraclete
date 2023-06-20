using Microsoft.Extensions.DependencyInjection;
using Paraclete;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var services = await Configurator.Configure();
if (services.GetRequiredService<Settings>().EnableLogging)
{
    Configurator.ConfigureLogging();
}

await services.GetRequiredService<MainLoop>().Run();
