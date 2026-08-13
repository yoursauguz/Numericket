using ConsoleLibrary;

namespace TossLibrary;

/// <summary>
/// Serves as base class for the toss manager which is operated based on heads or tails strategy
/// Implements the supporting methods for the heads or tails style of toss execution pipeline for initiating the toss, capturing the caller choice, determining if the caller has won and displaying the result
/// </summary>
public abstract class AbstractHeadsOrTailsTossManager : AbstractTossManager
{

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractHeadsOrTailsTossManager"/> class with the specified console manager.
    /// </summary>
    /// <param name="consoleManager">The console manager instance used to handle I/O operations.</param>
    protected AbstractHeadsOrTailsTossManager(IConsoleManager consoleManager) : base(consoleManager)
    {
    }

    /**
     * ABSTRACT METHODS
     */

    /// <summary>
    /// Displays the result of the tossed coin.
    /// </summary>
    /// <param name="isHeads"><c>true</c> if the coin ends up showing heads; otherwise, <c>false</c>.</param>
    protected abstract void DisplayTossedCoinResult(bool isHeads);

    /**
     * OVERRIDDEN / IMPLEMENTED METHODS 
     */

    /// <inheritdoc />
    protected override bool EvaluateTossOutcome(int selectedOption)
    {
        // simulates a coin flip, and checks if the coin has landed showing heads
        bool isResultHeads = Random.Shared.Next(1, 101) % 2 != 0;

        // displays the result of the coin flip
        DisplayTossedCoinResult(isResultHeads);

        // checks if heads has been called by the caller
        var isHeadsSelectedByCaller = selectedOption == 1;

        // returns the result if caller had won the toss
        return isHeadsSelectedByCaller == isResultHeads;
    }
}
