using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Common;
using Telerik.XamarinForms.DataGrid;
using Telerik.XamarinForms.DataGrid.Commands;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HoatDongList : ContentPage
    {
        int a = 0;
        public HoatDongListViewModel viewModel;
        public HoatDongList()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new HoatDongListViewModel();
            LoadingHelper.Show();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
            LoadingHelper.Hide();
        }
        protected override void OnAppearing()
        {
            if (viewModel != null && a ==1) viewModel.RefreshCommand.Execute(null);
        }
        protected override void OnDisappearing()
        {
            a = 1;
        }

        private async void NewTaskMenu_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Navigation.PushAsync(new TaskForm());
            LoadingHelper.Hide();
        }
        private async void PhoneMenu_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Navigation.PushAsync(new PhoneCallForm());
            LoadingHelper.Hide();
        }

        private async void MeetingMenu_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Navigation.PushAsync(new MeetingForm());
            LoadingHelper.Hide();
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            LoadingHelper.Show();
            HoatDongListModel val = e.Item as HoatDongListModel;           
            if (val.activitytypecode == "task")
            {
                TaskForm newPage = new TaskForm(val.activityid);
                newPage.CheckTaskForm = async (CheckEventData) =>
                {
                    if (CheckEventData == true)
                    {
                        await Navigation.PushAsync(newPage);                      
                    }
                    LoadingHelper.Hide();
                };
            }
            else if (val.activitytypecode == "phonecall")
            {
                PhoneCallForm newPage = new PhoneCallForm(val.activityid);
                newPage.CheckPhoneCell = async (CheckEventData) =>
                {
                    if (CheckEventData == true)
                    {
                        await Navigation.PushAsync(newPage);
                    }
                    LoadingHelper.Hide();
                };
            }
            else if (val.activitytypecode == "appointment")
            {
                MeetingForm newPage = new MeetingForm(val.activityid);
                newPage.CheckMeeting = async (CheckEventData) =>
                {
                    if (CheckEventData == true)
                    {
                        await Navigation.PushAsync(newPage);
                    }
                    LoadingHelper.Hide();
                };
            }          
        }

        private async void SearchBar_SearchButtonPressed(System.Object sender, System.EventArgs e)
        {
            LoadingHelper.Show();
            await viewModel.LoadOnRefreshCommandAsync();
            LoadingHelper.Hide();
        }

        private void SearchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.Keyword))
            {
                SearchBar_SearchButtonPressed(null, EventArgs.Empty);
            }
        }
    }
}