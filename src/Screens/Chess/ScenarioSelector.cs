namespace Paraclete.Screens.Chess;

using Paraclete.Modules.Chess.Scenarios;

public class ScenarioSelector(IServiceProvider services)
{
    private readonly IServiceProvider _services = services;
    private readonly List<IScenario> _scenarios = [];

    private int _selectedScenarioIndex;

    public IScenario SelectedScenario => _scenarios[_selectedScenarioIndex];
    public IEnumerable<IScenario> Scenarios => _scenarios;

    public void StartSelectScenario()
    {
        _selectedScenarioIndex = 0;

        if (!_scenarios.Any())
        {
            TypeEnumerator.GetDerivedContainerInstancesOf<IScenario>(_services).Foreach(_ => Log.Information(_.Name));
            _scenarios.AddRange(TypeEnumerator.GetDerivedContainerInstancesOf<IScenario>(_services));
        }
    }

    public void MoveSelectionMarkerUp()
    {
        var selectedScenarioIndex = _selectedScenarioIndex - 1;
        _selectedScenarioIndex = selectedScenarioIndex < 0
            ? _scenarios.Count - 1
            : selectedScenarioIndex;
    }

    public void MoveSelectionMarkerDown()
    {
        var selectedScenarioIndex = _selectedScenarioIndex + 1;
        _selectedScenarioIndex = selectedScenarioIndex >= _scenarios.Count
            ? 0
            : selectedScenarioIndex;
    }
}
