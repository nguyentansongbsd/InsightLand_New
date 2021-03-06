using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class LeadDetailPageViewModel : BaseViewModel
    {
        public ObservableCollection<FloatButtonItem> ButtonCommandList { get; set; } = new ObservableCollection<FloatButtonItem>();
        private LeadFormModel _singleLead;
        public LeadFormModel singleLead { get => _singleLead; set { _singleLead = value; OnPropertyChanged(nameof(singleLead)); } }
        private OptionSet _singleGender;
        public OptionSet singleGender { get => _singleGender; set { _singleGender = value; OnPropertyChanged(nameof(singleGender)); } }
        private OptionSet _leadSource;
        public OptionSet LeadSource { get => _leadSource; set { _leadSource = value; OnPropertyChanged(nameof(LeadSource)); } }
        private OptionSet _singleIndustrycode;
        public OptionSet singleIndustrycode { get => _singleIndustrycode; set { _singleIndustrycode = value; OnPropertyChanged(nameof(singleIndustrycode)); } }

        private PhongThuyModel _PhongThuy;
        public PhongThuyModel PhongThuy { get => _PhongThuy; set { _PhongThuy = value; OnPropertyChanged(nameof(PhongThuy)); } }
        public ObservableCollection<OptionSet> list_gender_optionset { get; set; }
        public ObservableCollection<OptionSet> list_industrycode_optionset { get; set; }
        public ObservableCollection<HuongPhongThuy> list_HuongTot { set; get; }

        public ObservableCollection<HuongPhongThuy> list_HuongXau { set; get; }

        private string _address;
        public string Address { get => _address; set { _address = value; OnPropertyChanged(nameof(Address)); } }

        public bool IsSuccessContact { get; set; } = false;
        public bool IsSuccessAccount { get; set; } = false;
        private LookUp Country = new LookUp();
        private LookUp Province = new LookUp();
        private LookUp District = new LookUp();
        public int LeadStatusCode { get; set; }
        public int LeadStateCode { get; set; }
        public ObservableCollection<HoatDongListModel> list_case { get; set; }
        public int PageCase { get; set; } = 1;
        private bool _showMoreCase;
        public bool ShowMoreCase { get => _showMoreCase; set { _showMoreCase = value; OnPropertyChanged(nameof(ShowMoreCase)); } }
        public string CodeLead = Controls.LookUpMultipleTabs.CodeLead;
        public LeadDetailPageViewModel()
        {
            singleGender = new OptionSet();
            singleIndustrycode = new OptionSet();

            list_gender_optionset = new ObservableCollection<OptionSet>();
            list_industrycode_optionset = new ObservableCollection<OptionSet>();
            list_case = new ObservableCollection<HoatDongListModel>();
            list_HuongTot = new ObservableCollection<HuongPhongThuy>();
            list_HuongXau = new ObservableCollection<HuongPhongThuy>();

            this.loadGender();
            this.loadIndustrycode();
        }

        public async Task LoadOneLead(String leadid)
        {
            singleLead = new LeadFormModel();
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='lead'>
                                    <attribute name='lastname' />
                                    <attribute name='subject' alias='bsd_topic_label'/>
                                    <attribute name='statuscode' />
                                    <attribute name='statecode' />
                                    <attribute name='leadqualitycode' />
                                    <attribute name='mobilephone' />
                                    <attribute name='telephone1' />
                                    <attribute name='emailaddress1' />
                                    <attribute name='jobtitle' />  
                                    <attribute name='companyname' />
                                    <attribute name='websiteurl' />
                                    <attribute name='address1_composite' />
                                    <attribute name='description' />
                                    <attribute name='industrycode' />
                                    <attribute name='revenue' />
                                    <attribute name='numberofemployees' />
                                    <attribute name='sic' />
                                    <attribute name='donotsendmm' />
                                    <attribute name='lastusedincampaign' />
                                    <attribute name='createdon' />
                                    <attribute name='address1_line1' />
                                    <attribute name='address1_city' />
                                    <attribute name='address1_stateorprovince' />
                                    <attribute name='address1_postalcode' />
                                    <attribute name='address1_country' />
                                    <attribute name='new_gender' />
                                    <attribute name='new_birthday' />
                                    <attribute name='leadsourcecode' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                        <condition attribute='leadid' operator='eq' value='{" + leadid + @"}' />
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
                                    <link-entity name='account' from='originatingleadid' to='leadid' link-type='outer'>
                                       <attribute name='accountid' alias='account_id'/>
                                    </link-entity>
                                    <link-entity name='contact' from='originatingleadid' to='leadid' link-type='outer'>
                                       <attribute name='contactid' alias='contact_id'/>
                                    </link-entity>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LeadFormModel>>("leads", fetch);
            if (result == null)
            {
                return;
            }
            var tmp = result.value.FirstOrDefault();
            this.singleLead = tmp;
            this.singleGender = list_gender_optionset.SingleOrDefault(x => x.Val == this.singleLead.new_gender);
            this.LeadSource = LeadSourcesData.GetLeadSourceById(this.singleLead.leadsourcecode);
            LoadAddress();
            await LoadCountryByName();
        }
        public async Task<bool> Qualify(Guid id)
        {
            string path = "/leads(" + id + ")";
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
        public async Task<bool> UpdateStatusCodeLead()
        {
            string path = "/leads(" + this.singleLead.leadid + ")";
            var content = await GetContentUpdateStatusCode();
            CrmApiResponse apiResponse = await CrmHelper.PatchData(path, content);
            if (apiResponse.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private async Task<object> GetContentUpdateStatusCode()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["statuscode"] = this.LeadStatusCode.ToString();
            data["statecode"] = this.LeadStateCode.ToString();
            return data;
        }
        public async Task CreateContact()
        {
            string path = "/contacts";
            singleLead.contact_id = Guid.NewGuid();
            var content = await this.getContentContact();
            CrmApiResponse result = await CrmHelper.PostData(path, content);
            if (result.IsSuccess)
            {
                IsSuccessContact = true;
                await CreateAccount();
            }
            else
            {
                IsSuccessContact = false;
            }
        }
        public async Task CreateAccount()
        {
            if (!string.IsNullOrWhiteSpace(singleLead.companyname))
            {
                string path = "/accounts";
                var content = await getContentAccount();
                CrmApiResponse result = await CrmHelper.PostData(path, content);
                if (result.IsSuccess)
                {
                    IsSuccessAccount = true;
                }
                else
                {
                    IsSuccessAccount = false;
                }
            }
        }
        public void loadGender()
        {
            list_gender_optionset.Add(new OptionSet() { Val = ("1"), Label = "Nam", });
            list_gender_optionset.Add(new OptionSet() { Val = ("2"), Label = "Nữ", });
            list_gender_optionset.Add(new OptionSet() { Val = ("100000000"), Label = "Khác", });
        }
        public async Task<OptionSet> loadOneGender(string id)
        {
            this.singleGender = list_gender_optionset.FirstOrDefault(x => x.Val == id); ;
            return singleGender;
        }

        //////// INDUSTRYCODE OPTIONSET AREA
        /// ////
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

        public async Task<OptionSet> loadOneIndustrycode(string id)
        {
            this.singleIndustrycode = list_industrycode_optionset.FirstOrDefault(x => x.Val == id); ;
            return singleIndustrycode;
        }
        public void LoadPhongThuy()
        {
            PhongThuy = new PhongThuyModel();
            _ = loadOneGender(singleLead.new_gender);
            if (list_HuongTot != null || list_HuongXau != null)
            {
                list_HuongTot.Clear();
                list_HuongXau.Clear();
                if (singleLead != null && singleLead.new_gender != null && singleGender != null && singleGender.Val != null)
                {
                    PhongThuy.gioi_tinh = Int32.Parse(singleLead.new_gender);
                    PhongThuy.nam_sinh = singleLead.new_birthday.HasValue ? singleLead.new_birthday.Value.Year : 0;
                    if (PhongThuy.huong_tot != null && PhongThuy.huong_tot != null)
                    {
                        string[] huongtot = PhongThuy.huong_tot.Split('\n');
                        string[] huongxau = PhongThuy.huong_xau.Split('\n');
                        int i = 1;
                        foreach (var x in huongtot)
                        {
                            string[] huong = x.Split(':');
                            string name_huong = i + ". " + huong[0];
                            string detail_huong = huong[1].Remove(0, 1);
                            list_HuongTot.Add(new HuongPhongThuy { Name = name_huong, Detail = detail_huong });
                            i++;
                        }
                        int j = 1;
                        foreach (var x in huongxau)
                        {
                            string[] huong = x.Split(':');
                            string name_huong = j + ". " + huong[0];
                            string detail_huong = huong[1].Remove(0, 1);
                            list_HuongXau.Add(new HuongPhongThuy { Name = name_huong, Detail = detail_huong });
                            j++;
                        }
                    }
                }
                else
                {
                    PhongThuy.gioi_tinh = 0;
                    PhongThuy.nam_sinh = 0;
                }
            }
        }
        private void LoadAddress()
        {
            List<string> address = new List<string>();
            if (!string.IsNullOrWhiteSpace(singleLead.address1_line1))
            {
                address.Add(singleLead.address1_line1);
            }
            if (!string.IsNullOrWhiteSpace(singleLead.address1_city))
            {
                address.Add(singleLead.address1_city);
            }
            if (!string.IsNullOrWhiteSpace(singleLead.address1_stateorprovince))
            {
                address.Add(singleLead.address1_stateorprovince);
            }
            if (!string.IsNullOrWhiteSpace(singleLead.address1_postalcode))
            {
                address.Add(singleLead.address1_postalcode);
            }
            if (!string.IsNullOrWhiteSpace(singleLead.address1_country))
            {
                address.Add(singleLead.address1_country);
            }

            Address = string.Join(", ", address);
        }
        private async Task<object> getContentContact()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["contactid"] = singleLead.contact_id;
            data["lastname"] = singleLead.lastname;
            data["bsd_fullname"] = singleLead.lastname;
            data["emailaddress1"] = singleLead.emailaddress1;
            data["mobilephone"] = singleLead.mobilephone;
            data["bsd_jobtitlevn"] = singleLead.jobtitle;
            data["telephone1"] = singleLead.telephone1;
            data["birthdate"] = singleLead.new_birthday.HasValue ? (DateTime.Parse(singleLead.new_birthday.ToString()).ToUniversalTime()).ToString("yyyy-MM-dd") : null;
            data["gendercode"] = singleLead.new_gender;
            data["originatingleadid@odata.bind"] = "/leads(" + singleLead.leadid + ")";

            if (UserLogged.Id != null)
            {
                data["bsd_employee@odata.bind"] = "/bsd_employees(" + UserLogged.Id + ")";
            }
            if (UserLogged.ManagerId != Guid.Empty)
            {
                data["ownerid@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            }

            return data;
        }
        private async Task<object> getContentAccount()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            Guid accountid = Guid.NewGuid();
            data["accountid"] = accountid;
            data["bsd_name"] = singleLead.companyname;
            data["websiteurl"] = singleLead.websiteurl;
            if (!string.IsNullOrWhiteSpace(singleLead.numberofemployees))
            {
                data["numberofemployees"] = int.Parse(singleLead.numberofemployees);
            }
            else
            {
                data["numberofemployees"] = null;
            }
            if (singleLead.contact_id == Guid.Empty)
            {
                await DeletLookup("accounts", "primarycontactid", accountid);
            }
            else
            {
                data["primarycontactid@odata.bind"] = "/contacts(" + singleLead.contact_id + ")"; /////Lookup Field
            }
            data["lastusedincampaign"] = singleLead.lastusedincampaign.HasValue ? (DateTime.Parse(singleLead.lastusedincampaign.ToString()).ToLocalTime()).ToString("yyyy-MM-dd\"T\"HH:mm:ss\"Z\"") : null;
            data["industrycode"] = singleLead.industrycode;
            data["revenue"] = singleLead?.revenue;
            data["donotsendmm"] = singleLead.donotsendmm.ToString();
            data["sic"] = singleLead.sic;
            data["description"] = singleLead.description;
            data["address1_composite"] = singleLead.address1_composite;

            data["bsd_housenumberstreet"] = singleLead.address1_line1;

            data["bsd_postalcode"] = singleLead.address1_postalcode;
            if (singleLead._transactioncurrencyid_value == null)
            {
                await DeletLookup("accounts", "transactioncurrencyid", accountid);
            }
            else
            {
                data["transactioncurrencyid@odata.bind"] = "/transactioncurrencies(" + singleLead._transactioncurrencyid_value + ")"; /////Lookup Field
            }

            if (singleLead.address1_country == null || Country.Id == Guid.Empty)
            {
                await DeletLookup("accounts", "bsd_nation", accountid);
            }
            else
            {
                data["bsd_nation@odata.bind"] = "/bsd_countries(" + Country.Id + ")"; /////Lookup Field
            }
            if (singleLead.address1_stateorprovince == null || Province.Id == Guid.Empty)
            {
                await DeletLookup("accounts", "bsd_province", accountid);
            }
            else
            {
                data["bsd_province@odata.bind"] = "/new_provinces(" + Province.Id + ")"; /////Lookup Field
            }
            if (singleLead.address1_city == null || District.Id == Guid.Empty)
            {
                await DeletLookup("accounts", "bsd_district", accountid);
            }
            else
            {
                data["bsd_district@odata.bind"] = "/new_districts(" + District.Id + ")"; /////Lookup Field
            }
            data["originatingleadid@odata.bind"] = "/leads(" + singleLead.leadid + ")";

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
        public async Task<Boolean> DeletLookup(string Entity, string fieldName, Guid Id)
        {
            var result = await CrmHelper.SetNullLookupField(Entity, Id, fieldName);
            return result.IsSuccess;
        }
        private async Task<object> getContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["statecode"] = "1";
            return data;
        }
        public async Task LoadCountryByName()
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
                Country = result.value.FirstOrDefault();
                await LoadProvinceByName();
            }
        }
        public async Task LoadProvinceByName()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='new_province'>
                                    <attribute name='bsd_provincename' alias='Name'/>
                                    <attribute name='new_provinceid' alias='Id'/>
                                    <order attribute='bsd_provincename' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='bsd_country' operator='eq' value='" + Country.Id + @"' />
                                        <condition attribute='bsd_provincename' operator='eq' value='" + singleLead.address1_stateorprovince + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("new_provinces", fetch);
            if (result != null && result.value.Count > 0)
            {
                this.Province = result.value.FirstOrDefault();
                await LoadDistrictByName();
            }
        }
        public async Task LoadDistrictByName()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='new_district'>
                                <attribute name='new_name' alias='Name'/>
                                <attribute name='new_districtid' alias='Id'/>
                                <attribute name='bsd_nameen' alias='Detail'/>
                                <order attribute='new_name' descending='false' />
                                <filter type='and'>
                                    <condition attribute='new_province' operator='eq' value='" + Province.Id + @"' />
                                    <condition attribute='new_name' operator='eq' value='" + singleLead.address1_city + @"' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("new_districts", fetch);
            if (result != null && result.value.Count > 0)
            {
                this.District = result.value.FirstOrDefault();
            }
        }
        //
        // chăm sóc khách hàng
        //
        public async Task LoadCase(string leadid)
        {
            if (list_case != null && !string.IsNullOrWhiteSpace(leadid))
            {
                await Task.WhenAll(
                    LoadTasks(leadid),
                    LoadMettings(leadid),
                    LoadPhoneCalls(leadid)
                );
            }
            ShowMoreCase = list_case.Count < (3 * PageCase) ? false : true;
        }
        public async Task LoadTasks(string leadID)
        {
            string fetchXml = $@"<fetch version='1.0' count='3' page='{PageCase}' output-format='xml-platform' mapping='logical' distinct='true'>
                                <entity name='task'>
                                    <attribute name='subject' />
                                    <attribute name='statecode' />
                                    <attribute name='activityid' />
                                    <attribute name='scheduledstart' />
                                    <attribute name='scheduledend' /> 
                                    <attribute name='activitytypecode' /> 
                                    <order attribute='modifiedon' descending='true' />
                                    <filter type='and'>
                                        <filter type='or'>
                                            <condition entityname='party' attribute='partyid' operator='eq' value='{leadID}'/>
                                            <condition attribute='regardingobjectid' operator='eq' value='{leadID}' />
                                        </filter>
                                        <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='{UserLogged.Id}' />
                                    </filter>
                                    <link-entity name='activityparty' from='activityid' to='activityid' link-type='inner' alias='party'/>
                                    <link-entity name='account' from='accountid' to='regardingobjectid' link-type='outer' alias='ae'>
                                        <attribute name='bsd_name' alias='accounts_bsd_name'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='regardingobjectid' link-type='outer' alias='af'>
                                        <attribute name='fullname' alias='contact_bsd_fullname'/>
                                    </link-entity>
                                    <link-entity name='lead' from='leadid' to='regardingobjectid' link-type='outer' alias='ag'>
                                        <attribute name='fullname' alias='lead_fullname'/>
                                    </link-entity>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<HoatDongListModel>>("tasks", fetchXml);
            if (result == null || result.value.Count == 0) return;

            foreach (var item in result.value)
            {
                if (!string.IsNullOrWhiteSpace(item.contact_bsd_fullname))
                {
                    item.customer = item.contact_bsd_fullname;
                }
                if (!string.IsNullOrWhiteSpace(item.accounts_bsd_name))
                {
                    item.customer = item.accounts_bsd_name;
                }
                if (!string.IsNullOrWhiteSpace(item.lead_fullname))
                {
                    item.customer = item.lead_fullname;
                }

                this.list_case.Add(item);
            }
        }
        public async Task LoadMettings(string leadID)
        {
            string fetchXml = $@"<fetch version='1.0' count='3' page='{PageCase}' output-format='xml-platform' mapping='logical' distinct='true'>
                                <entity name='appointment'>
                                    <attribute name='subject' />
                                    <attribute name='statecode' />
                                    <attribute name='activityid' />
                                    <attribute name='scheduledstart' />
                                    <attribute name='scheduledend' /> 
                                    <attribute name='activitytypecode' /> 
                                    <order attribute='modifiedon' descending='true' />
                                    <filter type='and'>
                                        <filter type='or'>
                                            <condition entityname='party' attribute='partyid' operator='eq' value='{leadID}'/>
                                            <condition attribute='regardingobjectid' operator='eq' value='{leadID}' />
                                        </filter>
                                        <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='{UserLogged.Id}' />
                                    </filter>
                                    <link-entity name='activityparty' from='activityid' to='activityid' link-type='inner' alias='party'/>
                                    <link-entity name='account' from='accountid' to='regardingobjectid' link-type='outer' alias='ae'>
                                        <attribute name='bsd_name' alias='accounts_bsd_name'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='regardingobjectid' link-type='outer' alias='af'>
                                        <attribute name='fullname' alias='contact_bsd_fullname'/>
                                    </link-entity>
                                    <link-entity name='lead' from='leadid' to='regardingobjectid' link-type='outer' alias='ag'>
                                        <attribute name='fullname' alias='lead_fullname'/>
                                    </link-entity>
                                    <link-entity name='activityparty' from='activityid' to='activityid' link-type='outer' alias='aee'>
                                        <filter type='and'>
                                            <condition attribute='participationtypemask' operator='eq' value='5' />
                                        </filter>
                                        <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='aff'>
                                            <attribute name='fullname' alias='callto_contact_name'/>
                                        </link-entity>
                                        <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='agg'>
                                            <attribute name='bsd_name' alias='callto_account_name'/>
                                        </link-entity>
                                        <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='ahh'>
                                            <attribute name='fullname' alias='callto_lead_name'/>
                                        </link-entity>
                                    </link-entity>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<HoatDongListModel>>("appointments", fetchXml);
            if (result == null || result.value.Count == 0) return;

            foreach (var item in result.value)
            {
                var meet = list_case.FirstOrDefault(x => x.activityid == item.activityid);
                if (meet != null)
                {
                    if (!string.IsNullOrWhiteSpace(item.callto_contact_name))
                    {
                        string new_customer = ", " + item.callto_contact_name;
                        meet.customer += new_customer;
                    }
                    if (!string.IsNullOrWhiteSpace(item.callto_account_name))
                    {
                        string new_customer = ", " + item.callto_account_name;
                        meet.customer += new_customer;
                    }
                    if (!string.IsNullOrWhiteSpace(item.callto_lead_name))
                    {
                        string new_customer = ", " + item.callto_lead_name;
                        meet.customer += new_customer;
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(item.callto_contact_name))
                    {
                        item.customer = item.callto_contact_name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.callto_account_name))
                    {
                        item.customer = item.callto_account_name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.callto_lead_name))
                    {
                        item.customer = item.callto_lead_name;
                    }
                    this.list_case.Add(item);
                }
            }
        }
        public async Task LoadPhoneCalls(string leadID)
        {
            string fetchXml = $@"<fetch version='1.0' count='3' page='{PageCase}' output-format='xml-platform' mapping='logical' distinct='true'>
                                <entity name='phonecall'>
                                    <attribute name='subject' />
                                    <attribute name='statecode' />
                                    <attribute name='activityid' />
                                    <attribute name='scheduledstart' />
                                    <attribute name='scheduledend' /> 
                                    <attribute name='activitytypecode' /> 
                                    <order attribute='modifiedon' descending='true' />
                                    <filter type='and'>
                                        <filter type='or'>
                                            <condition entityname='party' attribute='partyid' operator='eq' value='{leadID}'/>
                                            <condition attribute='regardingobjectid' operator='eq' value='{leadID}' />
                                        </filter>
                                        <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='{UserLogged.Id}' />
                                    </filter>
                                    <link-entity name='activityparty' from='activityid' to='activityid' link-type='inner' alias='party'/>
                                    <link-entity name='account' from='accountid' to='regardingobjectid' link-type='outer' alias='ae'>
                                        <attribute name='bsd_name' alias='accounts_bsd_name'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='regardingobjectid' link-type='outer' alias='af'>
                                        <attribute name='fullname' alias='contact_bsd_fullname'/>
                                    </link-entity>
                                    <link-entity name='lead' from='leadid' to='regardingobjectid' link-type='outer' alias='ag'>
                                        <attribute name='fullname' alias='lead_fullname'/>
                                    </link-entity>
                                    <link-entity name='activityparty' from='activityid' to='activityid' link-type='outer' alias='aee'>
                                        <filter type='and'>
                                            <condition attribute='participationtypemask' operator='eq' value='2' />
                                        </filter>
                                        <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='aff'>
                                            <attribute name='fullname' alias='callto_contact_name'/>
                                        </link-entity>
                                        <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='agg'>
                                            <attribute name='bsd_name' alias='callto_account_name'/>
                                        </link-entity>
                                        <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='ahh'>
                                            <attribute name='fullname' alias='callto_lead_name'/>
                                        </link-entity>
                                    </link-entity>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<HoatDongListModel>>("phonecalls", fetchXml);
            if (result == null || result.value.Count == 0) return;

            foreach (var item in result.value)
            {
                if (!string.IsNullOrWhiteSpace(item.callto_contact_name))
                {
                    item.customer = item.callto_contact_name;
                }
                if (!string.IsNullOrWhiteSpace(item.callto_account_name))
                {
                    item.customer = item.callto_account_name;
                }
                if (!string.IsNullOrWhiteSpace(item.callto_lead_name))
                {
                    item.customer = item.callto_lead_name;
                }

                this.list_case.Add(item);
            }
        }
    }
}
