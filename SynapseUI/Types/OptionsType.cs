using System.Collections.ObjectModel;
using System.ComponentModel;
using sxlib.Static;

namespace SynapseUI.Types
{
    /// <summary>
    /// A custom implementaion of how Synapse stores and handles options, it's practically the same apart from it includes { get; set; } accessors,
    /// which allow the property to be detected by object.GetProperty().
    /// </summary>
    public class Options : Data.Options, INotifyPropertyChanged
    {
        public new bool AutoAttach
        {
            get => base.AutoAttach;
            set => base.AutoAttach = value;
        }

        public new bool AutoJoin // Unused by sxlib.
        {
            get => base.AutoJoin;
            set => base.AutoJoin = value;
        }

        public new bool AutoLaunch
        {
            get => base.AutoLaunch;
            set => base.AutoLaunch = value;
        }

        public new bool ClearConfirmation
        {
            get => base.ClearConfirmation;
            set => base.ClearConfirmation = value;
        }

        public new bool CloseConfirmation
        {
            get => base.CloseConfirmation;
            set => base.CloseConfirmation = value;
        }

        public new bool InternalUI
        {
            get => base.InternalUI;
            set => base.InternalUI = value;
        }

        public new bool TopMost
        {
            get => base.TopMost;
            set
            {
                base.TopMost = value;
                OnPropertyChanged("TopMost");
            }
        }

        public new bool UnlockFPS
        {
            get => base.UnlockFPS;
            set => base.UnlockFPS = value;
        }

        public Options() : base() { }

        public Options(Data.Options options)
        {
            CopyFrom(options);
        }

        public void CopyFrom(Data.Options options)
        {
            AutoAttach = options.AutoAttach;
            AutoJoin = options.AutoJoin;
            AutoLaunch = options.AutoLaunch;
            ClearConfirmation = options.ClearConfirmation;
            CloseConfirmation = options.CloseConfirmation;
            InternalUI = options.InternalUI;
            TopMost = options.TopMost;
            UnlockFPS = options.UnlockFPS;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void SetProperty(string name, bool value)
        {
            var propInfo = this.GetType().GetProperty(name);
            propInfo.SetValue(this, value);
        }

        public bool GetProperty(string name)
        {
            var propInfo = this.GetType().GetProperty(name);
            return (bool)propInfo.GetValue(this);
        }
    }

    public class OptionEntry
    {
        public string FriendlyName { get; set; }
        public string Name { get; set; }

        public OptionEntry(string friendlyName, string name)
        {
            FriendlyName = friendlyName;
            Name = name;
        }
    }

    public class OptionsEntryList : ObservableCollection<OptionEntry>
    {
        public OptionsEntryList() : base()
        {
            Add(new OptionEntry("Unlock FPS", "UnlockFPS"));
            Add(new OptionEntry("Auto-Launch", "AutoLaunch"));
            Add(new OptionEntry("Auto-Attach", "AutoAttach"));
            Add(new OptionEntry("Clear Editor Prompt", "ClearConfirmation"));
            Add(new OptionEntry("File Closing Prompt", "CloseConfirmation"));
            Add(new OptionEntry("Internal UI", "InternalUI"));
            Add(new OptionEntry("Top Most", "TopMost"));
        }
    }
}
