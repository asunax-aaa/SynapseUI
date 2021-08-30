using System;
using System.IO;
using System.Windows;
using System.Reflection;
using System.Collections.Generic;
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

        public static readonly string CURRENT_DIR = Directory.GetCurrentDirectory();

        public static readonly List<string> ASSEMBLIES = new List<string>
        {
            "sxlib",
            "CefSharp.Wpf",
            "CefSharp.Core",
            "CefSharp"
        };

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

        public App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (o, e) =>
            {
                return ResolveAssembly(e);
            };
        }

        /// <summary>
        /// Loads the already pre-existing Synapse X libraries.
        /// </summary>
        /// <param name="args">The ResolveEventArgs arguments, carried on from the AssemblyResolve event.</param>
        /// <returns>The loaded assembly if found, otherwise null.</returns>
        [STAThread]
        private Assembly ResolveAssembly(ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name);
            if (!ASSEMBLIES.Contains(assemblyName.Name))
                return null;

            string probingPath = assemblyName.Name.Contains("sxlib") ?
                Path.Combine(CURRENT_DIR, "bin", "sxlib") :
                Path.Combine(CURRENT_DIR, "bin");

            string path = Path.Combine(probingPath, assemblyName.Name);
            path += !path.EndsWith(".dll") ? ".dll" : "";

            if (File.Exists(path))
            {
                var assembly = Assembly.LoadFile(path);
                return assembly;
            }

            return null;
        }

        /// <summary>
        /// Firstly, validates Synapses installation then the custom UIs installation.
        /// Secondly, initializes CefSharp libraries.
        /// Lastly, starts up the SplashScreen window if no errors occur.
        /// </summary>
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

        /// <summary>
        /// Validates Synapse X install by looking at the required folders, throws error when not found.
        /// </summary>
        /// <returns>true if an error occured, false otherwise.</returns>
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

        /// <summary>
        /// Validaes the custom installation of this UI, downloads required files.
        /// </summary>
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

            // NO LONGER NEEDED.
            /*
            downloader.Add(new FileEntry("HelpInfo.xml"));
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

        public static void Debug(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }
    }
}