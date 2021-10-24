using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Microsoft.Win32;

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
}
