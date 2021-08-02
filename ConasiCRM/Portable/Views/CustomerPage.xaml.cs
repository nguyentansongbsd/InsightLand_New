using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConasiCRM.Portable.Helper;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Views
{
    public partial class CustomerPage : ContentPage
    {
        private LeadsContentView LeadsContentView;
        private ContactsContentview ContactsContentview;
        private AccountsContentView AccountsContentView;
        private Label DataNull = new Label() { Text = "Không có dữ liệu", FontSize = 18, TextColor = Color.Gray, HorizontalTextAlignment = TextAlignment.Center, Margin = new Thickness(0, 30, 0, 0) };
        public CustomerPage()
        {
            LoadingHelper.Show();
            InitializeComponent();
            Init();
        }
        public async void Init()
        {
            VisualStateManager.GoToState(radBorderLead, "Active");
            VisualStateManager.GoToState(radBorderAccount, "InActive");
            VisualStateManager.GoToState(radBorderContact, "InActive");
            VisualStateManager.GoToState(lblLead, "Active");
            VisualStateManager.GoToState(lblAccount, "InActive");
            VisualStateManager.GoToState(lblContact, "InActive");
            if (LeadsContentView == null)
            {
                LeadsContentView = new LeadsContentView();
            }
            LeadsContentView.OnCompleted = async (IsSuccess) =>
            {
                if (IsSuccess)
                {
                    CustomerContentView.Children.Add(LeadsContentView);
                    LoadingHelper.Hide();
                }
                else
                {
                    CustomerContentView.Children.Add(DataNull);
                    LoadingHelper.Hide();
                }
            };
        }

        private void Lead_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radBorderLead, "Active");
            VisualStateManager.GoToState(radBorderAccount, "InActive");
            VisualStateManager.GoToState(radBorderContact, "InActive");
            VisualStateManager.GoToState(lblLead, "Active");
            VisualStateManager.GoToState(lblAccount, "InActive");
            VisualStateManager.GoToState(lblContact, "InActive");
            LeadsContentView.IsVisible = true;
            if (AccountsContentView != null)
            {
                AccountsContentView.IsVisible = false;
            }
            if (ContactsContentview != null)
            {
                ContactsContentview.IsVisible = false;
            }
        }

        private void Account_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radBorderLead, "InActive");
            VisualStateManager.GoToState(radBorderAccount, "Active");
            VisualStateManager.GoToState(radBorderContact, "InActive");
            VisualStateManager.GoToState(lblLead, "InActive");
            VisualStateManager.GoToState(lblAccount, "Active");
            VisualStateManager.GoToState(lblContact, "InActive");
            if (AccountsContentView == null)
            {
                LoadingHelper.Show();
                AccountsContentView = new AccountsContentView();
            }
            AccountsContentView.OnCompleted = (IsSuccess) =>
            {
                if (IsSuccess)
                {
                    CustomerContentView.Children.Add(AccountsContentView);
                    LoadingHelper.Hide();
                }
                else
                {
                    CustomerContentView.Children.Add(DataNull);
                    LoadingHelper.Hide();
                }
            };
            LeadsContentView.IsVisible = false;
            AccountsContentView.IsVisible = true;
            if (ContactsContentview != null)
            {
                ContactsContentview.IsVisible = false;
            }
        }

        private void Contact_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radBorderLead, "InActive");
            VisualStateManager.GoToState(radBorderAccount, "InActive");
            VisualStateManager.GoToState(radBorderContact, "Active");
            VisualStateManager.GoToState(lblLead, "InActive");
            VisualStateManager.GoToState(lblAccount, "InActive");
            VisualStateManager.GoToState(lblContact, "Active");
            if (ContactsContentview == null)
            {
                LoadingHelper.Show();
                ContactsContentview = new ContactsContentview();
            }
            ContactsContentview.OnCompleted = (IsSuccess) =>
            {
                if (IsSuccess)
                {
                    CustomerContentView.Children.Add(ContactsContentview); ;
                    LoadingHelper.Hide();
                }
                else
                {
                    CustomerContentView.Children.Add(DataNull);
                    LoadingHelper.Hide();
                }
            };
            LeadsContentView.IsVisible = false;
            ContactsContentview.IsVisible = true;
            if (AccountsContentView != null)
            {
                AccountsContentView.IsVisible = false;
            }
        }

        private async void NewCustomer_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            string[] options = new string[] { "Khách hàng tiềm năng", "Khách hàng cá nhân", "Khách hàng doanh nghiệp" };
            string asw = await DisplayActionSheet("Tuỳ chọn", "Huỹ", null, options);
            if (asw == "Khách hàng tiềm năng")
            {
                await Navigation.PushAsync(new LeadForm());
            }
            else if (asw == "Khách hàng cá nhân")
            {
                await Navigation.PushAsync(new ContactForm());
            }
            else if (asw == "Khách hàng doanh nghiệp")
            {
                await Navigation.PushAsync(new AccountForm());
            }

            LoadingHelper.Hide();
        }
    }
}
