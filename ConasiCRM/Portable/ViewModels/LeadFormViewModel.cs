using System;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Helper;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using ConasiCRM.Portable.Settings;

namespace ConasiCRM.Portable.ViewModels
{
    public class LeadFormViewModel : BaseViewModel
    {
        public Guid LeadId { get; set; }

        private LeadFormModel _singleLead;
        public LeadFormModel singleLead { get => _singleLead; set { _singleLead = value; OnPropertyChanged(nameof(singleLead)); } }

        public ObservableCollection<OptionSet> list_currency_lookup { get; set; } = new ObservableCollection<OptionSet>();

        private OptionSet _selectedCurrency;
        public OptionSet SelectedCurrency { get => _selectedCurrency; set { _selectedCurrency = value; OnPropertyChanged(nameof(SelectedCurrency)); } }

        public ObservableCollection<OptionSet> list_industrycode_optionset { get; set; } = new ObservableCollection<OptionSet>();

        private OptionSet _industryCode;
        public OptionSet IndustryCode { get => _industryCode; set { _industryCode = value; OnPropertyChanged(nameof(IndustryCode)); } }

        public ObservableCollection<OptionSet> list_campaign_lookup { get; set; } = new ObservableCollection<OptionSet>();

        private OptionSet _campaign;
        public OptionSet Campaign { get => _campaign; set { _campaign = value; OnPropertyChanged(nameof(Campaign)); } }

        private string _addressComposite;
        public string AddressComposite { get => _addressComposite; set { _addressComposite = value; OnPropertyChanged(nameof(AddressComposite)); } }

        private LookUp _addressCountry;
        public LookUp AddressCountry
        {
            get => _addressCountry;
            set
            {
                _addressCountry = value;
                OnPropertyChanged(nameof(AddressCountry));
                AddressStateProvince = null;
                list_province_lookup.Clear();
            }
        }

        private string _addressPostalCode;
        public string AddressPostalCode { get => _addressPostalCode; set { _addressPostalCode = value; OnPropertyChanged(nameof(AddressPostalCode)); } }

        private LookUp _addressStateProvince;
        public LookUp AddressStateProvince
        {
            get => _addressStateProvince;
            set
            {
                _addressStateProvince = value;
                OnPropertyChanged(nameof(AddressStateProvince));
                AddressCity = null;
                list_district_lookup.Clear();
            }
        }

        private LookUp _addressCity;
        public LookUp AddressCity { get => _addressCity; set { _addressCity = value; OnPropertyChanged(nameof(AddressCity)); } }

        private string _addressLine3;
        public string AddressLine3 { get => _addressLine3; set { _addressLine3 = value; OnPropertyChanged(nameof(AddressLine3)); } }

        private string _addressLine2;
        public string AddressLine2 { get => _addressLine2; set { _addressLine2 = value; OnPropertyChanged(nameof(AddressLine2)); } }

        private string _addressLine1;
        public string AddressLine1 { get => _addressLine1; set { _addressLine1 = value; OnPropertyChanged(nameof(AddressLine1)); } }

        private string _revenue;
        public string Revenue { get => _revenue; set { _revenue = value; OnPropertyChanged(nameof(Revenue)); } }

        public ObservableCollection<LookUp> list_country_lookup { get; set; } = new ObservableCollection<LookUp>();
        public ObservableCollection<LookUp> list_province_lookup { get; set; } = new ObservableCollection<LookUp>();
        public ObservableCollection<LookUp> list_district_lookup { get; set; } = new ObservableCollection<LookUp>();

        public int pageLookup_country = 1;
        public int pageLookup_province = 1;
        public int pageLookup_district = 1;

        public LeadFormViewModel()
        {
            singleLead = new LeadFormModel();
            this.loadIndustrycode();
        }

