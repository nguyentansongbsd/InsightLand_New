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

        public async void SetPreOpen()
        {
            Lookup_CallFrom.PreOpen = async () =>
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

            if (viewModel.AccountsLookUp.Count <= 0)
            {
                LoadingHelper.Show();
                await viewModel.LoadAccountsLookUp();
                LoadingHelper.Hide();
            }
            if (viewModel.ContactsLookUp.Count <= 0)
            {
                LoadingHelper.Show();
                await viewModel.LoadContactsLookUp();
                LoadingHelper.Hide();
            }
            if (viewModel.LeadsLookUp.Count <= 0)
            {
                LoadingHelper.Show();
                await viewModel.LoadLeadsLookUp();
                LoadingHelper.Hide();
            }
            LookUpAccount.SetList(viewModel.AccountsLookUp, "Label");
            LookUpContact.SetList(viewModel.ContactsLookUp, "Label");
            LookUpLead.SetList(viewModel.LeadsLookUp, "Label");
            LookUpAccount.lookUpListView.ItemTapped += LookUpAccount_ItemTapped;
            LookUpContact.lookUpListView.ItemTapped += LookUpContac_ItemTapped;
            LookUpLead.lookUpListView.ItemTapped += LookUpLead_ItemTapped;
        }

        private async void LookUpAccount_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var CodeAccount = "3";
            if (e != null)
            {
                var item = e.Item as Models.OptionSet;
                if (item != null)
                {
                    if (centerModalKH.Title == "Chọn người thực hiện cuộc gọi")
                    {
                        viewModel.CallFrom = new Models.LookUp();
                        viewModel.CallFrom.Id = Guid.Parse(item.Val);
                        viewModel.CallFrom.Name = item.Label;
                        viewModel.CallFrom.Detail = CodeAccount;

                        await centerModalKH.Hide();
                    }
                    if (centerModalKH.Title == "Chọn người liên quan")
                    {
                        viewModel.Customer = new Models.LookUp();
                        viewModel.Customer.Id = Guid.Parse(item.Val);
                        viewModel.Customer.Name = item.Label;
                        viewModel.Customer.Detail = CodeAccount;

                        await centerModalKH.Hide();
                    }
                }
            }
        }

        private async void LookUpContac_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var CodeContac = "2";
            if (e != null)
            {
                var item = e.Item as Models.OptionSet;
                if (item != null)
                {
                    if (centerModalKH.Title == "Chọn người thực hiện cuộc gọi")
                    {
                        viewModel.CallFrom = new Models.LookUp();
                        viewModel.CallFrom.Id = Guid.Parse(item.Val);
                        viewModel.CallFrom.Name = item.Label;
                        viewModel.CallFrom.Detail = CodeContac;

                        await centerModalKH.Hide();
                    }
                    if (centerModalKH.Title == "Chọn người liên quan")
                    {
                        viewModel.Customer = new Models.LookUp();
                        viewModel.Customer.Id = Guid.Parse(item.Val);
                        viewModel.Customer.Name = item.Label;
                        viewModel.Customer.Detail = CodeContac;

                        await centerModalKH.Hide();
                    }
                }
            }
        }

        private async void LookUpLead_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var CodeLead = "1";
            if (e != null)
            {
                var item = e.Item as Models.OptionSet;
                if (item != null)
                {
                    if (centerModalKH.Title == "Chọn người thực hiện cuộc gọi")
                    {
                        viewModel.CallFrom = new Models.LookUp();
                        viewModel.CallFrom.Id = Guid.Parse(item.Val);
                        viewModel.CallFrom.Name = item.Label;
                        viewModel.CallFrom.Detail = CodeLead;

                        await centerModalKH.Hide();
                    }
                    if (centerModalKH.Title == "Chọn người liên quan")
                    {
                        viewModel.Customer = new Models.LookUp();
                        viewModel.Customer.Id = Guid.Parse(item.Val);
                        viewModel.Customer.Name = item.Label;
                        viewModel.Customer.Detail = CodeLead;

                        await centerModalKH.Hide();
                    }
                }
            }
        }       

        private async void ChonNguoiLienQuan_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            Tab_Tapped(1);
            centerModalKH.Title = "Chọn người liên quan";
            await centerModalKH.Show();
            LoadingHelper.Hide();
        }

        private void Lead_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(1);
        }

        private void Contact_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(2);
        }

        private void Account_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(3);
        }

        private void Tab_Tapped(int tab)
        {
            if (tab == 1)
            {
                VisualStateManager.GoToState(radBorderLead, "Selected");
                VisualStateManager.GoToState(lbLead, "Selected");
                LookUpLead.IsVisible = true;
            }
            else
            {
                VisualStateManager.GoToState(radBorderLead, "Normal");
                VisualStateManager.GoToState(lbLead, "Normal");
                LookUpLead.IsVisible = false;
            }
            if (tab == 2)
            {
                VisualStateManager.GoToState(radBorderContact, "Selected");
                VisualStateManager.GoToState(lbContact, "Selected");
                LookUpContact.IsVisible = true;
            }
            else
            {
                VisualStateManager.GoToState(radBorderContact, "Normal");
                VisualStateManager.GoToState(lbContact, "Normal");
                LookUpContact.IsVisible = false;
            }
            if (tab == 3)
            {
                VisualStateManager.GoToState(radBorderAccount, "Selected");
                VisualStateManager.GoToState(lbAccount, "Selected");
                LookUpAccount.IsVisible = true;
            }
            else
            {
                VisualStateManager.GoToState(radBorderAccount, "Normal");
                VisualStateManager.GoToState(lbAccount, "Normal");
                LookUpAccount.IsVisible = false;
            }
        }

        private void CreatePhoneCall_Clicked(object sender, EventArgs e)
        {

        }
    }
}