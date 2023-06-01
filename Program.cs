using Microsoft.Extensions.DependencyInjection;
using Time;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.Write(AnsiConstants.ClearScreen);
Console.CursorVisible = false;

var services = Configurator.Configure();

await services.GetRequiredService<MainLoop>().Run();
