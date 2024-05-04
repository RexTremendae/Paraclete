namespace Paraclete.Configuration;

using Paraclete.Modules.Chess;
using Paraclete.Screens.Chess;

public static class ChessModuleConfigurator
{
    public static ServiceProviderBuilder AddChessModule(this ServiceProviderBuilder services)
    {
        services
            .AddScoped<ChessBoard>()
            .AddScoped<MoveHistory>()
            .AddScoped<PieceSelectionService>()
            .AddScoped<PossibleMovesTracker>()
            .AddScoped<ScenarioSelector>()
            .AddScoped<SpecialMovesCalculator>()
        ;

        return services;
    }
}
