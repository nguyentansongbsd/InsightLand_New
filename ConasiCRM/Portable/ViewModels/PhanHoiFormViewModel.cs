using ConasiCRM.Portable.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ConasiCRM.Portable.Helper;
using System.Collections.Generic;
using ConasiCRM.Portable.Settings;

namespace ConasiCRM.Portable.ViewModels
{
    public class PhanHoiFormViewModel : BaseViewModel
    {
        private PhanHoiFormModel _singlePhanHoi;
        public PhanHoiFormModel singlePhanHoi { get => _singlePhanHoi; set { _singlePhanHoi = value; OnPropertyChanged(nameof(singlePhanHoi)); } }

        private List<OptionSet> _caseTypes;
        public List<OptionSet> CaseTypes { get => _caseTypes; set { _caseTypes = value;OnPropertyChanged(nameof(CaseTypes)); } }
        private List<OptionSet> _subjects;
        public List<OptionSet> Subjects { get => _subjects; set { _subjects = value; OnPropertyChanged(nameof(Subjects)); } }
        private List<OptionSet> _caseLienQuans;
        public List<OptionSet> CaseLienQuans { get => _caseLienQuans; set { _caseLienQuans = value; OnPropertyChanged(nameof(CaseLienQuans)); } }
        private List<OptionSet> _caseOrigins;
        public List<OptionSet> CaseOrigins { get => _caseOrigins; set { _caseOrigins = value; OnPropertyChanged(nameof(CaseOrigins)); } }
        private List<OptionSet> _projects;
        public List<OptionSet> Projects { get => _projects; set { _projects = value; OnPropertyChanged(nameof(Projects)); } }
        private List<OptionSet> _units;
        public List<OptionSet> Units { get => _units; set { _units = value; OnPropertyChanged(nameof(Units)); } }
        private List<OptionSet> _contacts;
        public List<OptionSet> Contacts { get => _contacts; set { _contacts = value; OnPropertyChanged(nameof(Contacts)); } }
        private List<OptionSet> _accounts;
        public List<OptionSet> Accounts { get => _accounts; set { _accounts = value; OnPropertyChanged(nameof(Accounts)); } }
        private List<OptionSet> _queues;
        public List<OptionSet> Queues { get => _queues; set { _queues = value; OnPropertyChanged(nameof(Queues)); } }
        private List<OptionSet> _quotes;
        public List<OptionSet> Quotes { get => _quotes; set { _quotes = value; OnPropertyChanged(nameof(Quotes)); } }
        private List<OptionSet> _optionEntries;
        public List<OptionSet> OptionEntries { get => _optionEntries; set { _optionEntries = value; OnPropertyChanged(nameof(OptionEntries)); } }

        private OptionSet _caseType;
        public OptionSet CaseType { get => _caseType; set { _caseType = value; OnPropertyChanged(nameof(CaseType)); } }
        private OptionSet _subject;
        public OptionSet Subject { get => _subject; set { _subject = value; OnPropertyChanged(nameof(Subject)); } }
        private OptionSet _caseLienQuan;
        public OptionSet CaseLienQuan { get => _caseLienQuan; set { _caseLienQuan = value; OnPropertyChanged(nameof(CaseLienQuan)); } }
        private OptionSet _caseOrigin;
        public OptionSet CaseOrigin { get => _caseOrigin; set { _caseOrigin = value; OnPropertyChanged(nameof(CaseOrigin)); } }
        private OptionSet _project;
        public OptionSet Project { get => _project; set { _project = value; OnPropertyChanged(nameof(Project)); } }
        private OptionSet _unit;
        public OptionSet Unit { get => _unit; set { _unit = value; OnPropertyChanged(nameof(Unit)); } }

        private List<List<OptionSet>> _allItemSourceCustomer;
        public List<List<OptionSet>> AllItemSourceCustomer { get => _allItemSourceCustomer; set { _allItemSourceCustomer = value; OnPropertyChanged(nameof(AllItemSourceCustomer)); } }
        private List<string> _tabsCustomer;
        public List<string> TabsCustomer { get=>_tabsCustomer; set { _tabsCustomer = value;OnPropertyChanged(nameof(TabsCustomer)); } }
        private OptionSet _customer;
        public OptionSet Customer { get => _customer; set { _customer = value; OnPropertyChanged(nameof(Customer)); } }

