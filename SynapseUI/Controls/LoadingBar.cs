using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Data;
using System.Globalization;
using SynapseUI.Functions.Utils;

namespace SynapseUI.Controls
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

        private readonly TimeSpan Duration = TimeSpan.FromMilliseconds(500);

        private DoubleAnimation CreateAnimation(double value)
        {
            var anim = new DoubleAnimation
            {
                From = InnerBorder.Width,
                To = PercentageParse(value),
                Duration = Duration,
                EasingFunction = Animation.QuarticEase
            };

            Storyboard.SetTarget(anim, InnerBorder);
            Storyboard.SetTargetProperty(anim, new PropertyPath(WidthProperty));

            return anim;
        }

        public void AnimateProgress(double targetValue)
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

            var anim = CreateAnimation(targetValue);

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
}