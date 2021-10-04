using System.Collections.Generic;
using System.Collections.ObjectModel;
using static sxlib.Specialized.SxLibBase;

namespace SynapseUI.Types
{
    public static class ScriptResourceMap
    {
        public static Dictionary<string, string> ScriptMap = new Dictionary<string, string>
        {
            { "Dark Dex", "dark_dex.png" },
            { "Unnamed ESP", "esp.png" },
            { "Remote Spy", "remote_spy.png" },
            { "Script Dumper", "script_dumper.png" }
        };

    }

    public class ScriptHubEntry
    {
        public SynHubEntry Entry { get; }
        public string Name
        {
            get => Entry.Name;
        }
        public string ImageSource
        {
            get
            {
                if (ScriptResourceMap.ScriptMap.TryGetValue(Name, out string image))
                    return $"pack://application:,,,/SynapseUI;component/Resources/ScriptHub/{image}";

                return "";
            }
        }

        public ScriptHubEntry(SynHubEntry entry)
        {
            Entry = entry;
        }

        public void Execute()
        {
            Entry?.Execute();
        }
    }

    public class ScriptHubEntries : ObservableCollection<ScriptHubEntry>
    {
        public ScriptHubEntries() : base() { }
    }
}
