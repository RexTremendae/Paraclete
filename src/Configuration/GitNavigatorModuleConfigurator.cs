using Paraclete.Modules.GitNavigator;
using Paraclete.Painting;

public static class GitNavigatorModuleConfigurator
{
    public static ServiceProviderBuilder AddGitNavigatorModule(this ServiceProviderBuilder services)
    {
        services
            .AddScoped<GitLogPainter>()
            .AddScoped<GitRepositorySelectorPainter>()
            .AddScoped<LogStore>()
            .AddScoped<RepositorySelector>()
        ;

        return services;
    }
}
