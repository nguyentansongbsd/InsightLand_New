using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ConasiCRM.Portable.Helper;
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
