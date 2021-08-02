using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoOwnerForm : ContentPage
    {
        public CoOwnerFormViewModel viewModel;
        public Guid CoOwnerId;
        public CoOwnerForm(Guid coOwnerId) // form detail,chinh sua. 
        {
            InitializeComponent();
            CoOwnerId = coOwnerId;
            this.BindingContext = viewModel = new CoOwnerFormViewModel();
            viewModel.CoOwner = new CoOwnerFormModel();
            LoadData();
            viewModel.ModalLookUp = modalLookUp;
            viewModel.InitializeModal();
            viewModel.AfterLookUpClose += AfterLookUpClose;

            viewModel.IsBusy = false;
        }

        private async void LoadData()
        {
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<CoOwnerFormModel>>("bsd_coowners", @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='bsd_coowner'>
                <attribute name='bsd_coownerid' />
                <attribute name='bsd_name' />
                <attribute name='createdon' />
                <attribute name='bsd_relationship' />
                <order attribute='bsd_name' descending='false' />
                <filter type='and'>
                  <condition attribute='bsd_coownerid' operator='eq' uitype='bsd_coowner' value='" + CoOwnerId + @"' />
                </filter>
                <link-entity name='contact' from='contactid' to='bsd_customer' visible='false' link-type='outer' alias='a_6b0d05eeb214e911a97f000d3aa04914'>
                  <attribute name='contactid' alias='contact_id' />
                  <attribute name='bsd_fullname' alias='contact_name' />
                </link-entity>
                <link-entity name='account' from='accountid' to='bsd_customer' visible='false' link-type='outer' alias='a_1324f6d5b214e911a97f000d3aa04914'>
                  <attribute name='accountid' alias='account_id' />
                  <attribute name='bsd_name' alias='account_name' />
                </link-entity>
                <link-entity name='quote' from='quoteid' to='bsd_reservation' visible='false' link-type='outer' alias='a_1a89efffb214e911a97f000d3aa04914'>
                  <attribute name='quoteid' alias='reservation_id' />
                  <attribute name='name' alias='reservation_name' />
                </link-entity>
              </entity>
            </fetch>");
            var coOwner = result?.value.SingleOrDefault();
            if (coOwner == null)
            {
                await DisplayAlert("Thông báo", "Không tìm thấy người đồng sở hữu này", "Đóng");
                return;
            }

            viewModel.CoOwner = coOwner;


            if (coOwner.contact_id != Guid.Empty)
            {
                viewModel.Contact = new LookUp()
                {
                    Id = coOwner.contact_id,
                    Name = coOwner.contact_name
                };
            }
            else
            {
                viewModel.Account = new LookUp()
                {
                    Id = coOwner.contact_id,
                    Name = coOwner.contact_name
                };
            }

            viewModel.RelationShip = viewModel.RelationShipOptionList.SingleOrDefault(x => x.Val == coOwner.bsd_relationship);


        }

        public CoOwnerForm(LookUp ReservationLookUp)  // tạo mới từ form báo giá.
        {
            InitializeComponent();
            LoadingHelper.Show();
            this.BindingContext = viewModel = new CoOwnerFormViewModel();          
            viewModel.CoOwner = new CoOwnerFormModel()
            {
                reservation_id = ReservationLookUp.Id,
                reservation_name = ReservationLookUp.Name
            };

            viewModel.ModalLookUp = modalLookUp;
            viewModel.InitializeModal();
            viewModel.AfterLookUpClose += AfterLookUpClose;

            LoadingHelper.Hide();
        }

        private void BsdLookUp_OpenClicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.InitCustomerLookUpHeader();
            viewModel.BtnContact.Clicked += ContactOpen;
            viewModel.BtnAccount.Clicked += AccountOpen;
            ContactOpen(viewModel.BtnContact, EventArgs.Empty);
            LoadingHelper.Hide();

        }

        public void ContactOpen(object sender, EventArgs e)
        {
            if (viewModel.AccountLookUpConfig.ListView != null) viewModel.AccountLookUpConfig.ListView.IsVisible = false;
            viewModel.BtnContact.BackgroundColor = Color.FromHex("#999999");
            viewModel.BtnAccount.BackgroundColor = Color.Transparent;
            viewModel.CurrentLookUpConfig = viewModel.ContactLookUpConfig;
            viewModel.ProcessLookup(nameof(viewModel.ContactLookUpConfig));
        }
        public void AccountOpen(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.ContactLookUpConfig.ListView.IsVisible = false;
            viewModel.BtnContact.BackgroundColor = Color.Transparent;
            viewModel.BtnAccount.BackgroundColor = Color.FromHex("#999999");
            viewModel.CurrentLookUpConfig = viewModel.AccountLookUpConfig;
            viewModel.ProcessLookup(nameof(viewModel.AccountLookUpConfig));
            LoadingHelper.Hide();
        }

        public void OnSwitch()
        {
            if (viewModel.searchBar.Text != null && viewModel.searchBar.Text.Length > 0)
            {
                viewModel.CurrentLookUpConfig.LookUpData.Clear();
                viewModel.searchBar.Text = null;
            }
        }

        public void AfterLookUpClose(object sender, EventArgs e)
        {
            viewModel.BtnContact.Clicked -= ContactOpen;
            viewModel.BtnAccount.Clicked -= AccountOpen;
        }

        private void ClearOptionSet_Clicked(object sender, EventArgs e)
        {
            string fieldName = (string)((Button)sender).CommandParameter;
            var model = viewModel.CoOwner;
            PropertyInfo prop = model.GetType().GetProperty(fieldName);
            prop.SetValue(model, null);
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            if (viewModel.Customer == null)
            {
                await DisplayAlert("Thông báo", "Vui lòng chọn khách hàng", "Đóng");
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.CoOwner.bsd_name))
            {
                await DisplayAlert("Thông báo", "Vui lòng nhập mô tả", "Đóng");
                return;
            }
            if (viewModel.Customer == null)
            {
                await DisplayAlert("Thông báo", "Vui lòng chọn khách hàng", "Đóng");
                return;
            }
            if (viewModel.RelationShip == null)
            {
                await DisplayAlert("Thông báo", "Vui lòng chọn mối quan hệ", "Đóng");
                return;
            }


            LoadingHelper.Show();

            IDictionary<string, object> data = new Dictionary<string, object>();
            data["bsd_name"] = viewModel.CoOwner.bsd_name;
            data["bsd_relationship"] = viewModel.RelationShip.Val;
            data["bsd_reservation@odata.bind"] = $"quotes({viewModel.CoOwner.reservation_id})";

            CrmApiResponse clearLookupResponse = new CrmApiResponse()
            {
                IsSuccess = true
            };
            if (viewModel.Customer.Type == 1)
            {
                data["bsd_customer_contact@odata.bind"] = $"/contacts({viewModel.Customer.Id})";
                if (CoOwnerId != Guid.Empty) clearLookupResponse = await CrmHelper.SetNullLookupField("quotes", viewModel.CoOwner.reservation_id, "customerid_account");
            }
            else
            {
                data["bsd_customer_account@odata.bind"] = $"/accounts({viewModel.Customer.Id})";
                if (CoOwnerId != Guid.Empty) clearLookupResponse = await CrmHelper.SetNullLookupField("quotes", viewModel.CoOwner.reservation_id, "customerid_contact");
            }

            if (clearLookupResponse.IsSuccess)
            {
                try
                {
                    var insertResponse = new CrmApiResponse();
                    if (CoOwnerId == Guid.Empty)
                    {
                        insertResponse = await CrmHelper.PostData("/bsd_coowners", data);
                    }
                    else
                    {
                        insertResponse = await CrmHelper.PatchData($"/bsd_coowners({CoOwnerId})", data);
                    }

                    if (insertResponse.IsSuccess)
                    {
                        MessagingCenter.Send<CoOwnerForm, bool>(this, "LoadCoOwners", true);
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        await DisplayAlert("Thông báo", insertResponse.GetErrorMessage(), "Đóng");
                    }
                }
                catch (Exception ex)
                {
                    string mes = ex.Message;
                    await DisplayAlert("", mes, "Đóng");
                }
            }

            LoadingHelper.Hide();
        }

        private async void Cancle_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}