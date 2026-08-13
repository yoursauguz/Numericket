namespace NumericketConsoleApp.Toss
{
    /// <summary>
    /// Defines a contract for executing a toss sequence and checks whether the home team is batting first.
    /// </summary>
    public interface INumericketTossManager
    {
        /// <summary>
        /// Tells whether the home team is going to bat first.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the home team has found a way to bat first; otherwise, <c>false</c>.
        /// </returns>
        public bool IsHomeTeamBattingFirst();

        /// <summary>
        /// Sets the home and away team names
        /// </summary>
        /// <param name="homeTeamName">home team name to be set</param>
        /// <param name="awayTeamName">away team name to be set</param>
        public void SetTeamNames(string homeTeamName, string awayTeamName);
    }
}
