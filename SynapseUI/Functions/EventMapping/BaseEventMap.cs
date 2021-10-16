using System.Collections.Generic;
using SynapseUI.Exceptions;

namespace SynapseUI.EventMapping
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
            { BaseException.CEF_NOT_FOUND, "CefSharp libraries not found." },
            { BaseException.GENERIC_EXCEPTION, "Generic custom Synapse UI exception." },
            { BaseException.INVALID_SYNAPSE_INSTALL, "Invalid Synapse installation." },
            { BaseException.ALREADY_RUNNING, "Instance already exists." }
        };

        /// <summary>
        /// Generic Critical errors that only get called by the custom Synapse UI.
        /// </summary>
        public static Dictionary<BaseException, string> GenericErrorEvents = new Dictionary<BaseException, string>
        {
            { BaseException.CEF_NOT_FOUND, "CefSharp libraries used for the Monaco editor are not found, this custom UI reuses these." },
            { BaseException.GENERIC_EXCEPTION, "A generic exception was thrown, expand the help information section." },
            { BaseException.INVALID_SYNAPSE_INSTALL, "The detected Synapse installation is incorrect, it does not have the required folders/files." },
            { BaseException.ALREADY_RUNNING, "An instance of this UI is already running, close the other one before starting this one." }
        };
    }
}
