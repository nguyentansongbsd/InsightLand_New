using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Xamarin.Forms;
using ConasiCRM.Portable.IServices;
using Newtonsoft.Json;
using ConasiCRM.Portable.Controls;

namespace ConasiCRM.Portable.ViewModels
{
    public class ContactDetailPageViewModel : BaseViewModel
    {
        private ImageSource _myImage;
        public ImageSource MyImage { get => _myImage; set { _myImage = value; OnPropertyChanged(nameof(MyImage)); } }

        private ContactFormModel _singleContact;
        public ContactFormModel singleContact { get { return _singleContact; } set { _singleContact = value; OnPropertyChanged(nameof(singleContact)); } }

        private OptionSet _singleGender;
        public OptionSet singleGender { get => _singleGender; set { _singleGender = value; OnPropertyChanged(nameof(singleGender)); } }

        private OptionSet _singleLocalization;
        public OptionSet SingleLocalization { get => _singleLocalization; set { _singleLocalization = value; OnPropertyChanged(nameof(SingleLocalization)); } }
        private ObservableCollection<QueueFormModel> _list_danhsachdatcho;
        public ObservableCollection<QueueFormModel> list_danhsachdatcho { get => _list_danhsachdatcho; set { _list_danhsachdatcho = value; OnPropertyChanged(nameof(list_danhsachdatcho)); } }

        private bool _showMoreDanhSachDatCho;
        public bool ShowMoreDanhSachDatCho { get => _showMoreDanhSachDatCho; set { _showMoreDanhSachDatCho = value; OnPropertyChanged(nameof(ShowMoreDanhSachDatCho)); } }
        public int PageDanhSachDatCho { get; set; } = 1;

        private ObservableCollection<ReservationListModel> _list_danhsachdatcoc;
        public ObservableCollection<ReservationListModel> list_danhsachdatcoc { get => _list_danhsachdatcoc; set { _list_danhsachdatcoc = value; OnPropertyChanged(nameof(list_danhsachdatcoc)); } }
        private bool _showMoreDanhSachDatCoc;
        public bool ShowMoreDanhSachDatCoc { get => _showMoreDanhSachDatCoc; set { _showMoreDanhSachDatCoc = value; OnPropertyChanged(nameof(ShowMoreDanhSachDatCoc)); } }
        public int PageDanhSachDatCoc { get; set; } = 1;

        private ObservableCollection<ContractModel> _list_danhsachhopdong;
        public ObservableCollection<ContractModel> list_danhsachhopdong { get => _list_danhsachhopdong; set { _list_danhsachhopdong = value; OnPropertyChanged(nameof(list_danhsachhopdong)); } }
        private bool _showMoreDanhSachHopDong;
        public bool ShowMoreDanhSachHopDong { get => _showMoreDanhSachHopDong; set { _showMoreDanhSachHopDong = value; OnPropertyChanged(nameof(ShowMoreDanhSachHopDong)); } }
        public int PageDanhSachHopDong { get; set; } = 1;

        private ObservableCollection<HoatDongListModel> _list_chamsockhachhang;
        public ObservableCollection<HoatDongListModel> list_chamsockhachhang { get => _list_chamsockhachhang; set { _list_chamsockhachhang = value; OnPropertyChanged(nameof(list_chamsockhachhang)); } }
        private bool _showMoreChamSocKhachHang;
        public bool ShowMoreChamSocKhachHang { get => _showMoreChamSocKhachHang; set { _showMoreChamSocKhachHang = value; OnPropertyChanged(nameof(ShowMoreChamSocKhachHang)); } }
        public int PageChamSocKhachHang { get; set; } = 1;

        private PhongThuyModel _PhongThuy;
        public PhongThuyModel PhongThuy { get => _PhongThuy; set { _PhongThuy = value; OnPropertyChanged(nameof(PhongThuy)); } }
        public ObservableCollection<HuongPhongThuy> list_HuongTot { set; get; }
        public ObservableCollection<HuongPhongThuy> list_HuongXau { set; get; }

