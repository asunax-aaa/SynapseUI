using System;
using System.Media;
using System.Windows;
using System.Windows.Input;
using SynapseUI.Types;

namespace SynapseUI
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        public BaseError Error { get; } = new BaseError();

        public ErrorWindow(BaseError error)
        {
            Init();

            Error.CopyFrom(error);
            Error.Parse(informationBox);
        }

        public ErrorWindow(BaseError error, string helpInformation)
        {
            Init();

            Error.CopyFrom(error);
            Error.SetHelpInformation(informationBox, helpInformation);
        }

        private void Init()
        {
            InitializeComponent();

            dropDownButton.Window = this;
            dropDownButton.TargetHeight = 430d;
        }

        // Window Events //
        public override void BeginInit()
        {
            SystemSounds.Exclamation.Play();
            base.BeginInit();
        }

        private void DraggableTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
