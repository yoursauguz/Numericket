using CacheManagerLibrary;
using NumericketConsoleApp.Models;

namespace NumericketConsoleApp.Data;

/// <summary>
/// <inheritdoc/>
/// </summary>
public class PlayerCacheRepository : AbstractJsonCacheRepository<List<Player>>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    protected override string GetFilePath()
    {
        string baseDir = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName
                     ?? Directory.GetCurrentDirectory();

        return Path.Combine(baseDir, "cache", "players.json");
    }

    /// <summary>
    /// Checks if the provided jersey number is unique across the players present in the list.
    /// </summary>
    /// <param name="jerseyNumber">The jersey number to match</param>
    /// <returns></returns>
    public bool IsUniqueJerseyNumber(int jerseyNumber)
    {
        return Data.Where(p => p.JerseyNumber == jerseyNumber).Count() == 0;
    }

    /// <summary>
    /// Adds a new player to the repository.
    /// </summary>
    /// <param name="jerseyNumber">Jersey number of the player</param>
    /// <param name="name">Name of the player</param>
    /// <param name="password">Password of the player</param>
    public void AddPlayer(int jerseyNumber, string name, string password)
    {
        Player player = new Player { JerseyNumber = jerseyNumber, Name = name, Password = password };
        Data.Add(player);
    }

    /// <summary>
    /// Deletes a player based on the provided jersey number.
    /// </summary>
    /// <param name="jerseyNumber">Jersey number of the player</param>
    public void DeletePlayer(int jerseyNumber)
    {
        Data = Data.Where(p => p.JerseyNumber != jerseyNumber).ToList();
    }

    /// <summary>
    /// Edits a player name by the jersey number.
    /// </summary>
    /// <param name="jerseyNumber">Jersey number of the player</param>
    /// <param name="newName">The new name to be updated</param>
    public void EditPlayerName(int jerseyNumber, string newName)
    {
        var player = Data.Where(p => p.JerseyNumber == jerseyNumber).FirstOrDefault();

        if (player == null || String.IsNullOrEmpty(player.Password))
            return;

        player.Name = newName;
    }

    /// <summary>
    /// Tells whether the player is can be authorized by the password entered.
    /// </summary>
    /// <param name="jerseyNumber">Jersey number of the player</param>
    /// <param name="password">Password of the player</param>
    /// <returns></returns>
    public bool canAuthorizePlayer(int jerseyNumber, string password)
    {
        var player = Data.Where(p => p.JerseyNumber == jerseyNumber).FirstOrDefault();

        if (player == null || String.IsNullOrEmpty(player.Password))
            return false;

        return PasswordHasher.VerifyPassword(password, player.Password);
    }

    /// <summary>
    /// Gets the player information to be printed in separate columns.
    /// </summary>
    /// <returns>the column based data of players to be printed</returns>
    public string[][] GetPlayerInformationToPrint()
    {
        var list = Data?.ToList();
        if (list == null || list.Count == 0)
            return Array.Empty<string[]>();

        var jerseyNumbers = new string[list.Count];
        var names = new string[list.Count];

        for (int i = 0; i < list.Count; i++)
        {
            jerseyNumbers[i] = '#' + list[i].JerseyNumber.ToString();
            names[i] = list[i].Name ?? string.Empty;
        }

        return new[] { jerseyNumbers, names };
    }

    /// <summary>
    /// gets the number of players present in the list
    /// </summary>
    /// <returns></returns>
    public int GetPlayerCount()
    {
        if (Data == null)
            return 0;

        return Data.Count;
    }
}