        private string IMAGE_CMND_FOLDER = "Contact_CMND";

        private string _frontImage;
        public string frontImage { get => _frontImage; set { _frontImage = value; OnPropertyChanged(nameof(frontImage)); } }

        private string _behindImage;
        public string behindImage { get => _behindImage; set { _behindImage = value; OnPropertyChanged(nameof(behindImage)); } }

        private bool _showCMND;
        public bool ShowCMND { get => _showCMND; set { _showCMND = value; OnPropertyChanged(nameof(ShowCMND)); } }
        public ObservableCollection<PhotoCMND> CollectionCMNDs { get; set; } = new ObservableCollection<PhotoCMND>();
        public ObservableCollection<FloatButtonItem> ButtonCommandList { get; set; } = new ObservableCollection<FloatButtonItem>();

        public string CodeContac = LookUpMultipleTabs.CodeContac;

        private AddressModel _contactAddress;
        public AddressModel ContactAddress { get => _contactAddress; set { _contactAddress = value; OnPropertyChanged(nameof(ContactAddress)); } }

        private AddressModel _permanentAddress;
        public AddressModel PermanentAddress { get => _permanentAddress; set { _permanentAddress = value; OnPropertyChanged(nameof(PermanentAddress)); } }

        public ContactDetailPageViewModel()
        {
            singleContact = new ContactFormModel();
            list_HuongTot = new ObservableCollection<HuongPhongThuy>();
            list_HuongXau = new ObservableCollection<HuongPhongThuy>();
        }

        // load one contat
        public async Task loadOneContact(String id)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='contact'>
                                    <all-attributes />
                                    <order attribute='createdon' descending='true' />
                                    <link-entity name='account' from='accountid' to='parentcustomerid' visible='false' link-type='outer' alias='aa'>
                                          <attribute name='accountid' alias='_parentcustomerid_value' />
                                          <attribute name='bsd_name' alias='parentcustomerid_label' />
                                    </link-entity>
                                    <link-entity name='bsd_employee' from='bsd_employeeid' to='bsd_employee' visible='false' link-type='outer' alias='a_cf81d7378befeb1194ef000d3a81fcba'>
                                      <attribute name='bsd_employeeid' alias='employee_id'/>
                                    </link-entity>
                                    <filter type='and'>
                                        <condition attribute='contactid' operator='eq' value='" + id + @"' />
                                    </filter>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactFormModel>>("contacts", fetch);
            if (result == null)
            {
                return;
            }
            var tmp = result.value.FirstOrDefault();
            this.singleContact = tmp;
        }
        public async Task GetImageCMND()
        {
            if (!string.IsNullOrWhiteSpace(singleContact.bsd_etag_behind) || !string.IsNullOrWhiteSpace(singleContact.bsd_etag_front))
                ShowCMND = true;
            if (!string.IsNullOrWhiteSpace(singleContact.bsd_etag_behind))
            {
                var urlVideo = await CrmHelper.RetrieveImagesSharePoint<RetrieveMultipleApiResponse<GraphThumbnailsUrlModel>>($"{Config.OrgConfig.SharePointContact_CMNDId}/items/{singleContact.bsd_etag_behind}/driveItem/thumbnails");
                singleContact.bsd_etag_behind_url = urlVideo.value.SingleOrDefault().large.url;
            }
            if (!string.IsNullOrWhiteSpace(singleContact.bsd_etag_front))
            {
                var urlVideo = await CrmHelper.RetrieveImagesSharePoint<RetrieveMultipleApiResponse<GraphThumbnailsUrlModel>>($"{Config.OrgConfig.SharePointContact_CMNDId}/items/{singleContact.bsd_etag_front}/driveItem/thumbnails");
                singleContact.bsd_etag_front_url = urlVideo.value.SingleOrDefault().large.url;
            }
        }

        // giao dich

