using System;
using System.IO;
using System.Windows;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using SynapseUI.Exceptions;
using SynapseUI.Functions.Utils;
using SynapseUI.Functions.Web;
using SynapseUI.Controls.AceEditor;

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

        public static Settings.AppSettings SETTINGS = new Settings.AppSettings();

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

        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.AssemblyResolve += (o, ev) =>
            {
                return ResolveAssembly(ev);
            };

            DispatcherUnhandledException += App_DispatcherUnhandledException;

            base.OnStartup(e);
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string message = ErrorGen.ErrorToMessage(e);

            Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ThrowError(BaseException.GENERIC_EXCEPTION, message);
            }));

            e.Handled = true;
        }

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

        [STAThread]
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            if (CheckProcesses())
            {
                ThrowError(BaseException.ALREADY_RUNNING);
                return;
            }

            if (ValidateSynapseInstall())
            {
                ThrowError(BaseException.INVALID_SYNAPSE_INSTALL);
                return;
            }

            ValidateCustomInstall();

            if (!SKIP_CEF)
            {
                if (CefLoader.Init())
                {
                    ThrowError(BaseException.CEF_NOT_FOUND);
                    return;
                }
            }

            SETTINGS.Load();

            new SplashScreen().Show();
        }

        private bool CheckProcesses()
        {
            string name = AppDomain.CurrentDomain.FriendlyName.Replace(".exe", "");
            var procs = Process.GetProcessesByName(name);

            return procs.Length != 1;
        }

        private bool ValidateSynapseInstall()
        {
            //if (!File.Exists(@".\S^X.exe"))
            //    return true;

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
                    return true;
            }

            return false;
        }

        private void ValidateCustomInstall()
        {
            string[] folders = new string[]
            {
                @".\bin\custom\",
                @".\bin\custom\ace\"
            };

            foreach (string folder in folders)
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

            var downloader = new FileDownloader
            {
                BasePath = CURRENT_DIR + @"\bin\custom\",
                BaseUrl = @"https://raw.githubusercontent.com/asunax-aaa/SynapseUI/master/SynapseUI/Resources/"
            };

            downloader.Add(new FileEntry("Editor.html", "", "Monaco"));
            downloader.Add(new FileEntry("mode-lua.js", "ace", "Monaco/ace"));
            downloader.Add(new FileEntry("Updater.exe"));
            downloader.Add(new FileEntry("SLInjector.dll", Path.Combine(CURRENT_DIR, "bin"), "UnknownPatch", false));

            if (!DEBUG)
                VersionChecker.Run(downloader);

            downloader.Begin();
        }

        private void ThrowError(BaseException error, string helpInfo)
        {
            new ErrorWindow(new Types.BaseError(error), helpInfo).ShowDialog();
        }

        private void ThrowError(BaseException error)
        {
            new ErrorWindow(new Types.BaseError(error)).ShowDialog();
        }

        public static void Debug(string text)
        {
            System.Diagnostics.Debug.WriteLine(text);
        }
    }
}