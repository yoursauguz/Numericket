namespace Numericket;

internal class Program
{
    static void Main(string[] args)
    {
        NumericketTossManager manager = new NumericketTossManager(new NumericketConsoleManager());
        var isHomeTeamBattingFirst = manager.IsHomeTeamBattingFirst();

        Console.ReadKey();
    }
}
