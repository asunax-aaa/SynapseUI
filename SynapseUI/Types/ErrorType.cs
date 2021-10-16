using static sxlib.Specialized.SxLibBase;
using static SynapseUI.EventMapping.EventMap;

namespace SynapseUI.Types
{
    /// <summary>
    /// An extended version of the GenericError that covers Sxlib errors.
    /// Sxlib doesn't implement a shared interface for errors, they are Enums afterall.
    /// </summary>
    public class Error : BaseError
    {
        public Error() { }

        public Error(SynLoadEvents error)
        {
            ErrorWelcome = "An error occured while trying to start Synapse X.";
            ErrorType = LoadEventMap[error];
            ErrorInformation = LoadErrorEvents[error];
            ErrorName = error.ToString();
        }

        public Error(SynAttachEvents error)
        {
            ErrorWelcome = "An error occured while trying to inject Synapse X.";
            ErrorType = AttachEventMap[error];
            ErrorInformation = AttachEventMap[error];
            ErrorName = error.ToString();
        }
    }
}
