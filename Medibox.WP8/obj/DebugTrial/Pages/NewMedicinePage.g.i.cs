﻿#pragma checksum "H:\PROJECTS\Windows Phone\Medibox\Sources\Medibox\Medibox.WP8.Free\Pages\NewMedicinePage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A154BBF14E193160975857EFB597D1EF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34011
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Medibox.Pages {
    
    
    public partial class NewPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock txtHelpMedicinName;
        
        internal Microsoft.Phone.Controls.PhoneTextBox boxMedicinName;
        
        internal System.Windows.Controls.TextBlock txtHelpDose;
        
        internal Microsoft.Phone.Controls.PhoneTextBox boxDose;
        
        internal System.Windows.Controls.TextBlock txtHelpNumberOfDosePerDay;
        
        internal Microsoft.Phone.Controls.ListPicker boxNumberOfDosePerDay;
        
        internal System.Windows.Controls.TextBlock txtHelpStartDate;
        
        internal Microsoft.Phone.Controls.DatePicker boxStartDate;
        
        internal System.Windows.Controls.TextBlock txtHelpStartTime;
        
        internal Microsoft.Phone.Controls.TimePicker boxStartTime;
        
        internal System.Windows.Controls.TextBlock txtStopDate;
        
        internal Microsoft.Phone.Controls.DatePicker boxStopDate;
        
        internal System.Windows.Controls.TextBlock txtHelpReminder;
        
        internal Microsoft.Phone.Controls.ToggleSwitch btnReminder;
        
        internal System.Windows.Controls.TextBlock txtHelpPriority;
        
        internal Microsoft.Phone.Controls.ToggleSwitch btnPriority;
        
        internal System.Windows.Controls.TextBlock txtHelpNote;
        
        internal Microsoft.Phone.Controls.PhoneTextBox boxNote;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Medibox;component/Pages/NewMedicinePage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txtHelpMedicinName = ((System.Windows.Controls.TextBlock)(this.FindName("txtHelpMedicinName")));
            this.boxMedicinName = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("boxMedicinName")));
            this.txtHelpDose = ((System.Windows.Controls.TextBlock)(this.FindName("txtHelpDose")));
            this.boxDose = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("boxDose")));
            this.txtHelpNumberOfDosePerDay = ((System.Windows.Controls.TextBlock)(this.FindName("txtHelpNumberOfDosePerDay")));
            this.boxNumberOfDosePerDay = ((Microsoft.Phone.Controls.ListPicker)(this.FindName("boxNumberOfDosePerDay")));
            this.txtHelpStartDate = ((System.Windows.Controls.TextBlock)(this.FindName("txtHelpStartDate")));
            this.boxStartDate = ((Microsoft.Phone.Controls.DatePicker)(this.FindName("boxStartDate")));
            this.txtHelpStartTime = ((System.Windows.Controls.TextBlock)(this.FindName("txtHelpStartTime")));
            this.boxStartTime = ((Microsoft.Phone.Controls.TimePicker)(this.FindName("boxStartTime")));
            this.txtStopDate = ((System.Windows.Controls.TextBlock)(this.FindName("txtStopDate")));
            this.boxStopDate = ((Microsoft.Phone.Controls.DatePicker)(this.FindName("boxStopDate")));
            this.txtHelpReminder = ((System.Windows.Controls.TextBlock)(this.FindName("txtHelpReminder")));
            this.btnReminder = ((Microsoft.Phone.Controls.ToggleSwitch)(this.FindName("btnReminder")));
            this.txtHelpPriority = ((System.Windows.Controls.TextBlock)(this.FindName("txtHelpPriority")));
            this.btnPriority = ((Microsoft.Phone.Controls.ToggleSwitch)(this.FindName("btnPriority")));
            this.txtHelpNote = ((System.Windows.Controls.TextBlock)(this.FindName("txtHelpNote")));
            this.boxNote = ((Microsoft.Phone.Controls.PhoneTextBox)(this.FindName("boxNote")));
        }
    }
}
