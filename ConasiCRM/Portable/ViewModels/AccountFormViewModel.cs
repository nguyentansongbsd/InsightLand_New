using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using ConasiCRM.Portable.Helper;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections;
using ConasiCRM.Portable.Config;
using System.Net.Http.Headers;
using System.Net;

using System.Diagnostics;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.ViewModels
{
    public class AccountFormViewModel : FormLookupViewModel
    {
        //public Account Account { get { return _Account; } set { _Account = value; OnPropertyChanged(nameof(Account)); } }
        //private Account _Account;

        public ObservableCollection<OptionSet> LoaiHinhOptions { get; set; }
        public ObservableCollection<string> SelectedLoaiHinh { get; set; }

        private AccountFormModel _singleAccount;
        public AccountFormModel singleAccount { get => _singleAccount; set { _singleAccount = value; OnPropertyChanged(nameof(singleAccount)); } }

        private ContactMandatoryPrimary _singleContactMandatoryPrimary;
        public ContactMandatoryPrimary singleContactMandatoryPrimary { get => _singleContactMandatoryPrimary; set { _singleContactMandatoryPrimary = value; OnPropertyChanged(nameof(singleContactMandatoryPrimary)); } }

        private ListCountry _singleListCountry;
        public ListCountry singleListCountry { get => _singleListCountry; set { _singleListCountry = value; OnPropertyChanged(nameof(singleListCountry)); } }

        private ListProvince _singleListProvince;
        public ListProvince singleListProvince { get => _singleListProvince; set { _singleListProvince = value; OnPropertyChanged(nameof(singleListProvince)); } }

        private ListDistrict _singleListDistrict;
        public ListDistrict singleListDistrict { get => _singleListDistrict; set { _singleListDistrict = value; OnPropertyChanged(nameof(singleListDistrict)); } }

        private ListQueueingAcc _singleListQueueingAcc;
        public ListQueueingAcc singleListQueueingAcc { get => _singleListQueueingAcc; set { _singleListQueueingAcc = value; OnPropertyChanged(nameof(singleListQueueingAcc)); } }

        private ListQuotationAcc _singleListQuotationAcc;
        public ListQuotationAcc singleListQuotationAcc { get => _singleListQuotationAcc; set { _singleListQuotationAcc = value; OnPropertyChanged(nameof(singleListQuotationAcc)); } }

        private ListContractAcc _singleListContractAcc;
        public ListContractAcc singleListContractAcc { get => _singleListContractAcc; set { _singleListContractAcc = value; OnPropertyChanged(nameof(singleListContractAcc)); } }

        private ListCaseAcc _singleListCaseAcc;
        public ListCaseAcc singleListCaseAcc { get => _singleListCaseAcc; set { _singleListCaseAcc = value; OnPropertyChanged(nameof(singleListCaseAcc)); } }

        private ListActivitiesAcc _singleListActivitiesAcc;
        public ListActivitiesAcc singleListActivitiesAcc { get => _singleListActivitiesAcc; set { _singleListActivitiesAcc = value; OnPropertyChanged(nameof(singleListActivitiesAcc)); } }

        public OptionSet _singleLocalization;
        public OptionSet singleLocalization { get => _singleLocalization; set { _singleLocalization = value; OnPropertyChanged(nameof(singleLocalization)); } }

        public OptionSet _singleCustomergroup;
        public OptionSet singleCustomergroup { get => _singleCustomergroup; set { _singleCustomergroup = value; OnPropertyChanged(nameof(singleCustomergroup)); } }

        public ObservableCollection<ContactMandatoryPrimary> list_lookup_primarycontactid { get; set; }
        public ObservableCollection<OptionSet> list_picker_bsd_customergroup { get; set; }
        public ObservableCollection<OptionSet> list_picker_bsd_localization { get; set; }

        public ObservableCollection<ListCountry> list_lookup_Country { get; set; }
        public ObservableCollection<ListProvince> list_lookup_Province { get; set; }
        public ObservableCollection<ListDistrict> list_lookup_District { get; set; }

        public ObservableCollection<ListQueueingAcc> list_thongtinqueing { get; set; }
        public ObservableCollection<ListQuotationAcc> list_thongtinquotation { get; set; }
        public ObservableCollection<ListContractAcc> list_thongtincontract { get; set; }
        public ObservableCollection<ListCaseAcc> list_thongtincase { get; set; }
        public ObservableCollection<ListActivitiesAcc> list_thongtinactivitie { get; set; }

        public int pageLookup_primary;
        public bool morelookup_primary;
        public int pageLookup_province;
        public bool morelookup_province;
        public int pageLookup_country;
        public bool morelookup_country;
        public int pageLookup_district;
        public bool morelookup_district;

        //--------------------------------------------------------------------//
        public ObservableCollection<OptionSet> BusinessTypeOptionList { get; set; }
        public ObservableCollection<OptionSet> LocalizationOptionList { get; set; }
        public ObservableCollection<OptionSet> CustomerGroupOptionList { get; set; }

        private Account _account;
        public Account Account
        {
            get { return _account; }
            set
            {
                if (_account != value)
                {
                    _account = value;
                    OnPropertyChanged(nameof(Account));
                }
            }
        }
        private LookUp _PrimaryContact;
        public LookUp PrimaryContact
        {
            get => _PrimaryContact;
            set
            {
                if (_PrimaryContact != value)
                { this._PrimaryContact = value; OnPropertyChanged(nameof(PrimaryContact)); }
            }
        }

        public LookUpConfig PrimaryContactConfig { get; set; }
        public LookUpConfig ProjectConfig { get; set; }

        private bool _showBusinessTypeModal;
        public bool ShowBusinessTypeModal
        {
            get { return _showBusinessTypeModal; }
            set
            {
                if (_showBusinessTypeModal != value)
                {
                    _showBusinessTypeModal = value;
                    OnPropertyChanged(nameof(ShowBusinessTypeModal));
                }
            }
        }

        private ObservableCollection<ProjectList> _list_Duanquantam;
        public ObservableCollection<ProjectList> list_Duanquantam { get { return _list_Duanquantam; } set { _list_Duanquantam = value; OnPropertyChanged(nameof(_list_Duanquantam)); } }

        public ObservableCollection<LookUp> list_popup_topic { get; set; }
        private AccountForm_CheckdataModel _list_check_data;
        public AccountForm_CheckdataModel list_check_data { get { return _list_check_data; } set { _list_check_data = value; OnPropertyChanged(nameof(list_check_data)); } }

        private bool _optionEntryHasOnlyTerminatedStatus;
        public bool optionEntryHasOnlyTerminatedStatus { get { return _optionEntryHasOnlyTerminatedStatus; } set { _optionEntryHasOnlyTerminatedStatus = value; OnPropertyChanged(nameof(optionEntryHasOnlyTerminatedStatus)); } }

        public ObservableCollection<MandatorySecondaryModel> list_MandatorySecondary { get; set; }

        public int PageQueueing { get; set; } = 1;
        public int PageQuotation { get; set; } = 1;
        public int PageContract { get; set; } = 1;
        public int PageCase { get; set; } = 1;
        public int PageActivities { get; set; } = 1;
        public int PageMandatory { get; set; } = 1;

        private bool _showMoreQueueing;
        public bool ShowMoreQueueing { get => _showMoreQueueing; set { _showMoreQueueing = value; OnPropertyChanged(nameof(ShowMoreQueueing)); } }

        private bool _showMoreQuotation;
        public bool ShowMoreQuotation { get => _showMoreQuotation; set { _showMoreQuotation = value; OnPropertyChanged(nameof(ShowMoreQuotation)); } }

        private bool _showMoreContract;
        public bool ShowMoreContract { get => _showMoreContract; set { _showMoreContract = value; OnPropertyChanged(nameof(ShowMoreContract)); } }

        private bool _showMoreCase;
        public bool ShowMoreCase { get => _showMoreCase; set { _showMoreCase = value; OnPropertyChanged(nameof(ShowMoreCase)); } }

        private bool _showMoreActivities;
        public bool ShowMoreActivities { get => _showMoreActivities; set { _showMoreActivities = value; OnPropertyChanged(nameof(ShowMoreActivities)); } }

        private bool _showMoreMandatory;
        public bool ShowMoreMandatory { get => _showMoreMandatory; set { _showMoreMandatory = value; OnPropertyChanged(nameof(ShowMoreMandatory)); } }

        public AccountFormViewModel()
        {
            SelectedLoaiHinh = new ObservableCollection<string>();

            list_lookup_primarycontactid = new ObservableCollection<ContactMandatoryPrimary>();
            list_lookup_Country = new ObservableCollection<ListCountry>();
            list_lookup_Province = new ObservableCollection<ListProvince>();
            list_lookup_District = new ObservableCollection<ListDistrict>();

            list_thongtinqueing = new ObservableCollection<ListQueueingAcc>();
            list_thongtinquotation = new ObservableCollection<ListQuotationAcc>();
            list_thongtincontract = new ObservableCollection<ListContractAcc>();
            list_thongtincase = new ObservableCollection<ListCaseAcc>();
            list_thongtinactivitie = new ObservableCollection<ListActivitiesAcc>();
            list_check_data = new AccountForm_CheckdataModel();
            list_MandatorySecondary = new ObservableCollection<MandatorySecondaryModel>();

            pageLookup_primary = 1;
            morelookup_primary = true;
            pageLookup_province = 1;
            morelookup_province = true;
            pageLookup_country = 1;
            morelookup_country = true;
            pageLookup_province = 1;
            morelookup_province = true;
            pageLookup_district = 1;
            morelookup_district = true;
            optionEntryHasOnlyTerminatedStatus = true;

            LoaiHinhOptions = new ObservableCollection<OptionSet>()
            {
                new OptionSet("100000000","Khách hàng"),
                new OptionSet("100000001","Cộng tác viên"),
                new OptionSet("100000002","Người được uỷ quyền"),
                new OptionSet("100000003","Người đại diện pháp lý")
            };

            BusinessTypeOptionList = new ObservableCollection<OptionSet>();
            BusinessTypeOptionList.Add(new OptionSet("100000000", "Customer"));
            BusinessTypeOptionList.Add(new OptionSet("100000001", "Partner"));
            BusinessTypeOptionList.Add(new OptionSet("100000002", "Sales Argents"));
            BusinessTypeOptionList.Add(new OptionSet("100000003", "Developer"));

            LocalizationOptionList = new ObservableCollection<OptionSet>();
            LocalizationOptionList.Add(new OptionSet("100000000", "Local"));
            LocalizationOptionList.Add(new OptionSet("100000001", "Foreigner"));

            CustomerGroupOptionList = new ObservableCollection<OptionSet>();
            CustomerGroupOptionList.Add(new OptionSet("100000000", "Ưu tiên (VIP)"));
            CustomerGroupOptionList.Add(new OptionSet("100000001", "An cư"));
            CustomerGroupOptionList.Add(new OptionSet("100000002", "Đầu tư"));
            CustomerGroupOptionList.Add(new OptionSet("100000003", "Đền bù"));


            _account = new Account();

            PrimaryContactConfig = new LookUpConfig()
            {
                FetchXml = @"<fetch version='1.0' count='15' page='{0}' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='contactid' alias='Id' />
                    <attribute name='fullname' alias='Name' />
                    <attribute name='createdon' alias='Detail' />
                    <order attribute='fullname' descending='false' />
                  </entity>
                </fetch>",
                EntityName = "contacts",
                PropertyName = "PrimaryContact",
                LookUpTitle = "Chọn người đại diện"

            };

            list_Duanquantam = new ObservableCollection<ProjectList>();
            ProjectConfig = new LookUpConfig()
            {
                FetchXml = @"<fetch version='1.0' count='15' page='{0}' output-format='xml-platform' mapping='logical' distinct='false'>
                    <entity name='bsd_project'>
                        <attribute name='bsd_projectid' alias='Id' />
                        <attribute name='bsd_name' alias='Name' />
                        <attribute name='createdon' />
                        <order attribute='bsd_name' descending='false' />
                      </entity>
                </fetch>",
                EntityName = "bsd_projects",
                PropertyName = "Project"
            };


            //////////////////////////////////////////////////////////////////

            Account = new Account();

            list_picker_bsd_customergroup = new ObservableCollection<OptionSet>()
            {
                new OptionSet { Val="100000000",Label = "Ưu tiên(VIP)"},
                new OptionSet { Val="100000001",Label = "An cư"},
                new OptionSet { Val="100000002",Label = "Đầu tư"},
                new OptionSet { Val="100000003",Label = "Đền bù"}
             };

            list_picker_bsd_localization = new ObservableCollection<OptionSet>()
            {
                new OptionSet { Val="100000000",Label = "Trong nước"},
                new OptionSet { Val="100000001",Label = "Nước ngoài"},
             };
        }

        public OptionSet getCustomergroup(string id)
        {
            singleCustomergroup = list_picker_bsd_customergroup.FirstOrDefault(x => x.Val == id);
            return singleCustomergroup;
        }

        public OptionSet getLocalization(string id)
        {
            singleLocalization = list_picker_bsd_localization.FirstOrDefault(x => x.Val == id);
            return singleLocalization;
        }

        public async Task LoadOneAccount(Guid accountid)
        {
            //Debug.WriteLine("abc5" + accountid);
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='account'>
                                    <all-attributes/>
                                    <order attribute='createdon' descending='true' />
                                    <link-entity name='contact' from='contactid' to='primarycontactid' visible='false' link-type='outer' alias='contacts'>
                                        <attribute name='bsd_fullname' alias='primarycontactname'/>
                                    </link-entity>
                                    <link-entity name='new_district' from='new_districtid' to='bsd_district' visible='false' link-type='outer' alias='new_districts'>
                                        <attribute name='new_name' alias='district_name' />
                                        <attribute name='bsd_nameen' alias='district_nameen' />
                                    </link-entity>
                                    <link-entity name='new_province' from='new_provinceid' to='bsd_province' visible='false' link-type='outer' alias='new_provinces'>
                                        <attribute name='new_name' alias='province_name' />
                                        <attribute name='bsd_nameen' alias='province_nameen' />
                                    </link-entity>
                                    <link-entity name='bsd_country' from='bsd_countryid' to='bsd_nation' visible='false' link-type='outer' alias='bsd_countrys'>
                                        <attribute name='bsd_name' alias='nation_name' />
                                        <attribute name='bsd_nameen' alias='nation_nameen' />
                                    </link-entity>
                                    <link-entity name='new_district' from='new_districtid' to='bsd_permanentdistrict' visible='false' link-type='outer' alias='new_permanentdistricts'>
                                      <attribute name='new_name' alias='permanentdistrict_name'/> 
                                      <attribute name='bsd_nameen' alias='permanentdistrict_nameen' />
                                    </link-entity>
                                    <link-entity name='new_province' from='new_provinceid' to='bsd_permanentprovince' visible='false' link-type='outer' alias='new_permanentprovinces'>
                                       <attribute name='new_name' alias='permanentprovince_name'/>
                                       <attribute name='bsd_nameen' alias='permanentprovince_nameen' />
                                    </link-entity>
                                    <link-entity name='bsd_country' from='bsd_countryid' to='bsd_permanentnation' visible='false' link-type='outer' alias='bsd_permanentcountrys'>
                                      <attribute name='bsd_name' alias='permanentnation_name'/>
                                      <attribute name='bsd_nameen' alias='permanentnation_nameen' />
                                    </link-entity>
                                    <filter type='and'>
                                        <condition attribute='accountid' operator='eq' value='{" + accountid + @"}' />
                                    </filter>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<AccountFormModel>>("accounts", fetch);
            var tmp = result.value.FirstOrDefault();
            if (tmp == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                await Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
            }

            this.singleAccount = tmp;
            PrimaryContact = new LookUp() { Id = Guid.Parse(tmp._primarycontactid_value), Name = tmp.primarycontactname };
            //System.Diagnostics.Debugger.Break();
        }

        public async Task LoadListMandatoryPrimary()
        {
            if (morelookup_primary)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_primary + @"' output-format='xml-platform' mapping='logical' distinct='false'>    
                                <entity name='contact'>      
                                    <attribute name='fullname' />      
                                    <attribute name='emailaddress1' />      
                                    <attribute name='ownerid' />      
                                    <attribute name='mobilephone' />      
                                    <attribute name='bsd_identitycardnumber' />      
                                    <attribute name='gendercode' />      
                                    <attribute name='statuscode' />      
                                    <attribute name='createdon' />      
                                    <attribute name='jobtitle' />      
                                    <attribute name='birthdate' />      
                                    <attribute name='bsd_fullname' />      
                                    <attribute name='bsd_diachi' />      
                                    <attribute name='contactid' />     
                                    <order attribute='createdon' descending='true' />      
                                    <filter type='and'>       
                                    <condition attribute='statecode' operator='eq' value='0' />      
                                    </filter>    
                                </entity>  
                            </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactMandatoryPrimary>>("contacts", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    morelookup_primary = false;
                    return;
                }

                var data = result.value;
                foreach (var item in data)
                {
                    list_lookup_primarycontactid.Add(new ContactMandatoryPrimary { Id = item.contactid, Name = item.fullname });
                }
            }
        }

        public async Task LoadListCountry()
        {
            if (morelookup_country)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_country + @"' output-format='xml-platform' mapping='logical' distinct='false'> 
                                <entity name='bsd_country'> 
                                    <attribute name='bsd_name' /> 
                                    <attribute name='bsd_nameen' />
                                    <attribute name='createdon' /> 
                                    <attribute name='bsd_shortname' /> 
                                    <attribute name='bsd_id' /> 
                                    <attribute name='bsd_priority' /> 
                                    <attribute name='bsd_countryname' /> 
                                    <attribute name='bsd_countryid' /> 
                                    <order attribute='createdon' descending='false' /> 
                                </entity> 
                              </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListCountry>>("bsd_countries", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    morelookup_country = false;
                    return;
                }

                var data = result.value;
                foreach (var item in data)
                {
                    list_lookup_Country.Add(new ListCountry { Id = item.bsd_countryid, Name = item.bsd_countryname, Nameen = item.bsd_nameen });
                }
            }
        }

        public async Task LoadListProvince()
        {
            if (morelookup_province)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_province + @"' output-format='xml-platform' mapping='logical' distinct='false'> 
                                <entity name='new_province'> 
                                    <attribute name='new_name' /> 
                                    <attribute name='bsd_nameen' /> 
                                    <attribute name='createdon' /> 
                                    <attribute name='new_id' /> 
                                    <attribute name='bsd_country' /> 
                                    <attribute name='bsd_priority' /> 
                                    <attribute name='bsd_provincename' /> 
                                    <attribute name='new_provinceid' /> 
                                    <order attribute='bsd_priority' descending='false' /> 
                                </entity> 
                              </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListProvince>>("new_provinces", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    morelookup_province = false;
                    return;
                }

                var data = result.value;
                foreach (var item in data)
                {
                    list_lookup_Province.Add(new ListProvince { Id = item.new_provinceid, Name = item.bsd_provincename, Nameen = item.bsd_nameen });
                }
            }
        }

        public async Task LoadListProvinceId(string provinceid)
        {
            if (morelookup_province)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_province + @"' output-format='xml-platform' mapping='logical' distinct='false'>
                        <entity name='new_province'>
                            <attribute name='new_name' />
                            <attribute name='bsd_nameen' />
                            <attribute name='createdon' />
                            <attribute name='bsd_priority' />
                            <attribute name='new_id' />
                            <attribute name='new_provinceid' />
                            <attribute name='bsd_provincename' />
                            <attribute name='bsd_country' />
                            <order attribute='bsd_priority' descending='false' />
                            <filter type='and'> <condition attribute='bsd_country' operator='eq' value='{" + provinceid + @"}' />
                            </filter>
                        </entity>
                    </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListProvince>>("new_provinces", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    morelookup_province = false;
                    return;
                }

                list_lookup_Province.Clear();
                var data = result.value;
                foreach (var item in data)
                {
                    list_lookup_Province.Add(new ListProvince { Id = item.new_provinceid, Name = item.bsd_provincename, Nameen = item.bsd_nameen });
                }
            }
        }

        public async Task LoadListDistrict()
        {
            if (morelookup_district)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_district + @"' output-format='xml-platform' mapping='logical' distinct='false'> 
                                <entity name='new_district'> 
                                    <attribute name='new_name' /> 
                                    <attribute name='bsd_nameen' /> 
                                    <attribute name='createdon' /> 
                                    <attribute name='new_longitude' /> 
                                    <attribute name='new_latitude' /> 
                                    <attribute name='new_id' /> 
                                    <attribute name='new_province' /> 
                                    <attribute name='new_districtid' /> 
                                    <order attribute='new_id' descending='false' /> 
                                </entity> 
                            </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListDistrict>>("new_districts", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    morelookup_district = false;
                    return;
                }

                var data = result.value;
                foreach (var item in data)
                {
                    list_lookup_District.Add(new ListDistrict { Id = item.new_districtid, Name = item.new_name, Nameen = item.bsd_nameen });
                }
            }
        }

        public async Task LoadListDistrictId(string districtid)
        {
            if (morelookup_district)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_district + @"' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='new_district'>
                                    <attribute name='new_districtid' />
                                    <attribute name='new_name' />
                                        <attribute name='bsd_nameen' />
                                        <attribute name='new_latitude' />
                                        <attribute name='new_longitude' />
                                        <attribute name='new_province' />
                                        <attribute name='createdon' />
                                        <order attribute='new_name' descending='false' />
                                        <filter type='and'> <condition attribute='new_province' operator='eq' value='{" + districtid + @"}' />
                                        </filter>
                                    </entity>
                                </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListDistrict>>("new_districts", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    morelookup_district = false;
                    return;
                }

                list_lookup_District.Clear();
                var data = result.value;
                foreach (var item in data)
                {
                    list_lookup_District.Add(new ListDistrict { Id = item.new_districtid, Name = item.new_name, Nameen = item.bsd_nameen });
                }
            }
        }

        public async Task LoadDSQueueingAccount(Guid accountid)
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageQueueing}' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='opportunity'>
                                    <all-attributes/>
                                    <order attribute='createdon' descending='true' />
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='bsd_projects'>
                                        <attribute name='bsd_name' alias='que_nameproject'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='accounts'>
                                        <attribute name='bsd_name' alias='que_nameaccount'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='contacts'>
                                        <attribute name='bsd_fullname' alias='que_namecontact'/>
                                    </link-entity>
                                    <filter type='and'>
                                        <condition attribute='parentaccountid' operator='eq' uitype='account' value='" + accountid + @"' />
                                    </filter>
                                </entity>
                             </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListQueueingAcc>>("opportunities", fetch);
            //if (result == null)
            //{
            //    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
            //    await Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
            //}
            var data = result.value;

            if (data.Count < 3)
            {
                ShowMoreQueueing = false;
            }
            else
            {
                ShowMoreQueueing = true;
            }

            if (data.Any())
            {
                foreach (var item in data)
                {
                    if (item.que_nameproject == null)
                    {
                        item.que_nameproject = " ";
                    }

                    if (item.que_nameaccount != null)
                    {
                        item.customerid = item.que_nameaccount;
                    }
                    else
                    {
                        item.customerid = item.que_namecontact;
                    }

                    list_thongtinqueing.Add(item);
                }
            }
        }

        public async Task LoadDSQuotationAccount(Guid accountid)
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageQuotation}' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='quote'>
                                <all-attributes/>
                                <order attribute='createdon' descending='true' />
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' visible='false' link-type='outer' alias='bsd_projects'>
                                    <attribute name='bsd_name' alias='quo_nameproject'/>
                                </link-entity>
                                <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='accounts'>
                                    <attribute name='bsd_name' alias='quo_nameaccount'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='contacts'>
                                    <attribute name='bsd_fullname' alias='quo_namecontact'/>
                                </link-entity>
                                <link-entity name='product' from='productid' to='bsd_unitno' visible='false' link-type='outer' alias='products'>
                                    <attribute name='name' alias='quo_nameproduct'/>
                                </link-entity>
                                <filter type='and'>
                                    <condition attribute='accountid' operator='eq' uitype='account' value='" + accountid + @"' />
                                </filter>
                            </entity>
                        </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListQuotationAcc>>("quotes", fetch);
            //if (result == null)
            //{
            //    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
            //    await Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
            //}
            var data = result.value;

            if (data.Count < 3)
            {
                ShowMoreQuotation = false;
            }
            else
            {
                ShowMoreQuotation = true;
            }
            if (data.Any())
            {
                foreach (var item in data)
                {
                    if (item.quo_nameproject == null)
                    {
                        item.quo_nameproject = " ";
                    }

                    if (item.quo_nameproduct == null)
                    {
                        item.quo_nameproduct = " ";
                    }

                    if (item.quo_nameaccount != null)
                    {
                        item.customerid = item.quo_nameaccount;
                    }
                    else
                    {
                        item.customerid = item.quo_namecontact;
                    }

                    list_thongtinquotation.Add(item);
                }
            }
        }

        public async Task LoadDSContractAccount(Guid accountid)
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageContract}' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='salesorder'>
                                <all-attributes/>
                                <order attribute='createdon' descending='true' />
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' >
                                    <attribute name='bsd_name' alias='contract_nameproject'/>
                                </link-entity>
                                <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' >
                                    <attribute name='bsd_name' alias='contract_nameaccount'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='contacts'>
                                    <attribute name='bsd_fullname' alias='contract_namecontact'/>
                                </link-entity>
                                <link-entity name='product' from='productid' to='bsd_unitnumber' visible='false' link-type='outer' alias='products'>
                                  <attribute name='name' alias='contract_nameproduct'/>
                                </link-entity>
                                <filter type='and'>
                                    <condition attribute='accountid' operator='eq' uitype='account' value='" + accountid + @"' />
                                </filter>
                            </entity>
                        </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListContractAcc>>("salesorders", fetch);
            //if (result == null)
            //{
            //    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
            //    await Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
            //}
            var data = result.value;

            if (data.Count < 3)
            {
                ShowMoreContract = false;
            }
            else
            {
                ShowMoreContract = true;
            }
            if (data.Any())
            {
                foreach (var item in data)
                {
                    if (item.contract_nameproject == null)
                    {
                        item.contract_nameproject = " ";
                    }

                    if (item.contract_nameproduct == null)
                    {
                        item.contract_nameproduct = " ";
                    }

                    if (item.contract_nameaccount != null)
                    {
                        item.customerid = item.contract_nameaccount;
                    }
                    else
                    {
                        item.customerid = item.contract_namecontact;
                    }
                    if (item.statuscode != 100000006) { optionEntryHasOnlyTerminatedStatus = false; }

                    list_thongtincontract.Add(item);

                }

            }
        }

        public async Task LoadDSCaseAccount(Guid accountid)
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageCase}' output-format='xml-platform' mapping='logical' distinct='false'>
                        <entity name='incident'>
                            <all-attributes/>
                            <order attribute='createdon' descending='true' />
                            <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='accounts'>
                                <attribute name='bsd_name' alias='case_nameaccount'/>
                            </link-entity>
                            <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='contacts'>
                                <attribute name='bsd_fullname' alias='case_nameaccontact'/>
                            </link-entity>
                            <filter type='and'>
                                <condition attribute='customerid' operator='eq' uitype='account' value='" + accountid + @"' />
                            </filter>
                        </entity>
                    </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListCaseAcc>>("incidents", fetch);
            //if (result == null)
            //{
            //    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
            //    await Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
            //}
            var data = result.value;
            if (data.Count < 3)
            {
                ShowMoreCase = false;
            }
            else
            {
                ShowMoreCase = true;
            }
            if (data.Any())
            {
                foreach (var item in data)
                {
                    if (item.case_nameaccount != null)
                    {
                        item.customerid = item.case_nameaccount;
                    }
                    else
                    {
                        item.customerid = item.case_nameaccontact;
                    }

                    list_thongtincase.Add(item);
                }
            }
        }

        public async Task LoadDSActivitiesAccount(Guid accountid)
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageActivities}' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='activitypointer'>
                                <all-attributes/>
                                <order attribute='createdon' descending='true' />
                                <order attribute='scheduledend' descending='true' />
                                <link-entity name='account' from='accountid' to='regardingobjectid' link-type='inner' alias='af'>
                                <filter type='and'><condition attribute='accountid' operator='eq' uitype='account' value='" + accountid + @"' />
                                </filter>
                                </link-entity>
                            </entity>
                        </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListActivitiesAcc>>("activitypointers", fetch);
            //if (result == null)
            //{
            //    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
            //    await Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
            //}
            var data = result.value;
            if (data.Count < 3)
            {
                ShowMoreActivities = false;
            }
            else
            {
                ShowMoreActivities = true;
            }
            if (data.Any())
            {
                foreach (var item in data)
                {
                    list_thongtinactivitie.Add(item);
                }
            }
        }

        public async Task<bool> Check_form_keydata(string bsd_vatregistrationnumber, string bsd_registrationcode, string accountid)
        {
            var fetchxml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='account'>
                                <attribute name='bsd_vatregistrationnumber' />
                                <attribute name='bsd_registrationcode' />
                                <attribute name='accountid' />
                                <order attribute='createdon' descending='true' />
                                <link-entity name='account' from='accountid' to='accountid' visible='false' link-type='outer' alias='has_account'>
                                        <attribute name='bsd_name' alias='bsd_account_name'/>
                                    </link-entity>
                                <filter type='and'>
                                  <filter type='or'>
                                    <filter type='and'>
                                      <condition attribute='accountid' operator='ne' value='{" + accountid + @"}' />
                                      <condition attribute='bsd_vatregistrationnumber' operator='eq' value='" + bsd_vatregistrationnumber + @"' />
                                    </filter>
                                    <filter type='and'>
                                      <condition attribute='bsd_registrationcode' operator='eq' value='" + bsd_registrationcode + @"' />
                                      <condition attribute='accountid' operator='ne' value='{" + accountid + @"}' />
                                    </filter>
                                  </filter>
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<AccountForm_CheckdataModel>>("accounts", fetchxml);
            var tmp = result.value.FirstOrDefault();
            if (tmp != null)
            {
                this.list_check_data = tmp;
                return false;
            }
            return true;
            //if (result == null)
        }

        //ADD Sub-grid "Mandatory Secondary"
        public async Task Load_List_Mandatory_Secondary(string accountid)
        {
            string fetchxml = $@"<fetch version='1.0' count='3' page='{PageMandatory}' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_mandatorysecondary'>
                                    <attribute name='bsd_mandatorysecondaryid' />
                                    <attribute name='bsd_name' />
                                    <attribute name='createdon' />
                                    <attribute name='statuscode' />
                                    <attribute name='ownerid' />
                                    <attribute name='bsd_jobtitlevn' />
                                    <attribute name='bsd_jobtitleen' />
                                    <attribute name='bsd_effectivedateto' />
                                    <attribute name='bsd_effectivedatefrom' />
                                    <attribute name='bsd_developeraccount' />
                                    <attribute name='bsd_contact' />
                                    <order attribute='bsd_name' descending='false' />
                                    <link-entity name='contact' from='contactid' to='bsd_contact' visible='false' link-type='outer' alias='contacts'>
                                        <attribute name='bsd_fullname' alias='bsd_contact_name'/>
                                    </link-entity>
                                    <filter type='and'>
                                      <condition attribute='bsd_developeraccount' operator='eq' value='{accountid}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<MandatorySecondaryModel>>("bsd_mandatorysecondaries", fetchxml);
            if (result == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }
            var data = result.value;
            ShowMoreMandatory = data.Count < 3 ? false : true;

            if (data.Any())
            {
                foreach (var x in data)
                {
                    x.bsd_developeraccount = singleAccount.bsd_name;
                    list_MandatorySecondary.Add(x);
                }
            }

        }
        //chưa co quan he nay
        //public async Task Load_DanhSachDuAn(string accountid)
        //{
        //    string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
        //                      <entity name='bsd_project'>
        //                        <attribute name='bsd_name' />
        //                        <attribute name='bsd_projectcode' />
        //                        <attribute name='bsd_landvalueofproject' />
        //                        <attribute name='bsd_esttopdate' />
        //                        <attribute name='bsd_acttopdate' />
        //                        <attribute name='bsd_projectid' />
        //                        <order attribute='bsd_name' descending='false' />
        //                        <filter type='and'>
        //                          <condition attribute='statecode' operator='eq' value='0' />
        //                        </filter>
        //                      </entity>
        //                    </fetch>";
        //    var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ProjectList>>("contacts(" + contactid + @")/bsd_contact_bsd_project", fetch);
        //    if (result == null)
        //    {
        //        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
        //        return;
        //    }

        //    foreach (var x in result.value)
        //    {
        //        list_Duanquantam.Add(x);
        //    }
        //}
    };
}