using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ConasiCRM.Models;
using ConasiCRM.Portable.Controls;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Resources;
using ConasiCRM.Portable.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeadForm : ContentPage
    {
        public Action<bool> CheckSingleLead { get; set; }
        public LeadFormViewModel viewModel;

        public LeadForm()
        {
            InitializeComponent();
            this.Title = Language.tao_moi_khach_hang_tiem_nang_title;
            Init();
            datePickerNgaySinh.DefaultDisplay = DateTime.Now;
            viewModel.Rating = RatingData.GetRatingById("2");//mac dinh la warm
        }
        public LeadForm(Guid Id)
        {
            InitializeComponent();
            this.Title = Language.cap_nhat_khach_hang_tiem_nang_title;
            btn_save_lead.Text = Language.cap_nhat_khach_hang;
            Init();
            viewModel.LeadId = Id;
            InitUpdate();
        }

        public async void Init()
        {
            this.BindingContext = viewModel = new LeadFormViewModel();
            centerModalAddress.Body.BindingContext = viewModel;
            SetPreOpen();
            lookUpDanhGia.HideClearButton();
            CheckSingleLead?.Invoke(true);
        }

        public async void InitUpdate()
        {
            await viewModel.LoadOneLead();

            if (viewModel.singleLead.leadid != Guid.Empty)
            {
                viewModel.AddressComposite = viewModel.singleLead.address1_composite;
                viewModel.AddressLine1 = viewModel.singleLead.address1_line1;

                viewModel.IndustryCode = viewModel.list_industrycode_optionset.SingleOrDefault(x => x.Val == viewModel.singleLead.industrycode);
                viewModel.Rating = RatingData.GetRatingById(viewModel.singleLead.leadqualitycode.ToString());

                if (!string.IsNullOrWhiteSpace(viewModel.singleLead.new_gender))
                {
                    viewModel.Gender = viewModel.Genders.SingleOrDefault(x => x.Val == viewModel.singleLead.new_gender);
                }

                if (!string.IsNullOrWhiteSpace(viewModel.singleLead.leadsourcecode))
                {
                    viewModel.LeadSource = LeadSourcesData.GetLeadSourceById(viewModel.singleLead.leadsourcecode);
                }

                if (!viewModel.singleLead.new_birthday.HasValue)
                {
                    datePickerNgaySinh.DefaultDisplay = DateTime.Now;
                }

                if (!string.IsNullOrWhiteSpace(viewModel.singleLead._transactioncurrencyid_value))
                {
                    OptionSet currency = new OptionSet()
                    {
                        Val = viewModel.singleLead._transactioncurrencyid_value,
                        Label = viewModel.singleLead.transactioncurrencyid_label
                    };
                    viewModel.SelectedCurrency = currency;
                }

                if (!string.IsNullOrWhiteSpace(viewModel.singleLead._campaignid_value))
                {
                    OptionSet campaign = new OptionSet()
                    {
                        Val = viewModel.singleLead._campaignid_value,
                        Label = viewModel.singleLead.campaignid_label
                    };
                    viewModel.SelectedCurrency = campaign;
                }

                CheckSingleLead?.Invoke(true);
            }

            else
                CheckSingleLead?.Invoke(false);
        }

        public void SetPreOpen()
        {
            lookUpDanhGia.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.Ratings = RatingData.Ratings();
                LoadingHelper.Hide();
            };          

            lookUpLinhVuc.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.loadIndustrycode();
                LoadingHelper.Hide();
            };

            lookUpChienDich.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadCampainsForLookup();
                if (viewModel.list_campaign_lookup.Count == 0)
                {
                    ToastMessageHelper.ShortMessage(Language.khong_load_duoc_chien_dich);
                }
                LoadingHelper.Hide();
            };

            lookUpCountry.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadCountryForLookup();
                if (viewModel.list_country_lookup.Count == 0)
                {
                    ToastMessageHelper.ShortMessage(Language.khong_load_duoc_quoc_gia);
                }
                LoadingHelper.Hide();
            };

            lookUpProvince.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.loadProvincesForLookup();
                if (viewModel.list_province_lookup.Count == 0)
                {
                    ToastMessageHelper.ShortMessage(Language.khong_load_duoc_tinh_thanh);
                }
                LoadingHelper.Hide();
            };

            lookUpDistrict.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.loadDistrictForLookup();
                if (viewModel.list_district_lookup.Count == 0)
                {
                    ToastMessageHelper.ShortMessage(Language.khong_load_duoc_quan_huyen);
                }
                LoadingHelper.Hide();
            };

            lookUpLeadSource.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.LeadSources = LeadSourcesData.GetListSources();
                LoadingHelper.Hide();
            };
        }

        #region chua dung toi
        //private async Task<String> checkData()
        //{
        //    if (viewModel.singleLead._bsd_topic_value == null || string.IsNullOrWhiteSpace(viewModel.singleLead.fullname) || string.IsNullOrWhiteSpace(viewModel.singleLead.mobilephone))
        //    {
        //        return "Vui lòng nhập các trường bắt buộc";
        //    }

        //    if(!PhoneNumberFormatVNHelper.CheckValidate(viewModel.singleLead.mobilephone))
        //    {
        //        return "Số điện thoại sai định dạng";
        //    }

        //    if (viewModel.singleLead.new_birthday != null && (DateTime.Now.Year - DateTime.Parse(viewModel.singleLead.new_birthday.ToString()).Year < 18))
        //    {
        //        return "Khách hàng phải từ 18 tuổi";
        //    }

        //    //Kiem tra trùng tên - số điện thoại, tên - email
        //    await viewModel.Checkdata_identical_lock(viewModel.singleLead.fullname, viewModel.singleLead.mobilephone, viewModel.singleLead.emailaddress1, viewModel.singleLead.leadid);
        //    if (viewModel.single_Leadcheck != null)
        //    {
        //        if (viewModel.singleLead.fullname.Trim() == viewModel.single_Leadcheck.fullname && viewModel.singleLead.mobilephone == viewModel.single_Leadcheck.mobilephone)
        //        {
        //            return "Khách hàng - Số điện thoại đã tồn tại";
        //        }
        //        else if (viewModel.singleLead.fullname.Trim() == viewModel.single_Leadcheck.fullname && viewModel.singleLead.emailaddress1 == viewModel.single_Leadcheck.emailaddress1)
        //        {
        //            return "Khách hàng - Email đã tồn tại";
        //        }
        //    }
        //    return "Sucesses";
        //}

        //private void MyNewDatePicker_DateChanged(object sender, EventArgs e)
        //{
        //    if (viewModel.singleLead.new_birthday != null && (DateTime.Now.Year - DateTime.Parse(viewModel.singleLead.new_birthday.ToString()).Year < 18))
        //    {
        //        Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Lỗi", "Khách hàng phải từ 18 tuổi", "OK");
        //        viewModel.singleLead.new_birthday = null;
        //    }
        //    viewModel.PhongThuy.gioi_tinh = viewModel.singleLead.new_gender != null ? Int32.Parse(viewModel.singleLead.new_gender) : 0;
        //    viewModel.PhongThuy.nam_sinh = viewModel.singleLead.new_birthday.HasValue ? viewModel.singleLead.new_birthday.Value.Year : 0;
        //}

        #endregion

        private async void Address_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.AddressCountry == null && !string.IsNullOrWhiteSpace(viewModel.singleLead.address1_country))
            {
                viewModel.AddressCountry = await viewModel.LoadCountryByName();
            }

            if (viewModel.AddressStateProvince == null && !string.IsNullOrWhiteSpace(viewModel.singleLead.address1_stateorprovince))
            {
                viewModel.AddressStateProvince = await viewModel.LoadProvinceByName(); ;
            }

            if (viewModel.AddressCity == null && !string.IsNullOrWhiteSpace(viewModel.singleLead.address1_city))
            {
                viewModel.AddressCity = await viewModel.LoadDistrictByName();
            }

            await centerModalAddress.Show();
            LoadingHelper.Hide();
        }

        private async void CloseAddress_Clicked(object sender, EventArgs e)
        {
            await centerModalAddress.Hide();
        }

        private async void Country_Changed(object sender, EventArgs e)
        {
            await viewModel.loadProvincesForLookup();
        }

        private async void Province_Changed(object sender, EventArgs e)
        {
            await viewModel.loadDistrictForLookup();
        }

        private async void District_Changed(object sender, EventArgs e)
        {

        }

        private async void ConfirmAddress_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.AddressLine1))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_so_nha_duong_phuong);
                return;
            }

            List<string> address = new List<string>();
            if (!string.IsNullOrWhiteSpace(viewModel.AddressLine1))
            {
                address.Add(viewModel.AddressLine1);
            }

            if (viewModel.AddressCity != null)
            {
                address.Add(viewModel.AddressCity.Name);
            }

            if (viewModel.AddressStateProvince != null)
            {
                address.Add(viewModel.AddressStateProvince.Name);
            }           

            if (viewModel.AddressCountry != null)
            {
                address.Add(viewModel.AddressCountry.Name);
            }

            viewModel.AddressComposite = string.Join(",", address);
            await centerModalAddress.Hide();
        }

        private void ClearAddress_Tapped(object sender, EventArgs e)
        {
            viewModel.AddressComposite = null;
            viewModel.AddressLine1 = null;
            viewModel.AddressCity = null;
            viewModel.AddressStateProvince = null;
            viewModel.AddressCountry = null;

            viewModel.singleLead.address1_line1 = null;
            viewModel.singleLead.address1_city = null;
            viewModel.singleLead.address1_stateorprovince = null;
            viewModel.singleLead.address1_country = null;
            viewModel.singleLead.address1_composite = null;
        }

        private async void SaveLead_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.singleLead.bsd_topic_label))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_tieu_de);
                return;
            }

            if (string.IsNullOrWhiteSpace(viewModel.singleLead.lastname))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_ho_ten);
                return;
            }

            if (string.IsNullOrWhiteSpace(viewModel.singleLead.mobilephone))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_sdt);
                return;
            }

            if (viewModel.singleLead.new_birthday != null && (DateTime.Now.Year - DateTime.Parse(viewModel.singleLead.new_birthday.ToString()).Year < 18))
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_phai_tu_18_tuoi);
                return ;
            }

            LoadingHelper.Show();

            viewModel.singleLead.address1_city = viewModel.AddressCity != null ? viewModel.AddressCity.Name : null;
            viewModel.singleLead.address1_stateorprovince = viewModel.AddressStateProvince != null ? viewModel.AddressStateProvince.Name : null;
            viewModel.singleLead.address1_country = viewModel.AddressCountry != null ? viewModel.AddressCountry.Name : null;

            viewModel.singleLead.address1_line1 = viewModel.AddressLine1;
            viewModel.singleLead.address1_composite = viewModel.AddressComposite;

            viewModel.singleLead.industrycode = viewModel.IndustryCode != null ? viewModel.IndustryCode.Val : null;
            viewModel.singleLead._transactioncurrencyid_value = viewModel.SelectedCurrency != null ? viewModel.SelectedCurrency.Val : null;
            viewModel.singleLead._campaignid_value = viewModel.Campaign != null ? viewModel.Campaign.Val : null;

            if (viewModel.LeadId == Guid.Empty)
            {
                var result = await viewModel.createLead();
                if (result.IsSuccess)
                {
                    if (Dashboard.NeedToRefreshLeads.HasValue) Dashboard.NeedToRefreshLeads = true;
                    if (CustomerPage.NeedToRefreshLead.HasValue) CustomerPage.NeedToRefreshLead = true;
                    ToastMessageHelper.ShortMessage(Language.tao_moi_thanh_cong);
                    await Navigation.PopAsync();
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_them_duoc_khach_hang_vui_long_thu_lai);
                }
            }
            else
            {
                bool IsSuccess = await viewModel.updateLead();
                if (IsSuccess)
                {
                    if (CustomerPage.NeedToRefreshLead.HasValue) CustomerPage.NeedToRefreshLead = true;
                    if (LeadDetailPage.NeedToRefreshLeadDetail.HasValue) LeadDetailPage.NeedToRefreshLeadDetail = true;
                    await Navigation.PopAsync();
                    ToastMessageHelper.ShortMessage(Language.cap_nhat_thanh_cong);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_cap_nhat_duoc_khach_hang_vui_long_thu_lai);
                }
            }
        }
    }
}