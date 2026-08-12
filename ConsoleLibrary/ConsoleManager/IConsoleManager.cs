namespace ConsoleLibrary.ConsoleManager;

public interface IConsoleManager
{
    public int GetAllowedNumericInput(IReadOnlyDictionary<char, int> allowedValues, string errorMessage = "");

    public int[] GetAllowedNumericInputs(IReadOnlyDictionary<char, int>[] allowedValuesList, string[]? acknowledgements = null, string errorMessage = "");

    public void DisplayMessage(ConsoleMessageType type, string message, string title = "");

    public void DisplayTable(string[] columnHeaders, string[][] table, TableSpacing spacing = TableSpacing.CENTER);

}