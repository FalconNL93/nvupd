namespace Nvupd.Core.Helpers;

public static class StringExtensions
{
    public static string PrependAmount(this string content, string prependContent, int amount = 1)
    {
        if (amount == 0)
        {
            return content;
        }

        var counter = 0;
        var output = string.Empty;
        do
        {
            output = $"{output}{prependContent}";
            counter++;
        } while (counter < amount);

        return $"{output}{content}";
    }

    public static string PrependNewLines(this string content, int amount = 1)
    {
        return PrependAmount(content, Environment.NewLine, amount);
    }
}