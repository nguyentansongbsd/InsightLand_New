using ConasiCRM.Portable.Controls;
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
    public class PhoneCallViewModel : BaseViewModel
    {
        public PhoneCellModel _phoneCellModel;
        public PhoneCellModel PhoneCellModel { get => _phoneCellModel; set { _phoneCellModel = value;OnPropertyChanged(nameof(PhoneCellModel)); } }

        private OptionSet _customer;
        public OptionSet Customer { get => _customer; set { _customer = value; OnPropertyChanged(nameof(Customer)); } }

        private string _callFrom;
        private string CallFrom { get => _callFrom; set { _callFrom = value; OnPropertyChanged(nameof(CallFrom)); } }

        public OptionSet _callTo;
        public OptionSet CallTo { get => _callTo; set { _callTo = value; OnPropertyChanged(nameof(CallTo)); } }

        public string CodeAccount = LookUpMultipleTabs.CodeAccount;

        public string CodeContac = LookUpMultipleTabs.CodeContac;

        public string CodeLead = LookUpMultipleTabs.CodeLead;

        public bool _showButton;
        public bool ShowButton { get => _showButton; set { _showButton = value; OnPropertyChanged(nameof(ShowButton)); } }

        public PhoneCallViewModel()
        {
            PhoneCellModel = new PhoneCellModel();
            CallFrom = UserLogged.User;
            ShowButton = true;  
        }

        public async Task<Boolean> DeletLookup(string fieldName, Guid id)
        {
            var result = await CrmHelper.SetNullLookupField("phonecalls", id, fieldName);
            return result.IsSuccess;
        }
        private async Task<object> getContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["activityid"] = PhoneCellModel.activityid.ToString();
            data["subject"] = PhoneCellModel.subject;
            data["description"] = PhoneCellModel.description;
            data["scheduledstart"] = PhoneCellModel.scheduledstart.Value.ToUniversalTime();
            data["scheduledend"] = PhoneCellModel.scheduledend.Value.ToUniversalTime();
            data["actualdurationminutes"] = PhoneCellModel.actualdurationminutes;
            data["statecode"] = PhoneCellModel.statecode;
            data["statuscode"] = PhoneCellModel.statuscode;           
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
            item_from["partyid_systemuser@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            item_from["participationtypemask"] = 1;

            dataFromTo.Add(item_from);

            IDictionary<string, object> item_to = new Dictionary<string, object>();
                if (CallTo.Title == CodeLead)
                {
                    item_to["partyid_lead@odata.bind"] = "/leads(" + CallTo.Val + ")";
                    item_to["participationtypemask"] = 2;
                }
                else if (CallTo.Title == CodeContac)
                {
                    item_to["partyid_contact@odata.bind"] = "/contacts(" + CallTo.Val + ")";
                    item_to["participationtypemask"] = 2;
                }
                else if (CallTo.Title == CodeAccount)
                {
                    item_to["partyid_account@odata.bind"] = "/accounts(" + CallTo.Val + ")";
                    item_to["participationtypemask"] = 2;
                }
                dataFromTo.Add(item_to);            

            data["phonecall_activity_parties"] = dataFromTo;

            if (UserLogged.ManagerId != Guid.Empty)
            {
                data["ownerid@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            }
            if (UserLogged.Id != Guid.Empty)
            {
                data["bsd_employee_PhoneCall@odata.bind"] = "/bsd_employees(" + UserLogged.Id + ")";
            }
            return data;
        }

        public async Task<bool> createPhoneCall()
        {
            PhoneCellModel.activityid = Guid.NewGuid();
            PhoneCellModel.statecode = 0;
            PhoneCellModel.statuscode = 1;
            var actualdurationminutes = Math.Round((PhoneCellModel.scheduledend.Value - PhoneCellModel.scheduledstart.Value).TotalMinutes);
            PhoneCellModel.actualdurationminutes = int.Parse(actualdurationminutes.ToString());
            string path = "/phonecalls";
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

        public async Task<bool> UpdatePhoneCall(Guid phonecallid)
        {
            PhoneCellModel.statecode = 0;
            PhoneCellModel.statuscode = 1;
            var actualdurationminutes = Math.Round((PhoneCellModel.scheduledend.Value - PhoneCellModel.scheduledstart.Value).TotalMinutes);
            PhoneCellModel.actualdurationminutes = int.Parse(actualdurationminutes.ToString());
            string path = "/phonecalls(" + phonecallid + ")";
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

        public async Task loadPhoneCall(Guid id)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
            <entity name='phonecall'>
                <attribute name='subject' />
                <attribute name='statecode' />
                <attribute name='prioritycode' />
                <attribute name='scheduledend' alias='scheduledend' />
                <attribute name='createdby' />
                <attribute name='regardingobjectid' />
                <attribute name='activityid' />
                <attribute name='statuscode' />
                <attribute name='scheduledstart' alias='scheduledstart' />
                <attribute name='actualdurationminutes' />
                <attribute name='description' />
                <attribute name='activitytypecode' />
                <attribute name='phonenumber' />
                <order attribute='subject' descending='false' />
                <filter type='and'>
                    <condition attribute='activityid' operator='eq' uitype='phonecall' value='" + id + @"' />
                </filter>
                <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='outer' alias='a_9b4f4019bdc24dd79b1858c2d087a27d'>
                    <attribute name='accountid' alias='account_id' />                  
                    <attribute name='bsd_name' alias='account_name'/>
                </link-entity>
                <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer' alias='a_66b6d0af970a40c9a0f42838936ea5ce'>
                    <attribute name='contactid' alias='contact_id' />                  
                    <attribute name='fullname' alias='contact_name'/>
                </link-entity>
                <link-entity name='lead' from='leadid' to='regardingobjectid' visible='false' link-type='outer' alias='a_fb87dbfd8304e911a98b000d3aa2e890'>
                    <attribute name='leadid' alias='lead_id'/>                  
                    <attribute name='fullname' alias='lead_name'/>
                </link-entity>
                <link-entity name='bsd_employee' from='bsd_employeeid' to='bsd_employee' link-type='outer' alias='aa'>
                    <attribute name='bsd_name' alias='user_name'/>
                    <attribute name='bsd_employeeid' alias='user_id'/>
                </link-entity>
            </entity>
          </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhoneCellModel>>("phonecalls", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value.FirstOrDefault();
            PhoneCellModel.activityid = data.activityid;
            PhoneCellModel.subject = data.subject;
            PhoneCellModel.statecode = data.statecode;
            PhoneCellModel.statuscode = data.statuscode;
            PhoneCellModel.phonenumber = data.phonenumber;
            PhoneCellModel.description = data.description;
    //        PhoneCellModel = data;
            if (data.scheduledend != null && data.scheduledstart != null)
            {
                PhoneCellModel.scheduledend = data.scheduledend.Value.ToLocalTime();
                PhoneCellModel.scheduledstart = data.scheduledstart.Value.ToLocalTime();
            }

            if (data.contact_id != Guid.Empty)
            {
                Customer = new OptionSet
                {
                    Title = CodeContac,
                    Val = data.contact_id.ToString(),
                    Label = data.contact_name
                };
            }
            else if (data.account_id != Guid.Empty)
            {
                Customer = new OptionSet
                {
                    Title = CodeAccount,
                    Val = data.account_id.ToString(),
                    Label = data.account_name
                };
            }
            else if (data.lead_id != Guid.Empty)
            {
                Customer = new OptionSet
                {
                    Title = CodeLead,
                    Val = data.lead_id.ToString(),
                    Label = data.lead_name
                };
            }

            if (PhoneCellModel.statecode == 0)
                ShowButton = true;
            else
                ShowButton = false;
        }
        public async Task loadFromTo(Guid id)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true' >
                                    <entity name='phonecall' >
                                        <attribute name='subject' />
                                        <attribute name='activityid' />
                                        <order attribute='subject' descending='false' />
                                        <filter type='and' >
                                            <condition attribute='activityid' operator='eq' value='" + id + @"' />
                                        </filter>
                                        <link-entity name='activityparty' from='activityid' to='activityid' link-type='inner' alias='ab' >
                                            <attribute name='partyid' alias='partyID'/>
                                            <attribute name='participationtypemask' alias='typemask' />
                                            <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='partyaccount' >
                                                <attribute name='bsd_name' alias='account_name'/>
                                            </link-entity>
                                            <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='partycontact' >
                                                <attribute name='fullname' alias='contact_name'/>
                                            </link-entity>
                                            <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='partylead' >
                                                <attribute name='fullname' alias='lead_name'/>
                                            </link-entity>
                                            <link-entity name='systemuser' from='systemuserid' to='partyid' link-type='outer' alias='partyuser' >
                                                <attribute name='fullname' alias='user_name'/>
                                            </link-entity>
                                        </link-entity>
                                    </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PartyModel>>("phonecalls", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    if (item.typemask == 1) // from call
                    {
                        CallFrom = item.user_name;                 
                    }
                    else if (item.typemask == 2) // to call
                    {
                        if (item.contact_name != null && item.contact_name != string.Empty)
                        {
                            CallTo = new OptionSet
                            {
                                Title = CodeContac,
                                Val = item.partyID.ToString(),
                                Label = item.contact_name
                            };
                        }
                        else if (item.account_name != null && item.account_name != string.Empty)
                        {
                            CallTo = new OptionSet
                            {
                                Title = CodeAccount,
                                Val = item.partyID.ToString(),
                                Label = item.account_name
                            };
                        }
                        else if (item.lead_name != null && item.lead_name != string.Empty)
                        {
                            CallTo = new OptionSet
                            {
                                Title = CodeLead,
                                Val = item.partyID.ToString(),
                                Label = item.lead_name
                            };
                        }                       
                    }               
                }
            }

        }

        public async Task LoadOneAccount(string accountid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='account'>                              
                                <attribute name='telephone1' />
                                <attribute name='bsd_name' />
                                <attribute name='name' />
                                <attribute name='accountid' />                               
                                <order attribute='createdon' descending='true' />                                
                                <filter type='and'>
                                  <condition attribute='accountid' operator='eq' value='" + accountid + @"' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<AccountFormModel>>("accounts", fetch);
            if (result == null || result.value == null)
                return;
            var tmp = result.value.FirstOrDefault();
            PhoneCellModel.phonenumber = tmp.telephone1;
        }

        public async Task loadOneContact(String contactid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='contact'>
                                    <attribute name='fullname' />  
                                    <attribute name='mobilephone' />
                                    <attribute name='bsd_fullname' />
                                    <attribute name='contactid' />
                                    <order attribute='createdon' descending='true' />                                   
                                    <filter type='and'>
                                        <condition attribute='contactid' operator='eq' value='" + contactid + @"' />
                                    </filter>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactFormModel>>("contacts", fetch);
            if (result == null || result.value == null)
                return;
            var tmp = result.value.FirstOrDefault();
            PhoneCellModel.phonenumber = tmp.mobilephone;
        }

        public async Task LoadOneLead(String leadid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='lead'>
                                    <attribute name='lastname' />                                  
                                    <attribute name='mobilephone' />                                  
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                        <condition attribute='leadid' operator='eq' value='{" + leadid + @"}' />
                                    </filter>                                  
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LeadFormModel>>("leads", fetch);
            if (result == null || result.value == null)
                return;
            var tmp = result.value.FirstOrDefault();
            PhoneCellModel.phonenumber = tmp.mobilephone;
        }
    }
}
