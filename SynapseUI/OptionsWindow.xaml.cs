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
        public Options TempOptions = new Options();

        private SxLibWPF SxUI;
        private bool _firstLoad = true;

        public OptionsWindow(SxLibWPF lib, ExecuteWindow main)
        {
            InitializeComponent();
            SxUI = lib;

            Left = main.Left + (main.ActualWidth - Width) / 2;
            Top = main.Top + 10;

            Loaded += delegate
            {
                AnimateShow();
                LoadOptions();
                _firstLoad = false;
            };
        }

        public event OptionChangedEventHandler OptionChanged;

        protected virtual void OnOptionChanged(OptionChangedEventArgs e)
        {
            if (SxUI is null)
                return;

            OptionChanged?.Invoke(this, e);
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
                var grid = child.ContentTemplate.FindName("gridContainer", child) as Grid;
                var slider = grid.Children[1] as CustomControls.SliderToggle;

                var entry = (OptionEntry)child.DataContext;
                slider.IsToggled = options.GetProperty(entry.Name);
            }
        }

        // Window Events //
        private void SliderToggle_ToggledStatusChanged(object sender, CustomControls.ToggledStatusChangedEventArgs e)
        {
            CustomControls.SliderToggle slider = (CustomControls.SliderToggle)sender;
            OptionEntry entry = (OptionEntry)slider.DataContext;

            TempOptions.SetProperty(entry.Name, e.Value);
            if (!_firstLoad && SxUI != null)
            {
                SxUI.SetOptions(TempOptions);
                OnOptionChanged(new OptionChangedEventArgs(TempOptions));
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
        public Options Option { get; private set; }
        public OptionChangedEventArgs(Options opt)
        {
            Option = opt;
        }
    }
}
