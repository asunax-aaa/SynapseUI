using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using sxlib.Specialized;
using SynapseUI.Types;
using SynapseUI.Functions.Utils;

namespace SynapseUI
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public OptionsEntryList OptionsList { get; } = new OptionsEntryList();
        public ScriptHubEntries ScriptEntries { get; } = new ScriptHubEntries();

        private SxLibWPF SxUI;
        private bool _firstLoad = true;

        private Options _tempOptions = new Options();

        public OptionsWindow(SxLibWPF lib, ExecuteWindow main)
        {
            InitializeComponent();
            SxUI = lib;

            Left = main.Left + (main.ActualWidth - Width) / 2;
            Top = main.Top + 10;

            Loaded += OptionsWindow_Loaded;
            Closing += (s, e) =>
            {
                SxUI.ScriptHubMarkAsClosed();
            };
        }

        private void OptionsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AnimateShow();

            LoadOptions();
            LoadScripts();

            _firstLoad = false;
        }

        /// <summary>
        /// Animates the startup visuals of the Options window, opacity and position animation.
        /// </summary>
        private void AnimateShow()
        {
            var stry = (Storyboard)FindResource("moveWindowAnimation");
            var anim = (DoubleAnimation)stry.Children[0];

            anim.From = Top;
            anim.To = Top - 10;

            stry.Begin();
        }

        /// <summary>
        /// Loads the current saved option and toggles the visual switches.
        /// </summary>
        public void LoadOptions()
        {
            if (SxUI is null)
                return;

            Options options = new Options(SxUI.GetOptions());

            var presenter = VisualHelper.GetVisualChild<ItemsPresenter>(OptionsPresenter);
            var panel = VisualTreeHelper.GetChild(presenter, 0) as StackPanel;

            foreach (ContentPresenter child in panel.Children)
            {
                var slider = child.ContentTemplate.FindName("toggle", child) as CustomControls.SliderToggle;
                var entry = (OptionEntry)child.DataContext;

                slider.IsToggled = options.GetProperty(entry.Name);
            }
        }

        /// <summary>
        /// Loads Synapse X script hub entries.
        /// </summary>
        public void LoadScripts()
        {
            SxUI.ScriptHubEvent += (entries) =>
            {
                foreach (var entry in entries)
                    ScriptEntries.Add(new ScriptHubEntry(entry));
            };

            SxUI.ScriptHub();
        }

        public event OptionChangedEventHandler OptionChanged;

        protected virtual void OnOptionChanged(OptionChangedEventArgs e)
        {
            if (SxUI is null)
                return;

            OptionChanged?.Invoke(this, e);
        }


        // Window Events //
        private void SliderToggle_ToggledStatusChanged(object sender, CustomControls.ToggledStatusChangedEventArgs e)
        {
            var slider = sender as CustomControls.SliderToggle;
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
