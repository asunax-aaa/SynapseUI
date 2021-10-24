using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace SynapseUI.Controls
{
    public class OptionTab : TabItem, INotifyPropertyChanged
    {
        public new TabControl Parent
        {
            get => (TabControl)base.Parent;
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
            DependencyProperty.Register("IsMouseLeave", typeof(bool), typeof(TabControl), new PropertyMetadata(null));

        public OptionTab() : base()
        {
            MouseLeave += (o, e) =>
            {
                if (Parent != null && this != (OptionTab)Parent.SelectedItem)
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
    }
}
