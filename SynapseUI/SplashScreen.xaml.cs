﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using sxlib;
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
            Loaded += (o, e) => { Initialize(); };
        }

        private void Initialize()
        {
            if (App.DEBUG)
            {
                OpenMainWindow(null);
                return;
            }

            SxUI = SxLib.InitializeWPF(this, App.CURRENT_DIR);

            SxUI.LoadEvent += LoadEventTriggered;

            SxUI.Load();
        }

        // Sx Load Events //
        private async void LoadEventTriggered(SxLibBase.SynLoadEvents Event, object Param)
        {
            if (EventMap.LoadEventMap.TryGetValue(Event, out string text))
            {
                if (EventMap.LoadErrorEvents.ContainsKey(Event))
                    ThrowLoadError(Event);

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
            new ExecuteWindow(lib).Show();
            this.Close();
        }

        private void ThrowLoadError(SxLibBase.SynLoadEvents error)
        {
            new ErrorWindow(new Types.Error(error)).Show();
        }


        // Window Events //
        private void DraggableTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
