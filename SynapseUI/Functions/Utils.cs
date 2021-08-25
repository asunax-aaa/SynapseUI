using System.Windows;
using System.Windows.Media;
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
                Filter = "Lua script (*.lua, *.txt)|*lua;*.txt|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
                Title = "Open Script File"
            };

            return dialog;
        }

        public static SaveFileDialog SaveFileDialog()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Lua script (*.lua, *.txt)|*lua;*.txt|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true,
                Title = "Save Script File"
            };

            return dialog;
        }
    }
}
