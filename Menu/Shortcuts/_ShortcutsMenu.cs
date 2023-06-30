using System.Text.Json;

namespace Paraclete.Menu.Shortcuts;

public class _ShortcutsMenu : MenuBase, IInitializer
{
    private const string _shortcutsFilename = "shortcuts.json";

    public _ShortcutsMenu(IServiceProvider services)
        : base(services, new Type[]
    {
        typeof(ExitApplicationCommand),
        typeof(StartTaskManagerCommand),
        typeof(OutlookCommand),
        typeof(PowerShellCommand),
        typeof(HibernateCommand)
    })
    { }

    public async Task Initialize()
    {
        if (!File.Exists(_shortcutsFilename))
        {
            return;
        }

        var json = await File.ReadAllTextAsync(_shortcutsFilename);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var shortcutList = JsonSerializer.Deserialize<CustomShortcutList>(json, options)?.Shortcuts
            ?? Array.Empty<CustomShortcut>();

        foreach (var shortcut in shortcutList)
        {
            var shortcutKey = Enum.Parse<ConsoleKey>(shortcut.Key);
            AddCommand(shortcutKey, new CustomShortcutCommand(
                shortcutKey,
                shortDescription: shortcut.ShortDescription,
                longDescription: shortcut.LongDescription,
                command: shortcut.Command,
                arguments: shortcut.Arguments
            ));
        }
    }

    public class CustomShortcutList
    {
        public CustomShortcut[] Shortcuts { get; set; } = Array.Empty<CustomShortcut>();
    }

    public class CustomShortcut
    {
        public string Key { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string LongDescription { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public string[] Arguments { get; set; } = Array.Empty<string>();
    }
}
