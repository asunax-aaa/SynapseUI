using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using CefSharp;
using CefSharp.Wpf;

namespace SynapseUI.Functions
{
    public class CefLoader
    {
        private static string path = Path.Combine(App.CURRENT_DIR, @"bin\");

        /// <summary>
        /// To maximise compatibility and reduce download times and files, this function loads the already downloaded CefSharp libraries that Synapse typically uses.
        /// </summary>
        /// <returns>True is the provided CefSharp installation is invalid, otherwise False.</returns>
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
        public event EventHandler<SaveFileEventArgs> SaveAsRequest;
        public event EventHandler OpenFileRequest;

        public void saveFileRequest(object contents)
        {
            SaveFileRequest?.Invoke(this, new SaveFileEventArgs((string)contents));
        }

        public void saveAsRequest(object contents)
        {
            SaveAsRequest?.Invoke(this, new SaveFileEventArgs((string)contents));
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
        public CustomControls.ScriptsTabPanel ScriptsPanel;

        public CefSharpService Service = new CefSharpService();
        public Dictionary<string, string> ScriptMap = new Dictionary<string, string>();

        private bool _loadFromXMLPremature = true; //false; TODO
        private bool _loadFromXMLFinished = true; //false; TODO

        public AceEditor(string url, CustomControls.ScriptsTabPanel scriptsTab) : base(url)
        {
            ScriptsPanel = scriptsTab;
            ScriptsPanel.SelectedScriptChanged += ScriptTabChanged;
            ScriptsPanel.ScriptTabClosed += ScriptTabClosed;
            ScriptsPanel.ScriptTabAdded += ScriptTabAdded;
            Service.OpenFileRequest += (o, e) => { OpenScript(); };
            Service.SaveFileRequest += (o, e) => { SaveScript(e.Value); };
            Service.SaveAsRequest += (o, e) => { SaveScript(e.Value, true); };

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

        public void SetTheme(string theme)
        {
            CefExecute("SetTheme", new object[] { theme });
        }

        public bool IsEmpty()
        {
            return GetText().Length == 0;
        }

        public void OpenScriptFile(string filename, string path)
        {
            if (ScriptMap.ContainsKey(filename))
                return;

            string contents = File.ReadAllText(path);
            ScriptMap.Add(filename, contents);
            ScriptsPanel.AddScript(filename, path, true);
        }
        
        public bool OpenScriptsFromXML()
        {
            var scripts = TabSaver.LoadFromXML();

            for (int i = 0; i < scripts.Count; i++)
            {
                if (i == scripts.Count - 1)
                    _loadFromXMLPremature = true;

                var script = scripts[i];

                if (!ScriptMap.ContainsKey(script.Filename))
                {
                    ScriptMap.Add(script.Filename, script.Contents);
                    ScriptsPanel.AddScript(script.Filename, script.Path, _loadFromXMLPremature);
                }
            }

            _loadFromXMLFinished = true;

            return scripts.Count == 0;
        }

        public void OpenScript()
        {
            var diag = Utils.Dialog.OpenFileDialog();
            switch (diag.ShowDialog())
            {
                case true:
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        OpenScriptFile(diag.SafeFileName, diag.FileName);
                    }));
                    break;

                case false:
                default:
                    break;
            }
        }

        public void SaveScript(string contents = null, bool saveAs = false)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                var tab = ScriptsPanel.SelectedTab;
                if (string.IsNullOrWhiteSpace((string)tab.Tag) || saveAs)
                {
                    // Not a script file.
                    var diag = Utils.Dialog.SaveFileDialog();
                    switch (diag.ShowDialog())
                    {
                        case true:
                            tab.Header = diag.SafeFileName;
                            tab.Tag = diag.FileName;
                            File.WriteAllText(diag.FileName, contents ?? GetText());
                            break;

                        case false:
                        default:
                            break;
                    }
                }
                else
                {
                    // Already a script file.
                    File.WriteAllText((string)tab.Tag, contents ?? GetText());
                }
            }));
        }

        private void ScriptTabChanged(object sender, CustomControls.ScriptChangedEventArgs e)
        {
            if (!_loadFromXMLPremature)
                return;

            if (ScriptsPanel.Items.Count != 1 && _loadFromXMLFinished)
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

        private void ScriptTabClosed(object sender, CustomControls.ScriptChangedEventArgs e)
        {
            ScriptMap.Remove(e.File);
        }

        private void ScriptTabAdded(object sender, CustomControls.ScriptChangedEventArgs e)
        {
            if (!_loadFromXMLFinished)
                return;

            if (!ScriptMap.ContainsKey(e.File))
            {
                ScriptMap.Add(e.File, "");
                SetText("");
            }
        }
    }

    public static class AceEditorTheme
    {
        public static string[] AceThemes = new string[]
        {
                "Ambiance",
                "Chaos",
                "Chrome",
                "Clouds",
                "Clouds-midnight",
                "Cobalt",
                "Crimson_editor",
                "Dawn",
                "Dracula",
                "Dreamweaver",
                "Eclipse",
                "Github",
                "Gob",
                "Gruvbox",
                "Idle-fingers",
                "Iplastic",
                "Katzenmilch",
                "Kr-theme",
                "Kuroir",
                "Merbivore",
                "Merbivore-soft",
                "Monokai",
                "Mono-industrial",
                "Nord-dark",
                "Pastel-on-dark",
                "Solarized-dark",
                "Solarized-light",
                "Sqlserver",
                "Terminal",
                "Textmate",
                "Tomorrow",
                "Tomorrow-night",
                "Tomorrow-night-blue",
                "Tomorrow-night-bright",
                "Tomorrow-night-eighties",
                "Twilight",
                "Vibrant-ink",
                "Xcode"
        };
    }
}
