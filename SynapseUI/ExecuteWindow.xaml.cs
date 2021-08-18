using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using sxlib.Specialized;
using CefSharp;
using CefSharp.Wpf;
using SynapseUI.Types;
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
                Functions.CefLoader.InitBrowser(cefSharpGrid);
            }


            /*
            var browser = new ChromiumWebBrowser();
            mainGrid.Children.Add(browser);
            await Task.Delay(3000);

            browser.Load("https://x.synapse.to/");
            */
        }

        private void ScriptWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (!(e.Name.EndsWith(".txt") || e.Name.EndsWith(".lua")))
                return;

            this.Dispatcher.Invoke(() =>
            {
                int i = scriptsListBox.Items.IndexOf(e.Name);
                if (i != -1)
                    scriptsListBox.Items.RemoveAt(i);
            });
        }

        private void ScriptWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (!(e.Name.EndsWith(".txt") || e.Name.EndsWith(".lua")))
                return;

            this.Dispatcher.Invoke(() =>
            {
                scriptsListBox.Items.Add(e.Name);
            });
        }

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

        }

        private void ExecuteEditorButton_Click(object sender, RoutedEventArgs e)
        {

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