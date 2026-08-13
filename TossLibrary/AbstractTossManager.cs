using ConsoleLibrary;

namespace TossLibrary;

/// <summary>
/// Serves as the base class for managing the toss mechanics using the Template Method design pattern.
/// Implements the execution pipeline for initiating the toss, capturing the caller choice, determining if the caller has won and displaying the result
/// </summary>
public abstract class AbstractTossManager: ITossManager
{
    /// <summary>
    /// The console manager abstraction used to interact with the user for inputs and helps to display the output in the console.
    /// </summary>
    protected readonly IConsoleManager ConsoleManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractTossManager"/> class with the specified console manager.
    /// </summary>
    /// <param name="consoleManager">The console manager instance used to handle I/O operations.</param>
    protected AbstractTossManager(IConsoleManager consoleManager)
    {
        ConsoleManager = consoleManager;
    }

    /**
     * ABSTRACT METHODS
     */

    /// <summary>
    /// Gets the dictionary mapping of allowed input characters with their corresponding option values for the toss caller.
    /// </summary>
    /// <returns>An <see cref="IReadOnlyDictionary{TKey, TValue}"/> containing valid input keys as characters and their numeric values.</returns>
    protected abstract IReadOnlyDictionary<char, int> GetAllowedInputsForTossCaller();

    /// <summary>
    /// Displays the prompt asking the caller to make their toss choice.
    /// </summary>
    protected abstract void DisplayPromptToCallForToss();

    /// <summary>
    /// Displays the choice selected by the toss caller.
    /// </summary>
    /// <param name="selectedOption">The numeric option value selected by the caller.</param>
    protected abstract void DisplayCalledTossChoice(int selectedOption);

    /// <summary>
    /// Evaluates the outcome of the toss based on game-specific rules (e.g., Odd/Even parity or Heads/Tails).
    /// </summary>
    /// <param name="selectedOption">The numeric option selected by the caller.</param>
    /// <returns><c>true</c> if the caller won the toss evaluation; otherwise, <c>false</c>.</returns>
    protected abstract bool EvaluateTossOutcome(int selectedOption);

    /// <summary>
    /// Displays the final winner announcement for the toss.
    /// </summary>
    /// <param name="didCallerWin"><c>true</c> if the toss caller won; otherwise, <c>false</c>.</param>
    protected abstract void DisplayTossWinner(bool didCallerWin);

    /// <summary>
    /// Gets the interval in milliseconds to wait before starting the toss evaluation.
    /// </summary>
    /// <returns>An <see cref="int"/> containing the wait time in milliseconds.</returns>
    protected abstract int GetWaitTimeForEvaluatingToss();

    /**
     * PUBLIC METHODS
     */

    /// <summary>
    /// Executes the full toss workflow pipeline step-by-step.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the caller wins the toss; otherwise, <c>false</c>.
    /// </returns>
    public bool DidCallerWin()
    {
        // displays a prompt to ask the caller to call for the toss
        DisplayPromptToCallForToss();

        // gets the input from the caller
        int selectedOption = ConsoleManager.GetAllowedNumericInput(GetAllowedInputsForTossCaller());

        // displays a prompt to announce the caller's choice
        DisplayCalledTossChoice(selectedOption);

        // waits for a specific time and starts to evaluate the toss
        Thread.Sleep(GetWaitTimeForEvaluatingToss());
        var didCallerWin = EvaluateTossOutcome(selectedOption);

        // displays the prompt and gives out the decision if the caller had won the tass
        DisplayTossWinner(didCallerWin);
        return didCallerWin;
    }
}
