// Updated by XamlIntelliSenseFileGenerator 4/2/2022 6:06:33 PM
#pragma checksum "..\..\..\ErrorWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "2E575FE50C61ECD560C643095117B42242F2CD04BB8F81BCAAF1DBA6BB722D21"
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
    /// ErrorWindow
    /// </summary>
    public partial class ErrorWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
    {

#line default
#line hidden


#line 50 "..\..\..\ErrorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid draggableTop;

#line default
#line hidden


#line 64 "..\..\..\ErrorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button closeWindow;

#line default
#line hidden


#line 121 "..\..\..\ErrorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal SynapseUI.Controls.DropDownButton dropDownButton;

#line default
#line hidden


#line 136 "..\..\..\ErrorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button closeWindowButton;

#line default
#line hidden


#line 152 "..\..\..\ErrorWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock informationBox;

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
            System.Uri resourceLocater = new System.Uri("/SynapseUI;component/errorwindow.xaml", System.UriKind.Relative);

#line 1 "..\..\..\ErrorWindow.xaml"
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
                    this.ErrWindow = ((SynapseUI.ErrorWindow)(target));
                    return;
                case 2:
                    this.draggableTop = ((System.Windows.Controls.Grid)(target));

#line 53 "..\..\..\ErrorWindow.xaml"
                    this.draggableTop.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.DraggableTop_MouseLeftButtonDown);

#line default
#line hidden
                    return;
                case 3:
                    this.closeWindow = ((System.Windows.Controls.Button)(target));

#line 66 "..\..\..\ErrorWindow.xaml"
                    this.closeWindow.Click += new System.Windows.RoutedEventHandler(this.CloseWindowButton_Click);

#line default
#line hidden
                    return;
                case 4:
                    this.dropDownButton = ((SynapseUI.Controls.DropDownButton)(target));
                    return;
                case 5:
                    this.closeWindowButton = ((System.Windows.Controls.Button)(target));

#line 141 "..\..\..\ErrorWindow.xaml"
                    this.closeWindowButton.Click += new System.Windows.RoutedEventHandler(this.CloseWindowButton_Click);

#line default
#line hidden
                    return;
                case 6:
                    this.informationBox = ((System.Windows.Controls.TextBlock)(target));
                    return;
            }
            this._contentLoaded = true;
        }

        internal System.Windows.Window ErrWindow;
    }
}

