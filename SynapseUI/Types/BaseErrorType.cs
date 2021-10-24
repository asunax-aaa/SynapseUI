using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Documents;
using SynapseUI.Exceptions;
using SynapseUI.Functions.InfoParser;
using static SynapseUI.EventMapping.BaseEventMap;

namespace SynapseUI.Types
{
    /// <summary>
    /// A base error class that does not use any CefSharp or sxlib library imports.
    /// </summary>
    public class BaseError : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string errorWelcome;
        public string ErrorWelcome
        {
            get => errorWelcome;
            set
            {
                errorWelcome = value;
                OnPropertyChanged("ErrorWelcome");
            }
        }

        private string errorType;
        public string ErrorType
        {
            get => errorType;
            set
            {
                errorType = value;
                OnPropertyChanged("ErrorType");
            }
        }

        private string errorInformation;
        public string ErrorInformation
        {
            get => errorInformation;
            set
            {
                errorInformation = value;
                OnPropertyChanged("ErrorInformation");
            }
        }

        private string errorName;
        public string ErrorName
        {
            get => errorName;
            set
            {
                errorName = value;
                OnPropertyChanged("ErrorName");
            }
        }

        public BaseError() { }

        public BaseError(BaseException error)
        {
            ErrorWelcome = "An error occured while trying to start the custom Synapse UI.";
            ErrorType = GenericErrorMap[error];
            ErrorInformation = GenericErrorEvents[error];
            ErrorName = error.ToString();
        }

        public void CopyFrom(BaseError error)
        {
            ErrorWelcome = error.ErrorWelcome;
            ErrorType = error.ErrorType;
            ErrorInformation = error.ErrorInformation;
            ErrorName = error.ErrorName;
        }

        public void Parse(TextBlock block)
        {
            if (string.IsNullOrWhiteSpace(ErrorName))
                return;

            var infoParser = new HelpInfoParser();
            infoParser.Parse(block, ErrorName);
        }

        public void SetHelpInformation(TextBlock block, string text)
        {
            block.Inlines.Add(new Run { Text = text });
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
