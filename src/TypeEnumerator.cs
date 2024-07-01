namespace Paraclete;

public static class TypeEnumerator
{
    public static IEnumerable<Type> GetDerivedInstanceTypesOf<T>()
    {
        return typeof(T).Assembly.GetTypes()
            .Where(_ =>
                _.IsAssignableTo(typeof(T)) &&
                !_.IsAbstract &&
                !_.HasAttribute<ExcludeFromEnumerationAttribute>());
    }

    public static IEnumerable<T> GetNewDerivedInstancesOf<T>()
    {
        return GetDerivedInstanceTypesOf<T>()
            .Select(_ => (T)Activator.CreateInstance(_)!);
    }

    public static IEnumerable<T> GetDerivedContainerInstancesOf<T>(IServiceProvider services)
    {
        return GetDerivedInstanceTypesOf<T>()
            .Select(services.GetService)
            .OfType<T>();
    }
}
