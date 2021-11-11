using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Diagnostics;
using System.Threading;
using sxlib.Specialized;
using SynapseUI.Types;
using SynapseUI.Functions.Utils;
using SynapseUI.Controls.AceEditor;
using System.IO;
using System.Linq;

namespace SynapseUI
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public static Mutex RobloxMutex;

        private static bool _mutexActive = false;
        private static string _theme = "Tomorrow-night-eighties";

        public OptionsEntryList OptionsList { get; } = new OptionsEntryList();
        public ScriptHubEntries ScriptEntries { get; } = new ScriptHubEntries();

        private SxLibWPF SxUI;
        private bool _firstLoad = true;
        private AceEditor _aceEditor;

        private Options _tempOptions = new Options();

        public OptionsWindow(SxLibWPF lib, ExecuteWindow main, AceEditor editor)
        {
            InitializeComponent();
            SxUI = lib;

            _aceEditor = editor;

            Left = main.Left + (main.ActualWidth - Width) / 2;
            Top = main.Top + 10;

            Closing += (s, e) =>
            {
                SxUI?.ScriptHubMarkAsClosed();
                App.SETTINGS.Save();
            };
        }

        private void OptionsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AnimateShow();

            LoadOptions();
            LoadScripts();

            mutexToggle.IsToggled = _mutexActive;
            aceThemesComboBox.SelectedItem = _theme;

            roundedCornerToggle.IsToggled = App.SETTINGS.RoundedCorners;

            _firstLoad = false;
        }

        private void AnimateShow()
        {
            var stry = (Storyboard)FindResource("moveWindowAnimation");
            var anim = (DoubleAnimation)stry.Children[0];

            anim.From = Top;
            anim.To = Top - 10;

            stry.Begin();
        }

        public void LoadOptions()
        {
            if (SxUI is null)
                return;

            Options options = new Options(SxUI.GetOptions());

            var presenter = VisualHelper.GetVisualChild<ItemsPresenter>(OptionsPresenter);
            var panel = VisualTreeHelper.GetChild(presenter, 0) as StackPanel;

            foreach (ContentPresenter child in panel.Children)
            {
                var slider = child.ContentTemplate.FindName("toggle", child) as Controls.SliderToggle;
                var entry = (OptionEntry)child.DataContext;

                slider.IsToggled = options.GetProperty(entry.Name);
            }
        }

        public void LoadScripts()
        {
            if (SxUI is null)
                return;

            SxUI.ScriptHubEvent += (entries) =>
            {
                foreach (var entry in entries)
                    ScriptEntries.Add(new ScriptHubEntry(entry));
            };

            SxUI.ScriptHub();
        }

        public static void DisposeMutex()
        {
            if (RobloxMutex != null)
            {
                RobloxMutex.ReleaseMutex();
                RobloxMutex.Close();
                RobloxMutex.Dispose();

                RobloxMutex = null;
            }
        }

        public static string[] GetSynapseProcesses()
        {
            string[] excluded = new string[]
            {
                "CefSharp.BrowserSubprocess.exe",
                "lua-decomp.exe"
            };

            var files = new DirectoryInfo(@".\bin\").GetFiles("*.exe")
                .Where(f => !excluded.Contains(f.Name))
                .Select(f => f.Name.Substring(0, f.Name.Length - 4))
                .ToArray();

            return files;
        }

        public event OptionChangedEventHandler OptionChanged;

        protected virtual void OnOptionChanged(OptionChangedEventArgs e)
        {
            if (SxUI is null)
                return;

            OptionChanged?.Invoke(this, e);
        }


        // Window Events //
        private void SliderToggle_ToggledStatusChanged(object sender, Controls.ToggledStatusChangedEventArgs e)
        {
            var slider = sender as Controls.SliderToggle;
            OptionEntry entry = (OptionEntry)slider.DataContext;

            _tempOptions.SetProperty(entry.Name, e.Value);
            if (!_firstLoad && SxUI != null)
            {
                SxUI.SetOptions(_tempOptions);
                OnOptionChanged(new OptionChangedEventArgs(entry, e.Value));
            }
        }

        private void Execute_ScriptButton(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;
            (image.DataContext as ScriptHubEntry).Execute();
        }

        private void KillRobloxButton_Click(object sender, RoutedEventArgs e)
        {
            var processes = Process.GetProcessesByName("RobloxPlayerBeta");
            foreach (var process in processes)
            {
                process.Kill();
            }
        }

        private void KillSynapseButton_Click(object sender, RoutedEventArgs e)
        {
            var procs = GetSynapseProcesses();
            foreach (var proc in procs)
            {
                foreach (var process in Process.GetProcessesByName(proc))
                {
                    process.Kill();
                }
            }
        }

        private void MultiRBLX_ToggledStatusChanged(object sender, Controls.ToggledStatusChangedEventArgs e)
        {
            if (e.Value)
            {
                if (RobloxMutex is null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        RobloxMutex = new Mutex(true, "ROBLOX_singletonMutex");
                    });
                }
            }
            else
            {
                DisposeMutex();
            }

            _mutexActive = e.Value;
        }

        private void RoundedCorner_ToggledStatusChanged(object sender, Controls.ToggledStatusChangedEventArgs e)
        {
            App.SETTINGS.RoundedCorners = e.Value;
        }

        private void AceThemesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_aceEditor is null)
                return;

            if (e.AddedItems.Count > 0)
            {
                string theme = (string)e.AddedItems[0];
                if (theme == _theme)
                    return;

                var builder = new StringBuilder(theme)
                    .Replace('-', '_')
                    .Remove(0, 1)
                    .Insert(0, char.ToLower(theme[0]));

                _theme = theme;
                _aceEditor.SetTheme(builder.ToString());
            }
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DraggableTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }

    public delegate void OptionChangedEventHandler(object sender, OptionChangedEventArgs e);

    public class OptionChangedEventArgs : EventArgs
    {
        public OptionEntry Entry { get; }
        public bool Value { get; }
        public OptionChangedEventArgs(OptionEntry entry, bool value)
        {
            Entry = entry;
            Value = value;
        }
    }
}
