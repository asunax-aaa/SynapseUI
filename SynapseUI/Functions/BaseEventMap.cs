using System.Collections.Generic;
using SynapseUI.Exceptions;

namespace SynapseUI.Functions.EventMapNames
{
    /// <summary>
    /// Generic base errors that do not involve CefSharp or sxlib.
    /// </summary>
   public static class BaseEventMap
   {
        /// <summary>
        /// Friendly names for custom Synapse UI exceptions.
        /// </summary>
        public static Dictionary<BaseException, string> GenericErrorMap = new Dictionary<BaseException, string>
        {
            { BaseException.CefSharpLibraryNotFound, "CefSharp libraries not found." },
            { BaseException.GenericSynapseException, "Generic custom Synapse UI exception." },
            { BaseException.InvalidSynapseInstall, "Invalid Synapse installation." }
        };

        /// <summary>
        /// Generic Critical errors that only get called by the custom Synapse UI.
        /// </summary>
        public static Dictionary<BaseException, string> GenericErrorEvents = new Dictionary<BaseException, string>
        {
            { BaseException.CefSharpLibraryNotFound, "CefSharp libraries used for the Monaco editor are not found, this custom UI reuses these." },
            { BaseException.GenericSynapseException, "A generic custom Synapse UI exception was thrown, no information provided." },
            { BaseException.InvalidSynapseInstall, "The detected Synapse installation is incorrect, it does not have the required folders/files." }
        };
    }
}
