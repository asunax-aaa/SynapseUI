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

        public static void SaveToXML(List<Script> scripts)
        {
            var serializer = new XmlSerializer(typeof(StoredScripts));

            var storedScripts = new StoredScripts();
            storedScripts.Scripts = scripts;
            using (TextWriter writer = new StreamWriter(DefaultPath))
            {
                serializer.Serialize(writer, storedScripts);
            }
        }
        
        public static List<Script> LoadFromXML()
        {
            if (!File.Exists(DefaultPath))
                return null;

            var serializer = new XmlSerializer(typeof(StoredScripts));

            StoredScripts storedScripts;
            using (var fs = new FileStream(DefaultPath, FileMode.Open))
            {
                storedScripts = (StoredScripts)serializer.Deserialize(fs);
            }

            return storedScripts.Scripts;
        }
    }

    [XmlRoot("StoredScripts", IsNullable = false)]
    public class StoredScripts
    {
        [XmlArray("ScriptCache")]
        public List<Script> Scripts;
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
