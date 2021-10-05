using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.ViewModels;
using Xamarin.Forms;

using System.Linq;
using Telerik.XamarinForms.Primitives;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using ConasiCRM.Portable.Helpers;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace ConasiCRM.Portable.Views
{
    public partial class DanhBa : ContentPage
    {
        public DanhBaViewModel viewModel;
        private ObservableCollection<DanhBaItemModel> SelectedContact = new ObservableCollection<DanhBaItemModel>();
        public DanhBa()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new DanhBaViewModel();
            LoadingHelper.Show();           
            viewModel.isCheckedAll = false;
            viewModel.total = 0;
            viewModel.numberChecked = 0;

            LoadContacts().GetAwaiter();
            LoadingHelper.Hide();
        }

        public void reset()
        {
            button_toLead.isVisible = false;
            viewModel.reset();
            SelectedContact.Clear();
            LoadContacts().GetAwaiter();
        }

        public async Task LoadContacts()
        {
            if(await PermissionHelper.RequestCameraPermission() != PermissionStatus.Granted)
            {
                await Navigation.PopAsync();
                return;
            }
            await viewModel.LoadLeadConvert();
            LoadingHelper.Show();
            var contacts = (await Plugin.ContactService.CrossContactService.Current.GetContactListAsync()).Where(x => x.Name != null);
            foreach (var tmp in contacts.OrderBy(x => x.Name))
            {
                var numbers = tmp.Numbers;
                foreach (var n in numbers)
                {
                    var sdt = n.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");

                    var item = new Models.DanhBaItemModel {
                        Name = tmp.Name,
                        numberFormated = sdt,
                        IsSelected = false
                    };
                    if (viewModel.LeadConvert.Where(x => x.mobilephone == sdt).ToList().Count <= 0)
                    {
                        item.IsConvertToLead = false;
                        viewModel.Contacts.Add(item);
                        SelectedContact.Add(item);
                    }
                    else
                    {
                        item.IsConvertToLead = true;
                        viewModel.Contacts.Add(item);
                    }                   
                }
            }          
            viewModel.total = SelectedContact.Count();
            LoadingHelper.Hide();
        }

        private void checkAll_IsCheckedChanged(object sender, Telerik.XamarinForms.Primitives.CheckBox.IsCheckedChangedEventArgs e)
        {
            foreach (var item in SelectedContact)
            {
                if(item.numberFormated != null)
                {
                    item.IsSelected = e.NewValue.Value;
                }
            }
            if (e.NewValue.Value) { viewModel.numberChecked = viewModel.total; button_toLead.isVisible = true; }
            else { viewModel.numberChecked = 0; button_toLead.isVisible = false; }

        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                listView.ItemsSource = viewModel.Contacts;
            }
            else
            {
                listView.ItemsSource = viewModel.Contacts.Where(x => x.numberFormated == null ||
                    x.Name.Replace("-","").Replace(" ","").Replace("(","").Replace(")","").IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0 
                    || x.numberFormated.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "").IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var grid = sender as Grid;
            var tapGes = (TapGestureRecognizer)grid.GestureRecognizers[0];
            var item = (Models.DanhBaItemModel)tapGes.CommandParameter;


            if (item.IsSelected == true) // đang là true đổi qua false thì set check all thành false.
            {
                if (viewModel.isCheckedAll == true)
                {
                    viewModel.isCheckedAll = false;
                    if (SelectedContact.Count > 1)
                        viewModel.numberChecked = 2;
                    else
                        viewModel.numberChecked = 1;
                }
                viewModel.numberChecked -= 1;
            }
            else
            {
                viewModel.numberChecked++;
            }
            if (SelectedContact.Count > 1)
                item.IsSelected = !item.IsSelected;
            // check all
            viewModel.isCheckedAll = viewModel.numberChecked == viewModel.total;
            button_toLead.isVisible = viewModel.numberChecked == 0 ? false : true;
        }

        private async void ConvertToLead_Clicked(object sender, EventArgs e)
        {
            if (SelectedContact.Any() == false)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn Contact để chuyển sang khách hàng tiềm năng");
                return;
            }

            var choice = await DisplayAlert("", "Chuyển liên hệ thành khách hàng tiềm năng?", "Chuyển", "Huỷ bỏ");
            if (choice)
            {
                this.ConvertToLead(SelectedContact);
            }
        }

        private async void ConvertToLead(IEnumerable<DanhBaItemModel> SelectedContact)
        {
            LoadingHelper.Show();          
            foreach (var i in SelectedContact)
            {
                var re = await createLead(new LeadFormModel()
                {
                    leadid = Guid.NewGuid(),
                    bsd_topic_label = "Khách Hàng Tiềm Năng Từ Danh Bạ",
                    lastname = i.Name,
                    mobilephone = i.numberFormated,
                });

                if (!re.IsSuccess)
                {
                    ToastMessageHelper.ShortMessage("Đã có lỗi xảy ra. Vui lòng thử lại sau");
                    LoadingHelper.Hide();
                    return;
                }
            }
            ToastMessageHelper.ShortMessage("Chuyển thành công");
            this.reset();
        }

        public async Task<CrmApiResponse> createLead(LeadFormModel leadFormModel)
        {
            string path = "/leads";
            var content = await this.getContent(leadFormModel);
            CrmApiResponse result = await CrmHelper.PostData(path, content);
            return result;
        }

        private async Task<object> getContent(LeadFormModel leadFormModel)
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["leadid"] = leadFormModel.leadid;
            data["subject"] = leadFormModel.bsd_topic_label;
            data["lastname"] = leadFormModel.lastname;
            data["mobilephone"] = leadFormModel.mobilephone;           
            if (UserLogged.Id != Guid.Empty)
            {
                data["bsd_employee@odata.bind"] = "/bsd_employees(" + UserLogged.Id + ")";
            }
            if (UserLogged.ManagerId != Guid.Empty)
            {
                data["ownerid@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            }
            return data;
        }
    }
}
