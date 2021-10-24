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

namespace ConasiCRM.Portable.ViewModels
{
    public class ContactDetailPageViewModel : BaseViewModel
    {
        private ContactFormModel _singleContact;
        public ContactFormModel singleContact { get { return _singleContact; } set { _singleContact = value; OnPropertyChanged(nameof(singleContact)); } }

        private OptionSet _singleGender;
        public OptionSet singleGender { get => _singleGender; set { _singleGender = value; OnPropertyChanged(nameof(singleGender)); } }

        private OptionSet _singleLocalization;
        public OptionSet SingleLocalization { get => _singleLocalization; set { _singleLocalization = value; OnPropertyChanged(nameof(SingleLocalization)); } }
        public ObservableCollection<QueueFormModel> list_danhsachdatcho { get; set; } = new ObservableCollection<QueueFormModel>();

        private bool _showMoreDanhSachDatCho;
        public bool ShowMoreDanhSachDatCho { get => _showMoreDanhSachDatCho; set { _showMoreDanhSachDatCho = value; OnPropertyChanged(nameof(ShowMoreDanhSachDatCho)); } }
        public int PageDanhSachDatCho { get; set; } = 1;

        public ObservableCollection<ReservationListModel> list_danhsachdatcoc { get; set; }
        private bool _showMoreDanhSachDatCoc;
        public bool ShowMoreDanhSachDatCoc { get => _showMoreDanhSachDatCoc; set { _showMoreDanhSachDatCoc = value; OnPropertyChanged(nameof(ShowMoreDanhSachDatCoc)); } }
        public int PageDanhSachDatCoc { get; set; } = 1;

        public ObservableCollection<OptionEntry> list_danhsachhopdong { get; set; }
        private bool _showMoreDanhSachHopDong;
        public bool ShowMoreDanhSachHopDong { get => _showMoreDanhSachHopDong; set { _showMoreDanhSachHopDong = value; OnPropertyChanged(nameof(ShowMoreDanhSachHopDong)); } }
        public int PageDanhSachHopDong { get; set; } = 1;

        public ObservableCollection<Case> list_chamsockhachhang { get; set; }
        private bool _showMoreChamSocKhachHang;
        public bool ShowMoreChamSocKhachHang { get => _showMoreChamSocKhachHang; set { _showMoreChamSocKhachHang = value; OnPropertyChanged(nameof(ShowMoreChamSocKhachHang)); } }
        public int PageChamSocKhachHang { get; set; } = 1;
        private bool _optionEntryHasOnlyTerminatedStatus;
        public bool optionEntryHasOnlyTerminatedStatus { get => _optionEntryHasOnlyTerminatedStatus; set { _optionEntryHasOnlyTerminatedStatus = value; OnPropertyChanged(nameof(optionEntryHasOnlyTerminatedStatus)); } }

        private PhongThuyModel _PhongThuy;
        public PhongThuyModel PhongThuy { get => _PhongThuy; set { _PhongThuy = value; OnPropertyChanged(nameof(PhongThuy)); } }
        public ObservableCollection<HuongPhongThuy> list_HuongTot { set; get; }
        public ObservableCollection<HuongPhongThuy> list_HuongXau { set; get; }

        private string IMAGE_CMND_FOLDER = "Contact_CMND";

        private string _frontImage;
        public string frontImage { get => _frontImage; set { _frontImage = value; OnPropertyChanged(nameof(frontImage)); } }

        private string _behindImage;
        public string behindImage { get => _behindImage; set { _behindImage = value; OnPropertyChanged(nameof(behindImage)); } }
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
            if(result == null)
            {
                return;
            }    
            var tmp = result.value.FirstOrDefault();
            this.singleContact = tmp;
        }

        public async Task GetImageCMND()
        {
            if (this.singleContact.contactid != Guid.Empty)
            {
                var frontImage_name = this.singleContact.contactid.ToString().Replace("-", String.Empty).ToUpper() + "_front.jpg";
                var behindImage_name = this.singleContact.contactid.ToString().Replace("-", String.Empty).ToUpper() + "_behind.jpg";

                string token = (await CrmHelper.getSharePointToken()).access_token;
                frontImage = OrgConfig.SharePointResource + "/sites/" + OrgConfig.SharePointSiteName + "/_layouts/15/download.aspx?SourceUrl=/sites/" + OrgConfig.SharePointSiteName + "/" + IMAGE_CMND_FOLDER + "/" + frontImage_name + "&access_token=" + token;
                behindImage = OrgConfig.SharePointResource + "/sites/" + OrgConfig.SharePointSiteName + "/_layouts/15/download.aspx?SourceUrl=/sites/" + OrgConfig.SharePointSiteName + "/" + IMAGE_CMND_FOLDER + "/" + behindImage_name + "&access_token=" + token;               
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

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QuotationReseravtion>>("quotes", fetch);
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
                            <attribute name='salesorderid' />
                            <attribute name='bsd_optionno' />
                            <attribute name='statuscode' />
                            <attribute name='totalamount' />
                            <attribute name='bsd_signingexpired' />
                            <attribute name='createdon' />
                            <order attribute='bsd_signingexpired' descending='true' />
                            <filter type='and'>
                              <condition attribute='customerid' operator='eq' value='{customerId}' />
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
                            <link-entity name='bsd_employee' from='bsd_employeeid' to='bsd_employee' link-type='inner' alias='ae'>
                                    <filter type='and'>
                                          <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                                    </filter>
                                </link-entity>
                          </entity>
                        </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionEntry>>("salesorders", fetch);
            if (result == null || result.value.Count == 0)
            {
                ShowMoreDanhSachHopDong = false;
                return;
            }
            var data = result.value;

            if (data.Count < 3)
            {
                ShowMoreDanhSachHopDong = false;
            }
            else
            {
                ShowMoreDanhSachHopDong = true;
            }

            foreach (var x in data)
            {
                if (x.statuscode != "100000006") { optionEntryHasOnlyTerminatedStatus = false; }
                list_danhsachhopdong.Add(x);
            }
        }

        // CHAM SOC KHACH HANG
        public async Task LoadCaseForContactForm(string customerId)
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageChamSocKhachHang}' output-format='xml-platform' mapping='logical' distinct='false'>
                          <entity name='incident'>
                            <attribute name='title' alias='title_label'/>
                            <attribute name='ticketnumber' />
                            <attribute name='createdon' />
                            <attribute name='incidentid' />
                            <attribute name='caseorigincode' />
                            <attribute name='statuscode' />
                            <attribute name='prioritycode' />
                            <order attribute='title' descending='false' />
                            <filter type='and'>
                              <condition attribute='customerid' operator='eq' value='{customerId}' />
                            </filter>
                            <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer'>
                                <attribute name='fullname'  alias='customerid_label_contact'/>
                            </link-entity>
                            <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer'>
                                <attribute name='name'  alias='customerid_label_account'/>
                            </link-entity>
                            <link-entity name='bsd_employee' from='bsd_employeeid' to='bsd_employee' link-type='inner' alias='ae'>
                                    <filter type='and'>
                                          <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                                    </filter>
                            </link-entity>
                          </entity>
                        </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Case>>("incidents", fetch);
            if (result == null || result.value.Count == 0)
            {
                ShowMoreChamSocKhachHang = false;
                return;
            }
            var data = result.value;

            if (data.Count < 3)
            {
                ShowMoreChamSocKhachHang = false;
            }
            else
            {
                ShowMoreChamSocKhachHang = true;
            }

            foreach (var x in data)
            {
                list_chamsockhachhang.Add(x);
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
