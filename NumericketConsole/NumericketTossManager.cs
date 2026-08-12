using TossLibrary.TossManager;
using ConsoleLibrary.ConsoleManager;

namespace Numericket;

/// <summary>
/// Manages and executes the toss strategy required for the numericket game and identifies which team is gonna bat first.
/// </summary>
public class NumericketTossManager : AbstractOddOrEvenTossManager
{
    /// <summary>
    /// Possible input options for the team one to use during toss.
    /// </summary>
    private readonly IReadOnlyDictionary<char, int> _homeTeamInputs = new Dictionary<char, int>() { { 'Q', 1 }, { 'W', 2 }, { 'E', 3 }, { 'A', 4 }, { 'S', 5 }, { 'D', 6 } };

    /// <summary>
    /// Possible input options for the team two to use during toss.
    /// </summary>
    private readonly IReadOnlyDictionary<char, int> _awayTeamInputs = new Dictionary<char, int>() { { 'I', 1 }, { 'O', 2 }, { 'P', 3 }, { 'J', 4 }, { 'K', 5 }, { 'L', 6 } };

    /// <summary>
    /// The console manager instance used to handle I/O operations.
    /// </summary>
    private readonly IConsoleManager _consoleManager;

    /// <summary>
    /// Name of the home team
    /// </summary>
    public string HomeTeamName { get; init; } = "Home Team";


    /// <summary>
    /// Name of the away team
    /// </summary>
    public string AwayTeamName { get; init; } = "Away Team";

    /// <summary>
    /// Initializes a new instance of the <see cref="NumericketTossManager"/> class with the specified console manager.
    /// </summary>
    /// <param name="consoleManager">The console manager instance used to handle I/O operations.</param>
    public NumericketTossManager(IConsoleManager consoleManager) : base(consoleManager)
    {
        _consoleManager = consoleManager;
    }

    /**
     * OVERRIDDEN / IMPLEMENTED METHODS
     */

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override IReadOnlyDictionary<char, int> GetAllowedInputsForTossCaller()
    {
        return _awayTeamInputs
        .Where(kvp => kvp.Value == 1 || kvp.Value == 2)
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override IReadOnlyDictionary<char, int> GetAllTeamOneTossInputs()
    {
        return _homeTeamInputs;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override IReadOnlyDictionary<char, int> GetAllTeamTwoTossInputs()
    {
        return _awayTeamInputs;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void DisplayPromptToCallForToss()
    {
        _consoleManager.DisplayMessage(ConsoleMessageType.DEFAULT, "Its time for the toss\n");
        _consoleManager.DisplayMessage(ConsoleMessageType.INFORMATION, "Its your call Odd or Even\n", AwayTeamName);
        _consoleManager.DisplayTable([], new string[][]
        {
           _awayTeamInputs.Where(input => input.Value == 1).Select(x => x.Key + " - Odd").ToArray(),
           _awayTeamInputs.Where(input => input.Value == 2).Select(x => x.Key + " - Even").ToArray(),
        }, TableSpacing.SPACE_BETWEEN);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void DisplayCalledTossChoice(int selectedOption)
    {
        _consoleManager.DisplayMessage(ConsoleMessageType.SUCCESS, $"{AwayTeamName} has chosen - {(selectedOption == 1 ? "odd" : "even")}");
    }


    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void DisplayTossWinner(bool didCallerWin)
    {
        _consoleManager.DisplayMessage(ConsoleMessageType.SUCCESS, "Won the toss", $"{(didCallerWin ? AwayTeamName : HomeTeamName)}");
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void DisplayPromptToGetTossInputsFromAllParties()
    {
        _consoleManager.DisplayMessage(ConsoleMessageType.DEFAULT, "Enter the number to proceed for the toss");
        _consoleManager.DisplayTable([HomeTeamName, AwayTeamName], new string[][]
        {
           _homeTeamInputs.Select(x => x.Key + " - " + x.Value).ToArray(),
           _awayTeamInputs.Select(x => x.Key + " - " + x.Value).ToArray(),
        }, TableSpacing.SPACE_BETWEEN);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void DisplayInputSpecifiedByBothParties(int teamOneInput, int teamTwoInput)
    {
        _consoleManager.DisplayTable([HomeTeamName, AwayTeamName], new string[][]
       {
           [teamOneInput.ToString()],
           [teamTwoInput.ToString()],
       }, TableSpacing.SPACE_BETWEEN);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override int GetWaitTimeForEvaluatingToss()
    {
        return 2000;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override string GetTeamOneName()
    {
        return HomeTeamName;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override string GetTeamTwoName()
    {
        return AwayTeamName;
    }

    /**
     * PUBLIC METHODS
     */

    /// <summary>
    /// Tells whether the home team is going to bat first.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the home team has found a way to bat first; otherwise, <c>false</c>.
    /// </returns>
    public bool IsHomeTeamBattingFirst()
    {
        // checks if the away team has won the toss
        var didCallerWin = DidCallerWin();

        // asks the winning team to choose to bat or bowl
        _consoleManager.DisplayMessage(ConsoleMessageType.INFORMATION, "What do you want to do first ? Bat or Bowl", $"{(didCallerWin ? AwayTeamName : HomeTeamName)}");

        var winningTeamInputs = didCallerWin ? _awayTeamInputs : _homeTeamInputs;

        _consoleManager.DisplayTable([], new string[][]
       {
           winningTeamInputs.Where(input => input.Value == 1).Select(x => x.Key + " - Bat").ToArray(),
           winningTeamInputs.Where(input => input.Value == 2).Select(x => x.Key + " - Bowl").ToArray(),
       }, TableSpacing.SPACE_BETWEEN);

        // reads the response from the winning team and checks whether batting was elected by the winning team
        var isBatting = _consoleManager.GetAllowedNumericInput(winningTeamInputs
        .Where(kvp => kvp.Value == 1 || kvp.Value == 2)
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value), "") == 1;

        // displays the toss summary and tells whether the home team is going to bat first or not
        _consoleManager.DisplayMessage(ConsoleMessageType.SUCCESS, "Wins the toss", $"{(didCallerWin ? AwayTeamName : HomeTeamName)}");
        _consoleManager.DisplayMessage(ConsoleMessageType.SUCCESS, $"And chose to {(isBatting ? "bat" : "bowl")} first");
        return !didCallerWin && isBatting;
    }
}
