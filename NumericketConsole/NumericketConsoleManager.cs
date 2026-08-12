using ConsoleLibrary.ConsoleManager;

namespace Numericket;

public class NumericketConsoleManager : AbstractConsoleManager
{
    protected override int MaximumCharactersPerRow()
    {
        return 50;
    }

    protected override string FormatMessage(string message, string title)
    {
        if (String.IsNullOrEmpty(message))
            return "";

        if (String.IsNullOrEmpty(title))
            return message;

        return $"{title.ToUpper()} - {message}";
    }
}
