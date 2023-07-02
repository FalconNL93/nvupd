namespace Nvupd.Cli;

public static class ConsoleX
{
    /// <summary>
    /// Asks the user a yes or no question
    /// </summary>
    /// <param name="text">The question to print</param>
    /// <param name="defaultValue">The default choice of the question</param>
    /// <param name="invalidChoice">Text to print when the user presses an invalid key</param>
    /// <returns>A boolean</returns>
    public static bool YesNo(
        string text,
        ConsoleKey defaultValue = ConsoleKey.N,
        string invalidChoice = ""
    )
    {
        var acceptedKeys = new List<ConsoleKey>()
        {
            ConsoleKey.Y,
            ConsoleKey.N,
            ConsoleKey.Enter
        };

        var choices = defaultValue == ConsoleKey.Y ? "(Y/n)" : "(y/N)";
        Console.WriteLine($"{text} {choices}");

        var userInput = Console.ReadKey(true).Key;
        while (!acceptedKeys.Exists(x => x == userInput))
        {
            Console.WriteLine(string.IsNullOrEmpty(invalidChoice)
                ? $"{text} {choices}"
                : invalidChoice);
            
            userInput = Console.ReadKey(true).Key;
        }

        return userInput switch
        {
            ConsoleKey.Escape => false,
            ConsoleKey.Y => true,
            _ => defaultValue == ConsoleKey.Y
        };
    }
}