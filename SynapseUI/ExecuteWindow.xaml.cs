using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using sxlib.Specialized;
using SynapseUI.Types;
using SynapseUI.Controls.AceEditor;
using static SynapseUI.EventMapping.EventMap;

namespace SynapseUI
{
    /// <summary>
    /// Interaction logic for ExecuteWindow.xaml
    /// </summary>
    public partial class ExecuteWindow : Window
    {
        public Options SynOptions { get; set; } = new Options();

        private SxLibWPF SxUI;
        private AceEditor Editor;
        private FileSystemWatcher ScriptWatcher;

        private bool _optionWindowOpened = false;

        public ExecuteWindow(SxLibWPF lib)
        {
            InitializeComponent();

            if (lib != null)
            {
                SxUI = lib;
                lib.SetWindow(this);

                SynOptions.CopyFrom(lib.GetOptions());

                SxUI.AttachEvent += SxUI_AttachEvent;
            }

            if (Directory.Exists("./scripts"))
            {
                ScriptWatcher = new FileSystemWatcher("./scripts");
                ScriptWatcher.Created += ScriptWatcher_Created;
                ScriptWatcher.Deleted += ScriptWatcher_Deleted;
                ScriptWatcher.Renamed += ScriptWatcher_Renamed;
                ScriptWatcher.Filter = "*";

                ScriptWatcher.EnableRaisingEvents = true;
            }
        }

        private void ExecutorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var file in new DirectoryInfo(@".\scripts\").GetFiles())
            {
                if (file.Extension == ".txt" || file.Extension == ".lua")
                    scriptsListBox.Items.Add(file.Name);
            }

            if (!App.SKIP_CEF)
                LoadCefBrowser();
        }

        private void ExecuteWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveScriptTabs();
            OptionsWindow.DisposeMutex();

            Environment.Exit(0);
        }
        

        private void LoadCefBrowser()
        {
            if (!File.Exists(@".\bin\custom\Editor.html"))
                return;

            var editor = new AceEditor(App.CURRENT_DIR + @"\bin\custom\Editor.html", scriptsTabPanel);
            cefSharpGrid.Children.Add(editor);

            Editor = editor;

            Editor.FrameLoadEnd += (s, args) =>
            {
                if (args.Frame.IsMain)
                {
                    LoadScriptTabs();
                }
            };

            Editor.Service.SaveFileRequest += async (s, args) =>
            {
                await Dispatcher.InvokeAsync(AlertFileSave);
            };

            scriptsTabPanel.RequestedTabClose += BeforeScriptTabDelete;
        }
        
        private void LoadScriptTabs()
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                bool empty = Editor.OpenScriptsFromXML();
                if (empty)
                    scriptsTabPanel.AddScript(true);

