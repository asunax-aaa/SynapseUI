using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using sxlib;
using sxlib.Internal;
using sxlib.Specialized;
using SynapseUI.Functions.EventMapNames;

namespace SynapseUI
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        public SxLibWPF SxUI;

        public SplashScreen()
        {
            InitializeComponent();
            Loaded += delegate { Initialize(); };
        }

        private void Initialize()
        {
            if (App.DEBUG)
            {
                OpenMainWindow(null);
                return;
            }

            string dir = Directory.GetCurrentDirectory();
            SxUI = SxLib.InitializeWPF(this, dir);

            RegisterEvents();

            SxUI.Load(); // handle errors later
        }

        private void RegisterEvents()
        {
            SxUI.LoadEvent += LoadEventTriggered;
        }

        private async void LoadEventTriggered(SxLibBase.SynLoadEvents Event, object Param)
        {
            if (EventMap.LoadEventMap.TryGetValue(Event, out string text))
            {
                if (EventMap.LoadErrorEvents.ContainsKey(Event))
                    ThrowLoadError(Event); // critical errors should halt, no need to return.

                statusLabel.Content = text;

                if (Event == SxLibBase.SynLoadEvents.READY)
                {
                    loadingBar.AnimateFinish();

                    await Task.Delay(500);
                    OpenMainWindow(SxUI);
                }
                else
                    loadingBar.AnimateProgress(loadingBar.Progress + 20);
            }
            else
            {
                throw new KeyNotFoundException($"Event '{Event}' does not exist in the LoadEvent mapping.");
            }
        }

        private void OpenMainWindow(SxLibWPF lib)
        {
            var window = new ExecuteWindow(lib);
            window.Show();

            this.Close();
        }

        private void ThrowLoadError(SxLibBase.SynLoadEvents error)
        {
            ErrorWindow errorWindow = new ErrorWindow(new Types.Error(error));
            errorWindow.Show();
        }

        private void DraggableTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
