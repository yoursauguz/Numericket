using NumericketConsoleApp.Managers;
using NumericketConsoleApp.UI;

namespace Numericket;

/// <summary>
/// Contains the landing point of the Numericket application and all the initial configurations required for the app.
/// </summary>
public class Program
{
    /// <summary>
    /// Loads the numericket application with the initial configurations.
    /// </summary>
    /// <param name="args">command line arguments</param>
    public static void Main(string[] args)
    {
        try
        {
            // Starts the numericket application
            NumericketApplication main = new(new NumericketConsoleManager());
            main.StartApplication();
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

}
