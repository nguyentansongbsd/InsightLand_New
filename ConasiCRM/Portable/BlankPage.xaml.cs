using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ConasiCRM.Portable.Helper;
using FormsVideoLibrary;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Forms;

namespace ConasiCRM.Portable
{
    public partial class BlankPage : ContentPage
    {
        public ViewModel viewModel;
        public BlankPage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ViewModel();
            media.Source = MediaSource.FromUri("https://firebasestorage.googleapis.com/v0/b/gglogin-c3e8a.appspot.com/o/Screen%20-%20CNS%20-%20Figma%202021-07-20%2016-28-24.mp4?alt=media&token=4a31d437-ffe2-4a98-8ac3-e39a6ce57fd3");
            var a = media.CurrentState;
            
        }

        void media_MediaOpened(System.Object sender, System.EventArgs e)
        {
            
        }
    }
    public class CategoricalData
    {
        public object Category { get; set; }

        public double Value { get; set; }
    }
    public class ViewModel
    {
        public ObservableCollection<CategoricalData> Data { get; set; }

        public ViewModel()
        {
            this.Data = GetCategoricalData();
        }

        private static ObservableCollection<CategoricalData> GetCategoricalData()
        {
            var data = new ObservableCollection<CategoricalData>
        {
            new CategoricalData { Category = "Greenings", Value = 52 },
            new CategoricalData { Category = "Perfecto", Value = 19 },
            new CategoricalData { Category = "NearBy", Value = 50 },
            new CategoricalData { Category = "Family", Value = 23 },
            new CategoricalData { Category = "Fresh", Value = 56 },
            new CategoricalData { Category = "Fresh", Value = 56 },
            new CategoricalData { Category = "Fresh", Value = 20 },
            new CategoricalData { Category = "Fresh", Value = 50 },
        };
            return data;
        }
    }
}
