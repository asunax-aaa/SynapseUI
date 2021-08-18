using System;
using System.IO;
using System.Windows;
using SynapseUI.Functions;
using SynapseUI.Exceptions;
using SynapseUI.Functions.Web;

namespace SynapseUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly bool OVERRIDE_DEBUG = false;
        public static readonly bool SKIP_CEF = false;

#if DEBUG
        public static bool DEBUG
        {
            get => true && !OVERRIDE_DEBUG;
        }
#else
        public static bool DEBUG
        {
            get => false || OVERRIDE_DEBUG;
        }
#endif

        [STAThread]
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            if (ValidateSynapseInstall())
                return;

            ValidateCustomInstall();

            if (!SKIP_CEF)
            {
                if (CefLoader.Init())
                {
                    ThrowError(BaseException.CEF_NOT_FOUND);
                    return;
                }
            }

            SplashScreen splash = new SplashScreen();
            splash.Show();
        }

        private bool ValidateSynapseInstall()
        {
            if (!File.Exists(@".\S^X.exe"))
            {
                ThrowError(BaseException.INVALID_SYNAPSE_INSTALL);
                return true;
            }

            string[] folders = new string[]
            {
                "auth",
                "autoexec",
                "bin",
                "scripts",
                "workspace",
                @".\bin\sxlib"
            };

            foreach (string folder in folders)
            {
                if (!Directory.Exists(folder))
                {
                    ThrowError(BaseException.INVALID_SYNAPSE_INSTALL);
                    return true;
                }
            }

            return false;
        }

        private void ValidateCustomInstall()
        {
            string[] folders = new string[]
            {
                @".\bin",
                @".\bin\custom\",
                @".\bin\custom\ace\"
            };

            foreach (string folder in folders)
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

            var downloader = new FileDownloader
            {
                BaseDir = @".\bin\custom\",
                BaseUrl = @"https://raw.githubusercontent.com/asunax-aaa/SynapseUI/master/SynapseUI/Resources/"
            };

            downloader.Add(new FileEntry("Editor.html", "", "Monaco"));
            downloader.Add(new FileEntry("mode-lua.js", "ace", "Monaco/ace"));
            downloader.Add(new FileEntry("HelpInfo.xml"));

            // NO LONGER NEEDED.
            /*
            downloader.Add(new FileEntry("ace.js", "ace", "Monaco/ace"));
            downloader.Add(new FileEntry("theme-tomorrow_night_eighties.js", "ace", "Monaco/ace"));
            */

            downloader.Begin();
        }

        private void ThrowError(BaseException error)
        {
            var err = new ErrorWindow(new Types.BaseError(error));
            err.Show();
        }
    }
}