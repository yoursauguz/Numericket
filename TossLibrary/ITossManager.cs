namespace TossLibrary;

/// <summary>
/// Defines a contract for executing a toss sequence and determining a winner.
/// </summary>
public interface ITossManager
{
    /// <summary>
    /// Prompts for toss inputs, processes the toss logic and determines whether the toss is won by the caller.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the caller won the toss; otherwise <c>false</c>.
    /// </returns>
    public bool DidCallerWin();
}
