﻿using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountForm : ContentPage
    {
        public Action<bool> OnCompleted;
        private Guid AccountId;
        private AccountFormViewModel viewModel;
        public ICRMService<Account> accountService;

        public AccountForm()
        {
            InitializeComponent();
            AccountId = Guid.Empty;
            Init();
            Create();
        }

        public AccountForm(Guid accountId)
        {
            InitializeComponent();
            AccountId = accountId;
            Init();
            Update();
        }

        public void Init()
        {
            this.BindingContext = viewModel = new AccountFormViewModel();
            centerModalContacAddress.Body.BindingContext = viewModel;            
            Lookup_BusinessType.BindingContext = viewModel;
            SetPreOpen();
        }

        private void Create()
        {
            this.Title = "Tạo Mới Khách Hàng Doanh Nghiệp";
            btnSave.Text = "Tạo Mới";
            btnSave.Clicked += CreateContact_Clicked;
        }

        private void CreateContact_Clicked(object sender, EventArgs e)
        {
            SaveData(null);
        }

        private async void Update()
        {
            viewModel.singleAccount = new AccountFormModel();
            this.Title = "Cập Nhật Khách Hàng Doanh Nghiệp";
            btnSave.Text = "Cập Nhật";
            btnSave.Clicked += UpdateContact_Clicked;

            await viewModel.LoadOneAccount(this.AccountId);

            viewModel.LoadBusinessTypeForLookup();
            Lookup_BusinessType.SetUpModal();           
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_businesstypesys))
            {
                List<string> ids = new List<string>();
                var businesstypes = viewModel.singleAccount.bsd_businesstypesys.Split(',');
                foreach (var item in businesstypes)
                {
                    var businessType = viewModel.BusinessTypeOptionList.SingleOrDefault(x => x.Val == item).Val;
                    ids.Add(businessType);
                }
                viewModel.BusinessType = ids;              
            }
            if (viewModel.singleAccount.bsd_localization != null)
            {
                viewModel.Localization = AccountLocalization.GetLocalizationById(viewModel.singleAccount.bsd_localization);
            }

            if (viewModel.singleAccount.primarycontactname != null)
            {
                viewModel.GetPrimaryContactByID();
            }

            viewModel.singleAccount.bsd_address = await SetAddress();

            if (viewModel.singleAccount.accountid != Guid.Empty)
                OnCompleted?.Invoke(true);
            else
                OnCompleted?.Invoke(false);
        }

        private async Task<string> SetAddress()
        {
            List<string> listaddress = new List<string>();
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_housenumberstreet))
            {
                listaddress.Add(viewModel.singleAccount.bsd_housenumberstreet);
            }
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.district_name))
            {
                listaddress.Add(viewModel.singleAccount.district_name);
            }
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.province_name))
            {
                listaddress.Add(viewModel.singleAccount.province_name);
            }
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_postalcode))
            {
                listaddress.Add(viewModel.singleAccount.bsd_postalcode);
            }
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.country_name))
            {
                listaddress.Add(viewModel.singleAccount.country_name);
            }

            string address = string.Join(", ", listaddress);

            return address;
        }

        private void UpdateContact_Clicked(object sender, EventArgs e)
        {
            SaveData(this.AccountId.ToString());
        }

        public void SetPreOpen()
        {
            Lookup_Localization.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                AccountLocalization.Localizations();
                foreach (var item in AccountLocalization.LocalizationOptions)
                {
                    viewModel.LocalizationOptionList.Add(item);
                }
                LoadingHelper.Hide();
            };         
            Lookup_BusinessType.PreShow = async () =>
            {
                LoadingHelper.Show();
                viewModel.LoadBusinessTypeForLookup();
                LoadingHelper.Hide();
            };            
            Lookup_PrimaryContact.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadContactForLookup();
                LoadingHelper.Hide();
            };
            lookUpContacAddressCountry.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadCountryForLookup();
                LoadingHelper.Hide();
            };          
        }

        private async void SaveData(string id)
        {
            if (viewModel.Localization == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn loại khách hàng");
                return;
            }
            if (viewModel.singleAccount.bsd_name == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng nhập tên công ty");
                return;
            }
            if (viewModel.PrimaryContact == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn người đại diện");
                return;
            }
            if (viewModel.singleAccount.telephone1 == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng nhập số điện thoại công ty");
                return;
            }
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.emailaddress1))
            {
                Match match = regex.Match(viewModel.singleAccount.emailaddress1);
                if (!match.Success)
                {
                    ToastMessageHelper.ShortMessage("Email sai địng dạng. Vui lòng thử lại");
                    return;
                }
            }
            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_email2))
            {
                Match match = regex.Match(viewModel.singleAccount.bsd_email2);
                if (!match.Success)
                {
                    ToastMessageHelper.ShortMessage("Email 2 sai địng dạng. Vui lòng thử lại");
                    return;
                }
            }
            if (viewModel.singleAccount.bsd_registrationcode == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng nhập số giấy phép kinh doanh");
                return;
            }
            if (!await viewModel.Check_form_keydata(null, viewModel.singleAccount.bsd_registrationcode, viewModel.singleAccount.accountid.ToString()))
            {
                ToastMessageHelper.ShortMessage("Số giấy phép kinh doanh đã tạo trong dữ liệu doanh nghiệp");
                return;
            }
            if (viewModel.singleAccount.bsd_vatregistrationnumber != null)
            {
                if (!await viewModel.Check_form_keydata(viewModel.singleAccount.bsd_vatregistrationnumber, null, viewModel.singleAccount.accountid.ToString()))
                {
                    ToastMessageHelper.ShortMessage("Mã số thuế đã tạo trong dữ liệu doanh nghiệp");
                    return;
                }
            }

            LoadingHelper.Show();
            if (viewModel.Localization != null && viewModel.Localization.Val != null)
            {
                viewModel.singleAccount.bsd_localization = viewModel.Localization.Val;
            }
            if (viewModel.PrimaryContact != null && viewModel.PrimaryContact.Id != null)
            {
                viewModel.singleAccount._primarycontactid_value = viewModel.PrimaryContact.Id;
            }
            if (viewModel.BusinessType != null && viewModel.BusinessType.Count > 0)
            {
                viewModel.singleAccount.bsd_businesstypesys = string.Join(", ", viewModel.BusinessType);
            }
            if (id == null)
            {
                var created = await viewModel.createAccount();
                if (created)
                {
                    if (CustomerPage.NeedToRefreshAccount.HasValue) CustomerPage.NeedToRefreshAccount = true;
                    ToastMessageHelper.ShortMessage("Tạo khách hàng doanh nghiệp thành công");
                    await Navigation.PopAsync();
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Tạo khách hàng doanh nghiệp thất bại");
                }
            }
            else
            {
                var updated = await viewModel.updateAccount();
                if (updated)
                {
                    if (CustomerPage.NeedToRefreshAccount.HasValue) CustomerPage.NeedToRefreshAccount = true;
                    if (AccountDetailPage.NeedToRefreshAccount.HasValue) AccountDetailPage.NeedToRefreshAccount = true;
                    await Navigation.PopAsync();
                    ToastMessageHelper.ShortMessage("Cập nhật khách hàng doanh nghiệp thành công");
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Cập nhật khách hàng doanh nghiệp thất bại");
                }
            }
        }

        private async void DiaChiLienLac_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();               
            if (viewModel.AddressLine1Contac == null && !string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_housenumberstreet))
            {
                viewModel.AddressLine1Contac = viewModel.singleAccount.bsd_housenumberstreet;
            }

            if (viewModel.AddressPostalCodeContac == null && !string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_postalcode))
            {
                viewModel.AddressPostalCodeContac = viewModel.singleAccount.bsd_postalcode;
            }

            if (viewModel.AddressCountryContac == null && !string.IsNullOrWhiteSpace(viewModel.singleAccount.country_name))
            {
                viewModel.AddressCountryContac = await viewModel.LoadCountryByName(viewModel.singleAccount.country_name);
                await viewModel.LoadProvincesForLookup(viewModel.AddressCountryContac);
            }

            if (viewModel.AddressStateProvinceContac == null && !string.IsNullOrWhiteSpace(viewModel.singleAccount.province_name))
            {
                viewModel.AddressStateProvinceContac = await viewModel.LoadProvinceByName(viewModel.singleAccount._bsd_country_value, viewModel.singleAccount.province_name); ;
                await viewModel.LoadDistrictForLookup(viewModel.AddressStateProvinceContac);
            }

            if (viewModel.AddressCityContac == null && !string.IsNullOrWhiteSpace(viewModel.singleAccount.district_name))
            {
                viewModel.AddressCityContac = await viewModel.LoadDistrictByName(viewModel.singleAccount._bsd_province_value, viewModel.singleAccount.district_name);
            }

            LoadingHelper.Hide();
            await centerModalContacAddress.Show();
        }      

        private async void ContacAddressCountry_Changed(object sender, LookUpChangeEvent e)
        {
            await viewModel.LoadProvincesForLookup(viewModel.AddressCountryContac);
        }

        private async void ContacAddressProvince_Changed(object sender, LookUpChangeEvent e)
        {
            await viewModel.LoadDistrictForLookup(viewModel.AddressStateProvinceContac);
        }

        private async void CloseContacAddress_Clicked(object sender, EventArgs e)
        {
            await centerModalContacAddress.Hide();
        }

        private async void ConfirmContacAddress_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.AddressLine1Contac))
            {
                ToastMessageHelper.ShortMessage("Vui lòng nhập số nhà/đường/phường");
                return;
            }

            List<string> address = new List<string>();
            if (!string.IsNullOrWhiteSpace(viewModel.AddressLine1Contac))
            {
                viewModel.singleAccount.bsd_housenumberstreet = viewModel.AddressLine1Contac;
                address.Add(viewModel.AddressLine1Contac);
            }
            else
            {
                viewModel.singleAccount.bsd_housenumberstreet = null;
            }         

            if (viewModel.AddressCityContac != null)
            {
                viewModel.singleAccount.district_name = viewModel.AddressCityContac.Name;
                viewModel.singleAccount._bsd_district_value = viewModel.AddressCityContac.Id.ToString();
                address.Add(viewModel.AddressCityContac.Name);
            }
            else
            {
                viewModel.singleAccount.district_name = null;
                viewModel.singleAccount._bsd_district_value = null;
            }
            if (viewModel.AddressStateProvinceContac != null)
            {
                viewModel.singleAccount.province_name = viewModel.AddressStateProvinceContac.Name;
                viewModel.singleAccount._bsd_province_value = viewModel.AddressStateProvinceContac.Id.ToString();
                address.Add(viewModel.AddressStateProvinceContac.Name);
            }
            else
            {
                viewModel.singleAccount.province_name = null;
                viewModel.singleAccount._bsd_province_value = null;
            }

            if (!string.IsNullOrWhiteSpace(viewModel.AddressPostalCodeContac))
            {
                viewModel.singleAccount.bsd_postalcode = viewModel.AddressPostalCodeContac;
                address.Add(viewModel.AddressPostalCodeContac);
            }
            else
            {
                viewModel.singleAccount.bsd_postalcode = null;
            }

            if (viewModel.AddressCountryContac != null)
            {
                viewModel.singleAccount.country_name = viewModel.AddressCountryContac.Name;
                viewModel.singleAccount._bsd_country_value = viewModel.AddressCountryContac.Id.ToString();
                address.Add(viewModel.AddressCountryContac.Name);
            }
            else
            {
                viewModel.singleAccount.country_name = null;
                viewModel.singleAccount._bsd_country_value = null;
            }
            viewModel.singleAccount.bsd_address = viewModel.AddressCompositeContac = string.Join(", ", address);
            await centerModalContacAddress.Hide();
        }
    }
}