using ConasiCRM.Portable.Controls;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PhoneCallForm : ContentPage
	{
        public Action<bool> OnCompleted;
        public PhoneCallViewModel viewModel;

        public PhoneCallForm()
        {
            InitializeComponent();            
            Init();
            Create();
        }

        private void Init()
        {
            LoadingHelper.Show();
            BindingContext = viewModel = new PhoneCallViewModel();         
            SetPreOpen();
        }

        private void Create()
        {
            viewModel.Title = "Tạo mới cuộc gọi";
        }

        public void SetPreOpen()
        {
            Lookup_CallFrom.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadAllLookUp();
                LoadingHelper.Hide();
            };

            Lookup_CallTo.PreShow = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadAllLookUp();
                LoadingHelper.Hide();
            };

            Lookup_Customer.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadAllLookUp();
                LoadingHelper.Hide();
            };
        }

        private void CreatePhoneCall_Clicked(object sender, EventArgs e)
        {

        }

        private void DateEnd_DateSelected(object sender, DateChangedEventArgs e)
        {

        }

        private void DateStart_DateSelected(object sender, DateChangedEventArgs e)
        {

        }

        private void TimeStart_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void TimeEnd_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }
    }
}