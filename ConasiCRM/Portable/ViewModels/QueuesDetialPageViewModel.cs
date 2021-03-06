using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Resources;
using ConasiCRM.Portable.Settings;

namespace ConasiCRM.Portable.ViewModels
{
    public class QueuesDetialPageViewModel : BaseViewModel
    {
        public Guid QueueId { get; set; }
        public string NumPhone { get; set; }

        private ObservableCollection<ReservationListModel> _bangTinhGiaList;
        public ObservableCollection<ReservationListModel> BangTinhGiaList { get => _bangTinhGiaList; set { _bangTinhGiaList = value; OnPropertyChanged(nameof(BangTinhGiaList)); } }

        private ObservableCollection<ReservationListModel> _datCocList;
        public ObservableCollection<ReservationListModel> DatCocList { get => _datCocList; set { _datCocList = value; OnPropertyChanged(nameof(DatCocList)); } }

        private ObservableCollection<ContractModel> _hopDongList;
        public ObservableCollection<ContractModel> HopDongList { get => _hopDongList; set { _hopDongList = value; OnPropertyChanged(nameof(HopDongList)); } }

        private QueuesDetailModel _queue;
        public QueuesDetailModel Queue { get => _queue; set { _queue = value; OnPropertyChanged(nameof(Queue)); } }

        private OptionSet _customer;
        public OptionSet Customer { get => _customer; set { _customer = value; OnPropertyChanged(nameof(Customer)); } }

        private QueuesStatusCodeModel _queueStatusCode;
        public QueuesStatusCodeModel QueueStatusCode { get => _queueStatusCode; set { _queueStatusCode = value; OnPropertyChanged(nameof(QueueStatusCode)); } }

        private string _queueProject;
        public string QueueProject { get => _queueProject; set { _queueProject = value; OnPropertyChanged(nameof(QueueProject)); } }

        private bool _showBtnHuyGiuCho;
        public bool ShowBtnHuyGiuCho { get => _showBtnHuyGiuCho; set { _showBtnHuyGiuCho = value; OnPropertyChanged(nameof(ShowBtnHuyGiuCho)); } }

        private bool _showBtnBangTinhGia;
        public bool ShowBtnBangTinhGia { get => _showBtnBangTinhGia; set { _showBtnBangTinhGia = value; OnPropertyChanged(nameof(ShowBtnBangTinhGia)); } }

        private bool _showButtons;
        public bool ShowButtons { get => _showButtons; set { _showButtons = value; OnPropertyChanged(nameof(ShowButtons)); } }

        private bool _showMoreBangTinhGia;
        public bool ShowMoreBangTinhGia { get => _showMoreBangTinhGia; set { _showMoreBangTinhGia = value; OnPropertyChanged(nameof(ShowMoreBangTinhGia)); } }

        private bool _showMoreDatCoc;
        public bool ShowMoreDatCoc { get => _showMoreDatCoc; set { _showMoreDatCoc = value; OnPropertyChanged(nameof(ShowMoreDatCoc)); } }

        private bool _showMoreHopDong;
        public bool ShowMoreHopDong { get => _showMoreHopDong; set { _showMoreHopDong = value; OnPropertyChanged(nameof(ShowMoreHopDong)); } }

        public int PageBangTinhGia { get; set; } = 1;
        public int PageDatCoc { get; set; } = 1;
        public int PageHopDong { get; set; } = 1;

        public string CodeContact = "2";

        public string CodeAccount = "3";

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
                                    <attribute name='accountid' alias='account_id'/>
                                    <attribute name='telephone1' alias='PhoneAccount'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='a_884f5ec290d1eb11bacc000d3a80021e'>
                                    <attribute name='bsd_fullname' alias='contact_name' />
                                    <attribute name='contactid' alias='contact_id'/>
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
                Customer = new OptionSet() { Val= data.account_id.ToString(), Label = data.account_name, Title= CodeAccount };
            }
            else if (!string.IsNullOrWhiteSpace(data.contact_name))
            {
                Customer = new OptionSet() { Val = data.contact_id.ToString(), Label = data.contact_name, Title = CodeContact }; ;
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
                QueueProject = Language.khong;// "Không";
            }
            else
            {
                QueueProject = Language.co; //"Có";
            }

            ShowBtnHuyGiuCho = (data.statuscode == 100000000 || data.statuscode == 100000002) ? true : false;
            ShowBtnBangTinhGia = (data.statuscode == 100000000 && !string.IsNullOrWhiteSpace(data.phaselaunch_name)) ? true : false;

            this.QueueStatusCode = QueuesStatusCodeData.GetQueuesById(data.statuscode.ToString());

