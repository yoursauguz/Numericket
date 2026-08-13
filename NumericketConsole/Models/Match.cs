

namespace NumericketConsoleApp.Models;

public class Match
{
    public required int Overs { get; init; }

    public required Team HomeTeam { get; init; }

    public required Team AwayTeam { get; init; }

    public bool IsHomeTeamBattingFirst { get; set; } = false;
}
