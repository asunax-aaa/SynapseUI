using System;
using System.Collections.Generic;
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
    }

    public class CefSharpService
    {
        public event EventHandler<SaveFileEventArgs> SaveFileRequest;
        public event EventHandler OpenFileRequest;

        public void saveFileRequest(object contents)
        {
            SaveFileRequest?.Invoke(this, new SaveFileEventArgs((string)contents));
        }

        public void openFileRequest()
        {
            OpenFileRequest?.Invoke(this, EventArgs.Empty);
        }
    }

    public class SaveFileEventArgs : EventArgs
    {
        public string Value { get; }
        public SaveFileEventArgs(string value)
        {
            Value = value;
        }
    }

    public class AceEditor : ChromiumWebBrowser
    {
        public CefSharpService Service = new CefSharpService();
        public CustomControls.ScriptsTabPanel ScriptsPanel;

        public Dictionary<string, string> ScriptMap = new Dictionary<string, string>();

        public AceEditor(string url, CustomControls.ScriptsTabPanel scriptsTab) : base(url)
        {
            ScriptsPanel = scriptsTab;
            ScriptsPanel.SelectedScriptChanged += ScriptTabChanged;
            ScriptsPanel.ScriptTabDeleted += ScriptTabDeleted;
            ScriptsPanel.ScriptTabAdded += ScriptTabAdded;
            Service.OpenFileRequest += (o, e) => { OpenScript(); };

            Loaded += (o, e) =>
            {
                JavascriptObjectRepository.Register("cefServiceAsync", Service, true, BindingOptions.DefaultBinder);
            };
        }

        public object CefExecute(string script, object[] args = null, bool eval = false)
        {
            if (IsInitialized && IsLoaded)
            {
                if (eval)
                {
                    var result = this.EvaluateScriptAsync(script, args ?? new object[] { });
                    return result.Result.Result;
                }
                else
                {
                    this.ExecuteScriptAsync(script, args ?? new object[] { });
                }
            }

            return null;
        }

        public void SetText(string contents)
        {
            CefExecute("SetText", new object[] { contents });
        }

        public void ClearEditor()
        {
            CefExecute("ClearText");
        }

        public string GetText()
        {
            var contents = (string)CefExecute("GetText", null, true);
            return contents;
        }

        public void OpenScript()
        {
            var diag = Utils.Dialog.OpenFileDialog();
            switch (diag.ShowDialog())
            {
                case true:
                    string filename = Path.GetFileName(diag.FileName);
                    if (ScriptMap.ContainsKey(filename))
                        return;
                    string contents = File.ReadAllText(diag.FileName);
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        ScriptMap.Add(filename, contents);
                        ScriptsPanel.AddScript(filename, diag.FileName);
                    }));
                    break;

                case false:
                default:
                    break;
            }
        }

        private void ScriptTabChanged(object sender, CustomControls.ScriptsTabPanel.ScriptChangedEventArgs e)
        {
            if (ScriptsPanel.Items.Count != 1)
            {
                if (ScriptsPanel.LastItem != null)
                {
                    var item = ScriptsPanel.LastItem;
                    ScriptMap[(string)item.Header] = GetText();
                }
            }

            if (ScriptMap.TryGetValue(e.File, out string contents))
            {
                SetText(contents);
            }
        }

        private void ScriptTabDeleted(object sender, CustomControls.ScriptsTabPanel.ScriptChangedEventArgs e)
        {
            ScriptMap.Remove(e.File);
        }

        private void ScriptTabAdded(object sender, CustomControls.ScriptsTabPanel.ScriptChangedEventArgs e)
        {
            if (!ScriptMap.ContainsKey(e.File))
            {
                ScriptMap.Add(e.File, "");
                SetText("");
            }
        }

    }
}
