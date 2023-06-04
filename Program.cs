using Microsoft.Extensions.DependencyInjection;
using Paraclete;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var services = Configurator.Configure();

await services.GetRequiredService<MainLoop>().Run();
