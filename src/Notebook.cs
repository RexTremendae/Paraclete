namespace Paraclete;

using System.Text.Json;

public class Notebook : IInitializer
{
    private const string _notesFilename = "notes.json";

    private readonly List<string> _sections = [];

    private Dictionary<string, string[]> _notes = [];

    public IEnumerable<string> Sections => _notes.Keys;
    public int SelectedSectionIndex { get; private set; }
    public string SelectedSection => SelectedSectionIndex < _sections.Count
        ? _sections[SelectedSectionIndex]
        : string.Empty;

    public IEnumerable<string> GetSections()
    {
        return _notes.Keys;
    }

    public IEnumerable<string> GetNotes(string? section = null)
    {
        return _notes.TryGetValue(section ?? SelectedSection, out var notes)
            ? notes
            : [];
    }

    public async Task Initialize(IServiceProvider services)
    {
        if (!File.Exists(_notesFilename))
        {
            return;
        }

        var fileContent = await File.ReadAllTextAsync(_notesFilename);
        var data = JsonSerializer.Deserialize<Dictionary<string, string[]>>(fileContent);

        if (data == null)
        {
            return;
        }

        _notes = data;
        _sections.Clear();
        _sections.AddRange(_notes.Keys);
        SelectedSectionIndex = 0;
    }

    public void SelectNextSection()
    {
        SelectedSectionIndex++;
        if (SelectedSectionIndex >= _sections.Count)
        {
            SelectedSectionIndex = 0;
        }
    }

    public void SelectPreviousSection()
    {
        SelectedSectionIndex--;
        if (SelectedSectionIndex < 0)
        {
            SelectedSectionIndex = _sections.Count - 1;
        }
    }
}
