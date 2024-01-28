using Spectre.Console;
using System.Runtime.InteropServices;
using System.Text;

namespace YamlExtractor.Net.Logger;

internal static class Logging
{
    public static void WriteLine(string line = "")
        => AnsiConsole.MarkupLine(line);

    public static void Write(string text = "")
        => AnsiConsole.Markup(text);

    public static void Log(string log)
        => WriteLine($"[blue][[!]][/]\t{log}");

    public static void LogWarning(string log)
        => WriteLine($"[yellow][[!]][/]\t{log}");

    public static void LogError(string log)
        => WriteLine($"[red][[!]][/]\t{log}");

    public static void WriteTitle(string title)
        => WriteTitle(new Rule(title).LeftJustified());

    public static void WriteTitle(Rule rule)
    {
        AnsiConsole.Write(rule);
        Console.WriteLine();
    }

    public static void WriteData(string header, params string[] data)
    {
        StringBuilder text = new($"{header}:\r\n");

        foreach (string d in data)
            text.Append($"\t{d}\r\n");

        WriteLine(text.ToString());
    }

    public static void LogException(string message, Exception e)
    {
        LogError(message);
        AnsiConsole.WriteException(e);
    }

    /* Debugging methods */

    /*
     * I could just make nest all of these methods within a preprocessor statement, but I don't trust that I'd keep all
     * references within preprocessor statements as well.
     */

    public static void DebugLogVar(string tag, string value)
    {
#if DEBUG
        DebugLog($"{tag}: {value}");
#endif
    }

    public static void DebugLog(string log)
    {
#if DEBUG
        AnsiConsole.MarkupLineInterpolated($"[darkmagenta]<debug \t\b'[/]{log}[darkmagenta]' />[/]");
#endif
    }

    public static void DebugLogNested(string header, [Optional] string log)
    {
#if DEBUG
        if (string.IsNullOrEmpty(log))
            (log, header) = (header, "");
        else
            header = $" title='[magenta1]{header}[/]'";

        log = $"\r\n{log}";

        AnsiConsole.Markup($"[darkmagenta]<debug{header}>[/]");
        AnsiConsole.MarkupLine(string.Join("\n\t| ", log.EscapeMarkup().Split('\n')));
        AnsiConsole.MarkupLine("[darkmagenta]<debug/>[/]");
#endif
    }
}
