using ConsoleLibrary;

namespace TossLibrary;

/// <summary>
/// Serves as base class for the toss manager which is operated based on Odd or Even strategy
/// Implements the supporting methods for the Odd or Even style of toss execution pipeline for initiating the toss, capturing the caller choice, determining if the caller has won and displaying the result
/// </summary>
public abstract class AbstractOddOrEvenTossManager : AbstractTossManager
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractOddOrEvenTossManager"/> class with the specified console manager.
    /// </summary>
    /// <param name="consoleManager">The console manager instance used to handle I/O operations.</param>
    protected AbstractOddOrEvenTossManager(IConsoleManager consoleManager) : base(consoleManager)
    {
    }

    /**
     * ABSTRACT METHODS 
     */

    /// <summary>
    /// Gets the dictionary mapping of allowed input characters with their corresponding option values for team one.
    /// </summary>
    /// <returns>An <see cref="IReadOnlyDictionary{TKey, TValue}"/> containing valid input keys as characters and their numeric values.</returns>
    protected abstract IReadOnlyDictionary<char, int> GetAllTeamOneTossInputs();

    /// <summary>
    /// Gets the dictionary mapping of allowed input characters with their corresponding option values for team two.
    /// </summary>
    /// <returns>An <see cref="IReadOnlyDictionary{TKey, TValue}"/> containing valid input keys as characters and their numeric values.</returns>
    protected abstract IReadOnlyDictionary<char, int> GetAllTeamTwoTossInputs();

    /// <summary>
    /// Displays the prompt asking both the teams to choose their numbers to execute toss.
    /// </summary>
    protected abstract void DisplayPromptToGetTossInputsFromAllParties();

    /// <summary>
    /// Displays the prompt to show the choices made by both the teams.
    /// </summary>
    /// <param name="teamOneInput">The number chose by the team one.</param>
    /// <param name="teamTwoInput">The number chose by the team two.</param>
    protected abstract void DisplayInputSpecifiedByBothParties(int teamOneInput, int teamTwoInput);

    /// <summary>
    /// Gets the name of the team one.
    /// </summary>
    /// <returns>the name of the team one.</returns>
    protected abstract string GetTeamOneName();

    /// <summary>
    /// Gets the name of the team two.
    /// </summary>
    /// <returns>the name of the team two.</returns>
    protected abstract string GetTeamTwoName();

    /**
     * OVERRIDDEN / IMPLEMENTED METHODS 
     */

    /// <inheritdoc />
    protected override bool EvaluateTossOutcome(int selectedOption)
    {
        // displays prompt to get inputs from both the users 
        DisplayPromptToGetTossInputsFromAllParties();

        // reads the input from both the users
        var teamInputs = ConsoleManager.GetAllowedNumericInputs([GetAllTeamOneTossInputs(), GetAllTeamTwoTossInputs()], [GetTeamOneName()+ " is ready with the input ", GetTeamTwoName() + " is ready with the input "]);

        // adds the result of both the user inputs and check if it is an odd number
        int teamOneInput = teamInputs[0];
        int teamTwoInput = teamInputs[1];
        int total = teamOneInput + teamTwoInput;
        bool isResultOdd = total % 2 != 0;

        // displays the choice of both the users
        DisplayInputSpecifiedByBothParties(teamOneInput, teamTwoInput);

        // checks if the caller had chosen the odd number
        var isOddSelectedByCaller = selectedOption == 1;

        // decides if the caller had won and returns the result
        return isOddSelectedByCaller == isResultOdd;
    }
}
