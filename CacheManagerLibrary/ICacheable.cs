
namespace CacheManagerLibrary;

/// <summary>
/// Defines a contract to cache an object through any supporting cache mechanism
/// </summary>
/// <typeparam name="T">The type of the object to be cached</typeparam>
public interface ICacheable<T> where T : class, new()
{
    /// <summary>
    /// Saves the data to the specified path
    /// </summary>
    Task SaveAsync();

    /// <summary>
    /// Loads the data from the specified path
    /// </summary>
    /// <returns></returns>
    Task LoadAsync();
}