        private List<List<OptionSet>> _allItemSourceDoiTuong;
        public List<List<OptionSet>> AllItemSourceDoiTuong { get => _allItemSourceDoiTuong; set { _allItemSourceDoiTuong = value; OnPropertyChanged(nameof(AllItemSourceDoiTuong)); } }
        private List<string> _tabsDoiTuong;
        public List<string> TabsDoiTuong { get => _tabsDoiTuong; set { _tabsDoiTuong = value; OnPropertyChanged(nameof(TabsDoiTuong)); } }
        private OptionSet _doiTuong;
        public OptionSet DoiTuong { get => _doiTuong; set { _doiTuong = value; OnPropertyChanged(nameof(DoiTuong)); } }





        public OptionSet _singleOrigin;
        public OptionSet singleOrigin { get => _singleOrigin; set { _singleOrigin = value; OnPropertyChanged(nameof(singleOrigin)); } }
        private ListSubjectCase _singleListSubject;
        public ListSubjectCase singleListSubject { get => _singleListSubject; set { _singleListSubject = value; OnPropertyChanged(nameof(singleListSubject)); } }
        private ListUnitCase _singleListUnitCase;
        public ListUnitCase singleListUnitCase { get => _singleListUnitCase; set { _singleListUnitCase = value; OnPropertyChanged(nameof(singleListUnitCase)); } }
        private ListLienHeCase _singleListLienHeCase;
        public ListLienHeCase singleListLienHeCase { get => _singleListLienHeCase; set { _singleListLienHeCase = value; OnPropertyChanged(nameof(singleListLienHeCase)); } }

        public ObservableCollection<OptionSet> list_picker_caseorigincode { get; set; }
        public ObservableCollection<ListSubjectCase> list_lookup_Subject { get; set; }
        public ObservableCollection<PhanHoiFormModel> list_lookup { get; set; }
        public ObservableCollection<PhanHoiFormModel> list_account_lookup { get; set; }
        public ObservableCollection<PhanHoiFormModel> list_contact_lookup { get; set; }
        public ObservableCollection<ListUnitCase> list_unit_lookup { get; set; }
        public ObservableCollection<ListLienHeCase> list_lookup_lienhe { get; set; }
        public ObservableCollection<PhanHoiFormModel> list_lookup_status { get; set; }

        public int pageLookup_subject;
        public bool morelookup_subject;
        public int pageLookup_account;
        public bool morelookup_account;
        public int pageLookup_contact;
        public bool morelookup_contact;
        public int pageLookup_unit;
        public bool morelookup_unit;
        public int pageLookup_lienhe;
        public bool morelookup_lienhe;

        public PhanHoiFormViewModel()
        {
            singlePhanHoi = new PhanHoiFormModel();

            list_account_lookup = new ObservableCollection<PhanHoiFormModel>();
            list_contact_lookup = new ObservableCollection<PhanHoiFormModel>();
            list_lookup_Subject = new ObservableCollection<ListSubjectCase>();
            list_unit_lookup = new ObservableCollection<ListUnitCase>();
            list_lookup_lienhe = new ObservableCollection<ListLienHeCase>();

            list_lookup_status = new ObservableCollection<PhanHoiFormModel>()
            {
                new PhanHoiFormModel { Name="In Progress", Id="1"},
                new PhanHoiFormModel { Name="On Hold", Id="2"},
                new PhanHoiFormModel { Name="Waiting for Details", Id="3"},
                new PhanHoiFormModel { Name="Researching", Id="4"},
                new PhanHoiFormModel { Name="Canceled", Id="6"},
                new PhanHoiFormModel { Name="Merged", Id="2000"}
             };

            list_picker_caseorigincode = new ObservableCollection<OptionSet>()
            {
                new OptionSet { Val="1",Label = "Phone"},
                new OptionSet { Val="2",Label = "Email"},
                new OptionSet { Val="3",Label = "Web"},
                new OptionSet { Val="2483",Label = "Facebook"},
                new OptionSet { Val="3986",Label = "Twitter"},
             };

            pageLookup_subject = 1;
            morelookup_subject = true;
            pageLookup_account = 1;
            morelookup_account = true;
            pageLookup_contact = 1;
            morelookup_contact = true;
            pageLookup_unit = 1;
            morelookup_unit = true;
            pageLookup_lienhe = 1;
            morelookup_lienhe = true;
        }

