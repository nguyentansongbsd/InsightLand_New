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
    public class UnitInfoViewModel : BaseViewModel
    {
        public Guid UnitId { get; set; }

        public ObservableCollection<QueueListModel> list_danhsachdatcho { get; set; } = new ObservableCollection<QueueListModel>();
        public ObservableCollection<QuotationReseravtion> list_danhsachdatcoc { get; set; } = new ObservableCollection<QuotationReseravtion>();
        public ObservableCollection<OptionEntry> list_danhsachhopdong { get; set; } = new ObservableCollection<OptionEntry>();

        private UnitInfoModel _unitInfo;
        public UnitInfoModel UnitInfo { get => _unitInfo; set { _unitInfo = value; OnPropertyChanged(nameof(UnitInfo)); } }

        private OptionSet _diretion;
        public OptionSet Direction { get => _diretion; set { _diretion = value; OnPropertyChanged(nameof(Direction)); } }

        private OptionSet _view;
        public OptionSet View { get => _view; set { _view = value; OnPropertyChanged(nameof(View)); } }

        private StatusCodeModel _statusCode;
        public StatusCodeModel StatusCode { get => _statusCode; set { _statusCode = value; OnPropertyChanged(nameof(StatusCode)); } }

        private bool _showMoreDanhSachDatCho;
        public bool ShowMoreDanhSachDatCho { get => _showMoreDanhSachDatCho; set { _showMoreDanhSachDatCho = value; OnPropertyChanged(nameof(ShowMoreDanhSachDatCho)); } }

        private bool _showMoreDanhSachDatCoc;
        public bool ShowMoreDanhSachDatCoc { get => _showMoreDanhSachDatCoc; set { _showMoreDanhSachDatCoc = value; OnPropertyChanged(nameof(ShowMoreDanhSachDatCoc)); } }

        private bool _showMoreDanhSachHopDong;
        public bool ShowMoreDanhSachHopDong { get => _showMoreDanhSachHopDong; set { _showMoreDanhSachHopDong = value; OnPropertyChanged(nameof(ShowMoreDanhSachHopDong)); } }

        public int PageDanhSachHopDong { get; set; } = 1;
        public int PageDanhSachDatCoc { get; set; } = 1;
        public int PageDanhSachDatCho { get; set; } = 1;

        public bool IsLoaded { get; set; } = false;

        public bool IsVip { get; set; } = false;

        public UnitInfoViewModel()
        {

        }

        public async Task<bool> LoadUnit()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='product'>
                <attribute name='productid' />
                <attribute name='bsd_units' />
                <attribute name='name' />
                <attribute name='productnumber' />
                <attribute name='bsd_queuingfee' />
                <attribute name='bsd_depositamount' />
                <attribute name='statuscode' />
                <attribute name='bsd_areavariance' />
                <attribute name='bsd_constructionarea' />
                <attribute name='price' />
                <attribute name='bsd_landvalueofunit' />
                <attribute name='bsd_landvalue' />
                <attribute name='bsd_maintenancefeespercent' />
                <attribute name='bsd_maintenancefees' />
                <attribute name='bsd_taxpercent'/>
                <attribute name='bsd_vat' />
                <attribute name='bsd_estimatehandoverdate' />
                <attribute name='bsd_numberofmonthspaidmf' />
                <attribute name='bsd_managementamountmonth' />
                <attribute name='bsd_handovercondition' />
                <attribute name='bsd_direction' />
                <attribute name='bsd_vippriority' />
                <attribute name='bsd_view' />
                <filter type='and'>
                  <condition attribute='productid' operator='eq' uitype='product' value='" + UnitId.ToString() + @"' />
                </filter>
                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectcode' visible='false' link-type='outer' alias='a_a77d98e66ce2e811a94e000d3a1bc2d1'>
                  <attribute name='bsd_name' alias='bsd_project_name' />
                </link-entity>
                <link-entity name='bsd_floor' from='bsd_floorid' to='bsd_floor' visible='false' link-type='outer' alias='a_4d73a1e06ce2e811a94e000d3a1bc2d1'>
                  <attribute name='bsd_name' alias='bsd_floor_name' />
                </link-entity>
                <link-entity name='bsd_block' from='bsd_blockid' to='bsd_blocknumber' visible='false' link-type='outer' alias='a_290ca3da6ce2e811a94e000d3a1bc2d1'>
                  <attribute name='bsd_name' alias='bsd_block_name' />
                </link-entity>
                <link-entity name='bsd_unittype' from='bsd_unittypeid' to='bsd_unittype' visible='false' link-type='outer' alias='a_493690ec6ce2e811a94e000d3a1bc2d1'>
                  <attribute name='bsd_name'  alias='bsd_unittype_name'/>
                </link-entity>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<UnitInfoModel>>("products", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                return false;
            }
            else
            {
                UnitInfo = result.value.FirstOrDefault();
                return true;
            }
        }

        public async Task LoadQueuesForContactForm()
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageDanhSachDatCho}' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='opportunity'>
                        <attribute name='name' alias='unit_name'/>
                        <attribute name='customerid' />
                        <attribute name='estimatedvalue' />
                        <attribute name='statuscode' />
                        <attribute name='createdon' />
                        <attribute name='bsd_queuenumber' />
                        <attribute name='actualclosedate' />
                        <attribute name='bsd_project' />
                        <attribute name='opportunityid' />
                        <order attribute='createdon' descending='true' />
                        <link-entity name='product' from='productid' to='bsd_units' link-type='inner' alias='ad'>
                          <filter type='and'>
                            <condition attribute='productid' operator='eq' value='{UnitInfo.productid}'/>
                          </filter>
                        </link-entity>
                        <link-entity name='bsd_employee' from='bsd_employeeid' to='bsd_employee' link-type='inner' alias='ae'>
                           <filter type='and'>
                              <condition attribute='bsd_employeeid' operator='eq' value='{UserLogged.Id}'/>
                           </filter>
                        </link-entity>
                        <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer'>
                           <attribute name='fullname'  alias='contact_name'/>
                        </link-entity>
                        <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer'>
                           <attribute name='name'  alias='account_name'/>
                        </link-entity>
                        <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer'>
                           <attribute name='bsd_name'  alias='project_name'/>
                        </link-entity>
                        
                      </entity>
                    </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueueListModel>>("opportunities", fetch);
            if (result == null)
            {
                return;
            }
            IsLoaded = true;
            var data = result.value;
            ShowMoreDanhSachDatCho = data.Count < 3 ? false : true;

            foreach (var x in data)
            {
                list_danhsachdatcho.Add(x);
            }
        }

        public async Task LoadReservationForContactForm()
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageDanhSachDatCoc}' output-format='xml-platform' mapping='logical' distinct='false'>
                          <entity name='quote'>
                            <attribute name='quoteid' />
                            <attribute name='bsd_projectid' />
                            <attribute name='bsd_unitno' />
                            <attribute name='bsd_reservationno' />
                            <attribute name='customerid' />
                            <attribute name='statuscode' />
                            <attribute name='totalamount' />
                            <attribute name='createdon' />
                            <order attribute='createdon' descending='true' />
                            <link-entity name='product' from='productid' to='bsd_unitno' visible='false' link-type='outer' alias='a_d52436e819dbeb11bacb002248168cad'>
                              <filter type='and'>
                                    <condition attribute='productid' operator='eq' value='{UnitInfo.productid}'/>
                                 </filter>
                            </link-entity>
                            <link-entity name='bsd_employee' from='bsd_employeeid' to='bsd_employee' link-type='inner' alias='ae'>
                                <filter type='and'>
                                    <condition attribute='bsd_employeeid' operator='eq' value='{UserLogged.Id}'/>
                                </filter>
                            </link-entity>
                            <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer'>
                                <attribute name='fullname'  alias='customerid_label_contact'/>
                            </link-entity>
                            <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer'>
                                <attribute name='name'  alias='customerid_label_account'/>
                            </link-entity>
                            <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' visible='false' link-type='outer'>
                                <attribute name='bsd_name'  alias='bsd_projectid_label'/>
                            </link-entity>
                            <link-entity name='product' from='productid' to='bsd_unitno' visible='false' link-type='outer'>
                                <attribute name='name'  alias='bsd_unitno_label'/>
                            </link-entity>
                            <link-entity name='transactioncurrency' from='transactioncurrencyid' to='transactioncurrencyid' visible='false' link-type='outer'>
                                <attribute name='currencysymbol'  alias='transaction_currency'/>
                            </link-entity>
                          </entity>
                        </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QuotationReseravtion>>("quotes", fetch);
            if (result == null) return;
            IsLoaded = true;
            var data = result.value;
            ShowMoreDanhSachDatCoc = data.Count < 3 ? false : true;

            foreach (var x in data)
            {
                list_danhsachdatcoc.Add(x);
            }
        }

        public async Task LoadOptoinEntryForContactForm()
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageDanhSachHopDong}' output-format='xml-platform' mapping='logical' distinct='false'>
                          <entity name='salesorder'>
                            <attribute name='salesorderid' />
                            <attribute name='bsd_optionno' />
                            <attribute name='statuscode' />
                            <attribute name='totalamount' />
                            <attribute name='bsd_signingexpired' />
                            <attribute name='createdon' />
                            <order attribute='bsd_signingexpired' descending='true' />
                            <link-entity name='product' from='productid' to='bsd_unitnumber' visible='false' link-type='outer' alias='a_e42436e819dbeb11bacb002248168cad'>
                                 <filter type='and'>
                                    <condition attribute='productid' operator='eq' value='{UnitInfo.productid}'/>
                                 </filter>
                            </link-entity>
                            <link-entity name='bsd_employee' from='bsd_employeeid' to='bsd_employee' link-type='inner' alias='ae'>
                                <filter type='and'>
                                    <condition attribute='bsd_employeeid' operator='eq' value='{UserLogged.Id}'/>
                                </filter>
                            </link-entity>
                            <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer'>
                                <attribute name='fullname'  alias='customerid_label_contact'/>
                            </link-entity>
                            <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer'>
                                <attribute name='name'  alias='customerid_label_account'/>
                            </link-entity>
                            <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer'>
                                <attribute name='bsd_name'  alias='bsd_project_label'/>
                            </link-entity>
                            <link-entity name='transactioncurrency' from='transactioncurrencyid' to='transactioncurrencyid' visible='false' link-type='outer'>
                                <attribute name='currencysymbol'  alias='transactioncurrency'/>
                            </link-entity>
                            <link-entity name='product' from='productid' to='bsd_unitnumber' visible='false' link-type='outer'>
                                <attribute name='name'  alias='bsd_unitnumber_label'/>
                            </link-entity>
                          </entity>
                        </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionEntry>>("salesorders", fetch);
            if (result == null) return;

            IsLoaded = true;
            var data = result.value;
            ShowMoreDanhSachHopDong = data.Count < 3 ? false : true;

            foreach (var x in data)
            {
                list_danhsachhopdong.Add(x);
            }
        }
    }
}
