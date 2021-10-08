﻿using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace SynapseUI.Functions.Web
{
    public class FileDownloader
    {
        public string BaseUrl { get; set; }
        public string BasePath { get; set; }

        public List<FileEntry> FileEntries = new List<FileEntry>();

        private List<(string Url, string Path)> _entries;

        public void Add(FileEntry entry)
        {
            FileEntries.Add(entry);
        }

        public void Begin()
        {
            using (WebClient client = new WebClient())
            {
                foreach ((string url, string path) in BuildEntries())
                {
                    if (!File.Exists(path))
                        client.DownloadFile(url, path);
                }
            }
        }

        public List<(string url, string path)> BuildEntries()
        {
            if (_entries != null)
                return _entries;

            var entries = new List<(string url, string path)>();
            foreach (var entry in FileEntries)
            {       
                string url = BaseUrl + entry.Url + "/" + entry.Filename;
                string path = entry.RelativePath ? Path.Combine(BasePath, entry.Path, entry.Filename) :
                    Path.Combine(entry.Path, entry.Filename);

                entries.Add((url, path));
            }

            _entries = entries;
            return _entries;
        }

        public string Build()
        {
            var s = new StringBuilder();

            foreach ((string url, string path) in BuildEntries())
            {
                if (!path.Contains("Updater.exe"))
                    s.Append($"{url}|{path}|");
            }

            s.Length--;
            return s.ToString();
        }
    }

    public class FileEntry
    {
        public string Filename { get; }
        public string Path { get; }
        public bool RelativePath { get; }
        public string Url { get; }

        public FileEntry(string filename, string path = "", string url = "", bool relativePath = true)
        {
            Filename = filename;
            Path = path;
            Url = url;
            RelativePath = relativePath;
        }
    }

    public static class VersionChecker
    {
        public static (string Version, string Url) GetLatestVersion()
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36");
                var contents = client.DownloadString("https://asunax.000webhostapp.com/latest-version.php").Split('|');

                if (contents.Length != 2)
                    return ("", "");

                return (contents[0], contents[1]);
            }
        }

        public static string GetCurrentVersion()
        {
            var ver = typeof(App).Assembly.GetName().Version;
            return $"{ver.Major}.{ver.Minor}.{ver.Build}";
        }

        public static void Run(FileDownloader fileDownloader)
        {
            string appName = AppDomain.CurrentDomain.FriendlyName;

            var latest = GetLatestVersion();
            string current = GetCurrentVersion();

            if (string.IsNullOrWhiteSpace(latest.Version) || latest.Version == current)
                return;

            string entries = fileDownloader.Build();
            string arguments = $"\"{App.CURRENT_DIR}\" \"{appName}\" \"{latest.Url}\" \"{entries}\"";

            var proc = new Process();
            proc.StartInfo.FileName = "Updater.exe";
            proc.StartInfo.Arguments = arguments;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.WorkingDirectory = Path.Combine(App.CURRENT_DIR, @"bin\custom\");
            proc.Start();

            Environment.Exit(0);
        }
    }
}
