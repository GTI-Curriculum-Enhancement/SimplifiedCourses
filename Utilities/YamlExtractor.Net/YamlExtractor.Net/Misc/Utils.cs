using System.Text.RegularExpressions;

namespace YamlExtractor.Net.Misc;

internal static partial class Utils
{
    public static string ExtractLinkText(string markdown)
    {
        Match match = HyperlinkText().Match(markdown);

        return match.Success
            ? match.Groups[1].Value
            : string.Empty;
    }

    public static string ExtractLeadingSpaces(string input)
    {
        Match match = Whitespaces().Match(input);

        return match.Success
            ? match.Groups[1].Value
            : string.Empty;
    }

    [GeneratedRegex("\\[([^\\]]+)\\]\\(([^)]+)\\)")]
    private static partial Regex HyperlinkText();
    [GeneratedRegex("^(\\s*)\\S")]
    private static partial Regex Whitespaces();
}