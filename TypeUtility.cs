namespace Paraclete;

public static class TypeUtility
{
    public static IEnumerable<Type> EnumerateImplementingTypesOf<T>()
    {
        return typeof(T).Assembly.GetTypes()
            .Where(_ =>
                _.IsAssignableTo(typeof(T)) &&
                !_.IsAbstract &&
                !_.HasAttribute<ExcludeFromEnumerationAttribute>());
    }

    public static IEnumerable<T> EnumerateImplementatingInstancesOf<T>(IServiceProvider services)
    {
        return EnumerateImplementingTypesOf<T>()
            .Select(_ => services.GetService(_))
            .OfType<T>();
    }
}
