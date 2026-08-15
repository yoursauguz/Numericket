using System.Text.Json;

namespace CacheManagerLibrary;

/// <summary>
/// JSON file based cache repository that can save and load data from the file and store data in memory.
/// </summary>
/// <typeparam name="T">Type of object to be cached</typeparam>
public abstract class AbstractJsonCacheRepository<T> : ICacheable<T> where T : class, new()
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected T Data { get; set; } = new();

    /**
     * ABSTRACT / VIRTUAL METHODS 
     */

    /// <summary>
    /// Gets the file path in which the json file to be stored.
    /// </summary>
    /// <returns></returns>
    protected abstract string GetFilePath();

    /// <summary>
    /// Gets the serialization option for the json persistence.
    /// </summary>
    /// <returns>the serialization option to be used for the json persistence</returns>
    protected virtual JsonSerializerOptions GetJsonSerializationOption() => new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        IncludeFields = true
    };

    /**
     * PUBLIC METHODS
     */

    /// <inheritdoc/> 
    /// <exception cref="ArgumentNullException">
    /// Thrown when file path is not specified to the repository call or left empty.
    /// </exception> 
    /// <exception cref="InvalidOperationException">
    /// Thrown when save operation cannot be completed successfully.
    /// </exception> 
    public async Task SaveAsync()
    {
        // get the file path to save the data and check if its null or empty
        var filePath = GetFilePath()?.Trim();

        if (String.IsNullOrEmpty(filePath))
            throw new ArgumentNullException("file path cannot be null or empty");

        // create a new directory based on the file path if required
        string? directory = Path.GetDirectoryName(filePath);

        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        // write the data to a temporary file
        string tempFilePath = $"{filePath}.tmp";

        try
        {
            await using (FileStream stream = File.Create(tempFilePath))
            {
                await JsonSerializer.SerializeAsync(stream, Data, GetJsonSerializationOption());
            }

            // replace the content from temp file to the actual file if actual file is present
            if (File.Exists(filePath))
            {
                File.Replace(tempFilePath, filePath, null);
                return;
            }

            // use the temp file as actual file if the actual file is not present already
            File.Move(tempFilePath, filePath);

        }
        catch (Exception ex)
        {
            // remove the temp file if found
            if (File.Exists(tempFilePath))
                File.Delete(tempFilePath);

            throw new InvalidOperationException("Unable to save the contents to the file", ex);
        }
    }


    /// <inheritdoc/> 
    /// <exception cref="ArgumentNullException">
    /// Thrown when file path is not specified to the repository call or left empty.
    /// </exception> 
    /// <exception cref="InvalidOperationException">
    /// Thrown when load operation cannot be completed successfully.
    /// </exception> 
    public async Task LoadAsync()
    {
        // get the file path and check if it is null or empty
        var filePath = GetFilePath()?.Trim();

        if (String.IsNullOrEmpty(filePath))
            throw new ArgumentNullException("file path cannot be null or empty");

        // prepare and return a default value if file not found in the path
        if (!File.Exists(filePath))
        {
            Data = new T();
            return;
        }

        try
        {
            // read the content if the file is present
            await using FileStream stream = File.OpenRead(filePath);

            // transform the read json content into the type object
            T? loaded = await JsonSerializer.DeserializeAsync<T>(stream, GetJsonSerializationOption());

            // load the newly read data to memory 
            Data = loaded ?? new T();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to read the file contents", ex);
        }
    }

    /// <summary>
    /// Clears the in memory cache.
    /// </summary>
    public void ClearCache()
    {
        Data = new T();
    }

    /// <summary>
    /// Clears the in memory cache and also clears the saved content in the file.
    /// </summary>
    /// <returns></returns>
    public async Task DeleteCacheAsync()
    {
        ClearCache();
        await SaveAsync();
    }
}
