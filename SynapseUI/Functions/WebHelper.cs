using System;
using System.Net;
using System.Collections.Generic;
using System.IO;

namespace SynapseUI.Functions.Web
{
    public class WebHelper
    {
        public WebClient Client;

        public WebHelper()
        {
            Client = new WebClient();
        }

        public void DownloadFile(string url, string location)
        {
            using (WebClient client = new WebClient())
            {
                Uri uri = new Uri(url);

                client.DownloadFile(uri, location);
            }
        }

        public static void DownloadFile(string url, string location, WebClient client)
        {
            using (client)
            {
                client.DownloadFile(url, location);
            }
        }
    }

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
            var web = new WebHelper();
            using (web.Client)
            {
                foreach (var entry in FileEntries)
                {
                    string path = Path.Combine(BaseDir, entry.Location, entry.Filename);
                    if (!File.Exists(path))
                    {
                        string url = BaseUrl + entry.Url + "/" + entry.Filename;
                        web.DownloadFile(url, path);
                    }
                }
            }
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

}
