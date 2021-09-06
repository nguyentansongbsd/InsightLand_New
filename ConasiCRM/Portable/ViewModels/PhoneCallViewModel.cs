using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class PhoneCallViewModel : FormViewModal
    {
        public PhoneCellModel _phoneCellModel;
        public PhoneCellModel PhoneCellModel { get => _phoneCellModel; set { _phoneCellModel = value; OnPropertyChanged(nameof(PhoneCellModel)); } }
        public List<OptionSet> LeadsLookUp { get; set; }
        public List<OptionSet> ContactsLookUp { get; set; }
        public List<OptionSet> AccountsLookUp { get; set; }
        public List<List<OptionSet>> AllsLookUp { get; set; }
        public List<string> Tabs { get; set; }

        private OptionSet _customer;
        public OptionSet Customer { get => _customer; set { _customer = value; OnPropertyChanged(nameof(Customer)); } }

        private OptionSet _callFrom;
        public OptionSet CallFrom { get => _callFrom; set { _callFrom = value; OnPropertyChanged(nameof(CallFrom)); } }

        public List<string> _callTo;
        public List<string> CallTo { get => _callTo; set { _callTo = value; OnPropertyChanged(nameof(CallTo)); } }
        public List<OptionSet> CallToOptionSet { get; set; }

        string CodeAccount = "3";

        string CodeContac = "2";

        string CodeLead = "1";

        public PhoneCallViewModel()
        {
            PhoneCellModel = new PhoneCellModel();
            ContactsLookUp = new List<OptionSet>();
            LeadsLookUp = new List<OptionSet>();
            AccountsLookUp = new List<OptionSet>();
            AllsLookUp = new List<List<OptionSet>>();
            CallToOptionSet = new List<OptionSet>();
            Tabs = new List<string>();
            Tabs.Add("KH Tiềm Năng");
            Tabs.Add("KH Cá Nhân");
            Tabs.Add("KH Doanh Nghiệp");
        }

        public async Task<Boolean> DeletLookup(string fieldName, Guid id)
        {
            var result = await CrmHelper.SetNullLookupField("phonecalls", id, fieldName);
            return result.IsSuccess;
        }
        private async Task<object> getContent(string value)
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["activityid"] = PhoneCellModel.activityid.ToString();
            data["subject"] = PhoneCellModel.subject;
            data["description"] = PhoneCellModel.description;
            data["scheduledstart"] = PhoneCellModel.scheduledstart.Value.ToUniversalTime();
            data["scheduledend"] = PhoneCellModel.scheduledend.Value.ToUniversalTime();
            // data["actualdurationminutes"] = dataPhone.actualdurationminutes;
            if (value == "update")
            {
                data["statecode"] = PhoneCellModel.statecode;
                data["statuscode"] = PhoneCellModel.statuscode;
            }
            data["phonenumber"] = PhoneCellModel.phonenumber;

            if (Customer != null)
            {
                if (Customer.Title == CodeLead)
                {
                    data["regardingobjectid_lead_phonecall@odata.bind"] = "/leads(" + Customer.Val + ")";
                }
                else if (Customer.Title == CodeContac)
                {
                    data["regardingobjectid_contact_phonecall@odata.bind"] = "/contacts(" + Customer.Val + ")";
                }
                else if (Customer.Title == CodeAccount)
                {
                    data["regardingobjectid_account_phonecall@odata.bind"] = "/accounts(" + Customer.Val + ")";
                }
            }
            else
            {
                await DeletLookup("regardingobjectid_contact_phonecall", PhoneCellModel.activityid);
                await DeletLookup("regardingobjectid_account_phonecall", PhoneCellModel.activityid);
                await DeletLookup("regardingobjectid_lead_phonecall", PhoneCellModel.activityid);
            }

            List<object> dataFromTo = new List<object>();

            IDictionary<string, object> item_from = new Dictionary<string, object>();
            if (CallFrom.Title == CodeLead)
            {
                item_from["partyid_lead@odata.bind"] = "/leads(" + CallFrom.Val + ")";
                item_from["participationtypemask"] = 1;
            }
            else if (CallFrom.Title == CodeContac)
            {
                item_from["partyid_contact@odata.bind"] = "/contacts(" + CallFrom.Val + ")";
                item_from["participationtypemask"] = 1;
            }
            else if (CallFrom.Title == CodeAccount)
            {
                item_from["partyid_account@odata.bind"] = "/accounts(" + CallFrom.Val + ")";
                item_from["participationtypemask"] = 1;
            }

            dataFromTo.Add(item_from);

            foreach (var item in CallToOptionSet)
            {
                IDictionary<string, object> item_to = new Dictionary<string, object>();
                if (item.Title == CodeLead)
                {
                    item_to["partyid_lead@odata.bind"] = "/leads(" + item.Val + ")";
                    item_to["participationtypemask"] = 2;
                }
                else if (item.Title == CodeContac)
                {
                    item_to["partyid_contact@odata.bind"] = "/contacts(" + item.Val + ")";
                    item_to["participationtypemask"] = 2;
                }
                else if (item.Title == CodeAccount)
                {
                    item_to["partyid_account@odata.bind"] = "/accounts(" + item.Val + ")";
                    item_to["participationtypemask"] = 2;
                }
                dataFromTo.Add(item_to);
            }

            data["phonecall_activity_parties"] = dataFromTo;

            //if (UserLogged.Id != Guid.Empty)
            //{
            //    data["bsd_employee@odata.bind"] = "/bsd_employees(" + UserLogged.Id + ")";
            //}
            if (UserLogged.ManagerId != Guid.Empty)
            {
                data["ownerid@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            }
            return data;
        }

        public async Task<bool> createPhoneCall()
        {
            PhoneCellModel.activityid = Guid.NewGuid();
            string path = "/phonecalls";
            var content = await this.getContent("create");
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

        public async Task LoadLeadsLookUp()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='lead'>
                                <attribute name='fullname' alias='Label' />
                                <attribute name='leadid' alias='Val' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                    <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("leads", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value;
            foreach (var item in data)
            {
                item.Title = CodeLead;
                LeadsLookUp.Add(item);
            }
        }

        public async Task LoadContactsLookUp()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='contactid' alias='Val' />
                    <attribute name='fullname' alias='Label' />
                    <order attribute='fullname' descending='false' />                   
                    <filter type='and'>
                        <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                    </filter>
                  </entity>
                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("contacts", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value;
            foreach (var item in data)
            {
                item.Title = CodeContac;
                ContactsLookUp.Add(item);
            }
        }

        public async Task LoadAccountsLookUp()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='account'>
                                <attribute name='name' alias='Label'/>
                                <attribute name='accountid' alias='Val'/>
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                    <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("accounts", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value;
            foreach (var item in data)
            {
                item.Title = CodeAccount;
                AccountsLookUp.Add(item);
            }
        }

        public async Task LoadAllLookUp()
        {
            if (LeadsLookUp.Count <= 0 && ContactsLookUp.Count <= 0 && AccountsLookUp.Count <= 0)
            {
                await LoadLeadsLookUp();
                await LoadContactsLookUp();
                await LoadAccountsLookUp();
            }
            if (AllsLookUp.Count <= 0)
            {              
                AllsLookUp.Add(LeadsLookUp);
                AllsLookUp.Add(ContactsLookUp);
                AllsLookUp.Add(AccountsLookUp);
            }
        }

        public void ConvertStringToOptionSet()
        {
            if (CallTo != null && CallTo.Count > 0)
            {
                foreach (var id in CallTo)
                {
                    var item = new OptionSet();
                    item.Val = id;
                    if (LeadsLookUp.SingleOrDefault(x => x.Val == id) != null)
                        item.Title = CodeLead;
                    else if (ContactsLookUp.SingleOrDefault(x => x.Val == id) != null)
                        item.Title = CodeContac;
                    else if (AccountsLookUp.SingleOrDefault(x => x.Val == id) != null)
                        item.Title = CodeAccount;
                    CallToOptionSet.Add(item);
                }
            }
        }
    }
}
