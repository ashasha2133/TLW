using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

public class UrbanDictionary
{
    private string[] definitions = new string[10];
    private int i = 0;
    private int max = 0;

    public string DefineTerm(string term)
    {
        i = 0;
        max = 0;
        Process process = null;

        ProcessStartInfo start = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = $"-u C:\\Users\\ashasha\\Desktop\\proxy.py ud \"{term}\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        try
        {
            using (process = Process.Start(start))
            {
                string response = process.StandardOutput.ReadToEnd();
                if (!process.HasExited)
                {
                    process.Kill();
                }

                if (response != "fail")
                {
                    // Extract definitions using Regex (simple, brittle but effective)
                    MatchCollection matches = Regex.Matches(
                        response,
                        "\"definition\":\\s*\"((?:\\\\.|[^\"\\\\])*)\"",
                        RegexOptions.Singleline
                    );

                    max = matches.Count - 1;
                    for (int j = 0; j < definitions.Length; j++)
                    {
                        if (j < matches.Count)
                            definitions[j] = UnescapeJson(matches[j].Groups[1].Value) + $"  {j}/{matches.Count - 1}";
                        else
                            definitions[j] = "";
                    }

                    return matches.Count > 0 ? definitions[0] : "No result found!";
                }
                return "fail";
            }
        }
        catch (Exception ex)
        {
            if (!process.HasExited)
            {
                process.Kill();
            }
            return "Process Exception: " + ex.Message + "\n\n\n Source: " + ex.Source + "\n\n\n InnecException: " + ex.InnerException;
        }
    }

    private string UnescapeJson(string input)
    {
        return input.Replace("\\n", "\n").Replace("\\\"", "\"").Replace("\\'", "'").Replace("\\\\", "\\").Replace("\\r", "\r");
    }

    public string Next()
    {
        if (i < max)
        {
            i++;
            return definitions[i];
        }
        return "";
    }

    public string Back()
    {
        if (i > 0)
        {
            i--;
            return definitions[i];
        }
        return "";
    }
}
