using ConsoleLibrary;
using NumericketConsoleApp.Data;
namespace NumericketConsoleApp.UI;

/// <summary>
/// Numericket application user interface that shows menus to the user and get the responses and helps to proceed with the desired operations.
/// </summary>
public class NumericketApplication
{
    /// <summary>
    /// Console Manager dedicated for the numericket application.
    /// </summary>
    private readonly IConsoleManager _consoleManager;

    /// <summary>
    /// Helps to manage the list of players.
    /// </summary>
    private readonly PlayerCacheRepository _playerRepository = new PlayerCacheRepository();

    /// <summary>
    /// Initializes a new instance of the <see cref="NumericketApplication"/> class with the specified console manager.
    /// </summary>
    /// <param name="consoleManager">The console manager instance used to handle I/O operations.</param>
    public NumericketApplication(IConsoleManager consoleManager)
    {
        _consoleManager = consoleManager;
    }

    /**
     * PUBLIC METHODS 
     */

    /// <summary>
    /// Starts the application and takes the user to the main menu
    /// </summary>
    public void StartApplication()
    {
        try
        {
            // show the main menu
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

    /**
     * MENUS 
     */

    /// <summary>
    /// Shows the main menu for the numericket application
    /// </summary>
    private void MainMenu()
    {
        // show the page title
        ShowNewPage("Main Menu");

        _consoleManager.DisplayTable(ConsoleMessageType.INFORMATION, ["Menus", "Options"], new string[][]
        {
            ["New Game","Manage Players","Manage Team", "Stats", "Save and Exit"],
            ["1","2","3", "4", "5"]
        }, TableSpacing.SPACE_BETWEEN);

        _consoleManager.DisplayMessage(ConsoleMessageType.DEFAULT, "\n\nPlease select an option from the menu to continue");

        // get the response and help the user to carry out the desired operation
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

    /**
     *  MANAGE PLAYERS AND THE SUBMENUS
     */

    /// <summary>
    /// Shows the manage players menu for the numericket application
    /// </summary>
    private void ManagePlayers()
    {
        // show the page title
        ShowNewPage("Manage Players");

        // show the menus related to players
        _consoleManager.DisplayTable(ConsoleMessageType.INFORMATION, ["Menus", "Options"], new string[][]
        {
            ["Create New Player","Rename Player","Delete Player", "Show All Players","Main Menu"],
            ["1","2","3", "4", "5"]
        }, TableSpacing.SPACE_BETWEEN);

        _consoleManager.DisplayMessage(ConsoleMessageType.DEFAULT, "\n\nPlease select an option from the menu to continue");

        // get the response and help the user to carry out the desired operation
        int menuResponse = _consoleManager.GetAllowedNumericInput(new Dictionary<char, int>() { { '1', 1 }, { '2', 2 }, { '3', 3 }, { '4', 4 }, { '5', 5 } });

        switch (menuResponse)
        {
            case 1:
                {
                    CreatePlayer();
                    break;
                }
            case 2:
                {
                    EditPlayerName();
                    break;
                }
            case 3:
                {
                    DeletePlayer();
                    break;
                }
            case 4:
                {
                    ShowAllPlayers();
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

    /// <summary>
    /// Gets the input from the user, validates and creates a new player
    /// </summary>
    private void CreatePlayer()
    {
        // show the page title
        ShowNewPage("Create a new player");

        string playerName;
        int playerJerseyNumber;
        string playerPassword;

        // get a valid jersey number
        while (true)
        {
            string input = _consoleManager.GetLine(ConsoleMessageType.INFORMATION, "Enter the player jersey number.");
            if (string.IsNullOrEmpty(input))
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "Jersey number cannot be empty !!");
                continue;
            }

            int result;
            if (!int.TryParse(input, out result))
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "Please enter a valid jersey number !!");
                continue;
            }

            if (result < 1)
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "Jersey number cannot be 0 or less than 0 !!");
                continue;
            }

            if (!_playerRepository.IsUniqueJerseyNumber(result))
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "Jersey number already taken, please pick another jersey number !!");
                continue;
            }

