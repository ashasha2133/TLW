using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

public class AI
{
    public static string AskAI(string term)
    {
        Process process = null;

        ProcessStartInfo start = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = $"-u C:\\Users\\ashasha\\Desktop\\proxy.py ai \"{term}\"",
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

                return response;
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
}
