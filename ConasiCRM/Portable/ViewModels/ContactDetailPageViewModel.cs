using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class ContactDetailPageViewModel : FormViewModal
    {
        private ContactFormModel _singleContact;
        public ContactFormModel singleContact { get { return _singleContact; } set { _singleContact = value; OnPropertyChanged(nameof(singleContact)); } }

        private OptionSet _singleGender;
        public OptionSet singleGender { get => _singleGender; set { _singleGender = value; OnPropertyChanged(nameof(singleGender)); } }
        public ObservableCollection<OptionSet> list_gender_optionset { get; set; }
        private string _singleContactgroup;
        public string SingleContactgroup { get => _singleContactgroup; set { _singleContactgroup = value; OnPropertyChanged(nameof(SingleContactgroup)); } }
        private string _singleType;
        public string SingleType { get => _singleType; set { _singleType = value; OnPropertyChanged(nameof(SingleType)); } }
    
        public ObservableCollection<QueueListModel> list_danhsachdatcho { get; set; }
        private bool _showMoreDanhSachDatCho;
        public bool ShowMoreDanhSachDatCho { get => _showMoreDanhSachDatCho; set { _showMoreDanhSachDatCho = value; OnPropertyChanged(nameof(ShowMoreDanhSachDatCho)); } }
        public int PageDanhSachDatCho { get; set; } = 1;

        public ObservableCollection<QuotationReseravtion> list_danhsachdatcoc { get; set; }
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

        string frontImage_name;
        string behindImage_name;

        public ContactDetailPageViewModel()
        {
            singleGender = new OptionSet();
            list_gender_optionset = new ObservableCollection<OptionSet>();
            list_danhsachdatcho = new ObservableCollection<QueueListModel>();
            list_danhsachdatcoc = new ObservableCollection<QuotationReseravtion>();
            list_danhsachhopdong = new ObservableCollection<OptionEntry>();
            list_chamsockhachhang = new ObservableCollection<Case>();
            list_HuongTot = new ObservableCollection<HuongPhongThuy>();
            list_HuongXau = new ObservableCollection<HuongPhongThuy>();            
            optionEntryHasOnlyTerminatedStatus = true;
            LoadGender();           
        }

        // load one contat
        public async Task loadOneContact(String id)
        {
            singleContact = new ContactFormModel();
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='contact'>
                                    <all-attributes />
                                    <order attribute='createdon' descending='true' />
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
            //if (tmp.bsd_loingysinh == false)
            //{
            //    checkbirth = true;
            //    checkbirthy = false;
            //}
            //else { checkbirth = false; checkbirthy = true; }
            frontImage_name = tmp.contactid.ToString().Replace("-", String.Empty).ToUpper() + "_front.jpg";
            behindImage_name = tmp.contactid.ToString().Replace("-", String.Empty).ToUpper() + "_behind.jpg";
        }
        //Gender
        public void LoadGender()
        {
            list_gender_optionset.Add(new OptionSet() { Val = ("1"), Label = "Nam", });
            list_gender_optionset.Add(new OptionSet() { Val = ("2"), Label = "Nữ", });
            list_gender_optionset.Add(new OptionSet() { Val = ("100000000"), Label = "Khác", });
        }

        public OptionSet LoadOneGender(string id)
        {
            this.singleGender = list_gender_optionset.FirstOrDefault(x => x.Val == id); ;
            return singleGender;
        }
        // giao dich

        //DANH SACH DAT COC
        public async Task LoadQueuesForContactForm(string customerId)
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageDanhSachDatCho}' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='opportunity'>
                                <attribute name='opportunityid' />
                                <attribute name='customerid' alias='customer_id' />
                                <attribute name='name' alias='unit_name'/>
                                <attribute name='bsd_queuenumber' />
                                <attribute name='bsd_project' alias='project_id' />
                                <attribute name='estimatedvalue' />
                                <attribute name='statuscode' />
                                <attribute name='actualclosedate' />
                                <attribute name='createdon' />
                                <order attribute='actualclosedate' descending='true' />
                                <filter type='and'>
                                  <condition attribute='customerid' operator='eq' value='{customerId}' />
                                </filter>
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
            var data = result.value;

            if (data.Count < 3)
            {
                ShowMoreDanhSachDatCho = false;
            }
            else
            {
                ShowMoreDanhSachDatCho = true;
            }

            foreach (var x in data)
            {
                list_danhsachdatcho.Add(x);
            }
        }
        // DANH SACH DAT COC
        public async Task LoadReservationForContactForm(string customerId)
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
                            <filter type='and'>
                              <condition attribute='customerid' operator='eq' value='{customerId}' />
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
            if (result == null)
            {
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
                          </entity>
                        </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionEntry>>("salesorders", fetch);
            if (result == null)
            {
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
                          </entity>
                        </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Case>>("incidents", fetch);
            if (result == null)
            {
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
            PhongThuy = new PhongThuyModel();
            LoadOneGender(singleContact.gendercode);
            if (list_HuongTot != null || list_HuongXau != null)
            {
                list_HuongTot.Clear();
                list_HuongXau.Clear();
                if (singleContact != null && singleContact.gendercode != null && singleGender != null && singleGender.Val != null)
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
