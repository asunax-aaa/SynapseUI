using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System;
using System.Diagnostics;

namespace SynapseUI.Functions.Web
{
    public class FileDownloader
    {
        public string BaseUrl { get; set; }
        public string BaseDir { get; set; }

        public List<FileEntry> FileEntries = new List<FileEntry>();

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
            var entries = new List<(string url, string path)>();
            foreach (var entry in FileEntries)
            {       
                string url = BaseUrl + entry.Url + "/" + entry.Filename;
                string path = Path.Combine(BaseDir, entry.Location, entry.Filename);
                entries.Add((url, path));
            }

            return entries;
        }

        public override string ToString()
        {
            string s = "";
            foreach ((string url, string path) in BuildEntries())
                if (!path.Contains("Updater.exe"))
                    s += url + "|" + path + "|";
            return s.Substring(0, s.Length - 1);
        }
    }

    public class FileEntry
    {
        public string Filename { get; set; }
        public string Location { get; set; }
        public string Url { get; set; }

        public FileEntry(string filename, string location = "", string url = "")
        {
            Filename = filename;
            Location = location;
            Url = url;
        }
    }

    public static class VersionChecker
    {
        public static (string version, string url) GetLatestVersion()
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36");
                string contents = client.DownloadString("https://api.github.com/repos/asunax-aaa/SynapseUI/releases/latest");

                string version = Regex.Match(contents, "(?<=\"tag_name\": \"v).+(?=\")").Value;
                if (version.Contains(GetCurrentVersion()))
                    return ("", "");

                return (version, Regex.Match(contents, "(?<=\"browser_download_url\": \").+(?=\")").Value);
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
            var data = GetLatestVersion();

            if (string.IsNullOrWhiteSpace(data.version))
                return;

            string entries = fileDownloader.ToString();
            string arguments = $"\"{App.CURRENT_DIR}\" \"{appName}\" \"{data.url}\" \"{entries}\"";

            var proc = new Process();
            proc.StartInfo.FileName = "Updater.exe";
            proc.StartInfo.Arguments = arguments;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.WorkingDirectory = Path.Combine(App.CURRENT_DIR, @"bin\custom\");
            proc.Start();

            Environment.Exit(0);
            
        }
    }

}
