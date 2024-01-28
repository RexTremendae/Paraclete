namespace Paraclete.Modules.GitNavigator;

using System.Text.Json;

public class RepositorySelector : IInitializer
{
    private const string _gitSettingsFilename = "git.json";

    private string[] _repositories = [];

    public string SelectedRepository { get; private set; } = string.Empty;

    public string[] GetRepositories()
    {
        return _repositories;
    }

    public async Task Initialize(IServiceProvider services)
    {
        if (!File.Exists(_gitSettingsFilename))
        {
            return;
        }

        var fileContent = await File.ReadAllTextAsync(_gitSettingsFilename);
        var data = JsonSerializer.Deserialize<string[]>(fileContent);

        if (data == null)
        {
            return;
        }

        _repositories = data;
        SelectedRepository = _repositories.First();
    }
}
