using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class DirectSaleDetailViewModel :BaseViewModel 
    {
        public string Keyword { get; set; }
        public string ProjectId { get; set; }
        public string PhasesLanchId { get; set; }
        public bool IsEvent { get; set; }
        public string UnitCode { get; set; }
        public List<string> Directions { get; set; }
        public List<string> Views { get; set; }
        public List<string> UnitStatuses { get; set; }
        public decimal? minNetArea { get; set; }
        public decimal? maxNetArea { get; set; }
        public decimal? minPrice { get; set; }
        public decimal? maxPrice { get; set; }

        private OptionSet _statusReason;
        public OptionSet StatusReason { get => _statusReason; set { _statusReason = value; OnPropertyChanged(nameof(StatusReason)); } }

        private List<OptionSet> _statusReasons;
        public List<OptionSet> StatusReasons { get => _statusReasons; set { _statusReasons = value; OnPropertyChanged(nameof(StatusReasons)); } }

        private List<Block> _blocks;
        public List<Block> Blocks { get=> _blocks; set { _blocks = value;OnPropertyChanged(nameof(Blocks)); } }

        public ObservableCollection<Floor> Floors { get; set; } = new ObservableCollection<Floor>();

        private ObservableCollection<QueueListModel> _queueList;
        public ObservableCollection<QueueListModel> QueueList { get => _queueList; set { _queueList = value; OnPropertyChanged(nameof(QueueList)); } }

        public string fetchXml { get; set; }

        private int _numChuanBiInBlock;
        public int NumChuanBiInBlock { get => _numChuanBiInBlock; set { _numChuanBiInBlock = value;OnPropertyChanged(nameof(NumChuanBiInBlock)); } }

        private int _numSanSangInBlock;
        public int NumSanSangInBlock { get => _numSanSangInBlock; set { _numSanSangInBlock = value; OnPropertyChanged(nameof(NumSanSangInBlock)); } }

        private int _numGiuChoInBlock;
        public int NumGiuChoInBlock { get => _numGiuChoInBlock; set { _numGiuChoInBlock = value; OnPropertyChanged(nameof(NumGiuChoInBlock)); } }

        private int _numDatCocInBlock;
        public int NumDatCocInBlock { get => _numDatCocInBlock; set { _numDatCocInBlock = value; OnPropertyChanged(nameof(NumDatCocInBlock)); } }

        private int _numDongYChuyenCoInBlock;
        public int NumDongYChuyenCoInBlock { get => _numDongYChuyenCoInBlock; set { _numDongYChuyenCoInBlock = value; OnPropertyChanged(nameof(NumDongYChuyenCoInBlock)); } }

        private int _numDaDuTienCocInBlock;
        public int NumDaDuTienCocInBlock { get => _numDaDuTienCocInBlock; set { _numDaDuTienCocInBlock = value; OnPropertyChanged(nameof(NumDaDuTienCocInBlock)); } }

        private int _numThanhToanDot1InBlock;
        public int NumThanhToanDot1InBlock { get => _numThanhToanDot1InBlock; set { _numThanhToanDot1InBlock = value; OnPropertyChanged(nameof(NumThanhToanDot1InBlock)); } }

        private int _numDaBanInBlock;
        public int NumDaBanInBlock { get => _numDaBanInBlock; set { _numDaBanInBlock = value; OnPropertyChanged(nameof(NumDaBanInBlock)); } }

        public Guid blockId;

        private Unit _unit;
        public Unit Unit { get => _unit; set { _unit = value;OnPropertyChanged(nameof(Unit)); } }

        private StatusCodeModel _unitStatusCode;
        public StatusCodeModel UnitStatusCode { get => _unitStatusCode; set { _unitStatusCode = value;OnPropertyChanged(nameof(UnitStatusCode)); } }

        private OptionSet _unitDirection;
        public OptionSet UnitDirection { get => _unitDirection; set { _unitDirection = value;OnPropertyChanged(nameof(UnitDirection)); } }

        private OptionSet _unitView;
        public OptionSet UnitView { get => _unitView; set { _unitView = value; OnPropertyChanged(nameof(UnitView)); } }

        public int PageDanhSachDatCho = 1;

        private bool _showMoreDanhSachDatCho;
        public bool ShowMoreDanhSachDatCho { get => _showMoreDanhSachDatCho; set { _showMoreDanhSachDatCho = value; OnPropertyChanged(nameof(ShowMoreDanhSachDatCho)); } }

        private bool _isShowBtnBangTinhGia;
        public bool IsShowBtnBangTinhGia { get => _isShowBtnBangTinhGia; set { _isShowBtnBangTinhGia = value; OnPropertyChanged(nameof(IsShowBtnBangTinhGia)); } }


        public DirectSaleDetailViewModel(DirectSaleSearchModel model)
        {
            this.ProjectId = model.ProjectId;
            this.PhasesLanchId = model.PhasesLanchId;
            this.IsEvent = model.IsEvent.Value;
            this.UnitCode = model.UnitCode;
            this.Directions = model.Directions;
            this.Views = model.Views;
            this.UnitStatuses = model.UnitStatuses;
            this.minNetArea = model.minNetArea;
            this.maxNetArea = model.maxNetArea;
            this.minPrice = model.minPrice;
            this.maxPrice = model.maxPrice;
            QueueList = new ObservableCollection<QueueListModel>();
        }

        public async Task LoadUnit()
        {
            string StatusReason_Condition = StatusReason == null ? "" : "<condition attribute='statuscode' operator='eq' value='" + StatusReason.Val + @"' />";
            string PhasesLaunch_Condition = (!string.IsNullOrWhiteSpace(PhasesLanchId))
                ? @"<condition attribute='bsd_phaseslaunchid' operator='eq' uitype='bsd_phaseslaunch' value='" + this.PhasesLanchId + @"' />"
                : "";

            string IsEvent_Condition = IsEvent ? @"<condition entityname='af' attribute='bsd_eventid' operator='not-null'/>" : "";

            string UnitCode_Condition = !string.IsNullOrEmpty(UnitCode) ? "<condition attribute='name' operator='eq' value='" + UnitCode + "' />" : "";

            string Direction_Condition = string.Empty;
            if (Directions != null && Directions.Count != 0)
            {
                string tmp = string.Empty;
                foreach (var i in Directions)
                {
                    tmp += "<value>" + i + "</value>";
                }
                Direction_Condition = @"<condition attribute='bsd_direction' operator='in'>" + tmp + "</condition>";
            }

            string View_Condition = string.Empty;
            if (Views != null && Views.Count != 0)
            {
                string tmp = string.Empty;
                foreach (var i in Views)
                {
                    tmp += "<value>" + i + "</value>";
                }
                View_Condition = @"<condition attribute='bsd_view' operator='in'>" + tmp + "</condition>";
            }


            string UnitStatus_Condition = string.Empty;
            if (UnitStatuses != null && UnitStatuses.Count != 0)
            {
                string tmp = string.Empty;
                foreach (var i in UnitStatuses)
                {
                    tmp += "<value>" + i + "</value>";
                }
                UnitStatus_Condition = @"<condition attribute='statuscode' operator='in'>" + tmp + "</condition>";
            }

            string minNetArea_Condition = minNetArea.HasValue ? $"<condition attribute='bsd_netsaleablearea' operator='ge' value='{minNetArea.Value}' />" : null;
            string maxNetArea_Condition = maxNetArea.HasValue ? $"<condition attribute='bsd_netsaleablearea' operator='le' value='{maxNetArea.Value}' />" : "";

            string minPrice_Condition = minPrice.HasValue ? $"<condition attribute='price' operator='ge' value='{minPrice.Value}' />" : "";
            string maxPrice_Condition = maxPrice.HasValue ? $"<condition attribute='price' operator='le' value='{maxPrice.Value}' />" : "";

            string Block_Condition = this.blockId != Guid.Empty ? $"<condition attribute='bsd_blocknumber' operator='eq' uitype='bsd_block' value='{this.blockId}'/>" : "";
            //string Floor_Condition = Floor != null ? $"<condition attribute='bsd_floorid' operator='eq' value='{Floor.Val}'/>" : "";

            fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='product'>
                                <attribute name='productid' />
                                <attribute name='name' />
                                <attribute name='statuscode' />
                                <attribute name='bsd_totalprice' />
                                <attribute name='price' />
                                <attribute name='bsd_netsaleablearea' />
                                <attribute name='bsd_constructionarea' />
                                <attribute name='bsd_direction' />
                                <attribute name='bsd_vippriority' />
                                <attribute name='bsd_view' />
                                <order attribute='bsd_blocknumber' descending='false' />
                                <filter type='and'>
                                    <condition attribute='statuscode' operator='ne' value='0' />
                                    <condition attribute='bsd_projectcode' operator='eq' uitype='bsd_project' value='" + this.ProjectId + @"'/>
                                    '" + Block_Condition + @"'
                                    '" + UnitCode_Condition + @"'
                                    '" + IsEvent_Condition + @"'
                                    '" + StatusReason_Condition + @"'
                                    '" + PhasesLaunch_Condition + @"'
                                    '" + View_Condition + @"'
                                    '" + UnitStatus_Condition + @"'
                                    '" + Direction_Condition + @"'
                                    '" + minNetArea_Condition + @"'
                                    '" + maxNetArea_Condition + @"'
                                    '" + minPrice_Condition + @"'
                                    '" + maxPrice_Condition + @"'
                                </filter>
                                <link-entity name='bsd_floor' count='10' page='1' from='bsd_floorid' to='bsd_floor' link-type='inner' alias='ad'>
                                  <attribute name='bsd_floorid' alias='floorid' />
                                  <attribute name='bsd_name' alias='floor_name' />
                                </link-entity>
                                <link-entity name='opportunity' from='bsd_units' to='productid' link-type='outer' alias='ag' >
	                                <attribute name='opportunityid' alias='queseid'/>
                                    <attribute name='statuscode' alias='queses_statuscode'/>
                                </link-entity>
                                <link-entity name='bsd_unittype' from='bsd_unittypeid' to='bsd_unittype' visible='false' link-type='outer' alias='a_493690ec6ce2e811a94e000d3a1bc2d1'>
                                  <attribute name='bsd_name'  alias='bsd_unittype_name'/>
                                </link-entity>
                                <link-entity name='bsd_block' from='bsd_blockid' to='bsd_blocknumber' link-type='inner' alias='ab'>
                                  <attribute name='bsd_blockid' alias='blockid' />
                                </link-entity>
                              </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Unit>>("products", fetchXml);

            if (result == null || result.value.Any() == false) return;

            #region Tinh so luong unit trong 1 block
            List<Unit> unitInBlock = new List<Unit>();
            if (this.blockId != Guid.Empty) // truong hop khi tu gio hang khoong chon DMB va khoong nhap san pham
            {
                unitInBlock = result.value.GroupBy(x => new
                {
                    unitid = x.productid,
                    statuscode = x.statuscode
                }).Select(y => y.First()).ToList();
            }
            else
            {
                this.blockId = result.value.FirstOrDefault().blockid;
                unitInBlock = result.value.Where(x => x.blockid == blockId).GroupBy(y => new
                {
                    unitid = y.productid,
                    statuscode = y.statuscode
                }).Select(z => z.First()).ToList();
            }
            for (int i = 0; i < unitInBlock.Count; i++)
            {
                switch (unitInBlock[i].statuscode)
                {
                    case 1:
                        NumChuanBiInBlock++;
                        break;
                    case 100000000:
                        NumSanSangInBlock++;
                        break;
                    case 100000004:
                        NumGiuChoInBlock++;
                        break;
                    case 100000006:
                        NumDatCocInBlock++;
                        break;
                    case 100000005:
                        NumDongYChuyenCoInBlock++;
                        break;
                    case 100000003:
                        NumDaDuTienCocInBlock++;
                        break;
                    case 100000001:
                        NumThanhToanDot1InBlock++;
                        break;
                    case 100000002:
                        NumDaBanInBlock++;
                        break;
                    default:
                        break;
                }
            }
            #endregion

            #region Tinh so luong unit trong 1 floor. Danh sach unit trong 1 floor
            List<Unit> unitsGroupByFloor = unitInBlock.GroupBy(i => new
            {
                floorId = i.floorid,
            }).Select(x => x.First()).ToList();

            // lay danh sach unit co nhung trang thai giu cho la: queuing, waiting,completed
            List<Unit> listUnitByQueue = result.value.Where(x => x.queses_statuscode == "100000000" || x.queses_statuscode == "100000002" || x.queses_statuscode == "100000004").ToList();

            for (int i = 0; i < unitsGroupByFloor.Count; i++)
            {
                Floor floor = new Floor();
                floor.bsd_floorid = unitsGroupByFloor[i].floorid;
                floor.bsd_name = unitsGroupByFloor[i].floor_name;

                var units = result.value.Where(x => x.floorid == unitsGroupByFloor[i].floorid);
                var unitGroupBy = units.GroupBy(x => new
                {
                    unitid = x.productid,
                    statuscode = x.statuscode
                }).Select(y => y.First()).ToList();
                foreach (var unit in unitGroupBy)
                {
                    switch (unit.statuscode) // dem so luong trang thai cua unit
                    {
                        case 1:
                            floor.NumChuanBiInFloor++;
                            break;
                        case 100000000:
                            floor.NumSanSangInFloor++;
                            break;
                        case 100000004:
                            floor.NumGiuChoInFloor++;
                            break;
                        case 100000006:
                            floor.NumDatCocInFloor++;
                            break;
                        case 100000005:
                            floor.NumDongYChuyenCoInFloor++;
                            break;
                        case 100000003:
                            floor.NumDaDuTienCocInFloor++;
                            break;
                        case 100000001:
                            floor.NumThanhToanDot1InFloor++;
                            break;
                        case 100000002:
                            floor.NumDaBanInFloor++;
                            break;
                        default:
                            break;
                    }

                    int count = 0;
                    foreach (var unitbyQueue in listUnitByQueue) // dem so giu cho cua unit
                    {
                        if (unitbyQueue.productid == unit.productid)
                        {
                            count++;
                        }
                    }
                    unit.NumQueses = count;

                    if (unit.statuscode.HasValue)// set backgroundcolor cho tung unit
                    {
                        unit.item_background = StatusCodeUnit.GetStatusCodeById(unit.statuscode.Value.ToString()).Background;
                    }

                    floor.Units.Add(unit);
                }

                Floors.Add(floor);
            }
            #endregion
        }

        public async Task LoadBlocks()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_block'>
                                <attribute name='bsd_name' />
                                <attribute name='bsd_project' />
                                <attribute name='bsd_blockid' />
                                <order attribute='bsd_name' descending='false' />
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='inner' alias='aa'>
                                  <filter type='and'>
                                    <condition attribute='bsd_projectid' operator='like' value='{this.ProjectId}' />
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";

            var block_result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Block>>("bsd_blocks", fetchXml);
            if (block_result == null || block_result.value.Count == 0) return;

            this.Blocks = block_result.value;
        }

        public async Task LoadQueues()
        {
            string fetch = $@"<fetch version='1.0' count='5' page='{PageDanhSachDatCho}' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='opportunity'>
                        <attribute name='name' alias='unit_name'/>
                        <attribute name='statuscode' />
                        <attribute name='bsd_project' />
                        <attribute name='opportunityid' />
                        <attribute name='bsd_queuingexpired' />
                        <order attribute='createdon' descending='true' />
                        <link-entity name='product' from='productid' to='bsd_units' link-type='inner' alias='ad'>
                          <filter type='and'>
                            <condition attribute='productid' operator='eq' value='{Unit.productid}'/>
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
                      </entity>
                    </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueueListModel>>("opportunities", fetch);
            if (result == null)
            {
                return;
            }

            var data = result.value;
            ShowMoreDanhSachDatCho = data.Count < 5 ? false : true;

            foreach (var x in data)
            {
                x.statuscode_label = QueuesStatusCodeData.GetQueuesById(x.statuscode.ToString()).Name;
                QueueList.Add(x);
            }
        }

        public async Task CheckShowBtnBangTinhGia()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                              <entity name='bsd_phaseslaunch'>
                                <attribute name='bsd_name' />
                                <attribute name='createdon' />
                                <attribute name='bsd_phaseslaunchid' />
                                <order attribute='createdon' descending='true' />
                                <link-entity name='product' from='bsd_phaseslaunchid' to='bsd_phaseslaunchid' link-type='inner' alias='al'>
                                  <filter type='and'>
                                    <condition attribute='productid' operator='eq' value='{Unit.productid}' />
                                  </filter>
                                </link-entity>
                                <link-entity name='bsd_event' from='bsd_phaselaunch' to='bsd_phaseslaunchid' link-type='inner' alias='an' >
                                   <attribute name='bsd_startdate' alias='startdate_event' />
                                   <attribute name='bsd_enddate' alias='enddate_event'/>
                                </link-entity>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhasesLanchModel>>("bsd_phaseslaunchs", fetchXml);
            if (result == null || result.value.Any() == false) return;

            var data = result.value;
            foreach (var item in data)
            {
                if (item.startdate_event < DateTime.Now && item.enddate_event > DateTime.Now)
                {
                    IsShowBtnBangTinhGia = true;
                    return;
                }
                else
                {
                    IsShowBtnBangTinhGia = false;
                }
            }
        }
    }
}
