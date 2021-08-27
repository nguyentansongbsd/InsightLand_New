using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class DirectSaleDetailViewModel : BaseViewModel
    {
        public string Keyword { get; set; }
        public string ProjectId { get; set; }
        public string PhasesLanchId { get; set; }
        public bool IsEvent { get; set; }
        public string UnitCode { get; set; }
        public List<string> Directions { get; set; }
        //public List<string> Views { get; set; }
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
        public List<Block> Blocks { get => _blocks; set { _blocks = value; OnPropertyChanged(nameof(Blocks)); } }

        private List<Root> _apiResponse;
        public List<Root> ApiResponse { get => _apiResponse; set { _apiResponse = value; OnPropertyChanged(nameof(ApiResponse)); } }

        public ObservableCollection<Floor> Floors { get; set; } = new ObservableCollection<Floor>();

        private ObservableCollection<QueueListModel> _queueList;
        public ObservableCollection<QueueListModel> QueueList { get => _queueList; set { _queueList = value; OnPropertyChanged(nameof(QueueList)); } }

        public string fetchXml { get; set; }

        private int _numChuanBiInBlock;
        public int NumChuanBiInBlock { get => _numChuanBiInBlock; set { _numChuanBiInBlock = value; OnPropertyChanged(nameof(NumChuanBiInBlock)); } }

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
        public Unit Unit { get => _unit; set { _unit = value; OnPropertyChanged(nameof(Unit)); } }

        private StatusCodeModel _unitStatusCode;
        public StatusCodeModel UnitStatusCode { get => _unitStatusCode; set { _unitStatusCode = value; OnPropertyChanged(nameof(UnitStatusCode)); } }

        private OptionSet _unitDirection;
        public OptionSet UnitDirection { get => _unitDirection; set { _unitDirection = value; OnPropertyChanged(nameof(UnitDirection)); } }

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
            this.UnitStatuses = model.UnitStatuses;
            this.minNetArea = model.minNetArea;
            this.maxNetArea = model.maxNetArea;
            this.minPrice = model.minPrice;
            this.maxPrice = model.maxPrice;
            //this.Blocks = model.Blocks;
            QueueList = new ObservableCollection<QueueListModel>();
        }

        public async Task LoadTotalDirectSale()
        {
            try
            {
                var content = new
                {
                    ProjectId = this.ProjectId,
                    PhasesLanchId = this.PhasesLanchId,
                    IsEvent = this.IsEvent,
                    UnitCode = this.UnitCode,
                    Directions = (this.Directions != null && this.Directions.Count != 0) ? string.Join(",", this.Directions) : null,
                    UnitStatuses = (this.UnitStatuses != null && this.UnitStatuses.Count != 0) ? string.Join(",", this.UnitStatuses) : null,
                    minNetArea = this.minNetArea,
                    maxNetArea = this.maxNetArea,
                    //minPrice = this.minPrice,
                    //maxPrice = this.maxPrice
                };
                string json = JsonConvert.SerializeObject(content);
                var client = BsdHttpClient.Instance();
                string Token = App.Current.Properties["Token"] as string;
                var request = new HttpRequestMessage(HttpMethod.Get, $"{OrgConfig.ApiUrl}/bsd_GetTotalQtyDirectSale(input='{json}')");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    string a = body.Replace(@"\", "");
                    string a1 = a.Replace(@"https://conasicrm.api.crm5.dynamics.com/api/data/v9.1/$metadata#Microsoft.Dynamics.CRM.bsd_GetTotalQtyDirectSaleResponse", "").Replace("@odata.context", "").Replace("output", "").Remove(0, 11);
                    string a2 = a1.Substring(0, a1.Length - 2);
                    ApiResponse = JsonConvert.DeserializeObject<List<Root>>(a2);
                    if (ApiResponse.Count == 0) return;

                    List<Block> blocks = new List<Block>();
                    foreach (var item in ApiResponse)
                    {
                        Block block = new Block();
                        block.bsd_blockid = Guid.Parse(item.ID);
                        block.bsd_name = item.name;
                        blocks.Add(block);
                    }
                    Blocks = blocks;

                    var result = ApiResponse.FirstOrDefault();
                    blockId = Guid.Parse(result.ID);
                    var arrStatus = result.stringQty.Split(',');
                    NumChuanBiInBlock = int.Parse(arrStatus[0]);
                    NumSanSangInBlock = int.Parse(arrStatus[1]);
                    NumGiuChoInBlock = int.Parse(arrStatus[2]);
                    NumDatCocInBlock = int.Parse(arrStatus[3]);
                    NumDongYChuyenCoInBlock = int.Parse(arrStatus[4]);
                    NumDaDuTienCocInBlock = int.Parse(arrStatus[5]);
                    NumThanhToanDot1InBlock = int.Parse(arrStatus[6]);
                    NumDaBanInBlock = int.Parse(arrStatus[7]);

                    foreach (var item in result.listFloor)
                    {
                        Floor floor = new Floor();
                        floor.bsd_floorid = Guid.Parse(item.ID);
                        floor.bsd_name = item.name;
                        var arrStatusInFloor = item.stringQty.Split(',');
                        floor.NumChuanBiInFloor = int.Parse(arrStatusInFloor[0]);
                        floor.NumSanSangInFloor = int.Parse(arrStatusInFloor[1]);
                        floor.NumGiuChoInFloor = int.Parse(arrStatusInFloor[2]);
                        floor.NumDatCocInFloor = int.Parse(arrStatusInFloor[3]);
                        floor.NumDongYChuyenCoInFloor = int.Parse(arrStatusInFloor[4]);
                        floor.NumDaDuTienCocInFloor = int.Parse(arrStatusInFloor[5]);
                        floor.NumThanhToanDot1InFloor = int.Parse(arrStatusInFloor[6]);
                        floor.NumDaBanInFloor = int.Parse(arrStatusInFloor[7]);
                        Floors.Add(floor);
                    };

                }
                else
                {
                    var a = response.RequestMessage;
                }
            }
            catch (Exception ex)
            {
                string mess = ex.Message;
            }
        }

        public async Task LoadUnitByFloor(Guid floorId)
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

            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='product'>
                                <attribute name='productid' />
                                <attribute name='name' />
                                <attribute name='statuscode' />
                                <attribute name='price' />
                                <order attribute='name' descending='false' />
                                <filter type='and'>
                                    <condition attribute='statuscode' operator='ne' value='0' />
                                    <condition attribute='bsd_projectcode' operator='eq' uitype='bsd_project' value='{this.ProjectId}'/>
                                    <condition attribute='bsd_floor' operator='eq' uitype='bsd_floor' value='{floorId}' />
                                    '{PhasesLaunch_Condition}'
                                    '{UnitCode_Condition}'
                                    '{IsEvent_Condition}'
                                    '{StatusReason_Condition}'
                                    '{UnitStatus_Condition}'
                                    '{Direction_Condition}'
                                    '{minNetArea_Condition}'
                                    '{maxNetArea_Condition}'
                                    '{minPrice_Condition}'
                                    '{maxPrice_Condition}'
                                </filter>
                                <link-entity name='opportunity' from='bsd_units' to='productid' link-type='outer' alias='ag' >
                                    <attribute name='statuscode' alias='queses_statuscode'/>
                                </link-entity>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Unit>>("products", fetchXml);
            if (result == null || result.value.Any() == false) return;

            List<Unit> unitsResult = result.value.GroupBy(x => new
            {
                productid = x.productid
            }).Select(y => y.First()).ToList();

            List<Unit> units = new List<Unit>();
            foreach (var item in unitsResult)
            {
                // dem unit co nhung trang thai giu cho la: queuing, waiting,completed
                item.NumQueses = result.value.Where(x => x.productid == item.productid && (x.queses_statuscode == "100000000" || x.queses_statuscode == "100000002" || x.queses_statuscode == "100000004")).ToList().Count();
                units.Add(item);
            }

            Floors.SingleOrDefault(x => x.bsd_floorid == floorId).Units.AddRange(units);
        }

        public async Task LoadUnitById(Guid unitId)
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='product'>
                                    <attribute name='name' />
                                    <attribute name='statuscode' />
                                    <attribute name='price' />
                                    <attribute name='productid' />
                                    <attribute name='bsd_view' />
                                    <attribute name='bsd_direction' />
                                    <attribute name='bsd_constructionarea' />
                                    <order attribute='bsd_constructionarea' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='productid' operator='eq' uitype='product' value='{unitId}' />
                                    </filter>
                                    <link-entity name='bsd_unittype' from='bsd_unittypeid' to='bsd_unittype' visible='false' link-type='outer' alias='a_493690ec6ce2e811a94e000d3a1bc2d1'>
                                      <attribute name='bsd_name'  alias='bsd_unittype_name'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Unit>>("products", fetchXml);
            if (result == null || result.value.Any() == false) return;

            Unit = result.value.FirstOrDefault();
        }

        public async Task LoadQueues(Guid unitId)
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
                            <condition attribute='productid' operator='eq' value='{unitId}'/>
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

        public async Task CheckShowBtnBangTinhGia(Guid unitId)
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                              <entity name='bsd_phaseslaunch'>
                                <attribute name='bsd_phaseslaunchid' />
                                <order attribute='createdon' descending='true' />
                                <link-entity name='product' from='bsd_phaseslaunchid' to='bsd_phaseslaunchid' link-type='inner' alias='al'>
                                  <filter type='and'>
                                    <condition attribute='productid' operator='eq' value='{unitId}' />
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
    public class ListFloor
    {
        [JsonProperty("ID")]
        public string ID { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("sumQty")]
        public string sumQty { get; set; }
        [JsonProperty("stringQty")]
        public string stringQty { get; set; }
    }

    public class Root
    {
        [JsonProperty("ID")]
        public string ID { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("sumQty")]
        public string sumQty { get; set; }
        [JsonProperty("stringQty")]
        public string stringQty { get; set; }
        [JsonProperty("listFloor")]
        public List<ListFloor> listFloor { get; set; }
    }
}
