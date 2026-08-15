

namespace NumericketConsoleApp.Models;

/// <summary>
/// Contains the information about a player 
/// </summary>
public class Player
{
    /// <summary>
    /// Unique jersey number associated to the player
    /// </summary>
    public int JerseyNumber { get; set; }

    /// <summary>
    /// Name of the player
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Secret key to be validated when the player records are being accessed
    /// </summary>
    public string? Password { get; set; }
}