        //DANH SACH DAT CHO
        public async Task LoadQueuesForContactForm(string customerId)
        {
            string fetchXml = $@"<fetch version='1.0' count='3' page='{PageDanhSachDatCho}' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='opportunity'>
                                <attribute name='name' />
                                <attribute name='customerid' />
                                <attribute name='createdon' />
                                <attribute name='bsd_queuingexpired' />
                                <attribute name='opportunityid' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                  <condition attribute='parentcontactid' operator='eq' uitype='contact' value='{customerId}' />
                                  <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='{UserLogged.Id}' />
                                </filter> 
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='inner' alias='ab'>
                                    <attribute name='bsd_name' alias='bsd_project_name'/>
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
            if (result == null || result.value.Count == 0)
            {
                ShowMoreDanhSachDatCho = false;
                return;
            }
            var data = result.value;
            ShowMoreDanhSachDatCho = data.Count < 3 ? false : true;
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
                list_danhsachdatcho.Add(queue);
            }
        }
        // DANH SACH DAT COC
        public async Task LoadReservationForContactForm(string customerId)
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageDanhSachDatCoc}' output-format='xml-platform' mapping='logical' distinct='false'>
                          <entity name='quote'>
                                <attribute name='name' />
                                <attribute name='totalamount' />
                                <attribute name='bsd_unitno' alias='bsd_unitno_id' />
                                <attribute name='statuscode' />
                                <attribute name='bsd_projectid' alias='bsd_project_id' />
                                <attribute name='quoteid' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                  <condition attribute='customerid' operator='eq' value='{customerId}' />
                                  <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='{UserLogged.Id}' />
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

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationListModel>>("quotes", fetch);
            if (result == null || result.value.Count == 0)
            {
                ShowMoreDanhSachDatCoc = false;
                return;
            }
            var data = result.value;

            if (data.Count < 3)
            {
                ShowMoreDanhSachDatCoc = false;
            }
            else
            {
                ShowMoreDanhSachDatCoc = true;
            }

            foreach (var x in data)
            {
                list_danhsachdatcoc.Add(x);
            }
        }

        // DANH SACH HOP DONG
        public async Task LoadOptoinEntryForContactForm(string customerId)
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageDanhSachHopDong}' output-format='xml-platform' mapping='logical' distinct='false'>
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
                                        <condition attribute='customerid' operator='eq' value='{customerId}' />               
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
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContractModel>>("salesorders", fetch);
            if (result == null || result.value.Count == 0)
            {
                ShowMoreDanhSachHopDong = false;
                return;
            }
            var data = result.value;
            ShowMoreDanhSachHopDong = data.Count < 3 ? false : true;

            foreach (var x in data)
            {
                list_danhsachhopdong.Add(x);
            }
        }

        // CHAM SOC KHACH HANG
        public async Task LoadCaseForContactForm()
        {
            if(list_chamsockhachhang != null && singleContact.contactid != Guid.Empty)
            {
                await Task.WhenAll(
                //LoadActiviy(singleContact.contactid, "task", "tasks"),
                //LoadActiviy(singleContact.contactid, "phonecall", "phonecalls"),
                //LoadActiviy(singleContact.contactid, "appointment", "appointments"),
                LoadTasks(singleContact.contactid),
                LoadMettings(singleContact.contactid),
                LoadPhoneCalls(singleContact.contactid)
                );
            }
            ShowMoreChamSocKhachHang = list_chamsockhachhang.Count < (3* PageChamSocKhachHang) ? false : true;
        }

        public async Task LoadActiviy(Guid contactID, string entity, string entitys)
        {
            string forphonecall = null;
            if (entity == "phonecall")
            {
                forphonecall = @"<link-entity name='activityparty' from='activityid' to='activityid' link-type='outer' alias='aee'>
                                        <filter type='and'>
                                            <condition attribute='participationtypemask' operator='eq' value='2' />
                                        </filter>
                                        <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='aff'>
                                            <attribute name='fullname' alias='callto_contact_name'/>
                                        </link-entity>
                                        <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='agg'>
                                            <attribute name='bsd_name' alias='callto_account_name'/>
                                        </link-entity>
                                        <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='ahh'>
                                            <attribute name='fullname' alias='callto_lead_name'/>
                                        </link-entity>
                                    </link-entity>";
            }

            string fetch = $@"<fetch version='1.0' count='3' page='{PageChamSocKhachHang}' output-format='xml-platform' mapping='logical' distinct='true'>
                                <entity name='{entity}'>
                                    <attribute name='subject' />
                                    <attribute name='statecode' />
                                    <attribute name='activityid' />
                                    <attribute name='scheduledstart' />
                                    <attribute name='scheduledend' /> 
                                    <attribute name='activitytypecode' /> 
                                    <order attribute='modifiedon' descending='true' />
                                    <filter type='and'>
                                        <filter type='or'>
                                            <condition entityname='party' attribute='partyid' operator='eq' value='{contactID}'/>
                                            <condition attribute='regardingobjectid' operator='eq' value='{contactID}' />
                                        </filter>
                                        <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='{UserLogged.Id}' />
                                    </filter>
                                    <link-entity name='activityparty' from='activityid' to='activityid' link-type='inner' alias='party'/>
                                    <link-entity name='account' from='accountid' to='regardingobjectid' link-type='outer' alias='ae'>
                                        <attribute name='bsd_name' alias='accounts_bsd_name'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='regardingobjectid' link-type='outer' alias='af'>
                                        <attribute name='fullname' alias='contact_bsd_fullname'/>
                                    </link-entity>
                                    <link-entity name='lead' from='leadid' to='regardingobjectid' link-type='outer' alias='ag'>
                                        <attribute name='fullname' alias='lead_fullname'/>
                                    </link-entity>
                                    {forphonecall}
                                </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<HoatDongListModel>>(entitys, fetch);
            if (result != null || result.value.Count > 0)
            {
                var data = result.value;
                foreach (var x in data)
                {
                    list_chamsockhachhang.Add(x);
                }
            }
        }

        public async Task LoadTasks(Guid contactID)
        {
            string fetchXml = $@"<fetch version='1.0' count='3' page='{PageChamSocKhachHang}' output-format='xml-platform' mapping='logical' distinct='true'>
                                <entity name='task'>
                                    <attribute name='subject' />
                                    <attribute name='statecode' />
                                    <attribute name='activityid' />
                                    <attribute name='scheduledstart' />
                                    <attribute name='scheduledend' /> 
                                    <attribute name='activitytypecode' /> 
                                    <order attribute='modifiedon' descending='true' />
                                    <filter type='and'>
                                        <filter type='or'>
                                            <condition entityname='party' attribute='partyid' operator='eq' value='{contactID}'/>
                                            <condition attribute='regardingobjectid' operator='eq' value='{contactID}' />
                                        </filter>
                                        <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='{UserLogged.Id}' />
                                    </filter>
                                    <link-entity name='activityparty' from='activityid' to='activityid' link-type='inner' alias='party'/>
                                    <link-entity name='account' from='accountid' to='regardingobjectid' link-type='outer' alias='ae'>
                                        <attribute name='bsd_name' alias='accounts_bsd_name'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='regardingobjectid' link-type='outer' alias='af'>
                                        <attribute name='fullname' alias='contact_bsd_fullname'/>
                                    </link-entity>
                                    <link-entity name='lead' from='leadid' to='regardingobjectid' link-type='outer' alias='ag'>
                                        <attribute name='fullname' alias='lead_fullname'/>
                                    </link-entity>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<HoatDongListModel>>("tasks", fetchXml);
            if (result == null || result.value.Count == 0) return;

            foreach (var item in result.value)
            {
                if (!string.IsNullOrWhiteSpace(item.contact_bsd_fullname))
                {
                    item.customer = item.contact_bsd_fullname;
                }
                if (!string.IsNullOrWhiteSpace(item.accounts_bsd_name))
                {
                    item.customer = item.accounts_bsd_name;
                }
                if (!string.IsNullOrWhiteSpace(item.lead_fullname))
                {
                    item.customer = item.lead_fullname;
                }

                this.list_chamsockhachhang.Add(item);
            }
        }

        public async Task LoadMettings(Guid contactID)
        {
            string fetchXml = $@"<fetch version='1.0' count='3' page='{PageChamSocKhachHang}' output-format='xml-platform' mapping='logical' distinct='true'>
                                <entity name='appointment'>
                                    <attribute name='subject' />
                                    <attribute name='statecode' />
                                    <attribute name='activityid' />
                                    <attribute name='scheduledstart' />
                                    <attribute name='scheduledend' /> 
                                    <attribute name='activitytypecode' /> 
                                    <order attribute='modifiedon' descending='true' />
                                    <filter type='and'>
                                        <filter type='or'>
                                            <condition entityname='party' attribute='partyid' operator='eq' value='{contactID}'/>
                                            <condition attribute='regardingobjectid' operator='eq' value='{contactID}' />
                                        </filter>
                                        <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='{UserLogged.Id}' />
                                    </filter>
                                    <link-entity name='activityparty' from='activityid' to='activityid' link-type='inner' alias='party'/>
                                    <link-entity name='account' from='accountid' to='regardingobjectid' link-type='outer' alias='ae'>
                                        <attribute name='bsd_name' alias='accounts_bsd_name'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='regardingobjectid' link-type='outer' alias='af'>
                                        <attribute name='fullname' alias='contact_bsd_fullname'/>
                                    </link-entity>
                                    <link-entity name='lead' from='leadid' to='regardingobjectid' link-type='outer' alias='ag'>
                                        <attribute name='fullname' alias='lead_fullname'/>
                                    </link-entity>
                                    <link-entity name='activityparty' from='activityid' to='activityid' link-type='outer' alias='aee'>
                                        <filter type='and'>
                                            <condition attribute='participationtypemask' operator='eq' value='5' />
                                        </filter>
                                        <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='aff'>
                                            <attribute name='fullname' alias='callto_contact_name'/>
                                        </link-entity>
                                        <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='agg'>
                                            <attribute name='bsd_name' alias='callto_account_name'/>
                                        </link-entity>
                                        <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='ahh'>
                                            <attribute name='fullname' alias='callto_lead_name'/>
                                        </link-entity>
                                    </link-entity>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<HoatDongListModel>>("appointments", fetchXml);
            if (result == null || result.value.Count == 0) return;

            foreach (var item in result.value)
            {
                var meet = list_chamsockhachhang.FirstOrDefault(x => x.activityid == item.activityid);
                if (meet != null)
                {
                    if (!string.IsNullOrWhiteSpace(item.callto_contact_name))
                    {
                        string new_customer = ", " + item.callto_contact_name;
                        meet.customer += new_customer;
                    }
                    if (!string.IsNullOrWhiteSpace(item.callto_account_name))
                    {
                        string new_customer = ", " + item.callto_account_name;
                        meet.customer += new_customer;
                    }
                    if (!string.IsNullOrWhiteSpace(item.callto_lead_name))
                    {
                        string new_customer = ", " + item.callto_lead_name;
                        meet.customer += new_customer;
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(item.callto_contact_name))
                    {
                        item.customer = item.callto_contact_name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.callto_account_name))
                    {
                        item.customer = item.callto_account_name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.callto_lead_name))
                    {
                        item.customer = item.callto_lead_name;
                    }
                    this.list_chamsockhachhang.Add(item);
                }
            }
        }

        public async Task LoadPhoneCalls(Guid contactID)
        {
            string fetchXml = $@"<fetch version='1.0' count='3' page='{PageChamSocKhachHang}' output-format='xml-platform' mapping='logical' distinct='true'>
                                <entity name='phonecall'>
                                    <attribute name='subject' />
                                    <attribute name='statecode' />
                                    <attribute name='activityid' />
                                    <attribute name='scheduledstart' />
                                    <attribute name='scheduledend' /> 
                                    <attribute name='activitytypecode' /> 
                                    <order attribute='modifiedon' descending='true' />
                                    <filter type='and'>
                                        <filter type='or'>
                                            <condition entityname='party' attribute='partyid' operator='eq' value='{contactID}'/>
                                            <condition attribute='regardingobjectid' operator='eq' value='{contactID}' />
                                        </filter>
                                        <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='{UserLogged.Id}' />
                                    </filter>
                                    <link-entity name='activityparty' from='activityid' to='activityid' link-type='inner' alias='party'/>
                                    <link-entity name='account' from='accountid' to='regardingobjectid' link-type='outer' alias='ae'>
                                        <attribute name='bsd_name' alias='accounts_bsd_name'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='regardingobjectid' link-type='outer' alias='af'>
                                        <attribute name='fullname' alias='contact_bsd_fullname'/>
                                    </link-entity>
                                    <link-entity name='lead' from='leadid' to='regardingobjectid' link-type='outer' alias='ag'>
                                        <attribute name='fullname' alias='lead_fullname'/>
                                    </link-entity>
                                    <link-entity name='activityparty' from='activityid' to='activityid' link-type='outer' alias='aee'>
                                        <filter type='and'>
                                            <condition attribute='participationtypemask' operator='eq' value='2' />
                                        </filter>
                                        <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='aff'>
                                            <attribute name='fullname' alias='callto_contact_name'/>
                                        </link-entity>
                                        <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='agg'>
                                            <attribute name='bsd_name' alias='callto_account_name'/>
                                        </link-entity>
                                        <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='ahh'>
                                            <attribute name='fullname' alias='callto_lead_name'/>
                                        </link-entity>
                                    </link-entity>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<HoatDongListModel>>("phonecalls", fetchXml);
            if (result == null || result.value.Count == 0) return;

            foreach (var item in result.value)
            {
                if (!string.IsNullOrWhiteSpace(item.callto_contact_name))
                {
                    item.customer = item.callto_contact_name;
                }
                if (!string.IsNullOrWhiteSpace(item.callto_account_name))
                {
                    item.customer = item.callto_account_name;
                }
                if (!string.IsNullOrWhiteSpace(item.callto_lead_name))
                {
                    item.customer = item.callto_lead_name;
                }

                this.list_chamsockhachhang.Add(item);
            }
        }

        // phon thuy
        public void LoadPhongThuy()
        {
            if (singleContact.gendercode != null)
            {
                singleGender = ContactGender.GetGenderById(singleContact.gendercode);
            }
            if (list_HuongTot != null || list_HuongXau != null)
            {
                list_HuongTot.Clear();
                list_HuongXau.Clear();
                if (singleContact != null && singleContact.gendercode != null && singleGender != null)
                {
                    PhongThuy.gioi_tinh = Int32.Parse(singleContact.gendercode);
                    PhongThuy.nam_sinh = singleContact.birthdate.HasValue ? singleContact.birthdate.Value.Year : 0;
                    if (PhongThuy.huong_tot != null && PhongThuy.huong_tot != null)
                    {
                        string[] huongtot = PhongThuy.huong_tot.Split('\n');
                        string[] huongxau = PhongThuy.huong_xau.Split('\n');
                        int i = 1;
                        foreach (var x in huongtot)
                        {
                            string[] huong = x.Split(':');
                            string name_huong = i + ". " + huong[0];
                            string detail_huong = huong[1].Remove(0, 1);
                            list_HuongTot.Add(new HuongPhongThuy { Name = name_huong, Detail = detail_huong });
                            i++;
                        }
                        int j = 1;
                        foreach (var x in huongxau)
                        {
                            string[] huong = x.Split(':');
                            string name_huong = j + ". " + huong[0];
                            string detail_huong = huong[1].Remove(0, 1);
                            list_HuongXau.Add(new HuongPhongThuy { Name = name_huong, Detail = detail_huong });
                            j++;
                        }
                    }
                }
                else
                {
                    PhongThuy.gioi_tinh = 0;
                    PhongThuy.nam_sinh = 0;
                }
            }
        }
    }
}
