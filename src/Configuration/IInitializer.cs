namespace Paraclete.Configuration;

public interface IInitializer
{
    Task Initialize(IServiceProvider services);
}
