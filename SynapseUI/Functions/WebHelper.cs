using System;
using System.Net;

namespace SynapseUI.Functions.Web
{
    public static class WebHelper
    {
        public static void DownloadFile(string url, string location)
        {
            using (WebClient client = new WebClient())
            {
                Uri uri = new Uri(url);
                client.DownloadFileAsync(uri, location);
            }
        }
    }
}
