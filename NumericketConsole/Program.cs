using ConsoleLibrary;
using NumericketConsoleApp.Managers;

namespace Numericket;

public class Program
{
    private IConsoleManager _consoleManager = new NumericketConsoleManager();

    public static void Main(string[] args)
    {
        Program main = new Program();
        main.StartApplication();
    }

    public void StartApplication()
    {

        try
        {
            MainMenu();

            //var homeTeam = new Team("Smashers", [new Player(10, "Neil Melendez")]);
            //var awayTeam = new Team("Thunders", [new Player(7, "Shaun Murphy")]);

            //var match = new Match { Overs = 5, HomeTeam = homeTeam, AwayTeam = awayTeam };

            //var consoleManager = new NumericketConsoleManager();

            //MatchExecutor executor = new MatchExecutor(consoleManager, new NumericketTossManager(consoleManager));
            //executor.StartMatch(match);



            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void MainMenu()
    {
        _consoleManager.DisplayMessage(ConsoleMessageType.DEFAULT, "Numericket Console Game");

        _consoleManager.DisplayTable(ConsoleMessageType.HEADING, [], new string[][]
       {
           ["Main Menu"],
       }, TableSpacing.CENTER);

        _consoleManager.NewLine();
        _consoleManager.NewLine();

        _consoleManager.DisplayTable(ConsoleMessageType.INFORMATION, ["Menus", "Options"], new string[][]
        {
           ["New Game","Manage Players","Manage Team", "Stats", "Save and Exit"],
           ["1","2","3", "4", "5"]
        }, TableSpacing.SPACE_BETWEEN);

        _consoleManager.NewLine();
        _consoleManager.NewLine();

        _consoleManager.DisplayMessage(ConsoleMessageType.DEFAULT, "Please select an option from the menu to continue");
        int menuResponse = _consoleManager.GetAllowedNumericInput(new Dictionary<char, int>() { { '1', 1 }, { '2', 2 }, { '3', 3 }, { '4', 4 }, { '5', 5 } }, "");

        switch (menuResponse)
        {

            case 2:
                {
                    ManagePlayers();
                    break;
                }
            case 5:
                {
                    break;
                }
            default:
                break;
        }
    }

    private void ManagePlayers()
    {
        _consoleManager.DisplayMessage(ConsoleMessageType.DEFAULT, "Numericket Console Game");

        _consoleManager.DisplayTable(ConsoleMessageType.HEADING, [], new string[][]
       {
           ["Manage Players"],
       }, TableSpacing.CENTER);

        _consoleManager.NewLine();
        _consoleManager.NewLine();

        _consoleManager.DisplayTable(ConsoleMessageType.INFORMATION, ["Menus", "Options"], new string[][]
        {
           ["Create New Player","Rename Player","Delete Player", "Show All Players","Main Menu"],
           ["1","2","3", "4", "5"]
        }, TableSpacing.SPACE_BETWEEN);

        _consoleManager.NewLine();
        _consoleManager.NewLine();

        _consoleManager.DisplayMessage(ConsoleMessageType.DEFAULT, "Please select an option from the menu to continue");
        int menuResponse = _consoleManager.GetAllowedNumericInput(new Dictionary<char, int>() { { '1', 1 }, { '2', 2 }, { '3', 3 }, { '4', 4 }, { '5', 5 } }, "");

        switch (menuResponse)
        {

            case 2:
                { 
                    break;
                }
            case 5:
                {
                    MainMenu();
                    break;
                }
            default:
                break;
        }
    }
}
