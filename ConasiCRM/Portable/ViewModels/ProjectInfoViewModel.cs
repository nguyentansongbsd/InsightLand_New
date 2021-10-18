using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using FormsVideoLibrary;
using Foundation;
using Newtonsoft.Json;
using Stormlion.PhotoBrowser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class ProjectInfoViewModel : BaseViewModel
    {
        private ObservableCollection<CollectionData> _collections;
        public ObservableCollection<CollectionData> Collections { get => _collections; set { _collections = value; OnPropertyChanged(nameof(Collections)); } }

        public List<Photo> Photos;
        public List<Photo> Medias;
        public PhotoBrowser photoBrowser;

        private bool _showCollections;
        public bool ShowCollections { get => _showCollections; set { _showCollections = value; OnPropertyChanged(nameof(ShowCollections)); } }

        private int _totalMedia;
        public int TotalMedia { get => _totalMedia; set { _totalMedia = value; OnPropertyChanged(nameof(TotalMedia)); } }

        private int _totalPhoto;
        public int TotalPhoto { get => _totalPhoto; set { _totalPhoto = value; OnPropertyChanged(nameof(TotalPhoto)); } }
        public Guid ProjectId { get; set; }
        public List<ChartModel> unitChartModels { get; set; }
        public ObservableCollection<ChartModel> UnitChart { get; set; } = new ObservableCollection<ChartModel>();

        private ObservableCollection<QueueFormModel> _listGiuCho;
        public ObservableCollection<QueueFormModel> ListGiuCho { get => _listGiuCho; set { _listGiuCho = value; OnPropertyChanged(nameof(ListGiuCho)); } }

        private ProjectInfoModel _project;
        public ProjectInfoModel Project
        {
            get => _project;
            set
            {
                if (_project != value)
                { _project = value; OnPropertyChanged(nameof(Project)); }
            }
        }

        private OptionSet _projectType;
        public OptionSet ProjectType { get => _projectType; set { _projectType = value; OnPropertyChanged(nameof(ProjectType)); } }

        private OptionSet _propertyUsageType;
        public OptionSet PropertyUsageType { get => _propertyUsageType; set { _propertyUsageType = value; OnPropertyChanged(nameof(PropertyUsageType)); } }

        private OptionSet _handoverCoditionMinimum;
        public OptionSet HandoverCoditionMinimum { get => _handoverCoditionMinimum; set { _handoverCoditionMinimum = value; OnPropertyChanged(nameof(HandoverCoditionMinimum)); } }

        private bool _isShowBtnGiuCho;
        public bool IsShowBtnGiuCho { get => _isShowBtnGiuCho; set { _isShowBtnGiuCho = value; OnPropertyChanged(nameof(IsShowBtnGiuCho)); } }

        private int _numUnit = 0;
        public int NumUnit { get => _numUnit; set { _numUnit = value; OnPropertyChanged(nameof(NumUnit)); } }

        private int _soGiuCho = 0;
        public int SoGiuCho { get => _soGiuCho; set { _soGiuCho = value; OnPropertyChanged(nameof(SoGiuCho)); } }

        private int _soDatCoc = 0;
        public int SoDatCoc { get => _soDatCoc; set { _soDatCoc = value; OnPropertyChanged(nameof(SoDatCoc)); } }

        private int _soHopDong = 0;
        public int SoHopDong { get => _soHopDong; set { _soHopDong = value; OnPropertyChanged(nameof(SoHopDong)); } }

        private int _soBangTinhGia = 0;
        public int SoBangTinhGia { get => _soBangTinhGia; set { _soBangTinhGia = value; OnPropertyChanged(nameof(SoBangTinhGia)); } }

        private bool _showMoreBtnGiuCho;
        public bool ShowMoreBtnGiuCho { get => _showMoreBtnGiuCho; set { _showMoreBtnGiuCho = value; OnPropertyChanged(nameof(ShowMoreBtnGiuCho)); } }

        public int ChuanBi { get; set; } = 0;
        public int SanSang { get; set; } = 0;
        public int GiuCho { get; set; } = 0;
        public int DatCoc { get; set; } = 0;
        public int DongYChuyenCoc { get; set; } = 0;
        public int DaDuTienCoc { get; set; } = 0;
        public int ThanhToanDot1 { get; set; } = 0;
        public int DaBan { get; set; } = 0;

        public bool IsHasEvent { get; set; }
        public bool IsLoadedGiuCho { get; set; }

        public int PageListGiuCho = 1;

        public ProjectInfoViewModel()
        {
            ListGiuCho = new ObservableCollection<QueueFormModel>();
            Photos = new List<Photo>();
            Medias = new List<Photo>();
            this.Collections = new ObservableCollection<CollectionData>();
            photoBrowser = new PhotoBrowser
            {
                Photos = Photos,
            };
        }

        public async Task LoadData()
        {
            string FetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_project'>
                                <attribute name='bsd_projectid' />
                                <attribute name='bsd_projectcode' />
                                <attribute name='bsd_name' />
                                <attribute name='createdon' />
                                <attribute name='bsd_address' />
                                <attribute name='bsd_projecttype' />
                                <attribute name='bsd_propertyusagetype' />
                                <attribute name='bsd_depositpercentda' />
                                <attribute name='bsd_estimatehandoverdate' />
                                <attribute name='bsd_landvalueofproject' />
                                <attribute name='bsd_maintenancefeespercent' />
                                <attribute name='bsd_numberofmonthspaidmf' />
                                <attribute name='bsd_managementamount' />
                                <attribute name='bsd_bookingfee' />
                                <attribute name='bsd_depositamount' />
                                <order attribute='bsd_name' descending='false' />
                                <filter type='and'>
                                  <condition attribute='bsd_projectid' operator='eq' uitype='bsd_project' value='" + ProjectId.ToString() + @"' />
                                </filter>
                                <link-entity name='account' from='accountid' to='bsd_investor' visible='false' link-type='outer' alias='a_8924f6d5b214e911a97f000d3aa04914'>
                                  <attribute name='bsd_name' alias='bsd_investor_name' />
                                  <attribute name='accountid' alias='bsd_investor_id' />
                                </link-entity>
                              </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ProjectInfoModel>>("bsd_projects", FetchXml);
            if (result == null || result.value.Any() == false) return;
            Project = result.value.FirstOrDefault();
            await LoadAllCollection();
        }

        public async Task CheckEvent()
        {
            // ham check su kien hide/show cua du an (show khi du an dang trong thoi gian dien ra su kien, va trang thai la "Approved")
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_event'>
                                <attribute name='createdon' />
                                <attribute name='statuscode' />
                                <attribute name='bsd_startdate' />
                                <attribute name='bsd_enddate' />
                                <attribute name='bsd_eventid' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                  <condition attribute='statuscode' operator='eq' value='100000000' />
                                </filter>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='inner' alias='aa'>
                                  <filter type='and'>
                                    <condition attribute='bsd_projectid' operator='eq' value='{ProjectId}'/>
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<EventFormModel>>("bsd_events", fetchXml);
            if (result == null || result.value.Any() == false) return;

            var data = result.value;
            foreach (var item in data)
            {
                if (item.bsd_startdate < DateTime.Now && item.bsd_enddate > DateTime.Now)
                {
                    IsHasEvent = true;
                    return;
                }
            }
        }

        public async Task LoadThongKe()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='product'>
                                <attribute name='name' />
                                <attribute name='productnumber' />
                                <attribute name='statecode' />
                                <attribute name='productstructure' />
                                <attribute name='statuscode' />
                                <attribute name='bsd_projectcode' />
                                <attribute name='createdon' />
                                <attribute name='bsd_unitscodesams' />
                                <attribute name='productid' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                    <condition attribute='bsd_projectcode' operator='eq' uitype='bsd_project' value='" + this.ProjectId + @"'/>
                                  </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Unit>>("products", fetchXml);
            if (result == null || result.value.Any() == false)
            {
                ChuanBi++;
                SanSang++;
                GiuCho++;
                DatCoc++;
                DongYChuyenCoc++;
                DaDuTienCoc++;
                ThanhToanDot1++;
                DaBan++;
                IsShowBtnGiuCho = true;
            }
            else
            {
                IsShowBtnGiuCho = false;
                var data = result.value;
                NumUnit = data.Count;
                foreach (var item in data)
                {
                    switch (item.statuscode)
                    {
                        case 1:
                            ChuanBi++;
                            break;
                        case 100000000:
                            SanSang++;
                            break;
                        case 100000004:
                            GiuCho++;
                            break;
                        case 100000006:
                            DatCoc++;
                            SoDatCoc++;
                            break;
                        case 100000005:
                            DongYChuyenCoc++;
                            break;
                        case 100000003:
                            DaDuTienCoc++;
                            break;
                        case 100000001:
                            ThanhToanDot1++;
                            break;
                        case 100000002:
                            DaBan++;
                            break;
                        default:
                            break;

                    }
                }
            }

            unitChartModels = new List<ChartModel>()
            {
                    new ChartModel {Category ="Giữ chỗ",Value=GiuCho},
                    new ChartModel { Category = "Đặt cọc", Value = DatCoc },
                    new ChartModel {Category ="Đồng ý chuyển cọc",Value=DongYChuyenCoc },
                    new ChartModel { Category = "Đã đủ tiền cọc", Value = DaDuTienCoc },
                    new ChartModel {Category ="Thanh toán đợt 1",Value=ThanhToanDot1},
                    new ChartModel { Category = "Đã bán", Value =  DaBan},
                    new ChartModel {Category ="Chuẩn bị", Value=ChuanBi},
                    new ChartModel { Category = "Sẵn sàng", Value = SanSang }
            };
            foreach (var item in unitChartModels)
            {
                UnitChart.Add(item);
            }
        }

        public async Task LoadThongKeGiuCho()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='opportunity'>
                                    <attribute name='name' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_project' operator='eq' uitype='bsd_project' value='{" + ProjectId + @"}' />
                                      <condition attribute='statuscode' operator='in'>
                                        <value>100000000</value>
                                        <value>100000002</value>
                                      </condition>
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueueFormModel>>("opportunities", fetchXml);
            if (result == null || result.value.Any() == false) return;

            SoGiuCho = result.value.Count();
        }

        public async Task LoadThongKeHopDong()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='salesorder'>
                                <attribute name='name' />
                                <order attribute='createdon' descending='true' />
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='inner' alias='ad'>
                                  <filter type='and'>
                                    <condition attribute='bsd_projectid' operator='eq' value ='{ProjectId}'/>
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionEntry>>("salesorders", fetchXml);
            if (result == null || result.value.Any() == false) return;

            SoHopDong = result.value.Count();
        }

        public async Task LoadThongKeBangTinhGia()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='quote'>
                                <attribute name='name' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                  <condition attribute='statuscode' operator='ne' value='100000001' />
                                </filter>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' link-type='inner' alias='ae'>
                                  <filter type='and'>
                                    <condition attribute='bsd_projectid' operator='eq' value='{ProjectId}'/>
                                  </filter>
                                </link-entity>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QuoteModel>>("quotes", fetchXml);
            if (result == null || result.value.Any() == false) return;
            SoBangTinhGia = result.value.Count();
        }

        public async Task LoadGiuCho()
        {
            IsLoadedGiuCho = true;
            string fetchXml = $@"<fetch version='1.0' count='10' page='{PageListGiuCho}' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='opportunity'>
                                <attribute name='name' />
                                <attribute name='customerid' />
                                <attribute name='createdon' />
                                <attribute name='bsd_queuingexpired' />
                                <attribute name='opportunityid' />
                                <order attribute='createdon' descending='true' />
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='inner' alias='ab'>
                                    <attribute name='bsd_name' alias='bsd_project_name'/>
                                  <filter type='and'>
                                    <condition attribute='bsd_projectid' operator='eq' value='{ProjectId}'/>
                                  </filter>
                                </link-entity>
                                <link-entity name='bsd_employee' from='bsd_employeeid' to='bsd_employee' link-type='inner' alias='ac'>
                                  <filter type='and'>
                                    <condition attribute='bsd_employeeid' operator='eq' value='{UserLogged.Id}' />
                                  </filter>
                                </link-entity>
                                <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='a_434f5ec290d1eb11bacc000d3a80021e'>
                                  <attribute name='name' alias='account_name'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='a_884f5ec290d1eb11bacc000d3a80021e'>
                                  <attribute name='bsd_fullname' alias='contact_name'/>
                                </link-entity>
                              </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueueFormModel>>("opportunities", fetchXml);
            if (result == null || result.value.Any() == false) return;

            List<QueueFormModel> data = result.value;
            ShowMoreBtnGiuCho = data.Count < 10 ? false : true;
            foreach (var item in data)
            {
                QueueFormModel queue = new QueueFormModel();
                queue = item;
                if (!string.IsNullOrWhiteSpace(item.contact_name))
                {
                    queue.customer_name = item.contact_name;
                }
                else if (!string.IsNullOrWhiteSpace(item.account_name))
                {
                    queue.customer_name = item.account_name;
                }

                ListGiuCho.Add(queue);
            }
        }

        public async Task LoadAllCollection()
        {
            if (ProjectId != null && Project != null && !string.IsNullOrWhiteSpace(Project.bsd_name))
            {
                var Folder = Project.bsd_name.Replace('.', '-') + "_" + ProjectId.ToString().Replace("-", string.Empty).ToUpper();
                var Category = "Project";
                var category_value = "bsd_project";

                GetTokenResponse getTokenResponse = await CrmHelper.getSharePointToken();
                var client = BsdHttpClient.Instance();
                string fileListUrl = $"{OrgConfig.SharePointResource}/sites/" + OrgConfig.SharePointSiteName + "/_api/web/Lists/GetByTitle('" + Category + "')/RootFolder/Folders('" + Folder + "')/Files";
                var request = new HttpRequestMessage(HttpMethod.Get, fileListUrl);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", getTokenResponse.access_token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    TotalMedia = 0;
                    TotalPhoto = 0;
                    var body = await response.Content.ReadAsStringAsync();
                    SharePointFieldResult sharePointFieldResult = JsonConvert.DeserializeObject<SharePointFieldResult>(body);
                    var list = sharePointFieldResult.value;
                    foreach (var item in list)
                    {
                        if (item.Name.Split('.')[1] == "flv" || item.Name.Split('.')[1] == "mp4" || item.Name.Split('.')[1] == "m3u8" || item.Name.Split('.')[1] == "3gp" || item.Name.Split('.')[1] == "mov" || item.Name.Split('.')[1] == "avi" || item.Name.Split('.')[1] == "wmv")
                        {
                            TotalMedia++;
                            var soucre = OrgConfig.SharePointResource + "/sites/" + OrgConfig.SharePointSiteName + "/_layouts/15/download.aspx?SourceUrl=/sites/" + OrgConfig.SharePointSiteName + "/" + category_value + "/" + Folder + "/" + item.Name + "&access_token=" + getTokenResponse.access_token;
                            Medias.Add(new Photo { URL = soucre });
                            Collections.Add(new CollectionData { MediaSource = soucre, ImageSource = null, Index = TotalMedia });
                        }
                        else if (item.Name.ToLower().Split('.')[1] == "jpg" || item.Name.ToLower().Split('.')[1] == "jpeg" || item.Name.ToLower().Split('.')[1] == "png")
                        {
                            TotalPhoto++;
                            var soucre = OrgConfig.SharePointResource + "/sites/" + OrgConfig.SharePointSiteName + "/_layouts/15/download.aspx?SourceUrl=/sites/" + OrgConfig.SharePointSiteName + "/" + category_value + "/" + Folder + "/" + item.Name + "&access_token=" + getTokenResponse.access_token;
                            if (Device.RuntimePlatform == Device.iOS)
                            {
                                var jrURL = new NSUrl(new System.Uri(soucre).AbsoluteUri);
                                soucre = jrURL.AbsoluteString;
                            }
                            Photos.Add(new Photo { URL = soucre });
                            Collections.Add(new CollectionData { MediaSource = null, ImageSource = soucre, Index = TotalPhoto });
                        }
                    }
                }
                if (Collections != null && Collections.Count > 0)
                {
                    ShowCollections = true;
                }
                else
                {
                    ShowCollections = false;
                }
            }
        }
    }
}
