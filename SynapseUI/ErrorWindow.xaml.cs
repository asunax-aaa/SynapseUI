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
        public BaseError Error { set; get; } = new BaseError();

        public ErrorWindow(BaseError error)
        {
            InitializeComponent();

            dropDownButton.Window = this;
            dropDownButton.TargetHeight = 410d;

            Error.Copy(error);
            error.Parse(informationBox);
        }

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
            Environment.Exit(0); // closes both the error and splash window, unlike this.Close()
        }
    }
}
