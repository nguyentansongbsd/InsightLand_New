using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class AccountDetailPageViewModel : FormViewModal
    {
        public ObservableCollection<FloatButtonItem> ButtonCommandList { get; set; } = new ObservableCollection<FloatButtonItem>();
        public ObservableCollection<OptionSet> BusinessTypeOptions { get; set; }

        private string _businessTypes;
        public string BusinessTypes { get => _businessTypes; set { _businessTypes = value; OnPropertyChanged(nameof(BusinessTypes)); } }

        private string _localization;
        public string Localization { get => _localization; set { _localization = value; OnPropertyChanged(nameof(Localization)); } }

        private AccountFormModel _singleAccount;
        public AccountFormModel singleAccount { get => _singleAccount; set { _singleAccount = value; OnPropertyChanged(nameof(singleAccount)); } }

        private ContactFormModel _PrimaryContact;
        public ContactFormModel PrimaryContact { get => _PrimaryContact; set { if (_PrimaryContact != value) { this._PrimaryContact = value; OnPropertyChanged(nameof(PrimaryContact)); } } }

        public ObservableCollection<ListQueueingAcc> list_thongtinqueing { get; set; }
        public ObservableCollection<ListQuotationAcc> list_thongtinquotation { get; set; }
        public ObservableCollection<ListContractAcc> list_thongtincontract { get; set; }
        public ObservableCollection<ListCaseAcc> list_thongtincase { get; set; }
        public ObservableCollection<MandatorySecondaryModel> list_MandatorySecondary { get; set; }

        public int PageQueueing { get; set; } = 1;
        public int PageQuotation { get; set; } = 1;
        public int PageContract { get; set; } = 1;
        public int PageCase { get; set; } = 1;
        public int PageMandatory { get; set; } = 1;

        private bool _showMoreQueueing;
        public bool ShowMoreQueueing { get => _showMoreQueueing; set { _showMoreQueueing = value; OnPropertyChanged(nameof(ShowMoreQueueing)); } }

        private bool _showMoreQuotation;
        public bool ShowMoreQuotation { get => _showMoreQuotation; set { _showMoreQuotation = value; OnPropertyChanged(nameof(ShowMoreQuotation)); } }

        private bool _showMoreContract;
        public bool ShowMoreContract { get => _showMoreContract; set { _showMoreContract = value; OnPropertyChanged(nameof(ShowMoreContract)); } }

        private bool _showMoreCase;
        public bool ShowMoreCase { get => _showMoreCase; set { _showMoreCase = value; OnPropertyChanged(nameof(ShowMoreCase)); } }

        private bool _showMoreMandatory;
        public bool ShowMoreMandatory { get => _showMoreMandatory; set { _showMoreMandatory = value; OnPropertyChanged(nameof(ShowMoreMandatory)); } }

        public AccountDetailPageViewModel()
        {
            BusinessTypeOptions = new ObservableCollection<OptionSet>();
            BusinessTypeOptions.Add(new OptionSet("100000000", "Customer"));
            BusinessTypeOptions.Add(new OptionSet("100000001", "Partner"));
            BusinessTypeOptions.Add(new OptionSet("100000002", "Sales Argents"));
            BusinessTypeOptions.Add(new OptionSet("100000003", "Developer"));

            list_thongtinqueing = new ObservableCollection<ListQueueingAcc>();
            list_thongtinquotation = new ObservableCollection<ListQuotationAcc>();
            list_thongtincontract = new ObservableCollection<ListContractAcc>();
            list_thongtincase = new ObservableCollection<ListCaseAcc>();
            list_MandatorySecondary = new ObservableCollection<MandatorySecondaryModel>();
        }

        //tab thong tin
        public async Task LoadOneAccount(string accountid)
        {
            singleAccount = new AccountFormModel();
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='account'>
                                <attribute name='primarycontactid' />
                                <attribute name='telephone1' />
                                <attribute name='bsd_rocnumber2' />
                                <attribute name='bsd_rocnumber1' />
                                <attribute name='websiteurl' />
                                <attribute name='bsd_vatregistrationnumber' />
                                <attribute name='bsd_incorporatedate' />
                                <attribute name='bsd_hotlines' />
                                <attribute name='bsd_generalledgercompanynumber' />
                                <attribute name='fax' />
                                <attribute name='emailaddress1' />
                                <attribute name='bsd_email2' />
                                <attribute name='bsd_placeofissue' />
                                <attribute name='bsd_issuedon' />
                                <attribute name='bsd_permanentaddress1' />
                                <attribute name='bsd_groupgstregisttationnumber' />
                                <attribute name='statuscode' />
                                <attribute name='ownerid' />
                                <attribute name='createdon' />
                                <attribute name='address1_composite' alias='bsd_address' />
                                <attribute name='bsd_companycode' />
                                <attribute name='bsd_registrationcode' />
                                <attribute name='bsd_accountnameother' />
                                <attribute name='bsd_localization' />
                                <attribute name='bsd_name' />
                                <attribute name='name' />
                                <attribute name='accountid' />
                                <attribute name='bsd_businesstypesys' />
                                <order attribute='createdon' descending='true' />
                                    <link-entity name='contact' from='contactid' to='primarycontactid' visible='false' link-type='outer' alias='contacts'>
                                        <attribute name='bsd_fullname' alias='primarycontactname'/>
                                        <attribute name='mobilephone' alias='primarycontacttelephohne'/>
                                        <attribute name='bsd_contactaddress' alias='primarycontactaddress'/>
                                        <attribute name='bsd_permanentaddress1' alias='primarycontactpermanentaddress'/>
                                    </link-entity>
                                    <filter type='and'>
                                      <condition attribute='accountid' operator='eq' value='" + accountid + @"' />
                                    </filter>
                                    <filter type='and'>
                                          <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                                    </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<AccountFormModel>>("accounts", fetch);
            if (result == null)
                return;
            var tmp = result.value.FirstOrDefault();
            if (tmp == null)
            {
                return;
            }

            singleAccount = tmp;
            singleAccount.bsd_address = LoadAddress(tmp.bsd_address);
            singleAccount.bsd_permanentaddress1 = LoadAddress(tmp.bsd_permanentaddress1);
            PrimaryContact = new ContactFormModel()
            {
                contactid = tmp._primarycontactid_value
                ,
                bsd_fullname = tmp.primarycontactname
                ,
                mobilephone = tmp.primarycontacttelephohne
                ,
                bsd_contactaddress = LoadAddress(tmp.primarycontactaddress)
                ,
                bsd_permanentaddress1 = LoadAddress(tmp.primarycontactpermanentaddress)
            };
        }

        public void GetTypeById(string loai)
        {
            if (loai != string.Empty)
            {
                List<string> listType = new List<string>();
                var ids = singleAccount.bsd_businesstypesys.Split(',').ToList();
                foreach (var item in ids)
                {
                    OptionSet optionSet = BusinessTypeOptions.Single(x => x.Val == item);
                    listType.Add(optionSet.Label);
                }
                BusinessTypes = string.Join(", ", listType);
            }
        }

        private string LoadAddress(string address)
        {
            if (address == null)
            { return null; }
            var address_composite = address.Split('\n');
            return string.Join(", ", address_composite);
        }

        //tab giao dich
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
                                    <link-entity name='bsd_employee' from='bsd_employeeid' to='bsd_employee' link-type='inner' alias='ae'>
                                       <filter type='and'>
                                          <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                                       </filter>
                                    </link-entity>
                                    <filter type='and'>
                                        <condition attribute='parentaccountid' operator='eq' uitype='account' value='" + accountid + @"' />
                                    </filter>                                    
                                </entity>
                             </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListQueueingAcc>>("opportunities", fetch);
            if (result == null)
            {
                return;
            }
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
                                <link-entity name='bsd_employee' from='bsd_employeeid' to='bsd_employee' link-type='inner' alias='ae'>
                                       <filter type='and'>
                                          <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                                       </filter>
                                    </link-entity>
                                <filter type='and'>
                                    <condition attribute='accountid' operator='eq' uitype='account' value='" + accountid + @"' />
                                </filter>
                            </entity>
                        </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListQuotationAcc>>("quotes", fetch);
            if (result == null)
            {
                return;
            }
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
                                <link-entity name='bsd_employee' from='bsd_employeeid' to='bsd_employee' link-type='inner' alias='ae'>
                                       <filter type='and'>
                                          <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                                       </filter>
                                    </link-entity>
                                <filter type='and'>
                                    <condition attribute='accountid' operator='eq' uitype='account' value='" + accountid + @"' />
                                </filter>
                            </entity>
                        </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListContractAcc>>("salesorders", fetch);
            if (result == null)
            {
                return;
            }
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
                            <link-entity name='bsd_employee' from='bsd_employeeid' to='bsd_employee' link-type='inner' alias='ae'>
                                       <filter type='and'>
                                          <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                                       </filter>
                                    </link-entity>
                            <filter type='and'>
                                <condition attribute='customerid' operator='eq' uitype='account' value='" + accountid + @"' />
                            </filter>
                        </entity>
                    </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListCaseAcc>>("incidents", fetch);
            if (result == null)
            {
                return;
            }
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

        // tab nguoi uy quyyen
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
                                    <attribute name='bsd_employee' alias='bsd_employeeid' />
                                    <order attribute='bsd_name' descending='false' />
                                    <link-entity name='contact' from='contactid' to='bsd_contact' visible='false' link-type='outer' alias='contacts'>
                                        <attribute name='bsd_fullname' alias='bsd_contact_name'/>
                                        <attribute name='mobilephone' alias='bsd_contacmobilephone'/>
                                        <attribute name='bsd_contactaddress' alias='bsd_contactaddress'/>
                                    </link-entity>
                                  
                                    <filter type='and'>
                                      <condition attribute='bsd_developeraccount' operator='eq' value='{accountid}' />
                                    </filter>                                  
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<MandatorySecondaryModel>>("bsd_mandatorysecondaries", fetchxml);
            if (result == null)
            {
                return;
            }
            var data = result.value;
            ShowMoreMandatory = data.Count < 3 ? false : true;

            if (data.Any())
            {
                foreach (var x in data)
                {
                    if (UserLogged.Id == x.bsd_employeeid)
                        x.is_employee = true;
                    else
                        x.is_employee = false;

                    if (x.statuscode == "100000000")
                    {
                        x.statuscode_title = "Applying";                                            
                        list_MandatorySecondary.Insert(0,x);
                    }    
                    else
                    {
                        x.statuscode_title = "Cancel";
                        list_MandatorySecondary.Add(x);
                    }
                }
            }

        }

        public async Task<bool> DeleteMandatory_Secondary(MandatorySecondaryModel Mandatory)
        {
            if (Mandatory != null)
            {
                var deleteResponse = await CrmHelper.DeleteRecord($"/bsd_mandatorysecondaries({Mandatory.bsd_mandatorysecondaryid})");
                if (deleteResponse.IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }        
    }
}
