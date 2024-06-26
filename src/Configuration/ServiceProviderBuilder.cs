namespace Paraclete.Configuration;

using Microsoft.Extensions.DependencyInjection;

public class ServiceProviderBuilder
{
    private readonly IServiceCollection _services = new ServiceCollection();
    private readonly List<Type> _initializers = [];

    public ServiceProviderBuilder AddScoped<T>()
        where T : class
    {
        AddScoped(typeof(T));
        return this;
    }

    public ServiceProviderBuilder AddScoped(Type type)
    {
        AddScopedInner(type);
        return this;
    }

    public ServiceProviderBuilder AddSingleton<T>()
        where T : class
    {
        AddSingleton(typeof(T));
        return this;
    }

    public ServiceProviderBuilder AddSingleton(Type type)
    {
        AddSingletonInner(type);
        return this;
    }

    public ServiceProviderBuilder AddImplementationsOf<T>()
    {
        foreach (var type in TypeEnumerator.GetDerivedInstanceTypesOf<T>())
        {
            AddScoped(type);
        }

        return this;
    }

    public IServiceProvider BuildServiceProvider()
    {
        return _services.BuildServiceProvider();
    }

    public async Task InvokeInitializers(IServiceProvider services)
    {
        foreach (var initializerType in _initializers)
        {
            await (services.GetRequiredService(initializerType) as IInitializer)
                !.Initialize(services);
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

    private void AddSingletonInner(Type type)
    {
        _services.AddSingleton(type);
        if (type.IsAssignableTo(typeof(IInitializer)))
        {
            _initializers.Add(type);
        }
    }
}