        public async Task LoadOneLead()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                          <entity name='lead'>
                            <attribute name='lastname' />
                            <attribute name='companyname' />
                            <attribute name='subject' alias='bsd_topic_label'/>
                            <attribute name='statuscode' />
                            <attribute name='mobilephone' />
                            <attribute name='telephone1' />
                            <attribute name='jobtitle' />
                            <attribute name='websiteurl' />
                            <attribute name='address1_composite' />
                            <attribute name='address1_line1' />
                            <attribute name='address1_city' />
                            <attribute name='address1_stateorprovince' />
                            <attribute name='address1_postalcode' />
                            <attribute name='address1_country' />
                            <attribute name='description' />
                            <attribute name='industrycode' />
                            <attribute name='revenue' />
                            <attribute name='numberofemployees' />
                            <attribute name='sic' />
                            <attribute name='donotsendmm' />
                            <attribute name='emailaddress1' />
                            <attribute name='createdon' />
                            <attribute name='leadid' />
                            <order attribute='createdon' descending='true' />
                            <filter type='and'>
                                <condition attribute='leadid' operator='eq' value='{" + LeadId + @"}' />
                            </filter>
                            <link-entity name='transactioncurrency' from='transactioncurrencyid' to='transactioncurrencyid' visible='false' link-type='outer'>
                                <attribute name='currencyname'  alias='transactioncurrencyid_label'/>
                            </link-entity>
                            <link-entity name='campaign' from='campaignid' to='campaignid' visible='false' link-type='outer'>
                                <attribute name='name'  alias='campaignid_label'/>
                            </link-entity>
                            <filter type='and'>
                                     <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                                </filter>
                          </entity>
                        </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LeadFormModel>>("leads", fetch);
            var tmp = result.value.FirstOrDefault();
            if (result == null || tmp == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                await Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
            }

            this.singleLead = tmp;
        }

        public async Task<bool> updateLead()
        {
            string path = "/leads(" + LeadId + ")";
            singleLead.leadid = LeadId;
            var content = await this.getContent();
            CrmApiResponse result = await CrmHelper.PatchData(path, content);

            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<CrmApiResponse> createLead()
        {
            string path = "/leads";
            singleLead.leadid = Guid.NewGuid();
            var content = await this.getContent();
            CrmApiResponse result = await CrmHelper.PostData(path, content);
            return result;
        }

        public async Task<Boolean> DeletLookup(string fieldName, Guid leadId)
        {
            var result = await CrmHelper.SetNullLookupField("leads", leadId, fieldName);
            return result.IsSuccess;
        }

        private async Task<object> getContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["leadid"] = singleLead.leadid;
            data["subject"] = singleLead.bsd_topic_label;
            data["lastname"] = singleLead.lastname;
            data["mobilephone"] = singleLead.mobilephone;
            data["telephone1"] = singleLead.telephone1;
            data["jobtitle"] = singleLead.jobtitle;
            data["emailaddress1"] = singleLead.emailaddress1;
            data["companyname"] = singleLead.companyname;
            data["websiteurl"] = singleLead.websiteurl;
            data["address1_composite"] = singleLead.address1_composite;
            data["address1_line1"] = singleLead.address1_line1;
            data["address1_city"] = singleLead.address1_city;
            data["address1_stateorprovince"] = singleLead.address1_stateorprovince;
            data["address1_postalcode"] = singleLead.address1_postalcode;
            data["address1_country"] = singleLead.address1_country;
            data["description"] = singleLead.description;
            data["industrycode"] = singleLead.industrycode;
            if (!string.IsNullOrWhiteSpace(singleLead.revenue))
            {
                data["revenue"] = decimal.Parse(singleLead.revenue);
            }
            else
            {
                data["revenue"] = null;
            }
            if (!string.IsNullOrWhiteSpace(singleLead.numberofemployees))
            {
                data["numberofemployees"] = int.Parse(singleLead.numberofemployees);
            }
            else
            {
                data["numberofemployees"] = null;
            }
            
            data["sic"] = singleLead.sic;
            data["donotsendmm"] = singleLead.donotsendmm.ToString();
            data["lastusedincampaign"] = singleLead.lastusedincampaign.HasValue ? (DateTime.Parse(singleLead.lastusedincampaign.ToString()).ToLocalTime()).ToString("yyyy-MM-dd\"T\"HH:mm:ss\"Z\"") : null;

            if (singleLead._transactioncurrencyid_value == null)
            {
                await DeletLookup("transactioncurrencyid", singleLead.leadid);
            }
            else
            {
                data["transactioncurrencyid@odata.bind"] = "/transactioncurrencies(" + singleLead._transactioncurrencyid_value + ")"; /////Lookup Field
            }

            if (singleLead._campaignid_value == null)
            {
                await DeletLookup("CampaignId", singleLead.leadid);
            }
            else
            {
                data["campaignid@odata.bind"] = "/campaigns(" + singleLead._campaignid_value + ")"; /////Lookup Field
            }
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

        ///////////// CURRENCY LOOKUP AREA
        public async Task LoadCurrenciesForLookup()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='transactioncurrency'>
                                    <attribute name='transactioncurrencyid' alias='Val'/>
                                    <attribute name='currencyname' alias='Label'/>
                                    <order attribute='currencyname' descending='false' />
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("transactioncurrencies", fetch);
            if (result == null || result.value.Count == 0)
            {
                return;
            }

            foreach (var x in result.value)
            {
                list_currency_lookup.Add(x);
            }
        }

