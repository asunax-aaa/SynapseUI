using System;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Net;
using Microsoft.Win32;
using System.IO;

namespace SynapseUI.Functions.Utils
{
    public static class HexColorConverter
    {
        public static BrushConverter Converter = new BrushConverter();

        public static Brush Convert(string hexColor)
        {
            return (Brush)Converter.ConvertFrom(hexColor);
        }
    }

    public static class VisualHelper
    {
        public static T GetVisualChild<T>(DependencyObject parent) where T : Visual
        {
            T child = default(T);

            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }

    public static class Dialog
    {
        public static OpenFileDialog OpenFileDialog()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Lua scripts (*.lua, *.txt)|*lua;*.txt|All files (*.*)|*.*",
                RestoreDirectory = true,
                InitialDirectory = App.CURRENT_DIR + @"\scripts\",
                Title = "Open Script File"
            };

            return dialog;
        }

        public static SaveFileDialog SaveFileDialog()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Lua scripts (*.lua)|*lua|Text files|*txt|All files (*.*)|*.*",
                RestoreDirectory = true,
                InitialDirectory = App.CURRENT_DIR + @"\scripts\",
                DefaultExt = "lua",
                Title = "Save Script File"
            };

            return dialog;
        }
    }

    public static class Animation
    {
        public static IEasingFunction QuarticEase = new QuarticEase() { EasingMode = EasingMode.EaseOut };
    }

    public static class ErrorGen
    {
        public static string ErrorToMessage(System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var winVer = Environment.OSVersion.Version;
            string uiVer = Web.VersionChecker.GetCurrentVersion();

            var builder = new StringBuilder("Screenshot this and send it to asunax#5833. \n\n")
                .Append($"Exception: {e.Exception.GetType()}\n")
                .Append($"Message: {e.Exception.Message}\n")
                .Append($"UI Version: {uiVer}\n")
                .Append($"Windows: {winVer.Major}.{winVer.Minor}\n\n");

            switch (e.Exception)
            {
                case WebException _:
                    builder.Append("*Make sure you have the latest version of this UI running, https://github.com/asunax-aaa/SynapseUI. \nTry using a VPN.");
                    break;

                case FileNotFoundException exception:
                    builder.Append($"*Was not able to find the file '{exception.FileName}'.");
                    break;

                case System.ComponentModel.Win32Exception _:
                    builder.Append($"*Make sure you have the latest version of this UI running, https://github.com/asunax-aaa/SynapseUI.");
                    break;

            }

            return builder.ToString();
        }
    }
}