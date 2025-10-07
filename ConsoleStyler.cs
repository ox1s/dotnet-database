public static class ConsoleStyler
{
    private const string GreenAnsi = "\u001b[32m";
    private const string RedAnsi = "\u001b[31m";
    private const string EndColorAnsi = "\u001b[0m";

    public static string Green(string text)
    {
        return $"{GreenAnsi}{text}{EndColorAnsi}";
    }

    public static string Red(string text)
    {
        return $"{RedAnsi}{text}{EndColorAnsi}";
    }
}