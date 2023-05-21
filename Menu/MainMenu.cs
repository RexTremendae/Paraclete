using Microsoft.Extensions.DependencyInjection;
using Time.Menu.General;
using Time.Menu.Stopwatch;

namespace Time.Menu;

public class MainMenu
{
    public ICommand[] MenuItems { get; }

    public MainMenu(IServiceProvider services)
    {
        MenuItems = new ICommand[]
        {
            services.GetRequiredService<ExitApplicationCommand>(),
            services.GetRequiredService<StartStopCommand>(),
            services.GetRequiredService<ResetCommand>(),
            services.GetRequiredService<MarkTimeCommand>()
        };
    }
}
