using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class QueueFormViewModel : FormLookupViewModel
    {
        private QueueFormModel _queueFormModel;
        public QueueFormModel QueueFormModel { get => _queueFormModel; set { _queueFormModel = value; OnPropertyChanged(nameof(QueueFormModel)); } }

        public List<LookUp> ContactsLookUp { get; set; }
        public List<LookUp> AccountsLookUp { get; set; }

        private LookUp _customer;
        public LookUp Customer
        {
            get => _customer;
            set
            {
                if (_customer != value)
                {
                    _customer = value;
                    OnPropertyChanged(nameof(Customer));
                }
            }
        }

        private List<LookUp> _daiLyOptions;
        public List<LookUp> DaiLyOptions { get => _daiLyOptions; set { _daiLyOptions = value; OnPropertyChanged(nameof(DaiLyOptions)); } }

        private LookUp _daiLyOption;
        public LookUp DailyOption { get => _daiLyOption; set { _daiLyOption = value;OnPropertyChanged(nameof(DailyOption)); } }

        public QueueFormViewModel()
        {
            QueueFormModel = new QueueFormModel();
            ContactsLookUp = new List<LookUp>();
            AccountsLookUp = new List<LookUp>();
            DaiLyOptions = new List<LookUp>();
          
        }

        public async Task LoadFromProject(Guid ProjectId)
        {

            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_project'>
                                    <attribute name='bsd_projectid' alias='bsd_project_id' />
                                    <attribute name='bsd_name' alias='bsd_project_name' />
                                    <attribute name='createdon' />
                                    <attribute name='bsd_bookingfee' alias='bsd_bookingf'/>
                                    <attribute name='bsd_shortqueingtime' alias='bsd_shorttime' />
                                    <attribute name='bsd_longqueuingtime' alias='bsd_longtime' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='bsd_projectid' operator='eq' value='{" + ProjectId.ToString() + @"}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueueFormModel>>("bsd_projects", fetchXml);
            if (result == null)
                return;
            var tmp = result.value.FirstOrDefault();
            if (tmp == null)
            {
                return;
            }
            this.QueueFormModel = tmp;
            QueueFormModel.bsd_queuingfee = QueueFormModel.bsd_bookingf;
        }

        public async Task LoadFromUnit(Guid UnitId)
        {

            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='product'>
                                    <attribute name='name' alias='bsd_units_name' />                                   
                                    <attribute name='description' />                                
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_projectcode' />                                 
                                    <attribute name='productid' alias='bsd_units_id' />
                                    <attribute name='bsd_queuingfee' alias='bsd_units_queuingfee' />
                                    <attribute name='bsd_phaseslaunchid' />
                                    <order attribute='createdon' descending='true' />
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectcode' link-type='outer' alias='aa'>
 	                                    <attribute name='bsd_projectid' alias='bsd_project_id' />
    	                                <attribute name='bsd_name' alias='bsd_project_name' />
	                                    <attribute name='bsd_bookingfee' alias='bsd_bookingf' />
                                        <attribute name='bsd_longqueuingtime' alias='bsd_longtime' />
                                        <attribute name='bsd_shortqueingtime' alias='bsd_shorttime'/>
                                    </link-entity>
                                    <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaseslaunchid' visible='false' link-type='outer' alias='a_8d7b98e66ce2e811a94e000d3a1bc2d1'>
    	                                <attribute name='bsd_name' alias='bsd_phaseslaunch_name' />
    	                                <attribute name='bsd_phaseslaunchid' alias='bsd_phaseslaunch_id' />                   
                                    </link-entity>
                                    <filter type='and'>
                                        <condition attribute='productid' operator='eq' value='{" + UnitId.ToString() + @"}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueueFormModel>>("products", fetchXml);
            if (result == null)
                return;
            var tmp = result.value.FirstOrDefault();
            if (tmp == null)
            {
                return;
            }
            this.QueueFormModel = tmp;          
            if (QueueFormModel.bsd_units_queuingfee > 0)
                QueueFormModel.bsd_queuingfee = QueueFormModel.bsd_units_queuingfee;
            else if (QueueFormModel.bsd_bookingf > 0)
                QueueFormModel.bsd_queuingfee = QueueFormModel.bsd_bookingf;
        }

        public async Task<bool> SetQueueTime(Guid UnitId)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='opportunity'>
                                <attribute name='name' />
                                <attribute name='customerid' />
                                <attribute name='estimatedvalue' />
                                <attribute name='statuscode' />
                                <attribute name='createdon' />
                                <attribute name='bsd_queuenumber' />
                                <attribute name='bsd_project' />
                                <attribute name='opportunityid' />
                                <attribute name='bsd_queuingexpired' />
                                <attribute name='bsd_bookingtime' />
                                <order attribute='createdon' descending='true' />      
                                <link-entity name='contact' from='contactid' to='parentcontactid' visible='false' link-type='outer' alias='a_7eff24578704e911a98b000d3aa2e890'>
                                      <attribute name='contactid' alias='contact_id' />
                                      <attribute name='bsd_fullname' alias='contact_name' />
                                </link-entity>
                                <link-entity name='account' from='accountid' to='parentaccountid' visible='false' link-type='outer' alias='a_77ff24578704e911a98b000d3aa2e890'>
                                      <attribute name='accountid' alias='account_id' />
                                      <attribute name='bsd_name' alias='account_name' />
                                </link-entity>
                                <filter type='and'>
                                  <condition attribute='bsd_units' operator='eq' uitype='product' value='{" + UnitId.ToString() + @"}' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueueFormModel>>("opportunities", fetch);
            if (result == null)
                return false;
            var data = result.value;
            if (data == null)
                return false;                    

            if (data.SingleOrDefault(x => x.account_id == Customer.Id) != null || data.SingleOrDefault(x => x.contact_id == Customer.Id) != null)
            {
                return false;
            }

            if (data.Count <= 0 || data.Where(x => x.statuscode == 100000000) == null)
            {
                QueueFormModel.bsd_bookingtime = DateTime.Now;
                QueueFormModel.statuscode = 100000000;
            }
            else
            {
                var queue = (QueueFormModel)data.OrderBy(x => x.bsd_queuingexpired).LastOrDefault();
                QueueFormModel.bsd_bookingtime = queue.bsd_queuingexpired;
                QueueFormModel.statuscode = 100000002;
            }

            if (QueueFormModel.bsd_phaseslaunch_id != null || QueueFormModel.bsd_phaseslaunch_id != Guid.Empty)
            {
                QueueFormModel.bsd_queuingexpired = QueueFormModel.bsd_bookingtime.AddHours(QueueFormModel.bsd_shorttime);                
            }
            else
            {
                QueueFormModel.bsd_queuingexpired = QueueFormModel.bsd_bookingtime.AddDays(QueueFormModel.bsd_longtime);              
            }
            return true;
        }

        public async Task<bool> SetQueueTimeProject(Guid ProjectID)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='opportunity'>
                                <attribute name='name' />
                                <attribute name='customerid' />
                                <attribute name='estimatedvalue' />
                                <attribute name='statuscode' />
                                <attribute name='createdon' />
                                <attribute name='bsd_queuenumber' />
                                <attribute name='bsd_project' />
                                <attribute name='opportunityid' />
                                <attribute name='bsd_queuingexpired' />
                                <attribute name='bsd_bookingtime' />
                                <order attribute='createdon' descending='true' />                               
                                <link-entity name='contact' from='contactid' to='parentcontactid' visible='false' link-type='outer' alias='a_7eff24578704e911a98b000d3aa2e890'>
                                      <attribute name='contactid' alias='contact_id' />
                                      <attribute name='bsd_fullname' alias='contact_name' />
                                </link-entity>
                                <link-entity name='account' from='accountid' to='parentaccountid' visible='false' link-type='outer' alias='a_77ff24578704e911a98b000d3aa2e890'>
                                      <attribute name='accountid' alias='account_id' />
                                      <attribute name='bsd_name' alias='account_name' />
                                </link-entity>
                                <filter type='and'>
                                    <condition attribute='bsd_project' operator='eq' uitype='bsd_project' value='{" + ProjectID.ToString() + @"}' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueueFormModel>>("opportunities", fetch);
            if (result == null)
                return false;
            var data = result.value;
            if (data == null)
                return false;

            if (data.SingleOrDefault(x => x.account_id == Customer.Id) != null || data.SingleOrDefault(x => x.contact_id == Customer.Id) != null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> createQueue()
        {
            string path = "/opportunities";
            QueueFormModel.opportunityid = Guid.NewGuid();
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

        public async Task<Boolean> updateQueue()
        {
            string path = "/opportunities(" + QueueFormModel.opportunityid + ")";
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

        public async Task LoadContactsLookUp()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='contactid' alias='Id' />
                    <attribute name='fullname' alias='Name' />
                    <attribute name='createdon' alias='Detail' />
                    <order attribute='fullname' descending='false' />                   
                    <filter type='and'>
                        <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                    </filter>
                  </entity>
                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("contacts", fetch);
            if (result == null)
                return;
            var data = result.value;
            foreach (var item in data)
            {
                ContactsLookUp.Add(item);
            }
        }

        public async Task LoadAccountsLookUp()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='account'>
                                <attribute name='name' alias='Name'/>
                                <attribute name='accountid' alias='Id'/>
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                    <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("accounts", fetch);
            if (result == null)
                return;
            var data = result.value;
            foreach (var item in data)
            {
                AccountsLookUp.Add(item);
            }
        }

        public async Task LoadSalesAgent()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='account'>
                                    <attribute name='name' alias='Name' />
                                    <attribute name='accountid' alias='Id' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_businesstypesys' operator='eq' value='100000002' />                                 
                                      <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("accounts", fetch);
            if (result == null)
                return;
            var data = result.value;
            foreach (var item in data)
            {
                DaiLyOptions.Add(item);
            }
        }

        private async Task<object> getContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            CrmApiResponse clearLookupResponse = new CrmApiResponse();
            data["opportunityid"] = QueueFormModel.opportunityid;

            if (QueueFormModel.bsd_project_id == null || QueueFormModel.bsd_project_id == Guid.Empty)
            {
                await DeletLookup("bsd_Project", QueueFormModel.opportunityid);
            }
            else
            {
                data["bsd_Project@odata.bind"] = $"/bsd_projects({QueueFormModel.bsd_project_id})";
            }

            if (QueueFormModel.bsd_units_id == null || QueueFormModel.bsd_units_id == Guid.Empty)
            {
                await DeletLookup("bsd_units", QueueFormModel.opportunityid);
            }
            else
            {
                data["bsd_units@odata.bind"] = $"/products({QueueFormModel.bsd_units_id})";
            }

            if (QueueFormModel.bsd_phaseslaunch_id == null || QueueFormModel.bsd_phaseslaunch_id == Guid.Empty)
            {
                await DeletLookup("bsd_phaseslaunch", QueueFormModel.opportunityid);
            }
            else
            {
                data["bsd_phaselaunch@odata.bind"] = $"/bsd_phaseslaunchs({QueueFormModel.bsd_phaseslaunch_id})";
            }

            data["bsd_queuingfee"] = QueueFormModel.bsd_queuingfee;

            data["name"] = QueueFormModel.name;

            if (Customer != null || Customer.Id != Guid.Empty)
            {
                if (Customer.Detail == "1")
                {
                    data["customerid_account@odata.bind"] = "/accounts(" + Customer.Id + ")";
                }
                else
                {
                    data["customerid_contact@odata.bind"] = "/contacts(" + Customer.Id + ")";
                }
            }

            data["budgetamount"] = QueueFormModel.budgetamount;
            data["description"] = QueueFormModel.description;

            if (DailyOption == null || DailyOption.Id == Guid.Empty)
            {
                await DeletLookup("bsd_salesagentcompany", QueueFormModel.opportunityid);
            }
            else
            {
                data["bsd_salesagentcompany@odata.bind"] = $"/accounts({DailyOption.Id})";
            }

            data["bsd_nameofstaffagent"] = QueueFormModel.bsd_nameofstaffagent;
         
            if (UserLogged.Id != null)
            {
                data["bsd_employee@odata.bind"] = "/bsd_employees(" + UserLogged.Id + ")";
            }
            if (QueueFormModel.statuscode != 0)
            {
                data["statuscode"] = QueueFormModel.statuscode;
                data["bsd_bookingtime"] = QueueFormModel.bsd_bookingtime;
                data["bsd_queuingexpired"] = QueueFormModel.bsd_queuingexpired;
            }
            data["createdon"] = QueueFormModel.createdon.ToString("yyyy-MM-dd\"T\"HH:mm:ss\"Z\"");

            return data;
        }

        public async Task<Boolean> DeletLookup(string fieldName, Guid AccountId)
        {
            var result = await CrmHelper.SetNullLookupField("opportunities", AccountId, fieldName);
            return result.IsSuccess;
        }
    }
}
