namespace Paraclete;

using System.Text.Json;

public class Notebook : IInitializer
{
    private const string _notesFilename = "notes.json";

    private readonly List<string> _sections;

    private int _selectedSectionIndex;
    private Dictionary<string, string[]> _notes;

    public Notebook()
    {
        _notes = new ();
        _sections = new ();
        _selectedSectionIndex = 0;
    }

    public IEnumerable<string> Sections => _notes.Keys;
    public string SelectedSection => _selectedSectionIndex < _sections.Count
        ? _sections[_selectedSectionIndex]
        : string.Empty;

    public IEnumerable<string> GetSections()
    {
        return _notes.Keys;
    }

    public IEnumerable<string> GetNotes()
    {
        return _notes.TryGetValue(SelectedSection, out var notes)
            ? notes
            : Array.Empty<string>();
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
        _selectedSectionIndex = 0;
    }

    public void SelectNextSection()
    {
        _selectedSectionIndex++;
        if (_selectedSectionIndex >= _sections.Count)
        {
            _selectedSectionIndex = 0;
        }
    }

    public void SelectPreviousSection()
    {
        _selectedSectionIndex--;
        if (_selectedSectionIndex < 0)
        {
            _selectedSectionIndex = _sections.Count - 1;
        }
    }
}
