namespace Nvupd.Cli;

public static class ConsoleX
{
    public static bool YesNo(string text, ConsoleKey defaultValue = ConsoleKey.N)
    {
        var acceptedKeys = new List<ConsoleKey>()
        {
            ConsoleKey.Y,
            ConsoleKey.N,
            ConsoleKey.Enter,
            ConsoleKey.Escape
        };

        var choices = defaultValue == ConsoleKey.Y ? "Y/n" : "y/N";
        Console.WriteLine($"{text} {choices}");
        var userInput = Console.ReadKey(true).Key;
        while (!acceptedKeys.Exists(x => x == userInput))
        {
            Console.WriteLine($"{text} {choices}");
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