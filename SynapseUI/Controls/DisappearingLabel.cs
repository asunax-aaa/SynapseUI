using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SynapseUI.Controls
{
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

        public static readonly DependencyProperty ActiveProperty = DependencyProperty.Register(
            "IsActive", typeof(bool), typeof(DisappearingLabel));

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
}