        //////// INDUSTRYCODE OPTIONSET AREA
        public void loadIndustrycode()
        {
            list_industrycode_optionset.Add(new OptionSet() { Val = ("1"), Label = "Kế toán", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("2"), Label = "Nông nghiệp và Trích xuất Tài nguyên Thiên nhiên Không Dầu", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("3"), Label = "In và Xuất bản Truyền thông", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("4"), Label = "Nhà môi giới", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("5"), Label = "Bán lẻ Dịch vụ Cấp nước trong Tòa nhà", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("6"), Label = "Dịch vụ Kinh doanh", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("7"), Label = "Tư vấn", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("8"), Label = "Dịch vụ Tiêu dùng", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("9"), Label = "Quản lý Thiết kế, Chỉ đạo và Quảng cáo", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("10"), Label = "Nhà phân phối, Người điều vận và Nhà chế biến", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("11"), Label = "Văn phòng và Phòng khám Bác sĩ", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("12"), Label = "Sản xuất Lâu bền", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("13"), Label = "Địa điểm Ăn Uống", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("14"), Label = "Bán lẻ Dịch vụ Giải trí", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("15"), Label = "Thuê và Cho thuê Thiết bị", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("16"), Label = "Tài chính", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("17"), Label = "Chế biến Thực phẩm và Thuốc lá", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("18"), Label = "Xử lý Dựa vào Nhiều vốn Chuyển về", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("19"), Label = "Sửa chữa và Bảo dưỡng Chuyển đến", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("20"), Label = "Bảo hiểm", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("21"), Label = "Dịch vụ Pháp lý", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("22"), Label = "Bán lẻ Hàng hóa Không lâu bền", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("23"), Label = "Dịch vụ Tiêu dùng Bên ngoài", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("24"), Label = "Trích xuất và Phân phối Hóa dầu", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("25"), Label = "Bán lẻ Dịch vụ", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("26"), Label = "Chi nhánh SIG", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("27"), Label = "Dịch vụ Xã hội", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("28"), Label = "Nhà thầu Giao dịch Bên ngoài Đặc biệt", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("29"), Label = "Bất động sản Đặc biệt", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("30"), Label = "Vận tải", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("31"), Label = "Tạo và Phân phối Tiện ích", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("32"), Label = "Bán lẻ Phương tiện", });
            list_industrycode_optionset.Add(new OptionSet() { Val = ("33"), Label = "Bán buôn", });
        }

