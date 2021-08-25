using CefSharp;
using CefSharp.Wpf;
using sxlib.Specialized;
using SynapseUI.Functions;
using SynapseUI.Types;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using static SynapseUI.Functions.EventMapNames.EventMap;

namespace SynapseUI
{
    /// <summary>
    /// Interaction logic for ExecuteWindow.xaml
    /// </summary>
    public partial class ExecuteWindow : Window
    {
        public Options SynOptions;

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

                SynOptions = new Options(lib.GetOptions());

                SxUI.AttachEvent += SxUI_AttachEvent;
            }

            if (Directory.Exists("./scripts"))
            {
                ScriptWatcher = new FileSystemWatcher("./scripts");
                ScriptWatcher.Created += ScriptWatcher_Created;
                ScriptWatcher.Deleted += ScriptWatcher_Deleted;
                ScriptWatcher.Filter = "*";

                ScriptWatcher.EnableRaisingEvents = true;
            }

            Loaded += ExecutorWindow_Loaded;
        }

        private void ExecutorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Adds scripts to scripts listbox.
            foreach (var file in new DirectoryInfo(@".\scripts\").GetFiles())
            {
                if (!(file.Extension != ".txt" || file.Extension != ".lua"))
                    return;

                scriptsListBox.Items.Add(file.Name);
            }

            // Add the CefSharp browser
            if (!App.SKIP_CEF)
            {
                if (!File.Exists(@".\bin\custom\Editor.html"))
                    return;

                var editor = new AceEditor(Directory.GetCurrentDirectory() + @"\bin\custom\Editor.html", scriptsTabPanel);
                cefSharpGrid.Children.Add(editor);

                Editor = editor;
            }
        }

        private void SaveEditorScript()
        {

        }

        private void OpenEditorScript()
        {

        }

        // SCRIPT WATCHER EVENTS
        private void ScriptWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (!(e.Name.EndsWith(".txt") || e.Name.EndsWith(".lua")))
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
            if (!(e.Name.EndsWith(".txt") || e.Name.EndsWith(".lua")))
                return;

            Dispatcher.BeginInvoke(new Action(delegate
            {
                scriptsListBox.Items.Add(e.Name);
            }));
        }

        // SX ATTACH EVENTS
        private async void SxUI_AttachEvent(SxLibBase.SynAttachEvents Event, object Param)
        {
            statusInfoLabel.Content = AttachEventMap.TryGetValue(Event, out string name) ? name : "";

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
                    await statusInfoLabel.SetActive(false);
                    break;
            }

        }

        // BUTTON EVENTS
        private void OpenOptions_Click(object sender, RoutedEventArgs e)
        {
            if (_optionWindowOpened)
                return;

            var p = new OptionsWindow(SxUI, this);

            p.Show();
            p.Closed += (s, ev) => { _optionWindowOpened = false; };
            p.OptionChanged += (s, ev) => { SynOptions = ev.Option; };

            _optionWindowOpened = true;
        }

        private void AttachButton_Click(object sender, RoutedEventArgs e)
        {
            if (SxUI != null)
                SxUI.Attach();
        }

        private void OpenScriptHubButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExecuteFileButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearEditorButton_Click(object sender, RoutedEventArgs e)
        {
            Editor.ClearEditor();
        }

        private void ExecuteEditorButton_Click(object sender, RoutedEventArgs e)
        {
            var contents = Editor.GetText();
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
        }

        private void LoadEditorMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (scriptsListBox.SelectedIndex == -1)
                return;

            string path = @".\scripts\" + (string)scriptsListBox.SelectedItem;
            if (File.Exists(path))
                Editor.SetText(File.ReadAllText(path));
        }

        // WINDOW EVENTS
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
            Environment.Exit(0); // Closes all Windows, unlike this.Close();
        }

        private void ScriptsListBox_LostFocus(object sender, RoutedEventArgs e) => scriptsListBox.SelectedItem = null;

        private void AddScript_Click(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            scriptsTabPanel.AddScript(r.Next(100).ToString(), "");
        }
    }
}


// Currently do not have a great method of maximising the window as the 'intended' method doesn't produce desired results.
// Most likely due to how the window is set up, a hack that makes the window resizable without a border.
/*
private void MaximiseWindow_Click(object sender, RoutedEventArgs e)
{
    if (WindowState == WindowState.Normal)
    {
        maximiseWindow.Content = char.ConvertFromUtf32(59683).ToString();
        WindowState = WindowState.Maximized;
    }
    else
    {
        WindowState = WindowState.Normal;
        maximiseWindow.Content = char.ConvertFromUtf32(59193).ToString();
    }
}
*/