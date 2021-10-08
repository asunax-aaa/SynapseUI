﻿using System;
using System.IO;
using System.Windows;
using System.Reflection;
using System.Diagnostics;
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
            string message = $"Screenshot this and send it to asunax#5833. \n\nException: {e.Exception.GetType()}\nMessage: {e.Exception.Message}";
            Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ThrowError(BaseException.GENERIC_EXCEPTION, message);
            }));

            e.Handled = true;
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

            new SplashScreen().Show();
        }

        /// <summary>
        /// Checks whether there already is a custom UI instance already running.
        /// </summary>
        /// <returns>True if an instance is already running, else False.</return>
        private bool CheckProcesses()
        {
            string name = AppDomain.CurrentDomain.FriendlyName.Replace(".exe", "");
            var procs = Process.GetProcessesByName(name);

            return procs.Length != 1;
        }

        /// <summary>
        /// Validates Synapse X install by looking at the required folders, throws error when not found.
        /// </summary>
        /// <returns>True if an error occured, False otherwise.</returns>
        private bool ValidateSynapseInstall()
        {
            if (!File.Exists(@".\S^X.exe"))
                return true;

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

        /// <summary>
        /// Validaes the custom installation of this UI, downloads required files.
        /// </summary>
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