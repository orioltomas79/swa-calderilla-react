namespace Calderilla.Test.Utils;

public static class StringExtensions
{
    private static readonly string[] LineSeparators = ["\r\n", "\n", "\r"];

    public static string[] SplitLines(this string input)
    {
        return input.Split(LineSeparators, StringSplitOptions.None);
    }
}

