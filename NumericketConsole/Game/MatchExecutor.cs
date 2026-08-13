using ConsoleLibrary;
using NumericketConsoleApp.Models;
using NumericketConsoleApp.Toss;

namespace NumericketConsoleApp.Game;

public class MatchExecutor
{
    private readonly IConsoleManager _consoleManager;
    private readonly INumericketTossManager _tossManager;
    public MatchExecutor(IConsoleManager consoleManager, INumericketTossManager tossManager)
    {
        _consoleManager = consoleManager ?? throw new ArgumentNullException(nameof(consoleManager));
        _tossManager = tossManager ?? throw new ArgumentNullException(nameof(tossManager));
    }

    public void StartMatch(Match match)
    {
        ValidateMatch(match);

        var homeTeamName = match.HomeTeam.TeamName;
        var awayTeamName = match.AwayTeam.TeamName;

        _consoleManager.DisplayMessage(ConsoleMessageType.INFORMATION, "Welcome to Numericket!\n");

        _consoleManager.DisplayMessage(ConsoleMessageType.INFORMATION, $"It's a {match.Overs} over(s) match between {homeTeamName} and {awayTeamName}");

        _consoleManager.GetInputCharacter(ConsoleMessageType.DEFAULT, "\n\n\n\nPress any key to begin...");

        _consoleManager.Clear();

        _consoleManager.DisplayMessage(ConsoleMessageType.DEFAULT, $"Lets have a quick look at the team\n");
        _consoleManager.DisplayTable(ConsoleMessageType.INFORMATION, [homeTeamName, awayTeamName], new string[][]
      {
           match.HomeTeam.Players.Select(x => x.ToString()).ToArray(),
           match.AwayTeam.Players.Select(x => x.ToString()).ToArray()
      }, TableSpacing.SPACE_BETWEEN);

        _consoleManager.GetInputCharacter(ConsoleMessageType.DEFAULT, "\n\n\n\nPress any key to continue...");

        _consoleManager.Clear();

        _tossManager.SetTeamNames(homeTeamName, awayTeamName);
        match.IsHomeTeamBattingFirst = _tossManager.IsHomeTeamBattingFirst();


        _consoleManager.GetInputCharacter(ConsoleMessageType.DEFAULT, "\n\n\n\nPress any key to start the first innings...");
        _consoleManager.Clear();
    }

    private void ValidateMatch(Match match)
    {
        ArgumentNullException.ThrowIfNull(match);

        if (match.Overs < 1)
            throw new ArgumentOutOfRangeException(nameof(match.Overs), "Match must have at least 1 over.");

        if (match.HomeTeam == null || string.IsNullOrWhiteSpace(match.HomeTeam.TeamName))
            throw new ArgumentException("A valid home team must be specified.", nameof(match));

        if (match.AwayTeam == null || string.IsNullOrWhiteSpace(match.AwayTeam.TeamName))
            throw new ArgumentException("A valid away team must be specified.", nameof(match));

        if (string.Equals(match.HomeTeam.TeamName, match.AwayTeam.TeamName, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Home team and Away team cannot have the same name.", nameof(match));
    }
}
