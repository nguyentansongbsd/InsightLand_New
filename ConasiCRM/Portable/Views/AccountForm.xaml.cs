using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountForm : ContentPage
    {
        public Action<bool> CheckSingleAccount;
        private Guid AccountId;
        private AccountFormViewModel viewModel;
        public ICRMService<Account> accountService;
        Label required_field = new Label()
        {
            HorizontalTextAlignment = TextAlignment.End,
            VerticalOptions = LayoutOptions.Center,
            TextColor = Color.FromHex("#fb0000"),
            FontSize = 18,
            Text = "*",
        };
        public AccountForm()
        {
            InitializeComponent();
            AccountId = Guid.Empty;
            Init();
        }
        public AccountForm(Guid accountId)
        {
            InitializeComponent();
            AccountId = accountId;
            CheckAccount();
        }

        private async void CheckAccount()
        {
            await Init();
            if (viewModel.singleAccount != null)
                CheckSingleAccount(true);
            else
                CheckSingleAccount(false);
        }

        public async Task Init()
        {
            accountService = new CRMService<Account>();
            this.BindingContext = viewModel = new AccountFormViewModel();
            viewModel.ModalLookUp = PrimaryContactLoopkup;
            viewModel.InitializeModal();

            if (AccountId != Guid.Empty)
            {
                viewModel.Title = "Cập Nhật Khách Hàng Doanh Nghiệp";
                btnSave.Text = "Cập nhật";
                await Start(AccountId);
            }
            else
            {
                viewModel.Title = "Thêm Khách Hàng Doanh Nghiệp";
                btnSave.Text = "Lưu";
                viewModel.singleAccount = new AccountFormModel();

                viewModel.singleCustomergroup = viewModel.list_picker_bsd_customergroup.SingleOrDefault(x => x.Val == "");
                viewModel.singleLocalization = viewModel.list_picker_bsd_localization.SingleOrDefault(x => x.Val == "");

                await viewModel.LoadListMandatoryPrimary();
                await viewModel.LoadListCountry();
                await viewModel.LoadListProvince();
                await viewModel.LoadListDistrict();

                datagridQueuing.IsVisible = false;
                datagridQuotation.IsVisible = false;
                datagridContract.IsVisible = false;
                datagridCase.IsVisible = false;
                datagridActivities.IsVisible = false;
                datagridMandatorySecondary.IsVisible = false;
            }

        }

        public async Task Start(Guid AccountId)
        {
            viewModel.Title = "Cập Nhật Khách Hàng Doanh Nghiệp";

            datagridQueuing.IsVisible = true;
            datagridQuotation.IsVisible = true;
            datagridContract.IsVisible = true;
            datagridCase.IsVisible = true;
            datagridActivities.IsVisible = true;
            datagridMandatorySecondary.IsVisible = true;

            viewModel.list_thongtinqueing.Clear();
            viewModel.list_thongtinquotation.Clear();
            viewModel.list_thongtincontract.Clear();
            viewModel.list_thongtincase.Clear();
            viewModel.list_thongtinactivitie.Clear();

            if (AccountId != null) { await viewModel.LoadOneAccount(AccountId); }
            await viewModel.LoadListMandatoryPrimary();
            await viewModel.LoadListCountry();
            await viewModel.LoadListProvince();
            await viewModel.LoadListDistrict();

            if (!string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_businesstypevalue))
            {
                var businesstype = viewModel.singleAccount.bsd_businesstypevalue.Split(',').ToList();

                foreach (var item in businesstype)
                {
                    multipleSelectLoaiHinh.addSelectedItem(item);
                }
            };

            if (viewModel.singleAccount.bsd_localization != null) { viewModel.getLocalization(viewModel.singleAccount.bsd_localization); }

            if (viewModel.singleAccount.bsd_customergroup != null) { viewModel.getCustomergroup(viewModel.singleAccount.bsd_customergroup); }

            if (AccountId != null) { await viewModel.LoadDSQueueingAccount(AccountId); }

            if (AccountId != null) { await viewModel.LoadDSQuotationAccount(AccountId); }

            if (AccountId != null) { await viewModel.LoadDSContractAccount(AccountId); }

            if (AccountId != null) { await viewModel.LoadDSCaseAccount(AccountId); }

            if (AccountId != null) { await viewModel.LoadDSActivitiesAccount(AccountId); }

            if (AccountId != null) { await viewModel.Load_List_Mandatory_Secondary(AccountId.ToString()); }

            var data = viewModel.list_thongtincontract;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    if (item.statuscode == 1
                    || item.statuscode == 2
                    || item.statuscode == 100000000
                    || item.statuscode == 100000001
                    || item.statuscode == 100000002
                    || item.statuscode == 100000003
                    || item.statuscode == 100000004
                    || item.statuscode == 100000005
                    )
                    {
                        Entry_nameacc.IsEnabled = false;
                    }
                }
            }
        }

        //async void show_popup_Mandatory(object sender, EventArgs e)
        //{
        //    popup_list_viewMandatory_primary.IsVisible = true;

        //    SearBarPrimary.Text = "";
        //}

        public async void ItemAppearingprimary(object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
        {
            viewModel.morelookup_primary = true;
            var itemAppearing = e.Item as Portable.Models.ContactMandatoryPrimary;
            var lastItem = viewModel.list_lookup_primarycontactid.LastOrDefault();
            if (lastItem != null && itemAppearing.Id.Equals(lastItem.Id))
            {
                if (viewModel.morelookup_primary == true)
                {
                    viewModel.pageLookup_primary += 1;
                    await viewModel.LoadListMandatoryPrimary();
                }
            }
            viewModel.morelookup_primary = false;
        }

        //private void SearchBar_Primary(object sender, TextChangedEventArgs e)
        //{
        //    if (stack_popupMandatory_primary.Children.FirstOrDefault() != null)
        //    {
        //        if (e.NewTextValue == null)
        //        {
        //            (stack_popupMandatory_primary.Children.FirstOrDefault() as ListView).ItemsSource = viewModel.list_lookup_primarycontactid;
        //            return;
        //        }
        //       (stack_popupMandatory_primary.Children.FirstOrDefault() as ListView).ItemsSource = viewModel.list_lookup_primarycontactid.Where(x => x.Name.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0);
        //    }
        //}

        //void Clearvalue_Mandatory(object sender, System.EventArgs e)
        //{
        //    primarycontactid_text.IsVisible = false;
        //    primarycontactid_text.Text = null;
        //    btn_Mandatory.IsVisible = false;
        //    viewModel.singleAccount._primarycontactid_value = null;

        //    primarycontactid_default.IsVisible = true;
        //}

        void OnSelectItem_Mandatory_primary(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            //    primarycontactid_text.IsVisible = true;
            //    primarycontactid_text.Text = (e.Item as ContactMandatoryPrimary).Name;
            //    viewModel.singleAccount.primarycontactname = (e.Item as ContactMandatoryPrimary).Name;
            //    viewModel.singleAccount._primarycontactid_value = (e.Item as ContactMandatoryPrimary).Id;
            //    btn_Mandatory.IsVisible = true;

            //    primarycontactid_default.IsVisible = false;
            //    popup_list_viewMandatory_primary.IsVisible = false;
        }

        //void Button_Clicked(object sender, System.EventArgs e)
        //{
        //    popup_list_viewMandatory_primary.IsVisible = false;
        //}

        public async void OnSelectItem_Country(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            popupaccountaddress_country.Text = (e.Item as ListCountry).Name;
            popupaccountaddressen_country.Text = (e.Item as ListCountry).Nameen;
            popupaccountaddress_country_id.Text = (e.Item as ListCountry).Id;
            popupaccountaddress_country.HasClearButton = true;

            popup_list_viewCountry.IsVisible = false;

            await viewModel.LoadListProvinceId((e.Item as ListCountry).Id);

        }

        void SearchBarCountry_TextChanged(object sender, System.EventArgs e)
        {
            //count = ' + OrgConfig.RecordPerPage + @' page = ' + viewModel.Page + @'
        }

        void Button_Country_Clicked(object sender, System.EventArgs e)
        {
            popup_list_viewCountry.IsVisible = false;
        }

        public async void OnSelectItem_Province(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            popupaccountaddress_province.Text = (e.Item as ListProvince).Name;
            popupaccountaddressen_province.Text = (e.Item as ListProvince).Nameen;
            popupaccountaddress_province_id.Text = (e.Item as ListProvince).Id;
            popupaccountaddress_province.HasClearButton = true;
            popup_list_viewProvince.IsVisible = false;

            await viewModel.LoadListDistrictId((e.Item as ListProvince).Id);
        }

        void Button_Province_Clicked(object sender, System.EventArgs e)
        {
            popup_list_viewProvince.IsVisible = false;
        }

        void OnSelectItem_District(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            popupaccountaddress_district.Text = (e.Item as ListDistrict).Name;
            popupaccountaddressen_district.Text = (e.Item as ListDistrict).Nameen;
            popupaccountaddress_district_id.Text = (e.Item as ListDistrict).Id;
            popupaccountaddress_district.HasClearButton = true;
            popup_list_viewDistrict.IsVisible = false;
        }

        void Button_District_Clicked(object sender, System.EventArgs e)
        {
            popup_list_viewDistrict.IsVisible = false;
        }

        void Selectvalue_localization(object sender, System.EventArgs e)
        {
            //btn_localization.IsVisible = true;

            viewModel.singleAccount.bsd_localization = viewModel.singleLocalization == null ? null : viewModel.singleLocalization.Val;
            //viewModel.singleLocalization = viewModel.singleLocalization.Label;
        }

        void Clearvalue_localization(object sender, System.EventArgs e)
        {
            viewModel.singleLocalization = null;
            viewModel.singleAccount.bsd_localization = null;
        }

        void Selectvalue_customer_group(object sender, System.EventArgs e)
        {
            btn_customer_group.IsVisible = true;
            viewModel.singleAccount.bsd_customergroup = viewModel.singleCustomergroup == null ? null : viewModel.singleCustomergroup.Val;
        }

        void Clearvalue_customer_group(object sender, System.EventArgs e)
        {
            viewModel.singleCustomergroup = null;
            viewModel.singleAccount.bsd_customergroup = null;
        }

        void value_DateChanged(object sender, System.EventArgs e)
        {

        }

        void Clear_DateChanged(object sender, System.EventArgs e)
        {
            btndate_placeofissues.NullableDate = null;
            btnclear_placeofissues.IsVisible = false;
        }

        void show_popup_accountaddress(object sender, System.EventArgs e)
        {
            popupaccount_footer.Children.Remove(popupaccount_footer.Children.Last());
            var button = new Button()
            {
                Text = "Xác nhận",
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            button.Clicked += save_popup_accountaddress;
            popupaccount_footer.Children.Add(button);

            popupaccountaddress_housenumberstreet.Text = viewModel.singleAccount.bsd_housenumberstreet;
            popupaccountaddress_housenumber.Text = viewModel.singleAccount.bsd_street;

            popupaccountaddress_country.Text = viewModel.singleAccount.nation_name;
            popupaccountaddressen_country.Text = viewModel.singleAccount.nation_nameen;
            popupaccountaddress_country.HasClearButton = viewModel.singleAccount.nation_name == null ? false : true;
            popupaccountaddress_country_id.Text = viewModel.singleAccount._bsd_nation_value;

            popupaccountaddress_province.Text = viewModel.singleAccount.province_name;
            popupaccountaddressen_province.Text = viewModel.singleAccount.province_nameen;
            popupaccountaddress_province.HasClearButton = viewModel.singleAccount.province_name == null ? false : true;
            popupaccountaddress_province_id.Text = viewModel.singleAccount._bsd_province_value;

            popupaccountaddress_district.Text = viewModel.singleAccount.district_name;
            popupaccountaddressen_district.Text = viewModel.singleAccount.district_nameen;
            popupaccountaddress_district.HasClearButton = viewModel.singleAccount.district_name == null ? false : true;
            popupaccountaddress_district_id.Text = viewModel.singleAccount._bsd_district_value;

            popup_account_address.IsVisible = true;
        }

        void save_popup_accountaddress(object sender, System.EventArgs e)
        {
            viewModel.singleAccount.bsd_housenumberstreet = popupaccountaddress_housenumberstreet.Text;
            viewModel.singleAccount.bsd_street = popupaccountaddress_housenumber.Text;

            viewModel.singleAccount.nation_name = popupaccountaddress_country.Text;
            viewModel.singleAccount.nation_nameen = popupaccountaddressen_country.Text;
            viewModel.singleAccount._bsd_nation_value = popupaccountaddress_country_id.Text;

            viewModel.singleAccount.province_name = popupaccountaddress_province.Text;
            viewModel.singleAccount.province_nameen = popupaccountaddressen_province.Text;
            viewModel.singleAccount._bsd_province_value = popupaccountaddress_province_id.Text;

            viewModel.singleAccount.district_name = popupaccountaddress_district.Text;
            viewModel.singleAccount.district_nameen = popupaccountaddressen_district.Text;
            viewModel.singleAccount._bsd_district_value = popupaccountaddress_district_id.Text;

            var tmp = new List<string>();
            if (!string.IsNullOrEmpty(viewModel.singleAccount.bsd_housenumberstreet)) { tmp.Add(viewModel.singleAccount.bsd_housenumberstreet); }
            if (!string.IsNullOrEmpty(viewModel.singleAccount.district_name)) { tmp.Add(viewModel.singleAccount.district_name); }
            if (!string.IsNullOrEmpty(viewModel.singleAccount.province_name)) { tmp.Add(viewModel.singleAccount.province_name); }
            if (!string.IsNullOrEmpty(viewModel.singleAccount.nation_name)) { tmp.Add(viewModel.singleAccount.nation_name); }

            var tmpen = new List<string>();
            if (!string.IsNullOrEmpty(viewModel.singleAccount.bsd_street)) { tmpen.Add(viewModel.singleAccount.bsd_street); }
            if (!string.IsNullOrEmpty(viewModel.singleAccount.district_nameen)) { tmpen.Add(viewModel.singleAccount.district_nameen); }
            if (!string.IsNullOrEmpty(viewModel.singleAccount.province_nameen)) { tmpen.Add(viewModel.singleAccount.province_nameen); }
            if (!string.IsNullOrEmpty(viewModel.singleAccount.nation_nameen)) { tmpen.Add(viewModel.singleAccount.nation_nameen); }

            viewModel.singleAccount.bsd_address = string.Join(", ", tmp);
            viewModel.singleAccount.bsd_diachi = string.Join(", ", tmpen);
            //Debug.WriteLine("xxx" + viewModel.Account.bsd_address);
            this.hide_popup_accountaddress(null, null);
        }

        void show_popup_permanent_accountaddress(object sender, System.EventArgs e)
        {
            popupaccount_footer.Children.Remove(popupaccount_footer.Children.Last());
            var button = new Button()
            {
                Text = "Xác nhận",
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            button.Clicked += save_popup_permanent_accountaddress;
            popupaccount_footer.Children.Add(button);

            popupaccountaddress_housenumberstreet.Text = viewModel.singleAccount.bsd_permanenthousenumberstreetwardvn;
            popupaccountaddress_housenumber.Text = viewModel.singleAccount.bsd_permanenthousenumberstreetward;

            popupaccountaddress_country.Text = viewModel.singleAccount.permanentnation_name;
            popupaccountaddressen_country.Text = viewModel.singleAccount.permanentnation_nameen;
            popupaccountaddress_country.HasClearButton = viewModel.singleAccount.permanentnation_name == null ? false : true;
            popupaccountaddress_country_id.Text = viewModel.singleAccount._bsd_permanentnation_value;

            popupaccountaddress_province.Text = viewModel.singleAccount.permanentprovince_name;
            popupaccountaddressen_province.Text = viewModel.singleAccount.permanentprovince_nameen;
            popupaccountaddress_province.HasClearButton = viewModel.singleAccount.permanentprovince_name == null ? false : true;
            popupaccountaddress_province_id.Text = viewModel.singleAccount._bsd_permanentprovince_value;

            popupaccountaddress_district.Text = viewModel.singleAccount.permanentdistrict_name;
            popupaccountaddressen_district.Text = viewModel.singleAccount.permanentdistrict_nameen;
            popupaccountaddress_district.HasClearButton = viewModel.singleAccount.permanentdistrict_name == null ? false : true;
            popupaccountaddress_district_id.Text = viewModel.singleAccount._bsd_permanentdistrict_value;

            popup_account_address.IsVisible = true;
        }

        void save_popup_permanent_accountaddress(object sender, System.EventArgs e)
        {
            viewModel.singleAccount.bsd_permanenthousenumberstreetwardvn = popupaccountaddress_housenumberstreet.Text;             viewModel.singleAccount.bsd_permanenthousenumberstreetward = popupaccountaddress_housenumber.Text;
             viewModel.singleAccount.permanentnation_name = popupaccountaddress_country.Text;
            viewModel.singleAccount.permanentnation_nameen = popupaccountaddressen_country.Text;             viewModel.singleAccount._bsd_permanentprovince_value = popupaccountaddress_country_id.Text;
             viewModel.singleAccount.permanentprovince_name = popupaccountaddress_province.Text;
            viewModel.singleAccount.permanentprovince_nameen = popupaccountaddressen_province.Text;             viewModel.singleAccount._bsd_permanentprovince_value = popupaccountaddress_province_id.Text;
             viewModel.singleAccount.permanentdistrict_name = popupaccountaddress_district.Text;
            viewModel.singleAccount.permanentdistrict_nameen = popupaccountaddressen_district.Text;             viewModel.singleAccount._bsd_permanentdistrict_value = popupaccountaddress_district_id.Text;              var tmp = new List<string>();             if (!string.IsNullOrEmpty(viewModel.singleAccount.bsd_permanenthousenumberstreetwardvn)) { tmp.Add(viewModel.singleAccount.bsd_permanenthousenumberstreetwardvn); }             if (!string.IsNullOrEmpty(viewModel.singleAccount.permanentdistrict_name)) { tmp.Add(viewModel.singleAccount.permanentdistrict_name); }             if (!string.IsNullOrEmpty(viewModel.singleAccount.permanentprovince_name)) { tmp.Add(viewModel.singleAccount.permanentprovince_name); }             if (!string.IsNullOrEmpty(viewModel.singleAccount.permanentnation_name)) { tmp.Add(viewModel.singleAccount.permanentnation_name); } 
            var tmpen = new List<string>();
            if (!string.IsNullOrEmpty(viewModel.singleAccount.bsd_permanenthousenumberstreetward)) { tmpen.Add(viewModel.singleAccount.bsd_permanenthousenumberstreetward); }
            if (!string.IsNullOrEmpty(viewModel.singleAccount.permanentdistrict_nameen)) { tmpen.Add(viewModel.singleAccount.permanentdistrict_nameen); }
            if (!string.IsNullOrEmpty(viewModel.singleAccount.permanentprovince_nameen)) { tmpen.Add(viewModel.singleAccount.permanentprovince_nameen); }
            if (!string.IsNullOrEmpty(viewModel.singleAccount.permanentnation_nameen)) { tmpen.Add(viewModel.singleAccount.permanentnation_nameen); }
             viewModel.singleAccount.bsd_permanentaddress1 = string.Join(", ", tmp);             viewModel.singleAccount.bsd_diachithuongtru = string.Join(", ", tmpen);

            //Debug.WriteLine("xxx" + viewModel.Account.bsd_permanentaddress1);
            this.hide_popup_accountaddress(null, null);
        }

        void show_popup_listview_country(object sender, System.EventArgs e)
        {
            popup_list_viewCountry.IsVisible = true;

            searchBarCountry.Text = "";
        }

        public async void ItemAppearingcountry(object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
        {
            viewModel.morelookup_country = true;
            var itemAppearing = e.Item as Portable.Models.ListCountry;
            var lastItem = viewModel.list_lookup_Country.LastOrDefault();
            if (lastItem != null && itemAppearing.Id.Equals(lastItem.Id))
            {
                if (viewModel.morelookup_country == true)
                {
                    viewModel.pageLookup_country += 1;
                    await viewModel.LoadListCountry();
                }
            }
            viewModel.morelookup_country = false;
        }

        private void SearchBar_Country(object sender, TextChangedEventArgs e)
        {
            if (stack_popupCountry.Children.FirstOrDefault() != null)
            {
                if (e.NewTextValue == null)
                {
                    (stack_popupCountry.Children.FirstOrDefault() as ListView).ItemsSource = viewModel.list_lookup_Country;
                    return;
                }
               (stack_popupCountry.Children.FirstOrDefault() as ListView).ItemsSource = viewModel.list_lookup_Country.Where(x => x.Name.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        void clear_entry_country(object sender, System.EventArgs e)
        {
            popupaccountaddress_country.Text = null;
            popupaccountaddressen_country.Text = null;
            popupaccountaddress_country_id.Text = null;
            popupaccountaddress_country.HasClearButton = false;
        }

        async void show_popup_listview_province(object sender, System.EventArgs e)
        {
            if (popupaccountaddress_country.Text == null)
            {
                await DisplayAlert("", "Vui lòng chọn quốc gia", "OK");
                return;
            }
            else
            {
                popup_list_viewProvince.IsVisible = true;

                SearBarProvince.Text = "";
            }
        }

        public async void ItemAppearingprovince(object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
        {
            viewModel.morelookup_province = true;
            var itemAppearing = e.Item as Portable.Models.ListProvince;
            var lastItem = viewModel.list_lookup_Province.LastOrDefault();
            if (lastItem != null && itemAppearing.Id.Equals(lastItem.Id))
            {
                if (viewModel.morelookup_province == true)
                {
                    viewModel.pageLookup_province += 1;
                    await viewModel.LoadListCountry();
                }
            }
            viewModel.morelookup_province = false;
        }

        private void SearchBar_Province(object sender, TextChangedEventArgs e)
        {
            if (stack_popupProvince.Children.FirstOrDefault() != null)
            {
                if (e.NewTextValue == null)
                {
                    (stack_popupProvince.Children.FirstOrDefault() as ListView).ItemsSource = viewModel.list_lookup_Province;
                    return;
                }
               (stack_popupProvince.Children.FirstOrDefault() as ListView).ItemsSource = viewModel.list_lookup_Province.Where(x => x.Name.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        void clear_entry_province(object sender, System.EventArgs e)
        {
            popupaccountaddress_province.Text = null;
            popupaccountaddressen_province.Text = null;
            popupaccountaddress_province_id.Text = null;
            popupaccountaddress_province.HasClearButton = false;
        }

        async void show_popup_listview_district(object sender, System.EventArgs e)
        {
            if (popupaccountaddress_province.Text == null)
            {
                await DisplayAlert("", "Vui lòng chọn tỉnh/thành", "OK");
                return;
            }
            else
            {
                popup_list_viewDistrict.IsVisible = true;

                searchBarDistrict.Text = "";
            }
        }

        public async void ItemAppearingdistrict(object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
        {
            viewModel.morelookup_district = true;
            var itemAppearing = e.Item as Portable.Models.ListDistrict;
            var lastItem = viewModel.list_lookup_District.LastOrDefault();
            if (lastItem != null && itemAppearing.Id.Equals(lastItem.Id))
            {
                if (viewModel.morelookup_district == true)
                {
                    viewModel.pageLookup_district += 1;
                    await viewModel.LoadListDistrict();
                }
            }
            viewModel.morelookup_district = false;
        }

        private void SearchBar_District(object sender, TextChangedEventArgs e)
        {
            if (stack_popupDistrict.Children.FirstOrDefault() != null)
            {
                if (e.NewTextValue == null)
                {
                    (stack_popupDistrict.Children.FirstOrDefault() as ListView).ItemsSource = viewModel.list_lookup_District;
                    return;
                }
               (stack_popupDistrict.Children.FirstOrDefault() as ListView).ItemsSource = viewModel.list_lookup_District.Where(x => x.Name.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        void clear_entry_district(object sender, System.EventArgs e)
        {
            popupaccountaddress_district.Text = null;
            popupaccountaddressen_district.Text = null;
            popupaccountaddress_district_id.Text = null;
            popupaccountaddress_district.HasClearButton = false;
        }

        void hide_popup_accountaddress(object sender, System.EventArgs e)
        {
            requiredform.Children.Remove(required_field);
            popup_account_address.IsVisible = false;
        }

        void SearchBar_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {

        }

        private async Task<String> checkData()
        {
            var tmp = new List<string>();
            //if (viewModel.singleAccount.bsd_businesstype_customer == true) { tmp.Add("100000000"); }
            //if (viewModel.singleAccount.bsd_businesstype_partner == true) { tmp.Add("100000001"); }
            //if (viewModel.singleAccount.bsd_businesstype_saleagents == true) { tmp.Add("100000002"); }
            //if (viewModel.singleAccount.bsd_businesstype_deverloper == true) { tmp.Add("100000003"); }

            //var a = viewModel.SelectedLoaiHinh;

            var bsd_businesstype = string.Join(", ", viewModel.SelectedLoaiHinh);
            if (viewModel.PrimaryContact != null)
            {
                viewModel.singleAccount._primarycontactid_value = viewModel.PrimaryContact.Id.ToString();
            }
            else
            {
                viewModel.singleAccount._primarycontactid_value = null;
            }

            if (string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_name) || string.IsNullOrWhiteSpace(viewModel.singleAccount._primarycontactid_value) ||
                string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_localization) || string.IsNullOrWhiteSpace(viewModel.singleAccount.telephone1) ||
                string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_registrationcode) || string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_housenumberstreet) ||
                string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_permanenthousenumberstreetwardvn) || string.IsNullOrEmpty(bsd_businesstype) ||
                string.IsNullOrEmpty(viewModel.singleAccount.bsd_name) || string.IsNullOrEmpty(viewModel.singleAccount.telephone1) ||
                string.IsNullOrEmpty(viewModel.singleAccount.bsd_registrationcode) || string.IsNullOrEmpty(viewModel.singleAccount.bsd_housenumberstreet) ||
                string.IsNullOrEmpty(viewModel.singleAccount.bsd_permanenthousenumberstreetwardvn))
            {
                viewModel.IsBusy = false;
                return "Vui lòng nhập các thông tin bắt buộc (trường có gắn dấu *)!";

            }
            else
            {
                if (!await viewModel.Check_form_keydata(viewModel.singleAccount.bsd_vatregistrationnumber, viewModel.singleAccount.bsd_registrationcode, viewModel.singleAccount.accountid.ToString()))
                {
                    if (viewModel.singleAccount.bsd_registrationcode == viewModel.list_check_data.bsd_registrationcode ||
                        (viewModel.singleAccount.bsd_vatregistrationnumber == viewModel.list_check_data.bsd_vatregistrationnumber && string.IsNullOrEmpty(viewModel.singleAccount.bsd_vatregistrationnumber.Trim())))
                    {
                        viewModel.IsBusy = false;
                        return "Số GPKD hoặc mã số thuế đã tạo trong dữ liệu doanh nghiệp [" + viewModel.list_check_data.bsd_account_name + "]";
                    }
                }

                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                if (!string.IsNullOrEmpty(viewModel.singleAccount.emailaddress1) && !string.IsNullOrWhiteSpace(viewModel.singleAccount.emailaddress1))
                {

                    Match match = regex.Match(viewModel.singleAccount.emailaddress1);
                    if (!match.Success) { viewModel.IsBusy = false; return "Địa chỉ mail sai. Vui lòng thử lại!"; }
                }

                if (!string.IsNullOrEmpty(viewModel.singleAccount.bsd_email2) && !string.IsNullOrWhiteSpace(viewModel.singleAccount.bsd_email2))
                {

                    Match match1 = regex.Match(viewModel.singleAccount.bsd_email2);
                    if (!match1.Success) { viewModel.IsBusy = false; return "Địa chỉ mail sai. Vui lòng thử lại!"; }
                }
                //MailAddress m = new MailAddress(viewModel.singleAccount.emailaddress1);
                if(!PhoneNumberFormatVNHelper.CheckValidate(viewModel.singleAccount.telephone1))
                {
                    return "Số điện thoại sai địng dạng. Vui lòng thử lại!";
                }
                return "Sucesses";
            }
        }

        private async void SaveMenu_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            int Mode = 1;
            var check = await checkData();
            if (viewModel.singleAccount.accountid == Guid.Empty)
            {
                viewModel.singleAccount.accountid = Guid.NewGuid();
                Mode = 1;
                if (check == "Sucesses")
                {
                    var created = await createAccount(viewModel);

                    if (created != new Guid())
                    {
                        if (AccountList.NeedToRefresh.HasValue) AccountList.NeedToRefresh = true;
                        await Navigation.PopAsync();
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Thông báo", "Tạo khách hàng doanh nghiệp thành công!", "OK");
                        //Xamarin.Forms.Application.Current.Properties["update"] = "1";
                        //this.Start(viewModel.singleAccount.accountid);
                    }
                    else
                    {
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Thông báo", "Tạo khách hàng doanh nghiệp thất bại", "OK");
                    }
                }
                else
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Thông báo", check, "OK");
                }
            }
            else
            {
                Mode = 2;
                if (check == "Sucesses")
                {
                    var updated = await updateAccount(viewModel);
                    if (updated)
                    {
                        if (AccountList.NeedToRefresh.HasValue) AccountList.NeedToRefresh = true;
                        await Navigation.PopAsync();
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Thông báo", "Cập nhật thành công!", "OK");
                        //Xamarin.Forms.Application.Current.Properties["update"] = "1";
                        //this.Start(viewModel.singleAccount.accountid);
                    }
                    else
                    {
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Thông báo", "Cập nhật thất bại!", "OK");
                    }
                }
                else
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Thông báo", check, "OK");
                }
            }
            LoadingHelper.Hide();
            //viewModel.IsBusy = false;
        }

        public async Task<Guid> createAccount(AccountFormViewModel viewModel)
        {
            string path = "/accounts";
            viewModel.singleAccount.accountid = Guid.NewGuid();
            var content = await this.getContent(viewModel);

            CrmApiResponse result = await CrmHelper.PostData(path, content);

            if (result.IsSuccess)
            {
                return viewModel.singleAccount.accountid;
            }
            else
            {
                var mess = result.ErrorResponse?.error?.message ?? "Đã xảy ra lỗi. Vui lòng thử lại.";
                await App.Current.MainPage.DisplayAlert("Thông báo", mess, "OK");
                return new Guid();
            }

        }

        public async Task<Boolean> updateAccount(AccountFormViewModel account)
        {
            string path = "/accounts(" + account.singleAccount.accountid + ")";
            var content = await this.getContent(account);
            CrmApiResponse result = await CrmHelper.PatchData(path, content);
            if (result.IsSuccess)
            {
                return true;
            }

            else
            {
                var mess = result.ErrorResponse?.error?.message ?? "Đã xảy ra lỗi. Vui lòng thử lại.";
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", mess, "OK");
                return false;
            }

        }

        public async Task<Boolean> DeletLookup(string fieldName, Guid AccountId)
        {
            var result = await CrmHelper.SetNullLookupField("accounts", AccountId, fieldName);
            return result.IsSuccess;
        }

        private async Task<object> getContent(AccountFormViewModel account)
        {
            //var tmp = new List<string>();
            //if (viewModel.singleAccount.bsd_businesstype_customer == true) { tmp.Add("100000000"); }
            //if (viewModel.singleAccount.bsd_businesstype_partner == true) { tmp.Add("100000001"); }
            //if (viewModel.singleAccount.bsd_businesstype_saleagents == true) { tmp.Add("100000002"); }
            //if (viewModel.singleAccount.bsd_businesstype_deverloper == true) { tmp.Add("100000003"); }

            viewModel.singleAccount.bsd_businesstypevalue = string.Join(", ", viewModel.SelectedLoaiHinh);

            IDictionary<string, object> data = new Dictionary<string, object>();
            data["accountid"] = account.singleAccount.accountid.ToString();
            data["bsd_name"] = account.singleAccount.bsd_name ?? "";
            data["bsd_accountnameother"] = account.singleAccount.bsd_accountnameother ?? "";
            data["bsd_companycode"] = account.singleAccount.bsd_companycode ?? "";
            if (account.singleAccount.bsd_businesstypevalue != null)
            {
                data["bsd_businesstypevalue"] = account.singleAccount.bsd_businesstypevalue.Replace(" ", "");
            }

            if (account.singleAccount.bsd_localization != null)
            {
                data["bsd_localization"] = int.Parse(account.singleAccount.bsd_localization);
            }

            if (account.singleAccount.bsd_customergroup != null)
            {
                data["bsd_customergroup"] = int.Parse(account.singleAccount.bsd_customergroup);
            }
            else
            {
                data["bsd_customergroup"] = null;
            }

            //data["bsd_diemdanhgia"] = account.singleAccount.bsd_diemdanhgia_format ?? null;
            data["emailaddress1"] = account.singleAccount.emailaddress1 ?? "";
            data["bsd_email2"] = account.singleAccount.bsd_email2 ?? "";
            data["websiteurl"] = account.singleAccount.websiteurl ?? "";
            data["fax"] = account.singleAccount.fax ?? "";
            data["telephone1"] = account.singleAccount.telephone1 ?? "";
            data["bsd_registrationcode"] = account.singleAccount.bsd_registrationcode ?? new Random().Next(1000, 9999).ToString();
            data["bsd_issuedon"] = account.singleAccount.bsd_issuedon.HasValue ? (DateTime.Parse(account.singleAccount.bsd_issuedon.ToString()).ToLocalTime()).ToString("yyyy-MM-dd\"T\"HH:mm:ss\"Z\"") : null;
            data["bsd_placeofissue"] = account.singleAccount.bsd_placeofissue ?? "";
            data["bsd_khachhangdagiaodich"] = account.singleAccount.bsd_khachhangdagiaodich.ToString() ?? null;
            data["bsd_vatregistrationnumber"] = account.singleAccount.bsd_vatregistrationnumber ?? "";
            data["bsd_address"] = account.singleAccount.bsd_address ?? "";
            data["bsd_diachi"] = account.singleAccount.bsd_diachi ?? "";
            data["bsd_permanentaddress1"] = account.singleAccount.bsd_permanentaddress1 ?? "";
            data["bsd_diachithuongtru"] = account.singleAccount.bsd_diachithuongtru ?? "";
            data["bsd_housenumberstreet"] = account.singleAccount.bsd_housenumberstreet ?? "";
            data["bsd_street"] = account.singleAccount.bsd_street ?? "";
            data["bsd_permanenthousenumberstreetwardvn"] = account.singleAccount.bsd_permanenthousenumberstreetwardvn ?? "";
            data["bsd_permanenthousenumberstreetward"] = account.singleAccount.bsd_permanenthousenumberstreetward ?? "";

            if (account.singleAccount._primarycontactid_value == null)
            {
                await DeletLookup("primarycontactid", account.singleAccount.accountid);
            }
            else
            {
                data["primarycontactid@odata.bind"] = "/contacts(" + account.singleAccount._primarycontactid_value + ")"; /////Lookup Field
            }
            if (account.singleAccount._bsd_nation_value == null)
            {
                await DeletLookup("bsd_nation", account.singleAccount.accountid);
            }
            else
            {
                data["bsd_nation@odata.bind"] = "/bsd_countries(" + account.singleAccount._bsd_nation_value + ")"; /////Lookup Field
            }
            if (account.singleAccount._bsd_province_value == null)
            {
                await DeletLookup("bsd_province", account.singleAccount.accountid);
            }
            else
            {
                data["bsd_province@odata.bind"] = "/new_provinces(" + account.singleAccount._bsd_province_value + ")"; /////Lookup Field
            }
            if (account.singleAccount._bsd_district_value == null)
            {
                await DeletLookup("bsd_district", account.singleAccount.accountid);
            }
            else
            {
                data["bsd_district@odata.bind"] = "/new_districts(" + account.singleAccount._bsd_district_value + ")"; /////Lookup Field
            }
            if (account.singleAccount._bsd_permanentnation_value == null)
            {
                await DeletLookup("bsd_PermanentNation", account.singleAccount.accountid);
            }
            else
            {
                data["bsd_PermanentNation@odata.bind"] = "/bsd_countries(" + account.singleAccount._bsd_permanentnation_value + ")"; /////Lookup Field
            }
            if (account.singleAccount._bsd_permanentprovince_value == null)
            {
                await DeletLookup("bsd_PermanentProvince", account.singleAccount.accountid);
            }
            else
            {
                data["bsd_PermanentProvince@odata.bind"] = "/new_provinces(" + account.singleAccount._bsd_permanentprovince_value + ")"; /////Lookup Field
            }
            if (account.singleAccount._bsd_permanentdistrict_value == null)
            {
                await DeletLookup("bsd_PermanentDistrict", account.singleAccount.accountid);
            }
            else
            {
                data["bsd_PermanentDistrict@odata.bind"] = "/new_districts(" + account.singleAccount._bsd_permanentdistrict_value + ")"; /////Lookup Field
            }
            return data;
        }

        //--------------------------------------------------------------------//

        /// <summary>
        /// ////////////////////////////////////////////////////////////////////
        /// </summary>

        public async Task Load()
        {
            var data = await accountService.RetrieveMultiple("accounts", @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='account'>
                <attribute name='primarycontactid' alias='primarycontact_id' />
                <attribute name='bsd_mandatorysecondary' />
                <attribute name='telephone1' />
                <attribute name='bsd_businesstypevalue' />
                <attribute name='bsd_customergroup' />
                <attribute name='bsd_localization' />
                <attribute name='bsd_rocnumber2' />
                <attribute name='bsd_rocnumber1' />
                <attribute name='websiteurl' />
                <attribute name='bsd_vatregistrationnumber' />
                <attribute name='bsd_incorporatedate' />
                <attribute name='bsd_hotlines' />
                <attribute name='bsd_generalledgercompanynumber' />
                <attribute name='fax' />
                <attribute name='emailaddress1' />
                <attribute name='bsd_groupgstregisttationnumber' />
                <attribute name='statuscode' />
                <attribute name='ownerid' />
                <attribute name='createdon' />
                <attribute name='address1_composite' />
                <attribute name='bsd_companycode' />
                <attribute name='bsd_registrationcode' />
                <attribute name='bsd_accountnameother' />
                <attribute name='bsd_name' />
                <attribute name='name' />
                <attribute name='accountid' />
                <link-entity name='contact' from='contactid' to='primarycontactid' visible='false' link-type='outer' alias='a_410707b195544cd984376608b1802904'>
                    <attribute name='bsd_fullname' alias='primarycontact_name' />
                </link-entity>
                <filter type='and'>
                  <condition attribute='accountid' operator='eq' uitype='account' value='" + AccountId + @"' />
                </filter>
              </entity>
            </fetch>");
            if (data.Any())
            {
                var account = data.SingleOrDefault();
                if (!string.IsNullOrWhiteSpace(account.bsd_businesstypevalue))
                {
                    string[] values = account.bsd_businesstypevalue.Split(',');
                    string[] labels = viewModel.BusinessTypeOptionList.Where(x => values.Any(y => y == x.Val)).Select(x => x.Label).ToArray();
                    account.BusinessType = new OptionSet()
                    {
                        Val = string.Join(",", values),
                        Label = string.Join(", ", labels),
                    };
                }

                if (!string.IsNullOrEmpty(account.bsd_localization))
                {
                    account.Localization = viewModel.LocalizationOptionList.SingleOrDefault(x => x.Val == account.bsd_localization);
                }

                if (!string.IsNullOrEmpty(account.bsd_customergroup))
                {
                    account.CustomerGroup = viewModel.CustomerGroupOptionList.SingleOrDefault(x => x.Val == account.bsd_customergroup);
                }

                // mandatory contact
                if (account.primarycontact_id != Guid.Empty)
                {
                    account.PrimaryContact = new LookUp()
                    {
                        Id = account.primarycontact_id,
                        Name = account.primarycontact_name ?? "No Name"
                    };
                }

                viewModel.Account = account;
                viewModel.Title = account.name;
            }
            viewModel.IsBusy = false;
        }

        private void ShowPopupBusinessType(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Account?.BusinessType.Val) == false) // Co gia tri
            {
                string[] values = viewModel.Account.BusinessType.Val.Split(',');

                // tim value co trong business types
                foreach (var item in viewModel.BusinessTypeOptionList.Where(x => values.Any(y => y == x.Val)))
                {
                    item.Selected = true;
                }

            }
            else // khong co gia tri
            {
                // un selected
                foreach (var item in viewModel.BusinessTypeOptionList)
                {
                    item.Selected = false;
                }
            }
            viewModel.ShowBusinessTypeModal = true;
        }

        private void BtnCloseBusinessTypeModal_Clicked(object sender, EventArgs e)
        {
            viewModel.ShowBusinessTypeModal = false;
        }

        private void BusinessTypeListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                OptionSet bt = e.Item as OptionSet;
                bt.Selected = !bt.Selected;
            }
        }

        private void BtnSelectedBusinessType_Clicked(object sender, EventArgs e)
        {
            var selecteds = viewModel.BusinessTypeOptionList.Where(x => x.Selected);
            viewModel.Account.BusinessType.Val = string.Join(", ", selecteds.Select(x => x.Val).ToArray());
            viewModel.Account.BusinessType.Label = string.Join(", ", selecteds.Select(x => x.Label).ToArray());
            viewModel.ShowBusinessTypeModal = false;
        }

        private async void OpenLookupModal(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                viewModel.CurrentLookUpConfig = ((Button)sender).CommandParameter as LookUpConfig;
            }
            else if (sender is Label)
            {
                Label label = ((Label)sender);
                TapGestureRecognizer tapGes = label.FindByName<TapGestureRecognizer>("labelTapGes");
                viewModel.CurrentLookUpConfig = tapGes.CommandParameter as LookUpConfig;
            }
            else
            {
                return;
            }

            //viewModel.LookUpData.Clear();
            viewModel.ShowLookUpModal = true;
            viewModel.LookUpLoading = true;
            //viewModel.LookUpPage = 1;
            //await viewModel.loadData();
            viewModel.LookUpLoading = false;
        }

        private void btnCloseLookUpModal(object sender, EventArgs e)
        {
            //viewModel.LookUpData.Clear();
            viewModel.ShowLookUpModal = false;
        }

        private void LvLookUp_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as LookUp;
            string itemid = item.Id.ToString();
            var account = viewModel.Account;
            PropertyInfo prop = account.GetType().GetProperty(viewModel.CurrentLookUpConfig.PropertyName);
            prop.SetValue(account, item);

            viewModel.ShowLookUpModal = false;
        }

        //private async void LvLookUp_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        //{
        //    viewModel.LookUpLoading = true;
        //    var itemAppearing = e.Item as Portable.Models.LookUp;
        //    var lastItem = viewModel.LookUpData.LastOrDefault();
        //    if (lastItem != null && itemAppearing.Id.Equals(lastItem.Id))
        //    {
        //        viewModel.LookUpPage += 1;
        //        await viewModel.loadData();
        //    }
        //    viewModel.LookUpLoading = false;
        //}

        private void ClearPrimaryContact(object sender, EventArgs e)
        {
            viewModel.Account.PrimaryContact = null;
        }

        private void ClearLocalization_Clicked(object sender, EventArgs e)
        {
            string fieldName = (string)((Button)sender).CommandParameter;
            var account = viewModel.Account;
            PropertyInfo prop = account.GetType().GetProperty(fieldName);
            prop.SetValue(account, null);
        }

        private void primarycontactname_Focused(object sender, EventArgs e)
        {
            viewModel.CurrentLookUpConfig = viewModel.PrimaryContactConfig;
            viewModel.ProcessLookup(nameof(viewModel.PrimaryContactConfig));
        }

        private void url_unfocused(object sender, FocusEventArgs e)
        {
            //var txtUrl = (sender as Entry).Text;
            //if (!string.IsNullOrEmpty(txt_url.Text) && !string.IsNullOrWhiteSpace(txt_url.Text) )
            //{
            //    if(txt_url.Text.Contains("http://"))
            //    txt_url.Text = "http://" + txt_url.Text;
            //}
        }

        private async void ShowMore_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.PageQueueing++;
            await viewModel.LoadDSQueueingAccount(AccountId);
            viewModel.IsBusy = false;
        }

        private async void ShowMoreQuotation_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.PageQuotation++;
            await viewModel.LoadDSQuotationAccount(AccountId);
            viewModel.IsBusy = false;
        }

        private async void ShowMoreContract_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.PageContract++;
            await viewModel.LoadDSContractAccount(AccountId);
            viewModel.IsBusy = false;
        }

        private async void ShowMoreCase_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.PageCase++;
            await viewModel.LoadDSCaseAccount(AccountId);
            viewModel.IsBusy = false;
        }

        private async void ShowMoreActivities_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.PageActivities++;
            await viewModel.LoadDSActivitiesAccount(AccountId);
            viewModel.IsBusy = false;
        }

        private async void ShowMoreMandatory_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.PageMandatory++;
            await viewModel.Load_List_Mandatory_Secondary(AccountId.ToString());
            viewModel.IsBusy = false;
        }
    }
}