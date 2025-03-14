using Bluscream;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace OverwolfPatcher.Classes
{
    public class Overwolf
    {
        private const string ProcessName = "overwolf";
        internal const string BaseRegKey = @"SOFTWARE\WOW6432Node\Overwolf";
        internal const string MainRegKey = @"Software\Overwolf\Overwolf";
        internal static Uri UrlProtocol => new Uri("overwolfstore://");
        internal static Uri DownloadUrl => new Uri("https://download.overwolf.com/install/Download?utm_source=web_app_store");

        public DirectoryInfo ProgramFolder { get; set; }
        public DirectoryInfo DataFolder { get; set; }

        public DirectoryInfo ExtensionsFolder => DataFolder.Combine("Extensions");
        public List<DirectoryInfo> ProgramVersionFolders => ProgramFolder.GetDirectories(searchPattern: "*.*.*.*").ToList();
        // public DirectoryInfo WindowsDesktopApp => new DirectoryInfo(@"C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App\8.0.11");
        public List<Process> Processes => Process.GetProcessesByName(ProcessName).ToList();
    }
}
