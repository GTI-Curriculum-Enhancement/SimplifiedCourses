using YamlExtractor.Net.Logger;
using YamlExtractor.Net.Misc;
using Spectre.Console;

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

            (int totalFiles, int extracted) = ExtractYamlFiles(targetDirectory, tokens);

            if (totalFiles == 0)
                AnsiConsole.MarkupLineInterpolated($"\r\n----------------\r\n[yellow]0[/] files to extract.");
            else
                AnsiConsole.MarkupLineInterpolated($"\r\n----------------\r\n[yellow]{totalFiles}/{extracted}[/] file{Qol.Pluralize(totalFiles, "s")} extracted successfully.");
        }
        catch (Exception e)
        {
            Logging.LogError($"Error: {e.Message}");
            return 5;
        }

        return 0;
    }

    static (int, int) ExtractYamlFiles(string directory, string tokens)
    {
        var yamlFilesPerDirectory = new Dictionary<string, int>();
        int totalFiles = 0;
        int extracted = 0;

        foreach (var filePath in Directory.GetFiles(directory, "*.yaml", SearchOption.AllDirectories))
        {
            var directoryKey = Path.GetDirectoryName(filePath)!;
            var fileName = Path.GetFileName(filePath);

            if (fileName == "FormatTokens.yaml")
            {
                Logging.Log("Skipping format tokens yaml file extract...");
                continue;
            }

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

            var readmePath = Path.Combine(directoryKey, "README.md");
            var yamlContent = File.ReadAllText(filePath);

            Logging.Log($"Extracting: [cyan]{fileName}[/]...");
            using var mdWriter = new StreamWriter(readmePath);
            mdWriter.WriteLine($"# YAML Data from '{filePath}'");
            mdWriter.WriteLine($"# At: {DateTime.Now}\r\n");
            mdWriter.WriteLine("```yaml");
            mdWriter.WriteLine(tokens);
            mdWriter.WriteLine();
            mdWriter.WriteLine(yamlContent);
            mdWriter.WriteLine("```");

            extracted++;
        }

        return (totalFiles, extracted);
    }

    static string? GetFormatTokens(string directory, string specificFile)
    {
        string specificFilePath = Directory.GetFiles(directory, specificFile, SearchOption.AllDirectories).FirstOrDefault();

        if (specificFilePath is not null)
            return File.ReadAllText(specificFilePath);

        return null;
    }
}