namespace YamlExtractor.Net.Misc;

internal static class Qol
{
    /// <summary>
    /// Return the pluralization for a word if <paramref name="count"/> <see langword="is not"/> 1
    /// </summary>
    /// <param name="count"></param>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Pluralize(int count, string str)
        => count == 1
            ? ""
            : str;

    /// <inheritdoc/>
    public static char Pluralize(int count, char chr)
    => count == 1
        ? '\0'
        : chr;

    /// <inheritdoc/>
    public static string Pluralize(this string l_str, int count, string str)
        => $"{l_str}{Pluralize(count, str)}";
}
