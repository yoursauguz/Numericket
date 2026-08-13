using NumericketConsoleApp.Game;
using NumericketConsoleApp.Managers;
using NumericketConsoleApp.Models;
using NumericketConsoleApp.Toss;

namespace Numericket;

internal class Program
{
    static void Main(string[] args)
    {

        try
        {
            var homeTeam = new Team("Smashers", [new Player(10, "Neil Melendez")]);
            var awayTeam = new Team("Thunders", [new Player(7, "Shaun Murphy")]);

            var match = new Match { Overs = 5, HomeTeam = homeTeam, AwayTeam = awayTeam };

            var consoleManager = new NumericketConsoleManager();

            MatchExecutor executor = new MatchExecutor(consoleManager, new NumericketTossManager(consoleManager));
            executor.StartMatch(match);

            Console.ReadKey();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
