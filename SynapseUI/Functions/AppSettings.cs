using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SynapseUI.Settings
{
    public class Settings
    {
        private static string defaultFilename = "settings.xml";
        private static string defaultPath = Path.Combine(App.CURRENT_DIR, @"bin\custom", defaultFilename);

        public static AppSettings Load()
        {
            if (!File.Exists(defaultPath))
            {
                var empty = new AppSettings();
                Save(empty);

                return empty;
            }

            var serializer = new XmlSerializer(typeof(AppSettings));

            AppSettings settings;
            using (var fs = new FileStream(defaultPath, FileMode.Open))
            {
                settings = (AppSettings)serializer.Deserialize(fs);
            }

            return settings;
        }

        public static void Save(AppSettings settings)
        {
            var serializer = new XmlSerializer(typeof(AppSettings));
            using (TextWriter writer = new StreamWriter(defaultPath))
            {
                serializer.Serialize(writer, settings);
            }
        }
    }

    [XmlRoot("AppSettings", IsNullable = false)]
    public class AppSettings : INotifyPropertyChanged
    {
        private bool roundedCorners = false;

        [XmlElement("RoundedCorners")]
        public bool RoundedCorners
        {
            get => roundedCorners;
            set
            {
                roundedCorners = value;
                RoundedValue = value ? 15 : 0;
            }
        }

        private int roundedValue = 0;
        [XmlIgnore]
        public int RoundedValue
        {
            get => roundedValue;
            set
            {
                roundedValue = value;
                OnPropertyChanged("RoundedValue");
            }
        }

        public AppSettings() { }

        public void Load()
        {
            var settings = Settings.Load();
            RoundedCorners = settings.RoundedCorners;
        }

        public void Save()
        {
            Settings.Save(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
