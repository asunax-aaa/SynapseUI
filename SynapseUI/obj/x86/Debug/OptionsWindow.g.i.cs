// Updated by XamlIntelliSenseFileGenerator 4/2/2022 5:56:41 PM
#pragma checksum "..\..\..\OptionsWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "A438395E58C4B3BC1F4B26574E2A9D890B259348EEA553FA09B059D3B47E5307"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SynapseUI;
using SynapseUI.Controls;
using SynapseUI.Controls.AceEditor;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace SynapseUI
{


    /// <summary>
    /// OptionsWindow
    /// </summary>
    public partial class OptionsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector
    {

#line default
#line hidden


#line 75 "..\..\..\OptionsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid draggableTop;

#line default
#line hidden


#line 87 "..\..\..\OptionsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button closeWindow;

#line default
#line hidden


#line 95 "..\..\..\OptionsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabControl optionsTabs;

#line default
#line hidden


#line 105 "..\..\..\OptionsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ItemsControl OptionsPresenter;

#line default
#line hidden


#line 133 "..\..\..\OptionsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox aceThemesComboBox;

#line default
#line hidden


#line 147 "..\..\..\OptionsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ItemsControl ScriptHubPresenter;

#line default
#line hidden


#line 248 "..\..\..\OptionsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SynapseUI.Controls.SliderToggle mutexToggle;

#line default
#line hidden


#line 274 "..\..\..\OptionsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SynapseUI.Controls.SliderToggle roundedCornerToggle;

#line default
#line hidden


#line 282 "..\..\..\OptionsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button killRobloxButton;

#line default
#line hidden


#line 291 "..\..\..\OptionsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button killSynapseButton;

#line default
#line hidden

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SynapseUI;component/optionswindow.xaml", System.UriKind.Relative);

#line 1 "..\..\..\OptionsWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);

#line default
#line hidden
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler)
        {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.executeWindow = ((SynapseUI.OptionsWindow)(target));

#line 15 "..\..\..\OptionsWindow.xaml"
                    this.executeWindow.Loaded += new System.Windows.RoutedEventHandler(this.OptionsWindow_Loaded);

#line default
#line hidden
                    return;
                case 2:
                    this.draggableTop = ((System.Windows.Controls.Grid)(target));

#line 78 "..\..\..\OptionsWindow.xaml"
                    this.draggableTop.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.DraggableTop_MouseLeftButtonDown);

#line default
#line hidden
                    return;
                case 3:
                    this.closeWindow = ((System.Windows.Controls.Button)(target));

#line 89 "..\..\..\OptionsWindow.xaml"
                    this.closeWindow.Click += new System.Windows.RoutedEventHandler(this.CloseWindow_Click);

#line default
#line hidden
                    return;
                case 4:
                    this.optionsTabs = ((System.Windows.Controls.TabControl)(target));
                    return;
                case 5:
                    this.OptionsPresenter = ((System.Windows.Controls.ItemsControl)(target));
                    return;
                case 6:
                    this.aceThemesComboBox = ((System.Windows.Controls.ComboBox)(target));

#line 139 "..\..\..\OptionsWindow.xaml"
                    this.aceThemesComboBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.AceThemesComboBox_SelectionChanged);

#line default
#line hidden
                    return;
                case 7:
                    this.ScriptHubPresenter = ((System.Windows.Controls.ItemsControl)(target));
                    return;
                case 9:
                    this.mutexToggle = ((SynapseUI.Controls.SliderToggle)(target));
                    return;
                case 10:
                    this.roundedCornerToggle = ((SynapseUI.Controls.SliderToggle)(target));
                    return;
                case 11:
                    this.killRobloxButton = ((System.Windows.Controls.Button)(target));

#line 286 "..\..\..\OptionsWindow.xaml"
                    this.killRobloxButton.Click += new System.Windows.RoutedEventHandler(this.KillRobloxButton_Click);

#line default
#line hidden
                    return;
                case 12:
                    this.killSynapseButton = ((System.Windows.Controls.Button)(target));

#line 295 "..\..\..\OptionsWindow.xaml"
                    this.killSynapseButton.Click += new System.Windows.RoutedEventHandler(this.KillSynapseButton_Click);

#line default
#line hidden
                    return;
            }
            this._contentLoaded = true;
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 8:

#line 171 "..\..\..\OptionsWindow.xaml"
                    ((System.Windows.Controls.Image)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Execute_ScriptButton);

#line default
#line hidden
                    break;
            }
        }

        internal System.Windows.Window executeWindow;
        internal System.Windows.Controls.Button reinstallRobloxButton;
    }
}

