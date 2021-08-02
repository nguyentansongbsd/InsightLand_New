using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.ViewModels;
using Xamarin.Forms;
using permissionType = Plugin.Permissions.Abstractions.Permission;
using permissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;
using System.Linq;
using Telerik.XamarinForms.Primitives;
using ConasiCRM.Portable.Models;

namespace ConasiCRM.Portable.Views
{
    public partial class DanhBa : ContentPage
    {
        public DanhBaViewModel viewModel;
        private DanhBaItemModel lastEmptyItem;

        public DanhBa()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new DanhBaViewModel();
            LoadingHelper.Show();
            viewModel.isCheckedAll = false;
            viewModel.total = 0;
            viewModel.numberChecked = 0;

            lastEmptyItem = new DanhBaItemModel()
            {
                Name = "\n\n",
            };

            LoadContacts().GetAwaiter();
            LoadingHelper.Hide();
        }

        public void reset()
        {
            button_toLead.isVisible = false;
            viewModel.reset();

            LoadContacts().GetAwaiter();
        }

        public async Task LoadContacts()
        {
            if(await PermissionHelper.CheckPermissions(permissionType.Contacts) != permissionStatus.Granted)
            {
                await Navigation.PopAsync();
                return;
            }

            LoadingHelper.Show();
            var contacts = (await Plugin.ContactService.CrossContactService.Current.GetContactListAsync()).Where(x => x.Name != null);
            foreach (var tmp in contacts.OrderBy(x => x.Name))
            {
                var numbers = tmp.Numbers;
                foreach (var n in numbers)
                {
                    viewModel.Contacts.Add(new Models.DanhBaItemModel()
                    {
                        Name = tmp.Name,
                        numberFormated = n,
                        Email = tmp.Email,
                        IsSelected = false,
                    });
                }
            }

            viewModel.total = viewModel.Contacts.Count();

            viewModel.Contacts.Add(lastEmptyItem);
            LoadingHelper.Hide();
        }

        private void checkAll_IsCheckedChanged(object sender, Telerik.XamarinForms.Primitives.CheckBox.IsCheckedChangedEventArgs e)
        {
            foreach (var item in viewModel.Contacts)
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
                viewModel.isCheckedAll = false;
                viewModel.numberChecked -= 1;
            }
            else
            {
                viewModel.numberChecked++;
            }
            item.IsSelected = !item.IsSelected;

            // check all
            viewModel.isCheckedAll = viewModel.numberChecked == viewModel.total;
            button_toLead.isVisible = viewModel.numberChecked == 0 ? false : true;
        }

        private async void ConvertToLead_Clicked(object sender, EventArgs e)
        {
            var SelectedContact = this.viewModel.Contacts.Where(x => x.IsSelected);
            if (SelectedContact.Any() == false)
            {
                await DisplayAlert("Thông báo", "Vui lòng chọn Contact để chuyển sang khách hàng tiềm năng", "Đóng");
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
            LeadFormViewModel leadViewModel = new LeadFormViewModel();
            foreach (var i in SelectedContact)
            {
                //var re = await leadViewModel.createLead(new LeadFormModel()
                //{
                //    firstname = i.Name,
                //    mobilephone = i.numberFormated,
                //    emailaddress1 = i.Email
                //});

                //if (re == new Guid())
                //{
                //    await DisplayAlert("", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                //    LoadingHelper.Hide();
                //    return;
                //}
            }
            await DisplayAlert("", "Chuyển thành công", "OK");
            this.reset();
        }
    }
}
