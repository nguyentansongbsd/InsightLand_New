using System;
using System.Linq;
using System.Threading.Tasks;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;

namespace ConasiCRM.Portable.ViewModels
{
    public class QueuesDetialPageViewModel : BaseViewModel
    {
        public Guid QueueId { get; set; }
        public string NumPhone { get; set; }
        private QueuesDetailModel _queue;
        public QueuesDetailModel Queue { get => _queue; set { _queue = value; OnPropertyChanged(nameof(Queue)); } }

        private string _customer;
        public string Customer { get => _customer; set { _customer = value; OnPropertyChanged(nameof(Customer)); } }

        private QueuesStatusCodeModel _queueStatusCode;
        public QueuesStatusCodeModel QueueStatusCode { get => _queueStatusCode; set { _queueStatusCode = value; OnPropertyChanged(nameof(QueueStatusCode)); } }

        private string _queueProject;
        public string QueueProject { get => _queueProject; set { _queueProject = value; OnPropertyChanged(nameof(QueueProject)); } }

        private bool _showBtnHuyGiuCho;
        public bool ShowBtnHuyGiuCho{ get => _showBtnHuyGiuCho; set { _showBtnHuyGiuCho = value; OnPropertyChanged(nameof(ShowBtnHuyGiuCho)); } }

        private bool _showBtnBangTinhGia;
        public bool ShowBtnBangTinhGia { get => _showBtnBangTinhGia; set { _showBtnBangTinhGia = value; OnPropertyChanged(nameof(ShowBtnBangTinhGia)); } }

        private bool _showButtons;
        public bool ShowButtons { get => _showButtons; set { _showButtons = value; OnPropertyChanged(nameof(ShowButtons)); } }

        public QueuesDetialPageViewModel()
        {
        }

        public async Task LoadQueue()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='opportunity'>
                                <attribute name='name' />
                                <attribute name='statuscode' />
                                <attribute name='createdon' />
                                <attribute name='bsd_bookingtime' />
                                <attribute name='bsd_queuingexpired' />
                                <attribute name='bsd_queuenumber' />
                                <attribute name='opportunityid' />
                                <attribute name='bsd_queuingfee' />
                                <attribute name='budgetamount' />
                                <attribute name='description' />
                                <attribute name='bsd_nameofstaffagent' />
                                <attribute name='bsd_project' />
                                <attribute name='bsd_units' alias='_bsd_units_value'/>
                                <attribute name='bsd_phaselaunch' />
                                <attribute name='bsd_salesagentcompany' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                  <condition attribute='opportunityid' operator='eq' value='{QueueId}'/>
                                </filter>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='a_805e44d019dbeb11bacb002248168cad'>
                                  <attribute name='bsd_name'  alias='project_name'/>
                                </link-entity>
                                <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='a_ba2436e819dbeb11bacb002248168cad'>
                                    <attribute name='name'  alias='unit_name'/>
                                </link-entity>
                                <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='a_434f5ec290d1eb11bacc000d3a80021e'>
                                    <attribute name='bsd_name' alias='account_name'/>
                                    <attribute name='telephone1' alias='PhoneAccount'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='a_884f5ec290d1eb11bacc000d3a80021e'>
                                  <attribute name='bsd_fullname' alias='contact_name' />
                                    <attribute name='mobilephone' alias='PhoneContact'/>
                                </link-entity>
                                <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaselaunch' visible='false' link-type='outer' alias='a_485347ca19dbeb11bacb002248168cad'>
                                  <attribute name='bsd_name' alias='phaselaunch_name'/>
                                </link-entity>
                                <link-entity name='account' from='accountid' to='bsd_salesagentcompany' visible='false' link-type='outer' alias='a_ab034cb219dbeb11bacb002248168cad'>
                                  <attribute name='bsd_name' alias='salesagentcompany_name'/>
                                </link-entity>
                              </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueuesDetailModel>>("opportunities", fetchXml);
            if (result == null || result.value.Any() == false) return;

            var data = result.value.FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(data.account_name))
            {
                Customer = data.account_name;
            }
            else if (!string.IsNullOrWhiteSpace(data.contact_name))
            {
                Customer = data.contact_name;
            }

            if (!string.IsNullOrWhiteSpace(data.PhoneAccount))
            {
                NumPhone = data.PhoneAccount;
            }
            else if (!string.IsNullOrWhiteSpace(data.PhoneContact))
            {
                NumPhone = data.PhoneContact;
            }

            if (data.unit_name != null)
            {
                QueueProject = "Không";
            }
            else
            {
                QueueProject = "Có";
            }

            ShowBtnHuyGiuCho = (data.statuscode == 100000000 || data.statuscode == 100000002) ? true : false;
            ShowBtnBangTinhGia = data.statuscode == 100000000 ? true : false;

            this.QueueStatusCode = QueuesStatusCodeData.GetQueuesById(data.statuscode.ToString());

            this.Queue = data;
        }
    }
}
