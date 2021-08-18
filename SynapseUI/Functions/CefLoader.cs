using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using CefSharp;
using CefSharp.Wpf;

namespace SynapseUI.Functions
{
    public class CefLoader
    {
        private static string path = Path.Combine(Directory.GetCurrentDirectory(), @"bin\");

        /// <summary>
        /// To maximise compatibility and reduce download times and files, this function loads the already downloaded CefSharp libraries that Synapse typically uses.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool Init()
        {
            var libraryLoader = new CefLibraryHandle(path + "libcef.dll");
            if (libraryLoader.IsInvalid)
                return true;

            var settings = new CefSettings
            {
                BrowserSubprocessPath = path + "CefSharp.BrowserSubprocess.exe",
                LocalesDirPath = path + @"locales\",
                CachePath = path + @"GPUCache\",
                LogFile = path + "debug.log",
                ResourcesDirPath = path,
                IgnoreCertificateErrors = true
            };

            Cef.Initialize(settings, performDependencyCheck: false, cefApp: null);

            libraryLoader.Dispose();

            return false;
        }

        public static void InitBrowser(Grid grid)
        {
            if (!File.Exists(@".\bin\custom\Editor.html"))
                return;

            var browser = new ChromiumWebBrowser(path + @"\custom\Editor.html");
            grid.Children.Add(browser);
        }
    }
}
