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
using sxlib.Specialized;
using CefSharp;
using CefSharp.Wpf;
using SynapseUI.Types;
using System.IO;

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

        private void ExecutorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //var p = new OptionsWindow();
            //p.Show();

            /*
            var browser = new ChromiumWebBrowser();
            mainGrid.Children.Add(browser);
            await Task.Delay(3000);

            browser.Load("https://x.synapse.to/");
            */
        }

        private void OpenOptions_Click(object sender, RoutedEventArgs e)
        {
            if (_optionWindowOpened)
                return;

            var p = new OptionsWindow(SxUI, this);

            p.Show();
            p.Closed += delegate (object s, EventArgs eve) { _optionWindowOpened = false; };
            p.OptionChanged += delegate (object s, OptionChangedEventArgs eve) { SynOptions = eve.Option; };

            _optionWindowOpened = true;
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