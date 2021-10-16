using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
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

namespace ConasiCRM.Portable.ViewModels
{
    public class UnitInfoViewModel : BaseViewModel
    {
        private ObservableCollection<CollectionData> _collections;
        public ObservableCollection<CollectionData> Collections { get => _collections; set { _collections = value; OnPropertyChanged(nameof(Collections)); } }

        public List<Photo> Photos;
        public List<Photo> Medias;
        public PhotoBrowser photoBrowser;

        private int _totalMedia;
        public int TotalMedia { get => _totalMedia; set { _totalMedia = value; OnPropertyChanged(nameof(TotalMedia)); } }

        private int _totalPhoto;
        public int TotalPhoto { get => _totalPhoto; set { _totalPhoto = value; OnPropertyChanged(nameof(TotalPhoto)); } }

        private bool _showCollections;
        public bool ShowCollections { get => _showCollections; set { _showCollections = value; OnPropertyChanged(nameof(ShowCollections)); } }
        public Guid UnitId { get; set; }

        public ObservableCollection<QueueFormModel> _list_danhsachdatcho;
        public ObservableCollection<QueueFormModel> list_danhsachdatcho { get => _list_danhsachdatcho; set { _list_danhsachdatcho = value; OnPropertyChanged(nameof(list_danhsachdatcho)); } }
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

        private bool _isShowBtnBangTinhGia;
        public bool IsShowBtnBangTinhGia { get => _isShowBtnBangTinhGia; set { _isShowBtnBangTinhGia = value; OnPropertyChanged(nameof(IsShowBtnBangTinhGia)); } }

        public int PageDanhSachHopDong { get; set; } = 1;
        public int PageDanhSachDatCoc { get; set; } = 1;
        public int PageDanhSachDatCho { get; set; } = 1;

        public bool IsLoaded { get; set; } = false;

        public bool IsVip { get; set; } = false;

        public UnitInfoViewModel()
        {
            list_danhsachdatcho = new ObservableCollection<QueueFormModel>();
            Photos = new List<Photo>();
            Medias = new List<Photo>();
            this.Collections = new ObservableCollection<CollectionData>();
            photoBrowser = new PhotoBrowser
            {
                Photos = Photos,
            };
        }

        public async Task LoadUnit()
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
            if (result == null || result.value.Count == 0) return;
            UnitInfo = result.value.FirstOrDefault();
            await LoadAllCollection();
        }

        public async Task LoadQueues()
        {
            string fetch = $@"<fetch version='1.0' count='5' page='{PageDanhSachDatCho}' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='opportunity'>
                        <attribute name='name' />
                        <attribute name='customerid' />
                        <attribute name='createdon' />
                        <attribute name='bsd_queuingexpired' />
                        <attribute name='opportunityid' />
                        <order attribute='createdon' descending='true' />
                        <filter type='and'>
                          <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='{UserLogged.Id}' />
                          <condition attribute='bsd_units' operator='eq' value='{UnitInfo.productid}' />
                        </filter>
                        <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer'>
                           <attribute name='fullname' alias='contact_name'/>
                        </link-entity>
                        <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer'>
                           <attribute name='name'  alias='account_name'/>
                        </link-entity>
                        <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='a_805e44d019dbeb11bacb002248168cad'>
                          <attribute name='bsd_name' alias='bsd_project_name'/>
                        </link-entity>
                      </entity>
                    </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueueFormModel>>("opportunities", fetch);
            if (result == null || result.value.Count ==0) return;

            IsLoaded = true;
            var data = result.value;
            ShowMoreDanhSachDatCho = data.Count < 5 ? false : true;

            foreach (var item in data)
            {
                item.customer_name = !string.IsNullOrWhiteSpace(item.contact_name) ? item.contact_name : item.account_name;
                list_danhsachdatcho.Add(item);
            }
        }

        public async Task LoadReservation()
        {
            string fetch = $@"<fetch version='1.0' count='5' page='{PageDanhSachDatCoc}' output-format='xml-platform' mapping='logical' distinct='false'>
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
                            <filter type='and'>
                              <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}'/>
                              <condition attribute='bsd_unitno' operator='eq' value='{UnitInfo.productid}'/>
                            </filter>
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
            ShowMoreDanhSachDatCoc = data.Count < 5 ? false : true;

            foreach (var x in data)
            {
                list_danhsachdatcoc.Add(x);
            }
        }

        public async Task LoadOptoinEntry()
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
                            <filter type='and'>
                              <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}'/>
                              <condition attribute='bsd_unitnumber' operator='eq' value='{UnitInfo.productid}'/>
                            </filter>
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
            ShowMoreDanhSachHopDong = data.Count < 5 ? false : true;

            foreach (var x in data)
            {
                list_danhsachhopdong.Add(x);
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
                                    <condition attribute='productid' operator='eq' value='{UnitId}' />
                                  </filter>
                                </link-entity><link-entity name='bsd_event' from='bsd_phaselaunch' to='bsd_phaseslaunchid' link-type='inner' alias='an' >
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

        public async Task LoadAllCollection()
        {
            if (UnitId != null && UnitInfo != null && !string.IsNullOrWhiteSpace(UnitInfo.name))
            {
                var Folder = UnitInfo.name.Replace('.', '-') + "_" + UnitId.ToString().Replace("-", string.Empty).ToUpper();
                var Category = "Units";
                var category_value = "product";

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
                            var soucre = OrgConfig.SharePointResource + "/sites/" + OrgConfig.SharePointSiteName + "/_layouts/15/download.aspx?SourceUrl=/sites/" + OrgConfig.SharePointSiteName + "/" + category_value + "/" + Folder + "/" + item.Name + "&access_token=" + getTokenResponse.access_token;
                            Medias.Add(new Photo { URL = soucre });
                            Collections.Add(new CollectionData { MediaSource = soucre, ImageSource = null, Index = TotalMedia });
                            TotalMedia++;
                        }
                        else if (item.Name.ToLower().Split('.')[1] == "jpg" || item.Name.ToLower().Split('.')[1] == "jpeg" || item.Name.ToLower().Split('.')[1] == "png")
                        {
                            var soucre = OrgConfig.SharePointResource + "/sites/" + OrgConfig.SharePointSiteName + "/_layouts/15/download.aspx?SourceUrl=/sites/" + OrgConfig.SharePointSiteName + "/" + category_value + "/" + Folder + "/" + item.Name + "&access_token=" + getTokenResponse.access_token;
                            Photos.Add(new Photo { URL = soucre });
                            Collections.Add(new CollectionData { MediaSource = null, ImageSource = soucre, Index = TotalPhoto });
                            TotalPhoto++;
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
