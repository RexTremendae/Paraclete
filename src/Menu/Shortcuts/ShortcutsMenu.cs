namespace Paraclete.Menu.Shortcuts;

using System.Text.Json;

public class ShortcutsMenu(IServiceProvider services)
    : MenuBase(services, [
        typeof(ExitApplicationCommand),
        typeof(StartTaskManagerCommand),
        typeof(OutlookCommand),
        typeof(PowerShellCommand),
        typeof(HibernateCommand),
    ]), IInitializer
{
    private const string _shortcutsFilename = "shortcuts.json";

    public async Task Initialize(IServiceProvider services)
    {
        if (!File.Exists(_shortcutsFilename))
        {
            return;
        }

        var json = await File.ReadAllTextAsync(_shortcutsFilename);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var shortcutList = JsonSerializer.Deserialize<CustomShortcutList>(json, options)?.Shortcuts
            ?? [];

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
        public CustomShortcut[] Shortcuts { get; set; } = [];
    }

    public class CustomShortcut
    {
        public string Key { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string LongDescription { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public string[] Arguments { get; set; } = [];
    }
}
