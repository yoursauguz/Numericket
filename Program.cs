namespace Numericket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NumcricketTossManager manager = new NumcricketTossManager(new NumericketConsoleManager());
            manager.IsHomeTeamBattingFirst();
        }
    }
}
