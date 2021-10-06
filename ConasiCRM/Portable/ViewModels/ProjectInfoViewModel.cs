using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using Stormlion.PhotoBrowser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class ProjectInfoViewModel : BaseViewModel
    {
        //public ObservableCollection<CollectionData> Data { get; set; }
        //public List<Photo> Photos;
        //public List<Photo> Media;
        //public PhotoBrowser photoBrowser;

        //private bool _onComplate;
        //public bool OnComplate { get => _onComplate; set { _onComplate = value; OnPropertyChanged(nameof(OnComplate)); } }

        //private ObservableCollection<CollectionData> GetCollectionData()
        //{
        //    var list = new List<CollectionData>
        //    {
        //        new CollectionData { MediaSource = "https://firebasestorage.googleapis.com/v0/b/gglogin-c3e8a.appspot.com/o/videoduan.mp4?alt=media&token=f1b9e0e7-0603-45a8-9261-2de7d6dcf545",ImageSource= null,Index = 1},
        //        new CollectionData { MediaSource = null,ImageSource="https://firebasestorage.googleapis.com/v0/b/gglogin-c3e8a.appspot.com/o/duan1.jpg?alt=media&token=78a2fc63-2009-4b25-b189-2613159a51bd",Index = 1},
        //        new CollectionData { MediaSource = null,ImageSource="duan2.jpg",Index = 2},
        //        new CollectionData { MediaSource = null,ImageSource="duan3.jpg",Index = 3},
        //        new CollectionData { MediaSource = null,ImageSource="duan4.jpg",Index = 4},
        //        new CollectionData { MediaSource = null,ImageSource="duan5.jpg",Index = 5},
        //        new CollectionData { MediaSource = null,ImageSource="duan6.jpg",Index = 6},
        //    };
        //    var data = new ObservableCollection<CollectionData>();
        //    Photos = new List<Photo>();
        //    Media = new List<Photo>();

        //    foreach (var item in list)
        //    {
        //        if (item.ImageSource != null)
        //        {
        //            Photos.Add(new Photo { URL = item.ImageSource });
        //            data.Add(item);
        //        }
        //        else
        //        {
        //            Media.Add(new Photo { URL = item.ImageSource });
        //            data.Add(item);
        //        }
        //    }
        //    return data;
        //}



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

            //this.Data = new ObservableCollection<CollectionData>();
            //this.Data = GetCollectionData();
            //OnComplate = true;
            //photoBrowser = new PhotoBrowser
            //{
            //    Photos = Photos,
            //};
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
    }
}
