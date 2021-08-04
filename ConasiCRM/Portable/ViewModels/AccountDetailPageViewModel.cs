using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
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
        public ObservableCollection<OptionSet> BusinessTypeOptions { get; set; }

        private string _businessTypes;
        public string BusinessTypes { get => _businessTypes; set { _businessTypes = value; OnPropertyChanged(nameof(BusinessTypes)); } }
       
        private string _localization;
        public string Localization { get => _localization; set { _localization = value; OnPropertyChanged(nameof(Localization)); } }

        private AccountFormModel _singleAccount;
        public AccountFormModel singleAccount { get => _singleAccount; set { _singleAccount = value; OnPropertyChanged(nameof(singleAccount)); } }

        private LookUp _PrimaryContact;
        public LookUp PrimaryContact { get => _PrimaryContact; set { if (_PrimaryContact != value) { this._PrimaryContact = value; OnPropertyChanged(nameof(PrimaryContact)); } } }

        public AccountDetailPageViewModel()
        {
            BusinessTypeOptions = new ObservableCollection<OptionSet>();
            BusinessTypeOptions.Add(new OptionSet("100000000", "Customer"));
            BusinessTypeOptions.Add(new OptionSet("100000001", "Partner"));
            BusinessTypeOptions.Add(new OptionSet("100000002", "Sales Argents"));
            BusinessTypeOptions.Add(new OptionSet("100000003", "Developer"));                    
        }

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
                                <attribute name='bsd_groupgstregisttationnumber' />
                                <attribute name='statuscode' />
                                <attribute name='ownerid' />
                                <attribute name='createdon' />
                                <attribute name='address1_composite' />
                                <attribute name='bsd_companycode' />
                                <attribute name='bsd_registrationcode' />
                                <attribute name='bsd_accountnameother' />
                                <attribute name='bsd_localization' />
                                <attribute name='bsd_name' />
                                <attribute name='name' />
                                <attribute name='accountid' />
                                <attribute name='bsd_businesstypesys' alias='bsd_businesstype' />
                                <order attribute='createdon' descending='true' />
                                    <link-entity name='contact' from='contactid' to='primarycontactid' visible='false' link-type='outer' alias='contacts'>
                                        <attribute name='bsd_fullname' alias='primarycontactname'/>
                                    </link-entity>
                                    <filter type='and'>
                                      <condition attribute='accountid' operator='eq' value='" + accountid + @"' />
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
            this.singleAccount = tmp;
            PrimaryContact = new LookUp() { Id = Guid.Parse(tmp._primarycontactid_value), Name = tmp.primarycontactname };
        }

        public void GetTypeById(string loai)
        {
            if (loai != string.Empty)
            {
                List<string> listType = new List<string>();
                var ids = singleAccount.bsd_businesstype.Split(',').ToList();
                foreach (var item in ids)
                {
                    OptionSet optionSet = BusinessTypeOptions.Single(x => x.Val == item);
                    listType.Add(optionSet.Label);
                }
                BusinessTypes = string.Join(", ", listType);
            }
        }    
    }
}
