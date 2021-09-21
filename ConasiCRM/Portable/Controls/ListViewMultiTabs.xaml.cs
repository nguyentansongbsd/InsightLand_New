using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewMultiTabs : Grid
    {
        private ListViewMultiTabsViewModel viewModel;
        public Action<OptionSet> ItemTapped { get; set; }
        public ListViewMultiTabs(string fetch, string entity)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ListViewMultiTabsViewModel(fetch,entity);
            this.LoadData();
        }

        private async void LoadData()
        {
            LoadingHelper.Show();
            await viewModel.LoadData();
            LoadingHelper.Hide();
        }

        public void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (searchBar.IsFocused)
            {
                searchBar.Unfocus();
            }
            if(e.Item != null)
            {
                ItemTapped?.Invoke(e.Item as OptionSet);
            }    
        }

        private void SearchBar_SearchButtonPressed(System.Object sender, System.EventArgs e)
        {
           
        }

        private void SearchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var text = e.NewTextValue;
            if (string.IsNullOrWhiteSpace(text))
            {
                listView.ItemsSource = viewModel.Data;
            }
            else
            {
                listView.ItemsSource = viewModel.Data.Where(x => x.Label == text);
            }
        }
    }
}