            playerJerseyNumber = result;
            break;
        }

        // get the player name
        while (true)
        {
            playerName = _consoleManager.GetLine(ConsoleMessageType.INFORMATION, "Enter the player name.");
            if (string.IsNullOrEmpty(playerName))
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "Player name cannot be empty !!");
                continue;
            }
            break;
        }


        // get password, and verify by asking the user to retype and prepare the hashed password
        while (true)
        {
            string password = _consoleManager.GetPassword(ConsoleMessageType.INFORMATION, "Enter a password (minimum 6 characters).");
            if (string.IsNullOrEmpty(password))
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "Player password cannot be empty !!");
                continue;
            }

            if (password.Length < 6)
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "Player password must contain at least 6 characters !!");
                continue;
            }

            string passwordToConfirm = _consoleManager.GetPassword(ConsoleMessageType.INFORMATION, "Please re-type the password to confirm.");
            if (string.IsNullOrEmpty(passwordToConfirm))
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "Player password cannot be empty !!");
                continue;
            }


            if (password != passwordToConfirm)
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "Incorrect password please enter the same password to verify !!");
                continue;
            }

            playerPassword = PasswordHasher.HashPassword(password);
            break;
        }

        // add a new player and display the success message
        _playerRepository.AddPlayer(playerJerseyNumber, playerName, playerPassword);
        _consoleManager.DisplayMessage(ConsoleMessageType.SUCCESS, "Player added successfully !!");

        // go back to the manager players menu
        GoToAnotherPage("manage players", ManagePlayers);
    }

    /// <summary>
    /// Edits a name of the player by jersey number and password.
    /// </summary>
    private void EditPlayerName()
    {
        // show the page title
        ShowNewPage("Edit player name");

        int playerJerseyNumber;
        string newPlayerName;

        // get a valid jersey number
        while (true)
        {
            string input = _consoleManager.GetLine(ConsoleMessageType.INFORMATION, "Enter the player jersey number.");
            if (string.IsNullOrEmpty(input))
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "Jersey number cannot be empty !!");
                continue;
            }

            int result;
            if (!int.TryParse(input, out result))
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "Please enter a valid jersey number !!");
                continue;
            }

            if (_playerRepository.IsUniqueJerseyNumber(result))
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "No player found with that jersey number!!");
                continue;
            }

            playerJerseyNumber = result;
            break;
        }

        // validate the user using the password
        while (true)
        {
            string password = _consoleManager.GetPassword(ConsoleMessageType.INFORMATION, "Enter the password.");
            if (string.IsNullOrEmpty(password))
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "Password cannot be empty !!");
                continue;
            }

            if (!_playerRepository.canAuthorizePlayer(playerJerseyNumber, password))
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "Unauthorized - Password incorrect!!");
                continue;
            }
            break;
        }

        // get the new name to be edited
        while (true)
        {
            newPlayerName = _consoleManager.GetLine(ConsoleMessageType.INFORMATION, "Enter a new name.");
            if (string.IsNullOrEmpty(newPlayerName))
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "Player name cannot be empty !!");
                continue;
            }
            break;
        }

        // edit a player name and display the success message
        _playerRepository.EditPlayerName(playerJerseyNumber, newPlayerName);
        _consoleManager.DisplayMessage(ConsoleMessageType.SUCCESS, "Player name edited successfully !!");

        // go back to the manager players menu
        GoToAnotherPage("manage players", ManagePlayers);
    }

    /// <summary>
    /// Deletes a player by the jersey number and the password
    /// </summary>
    private void DeletePlayer()
    {
        // show the page title
        ShowNewPage("Delete a player");

        int playerJerseyNumber;

        // get the valid jersey number
        while (true)
        {
            string input = _consoleManager.GetLine(ConsoleMessageType.INFORMATION, "Enter the player jersey number.");
            if (string.IsNullOrEmpty(input))
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "Jersey number cannot be empty !!");
                continue;
            }

            int result;
            if (!int.TryParse(input, out result))
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "Please enter a valid jersey number !!");
                continue;
            }

            if (_playerRepository.IsUniqueJerseyNumber(result))
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "No player found with that jersey number!!");
                continue;
            }

            playerJerseyNumber = result;
            break;
        }

        // validate the user using the password
        while (true)
        {
            string password = _consoleManager.GetPassword(ConsoleMessageType.INFORMATION, "Enter the password.");
            if (string.IsNullOrEmpty(password))
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "Password cannot be empty !!");
                continue;
            }

            if (!_playerRepository.canAuthorizePlayer(playerJerseyNumber, password))
            {
                _consoleManager.DisplayMessage(ConsoleMessageType.ERROR, "Unauthorized - Password incorrect!!");
                continue;
            }
            break;
        }

        // get the confirmation from the user to remove the player
        while (true)
        {
            char confirmation = _consoleManager.GetInputCharacter(ConsoleMessageType.INFORMATION, "Are you sure want to remove the player, Y/N");

            if (confirmation == 'Y')
            {
                // deletes a player and display the success message
                _playerRepository.DeletePlayer(playerJerseyNumber);
                _consoleManager.DisplayMessage(ConsoleMessageType.SUCCESS, "Player removed successfully !!");
                break;
            }
            else if (confirmation == 'N')
            {
                break;
            }
        }

        // go back to the manager players menu
        GoToAnotherPage("manage players", ManagePlayers);
    }

    /// <summary>
    /// Displays a quick summary of all the players.
    /// </summary>
    private void ShowAllPlayers()
    {
        // show the page title
        ShowNewPage("Show all players");

        // display the list of player
        if (_playerRepository.GetPlayerCount() == 0)
            _consoleManager.DisplayMessage(ConsoleMessageType.INFORMATION, "No players found !!");
        else
            _consoleManager.DisplayTable(ConsoleMessageType.INFORMATION, ["JerseyNumber", "Player name"], _playerRepository.GetPlayerInformationToPrint(), TableSpacing.SPACE_BETWEEN);

        // go back to the manager players menu
        GoToAnotherPage("manage players", ManagePlayers);
    }

    /**
     * REUSABLE METHODS
     */

    /// <summary>
    /// Shows a new page with page title
    /// </summary>
    /// <param name="pageTitle">page title to be showed on top</param>
    private void ShowNewPage(string pageTitle)
    {
        // show the page title
        _consoleManager.Clear();
        _consoleManager.DisplayMessage(ConsoleMessageType.DEFAULT, "Numericket Console Game");

        _consoleManager.DisplayTable(ConsoleMessageType.HEADING, [], new string[][]
       {
       [pageTitle],
       }, TableSpacing.CENTER);

        _consoleManager.NewLine();
        _consoleManager.NewLine();
    }

    /// <summary>
    /// Wait for the user acknowledgement and take them to a different page
    /// </summary>
    /// <param name="pageTitle">The title of the page to go</param>
    /// <param name="callback">callback function that takes the user to a different page</param>
    private void GoToAnotherPage(string pageTitle, Action callback)
    {
        // go to a different menu
        _consoleManager.NewLine();
        _consoleManager.NewLine();
        _consoleManager.GetLine(ConsoleMessageType.DEFAULT, $"Press enter key to go back to {pageTitle.ToLower()} menu.");
        callback();
    }
}
