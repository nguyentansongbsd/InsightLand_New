using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telerik.XamarinForms.DataControls.TreeView.Commands;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class DirectSaleDetailViewModel :BaseViewModel //: ListViewBaseViewModel2<Unit>
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

        private OptionSet _block;
        public OptionSet Block { get => _block; set { _block = value; OnPropertyChanged(nameof(Block)); } }

        private List<Block> _blocks;
        public List<Block> Blocks { get=> _blocks; set { _blocks = value;OnPropertyChanged(nameof(Blocks)); } }

        //private OptionSet _floor;
        //public OptionSet Floor { get => _floor; set { _floor = value; OnPropertyChanged(nameof(Floor)); } }

        //private List<Floor> _floors;
        //public List<Floor> Floors { get => _floors; set { _floors = value; OnPropertyChanged(nameof(Floors)); } }

        public ObservableCollection<Floor> Floors { get; set; } = new ObservableCollection<Floor>();

        public ObservableCollection<QueueListModel_DirectSale> QueueList { get; set; }

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

        public string blockId;


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
            QueueList = new ObservableCollection<QueueListModel_DirectSale>();
            

        }

        public async Task LoadUnit()
        {
            //string StatusReason_Condition = StatusReason == null ? "" : "<condition attribute='statuscode' operator='eq' value='" + StatusReason.Val + @"' />";
            //string PhasesLaunch_Condition = (!string.IsNullOrWhiteSpace(PhasesLanchId))
            //    ? @"<condition attribute='bsd_phaseslaunchid' operator='eq' uitype='bsd_phaseslaunch' value='" + this.PhasesLanchId + @"' />"
            //    : "";

            //string IsEvent_Condition = IsEvent ? @"<condition entityname='af' attribute='bsd_eventid' operator='not-null'/>" : "";

            //string UnitCode_Condition = !string.IsNullOrEmpty(UnitCode) ? "<condition attribute='name' operator='like' value='%" + UnitCode + "%' />" : "";

            //string Direction_Condition = string.Empty;
            //if (Directions.Count != 0)
            //{
            //    string tmp = string.Empty;
            //    foreach (var i in Directions)
            //    {
            //        tmp += "<value>" + i + "</value>";
            //    }
            //    Direction_Condition = @"<condition attribute='bsd_direction' operator='in'>" + tmp + "</condition>";
            //}

            //string View_Condition = string.Empty;
            //if (Views.Count != 0)
            //{
            //    string tmp = string.Empty;
            //    foreach (var i in Views)
            //    {
            //        tmp += "<value>" + i + "</value>";
            //    }
            //    View_Condition = @"<condition attribute='bsd_view' operator='in'>" + tmp + "</condition>";
            //}


            //string UnitStatus_Condition = string.Empty;
            //if (UnitStatuses.Count != 0)
            //{
            //    string tmp = string.Empty;
            //    foreach (var i in UnitStatuses)
            //    {
            //        tmp += "<value>" + i + "</value>";
            //    }
            //    UnitStatus_Condition = @"<condition attribute='statuscode' operator='in'>" + tmp + "</condition>";
            //}

            //string minNetArea_Condition = minNetArea.HasValue ? $"<condition attribute='bsd_netsaleablearea' operator='ge' value='{minNetArea.Value}' />" : null;
            //string maxNetArea_Condition = maxNetArea.HasValue ? $"<condition attribute='bsd_netsaleablearea' operator='le' value='{maxNetArea.Value}' />" : "";

            //string minPrice_Condition = minPrice.HasValue ? $"<condition attribute='price' operator='ge' value='{minPrice.Value}' />" : "";
            //string maxPrice_Condition = maxPrice.HasValue ? $"<condition attribute='price' operator='le' value='{maxPrice.Value}' />" : "";

            //string Block_Condition = Block != null ? $"<condition attribute='bsd_blockid' operator='eq' value='{Block.Val}'/>" : "";
            //string Floor_Condition = Floor != null ? $"<condition attribute='bsd_floorid' operator='eq' value='{Floor.Val}'/>" : "";

            fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='product'>
                                <attribute name='productid' />
                                <attribute name='name' />
                                <attribute name='statuscode' />
                                <attribute name='bsd_totalprice' />
                                <attribute name='price' />
                                <attribute name='bsd_netsaleablearea' />
                                <order attribute='bsd_blocknumber' descending='false' />
                                <filter type='and'>
                                    <condition attribute='bsd_projectcode' operator='eq' uitype='bsd_project' value='" + this.ProjectId + @"'/>
                                    <condition attribute='bsd_blocknumber' operator='eq' uitype='bsd_block' value='" + this.blockId + @"'/>
                                </filter>
                                <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaseslaunchid' link-type='outer' alias='ae'>
                                  <link-entity name='bsd_event' from='bsd_phaselaunch' to='bsd_phaseslaunchid' link-type='outer' alias='af'>
                                      <attribute name='bsd_eventid' alias='event_id' />                                       
                                   </link-entity>
                                </link-entity>
                                <link-entity name='bsd_floor' from='bsd_floorid' to='bsd_floor' link-type='inner' alias='ad'>
                                  <attribute name='bsd_floorid' alias='floorid' />
                                  <attribute name='bsd_name' alias='floor_name' />
                                  <filter type='and'>
                                    <condition attribute='bsd_project' operator='eq' uitype='bsd_project' value='" + this.ProjectId + @"' />
                                  </filter>
                                </link-entity>
                                <link-entity name='opportunity' from='bsd_units' to='productid' link-type='inner' alias='ag' >
	                                <attribute name='opportunityid' alias='queseid'/>
                                    <attribute name='statuscode' alias='queses_statuscode'/>
                                </link-entity>
                              </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Unit>>("products", fetchXml);

            if (result == null || result.value.Any() == false) return;

            var unitInBlock = result.value.GroupBy(x=>new {
                unitid= x.productid,
                statuscode= x.statuscode
            }).Select(y=>y.First()).ToList();
            foreach (var item in unitInBlock)
            {
                switch (item.statuscode)
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

            List<Unit> unitsGroupByFloor = result.value.GroupBy(i => new
            {
                floorId = i.floorid,
            }).Select(x=>x.First()).ToList();

            foreach (var item in unitsGroupByFloor)
            {
                Floor floor = new Floor();
                floor.bsd_floorid = item.floorid;
                floor.bsd_name = item.floor_name;

                var units = result.value.Where(x => x.floorid == item.floorid);
                var unitGroupBy = units.GroupBy(x => new
                {
                    unitid = x.productid,
                    statuscode = x.statuscode
                }).Select(y => y.First()).ToList();
                foreach (var unit in unitGroupBy)
                {
                    switch (unit.statuscode)
                    {
                        case 1:
                            floor.NumChuanBiInFloor++;
                            break;
                        case 100000000:
                            floor.NumSanSangInFloor++ ;
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
                    if (unit.queses_statuscode == "100000000" || unit.queses_statuscode == "100000002" || unit.queses_statuscode == "100000004")
                    {
                        unit.NumQueses++;
                    }
                    else
                    {
                        unit.NumQueses = 0;
                    }

                    unit.item_background = StatusCodeUnit.GetStatusCodeById(unit.statuscode.ToString()).Background;

                    floor.Units.Add(unit);
                }

                Floors.Add(floor);
            }

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
    }
}
