using Spectre.Console;
using System.Text;
using YamlExtractor.Net.Logger;
using YamlExtractor.Net.Misc;

namespace YamlExtractor.Net;

internal static class Program
{
    static int Main(string[] args)
    {
        // Default to current executing directory if not specified
        string targetDirectory = args.Length > 0
            ? args[0]
            : ".";

        try
        {
            Logging.Log("Looking for YAML tokens file...");
            var tokens = GetFormatTokens(targetDirectory, "FormatTokens.yaml");

            if (tokens is null)
            {
                Logging.LogError("Failed to locate YAML tokens file 'FormatTokens.yaml'");
                return 5;
            }

            var yamlRet = ExtractYamlFiles(targetDirectory, tokens);

            if (yamlRet.TotalFiles == 0)
                AnsiConsole.MarkupLineInterpolated($"\r\n----------------\r\n[yellow]0[/] files to extract.");
            else
                AnsiConsole.MarkupLineInterpolated(
                    $"\r\n----------------\r\n[yellow]{yamlRet.TotalFiles}/{yamlRet.ExtractedFiles}[/] file{Qol.Pluralize(yamlRet.TotalFiles, 's')} extracted successfully."
                );
        }
        catch (Exception e)
        {
#if DEBUG
            AnsiConsole.WriteException(e);
#else
            Logging.LogError($"Error: {e.Message}");
#endif
            return 5;
        }

        return 0;
    }

    static YamlReturn ExtractYamlFiles(string dir, string tokens)
    {
        var yamlFilesPerDirectory = new Dictionary<string, int>();

        // Keep track of paths to log all effected after
        List<string> paths = new();

        int totalFiles = 0;
        int extracted = 0;

        // Extract text from YAML files and place it in a README.md file in within the same directory
        foreach (var path in Directory.EnumerateFiles(dir, "*",
            new EnumerationOptions { IgnoreInaccessible = true, RecurseSubdirectories = true }))
        {
            // Check if it's a YAML file
            if (!path.ToLower().EndsWith(".yaml"))
                continue;

            var directoryKey = Path.GetDirectoryName(path)!;
            var fileName = Path.GetFileName(path);

            // Skip format symbols
            if (fileName == "FormatTokens.yaml")
            {
                Logging.Log("Skipping format tokens YAML file...");
                continue;
            }

            // Skip files after one has already been processed (Can only have one README per directory
            totalFiles++;
            if (yamlFilesPerDirectory.TryGetValue(directoryKey, out int value))
            {
                yamlFilesPerDirectory[directoryKey]++;

                if (value > 1)
                    Logging.LogError($"Multiple YAML files found in directory '{directoryKey}' : Skipping file '{fileName}'");

                continue;
            }
            else
                yamlFilesPerDirectory[directoryKey] = 1;

            var nestedReadme = Path.Combine(directoryKey, "README.md");
            var yamlContent = File.ReadAllText(path);

#if DEBUG
            Logging.DebugLogVar(nameof(nestedReadme), nestedReadme);
#endif

            Logging.Log($"Extracting: [cyan]{fileName}[/]...");
            using var mdWriter = new StreamWriter(nestedReadme);
            mdWriter.WriteLine($"<!-- YAML Data from '{path}' -->");
            mdWriter.WriteLine($"<!-- At: {DateTime.Now} -->\r\n");
            mdWriter.WriteLine("```yaml");
            mdWriter.WriteLine(tokens);
            mdWriter.WriteLine();
            mdWriter.WriteLine(yamlContent);
            mdWriter.WriteLine("```");

            extracted++;
            paths.Add(path);
        }

        Logging.Log("Gathering YAML information for root level README...");

        // Make sure all path items are in order for the dictionary
        paths.Sort();

#if DEBUG
        Console.WriteLine();
        Logging.DebugLog("Effected paths:");
        foreach (var path in paths)
            Console.WriteLine(path);
        Console.WriteLine();
#endif

        var readmePath = Path.Combine(dir, "README.md");
        var readmeText = YamlToMarkdown(paths);

#if DEBUG
        Logging.DebugLog("README.md dictionary:");
        Logging.DebugLogNested(string.Join("\r\n", readmeText));
        Logging.DebugLogVar("At", readmePath);
#endif

        using var rootRM = new StreamWriter(readmePath);
        rootRM.WriteLine("# Simplified Courses\r\n");
        foreach (var item in readmeText)
            rootRM.WriteLine(item);

        return new(totalFiles, extracted);
    }

    static string? GetFormatTokens(string directory, string specificFile)
    {
        string specificFilePath = Directory.GetFiles(directory, specificFile, SearchOption.AllDirectories).FirstOrDefault();

        if (specificFilePath is not null)
            return File.ReadAllText(specificFilePath);

        return null;
    }

    static List<string> YamlToMarkdown(List<string> paths)
    {
        List<string> markdownList = new();

        foreach (var path in paths)
        {
            string[] segments = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            StringBuilder sb = new();
            for (int i = 0; i < segments.Length - 1; i++)
            {
                if (segments[i] == ".")
                    continue;

                markdownList.Add($"{sb}- {segments[i]}");
                sb.Append("  ");
            }

            string fileName = segments[^1];
            string urlEncodedFileName = path.Replace(fileName, "README.md").Replace(" ", "%20");
            markdownList.Add($"{sb}- [{Path.GetFileNameWithoutExtension(fileName)}]({urlEncodedFileName})");
        }

        return markdownList.Distinct().ToList();
    }

    private struct YamlReturn
    {
        public YamlReturn(int total, int extracted)
        {
            TotalFiles = total;
            ExtractedFiles = extracted;
        }

        public int TotalFiles { get; set; }
        public int ExtractedFiles { get; set; }
    }
}