using System;
using System.ComponentModel;
using System.Windows.Controls;
using SynapseUI.Exceptions;
using SynapseUI.Functions.InfoParser;
using static SynapseUI.Functions.EventMapNames.BaseEventMap;

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

        /// <summary>
        /// Retrieves the stored help information for errors, error help information is provided witnin the HelpInfo.xml document. 
        /// </summary>
        /// <param name="block">The TextBlock to append the help information to.</param>
        /// <exception cref="System.ArgumentNullException">The stored error name is not set.</exception>
        public void Parse(TextBlock block)
        {
            if (string.IsNullOrWhiteSpace(ErrorName))
                throw new ArgumentNullException("Error has not been initialised.");

            var infoParser = new HelpInfoParser();
            infoParser.Parse(block, ErrorName);
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
