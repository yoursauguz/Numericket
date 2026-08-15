namespace ConsoleLibrary;

public interface IConsoleManager
{
    public void NewLine();

    public void Clear();

    public void DisplayMessage(ConsoleMessageType type, string message, string title = "");

    public void DisplayTable(ConsoleMessageType type, string[] columnHeaders, string[][] table, TableSpacing spacing = TableSpacing.CENTER);

    public string GetLine(ConsoleMessageType type, string message, string title = "");

    public string GetPassword(ConsoleMessageType type, string message, string title = "", char mask = '*');

    public char GetInputCharacter(ConsoleMessageType type, string message, string title = "");

    public int GetAllowedNumericInput(IReadOnlyDictionary<char, int> allowedValues, string errorMessage = "");

    public int[] GetAllowedNumericInputs(IReadOnlyDictionary<char, int>[] allowedValuesList, string[]? acknowledgements = null, string errorMessage = "");
}