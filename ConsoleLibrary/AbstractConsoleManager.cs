namespace ConsoleLibrary;

public abstract class AbstractConsoleManager : IConsoleManager
{
    protected virtual int MaximumCharactersPerRow() => 100;

    protected virtual ConsoleColor DefaultConsoleForegroundColor() => ConsoleColor.Gray;

    protected virtual string FormatMessage(string message, string title)
    {
        if (String.IsNullOrEmpty(message))
            return "";

        if (String.IsNullOrEmpty(title))
            return message;

        return $"{title}! - {message}";
    }

    public void DisplayMessage(ConsoleMessageType type, string message, string title = "")
    {
        if (String.IsNullOrEmpty(message))
            throw new ArgumentNullException("Message cannot be empty");

        var defaultColor = DefaultConsoleForegroundColor();

        var color = type switch
        {
            ConsoleMessageType.ERROR => ConsoleColor.Red,
            ConsoleMessageType.INFORMATION => ConsoleColor.Yellow,
            ConsoleMessageType.SUCCESS => ConsoleColor.Green,
            ConsoleMessageType.HEADING => ConsoleColor.Blue,
            _ => ConsoleColor.Gray
        };

        if (color != defaultColor)
            Console.ForegroundColor = color;

        Console.WriteLine(FormatMessage(message, title));

        if (color != defaultColor)
            Console.ForegroundColor = defaultColor;
    }

    public int GetAllowedNumericInput(IReadOnlyDictionary<char, int> allowedValues, string errorMessage = "")
    {
        if (allowedValues == null)
            throw new ArgumentNullException("Allowed values should not be null");

        if (allowedValues.Count == 0)
            throw new ArgumentException("Allowed values should not be empty");

        while (true)
        {
            ConsoleKeyInfo input = Console.ReadKey(intercept: true);
            char keyChar = char.ToUpper(input.KeyChar);

            if (allowedValues.TryGetValue(keyChar, out int result))
            {
                Console.Clear();
                return result;
            }

            if (!String.IsNullOrEmpty(errorMessage))
                DisplayMessage(ConsoleMessageType.ERROR, errorMessage);
        }
    }

    public int[] GetAllowedNumericInputs(IReadOnlyDictionary<char, int>[] allowedValuesList, string[]? acknowledgements = null, string errorMessage = "")
    {
        if (allowedValuesList == null)
            throw new ArgumentNullException("Allowed values list should not be null");


        foreach (var allowedValues in allowedValuesList)
        {
            if (allowedValues == null || allowedValues.Count == 0)
                throw new ArgumentNullException("Allowed values should not be null or empty");
        }

        acknowledgements ??= allowedValuesList
            .Select((_, index) => $"Input {index + 1} is ready")
            .ToArray();

        int totalRequired = allowedValuesList.Length;

        if (acknowledgements.Length != totalRequired)
            throw new ArgumentException("Acknowledgement list must have exact number of items as allowed values list");

        int[] inputs = new int[totalRequired];
        Array.Fill(inputs, -1);

        int filledCount = 0;
        while (true)
        {
            ConsoleKeyInfo input = Console.ReadKey(intercept: true);
            char keyChar = char.ToUpper(input.KeyChar);
            bool isSuccess = false;

            for (int i = 0; i < totalRequired; i++)
            {

                if (inputs[i] == -1 && allowedValuesList[i].TryGetValue(keyChar, out int result))
                {
                    inputs[i] = result;
                    isSuccess = true;
                    filledCount++;
                    DisplayMessage(ConsoleMessageType.SUCCESS, acknowledgements[i]);
                    break;
                }
            }

            if (filledCount == totalRequired)
            {
                Console.Clear();
                return inputs;
            }

            if (!String.IsNullOrEmpty(errorMessage) && !isSuccess)
                DisplayMessage(ConsoleMessageType.ERROR, errorMessage);
        }
    }

    public void DisplayTable(ConsoleMessageType type, string[] columnHeaders, string[][] table, TableSpacing spacing = TableSpacing.CENTER)
    {
        ArgumentNullException.ThrowIfNull(table);

        int maximumRowCount = table.Select(row => row.Length).DefaultIfEmpty(0).Max();

        if (maximumRowCount == 0)
            return;

        string divider = new string('=', MaximumCharactersPerRow());

        if (columnHeaders != null && columnHeaders.Length > 0)
        {
            DisplayMessage(ConsoleMessageType.DEFAULT, divider);
            RenderRowsByColumnValues(ConsoleMessageType.HEADING, columnHeaders, spacing);
        }

        DisplayMessage(ConsoleMessageType.DEFAULT, divider);

        for (int i = 0; i < maximumRowCount; i++)
        {
            string[] columns = new string[table.Length];
            Array.Fill(columns, " ");

            for (int j = 0; j < table.Length; j++)
            {
                if (table[j].Length > i)
                {
                    columns[j] = table[j][i];
                }
            }

            RenderRowsByColumnValues(type, columns, spacing);
        }

        DisplayMessage(ConsoleMessageType.DEFAULT, divider);
    }

    public void NewLine()
    {
        Console.WriteLine();
    }

    public void Clear()
    {
        Console.Clear();
    }


    public char GetInputCharacter(ConsoleMessageType type, string message, string title = "")
    {
        DisplayMessage(type, message, title);
        ConsoleKeyInfo input = Console.ReadKey(intercept: true);
        return char.ToUpper(input.KeyChar);
    }
    private void RenderRowsByColumnValues(ConsoleMessageType type, string[] columns, TableSpacing spacing = TableSpacing.CENTER)
    {
        if (columns.Length == 0)
            return;

        int interColumnSpaces = spacing == TableSpacing.CENTER && columns.Length > 1 ? columns.Length - 1 : 0;
        int totalTextLength = columns.Sum(column => column.Length) + interColumnSpaces;

        if (totalTextLength > MaximumCharactersPerRow())
            throw new InvalidDataException("Character limit exceeds the maximum characters per row.");

        int difference = MaximumCharactersPerRow() - totalTextLength;

        if (spacing == TableSpacing.CENTER)
        {
            int sidePadding = difference / 2;
            string rowToRender = new string(' ', sidePadding) + string.Join(" ", columns) + new string(' ', sidePadding);
            DisplayMessage(type, rowToRender);
        }
        else if (spacing == TableSpacing.SPACE_BETWEEN)
        {
            if (columns.Length == 1)
            {
                DisplayMessage(type, columns[0]);
            }
            else
            {
                int gap = difference / (columns.Length - 1);
                string rowToRender = string.Join(new string(' ', gap), columns);
                DisplayMessage(type, rowToRender);
            }
        }
    }
}
