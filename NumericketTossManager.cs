using Numericket.Common.AbstractTossManager;
using Numericket.Common.ConsoleManager;

namespace Numericket
{
    public class NumcricketTossManager : AbstractOddOrEvenTossManager
    {
        private readonly IReadOnlyDictionary<char, int> _homeTeamInputs = new Dictionary<char, int>() { { 'Q', 1 }, { 'W', 2 }, { 'E', 3 }, { 'A', 4 }, { 'S', 5 }, { 'D', 6 } };
        private readonly IReadOnlyDictionary<char, int> _awayTeamInputs = new Dictionary<char, int>() { { 'I', 1 }, { 'O', 2 }, { 'P', 3 }, { 'J', 4 }, { 'K', 5 }, { 'L', 6 } };

        private readonly IConsoleManager _consoleManager;

        public NumcricketTossManager(IConsoleManager consoleManager):base(consoleManager) {
            _consoleManager = consoleManager;
        }

        protected override IReadOnlyDictionary<char, int> GetAllowedInputsForTossCaller()
        {
            return _awayTeamInputs
            .Where(kvp => kvp.Value == 1 || kvp.Value == 2)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        protected override IReadOnlyDictionary<char, int> GetAllTeamOneTossInputs()
        {
            return _homeTeamInputs;
        }

        protected override IReadOnlyDictionary<char, int> GetAllTeamTwoTossInputs()
        {
            return _awayTeamInputs;
        }

        protected override void DisplayPromptToCallForToss()
        {
            _consoleManager.DisplayMessage(ConsoleMessageType.DEFAULT, "Its time for the toss\n");
            _consoleManager.DisplayMessage(ConsoleMessageType.INFORMATION, "Its your call Odd or Even\n", "Away Team");
            _consoleManager.DisplayTable([], new string[][]
            {
               _awayTeamInputs.Where(input => input.Value == 1).Select(x => x.Key + " - Odd").ToArray(),
               _awayTeamInputs.Where(input => input.Value == 2).Select(x => x.Key + " - Even").ToArray(),
            }, TableSpacing.SPACE_BETWEEN);
        }

        protected override void DisplayCalledTossChoice(int selectedOption)
        {
            _consoleManager.DisplayMessage(ConsoleMessageType.SUCCESS, $"Away team has chosen - {(selectedOption ==1  ? "odd" : "even")}");
        }


        protected override void DisplayTossWinner(bool didCallerWin)
        {
            _consoleManager.DisplayMessage(ConsoleMessageType.SUCCESS, "Won the toss", $"{(didCallerWin ? "Away Team" : "Home Team")}");
        }

        protected override void DisplayPromptToGetTossInputsFromAllParties()
        {
            _consoleManager.DisplayMessage(ConsoleMessageType.DEFAULT, "Enter the number to proceed for the toss");
            _consoleManager.DisplayTable(["Home Team", "Away Team"], new string[][]
            {
               _homeTeamInputs.Select(x => x.Key + " - " + x.Value).ToArray(),
               _awayTeamInputs.Select(x => x.Key + " - " + x.Value).ToArray(),
            }, TableSpacing.SPACE_BETWEEN);
        }

        protected override void DisplayInputSpecifiedByBothParties(int teamOneInput, int teamTwoInput)
        {
            _consoleManager.DisplayTable(["Home Team", "Away Team"], new string[][]
           {
               [teamOneInput.ToString()],
               [teamTwoInput.ToString()],
           }, TableSpacing.SPACE_BETWEEN);
        }

        protected override int GetWaitTimeForEvaluatingToss()
        {
            return 2000;
        }

        public void IsHomeTeamBattingFirst()
        {
            var didCallerWin = DidCallerWin();
          
            _consoleManager.DisplayMessage(ConsoleMessageType.INFORMATION, "What do you want to do first ? Bat or Bowl", $"{(didCallerWin ? "Away Team" : "Home Team")}");

            var winningTeamInputs = didCallerWin ? _awayTeamInputs : _homeTeamInputs;

            _consoleManager.DisplayTable([], new string[][]
           {
               winningTeamInputs.Where(input => input.Value == 1).Select(x => x.Key + " - Bat").ToArray(),
               winningTeamInputs.Where(input => input.Value == 2).Select(x => x.Key + " - Bowl").ToArray(),
           }, TableSpacing.SPACE_BETWEEN);

            var isBatting = _consoleManager.GetAllowedNumericInput(winningTeamInputs
            .Where(kvp => kvp.Value == 1 || kvp.Value == 2)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value), "") == 1;

            _consoleManager.DisplayMessage(ConsoleMessageType.SUCCESS, "Wins the toss", $"{(didCallerWin ? "Away Team" : "Home Team")}");
            _consoleManager.DisplayMessage(ConsoleMessageType.SUCCESS, $"And chose to {(isBatting ? "bat" : "bowl")} first");

            Console.ReadKey();

        }
    }
}
