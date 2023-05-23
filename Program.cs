using Microsoft.Extensions.DependencyInjection;
using Time;

Console.OutputEncoding = System.Text.Encoding.UTF8;
// Similar to Clear() but also clears the scrollable buffer, which Clear() doesn't.
Console.Write("\f\u001bc\x1b[3J");
Console.CursorVisible = false;

var services = Configurator.Configure();

await services.GetRequiredService<MainLoop>().Run();
