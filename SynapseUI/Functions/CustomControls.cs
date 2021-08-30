using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Shapes;
using SynapseUI.Functions.Utils;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SynapseUI.CustomControls
{
    [TemplatePart(Name = "outterBar", Type = typeof(Border))]
    [TemplatePart(Name = "innerBar", Type = typeof(Border))]
    public class CustomLoadingBar : Control
    {
        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, Math.Min(value, 100)); }
        }

        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(
                "Progress", typeof(double), typeof(CustomLoadingBar),
                new PropertyMetadata(
                    new PropertyChangedCallback(ProgressChangedCallback)));

        public CustomLoadingBar()
        {
            Loaded += CustomLoadingBar_Loaded;
        }

        private void CustomLoadingBar_Loaded(object sender, RoutedEventArgs e)
        {
            OutterBorder.Width = OutterBorder.ActualWidth;
            InnerBorder.Width = (OutterBorder.Width - 2) * (Progress / 100);

            AnimationStoryboard.Completed += (s, ee) =>
            {
                Locked = false;
                AnimationStoryboard.Stop();
            };
        }

        private static void ProgressChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            CustomLoadingBar loadingBar = (CustomLoadingBar)obj;
            double value = (double)args.NewValue;

            loadingBar.OnProgressChanged(
                new ProgressChangedEventArgs(ProgressChangedEvent, value));
        }

        public static readonly RoutedEvent ProgressChangedEvent =
            EventManager.RegisterRoutedEvent("ProgressChanged", RoutingStrategy.Direct,
                typeof(ProgressChangedEventHandler), typeof(CustomLoadingBar));

        public event ProgressChangedEventHandler ProgressChanged
        {
            add { AddHandler(ProgressChangedEvent, value); }
            remove { RemoveHandler(ProgressChangedEvent, value); }
        }

        protected virtual void OnProgressChanged(ProgressChangedEventArgs e)
        {
            if (InnerBorder != null)
                SetProgress(e.Value);

            RaiseEvent(e);
        }

        public Border OutterBorder { get; private set; }
        public Border InnerBorder { get; private set; }

        public override void OnApplyTemplate()
        {
            OutterBorder = (Border)GetTemplateChild("outterBar");
            InnerBorder = (Border)GetTemplateChild("innerBar");

            SetProgress(Progress);

            base.OnApplyTemplate();
        }

        public double PercentageParse(double perc)
        {
            return (OutterBorder.Width - 2) * (perc / 100);
        }

        public void SetProgress(double value)
        {
            InnerBorder.Width = PercentageParse(value);
            Progress = value;
        }

        public bool Locked { get; private set; } = false;
        public Storyboard AnimationStoryboard { get; private set; } = new Storyboard();

        public IEasingFunction EasingFunction { get; set; } = new QuarticEase() { EasingMode = EasingMode.EaseOut };
        public double Offset { get; set; } = 1d;

        private DoubleAnimation CreateAnimation(double value, int? duration, object easingFunction = null)
        {
            var anim = new DoubleAnimation
            {
                From = InnerBorder.Width,
                To = PercentageParse(value),
                Duration = duration is null ? TimeSpan.FromMilliseconds(500 / Offset) : TimeSpan.FromMilliseconds((int)duration),
                EasingFunction = (IEasingFunction)easingFunction ?? EasingFunction
            };

            Storyboard.SetTarget(anim, InnerBorder);
            Storyboard.SetTargetProperty(anim, new PropertyPath(WidthProperty));

            return anim;
        }

        /// <summary>
        /// Animates the progress to the given progress value.
        /// </summary>
        /// <param name="targetValue">The target percentage, as a whole number</param>
        /// <param name="duration">Animation duration, in miliseconds.</param>
        /// <param name="ease">A custom easing function used by the animation, the default is a QuarticEase, EasingMode = EaseOut.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when either the outter or inner border elements are not initialised.</exception>
        public void AnimateProgress(double targetValue, int? duration = null, object ease = null)
        {
            if (InnerBorder is null || OutterBorder is null)
                throw new ArgumentNullException("Inner or outter Border element not found, maybe Style is not applied?");

            if (AnimationStoryboard.Children.Count > 0 || Locked)
            {
                var state = AnimationStoryboard.GetCurrentState();
                if (state == ClockState.Filling)
                {
                    SetProgress(targetValue);
                    return;
                }
            }

            var anim = CreateAnimation(targetValue, duration, ease);

            AnimationStoryboard.Children.Clear();
            AnimationStoryboard.Children.Add(anim);

            Locked = true;
            AnimationStoryboard.Begin();

            Progress = targetValue;
        }

        public void AnimateFinish()
        {
            AnimateProgress(100);
        }
    }

    public delegate void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e);

    public class ProgressChangedEventArgs : RoutedEventArgs
    {
        public double Value { get; }

        public ProgressChangedEventArgs(RoutedEvent e, double val)
        {
            Value = val;
            RoutedEvent = e;
        }
    }

    public class InnerBorderValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value - double.Parse((string)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Not supported");
        }
    }

    [TemplatePart(Name = "borderButton", Type = typeof(Button))]
    [TemplatePart(Name = "contentPresenter", Type = typeof(ContentPresenter))]
    public class DropDownButton : Button
    {
        private readonly string dropChr = "▼";
        private readonly string upChr = "▲";

        public bool IsDropped { get; private set; } = false;
        public double TargetHeight { get; set; } = 0;
        public Border ButtonElement { get; private set; }
        public ContentPresenter ContentPresenterElement { get; private set; }

        private double baseHeight;
        private Window window;
        public Window Window
        {
            get => window;
            set
            {
                baseHeight = value.Height;
                window = value;
            }
        }

        public DropDownButton()
        {
            Loaded += (s, e) => { ContentPresenterElement.Content = dropChr; };
        }

        public override void OnApplyTemplate()
        {
            ButtonElement = (Border)GetTemplateChild("borderButton");
            ContentPresenterElement = (ContentPresenter)GetTemplateChild("contentPresenter");
            base.OnApplyTemplate();
        }

        public IEasingFunction EasingFunction { get; set; } = new QuarticEase() { EasingMode = EasingMode.EaseOut };
        public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(500);

        public Storyboard AnimationStoryboard { get; private set; } = new Storyboard();

        /// <summary>
        /// Handles the drop down state alongside animation.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The window element is not initialised</exception>
        /// <exception cref="System.ArgumentNullException">The target height is not initialised</exception>
        protected override void OnClick()
        {
            if (Window is null)
                throw new ArgumentNullException("Window to resize is not set.");

            if (TargetHeight == 0)
                throw new ArgumentNullException("Target window height is not set.");

            if (ButtonElement is null || ContentPresenterElement is null)
                return;

            IsDropped = !IsDropped;
            var anim = new DoubleAnimation
            {
                From = Window.Height,
                To = IsDropped ? TargetHeight : baseHeight,
                Duration = Duration,
                EasingFunction = EasingFunction
            };

            Storyboard.SetTarget(anim, Window);
            Storyboard.SetTargetProperty(anim, new PropertyPath(HeightProperty));

            AnimationStoryboard.Children.Clear();
            AnimationStoryboard.Children.Add(anim);

            AnimationStoryboard.Begin();

            ContentPresenterElement.Content = IsDropped ? upChr : dropChr;
            base.OnClick();
        }
    }

    [TemplatePart(Name = "outterBorder", Type = typeof(Border))]
    [TemplatePart(Name = "innerCircle", Type = typeof(Ellipse))]
    public class SliderToggle : Button, INotifyPropertyChanged
    {
        public bool IsToggled
        {
            get { return (bool)GetValue(ToggledProperty); }
            set
            {
                SetValue(ToggledProperty, value);
                OnPropertyChanged("IsToggled");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public static readonly DependencyProperty ToggledProperty = DependencyProperty.RegisterAttached(
            "Toggled", typeof(bool), typeof(SliderToggle),
            new PropertyMetadata(
                new PropertyChangedCallback(ToggledStatusChangedCallback)));

        private static void ToggledStatusChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SliderToggle slider = (SliderToggle)obj;
            bool toggled = (bool)args.NewValue;

            slider.OnToggledStatusChanged(
                new ToggledStatusChangedEventArgs(ToggledStatusEvent, toggled));
        }

        public static readonly RoutedEvent ToggledStatusEvent =
            EventManager.RegisterRoutedEvent("ToggledStatusChanged", RoutingStrategy.Direct,
                typeof(ToggledStatusChangedEventHandler), typeof(SliderToggle));


        public event ToggledStatusChangedEventHandler ToggledStatusChanged
        {
            add { AddHandler(ToggledStatusEvent, value); }
            remove { RemoveHandler(ToggledStatusEvent, value); }
        }

        protected virtual void OnToggledStatusChanged(ToggledStatusChangedEventArgs e)
        {
            RaiseEvent(e);
        }

        public Border OutterBorder { get; private set; }
        public Ellipse InnerCircle { get; private set; }

        public override void OnApplyTemplate()
        {
            OutterBorder = (Border)GetTemplateChild("outterBorder");
            InnerCircle = (Ellipse)GetTemplateChild("innerCircle");

            InnerCircle.Margin = IsToggled ? new Thickness(18, 0, 0, 0) : new Thickness(0, 0, 18, 0);
            OutterBorder.Background = IsToggled ? HexColorConverter.Convert("#FF3EA660") : HexColorConverter.Convert("#FF72767D");

            base.OnApplyTemplate();
        }

        protected override void OnClick()
        {
            IsToggled = !IsToggled;
            base.OnClick();
        }
    }

    public delegate void ToggledStatusChangedEventHandler(object sender, ToggledStatusChangedEventArgs e);

    public class ToggledStatusChangedEventArgs : RoutedEventArgs
    {
        public bool Value { get; }

        public ToggledStatusChangedEventArgs(RoutedEvent e, bool val)
        {
            Value = val;
            RoutedEvent = e;
        }
    }


    [TemplatePart(Name = "presenter", Type = typeof(ContentPresenter))]
    public class DisappearingLabel : Label, INotifyPropertyChanged
    {
        public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(1500);

        public bool IsActive
        {
            get { return (bool)GetValue(ActiveProperty); }
            private set
            {
                SetValue(ActiveProperty, value);
                OnPropertyChanged("IsActive");
            }
        }

        public DisappearingLabel() { }

        public async Task SetActive(bool val)
        {
            if (!val)
            {
                await Task.Delay(Duration);
                IsActive = false;
                await Task.Delay(TimeSpan.FromMilliseconds(250));
                Content = null;
            }
            IsActive = val;
        }

        public static readonly DependencyProperty ActiveProperty = DependencyProperty.Register(
            "IsActive", typeof(bool), typeof(DisappearingLabel));

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (!IsActive && Content != null)
                IsActive = true;

            base.OnContentChanged(oldContent, newContent);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class ScriptTab : TabItem, INotifyPropertyChanged
    {
        public new ScriptsTabPanel Parent
        {
            get => (ScriptsTabPanel)base.Parent;
        }

        public bool IsMouseLeave
        {
            get { return (bool)GetValue(IsMouseLeaveTriggeredProperty); }
            set
            {
                SetValue(IsMouseLeaveTriggeredProperty, value);
                OnPropertyChanged("IsMouseLeave");
            }
        }

        public static readonly DependencyProperty IsMouseLeaveTriggeredProperty =
            DependencyProperty.Register("IsMouseLeave", typeof(bool), typeof(ScriptsTabPanel), new PropertyMetadata(null));

        public ScriptTab() : base()
        {
            MouseLeave += (o, e) =>
            {
                if (Parent != null && this != Parent.SelectedTab)
                    IsMouseLeave = true;
            };

            MouseEnter += (o, e) =>
            {
                if (!IsMouseLeave)
                    IsMouseLeave = true;
                IsMouseLeave = false;
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override void OnApplyTemplate()
        {
            var child = (Border)VisualTreeHelper.GetChild(this, 0);
            var closeButton = (Button)child.FindName("closeButton");
            closeButton.Click += (o, e) => { Close(); };

            base.OnApplyTemplate();
        }
        
        public void Close()
        {
            var parent = Parent;
            if (parent.Items.Count != 1)
            {
                parent.Items.Remove(this);
                parent.ScriptTabDeleted?.Invoke(this, new ScriptChangedEventArgs((string)Header, (string)Tag));
            }
        }
    }

    public class ScriptsTabPanel : TabControl
    {
        private int defaultIndex = 0;
        private int lastIndex = 0;

        public ScriptTab LastItem
        {
            get => lastIndex != -1 ? (ScriptTab)Items[lastIndex] : null;
        }

        public ScriptTab SelectedTab
        {
            get => (ScriptTab)SelectedItem;
        }

        public EventHandler<ScriptChangedEventArgs> SelectedScriptChanged;
        public EventHandler<ScriptChangedEventArgs> ScriptTabDeleted;
        public EventHandler<ScriptChangedEventArgs> ScriptTabAdded;

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            var tab = SelectedTab;
            if (tab != null)
                SelectedScriptChanged?.Invoke(this, new ScriptChangedEventArgs((string)tab.Header, (string)tab.Tag));

            lastIndex = SelectedIndex;
            if (LastItem != null)
                LastItem.IsMouseLeave = true;

            base.OnSelectionChanged(e);
        }

        public ScriptTab AddScript(string header, string dir, bool update = false)
        {
            foreach (ScriptTab item in Items)
                if ((string)item.Header == header && (string)item.Tag == dir)
                    return null;
            
            var tab = new ScriptTab
            {
                Header = header,
                Tag = dir,
                Background = HexColorConverter.Convert("#FF121212")
            };

            Items.Add(tab);
            SelectedItem = tab;

            if (update)
                tab.IsMouseLeave = false;

            ScriptTabAdded?.Invoke(this, new ScriptChangedEventArgs(header, dir));

            return tab;
        }

        public ScriptTab AddScript(bool update = false)
        {
            return AddScript($"Script {++defaultIndex}", "", update);
        }
    }

    public class ScriptChangedEventArgs : EventArgs
    {
        public string File { get; }
        public string Dir { get; }
        public ScriptChangedEventArgs(string name, string dir)
        {
            File = name;
            Dir = dir;
        }
    }
}
