using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Settings;

namespace ConasiCRM.Portable.ViewModels
{
    public class AccountFormViewModel : BaseViewModel
    {
        private AccountFormModel _singleAccount;
        public AccountFormModel singleAccount { get => _singleAccount; set { _singleAccount = value; OnPropertyChanged(nameof(singleAccount)); } }

        private List<OptionSet> _businessTypeOptionList;
        public List<OptionSet> BusinessTypeOptionList { get => _businessTypeOptionList; set { _businessTypeOptionList = value; OnPropertyChanged(nameof(BusinessTypeOptionList)); } }
        public ObservableCollection<OptionSet> LocalizationOptionList { get; set; }

        public OptionSet _localization;
        public OptionSet Localization { get => _localization; set { _localization = value; OnPropertyChanged(nameof(Localization)); } }

        public OptionSet _businessType;
        public OptionSet BusinessType { get => _businessType; set { _businessType = value; OnPropertyChanged(nameof(BusinessType)); } }

        private List<LookUp> _primaryContactOptionList;
        public List<LookUp> PrimaryContactOptionList { get=>_primaryContactOptionList; set { _primaryContactOptionList = value;OnPropertyChanged(nameof(PrimaryContactOptionList)); } }

        private LookUp _PrimaryContact;
        public LookUp PrimaryContact { get => _PrimaryContact; set { _PrimaryContact = value; OnPropertyChanged(nameof(PrimaryContact)); } }

        private AddressModel _address1;
        public AddressModel Address1 { get => _address1; set { _address1 = value; OnPropertyChanged(nameof(Address1)); } }        

        public AccountFormViewModel()
        {
            singleAccount = new AccountFormModel();

            BusinessTypeOptionList = new List<OptionSet>();
            LocalizationOptionList = new ObservableCollection<OptionSet>();
            PrimaryContactOptionList = new List<LookUp>();          
        }

        public async Task LoadOneAccount(Guid accountid)
        {
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
                                <attribute name='bsd_address'/>
                                <attribute name='bsd_nation' alias='_bsd_country_value' />
                                <attribute name='bsd_province' alias='_bsd_province_value'/>
                                <attribute name='bsd_district' alias='_bsd_district_value'/>
                                <attribute name='bsd_postalcode' />
                                <attribute name='bsd_housenumberstreet' />
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
                                   <link-entity name='new_district' from='new_districtid' to='bsd_district' link-type='outer' alias='af' >
                                        <attribute name='new_name' alias='district_name' />  
                                        <attribute name='bsd_nameen'  alias='district_name_en'/>
                                    </link-entity>
                                     <link-entity name='new_province' from='new_provinceid' to='bsd_province' link-type='outer' alias='ag'>
                                        <attribute name='new_name' alias='province_name' />    
                                        <attribute name='bsd_nameen'  alias='province_name_en'/>
                                    </link-entity>
                                   <link-entity name='bsd_country' from='bsd_countryid' to='bsd_nation' link-type='outer' alias='as'>
                                        <attribute name='bsd_name' alias='country_name' />
                                        <attribute name='bsd_nameen'  alias='country_name_en'/>
                                    </link-entity>
                                    <filter type='and'>
                                        <condition attribute='accountid' operator='eq' value='{" + accountid + @"}' />
                                    </filter>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<AccountFormModel>>("accounts", fetch);
            if (result == null || result.value == null)
                return;
            var tmp = result.value.FirstOrDefault();
            this.singleAccount = tmp;

            Address1 = new AddressModel
            {
                country_id = singleAccount._bsd_country_value,
                country_name = !string.IsNullOrWhiteSpace(singleAccount.country_name_en) && UserLogged.Language == "en" ? singleAccount.country_name_en : singleAccount.country_name,
                country_name_en = singleAccount.country_name_en,
                province_id = singleAccount._bsd_province_value,
                province_name = !string.IsNullOrWhiteSpace(singleAccount.province_name_en) && UserLogged.Language == "en" ? singleAccount.province_name_en : singleAccount.province_name,
                province_name_en = singleAccount.province_name_en,
                district_id = singleAccount._bsd_district_value,
                district_name = !string.IsNullOrWhiteSpace(singleAccount.district_name_en) && UserLogged.Language == "en" ? singleAccount.district_name_en : singleAccount.district_name,
                district_name_en = singleAccount.district_name_en,
                address = singleAccount.bsd_address,
                lineaddress = singleAccount.bsd_housenumberstreet,
                //address_en = singleAccount.bsd_diachi,
                //lineaddress_en = singleAccount.bsd_street,
            };
        }

        public void GetPrimaryContactByID()
        {
            PrimaryContact = new LookUp{ Name = singleAccount.primarycontactname, Id = singleAccount._primarycontactid_value, Detail = "Contact" };
        }    

