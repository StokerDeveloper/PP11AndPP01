﻿#pragma checksum "..\..\..\..\Windows\ОкноВхода.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "DF43507AF65FFCF78A38E9522F4392DA0F734E8C"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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
using Точка_проката_ЦПКиО_им._Маяковского;


namespace Точка_проката_ЦПКиО_им._Маяковского {
    
    
    /// <summary>
    /// ОкноВхода
    /// </summary>
    public partial class ОкноВхода : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\..\Windows\ОкноВхода.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox логин;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\Windows\ОкноВхода.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox пароль;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\Windows\ОкноВхода.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock показПароля;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\Windows\ОкноВхода.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button войти;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.6.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Точка проката ЦПКиО им. Маяковского;V1.0.0.0;component/windows/%d0%9e%d0%ba%d0%b" +
                    "d%d0%be%d0%92%d1%85%d0%be%d0%b4%d0%b0.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\ОкноВхода.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "6.0.6.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\..\Windows\ОкноВхода.xaml"
            ((Точка_проката_ЦПКиО_им._Маяковского.ОкноВхода)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 8 "..\..\..\..\Windows\ОкноВхода.xaml"
            ((Точка_проката_ЦПКиО_им._Маяковского.ОкноВхода)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 20 "..\..\..\..\Windows\ОкноВхода.xaml"
            ((System.Windows.Controls.Label)(target)).MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.Label_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.логин = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.пароль = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 5:
            this.показПароля = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            
            #line 27 "..\..\..\..\Windows\ОкноВхода.xaml"
            ((System.Windows.Controls.Button)(target)).PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.ShowPassword);
            
            #line default
            #line hidden
            
            #line 27 "..\..\..\..\Windows\ОкноВхода.xaml"
            ((System.Windows.Controls.Button)(target)).PreviewMouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.CollapsePassword);
            
            #line default
            #line hidden
            return;
            case 7:
            this.войти = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\..\..\Windows\ОкноВхода.xaml"
            this.войти.Click += new System.Windows.RoutedEventHandler(this.Login_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

