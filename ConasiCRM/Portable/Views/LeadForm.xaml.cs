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
            this.Title = "TẠO MỚI KHÁCH HÀNG";
            Init();
            
        }
        public LeadForm(Guid Id)
        {
            InitializeComponent();
            this.Title = "CẬP NHẬT KHÁCH HÀNG";
            btn_save_lead.Text = "CẬP NHẬT KHÁCH HÀNG";
            Init();
            viewModel.LeadId = Id;
            InitUpdate();
        }

        public async void Init()
        {
            this.BindingContext = viewModel = new LeadFormViewModel();
            centerModalAddress.Body.BindingContext = viewModel;
            SetPreOpen();
            CheckSingleLead?.Invoke(true);
        }

        public async void InitUpdate()
        {
            await viewModel.LoadOneLead();
            viewModel.AddressComposite = viewModel.singleLead.address1_composite;
            viewModel.AddressLine1 = viewModel.singleLead.address1_line1;
            viewModel.AddressPostalCode = viewModel.singleLead.address1_postalcode;

            viewModel.IndustryCode = viewModel.list_industrycode_optionset.SingleOrDefault(x => x.Val == viewModel.singleLead.industrycode);

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

            if (viewModel.singleLead.leadid != Guid.Empty)
                CheckSingleLead?.Invoke(true);
            else
                CheckSingleLead?.Invoke(false);
        }

        public void SetPreOpen()
        {
            lookUpCurrency.PreOpenAsync = async () => {
                LoadingHelper.Show();
                await viewModel.LoadCurrenciesForLookup();
                if (viewModel.list_currency_lookup.Count == 0)
                {
                    ToastMessageHelper.ShortMessage("Không load được đơn vị tiền tệ");
                }
                LoadingHelper.Hide();
            };

            lookUpLinhVuc.PreOpenAsync = async () => {
                LoadingHelper.Show();
                viewModel.loadIndustrycode();
                LoadingHelper.Hide();
            };

            lookUpChienDich.PreOpenAsync = async () => {
                LoadingHelper.Show();
                await viewModel.LoadCampainsForLookup();
                if (viewModel.list_campaign_lookup.Count == 0)
                {
                    ToastMessageHelper.ShortMessage("Không load được chiến dịch");
                }
                LoadingHelper.Hide();
            };

            lookUpCountry.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadCountryForLookup();
                if (viewModel.list_country_lookup.Count == 0)
                {
                    ToastMessageHelper.ShortMessage("Không load được quốc gia");
                }
                LoadingHelper.Hide();
            };

            lookUpProvince.PreOpenAsync = async () => {
                LoadingHelper.Show();
                await viewModel.loadProvincesForLookup();
                if (viewModel.list_province_lookup.Count == 0)
                {
                    ToastMessageHelper.ShortMessage("Không load được tỉnh thành");
                }
                LoadingHelper.Hide();
            };

            lookUpDistrict.PreOpenAsync= async () => {
                LoadingHelper.Show();
                await viewModel.loadDistrictForLookup();
                if (viewModel.list_district_lookup.Count == 0)
                {
                    ToastMessageHelper.ShortMessage("Không load được quận/huyện");
                }
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
                ToastMessageHelper.ShortMessage("Vui lòng nhập số nhà/đường/phường");
                return;
            }

            List<string> address = new List<string>();
            if (!string.IsNullOrWhiteSpace(viewModel.AddressLine1))
            {
                viewModel.singleLead.address1_line1 = viewModel.AddressLine1;
                address.Add(viewModel.AddressLine1);
            }
            else
            {
                viewModel.singleLead.address1_line1 = null;
            }
            
            if (viewModel.AddressCity != null)
            {
                viewModel.singleLead.address1_city = viewModel.AddressCity.Name;
                address.Add(viewModel.AddressCity.Name);
            }
            else
            {
                viewModel.singleLead.address1_city = null;
            }
            if (viewModel.AddressStateProvince != null)
            {
                viewModel.singleLead.address1_stateorprovince = viewModel.AddressStateProvince.Name;
                address.Add(viewModel.AddressStateProvince.Name);
            }
            else
            {
                viewModel.singleLead.address1_stateorprovince = null;
            }
            if (!string.IsNullOrWhiteSpace(viewModel.AddressPostalCode))
            {
                viewModel.singleLead.address1_postalcode = viewModel.AddressPostalCode;
                address.Add(viewModel.AddressPostalCode);
            }
            else
            {
                viewModel.singleLead.address1_postalcode = null;
            }
            if (viewModel.AddressCountry != null)
            {
                viewModel.singleLead.address1_country = viewModel.AddressCountry.Name;
                address.Add(viewModel.AddressCountry.Name);
            }
            else
            {
                viewModel.singleLead.address1_country = null;
            }
            viewModel.singleLead.address1_composite = viewModel.AddressComposite = string.Join(",", address);
            await centerModalAddress.Hide();
        }

        private async void SaveLead_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.singleLead.bsd_topic_label))
            {
                ToastMessageHelper.ShortMessage("Vui lòng nhập tiêu đề");
                return;
            }

            if (string.IsNullOrWhiteSpace(viewModel.singleLead.fullname))
            {
                ToastMessageHelper.ShortMessage("Vui lòng nhập họ tên");
                return;
            }

            if (string.IsNullOrWhiteSpace(viewModel.singleLead.mobilephone))
            {
                ToastMessageHelper.ShortMessage("Vui lòng nhập số điện thoại");
                return;
            }

            if (!PhoneNumberFormatVNHelper.CheckValidate(viewModel.singleLead.mobilephone))
            {
                ToastMessageHelper.ShortMessage("Số điện thoại sai định dạng");
                return ;
            }

            LoadingHelper.Show();
            viewModel.singleLead.industrycode = viewModel.IndustryCode != null ? viewModel.IndustryCode.Val : null;
            viewModel.singleLead._transactioncurrencyid_value = viewModel.SelectedCurrency != null ? viewModel.SelectedCurrency.Val : null;
            viewModel.singleLead._campaignid_value = viewModel.Campaign != null ? viewModel.Campaign.Val : null;

            if (viewModel.LeadId == Guid.Empty)
            {
                var result = await viewModel.createLead();
                if (result.IsSuccess)
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Thành công");
                    await Navigation.PopAsync();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Không thêm được khách hàng. Vui lòng thử lại");
                }
            }
            else
            {
                bool IsSuccess = await viewModel.updateLead();
                if (IsSuccess)
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Thành công");
                    await Navigation.PopAsync();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Không cập nhật được khách hàng. Vui lòng thử lại");
                }
            }
        }
    }
}