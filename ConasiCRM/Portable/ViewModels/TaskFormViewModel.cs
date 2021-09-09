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
    public class TaskFormViewModel : BaseViewModel
    {
        public bool FocusTimePickerStart = false;
        public bool FocusTimePickerEnd = false;
        public bool FocusDateTimeStart = false;
        public bool FocusDateTimeEnd = false;

        private TaskFormModel _taskFormModel;
        public TaskFormModel TaskFormModel { get => _taskFormModel; set { _taskFormModel = value; OnPropertyChanged(nameof(TaskFormModel)); } }

        public LookUpConfig ContactLookUpConfig { get; set; }
        public LookUpConfig AccountLookUpConfig { get; set; }
        public LookUpConfig LeadLookUpConfig { get; set; }
        
        public TaskFormViewModel()
        {
            ContactLookUpConfig = new LookUpConfig()
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
                PropertyName = "Contact"
            };

            AccountLookUpConfig = new LookUpConfig()
            {
                FetchXml = @"<fetch version='1.0' count='15' page='{0}' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='account'>
                    <attribute name='accountid' alias='Id' />
                    <attribute name='name' alias='Name' />
                    <attribute name='createdon' alias='Detail' />
                    <order attribute='name' descending='false' />
                  </entity>
                </fetch>",
                EntityName = "accounts",
                PropertyName = "Account"
            };

            LeadLookUpConfig = new LookUpConfig()
            {
                FetchXml = @"<fetch version='1.0' page='{0}' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='lead'>
                                <attribute name='fullname' alias='Name'/>
                                <attribute name='createdon' alias='Detail'/>
                                <attribute name='leadid' alias='Id'/>
                                <order attribute='createdon' descending='true' />
                              </entity>
                            </fetch>",
                EntityName = "leads",
                PropertyName = "Lead"
            };
        }

        public async Task CreateTask()
        {
            TaskFormModel.activityid = Guid.NewGuid();
            var content = await getContent();
            string path = "/tasks";
            CrmApiResponse result = await CrmHelper.PostData(path, content);
            
            if (result.IsSuccess)
            {
                
            }
            else
            {
                //var mess = result.ErrorResponse?.error?.message ?? "Đã xảy ra lỗi. Vui lòng thử lại.";
                //await App.Current.MainPage.DisplayAlert("", mess, "OK");
                //return new Guid();
            }
        }

        private async Task<object> getContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["activityid"] = TaskFormModel.activityid.ToString();
            data["subject"] = TaskFormModel.subject;
            data["description"] = TaskFormModel.description ?? "";
            data["scheduledstart"] = TaskFormModel.scheduledstart.Value;
            data["scheduledend"] = TaskFormModel.scheduledend.Value;
            //if (dataTask.Customer.Type == 1)
            //{
            //    data["regardingobjectid_contact_task@odata.bind"] = "/contacts(" + dataTask.Customer.Id.ToString() + ")";
            //}
            //else if (dataTask.Customer.Type == 2)
            //{
            //    data["regardingobjectid_account_task@odata.bind"] = "/accounts(" + dataTask.Customer.Id.ToString() + ")";
            //}
            //else
            //{
            //    data["regardingobjectid_lead_task@odata.bind"] = "/leads(" + dataTask.Customer.Id.ToString() + ")";
            //}
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
    }
}
