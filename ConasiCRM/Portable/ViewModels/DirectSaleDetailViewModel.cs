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
    public class DirectSaleDetailViewModel : ListViewBaseViewModel2<Unit>
    {
        public string Keyword { get; set; }
        public Guid ProjectId { get; set; }
        public Guid PhasesLanchId { get; set; }
        public bool IsEvent { get; set; }
        public string UnitCode { get; set; }
        public ObservableCollection<string> Directions { get; set; }
        public ObservableCollection<string> Views { get; set; }
        public ObservableCollection<string> UnitStatuses { get; set; }
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

        private List<OptionSet> _blocks;
        public List<OptionSet> Blocks { get=> _blocks; set { _blocks = value;OnPropertyChanged(nameof(Blocks)); } }

        private OptionSet _floor;
        public OptionSet Floor { get => _floor; set { _floor = value; OnPropertyChanged(nameof(Floor)); } }

        private List<OptionSet> _floors;
        public List<OptionSet> Floors { get => _floors; set { _floors = value; OnPropertyChanged(nameof(Floors)); } }
        public ObservableCollection<QueueListModel_DirectSale> QueueList { get; set; }

        public string fetchXml { get; set; }

        public DirectSaleDetailViewModel(DirectSaleSearchModel model)
        {
            IsBusy = true;
            this.ProjectId = model.ProjectId;
            this.PhasesLanchId = model.PhasesLanchId;
            this.IsEvent = model.IsEvent;
            this.UnitCode = model.UnitCode;
            this.Directions = model.Directions;
            this.Views = model.Views;
            this.UnitStatuses = model.UnitStatuses;
            this.minNetArea = model.minNetArea;
            this.maxNetArea = model.maxNetArea;
            this.minPrice = model.minPrice;
            this.maxPrice = model.maxPrice;
            QueueList = new ObservableCollection<QueueListModel_DirectSale>();
            ResetXml();

            PreLoadData = new Command(() =>
            {
                FetchXml = string.Format(fetchXml, Page);
                EntityName = "products";
            });

        }

        public void ResetXml()
        {
            string Keyword_Conditon = string.IsNullOrWhiteSpace(Keyword) ? "" : "<condition attribute='name' operator='like' value='%" + Keyword + @"%' />";
            string StatusReason_Condition = StatusReason == null ? "" : "<condition attribute='statuscode' operator='eq' value='" + StatusReason.Val + @"' />";
            string PhasesLaunch_Condition = (PhasesLanchId != Guid.Empty)
                ? @"<condition attribute='bsd_phaseslaunchid' operator='eq' uitype='bsd_phaseslaunch' value='" + this.PhasesLanchId + @"' />"
                : "";

            string IsEvent_Condition = IsEvent ? @"<condition entityname='af' attribute='bsd_eventid' operator='not-null'/>" : "";

            string UnitCode_Condition = !string.IsNullOrEmpty(UnitCode) ? "<condition attribute='name' operator='like' value='%" + UnitCode + "%' />" : "";

            string Direction_Condition = string.Empty;
            if (Directions.Count != 0)
            {
                string tmp = string.Empty;
                foreach (var i in Directions)
                {
                    tmp += "<value>" + i + "</value>";
                }
                Direction_Condition = @"<condition attribute='bsd_direction' operator='in'>" + tmp + "</condition>";
            }

            string View_Condition = string.Empty;
            if (Views.Count != 0)
            {
                string tmp = string.Empty;
                foreach (var i in Views)
                {
                    tmp += "<value>" + i + "</value>";
                }
                View_Condition = @"<condition attribute='bsd_view' operator='in'>" + tmp + "</condition>";
            }


            string UnitStatus_Condition = string.Empty;
            if (UnitStatuses.Count != 0)
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

            string Block_Condition = Block != null ? $"<condition attribute='bsd_blockid' operator='eq' value='{Block.Val}'/>" : "";
            string Floor_Condition = Floor != null ? $"<condition attribute='bsd_floorid' operator='eq' value='{Floor.Val}'/>" : "";

            fetchXml = @"<fetch version='1.0' count='15' page='{0}' output-format='xml-platform' mapping='logical' distinct='false'>
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
                                    " + Keyword_Conditon + @"
                                    " + StatusReason_Condition + @"
                                    " + PhasesLaunch_Condition + @"
                                    " + IsEvent_Condition + @"
                                    " + UnitCode_Condition + @"
                                    " + Direction_Condition + @"
                                    " + View_Condition + @"
                                    " + UnitStatus_Condition + @"
                                    " + minNetArea_Condition + @"
                                    " + maxNetArea_Condition + @"
                                    " + minPrice_Condition + @"
                                    " + maxPrice_Condition + @"
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
                                    "+ Floor_Condition + @"
                                  </filter>
                                </link-entity>
                                <link-entity name='bsd_block' from='bsd_blockid' to='bsd_blocknumber' link-type='inner' alias='ab'>
                                  <attribute name='bsd_blockid' alias='blockid' />
                                  <attribute name='bsd_name' alias='block_name' />
                                  <filter type='and'>
                                    <condition attribute='bsd_project' operator='eq' uitype='bsd_project' value='" + this.ProjectId + @"' />
                                    "+ Block_Condition + @"
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";
        }

        public async Task LoadStatusReason()
        {
            IsBusy = true;
            this.StatusReasons = new List<OptionSet>()
            {
                new OptionSet("-1","Tất cả"),
                new OptionSet("1","Preparing"),
                new OptionSet("100000000","Available"),
                new OptionSet("100000007","Booking"),
                new OptionSet("100000004","Queuing"),
                new OptionSet("100000006","Reserve"),
                new OptionSet("100000005","Collected"),
                new OptionSet("100000003","Deposited"),
                new OptionSet("100000001","1st Installment"),
                new OptionSet("100000009","Singed D.A"),
                new OptionSet("100000008","Qualified"),
                new OptionSet("100000002","Sold"),
            };
            IsBusy = false;
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
            if (block_result == null || block_result.value.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("", "Lỗi. Vui lòng thử lại", "Đóng");
                return;
            }
            else
            {
                this.Blocks = new List<OptionSet>();
                this.Blocks.Add(new OptionSet() { Val = "-1", Label = "Tất cả" });
                var data = block_result.value;
                foreach (var item in data)
                {
                    this.Blocks.Add(new OptionSet(item.bsd_blockid.ToString(), item.bsd_name));
                }
            }
        }

        public async Task LoadFloors()
        {
            string filter_byblock = Block != null ? $@"<link-entity name='bsd_block' from='bsd_blockid' to='bsd_block' link-type='inner' alias='a_69e6a386df72e911a83a000d3a80e651'>
                                  <filter type='and'>
                                    <condition attribute='bsd_blockid' operator='eq' value='{Block.Val}'/>
                                  </filter>
                                </link-entity>" : "";

            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='bsd_floor'>
                        <attribute name='bsd_name' />
                        <attribute name='bsd_floorid' />
                        <order attribute='createdon' descending='false' />
                        <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='inner' alias='ab'>
                          <filter type='and'>
                            <condition attribute='bsd_projectid' operator='eq' value='{this.ProjectId}'/>
                          </filter>
                        </link-entity>
                        {filter_byblock}
                      </entity>
                    </fetch>";
            var floor_result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Floor>>("bsd_floors", fetchXml);
            if (floor_result == null || floor_result.value.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("", "Lỗi. Vui lòng thử lại", "Đóng");
                return;
            }
            else
            {
                this.Floors = new List<OptionSet>();
                this.Floors.Add(new OptionSet() { Val = "-1", Label = "Tất cả" });
                var data = floor_result.value;
                foreach (var item in data)
                {
                    this.Floors.Add(new OptionSet(item.bsd_floorid.ToString(), item.bsd_name));
                }
            }
        }
    }
}
