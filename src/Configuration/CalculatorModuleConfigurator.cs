namespace Paraclete.Configuration;

using Paraclete.Modules.Calculator;

public static class CalculatorModuleConfigurator
{
    public static ServiceProviderBuilder AddCalculatorModule(this ServiceProviderBuilder services)
    {
        services
            .AddScoped<CalculatorHistory>()
            .AddScoped<Expression>()
        ;

        return services;
    }
}