        public async Task<bool> CreateCase()
        {
            string path = "/incidents";
            singlePhanHoi.incidentid = Guid.NewGuid();
            var content = await GetContent();
            CrmApiResponse result = await CrmHelper.PostData(path, content);
            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<object> GetContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["incidentid"] = singlePhanHoi.incidentid.ToString();
            data["casetypecode"] = CaseType.Val;
            data["title"] = singlePhanHoi.title;
            data["caseorigincode"] = CaseOrigin != null ? CaseOrigin.Val : null;
            data["description"] = singlePhanHoi.description;
            if (Subject == null)
            {
                await DeletLookup("subjectid", singlePhanHoi.incidentid);
            }
            else
            {
                data["subjectid@odata.bind"] = "/subjects(" + Subject.Val + ")"; 
            }

            if (CaseLienQuan == null)
            {
                await DeletLookup("parentcaseid", singlePhanHoi.incidentid);
            }
            else
            {
                data["parentcaseid@odata.bind"] = $"incidents({CaseLienQuan.Val})";
            }

            if (Customer == null)
            {
                await DeletLookup("customerid_account", singlePhanHoi.incidentid);
                await DeletLookup("customerid_contact", singlePhanHoi.incidentid);
            }
            else if(Customer.Title == "2") // account
            {
                data["customerid_account@odata.bind"] = "/accounts(" + Customer.Val + ")";
            }
            else if (Customer.Title == "1") // contact
            {
                data["customerid_contact@odata.bind"] = "/contacts(" + Customer.Val + ")";
            }

            if (Unit == null)
            {
                await DeletLookup("productid", singlePhanHoi.incidentid);
            }
            else
            {
                data["productid@odata.bind"] = "/products(" + Unit.Val + ")";
            }

            return data;
        }

        private async Task<Boolean> DeletLookup(string fieldName, Guid IncidentId)
        {
            var result = await CrmHelper.SetNullLookupField("incidents", IncidentId, fieldName);
            return result.IsSuccess;
        }

        public async Task LoadSubjects()
        {
            string fetchXml = @"<fetch version='1.0'  output-format='xml-platform' mapping='logical' distinct='false'>    
                                <entity name='subject'>      
                                    <attribute name='title' alias ='Label'/>
                                    <attribute name='subjectid' alias ='Val'/>
                                    <order attribute='createdon' descending='true' />
                                </entity>  
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("subjects", fetchXml);
            if (result == null || result.value.Count == 0) return;
            this.Subjects = result.value;
        }

        public async Task LoadCaseLienQuan()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='incident'>
                                    <attribute name='title' alias ='Label'/>
                                    <attribute name='incidentid' alias= 'Val'/>
                                    <order attribute='createdon' descending='false' />
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("incidents", fetchXml);
            if (result == null || result.value.Count == 0) return;
            this.CaseLienQuans = result.value;
        }

