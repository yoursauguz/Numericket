using ConsoleLibrary.ConsoleManager;
using Moq;
using TossLibrary.TossManager;

namespace TossLibraryTests;

[TestFixture]
public class TossManagerTests
{
    private Mock<IConsoleManager> _mockConsoleManager;

    [SetUp]
    public void SetUp()
    {
        _mockConsoleManager = new Mock<IConsoleManager>();
    }

    private class TestableOddOrEvenTossManager : AbstractOddOrEvenTossManager
    {
        public int[] BothPartiesInputs { get; private set; }


        public bool PromptToGetTossInputsFromAllParties { get; private set; }


        public TestableOddOrEvenTossManager(IConsoleManager consoleManager) : base(consoleManager) { }

        protected override void DisplayCalledTossChoice(int selectedOption)
        {
        }

        protected override void DisplayInputSpecifiedByBothParties(int teamOneInput, int teamTwoInput)
        {
            BothPartiesInputs = [teamOneInput, teamTwoInput];
        }

        protected override void DisplayPromptToCallForToss()
        {
        }

        protected override void DisplayPromptToGetTossInputsFromAllParties()
        {
            PromptToGetTossInputsFromAllParties = true;
        }

        protected override void DisplayTossWinner(bool didCallerWin)
        {
        }

        protected override IReadOnlyDictionary<char, int> GetAllowedInputsForTossCaller() => new Dictionary<char, int>() { { 'I', 1 }, { 'O', 2 } };

        protected override IReadOnlyDictionary<char, int> GetAllTeamOneTossInputs() => new Dictionary<char, int>() { { 'Q', 1 }, { 'W', 2 }, { 'E', 3 }, { 'A', 4 }, { 'S', 5 }, { 'D', 6 } };

        protected override IReadOnlyDictionary<char, int> GetAllTeamTwoTossInputs() => new Dictionary<char, int>() { { 'I', 1 }, { 'O', 2 }, { 'P', 3 }, { 'J', 4 }, { 'K', 5 }, { 'L', 6 } };

        protected override int GetWaitTimeForEvaluatingToss() => 2000;

        public bool TestEvaluateTossOutcome(int selectedOption) => EvaluateTossOutcome(selectedOption);

        protected override string GetTeamOneName()
        {
            return "";
        }

        protected override string GetTeamTwoName()
        {
            return "";
        }
    }


    [TestCase(1, 1, 2, true, TestName = "Caller picked 1 (Odd). Inputs are 1+2 = 3 (Odd). Outcome: Caller Won")]
    [TestCase(1, 2, 2, false, TestName = "Caller picked 1 (Odd). Inputs are 2+2 = 4 (Even). Outcome: Caller Lost")]
    [TestCase(2, 1, 2, false, TestName = "Caller picked 2 (Even). Inputs are 1+2 = 3 (Odd). Outcome: Caller Lost")]
    [TestCase(2, 2, 2, true, TestName = "Caller picked 2 (Even). Inputs are 2+2 = 4 (Even). Outcome: Caller Won")]
    public void EvaluateOddOrEvenTossOutcome_ExecuteMethodsInOrderAndCalculateWinner_BasedOnTestCases(int selectedOption, int teamOneInput, int teamTwoInput, bool isCallerWon)
    {

        _mockConsoleManager.Setup(c => c.GetAllowedNumericInputs(It.IsAny<IReadOnlyDictionary<char, int>[]>()))
            .Returns(new[] { teamOneInput, teamTwoInput });

        var manager = new TestableOddOrEvenTossManager(_mockConsoleManager.Object);

        bool result = manager.TestEvaluateTossOutcome(selectedOption);

        Assert.Multiple(() =>
        {
            Assert.That(manager.PromptToGetTossInputsFromAllParties, Is.True);
            Assert.That(manager.BothPartiesInputs, Is.EqualTo([teamOneInput, teamTwoInput]));
            Assert.That(result, Is.EqualTo(isCallerWon));
        });

    }
}