                scriptsPanel.Visibility = Visibility.Visible;
            }));
        }

        private void SaveScriptTabs()
        {
            var scripts = new List<Script>();

            var selectedTab = (Controls.ScriptTab)scriptsTabPanel.SelectedItem;
            Editor.ScriptMap[selectedTab.Header] = Editor.GetText();

            foreach (var entry in Editor.ScriptMap)
            {
                string scriptPath = Editor.ScriptPathMap.TryGetValue(entry.Key, out string path) ? path : "";
                scripts.Add(new Script(entry.Key, scriptPath, entry.Value));
            }

            TabSaver.SaveToXML(scripts, scriptsTabPanel.DefaultIndex);
        }

        private async Task AlertFileSave()
        {
            statusInfoLabel.Content = "Saved file.";
            await statusInfoLabel.SetActive(false);
        }

        private void BeforeScriptTabDelete(object sender, EventArgs args)
        {
            var tab = sender as Controls.ScriptTab;

            if (SynOptions.CloseConfirmation && scriptsTabPanel.Items.Count != 1)
            {
                if (tab == scriptsTabPanel.SelectedTab)
                {
                    if (!Editor.IsEmpty())
                    {
                        bool res = new ConfirmationWindow("Are you sure you want to close this script? All changes will be lost!").ShowDialog();
                        if (!res) return;
                    }
                }
                else
                {
                    if (Editor.ScriptMap[tab.Header].Length != 0)
                    {
                        bool res = new ConfirmationWindow("Are you sure you want to close this script? All changes will be lost!").ShowDialog();
                        if (!res) return;
                    }
                }
            }

            tab.Close();
        }

        // Script watcher events //
        private void ScriptWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (!e.Name.EndsWith(".lua") && !e.Name.EndsWith(".txt"))
                return;

            Dispatcher.BeginInvoke(new Action(delegate
            {
                int i = scriptsListBox.Items.IndexOf(e.Name);
                if (i != -1)
                    scriptsListBox.Items.RemoveAt(i);
            }));
        }

        private void ScriptWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (!e.Name.EndsWith(".lua") && !e.Name.EndsWith(".txt"))
                return;

            Dispatcher.BeginInvoke(new Action(() => scriptsListBox.Items.Add(e.Name)));
        }

        private void ScriptWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            if (!e.Name.EndsWith(".lua") && !e.Name.EndsWith(".txt"))
                return;

            Dispatcher.BeginInvoke(new Action(delegate
            {
                int i = scriptsListBox.Items.IndexOf(e.OldName);
                if (i != -1)
                {
                    scriptsListBox.Items.RemoveAt(i);
                    scriptsListBox.Items.Add(e.Name);
                }
            }));
        }

        // Sx Attach Events //
        private async void SxUI_AttachEvent(SxLibBase.SynAttachEvents Event, object Param)
        {
            attachInfoLabel.Content = AttachEventMap.TryGetValue(Event, out string name) ? name : "";

            switch (Event)
            {
                case SxLibBase.SynAttachEvents.READY:
                case SxLibBase.SynAttachEvents.NOT_INJECTED:
                case SxLibBase.SynAttachEvents.NOT_RUNNING_LATEST_VER_UPDATING:
                case SxLibBase.SynAttachEvents.NOT_UPDATED:
                case SxLibBase.SynAttachEvents.FAILED_TO_ATTACH:
                case SxLibBase.SynAttachEvents.FAILED_TO_FIND:
                case SxLibBase.SynAttachEvents.FAILED_TO_UPDATE:
                case SxLibBase.SynAttachEvents.ALREADY_INJECTED:
                    await attachInfoLabel.SetActive(false);
                    break;
            }
        }

        // Button Events //
        private void OpenOptions_Click(object sender, RoutedEventArgs e)
        {
            if (_optionWindowOpened)
                return;

            var p = new OptionsWindow(SxUI, this, Editor);
            p.Closed += (s, ev) => { _optionWindowOpened = false; };
            p.OptionChanged += (s, ev) => { SynOptions.SetProperty(ev.Entry.Name, ev.Value); };

            p.Show();

            _optionWindowOpened = true;
        }

        private void AttachButton_Click(object sender, RoutedEventArgs e)
        {
            SxUI?.Attach();
        }

        private async void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            Editor?.SaveScript();
            await AlertFileSave();
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            Editor?.OpenScript();
        }

        private void ExecuteFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (SxUI is null)
                return;

            var diag = Functions.Utils.Dialog.OpenFileDialog();
            switch (diag.ShowDialog())
            {
                case true:
                    SxUI.Execute(File.ReadAllText(diag.FileName));
                    break;

                case false:
                default:
                    break;
            }
        }

        private void ClearEditorButton_Click(object sender, RoutedEventArgs e)
        {
            if (SynOptions.ClearConfirmation)
            {
                bool res = new ConfirmationWindow("Are you sure you want to clear the editor? All changes will be lost!").ShowDialog();
                if (!res) return;
            }

            Editor?.ClearEditor();
        }

        private void ExecuteEditorButton_Click(object sender, RoutedEventArgs e)
        {
            string contents = Editor?.GetText() ?? null;
            if (contents != null && SxUI != null)
                SxUI.Execute(contents);
        }

        private void ExecuteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (SxUI is null || scriptsListBox.SelectedIndex == -1)
                return;

            string path = @".\scripts\" + (string)scriptsListBox.SelectedItem;
            if (File.Exists(path))
                SxUI.Execute(File.ReadAllText(path));
            else
                scriptsListBox.Items.RemoveAt(scriptsListBox.SelectedIndex);
        }

        private void LoadEditorMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (scriptsListBox.SelectedIndex == -1)
                return;

            string path = @".\scripts\" + (string)scriptsListBox.SelectedItem;
            if (File.Exists(path))
                Editor?.SetText(File.ReadAllText(path));
            else
                scriptsListBox.Items.RemoveAt(scriptsListBox.SelectedIndex);
        }

        private void LoadFileIntoEditor_Click(object sender, RoutedEventArgs e)
        {
            if (scriptsListBox.SelectedIndex == -1)
                return;

            string script = (string)scriptsListBox.SelectedItem;
            string path = Path.Combine(App.CURRENT_DIR, "scripts", script);
            if (File.Exists(path))
                Editor?.OpenScriptFile(script, path);
            else
                scriptsListBox.Items.RemoveAt(scriptsListBox.SelectedIndex);
        }

        private void AddScript_Click(object sender, RoutedEventArgs e)
        {
            scriptsTabPanel.AddScript();
        }

        // Window Events //
        private void ScriptsListBox_LostFocus(object sender, RoutedEventArgs e)
        {
            scriptsListBox.SelectedItem = null;
        }

        private void DraggableTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MinimiseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}