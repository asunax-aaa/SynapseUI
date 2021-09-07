using System.Media;
using System.Windows;
using System.Windows.Input;

namespace SynapseUI
{
    /// <summary>
    /// Interaction logic for ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        private bool _result;

        public ConfirmationWindow(string message)
        {
            InitializeComponent();

            messageText.Text = message;
        }

        // Button Events //
        private void YesConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            _result = true;
            this.Close();
        }

        private void NoConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Window Events //
        public new bool ShowDialog()
        {
            base.ShowDialog();
            return _result;
        }

        public override void BeginInit()
        {
            SystemSounds.Exclamation.Play();
            base.BeginInit();
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DraggableTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