        public async Task<bool> createAccount()
        {
            string path = "/accounts";
            singleAccount.accountid = Guid.NewGuid();
            var content = await this.getContent();
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

        public async Task<Boolean> updateAccount( )
        {
            string path = "/accounts(" + singleAccount.accountid + ")";
            var content = await this.getContent();
            CrmApiResponse result = await CrmHelper.PatchData(path, content);
            if (result.IsSuccess)
            {
                return true;
            }else
            {
                return false;
            }

        }

        public async Task<Boolean> DeletLookup(string fieldName, Guid AccountId)
        {
            var result = await CrmHelper.SetNullLookupField("accounts", AccountId, fieldName);
            return result.IsSuccess;
        }

        private async Task<object> getContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["accountid"] = singleAccount.accountid;
            data["bsd_name"] = singleAccount.bsd_name;
            data["bsd_accountnameother"] = singleAccount.bsd_accountnameother;
            data["bsd_companycode"] = singleAccount.bsd_companycode;
            //if (singleAccount.bsd_businesstypesys != null)
            //{
            //    data["bsd_businesstypesys"] = singleAccount.bsd_businesstypesys.Replace(" ", "");
            //}
            data["bsd_businesstypesys"] = this.BusinessType.Val;

            if (singleAccount.bsd_localization != null)
            {
                data["bsd_localization"] = int.Parse(singleAccount.bsd_localization);
            }
            data["emailaddress1"] = singleAccount.emailaddress1;
            data["bsd_email2"] = singleAccount.bsd_email2;
            data["websiteurl"] = singleAccount.websiteurl;
            data["fax"] = singleAccount.fax;
            data["telephone1"] = singleAccount.telephone1;
            data["bsd_registrationcode"] = singleAccount.bsd_registrationcode;
            data["bsd_issuedon"] = singleAccount.bsd_issuedon.HasValue ? (DateTime.Parse(singleAccount.bsd_issuedon.ToString()).ToLocalTime()).ToString("yyyy-MM-dd\"T\"HH:mm:ss\"Z\"") : null;
            data["bsd_placeofissue"] = singleAccount.bsd_placeofissue;

            data["bsd_vatregistrationnumber"] = singleAccount.bsd_vatregistrationnumber;

         //   data["bsd_permanentaddress1"] = singleAccount.bsd_permanentaddress1;

         //   data["bsd_housenumberstreet"] = singleAccount.bsd_housenumberstreet;

         //   data["bsd_street"] = singleAccount.bsd_housenumberstreet;
         //   data["bsd_diachi"] = singleAccount.bsd_diachi;            
         ////   data["bsd_postalcode"] = singleAccount.bsd_postalcode;

            if (singleAccount._primarycontactid_value == null)
            {
                await DeletLookup("primarycontactid", singleAccount.accountid);
            }
            else
            {
                data["primarycontactid@odata.bind"] = "/contacts(" + singleAccount._primarycontactid_value + ")"; /////Lookup Field
            }
            
            if(Address1 != null && !string.IsNullOrWhiteSpace(Address1.lineaddress))
            {
                data["bsd_address"] = Address1.address;
                data["bsd_housenumberstreet"] = Address1.lineaddress;

                data["bsd_street"] = Address1.lineaddress_en;
                data["bsd_diachi"] = Address1.address_en;

                if (Address1.country_id != Guid.Empty)
                    data["bsd_nation@odata.bind"] = "/bsd_countries(" + Address1.country_id + ")";
                else
                    await DeletLookup("bsd_nation", singleAccount.accountid);

                if (Address1.province_id != Guid.Empty)
                    data["bsd_province@odata.bind"] = "/new_provinces(" + Address1.province_id + ")";
                else
                    await DeletLookup("bsd_province", singleAccount.accountid);

                if (Address1.district_id != Guid.Empty)
                    data["bsd_district@odata.bind"] = "/new_districts(" + Address1.district_id + ")";
                else
                    await DeletLookup("bsd_district", singleAccount.accountid);

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

        public void LoadBusinessTypeForLookup()
        {
            BusinessTypeOptionList = new List<OptionSet>();
            BusinessTypeOptionList.Add(new OptionSet("100000000", "Khách hàng"));
            BusinessTypeOptionList.Add(new OptionSet("100000001", "Đối tác"));
            BusinessTypeOptionList.Add(new OptionSet("100000002", "Đại lý"));
            BusinessTypeOptionList.Add(new OptionSet("100000003", "Chủ đầu tư"));
        }

        public async Task LoadContactForLookup() // bubg
        {
            string fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='contactid' alias='Id' />
                    <attribute name='fullname' alias='Name' />
                    <attribute name='createdon' alias='Detail' />
                    <order attribute='fullname' descending='false' />
                    <filter type='and'>
                      <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='{UserLogged.Id}' />
                    </filter>
                  </entity>
                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("contacts", fetch);
            if (result == null)
                return;
            PrimaryContactOptionList = result.value;
        }
        public async Task<bool> Check_form_keydata(string bsd_vatregistrationnumber, string bsd_registrationcode, string accountid)
        {
            var fetchxml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='account'>
                                <attribute name='bsd_vatregistrationnumber' />
                                <attribute name='bsd_registrationcode' />
                                <attribute name='accountid' />
                                <order attribute='createdon' descending='true' />                              
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
            if (result != null && result.value.Count > 0)
            {
                return false;
            }
            return true;           
        }

        public async Task<bool> Check_GPKD(string bsd_vatregistrationnumber, string bsd_registrationcode, string accountid)
        {
            var fetchxml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='account'>
                                <attribute name='bsd_vatregistrationnumber' />
                                <attribute name='bsd_registrationcode' />
                                <attribute name='accountid' />
                                <order attribute='createdon' descending='true' />                              
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
            if (result != null && result.value.Count > 0)
            {
                return false;
            }
            return true;
        }
    };
}
