using System.Collections.Generic;
using static sxlib.Specialized.SxLibBase;

namespace SynapseUI.EventMapping
{
    public static class EventMap
    {
        /// <summary>
        /// Friendly display names for Synapse loading events.
        /// </summary>
        public static Dictionary<SynLoadEvents, string> LoadEventMap = new Dictionary<SynLoadEvents, string>
        {
            { SynLoadEvents.ALREADY_EXISTING_WL, "Whitelist already exists." },
            { SynLoadEvents.CHANGING_WL, "Changing whitelist..." },
            { SynLoadEvents.CHECKING_DATA, "Checking data..." },
            { SynLoadEvents.CHECKING_WL, "Checking whitelist..." },
            { SynLoadEvents.DOWNLOADING_DATA, "Downloading data..." },
            { SynLoadEvents.DOWNLOADING_DLLS, "Downloading DLLs..." },
            { SynLoadEvents.FAILED_TO_DOWNLOAD, "Failed to download files." },
            { SynLoadEvents.FAILED_TO_VERIFY, "Failed to verify files." },
            { SynLoadEvents.NOT_ENOUGH_TIME, "24 Hour Error." },
            { SynLoadEvents.NOT_LOGGED_IN, "Not logged in." }, // should not occur as logins happen through official UI
            { SynLoadEvents.NOT_UPDATED, "Synapse is not updated." },
            { SynLoadEvents.READY, "Ready!" },
            { SynLoadEvents.UNAUTHORIZED_HWID, "Unauthorised HWID." },
            { SynLoadEvents.UNKNOWN, "Unknown Error" }
        };

        /// <summary>
        /// Critical Synapse loading errors that will halt the program and open up a new 'Error Window'.
        /// </summary>
        public static Dictionary<SynLoadEvents, string> LoadErrorEvents = new Dictionary<SynLoadEvents, string>
        {
            { SynLoadEvents.ALREADY_EXISTING_WL, "Rare error, whitelist already exists."},
            { SynLoadEvents.FAILED_TO_DOWNLOAD, "Failed to download Synapse files."},
            { SynLoadEvents.FAILED_TO_VERIFY, "Failed to verify Synapse files." },
            { SynLoadEvents.NOT_ENOUGH_TIME, "You changed your whitelist too recently, Please wait 24 hours and try again." },
            { SynLoadEvents.NOT_LOGGED_IN, "You are not logged into Synapse."},
            { SynLoadEvents.NOT_UPDATED, "Synapse is currently not updated, please wait for an update." },
            { SynLoadEvents.UNAUTHORIZED_HWID, "Your whitelist is not authorised!"},
            { SynLoadEvents.UNKNOWN, "Unknown - Unknown error caused by Synapse X, not this custom UI."}
        };

        /// <summary>
        /// Friendly names for Synapse attach events.
        /// </summary>
        public static Dictionary<SynAttachEvents, string> AttachEventMap = new Dictionary<SynAttachEvents, string>
        {
            { SynAttachEvents.ALREADY_INJECTED, "Synapse is already injected." },
            { SynAttachEvents.CHECKING, "Checking..." },
            { SynAttachEvents.CHECKING_WHITELIST, "Checking whitelist..." },
            { SynAttachEvents.FAILED_TO_ATTACH, "Failed to attach." },
            { SynAttachEvents.FAILED_TO_FIND, "Failed to find roblox." },
            { SynAttachEvents.FAILED_TO_UPDATE, "Failed to update Synapse." },
            { SynAttachEvents.INJECTING, "Injecting..." },
            { SynAttachEvents.NOT_INJECTED, "Not injected!" },
            { SynAttachEvents.NOT_RUNNING_LATEST_VER_UPDATING, "Not running the latest version of Synapse." },
            { SynAttachEvents.NOT_UPDATED, "Synapse is not updated!" },
            { SynAttachEvents.READY, "Ready!" },
            { SynAttachEvents.REINJECTING, "Reinjecting..." },
            { SynAttachEvents.SCANNING, "Scanning..." },
            { SynAttachEvents.UPDATING_DLLS, "Updating DLLs..." },

            // Currently do not have a use.
            //{ SynAttachEvents.PROC_CREATION, "Process creation." },
            //{ SynAttachEvents.PROC_DELETION, "Process deletion." },
        };
    }
}
