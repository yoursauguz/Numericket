

namespace NumericketConsoleApp.Models;

public record Player(int JerseyNumber, string PlayerName)
{
    public override string ToString()
    {
        return $"#{JerseyNumber} - {PlayerName}";
    }
}
