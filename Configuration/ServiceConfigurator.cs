using Microsoft.Extensions.DependencyInjection;

namespace Paraclete.Configuration;

public class ServiceConfigurator
{
    private readonly IServiceCollection _services;
    private List<Type> _initializers;

    public ServiceConfigurator()
    {
        _services = new ServiceCollection();
        _initializers = new();
    }

    public ServiceConfigurator AddScoped<T>() where T : class
    {
        AddScoped(typeof(T));
        return this;
    }

    public ServiceConfigurator AddScoped(Type type)
    {
        AddScopedInner(type);
        return this;
    }

    public ServiceConfigurator AddImplementationsOf<T>()
    {
        foreach (var menuItemType in TypeUtility.EnumerateImplementingTypesOf<T>())
        {
            AddScoped(menuItemType);
        }

        return this;
    }

    public IServiceProvider BuildServiceProvider()
    {
        return _services.BuildServiceProvider();
    }

    public async Task InvokeInitializers(IServiceProvider serviceProvider)
    {
        foreach (var initializerType in _initializers)
        {
            await (serviceProvider.GetRequiredService(initializerType) as IInitializer)!.Initialize();
        }
    }

    private void AddScopedInner(Type type)
    {
        _services.AddScoped(type);
        if (type.IsAssignableTo(typeof(IInitializer)))
        {
            _initializers.Add(type);
        }
    }
}
