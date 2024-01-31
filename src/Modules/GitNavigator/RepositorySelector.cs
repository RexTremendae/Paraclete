namespace Paraclete.Modules.GitNavigator;

using System.Text.Json;

public class RepositorySelector(LogStore logStore) : IInitializer
{
    private const string _gitSettingsFilename = "git.json";

    private readonly LogStore _logStore = logStore;

    private string[] _repositories = [];

    private int _selectdRepositoryIndex;

    public string SelectedRepository => _repositories[_selectdRepositoryIndex];

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
        _selectdRepositoryIndex = 0;

        await _logStore.Refresh(SelectedRepository);
    }

    public async Task SelectNext()
    {
        _selectdRepositoryIndex++;

        if (_selectdRepositoryIndex >= _repositories.Length)
        {
            _selectdRepositoryIndex = 0;
        }

        await _logStore.Refresh(SelectedRepository);
    }
}
