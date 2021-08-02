using ConasiCRM.Portable.Settings;
﻿using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.ViewModels;
using ConasiCRM.Portable.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {     
        private AppShellViewModel viewModel;     
        public AppShell()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new AppShellViewModel();
            appShell.CurrentItem = BanHang;
        }        
    }       
}