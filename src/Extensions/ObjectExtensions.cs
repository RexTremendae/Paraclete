namespace Paraclete.Extensions;

using System.Reflection;

public static class ObjectExtensions
{
    public static T? GetAttribute<T>(this object target)
        where T : Attribute
    {
        return target.GetType().GetCustomAttribute<T>();
    }

    public static bool HasAttribute<T>(this object target)
        where T : Attribute
    {
        return GetAttribute<T>(target) != null;
    }

    public static bool HasAttribute<T>(this Type target)
        where T : Attribute
    {
        return target.CustomAttributes.Any(_ => _.AttributeType == typeof(T));
    }
}