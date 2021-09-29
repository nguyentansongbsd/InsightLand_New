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
    public class QueueFormViewModel : BaseViewModel
    {
        private QueueFormModel _queueFormModel;
        public QueueFormModel QueueFormModel { get => _queueFormModel; set { _queueFormModel = value; OnPropertyChanged(nameof(QueueFormModel)); } }

        public ObservableCollection<LookUp> ContactsLookUp { get; set; } = new ObservableCollection<LookUp>();
        public ObservableCollection<LookUp> AccountsLookUp { get; set; } = new ObservableCollection<LookUp>();

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
        public LookUp DailyOption { get => _daiLyOption; set { _daiLyOption = value; OnPropertyChanged(nameof(DailyOption)); } }

        public QueueFormViewModel()
        {
            QueueFormModel = new QueueFormModel();
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
            QueueFormModel._queue_createdon = DateTime.Now;
        }

        public async Task LoadFromUnit(Guid UnitId)
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='product'>
                                    <attribute name='name' alias='bsd_units_name' />                                   
                                    <attribute name='description' />                                
                                    <attribute name='statuscode' alias='UnitStatusCode'/>
                                    <attribute name='bsd_projectcode' />                                 
                                    <attribute name='productid' alias='bsd_units_id' />
                                    <attribute name='bsd_queuingfee' alias='bsd_units_queuingfee' />
                                    <attribute name='bsd_phaseslaunchid' />
                                    <attribute name='pricelevelid' alias='pricelist_id'/>
                                    <attribute name='bsd_blocknumber' alias='bsd_block_id'/>
                                    <attribute name='bsd_floor' alias='bsd_floor_id'/>
                                    <attribute name='price' alias='unit_price'/>
                                    <attribute name='defaultuomid' alias='_defaultuomid_value' />
                                    <attribute name='transactioncurrencyid' alias='_transactioncurrencyid_value'/>
                                    <attribute name='bsd_taxpercent'/>
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
            QueueFormModel._queue_createdon = DateTime.Now;
        }

        public async Task<bool> SetQueueTime()
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
                                <attribute name='bsd_queuingexpired' alias='_queue_bsd_queuingexpired' />
                                <attribute name='bsd_bookingtime' alias='_queue_bsd_bookingtime' />
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
                                    <condition attribute='bsd_queueforproject' operator='eq' value='0' />
                                    <condition attribute='bsd_units' operator='eq' uitype='product' value='{" + QueueFormModel.bsd_units_id + @"}' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueueFormModel>>("opportunities", fetch);
            if (result == null || result.value == null)
                return false;
            var data = result.value;

            if (data.Where(x => x.account_id == Customer.Id).ToList().Count > 0 || data.Where(x => x.contact_id == Customer.Id).ToList().Count > 0)
            {
                return false;
            }

            if (data.Count <= 0 || data.Where(x => x.statuscode == 100000000).ToList().Count <= 0)
            {
                QueueFormModel._queue_bsd_bookingtime = DateTime.Now;
                QueueFormModel.statuscode = 100000000;
            }
            else
            {
                var queue = (QueueFormModel)data.OrderBy(x => x._queue_bsd_queuingexpired).LastOrDefault();
                QueueFormModel._queue_bsd_bookingtime = queue._queue_bsd_queuingexpired;
                QueueFormModel.statuscode = 100000002;
            }

            if (QueueFormModel.bsd_phaseslaunch_id != null || QueueFormModel.bsd_phaseslaunch_id != Guid.Empty)
            {
                QueueFormModel._queue_bsd_queuingexpired = QueueFormModel._queue_bsd_bookingtime.AddHours(QueueFormModel.bsd_shorttime);
            }
            else
            {
                QueueFormModel._queue_bsd_queuingexpired = QueueFormModel._queue_bsd_bookingtime.AddDays(QueueFormModel.bsd_longtime);
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
                if (QueueFormModel.UnitStatusCode > 0)
                {
                    UpdateStatusUnit();
                }
                QueueUnitModel queueUnit = await ContentQueueUnit();
                await CreateQueueUnit(queueUnit);
                return true;
            }
            else
            {
                return false;
            }

        }

        private async Task<QueueUnitModel> ContentQueueUnit()
        {
            QueueUnitModel queueUnit = new QueueUnitModel();
            queueUnit.opportunityproductid = Guid.NewGuid();
            queueUnit.bsd_status = true;
            queueUnit.bsd_pricelist = this.QueueFormModel.pricelist_id.ToString();
            queueUnit.bsd_booking = this.QueueFormModel.opportunityid.ToString();
            queueUnit.bsd_project = this.QueueFormModel.bsd_project_id.ToString();
            queueUnit.bsd_block = this.QueueFormModel.bsd_block_id.ToString();
            queueUnit.bsd_units = this.QueueFormModel.bsd_units_id.ToString();
            queueUnit.bsd_phaseslaunch = this.QueueFormModel.bsd_phaseslaunch_id.ToString();
            queueUnit.bsd_floor = this.QueueFormModel.bsd_floor_id.ToString();
            queueUnit.isproductoverridden = false;
            queueUnit.productid = this.QueueFormModel.bsd_units_id.ToString();
            queueUnit.ispriceoverridden = false;
            queueUnit.priceperunit = this.QueueFormModel.unit_price;
            queueUnit.uomid = this.QueueFormModel._defaultuomid_value;
            queueUnit.baseamount = this.QueueFormModel.unit_price;
            queueUnit.extendedamount = this.QueueFormModel.unit_price;
            queueUnit.transactioncurrencyid = this.QueueFormModel._transactioncurrencyid_value;
            queueUnit.tax = this.QueueFormModel.bsd_taxpercent;
            queueUnit.createdby = UserLogged.ManagerId.ToString();
            return queueUnit;
        }

        private async Task<bool> CreateQueueUnit(QueueUnitModel queueUnit)
        {
            string path = "/opportunityproducts";
            var content = await GetContentQueueUnit(queueUnit);
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

        private async Task<object> GetContentQueueUnit(QueueUnitModel queueUnit)
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["opportunityproductid"] = queueUnit.opportunityproductid;
            data["bsd_status"] = queueUnit.bsd_status;
            data["isproductoverridden"] = queueUnit.isproductoverridden;
            data["ispriceoverridden"] = queueUnit.ispriceoverridden;
            data["priceperunit"] = queueUnit.priceperunit;
            data["quantity"] = 1;

            data["opportunityid@odata.bind"] = $"/opportunities({queueUnit.bsd_booking})";
            if (!string.IsNullOrWhiteSpace(queueUnit.bsd_pricelist))
            {
                data["bsd_PriceList@odata.bind"] = $"/pricelevels({queueUnit.bsd_pricelist})";
            }

            if (!string.IsNullOrWhiteSpace(queueUnit.bsd_booking))
            {
                data["bsd_Booking@odata.bind"] = $"/opportunities({queueUnit.bsd_booking})";
            }

            if (!string.IsNullOrWhiteSpace(queueUnit.bsd_project))
            {
                data["bsd_Project@odata.bind"] = $"/bsd_projects({queueUnit.bsd_project})";
            }

            if (!string.IsNullOrWhiteSpace(queueUnit.bsd_block))
            {
                data["bsd_Block@odata.bind"] = $"/bsd_blocks({queueUnit.bsd_block})";
            }

            if (!string.IsNullOrWhiteSpace(queueUnit.bsd_units))
            {
                data["bsd_Units@odata.bind"] = $"/products({queueUnit.bsd_units})";
            }

            if (!string.IsNullOrWhiteSpace(queueUnit.bsd_phaseslaunch))
            {
                data["bsd_PhasesLaunch@odata.bind"] = $"/bsd_phaseslaunchs({queueUnit.bsd_phaseslaunch})";
            }

            if (!string.IsNullOrWhiteSpace(queueUnit.bsd_floor))
            {
                data["bsd_Floor@odata.bind"] = $"/bsd_floors({queueUnit.bsd_floor})";
            }
            
            if (!string.IsNullOrWhiteSpace(queueUnit.uomid))
            {
                data["uomid@odata.bind"] = $"/products({queueUnit.uomid})";
            }

            if (!string.IsNullOrWhiteSpace(queueUnit.productid))
            {
                data["productid@odata.bind"] = $"/products({queueUnit.productid})";
            }

            if (!string.IsNullOrWhiteSpace(queueUnit.transactioncurrencyid))
            {
                data["transactioncurrencyid@odata.bind"] = $"/transactioncurrencies({queueUnit.transactioncurrencyid})";
            }

            if (!string.IsNullOrWhiteSpace(queueUnit.createdby))
            {
                data["createdby@odata.bind"] = $"/systemusers({queueUnit.createdby})";
            }

            return data;
        }

        public async Task LoadContactsLookUp()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='contactid' alias='Id' />
                    <attribute name='fullname' alias='Name' />
                    <order attribute='createdon' descending='true' />                   
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
                                       <condition attribute='bsd_businesstypesys' operator='contain-values'>
                                         <value>100000002</value>
                                       </condition>                                
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
            if (UserLogged.ManagerId != Guid.Empty)
            {
                data["ownerid@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            }
            if (QueueFormModel.statuscode != 0)
            {
                data["statuscode"] = QueueFormModel.statuscode;
                data["bsd_bookingtime"] = QueueFormModel._queue_bsd_bookingtime;
                data["bsd_queuingexpired"] = QueueFormModel._queue_bsd_queuingexpired;
                data["bsd_queueforproject"] = false;
            }
            else
            {
                data["bsd_queueforproject"] = true;
            }
            data["createdon"] = QueueFormModel._queue_createdon;

            return data;
        }

        public async Task<Boolean> DeletLookup(string fieldName, Guid AccountId)
        {
            var result = await CrmHelper.SetNullLookupField("opportunities", AccountId, fieldName);
            return result.IsSuccess;
        }

        private async void UpdateStatusUnit()
        {
            if (QueueFormModel.UnitStatusCode == 1 || QueueFormModel.UnitStatusCode == 100000000)
            {
                await updateStatusUnit();
            }
        }

        public async Task<Boolean> updateStatusUnit()
        {
            string path = "/products(" + QueueFormModel.bsd_units_id + ")";
            var content = await this.getContentUnit();
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

        private async Task<object> getContentUnit()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["statecode"] = 0;
            data["statuscode"] = 100000004;
            return data;
        }
    }
}
