using ConsoleLibrary;
using TossLibrary;

namespace NumericketConsoleApp.Toss;

/// <summary>
/// Manages and executes the toss strategy required for the numericket game and identifies which team is gonna bat first.
/// </summary>
public class NumericketTossManager : AbstractOddOrEvenTossManager, INumericketTossManager
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
    public string HomeTeamName { get; private set; } = "Home Team";


    /// <summary>
    /// Name of the away team
    /// </summary>
    public string AwayTeamName { get; private set; } = "Away Team";

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
        _consoleManager.DisplayTable(ConsoleMessageType.INFORMATION, [], new string[][]
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
        _consoleManager.DisplayMessage(ConsoleMessageType.SUCCESS, $"{AwayTeamName} has chosen - {(selectedOption == 1 ? "odd" : "even")} \n");
    }


    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void DisplayTossWinner(bool didCallerWin)
    {
        _consoleManager.DisplayMessage(ConsoleMessageType.SUCCESS, "Won the toss \n\n", $"{(didCallerWin ? AwayTeamName : HomeTeamName)}");
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void DisplayPromptToGetTossInputsFromAllParties()
    {
        _consoleManager.DisplayMessage(ConsoleMessageType.DEFAULT, "Enter the number to proceed for the toss");
        _consoleManager.DisplayTable(ConsoleMessageType.INFORMATION,[HomeTeamName, AwayTeamName], new string[][]
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
        _consoleManager.DisplayTable(ConsoleMessageType.INFORMATION, [HomeTeamName, AwayTeamName], new string[][]
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
    /// <inheritdoc/>
    /// </summary>
    /// <exception cref="ArgumentNullException">throws exception if home and away team names are null or empty</exception>
    public void SetTeamNames(string homeTeamName, string awayTeamName)
    {
        if (String.IsNullOrEmpty(homeTeamName))
            throw new ArgumentNullException("home team name cannot be empty.", nameof(homeTeamName));

        if (String.IsNullOrEmpty(awayTeamName))
            throw new ArgumentNullException("away team name cannot be empty.", nameof(awayTeamName));

        HomeTeamName = homeTeamName;
        AwayTeamName = awayTeamName;
    }

    ///<summary>
    ///<inheritdoc/>
    ///</summary>
    public bool IsHomeTeamBattingFirst()
    {
        // checks if the away team has won the toss
        var didCallerWin = DidCallerWin();

        // asks the winning team to choose to bat or bowl
        _consoleManager.DisplayMessage(ConsoleMessageType.INFORMATION, "What do you want to do first ? Bat or Bowl", $"{(didCallerWin ? AwayTeamName : HomeTeamName)}");

        var winningTeamInputs = didCallerWin ? _awayTeamInputs : _homeTeamInputs;

        _consoleManager.DisplayTable(ConsoleMessageType.INFORMATION, [], new string[][]
       {
           winningTeamInputs.Where(input => input.Value == 1).Select(x => x.Key + " - Bat").ToArray(),
           winningTeamInputs.Where(input => input.Value == 2).Select(x => x.Key + " - Bowl").ToArray(),
       }, TableSpacing.SPACE_BETWEEN);

        // reads the response from the winning team and checks whether batting was elected by the winning team
        var isBatting = _consoleManager.GetAllowedNumericInput(winningTeamInputs
        .Where(kvp => kvp.Value == 1 || kvp.Value == 2)
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value), "") == 1;

        // displays the toss summary and tells whether the home team is going to bat first or not
        _consoleManager.DisplayMessage(ConsoleMessageType.SUCCESS, $"Wins the toss and chose to {(isBatting ? "bat" : "bowl")} first", $"{(didCallerWin ? AwayTeamName : HomeTeamName)}");
        return !didCallerWin && isBatting;
    }
}