        public async Task LoadContacts()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='contact'>
                                    <attribute name='fullname' alias='Label'/>
                                    <attribute name='contactid' alias = 'Val' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='{UserLogged.Id}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("contacts", fetchXml);
            if (result == null || result.value.Count == 0) return;
            var data = result.value;
            foreach (var item in data)
            {
                item.Title = "1";
                this.Contacts.Add(item);
            }
        }

        public async Task LoadAccounts()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='account'>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='accountid' alias='Val'/>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='{UserLogged.Id}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("accounts", fetchXml);
            if (result == null || result.value.Count == 0) return;
            var data = result.value;
            foreach (var item in data)
            {
                item.Title = "2";
                this.Accounts.Add(item);
            }
        }

        public async Task LoadProjects()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_project'>
                                    <attribute name='bsd_projectid' alias='Val' />
                                    <attribute name='bsd_name' alias='Label' />
                                    <order attribute='createdon' descending='false' />
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_projects", fetchXml);
            if (result == null || result.value.Count == 0) return;
            this.Projects = result.value;
        }

        public async Task LoadUnits()
        {
            if (Project == null) return;
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='product'>
                                <attribute name='name' alias='Label'/>
                                <attribute name='productid' alias='Val'/>
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                  <condition attribute='bsd_projectcode' operator='eq'  uitype='bsd_project' value='{Project.Val}' />
                                </filter>
                              </entity>
                            </fetch>";
            var resutl = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("products", fetchXml);
            if (resutl == null || resutl.value.Count == 0) return ;
            this.Units = resutl.value;
        }

        public async Task LoadQueues()
        {
            if (Customer == null) return;
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='opportunity'>
                                    <attribute name='name' alias='Label'/>
                                    <attribute name='opportunityid' alias='Val'/>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_employee' operator='eq' uiname='ngsong' uitype='bsd_employee' value='{UserLogged.Id}' />
                                        <filter type='or'>
                                          <condition attribute='parentaccountid' operator='eq' uitype='account' value='{Customer.Val}' />
                                          <condition attribute='parentcontactid' operator='eq' uitype='contact' value='{Customer.Val}' />
                                        </filter>
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("opportunities", fetchXml);
            if (result == null || result.value.Count == 0) return;
            this.Queues = result.value;
        }

        public async Task LoadQuotes()
        {
            if (Customer == null) return;
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='quote'>
                                    <attribute name='name' alias='Label'/>
                                    <attribute name='quoteid' alias='Val' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='customerid' operator='eq' value='{Customer.Val}' />
                                      <condition attribute='bsd_employee' operator='eq' value ='{UserLogged.Id}'/>
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("quotes", fetchXml);
            if (result == null || result.value.Count == 0) return;
            this.Quotes = result.value;
        }

        public async Task LoadOptionEntries()
        {
            if (Customer == null) return;
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='salesorder'>
                                    <attribute name='name' alias='Label'/>
                                    <attribute name='salesorderid' alias='Val'/>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}'/>
                                      <condition attribute='customerid' operator='eq' value='{Customer.Val}' />
                                    </filter>
                                  </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("salesorders", fetchXml);
            if (result == null || result.value.Count == 0) return;
            this.OptionEntries = result.value;
        }




        public OptionSet getOrigin(string id)
        {
            singleOrigin = list_picker_caseorigincode.FirstOrDefault(x => x.Val == id);
            return singleOrigin;
        }

        public async Task LoadOnePhanHoi(Guid incidentid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='incident'>
                                    <all-attributes/>
                                  <order attribute='title' descending='false' />
                                  <filter type='and'>
                                      <condition attribute='incidentid' operator='eq'  value='{" + incidentid + @"}' />
                                  </filter>
                                  <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='accounts'>
                                  <attribute name='bsd_name' alias='case_nameaccount'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='contacts'>
                                  <attribute name='bsd_fullname' alias='case_namecontact'/>
                                </link-entity>
                                <link-entity name='product' from='productid' to='productid' visible='false' link-type='outer' alias='products'>
                                  <attribute name='name' alias='productname'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='primarycontactid' visible='false' link-type='outer'>
                                  <attribute name='fullname' alias='contactname'/>
                                </link-entity>
                                <link-entity name='contract' from='contractid' to='contractid' visible='false' link-type='outer' alias='contracts'>
                                  <attribute name='title' alias='contractname'/>
                                </link-entity>
                                <link-entity name='subject' from='subjectid' to='subjectid' visible='false' link-type='outer' >
                                  <attribute name='title' alias='subjecttitle'/>
                                </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhanHoiFormModel>>("incidents", fetch);
            var tmp = result.value.FirstOrDefault();
            if (tmp == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                await Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
            }

            this.singlePhanHoi = tmp;
            //System.Diagnostics.Debugger.Break();
        }
        public async Task LoadListSubject()
        {
            if (morelookup_subject)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_subject + @"' output-format='xml-platform' mapping='logical' distinct='false'>    
                                <entity name='subject'>      
                                    <all-attributes/>
                                    <order attribute='createdon' descending='true' />
                                </entity>  
                            </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListSubjectCase>>("subjects", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    morelookup_subject = false;
                    return;
                }

                var data = result.value;
                foreach (var item in data)
                {
                    list_lookup_Subject.Add(new ListSubjectCase { Id = item.subjectid, Name = item.title });
                }
            }
        }

        public async Task LoadListAcc()
        {
            if (morelookup_account)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_account + @"' output-format='xml-platform' mapping='logical' distinct='false'>    
                                <entity name='account'>      
                                    <all-attributes/>
                                    <order attribute='createdon' descending='true' />
                                </entity>  
                            </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhanHoiFormModel>>("accounts", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    morelookup_account = false;
                    return;
                }

                var data = result.value;
                foreach (var item in data)
                {
                    list_account_lookup.Add(new PhanHoiFormModel { Id = item.accountid, Name = item.bsd_name });
                }
            }
        }

        public async Task LoadListContact()
        {
            if (morelookup_contact)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_contact+ @"' output-format='xml-platform' mapping='logical' distinct='false'>    
                                <entity name='contact'>      
                                    <all-attributes/>
                                    <order attribute='createdon' descending='true' />
                                </entity>  
                            </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhanHoiFormModel>>("contacts", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    morelookup_contact = false;
                    return;
                }

                var data = result.value;
                foreach (var item in data)
                {
                    list_contact_lookup.Add(new PhanHoiFormModel { Id = item.contactid, Name = item.bsd_fullname });
                }
            }
        }

        public async Task LoadListUnit()
        {
            if (morelookup_unit)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_unit + @"' output-format='xml-platform' mapping='logical' distinct='false'>    
                                <entity name='product'>      
                                    <all-attributes/>
                                    <order attribute='createdon' descending='true' />
                                </entity>  
                            </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListUnitCase>>("products", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    morelookup_unit = false;
                    return;
                }

                var data = result.value;
                foreach (var item in data)
                {
                    list_unit_lookup.Add(new ListUnitCase { Id = item.productid, Name = item.name });
                }
            }
        }

        public async Task LoadListLienHe(string _customerid_value)
        {
            if (morelookup_lienhe)
            {
                if (_customerid_value != null)
                {
                    string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_lienhe + @"' output-format='xml-platform' mapping='logical' distinct='false'>    
                                        <entity name='contact'>
                                            <attribute name='fullname' />
                                            <attribute name='statuscode' />
                                            <attribute name='ownerid' />
                                            <attribute name='mobilephone' />
                                            <attribute name='jobtitle' />
                                            <attribute name='bsd_identitycardnumber' />
                                            <attribute name='gendercode' />
                                            <attribute name='emailaddress1' />
                                            <attribute name='createdon' />
                                            <attribute name='birthdate' />
                                            <attribute name='address1_composite' />
                                            <attribute name='bsd_fullname' />
                                            <attribute name='contactid' />
                                            <order attribute='createdon' descending='true' />
                                            <filter type='and'>
                                              <condition attribute='parentcustomerid' operator='eq' uitype='account' value='{" + _customerid_value + @"}' />
                                            </filter>
                                          </entity>
                                        </fetch>";
                    var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListLienHeCase>>("contacts", fetch);
                    if (result == null)
                    {
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                        return;
                    }
                    if (result.value.Count == 0)
                    {
                        morelookup_lienhe = false;
                        return;
                    }

                    var data = result.value;
                    foreach (var item in data)
                    {
                        list_lookup_lienhe.Add(new ListLienHeCase { Id = item.contactid, Name = item.bsd_fullname });
                    }
                }
            }
        }
    }
}

