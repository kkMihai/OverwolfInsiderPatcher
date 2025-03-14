using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using OverwolfPatcher.Classes;

namespace Bluscream;

internal static class Utils
{
    const int defaultMinWidth = 80;
    const int defaultPadding = 10;

    internal static List<int> GetPadding(string input, int minWidth = defaultMinWidth, int padding = defaultPadding)
    {
        int totalWidth = minWidth + padding * 2;
        int leftPadding = (totalWidth - input.Length) / 2;
        int rightPadding = totalWidth - input.Length - leftPadding;
        return new List<int> { leftPadding, rightPadding, totalWidth };
    }
    internal static string Pad(string input, string outer = "||", int minWidth = defaultMinWidth, int padding = defaultPadding)
    {
        var padded = GetPadding(input, minWidth, padding);
        return $"{outer}{new string(' ', Math.Max(padded[index: 0], 0))}{input}{new string(' ', Math.Max(padded[1], 0))}{outer}";
    }

    internal static string Fill(char c, int width = defaultMinWidth, int padding = defaultPadding)
    {
        return new string(c, width + padding * 2 + 4);
    }

    internal static string Log(string text, int length = 73)
    {
        text = "|| " + text;
        for (int i = 0; text.Length < length; i++)
        {
            text += " ";
        }
        text = text + " ||";
        Console.WriteLine(text);
        return text;
    }
    static List<string> removeFromToRow(string from, string where, string to, string insert = "")
    {
        List<string> list;
        if (where.Contains("\r\n"))
            list = where.Split(new[] { "\r\n" }, StringSplitOptions.None).ToList();
        else
            list = where.Split(new[] { "\n" }, StringSplitOptions.None).ToList();
        return removeFromToRow(from, list, to, insert);
    }

    static List<string> removeFromToRow(string from, List<string> where, string to, string insert = "")
    {
        int start = -1;
        int end = -1;
        for (int i = 0; i < where.Count; i++)
        {
            if (where[i] == from)
            {
                start = i;
            }
            if (start != -1 && where[i] == to)
            {
                end = i;
                break;
            }
        }

        if (start != -1 && end != -1)
        {
            where.RemoveRange(start, end - start + 1);
        }
        if (insert != "")
        {
            where.Insert(start, insert);
        }

        return where;
    }
    internal static void Exit(int exitCode = 0)
    {
        Environment.Exit(exitCode);
        var currentP = Process.GetCurrentProcess();
        currentP.Kill();
    }
    public static void RestartAsAdmin(string[] arguments)
    {
        if (IsAdmin()) return;
        ProcessStartInfo proc = new ProcessStartInfo();
        proc.UseShellExecute = true;
        proc.WorkingDirectory = Environment.CurrentDirectory;
        proc.FileName = Assembly.GetEntryAssembly().CodeBase;
        proc.Arguments += arguments.ToString();
        proc.Verb = "runas";
        try
        {
            Process.Start(proc);
            Exit();
        } catch (Exception ex)
        {
            Console.WriteLine($"Unable to restart as admin automatically: {ex.Message}");
            Console.WriteLine("This app has to run with elevated permissions (Administrator) to be able to modify files in the Overwolf folder!");
            Console.ReadKey();
            Exit();
        }
    }
    internal static bool IsAdmin()
    {
        bool isAdmin;
        try
        {
            WindowsIdentity user = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(user);
            isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
        } catch (UnauthorizedAccessException)
        {
            isAdmin = false;
        } catch (Exception)
        {
            isAdmin = false;
        }
        return isAdmin;
    }
    internal static void ErrorAndExit(string message, bool reinstall_overwolf = false)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
        if (reinstall_overwolf) Overwolf.DownloadUrl.OpenInDefaultBrowser();
        Exit(1);
    }
}