
namespace Numericket.Common.ConsoleManager
{
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
                return;

            var defaultColor = DefaultConsoleForegroundColor();

            var color = type switch
            {
                ConsoleMessageType.ERROR => ConsoleColor.Red,
                ConsoleMessageType.INFORMATION => ConsoleColor.Yellow,
                ConsoleMessageType.SUCCESS => ConsoleColor.Green,
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

        public void DisplayTable(string[] columnHeaders, string[][] table, TableSpacing spacing = TableSpacing.CENTER)
        {
            if (table == null)
                throw new ArgumentNullException("Table cannot be null");

            int maximumRowCount = table.Select(row => row.Length).DefaultIfEmpty(0).Max();

            if (maximumRowCount == 0)
                return;

            if (columnHeaders != null)
                RenderRowsByColumnValues(columnHeaders, spacing);

            for (int i = 0; i < maximumRowCount; i++)
            {
                string[] columns = new string[table.Length];
                Array.Fill(columns, " ");

                for (int j = 0; j < table.Length; j++)
                {
                    if (table[j].Length <= i)
                        continue;

                    columns[j] = table[j][i];
                }

                RenderRowsByColumnValues(columns.ToArray(), spacing);
            }
        }

        private void RenderRowsByColumnValues(string[] columns, TableSpacing spacing = TableSpacing.CENTER)
        {
            if (columns.Length == 0) return;

            int characters = columns.Sum(column => column.Length);

            if (characters > MaximumCharactersPerRow())
                throw new InvalidDataException("Character limit exceeds the maximum characters per row");

            int difference = MaximumCharactersPerRow() - characters;

            string rowToRender = string.Empty;

            if (spacing == TableSpacing.CENTER)
            {
                int space = difference / 2;
                rowToRender = new string(' ', space) + string.Join(" ", columns) + " " + new string(' ', space);
                Console.WriteLine(rowToRender);
            }

            else if (spacing == TableSpacing.SPACE_BETWEEN)
            {
                rowToRender = string.Join(new string(' ', difference), columns);
                Console.WriteLine(rowToRender);
            }
        }
    }
}
