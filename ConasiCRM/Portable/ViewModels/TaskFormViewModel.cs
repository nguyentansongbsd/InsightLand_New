using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class TaskFormViewModel : BaseViewModel
    {
        private List<OptionSet> _leads;
        public List<OptionSet> Leads { get => _leads; set { _leads = value; OnPropertyChanged(nameof(Leads)); } }

        private List<OptionSet> _contacts;
        public List<OptionSet> Contacts { get => _contacts; set { _contacts = value; OnPropertyChanged(nameof(Contacts)); } }

        private List<OptionSet> _accounts;
        public List<OptionSet> Accounts { get => _accounts; set { _accounts = value; OnPropertyChanged(nameof(Accounts)); } }

        private TaskFormModel _taskFormModel;
        public TaskFormModel TaskFormModel { get => _taskFormModel; set { _taskFormModel = value; OnPropertyChanged(nameof(TaskFormModel)); } }

        private List<List<OptionSet>> _allLookUp;
        public List<List<OptionSet>> AllsLookUp { get=>_allLookUp; set { _allLookUp = value;OnPropertyChanged(nameof(AllsLookUp)); } }

        public List<string> Tabs { get; set; }

        private OptionSet _customer;
        public OptionSet Customer { get => _customer; set { _customer = value;OnPropertyChanged(nameof(Customer)); } }

        private DateTime? _scheduledStart;
        public DateTime? ScheduledStart { get => _scheduledStart; set { _scheduledStart = value; OnPropertyChanged(nameof(ScheduledStart)); } }

        private DateTime? _scheduledEnd;
        public DateTime? ScheduledEnd { get => _scheduledEnd; set { _scheduledEnd = value; OnPropertyChanged(nameof(ScheduledEnd)); } }

        private bool _isEventAllDay;
        public bool IsEventAllDay { get => _isEventAllDay; set { _isEventAllDay = value; OnPropertyChanged(nameof(IsEventAllDay)); } }

        public Guid TaskId { get; set; }
        public string customerTypeLead = "1";
        public string customerTypeContact = "2";
        public string customerTypeAccount = "3";

        public TaskFormViewModel()
        {
            Leads = new List<OptionSet>();
            Contacts = new List<OptionSet>();
            Accounts = new List<OptionSet>();
            Tabs = new List<string>();
            AllsLookUp = new List<List<OptionSet>>();
        }

        public async Task LoadLeads()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='lead'>
                                <attribute name='lastname' alias='Label'/>
                                <attribute name='leadid' alias='Val'/>
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                    <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("leads", fetchXml);
            if (result == null || result.value.Count == 0) return;
            foreach (var item in result.value)
            {
                item.Title = customerTypeLead;
                this.Leads.Add(item);
            }
            
        }

        public async Task LoadContact()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='contact'>
                                    <attribute name='contactid' alias='Val' />
                                    <attribute name='fullname' alias='Label' />
                                    <order attribute='fullname' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("contacts", fetchXml);
            if (result == null || result.value.Count == 0) return;
            foreach (var item in result.value)
            {
                item.Title = customerTypeContact;
                this.Contacts.Add(item);
            }
        }

        public async Task LoadAccount()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='account'>
                                    <attribute name='accountid' alias='Val' />
                                    <attribute name='bsd_name' alias='Label' />
                                    <order attribute='name' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("accounts", fetchXml);
            if (result == null || result.value.Count == 0) return;
            foreach (var item in result.value)
            {
                item.Title = customerTypeAccount;
                Accounts.Add(item);
            }
        }

        public async Task LoadTask()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='task'>
                                    <attribute name='subject' />
                                    <attribute name='scheduledend' />
                                    <attribute name='createdby' />
                                    <attribute name='activityid' />
                                    <attribute name='scheduledstart' />
                                    <attribute name='description' />
                                    <order attribute='subject' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='activityid' operator='eq' uitype='task' value='{TaskId}' />
                                    </filter>
                                    <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='outer' alias='a_d4c7f132a91c4d16952d82eb7932504a'>
                                      <attribute name='bsd_name' alias='account_name'/>
                                      <attribute name='accountid' alias='account_id' />
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer' alias='a_323155a4a4a9409b81e038bc0c521b36'>
                                      <attribute name='fullname' alias='contact_id'/>
                                      <attribute name='contactid' alias='contact_name'/>
                                    </link-entity>
                                    <link-entity name='lead' from='leadid' to='regardingobjectid' visible='false' link-type='outer' alias='a_1e67d7c87cd1eb11bacc000d3a80021e'>
                                      <attribute name='lastname' alias='lead_name'/>
                                      <attribute name='leadid' alias='lead_id' />
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<TaskFormModel>>("tasks", fetchXml);
            if (result == null || result.value.Count == 0) return;

            this.TaskFormModel = result.value.FirstOrDefault();

            //customer type = 1 -> lead.  customer type = 2 -> contact. customer type = 3 -> account
            if (this.TaskFormModel.lead_id != Guid.Empty)
            {
                OptionSet customer = new OptionSet();
                customer.Val = this.TaskFormModel.lead_id.ToString();
                customer.Label = this.TaskFormModel.lead_name;
                customer.Title = "1";
                this.Customer = customer;
            }
            else if(this.TaskFormModel.contact_id != Guid.Empty)
            {
                OptionSet customer = new OptionSet();
                customer.Val = this.TaskFormModel.contact_id.ToString();
                customer.Label = this.TaskFormModel.contact_name;
                customer.Title = "2";
                this.Customer = customer;
            }
            else if (this.TaskFormModel.account_id != Guid.Empty)
            {
                OptionSet customer = new OptionSet();
                customer.Val = this.TaskFormModel.account_id.ToString();
                customer.Label = this.TaskFormModel.account_name;
                customer.Title = "3";
                this.Customer = customer;
            }
            
            ScheduledStart = this.TaskFormModel.scheduledstart.Value.ToLocalTime();
            ScheduledEnd = this.TaskFormModel.scheduledend.Value.ToLocalTime();
        }

        public async Task<bool> CreateTask()
        {
            TaskFormModel.activityid = Guid.NewGuid();
            var content = await getContent();
            string path = "/tasks";
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

        public async Task<bool> UpdateTask()
        {
            string path = $"/tasks({TaskFormModel.activityid})";
            var content = await getContent();
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

        private async Task<object> getContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["activityid"] = TaskFormModel.activityid.ToString();
            data["subject"] = TaskFormModel.subject;
            data["description"] = TaskFormModel.description ?? "";
            data["scheduledstart"] = ScheduledStart.Value.ToUniversalTime();
            data["scheduledend"] = ScheduledEnd.Value.ToUniversalTime();

            if (Customer != null && Customer.Title == "1")
            {
                data["regardingobjectid_lead_task@odata.bind"] = "/leads(" + Customer.Val + ")";
                
            }
            else if (Customer != null && Customer.Title == "2")
            {
                data["regardingobjectid_contact_task@odata.bind"] = "/contacts(" + Customer.Val+ ")";
                
            }
            else if(Customer != null && Customer.Title == "3")
            {
                data["regardingobjectid_account_task@odata.bind"] = "/accounts(" + Customer.Val+ ")";
            }

            if (UserLogged.Id != Guid.Empty)
            {
                data["bsd_employee_Task@odata.bind"] = "/bsd_employees(" + UserLogged.Id + ")";
            }
            if (UserLogged.ManagerId != Guid.Empty)
            {
                data["ownerid@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            }

            return data;
        }

        public async Task LoadAllLookUp()
        {
            if(Leads.Count <= 0 && Contacts.Count <= 0 && Accounts.Count <= 0)
            {
                await LoadContact();
                await LoadContact();
                await LoadAccount();
            }    

            if (AllsLookUp.Count <= 0)
            {
                AllsLookUp.Add(Leads);
                AllsLookUp.Add(Contacts);
                AllsLookUp.Add(Accounts);
            }
        }

        public void SetUpTabs()
        {
            if (Tabs.Count <= 0)
            {
                Tabs.Add("KH Tiềm Năng");
                Tabs.Add("KH Cá Nhân");
                Tabs.Add("KH Doanh Nghiệp");
            }
        }
    }
}