            this.Queue = data;
        }
        public async Task LoadDanhSachBangTinhGia()
        {
            string fetchXml = $@"<fetch version='1.0' count='5' page='{PageBangTinhGia}' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='quote'>
                        <attribute name='name' />
                        <attribute name='totalamount' />
                        <attribute name='bsd_unitno' alias='bsd_unitno_id' />
                        <attribute name='statuscode' />
                        <attribute name='bsd_projectid' alias='bsd_project_id' />
                        <attribute name='quoteid' />
                        <order attribute='createdon' descending='true' />
                        <filter type='and'>
                            <condition attribute='opportunityid' operator='like'  value='{this.Queue.opportunityid}' />
                            <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}'/>
                            <filter type='or'>
                              <condition attribute='statuscode' operator='in'>
                                <value>100000007</value>
                              </condition>
                              <filter type='and'>
                                 <condition attribute='statuscode' operator='in'>
                                    <value>100000009</value>
                                    <value>6</value>
                                  </condition>
                                  <condition attribute='bsd_quotationsigneddate' operator='null' />
                              </filter>
                            </filter>
                        </filter>
                        <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' visible='false' link-type='outer' alias='a'>
                            <attribute name='bsd_name' alias='bsd_project_name' />
                        </link-entity>
                        <link-entity name='product' from='productid' to='bsd_unitno' visible='false' link-type='outer' alias='b'>
                          <attribute name='name' alias='bsd_unitno_name' />
                        </link-entity>
                        <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='c'>
                          <attribute name='bsd_name' alias='purchaser_accountname' />
                        </link-entity>
                        <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='d'>
                          <attribute name='bsd_fullname' alias='purchaser_contactname' />
                        </link-entity>
                      </entity>
                    </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationListModel>>("quotes", fetchXml);
            if (result == null || result.value.Any() == false) return;

            this.ShowMoreBangTinhGia = result.value.Count > 4 ? true : false;

            foreach (var item in result.value)
            {
                this.BangTinhGiaList.Add(item);
            }
        }
        public async Task CheckReserve()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='quote'>
                        <attribute name='name' alias='Label'/>
                        <filter type='and'>
                            <condition attribute='opportunityid' operator='like'  value='{this.Queue.opportunityid}' />
                            <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}'/>
                            <condition attribute='statuscode' operator='in'>
                                   <value>100000000</value>
                                   <value>100000001</value>
                                   <value>100000006</value>
                                   <value>3</value>
                                   <value>4</value>
                               </condition>
                        </filter>
                      </entity>
                    </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("quotes", fetchXml);
            if (result == null) return;
            if (result.value.Any() == false && this.Queue.statuscode == 100000000)
            {
                ShowBtnBangTinhGia = true;
            }
            else
            {
                ShowBtnBangTinhGia = false;
            }
        }
        public async Task LoadDanhSachDatCoc()
        {
            string fetchXml = $@"<fetch version='1.0' count='5' page='{PageDatCoc}' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='quote'>
                        <attribute name='name' />
                        <attribute name='totalamount' />
                        <attribute name='bsd_unitno' alias='bsd_unitno_id' />
                        <attribute name='statuscode' />
                        <attribute name='bsd_projectid' alias='bsd_project_id' />
                        <attribute name='quoteid' />
                        <order attribute='createdon' descending='true' />
                        <filter type='and'>
                            <condition attribute='opportunityid' operator='like'  value='{this.Queue.opportunityid}' />
                            <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}'/>
                            <filter type='or'>
                               <condition attribute='statuscode' operator='in'>
                                   <value>100000000</value>
                                   <value>100000001</value>
                                   <value>100000006</value>
                                   <value>3</value>
                                   <value>4</value>
                               </condition>
                               <filter type='and'>
                                   <condition attribute='statuscode' operator='in'>
                                       <value>100000009</value>
                                       <value>6</value>
                                   </condition>
                                   <condition attribute='bsd_quotationsigneddate' operator='not-null' />
                               </filter>
                             </filter>
                        </filter>
                        <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' visible='false' link-type='outer' alias='a'>
                            <attribute name='bsd_name' alias='bsd_project_name' />
                        </link-entity>
                        <link-entity name='product' from='productid' to='bsd_unitno' visible='false' link-type='outer' alias='b'>
                          <attribute name='name' alias='bsd_unitno_name' />
                        </link-entity>
                        <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='c'>
                          <attribute name='bsd_name' alias='purchaser_accountname' />
                        </link-entity>
                        <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='d'>
                          <attribute name='bsd_fullname' alias='purchaser_contactname' />
                        </link-entity>
                      </entity>
                    </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationListModel>>("quotes", fetchXml);
            if (result == null || result.value.Any() == false) return;

            this.ShowMoreDatCoc = result.value.Count > 4 ? true : false;

            foreach (var item in result.value)
            {
                this.DatCocList.Add(item);
            }
        }
        public async Task LoadDanhSachHopDong()
        {
            string fetchXml = $@"<fetch version='1.0' count='5' page='{PageHopDong}' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='salesorder'>
                                    <attribute name='name' />
                                    <attribute name='customerid' />
                                    <attribute name='statuscode' />
                                    <attribute name='totalamount' />
                                    <attribute name='bsd_unitnumber' alias='unit_id'/>
                                    <attribute name='bsd_project' alias='project_id'/>
                                    <attribute name='salesorderid' />
                                    <attribute name='ordernumber' />
                                    <order attribute='bsd_project' descending='true' />
                                    <filter type='and'>                                      
                                        <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}'/>
                                        <condition attribute='opportunityid' operator='eq' value='{this.Queue.opportunityid}'/>                
                                    </filter >
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='outer' alias='aa'>
                                        <attribute name='bsd_name' alias='project_name'/>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_unitnumber' link-type='outer' alias='ab'>
                                        <attribute name='name' alias='unit_name'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='customerid' link-type='outer' alias='ac'>
                                        <attribute name='name' alias='account_name'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='customerid' link-type='outer' alias='ad'>
                                        <attribute name='bsd_fullname' alias='contact_name'/>
                                    </link-entity>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContractModel>>("salesorders", fetchXml);
            if (result == null || result.value.Any() == false) return;
            this.ShowMoreHopDong = result.value.Count > 4 ? true : false;
            foreach (var item in result.value)
            {
                this.HopDongList.Add(item);
            }
        }

    }
}
