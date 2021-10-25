using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SynapseUI.Controls.AceEditor
{
    public static class TabSaver
    {
        public static string DefaultFilename = "script_cache.xml";
        public static string DefaultPath = Path.Combine(App.CURRENT_DIR, @"bin\custom", DefaultFilename);

        public static void SaveToXML(List<Script> scripts, int defaultIndex = 0)
        {
            var serializer = new XmlSerializer(typeof(StoredScripts));

            var storedScripts = new StoredScripts();
            storedScripts.Scripts = scripts;
            storedScripts.DefaultIndex = defaultIndex;
            using (TextWriter writer = new StreamWriter(DefaultPath))
            {
                serializer.Serialize(writer, storedScripts);
            }
        }
        
        public static StoredScripts LoadFromXML()
        {
            if (!File.Exists(DefaultPath))
                return null;

            var serializer = new XmlSerializer(typeof(StoredScripts));

            StoredScripts storedScripts;
            using (var fs = new FileStream(DefaultPath, FileMode.Open))
            {
                storedScripts = (StoredScripts)serializer.Deserialize(fs);
            }

            return storedScripts;
        }
    }

    [XmlRoot("StoredScripts", IsNullable = false)]
    public class StoredScripts
    {
        [XmlArray("ScriptCache")]
        public List<Script> Scripts;

        [XmlAttribute("DefaultIndex")]
        public int DefaultIndex = 0;
    }

    [Serializable]
    public class Script
    {
        [XmlAttribute("Filename")]
        public string Filename;

        [XmlAttribute("Path")]
        public string Path;

        public string Contents;

        public Script() { }

        public Script(string filename, string path, string contents)
        {
            Filename = filename;
            Path = path;
            Contents = contents;
        }
    }
}
