#pragma warning disable SA1200 // Using directive should appear within a namespace declaration

using Microsoft.Extensions.DependencyInjection;
using Paraclete;

#pragma warning restore SA1200 // Using directive should appear within a namespace declaration

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Disable Ctrl+C
Console.CancelKeyPress += (sender, e) =>
{
    e.Cancel = true;
};

var services = await Configurator.Configure();
if (services.GetRequiredService<Settings>().EnableLogging)
{
    Configurator.ConfigureLogging();
}

await services.GetRequiredService<MainLoop>().Run();
