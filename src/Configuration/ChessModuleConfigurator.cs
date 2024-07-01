namespace Paraclete.Configuration;

using Paraclete.Menu.Chess;
using Paraclete.Menu.MenuStateHandling;
using Paraclete.Modules.Chess;
using Paraclete.Screens.Chess;

public static class ChessModuleConfigurator
{
    public static ServiceProviderBuilder AddChessModule(this ServiceProviderBuilder services)
    {
        services
            .AddScoped<BoardSelectionService>()
            .AddScoped<ChessBoard>()
            .AddScoped<MoveHistory>()
            .AddScoped<BoardSelectionService>()
            .AddScoped<PossibleMovesTracker>()
            .AddScoped<ScenarioSelector>()
            .AddScoped<SpecialMovesCalculator>()

            .AddSingleton<MenuStateMachine<ChessMenuState, ChessMenuStateCommand>>()

            .AddImplementationsOf<MenuStateBase<ChessMenuState, ChessMenuStateCommand>>()
        ;

        return services;
    }
}
