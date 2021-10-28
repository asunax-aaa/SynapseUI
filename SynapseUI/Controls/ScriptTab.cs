using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SynapseUI.Functions.Utils;

namespace SynapseUI.Controls
{
    public class ScriptTab : TabItem, INotifyPropertyChanged
    {
        public new ScriptsTabPanel Parent
        {
            get => (ScriptsTabPanel)base.Parent;
        }

        public new string Header
        {
            get => (string)base.Header;
            set => base.Header = value;
        }

        public string FilePath { get; set; }

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

        public override void OnApplyTemplate()
        {
            var child = (Border)VisualTreeHelper.GetChild(this, 0);
            var closeButton = (Button)child.FindName("closeButton");
            closeButton.Click += (o, e) => { BeforeClose(); };

            base.OnApplyTemplate();
        }

        private void BeforeClose()
        {
            if (Parent.RequestedTabClose is null)
                Close();
            else
                Parent.RequestedTabClose?.Invoke(this, EventArgs.Empty);
        }

        public void Close()
        {
            var parent = Parent;
            if (parent.Items.Count != 1)
            {
                parent.Items.Remove(this);
                parent.ScriptTabClosed?.Invoke(this, new ScriptChangedEventArgs(Header, FilePath));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class ScriptsTabPanel : TabControl
    {
        public int DefaultIndex { get; set; } = 0;

        private int lastIndex = 0;

        public ScriptTab LastItem
        {
            get => lastIndex >= 0 && lastIndex < Items.Count ? (ScriptTab)Items[lastIndex] : null;
        }

        public ScriptTab SelectedTab
        {
            get => (ScriptTab)SelectedItem;
        }

        public EventHandler<ScriptChangedEventArgs> SelectedScriptChanged;
        public EventHandler<ScriptChangedEventArgs> ScriptTabClosed;
        public EventHandler<ScriptChangedEventArgs> ScriptTabAdded;

        public EventHandler RequestedTabClose;

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            var tab = SelectedTab;
            if (tab != null)
                SelectedScriptChanged?.Invoke(this, new ScriptChangedEventArgs(tab.Header, tab.FilePath));

            lastIndex = SelectedIndex;
            if (LastItem != null)
                LastItem.IsMouseLeave = true;

            base.OnSelectionChanged(e);
        }

        public ScriptTab AddScript(string header, string dir, bool update = false)
        {
            foreach (ScriptTab item in Items)
                if (item.Header == header && item.FilePath == dir)
                    return null;

            var tab = new ScriptTab
            {
                Header = header,
                FilePath = dir,
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
            string name = $"Script {DefaultIndex + 1}";

            foreach (ScriptTab item in Items)
                if (item.Header == name)
                    DefaultIndex++;

            return AddScript($"Script {++DefaultIndex}", "", update);
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
