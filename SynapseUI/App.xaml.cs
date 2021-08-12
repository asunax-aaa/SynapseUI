using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using SynapseUI.Exceptions;
using SynapseUI.Functions.Web;
/*
using CefSharp;
using CefSharp.Wpf;
*/

namespace SynapseUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly bool OVERRIDE_DEBUG = false;

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

        string path = Path.Combine(Directory.GetCurrentDirectory(), @"bin\");

        [STAThread]
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            if (ValidateSynapseInstall())
                return;

            ValidateCustomInstall();

            /*
            var libraryLoader = new CefLibraryHandle(path + "libcef.dll");
            var isValid = !libraryLoader.IsInvalid;

            LoadCef();

            libraryLoader.Dispose();
            */

            LoadCef();
        }

        /// <summary>
        /// To maximise compatibility and reduce download times and files, this function loads the already downloaded CefSharp libraries that Synapse typically uses.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void LoadCef()
        {
            /*
            var settings = new CefSettings
            {
                BrowserSubprocessPath = path + "CefSharp.BrowserSubprocess.exe",
                LocalesDirPath = path + @"locales\",
                CachePath = path + @"GPUCache\",
                LogFile = path + "debug.log",
                ResourcesDirPath = path,
                IgnoreCertificateErrors = true
            };

            //Cef.Initialize(settings, performDependencyCheck: false, cefApp: null);

            */

            //SplashScreen splash = new SplashScreen();
            //splash.Show();
        }

        private bool ValidateSynapseInstall()
        {
            if (!File.Exists(@".\S^X.exe"))
            {
                ThrowError(BaseException.InvalidSynapseInstall);
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
                    ThrowError(BaseException.InvalidSynapseInstall);
                    return true;
                }
            }

            return false;
        }

        [STAThread]
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

            downloader.Add(new FileEntry("HelpInfo.xml"));
            downloader.Add(new FileEntry("Editor.html", "", "Monaco"));
            downloader.Add(new FileEntry("ace.js", "ace", "Monaco/ace"));
            downloader.Add(new FileEntry("mode-lua.js", "ace", "Monaco/ace"));
            downloader.Add(new FileEntry("theme-tomorrow_night_eighties.js", "ace", "Monaco/ace"));

            downloader.Begin();
        }

        private void ThrowError(BaseException error)
        {
            var err = new ErrorWindow(new Types.BaseError(error));
            err.Show();
        }
    }
}