        ////////// CAMPAIGN LOOKP AREA
        public async Task LoadCampainsForLookup()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='campaign'>
                                    <attribute name='name' alias='Label'/>
                                    <attribute name='campaignid' alias='Val'/>
                                    <order attribute='name' descending='true' />
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("campaigns", fetch);
            if (result == null || result.value.Count == 0) return;

            foreach (var x in result.value)
            {
                list_campaign_lookup.Add(x);
            }
        }

        public async Task<LookUp> LoadCountryByName()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_country'>
                                    <attribute name='bsd_countryname' alias='Name'/>
                                    <attribute name='bsd_countryid' alias='Id'/>
                                    <order attribute='bsd_countryname' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_countryname' operator='eq' value='" + singleLead.address1_country + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("bsd_countries", fetch);
            if (result != null && result.value.Count > 0)
            {
                LookUp country = new LookUp();
                country = result.value.FirstOrDefault();
                return country;
            }
            else
            {
                return null;
            }
        }

        public async Task LoadCountryForLookup()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_country'>
                                    <attribute name='bsd_countryname' alias='Name'/>
                                    <attribute name='bsd_countryid' alias='Id'/>
                                    <attribute name='bsd_nameen' alias='Detail'/>
                                    <order attribute='bsd_countryname' descending='false' />
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("bsd_countries", fetch);
            if (result == null || result.value.Count == 0) return;
            foreach (var x in result.value)
            {
                list_country_lookup.Add(x);
            }
        }

        public async Task<LookUp> LoadProvinceByName()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='new_province'>
                                    <attribute name='bsd_provincename' alias='Name'/>
                                    <attribute name='new_provinceid' alias='Id'/>
                                    <order attribute='bsd_provincename' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='bsd_country' operator='eq' value='" + AddressCountry.Id + @"' />
                                        <condition attribute='bsd_provincename' operator='eq' value='" + singleLead.address1_stateorprovince + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("new_provinces", fetch);
            if (result != null && result.value.Count > 0)
            {
                LookUp Province = new LookUp();
                Province = result.value.FirstOrDefault();
                return Province;
            }
            else
            {
                return null;
            }
        }

        public async Task loadProvincesForLookup()
        {
            if (AddressCountry == null) return;
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='new_province'>
                                    <attribute name='bsd_provincename' alias='Name'/>
                                    <attribute name='new_provinceid' alias='Id'/>
                                    <attribute name='bsd_nameen' alias='Detail'/>
                                    <order attribute='bsd_provincename' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_country' operator='eq' value='" + AddressCountry.Id + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("new_provinces", fetch);
            if (result == null || result.value.Count == 0) return;

            foreach (var x in result.value)
            {
                list_province_lookup.Add(x);
            }

        }

        public async Task<LookUp> LoadDistrictByName()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='new_district'>
                                <attribute name='new_name' alias='Name'/>
                                <attribute name='new_districtid' alias='Id'/>
                                <attribute name='bsd_nameen' alias='Detail'/>
                                <order attribute='new_name' descending='false' />
                                <filter type='and'>
                                    <condition attribute='new_province' operator='eq' value='" + AddressStateProvince.Id + @"' />
                                    <condition attribute='new_name' operator='eq' value='" + singleLead.address1_city + @"' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("new_districts", fetch);
            if (result != null && result.value.Count > 0)
            {
                LookUp District = new LookUp();
                District = result.value.FirstOrDefault();
                return District;
            }
            else
            {
                return null;
            }
        }

        public async Task loadDistrictForLookup()
        {
            if (AddressStateProvince == null) return;
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='new_district'>
                                <attribute name='new_name' alias='Name'/>
                                <attribute name='new_districtid' alias='Id'/>
                                <attribute name='bsd_nameen' alias='Detail'/>
                                <order attribute='new_name' descending='false' />
                                <filter type='and'>
                                  <condition attribute='new_province' operator='eq' value='" + AddressStateProvince.Id + @"' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("new_districts", fetch);
            if (result == null || result.value.Count ==0)return;

            foreach (var x in result.value)
            {
                list_district_lookup.Add(x);
            }

        }
    }
}