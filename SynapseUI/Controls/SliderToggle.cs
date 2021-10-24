using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using SynapseUI.Functions.Utils;

namespace SynapseUI.Controls
{
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
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
}
