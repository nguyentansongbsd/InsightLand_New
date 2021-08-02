using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
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
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class ContactFormViewModel : FormViewModal
    {
        private ContactFormModel _singleContact;
        public ContactFormModel singleContact { get { return _singleContact; } set { _singleContact = value; OnPropertyChanged(nameof(singleContact)); } }
        private OptionSet _singleGender;
        public OptionSet singleGender { get => _singleGender; set { _singleGender = value; OnPropertyChanged(nameof(singleGender)); } }
        private OptionSet _singleLocalization;
        public OptionSet singleLocalization { get => _singleLocalization; set { _singleLocalization = value; OnPropertyChanged(nameof(singleLocalization)); } }
        public OptionSet _singleContactgroup;
        public OptionSet singleContactgroup { get => _singleContactgroup; set { _singleContactgroup = value; OnPropertyChanged(nameof(singleContactgroup)); } }

        public ObservableCollection<LookUp> list_contact_lookup { get; set; }

        public ObservableCollection<LookUp> list_account_lookup { get; set; }
        private LookUp _account;
        public LookUp Account { get => _account; set { _account = value; OnPropertyChanged(nameof(Account)); } }

        public ObservableCollection<LookUp> list_lookup { get; set; }
        
        public ObservableCollection<LookUp> list_country_lookup { get; set; }
        public ObservableCollection<LookUp> list_province_lookup { get; set; }
        public ObservableCollection<LookUp> list_district_lookup { get; set; }

        public ObservableCollection<Provinces> list_nhucauvediadiem { get; set; }
        public ObservableCollection<Provinces> list_provinceefornhucaudiadiem { get; set; }
        public ObservableCollection<QueueListModel> list_danhsachdatcho { get; set; }
        public ObservableCollection<QuotationReseravtion> list_danhsachdatcoc { get; set; }
        public ObservableCollection<OptionEntry> list_danhsachhopdong { get; set; }
        public ObservableCollection<Case> list_chamsockhachhang { get; set; }

        public ObservableCollection<OptionSet> list_gender_optionset { get; set; }
        public ObservableCollection<OptionSet> list_localization_optionset { get; set; }
        public ObservableCollection<OptionSet> list_contactgroup_optionset { get; set; }

        private ObservableCollection<ProjectList> _list_Duanquantam;
        public ObservableCollection<ProjectList> list_Duanquantam { get { return _list_Duanquantam; } set { _list_Duanquantam = value; OnPropertyChanged(nameof(_list_Duanquantam)); } }

        public ObservableCollection<ProjectList> list_project_lookup { set; get; }

        public int pageLookup_contact;
        public bool moreLookup_contact;
        public int pageLookup_province;
        public bool morelookup_province;
        public int pageLookup_account;
        public bool moreLooup_account;
        public int pageLookup_country;
        public bool morelookup_country;
        public int pageLookup_district;
        public bool morelookup_district;

        public int pageLookup_provincefornhucaudiadiem;
        public bool morelookup_provincefornhucaudiadiem;

        private bool _optionEntryHasOnlyTerminatedStatus;
        public bool optionEntryHasOnlyTerminatedStatus { get => _optionEntryHasOnlyTerminatedStatus; set { _optionEntryHasOnlyTerminatedStatus = value; OnPropertyChanged(nameof(optionEntryHasOnlyTerminatedStatus)); } }

        private PhongThuyModel _PhongThuy;
        public PhongThuyModel PhongThuy { get => _PhongThuy; set { _PhongThuy = value; OnPropertyChanged(nameof(PhongThuy)); } }

        private bool _looking_up;
        public bool looking_up { get => _looking_up; set { _looking_up = value; OnPropertyChanged(nameof(looking_up)); } }

        private string IMAGE_CMND_FOLDER = "Contact_CMND";
        string frontImage_name;
        string behindImage_name;

        public ObservableCollection<OptionSet> TypeOptions { get; set; }
        public ObservableCollection<string> SelectedTypes { get; set; }
        public bool checkbirth;
        public bool checkbirthy;

        private bool _showMoreNhuCauDiaDiem;
        public bool ShowMoreNhuCauDiaDiem { get => _showMoreNhuCauDiaDiem; set { _showMoreNhuCauDiaDiem = value; OnPropertyChanged(nameof(ShowMoreNhuCauDiaDiem)); } }

        public int PageNhuCauDiaDiem { get; set; } = 1;

        private bool _showMoreDanhSachDatCho;
        public bool ShowMoreDanhSachDatCho { get => _showMoreDanhSachDatCho; set { _showMoreDanhSachDatCho = value; OnPropertyChanged(nameof(ShowMoreDanhSachDatCho)); } }

        public int PageDanhSachDatCho { get; set; } = 1;

        private bool _showMoreDanhSachDatCoc;
        public bool ShowMoreDanhSachDatCoc { get => _showMoreDanhSachDatCoc; set { _showMoreDanhSachDatCoc = value; OnPropertyChanged(nameof(ShowMoreDanhSachDatCoc)); } }

        public int PageDanhSachDatCoc { get; set; } = 1;

        private bool _showMoreDanhSachHopDong;
        public bool ShowMoreDanhSachHopDong { get => _showMoreDanhSachHopDong; set { _showMoreDanhSachHopDong = value; OnPropertyChanged(nameof(ShowMoreDanhSachHopDong)); } }

        public int PageDanhSachHopDong { get; set; } = 1;

        private bool _showMoreChamSocKhachHang;
        public bool ShowMoreChamSocKhachHang { get => _showMoreChamSocKhachHang; set { _showMoreChamSocKhachHang = value; OnPropertyChanged(nameof(ShowMoreChamSocKhachHang)); } }
        public int PageChamSocKhachHang { get; set; } = 1;
       
        private bool _showMoreDuAnQuanTam;
        public bool ShowMoreDuAnQuanTam { get => _showMoreDuAnQuanTam; set { _showMoreDuAnQuanTam = value; OnPropertyChanged(nameof(ShowMoreDuAnQuanTam)); } }

        public int PageDuAnQuanTam { get; set; } = 1;

        public ContactFormViewModel()
        {
            singleContact = new ContactFormModel();
            singleGender = new OptionSet();
            singleContactgroup = new OptionSet();

            list_lookup = new ObservableCollection<LookUp>();
            list_contact_lookup = new ObservableCollection<LookUp>();
            list_account_lookup = new ObservableCollection<LookUp>();
            list_country_lookup = new ObservableCollection<LookUp>();
            list_province_lookup = new ObservableCollection<LookUp>();
            list_district_lookup = new ObservableCollection<LookUp>();

            list_nhucauvediadiem = new ObservableCollection<Provinces>();
            list_provinceefornhucaudiadiem = new ObservableCollection<Provinces>();
            list_danhsachdatcho = new ObservableCollection<QueueListModel>();
            list_danhsachdatcoc = new ObservableCollection<QuotationReseravtion>();
            list_danhsachhopdong = new ObservableCollection<OptionEntry>();
            list_chamsockhachhang = new ObservableCollection<Case>();

            list_gender_optionset = new ObservableCollection<OptionSet>();
            list_localization_optionset = new ObservableCollection<OptionSet>();
            list_contactgroup_optionset = new ObservableCollection<OptionSet>();

            list_project_lookup = new ObservableCollection<ProjectList>();
            list_Duanquantam = new ObservableCollection<ProjectList>();

            SelectedTypes = new ObservableCollection<string>();
            TypeOptions = new ObservableCollection<OptionSet>()
            {
                new OptionSet("100000000","Customer"),
                new OptionSet("100000001","Collaborator"),
                new OptionSet("100000002","Authorized"),
                new OptionSet("100000003","Legal Representative")
            };

            PhongThuy = new PhongThuyModel();

            pageLookup_contact = 1;
            moreLookup_contact = true;
            pageLookup_province = 1;
            morelookup_province = true;
            pageLookup_account = 1;
            moreLooup_account = true;
            pageLookup_country = 1;
            morelookup_country = true;
            pageLookup_province = 1;
            morelookup_province = true;
            pageLookup_district = 1;
            morelookup_district = true;

            pageLookup_provincefornhucaudiadiem = 1;
            morelookup_provincefornhucaudiadiem = true;

            optionEntryHasOnlyTerminatedStatus = true;

            looking_up = false;
            checkbirth = false;
            checkbirthy = false;

            this.loadGender();
            this.loadLocalization();
            this.loadContacGroup();
        }

        public void reset()
        {
            singleContact = new ContactFormModel();
         //   singleLocalization = new OptionSet();
            singleGender = new OptionSet();
            singleContactgroup = new OptionSet();

            list_nhucauvediadiem.Clear();
            list_danhsachdatcho.Clear();
            list_danhsachdatcoc.Clear();
            list_danhsachhopdong.Clear();
            list_chamsockhachhang.Clear();
            list_Duanquantam.Clear();

            optionEntryHasOnlyTerminatedStatus = true;

            list_gender_optionset.Clear();
            list_localization_optionset.Clear();

            this.loadGender();
            this.loadLocalization();
            this.loadContacGroup();
        }

        public async Task loadOneContact(String id)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='contact'>
                                    <all-attributes />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                        <condition attribute='contactid' operator='eq' value='" + id + @"' />
                                    </filter>
                                    <link-entity name='contact' from='contactid' to='bsd_protecter' visible='false' link-type='outer'>
                                        <attribute name='fullname'  alias='bsd_protecter_label'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='parentcustomerid' visible='false' link-type='outer'>
                                        <attribute name='fullname'  alias='parentcustomerid_label_contact'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='parentcustomerid' visible='false' link-type='outer'>
                                        <attribute name='name'  alias='parentcustomerid_label_account'/>
                                    </link-entity>
                                    <link-entity name='bsd_country' from='bsd_countryid' to='bsd_country' visible='false' link-type='outer'>
                                        <attribute name='bsd_countryname'  alias='bsd_country_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_country_en'/>
                                    </link-entity>
                                    <link-entity name='new_province' from='new_provinceid' to='bsd_province' visible='false' link-type='outer'>
                                        <attribute name='bsd_provincename'  alias='bsd_province_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_province_en'/>
                                    </link-entity>
                                    <link-entity name='new_district' from='new_districtid' to='bsd_district' visible='false' link-type='outer'>
                                        <attribute name='new_name'  alias='bsd_district_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_district_en'/>
                                    </link-entity>
                                    <link-entity name='bsd_country' from='bsd_countryid' to='bsd_permanentcountry' visible='false' link-type='outer'>
                                        <attribute name='bsd_countryname'  alias='bsd_permanentcountry_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_permanentcountry_en'/>
                                    </link-entity>
                                    <link-entity name='new_province' from='new_provinceid' to='bsd_permanentprovince' visible='false' link-type='outer'>
                                        <attribute name='bsd_provincename'  alias='bsd_permanentprovince_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_permanentprovince_en'/>
                                    </link-entity>
                                    <link-entity name='new_district' from='new_districtid' to='bsd_permanentdistrict' visible='false' link-type='outer'>
                                        <attribute name='new_name'  alias='bsd_permanentdistrict_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_permanentdistrict_en'/>
                                    </link-entity>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactFormModel>>("contacts", fetch);
            var tmp = result.value.FirstOrDefault();
            if (result == null || tmp == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                await Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
            }

            this.singleContact = tmp;
            if (tmp.bsd_loingysinh==false)
            {
                checkbirth = true;
                checkbirthy = false;
            }
            else { checkbirth = false; checkbirthy = true; }
           
            
            frontImage_name = tmp.contactid.ToString().Replace("-", String.Empty).ToUpper() + "_front.jpg";
            behindImage_name = tmp.contactid.ToString().Replace("-", String.Empty).ToUpper() + "_behind.jpg";
        }

        public async Task<Boolean> updateContact(ContactFormModel contact)
        {
            string path = "/contacts(" + contact.contactid + ")";
            var content = await this.getContent(contact);

            CrmApiResponse result = await CrmHelper.PatchData(path, content);

            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                var mess = result.ErrorResponse?.error?.message ?? "Đã xảy ra lỗi. Vui lòng thử lại.";
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", mess, "OK");
                return false;
            }
        }

        public async Task<Guid> createContact(ContactFormModel contact)
        {
            string path = "/contacts";
            contact.contactid = Guid.NewGuid();
            var content = await this.getContent(contact);

            CrmApiResponse result = await CrmHelper.PostData(path, content);

            if (result.IsSuccess)
            {
                return contact.contactid;
            }
            else
            {
                var mess = result.ErrorResponse?.error?.message ?? "Đã xảy ra lỗi. Vui lòng thử lại.";
                await App.Current.MainPage.DisplayAlert("", mess, "OK");
                return new Guid();
            }


        }

        public async Task<Boolean> DeletLookup(string fieldName, Guid contactId)
        {
            var result = await CrmHelper.SetNullLookupField("contacts", contactId, fieldName);
            return result.IsSuccess;
        }

        private async Task<object> getContent(ContactFormModel contact)
        {
            IDictionary<string, object> data = new Dictionary<string, object>();

            //if (contact.bsd_nmsinh != null && contact.bsd_loingysinh == true)
            //{
            //    var tmp = new DateTime(int.Parse(contact.bsd_nmsinh), 1, 1);
            //    contact.birthdate = tmp;
            //}
            data["bsd_loingysinh"] = contact.bsd_loingysinh;
            data["bsd_nmsinh"] = contact.bsd_nmsinh;
            contact.bsd_type = string.Join(",", SelectedTypes);
            data["bsd_lastname"] = contact.bsd_lastname;
            data["lastname"] = contact.lastname;
            data["bsd_fullname"] = contact.bsd_fullname;

            if (!string.IsNullOrEmpty(contact.bsd_firstname))
            {
                data["bsd_firstname"] = contact.bsd_firstname;
                data["firstname"] = contact.firstname;
            }

            data["emailaddress1"] = contact.emailaddress1;
            data["jobtitle"] = contact.jobtitle;
            data["birthdate"] = contact.birthdate.HasValue ? (DateTime.Parse(contact.birthdate.ToString()).ToLocalTime()).ToString("yyyy-MM-dd") : null;
            data["mobilephone"] = contact.mobilephone;
            data["gendercode"] = contact.gendercode;
            data["bsd_identitycardnumber"] = contact.bsd_identitycardnumber;
            data["contactid"] = contact.contactid;
            data["bsd_type"] = contact.bsd_type;
            data["bsd_localization"] = contact.bsd_localization;
            data["bsd_haveprotector"] = contact.bsd_haveprotector;
            data["bsd_dategrant"] = contact.bsd_dategrant.HasValue ? (DateTime.Parse(contact.bsd_dategrant.ToString()).ToLocalTime()).ToString("yyy-MM-dd") : null;
            data["bsd_placeofissue"] = contact.bsd_placeofissue;
            data["bsd_passport"] = contact.bsd_passport;
            data["bsd_issuedonpassport"] = contact.bsd_issuedonpassport.HasValue ? (DateTime.Parse(contact.bsd_issuedonpassport.ToString()).ToLocalTime()).ToString("yyyy-MM-dd") : null;
            data["bsd_placeofissuepassport"] = contact.bsd_placeofissuepassport;
            data["bsd_idcard"] = contact.bsd_idcard;
            data["bsd_issuedonpassport"] = contact.bsd_issuedateidcard.HasValue ? (DateTime.Parse(contact.bsd_issuedateidcard.ToString()).ToLocalTime()).ToString("yyyy-MM-dd") : null;
            data["bsd_placeofissuepassport"] = contact.bsd_placeofissuepassport;
            data["bsd_jobtitlevn"] = contact.bsd_jobtitlevn;
            data["bsd_taxcode"] = contact.bsd_taxcode;
            data["bsd_email2"] = contact.bsd_email2;
            data["telephone1"] = contact.telephone1;
            data["fax"] = contact.fax;
            data["bsd_totaltransaction"] = contact.bsd_totaltransaction;
            data["bsd_customergroup"] = contact.bsd_customergroup;
            data["bsd_housenumberstreet"] = contact.bsd_housenumberstreet;
            data["bsd_housenumber"] = contact.bsd_housenumber;
            data["bsd_permanentaddress"] = contact.bsd_permanentaddress;
            data["bsd_permanenthousenumber"] = contact.bsd_permanenthousenumber;
            data["bsd_contactaddress"] = contact.bsd_contactaddress;
            data["bsd_diachi"] = contact.bsd_diachi;
            data["bsd_permanentaddress1"] = contact.bsd_permanentaddress1;
            data["bsd_diachithuongtru"] = contact.bsd_diachithuongtru;
            data["bsd_tieuchi_vitri"] = contact.bsd_tieuchi_vitri;
            data["bsd_tieuchi_phuongthucthanhtoan"] = contact.bsd_tieuchi_phuongthucthanhtoan;
            data["bsd_tieuchi_giacanho"] = contact.bsd_tieuchi_giacanho;
            data["bsd_tieuchi_nhadautuuytin"] = contact.bsd_tieuchi_nhadautuuytin;
            data["bsd_tieuchi_moitruongsong"] = contact.bsd_tieuchi_moitruongsong;
            data["bsd_tieuchi_baidauxe"] = contact.bsd_tieuchi_baidauxe;
            data["bsd_tieuchi_hethonganninh"] = contact.bsd_tieuchi_hethonganninh;
            data["bsd_tieuchi_huongcanho"] = contact.bsd_tieuchi_huongcanho;
            data["bsd_tieuchi_hethongcuuhoa"] = contact.bsd_tieuchi_hethongcuuhoa;
            data["bsd_tieuchi_nhieutienich"] = contact.bsd_tieuchi_nhieutienich;
            data["bsd_tieuchi_ganchosieuthi"] = contact.bsd_tieuchi_ganchosieuthi;
            data["bsd_tieuchi_gantruonghoc"] = contact.bsd_tieuchi_gantruonghoc;
            data["bsd_tieuchi_ganbenhvien"] = contact.bsd_tieuchi_ganbenhvien;
            data["bsd_tieuchi_dientichcanho"] = contact.bsd_tieuchi_dientichcanho;
            data["bsd_tieuchi_thietkenoithatcanho"] = contact.bsd_tieuchi_thietkenoithatcanho;
            data["bsd_tieuchi_tangdepcanhodep"] = contact.bsd_tieuchi_tangdepcanhodep;
            data["bsd_dientich_3060m2"] = contact.bsd_dientich_3060m2;
            data["bsd_dientich_6080m2"] = contact.bsd_dientich_6080m2;
            data["bsd_dientich_80100m2"] = contact.bsd_dientich_80100m2;
            data["bsd_dientich_100120m2"] = contact.bsd_dientich_100120m2;
            data["bsd_dientich_lonhon120m2"] = contact.bsd_dientich_lonhon120m2;
            data["bsd_quantam_datnen"] = contact.bsd_quantam_datnen;
            data["bsd_quantam_canho"] = contact.bsd_quantam_canho;
            data["bsd_quantam_bietthu"] = contact.bsd_quantam_bietthu;
            data["bsd_quantam_khuthuongmai"] = contact.bsd_quantam_khuthuongmai;
            data["bsd_quantam_nhapho"] = contact.bsd_quantam_nhapho;
            data["bsd_birthyear"] = contact.birthdate.HasValue ? contact.birthdate.Value.Year : 0;
            data["bsd_birthmonth"] = contact.birthdate.HasValue ? contact.birthdate.Value.Month : 0;
            data["bsd_birthdate"] = contact.birthdate.HasValue ? contact.birthdate.Value.Day : 0;
            //data["bsd_mattruoccmnd"] = contact.bsd_mattruoccmnd;
            //data["bsd_matsaucmnd"] = contact.bsd_matsaucmnd;
            

            if (contact._bsd_protecter_value == null || !contact.bsd_haveprotector)
            {
                await DeletLookup("bsd_protecter", contact.contactid);
            }
            else
            {
                data["bsd_protecter@odata.bind"] = "/contacts(" + contact._bsd_protecter_value + ")"; /////Lookup Field
            }

            if (contact._parentcustomerid_value == null)
            {
                await DeletLookup("parentcustomerid", contact.contactid);
            }
            else
            {
                if (contact.parentcustomerid_label_account != null)
                {
                    data["parentcustomerid_account@odata.bind"] = "/accounts(" + contact._parentcustomerid_value + ")"; /////Lookup Field
                }
                else
                {
                    data["parentcustomerid_contact@odata.bind"] = "/contacts(" + contact._parentcustomerid_value + ")"; /////Lookup Field
                }
            }

            if (contact._bsd_country_value == null)
            {
                await DeletLookup("bsd_country", contact.contactid);
            }
            else
            {
                data["bsd_country@odata.bind"] = "/bsd_countries(" + contact._bsd_country_value + ")"; /////Lookup Field
            }

            if (contact._bsd_province_value == null)
            {
                await DeletLookup("bsd_province", contact.contactid);
            }
            else
            {
                data["bsd_province@odata.bind"] = "/new_provinces(" + contact._bsd_province_value + ")"; /////Lookup Field
            }

            if (contact._bsd_district_value == null)
            {
                await DeletLookup("bsd_district", contact.contactid);
            }
            else
            {
                data["bsd_district@odata.bind"] = "/new_districts(" + contact._bsd_district_value + ")"; /////Lookup Field
            }

            if (contact._bsd_permanentcountry_value == null)
            {
                await DeletLookup("bsd_permanentcountry", contact.contactid);
            }
            else
            {
                data["bsd_permanentcountry@odata.bind"] = "/bsd_countries(" + contact._bsd_permanentcountry_value + ")"; /////Lookup Field
            }

            if (contact._bsd_permanentprovince_value == null)
            {
                await DeletLookup("bsd_permanentprovince", contact.contactid);
            }
            else
            {
                data["bsd_permanentprovince@odata.bind"] = "/new_provinces(" + contact._bsd_permanentprovince_value + ")"; /////Lookup Field
            }

            if (contact._bsd_permanentdistrict_value == null)
            {
                await DeletLookup("bsd_permanentdistrict", contact.contactid);
            }
            else
            {
                data["bsd_permanentdistrict@odata.bind"] = "/new_districts(" + contact._bsd_permanentdistrict_value + ")"; /////Lookup Field
            }

            return data;
        }

        public async Task LoadContactsForLookup()
        {
            if (moreLookup_contact)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_contact + @"' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='contact'>
                                        <attribute name='fullname' alias='Name'/>
                                        <attribute name='contactid' alias='Id'/>
                                        <order attribute='createdon' descending='true' />
                                    </entity>
                                </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("contacts", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    moreLookup_contact = false;
                    return;
                }
                foreach (var x in result.value)
                {
                    x.Detail = "Contact";
                    list_contact_lookup.Add(x);
                }
            }

        }


        /////  NHU CAU VE DIA DIEM AREA
        /////////////////////////

        public async Task Load_NhuCauVeDiaDiem(string contactid)
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageNhuCauDiaDiem}' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='new_province'>
                                    <attribute name='new_name' />
                                    <attribute name='createdon' />
                                    <attribute name='new_id' />
                                    <attribute name='bsd_country' />
                                    <attribute name='bsd_priority' />
                                    <attribute name='bsd_provincename' />
                                    <attribute name='new_provinceid' />
                                    <order attribute='bsd_priority' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='statecode' operator='eq' value='0' />
                                    </filter>
                                    <link-entity name='bsd_country' from='bsd_countryid' to='bsd_country' visible='false' link-type='outer'>
                                        <attribute name='bsd_name'  alias='bsd_countries'/>
                                    </link-entity>
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Provinces>>("contacts(" + contactid + @")/bsd_contact_new_province", fetch);
            if (result == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }

            var data = result.value;

            if (data.Count < 3)
            {
                ShowMoreNhuCauDiaDiem = false;
            }
            else
            {
                ShowMoreNhuCauDiaDiem = true;
            }

            foreach (var x in result.value)
            {
                list_nhucauvediadiem.Add(x);
            }
        }

        public async Task<Boolean> Add_NhuCauDiaDiem(string id, Guid contactid)
        {
            string path = $"/contacts({contactid})/bsd_contact_new_province/$ref";

            IDictionary<string, string> data = new Dictionary<string, string>();
            data["@odata.id"] = $"{OrgConfig.ApiUrl}/new_provinces(" + id + ")";
            CrmApiResponse result = await CrmHelper.PostData(path, data);

            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                var mess = result.ErrorResponse?.error?.message ?? "Đã xảy ra lỗi. Vui lòng thử lại.";
                return false;
            }
        }

        public async Task<Boolean> Delete_NhuCauDiaDiem(string id, Guid contactid)
        {
            string Token = App.Current.Properties["Token"] as string;

            var request = $"{OrgConfig.ApiUrl}/contacts(" + contactid + ")/bsd_contact_new_province(" + id + ")/$ref";

            using (HttpClientHandler ClientHandler = new HttpClientHandler())
            using (HttpClient Client = new HttpClient(ClientHandler))
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                using (HttpRequestMessage RequestMessage = new HttpRequestMessage(new HttpMethod("DELETE"), request))
                {
                    using (HttpResponseMessage ResponseMessage = await Client.SendAsync(RequestMessage))
                    {
                        string result = await ResponseMessage.Content.ReadAsStringAsync();

                        if (ResponseMessage.StatusCode == HttpStatusCode.NoContent)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

        public async Task LoadAllProvincesForNhuCauVeDiaDiem()
        {
            if (morelookup_provincefornhucaudiadiem)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_provincefornhucaudiadiem + @"' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='new_province'>
                                    <attribute name='new_name' />
                                    <attribute name='createdon' />
                                    <attribute name='new_id' />
                                    <attribute name='bsd_country' />
                                    <attribute name='bsd_priority' />
                                    <attribute name='bsd_provincename' />
                                    <attribute name='new_provinceid' />
                                    <order attribute='bsd_priority' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='statecode' operator='eq' value='0' />
                                    </filter>
                                    <link-entity name='bsd_country' from='bsd_countryid' to='bsd_country' visible='false' link-type='outer' alias='bsd_countries'>
                                        <attribute name='bsd_name' alias='bsd_countries' />
                                    </link-entity>
                                </entity>
                            </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Provinces>>("new_provinces", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    morelookup_provincefornhucaudiadiem = false;
                    return;
                }
                foreach (var x in result.value)
                {
                    list_provinceefornhucaudiadiem.Add(x);
                }
            }

        }

        ///////// GENDER OPTION SET AREA
        /// /////////////////


        public void loadGender()
        {
            list_gender_optionset.Add(new OptionSet() { Val = ("1"), Label = "Nam", });
            list_gender_optionset.Add(new OptionSet() { Val = ("2"), Label = "Nữ", });
            list_gender_optionset.Add(new OptionSet() { Val = ("100000000"), Label = "Khác", });
        }

        public async Task<OptionSet> loadOneGender(string id)
        {
            this.singleGender = list_gender_optionset.FirstOrDefault(x => x.Val == id); ;
            return singleGender;
        }

        //////// LOCALIZATION OPTION SET AREA
        /// /////////////////


        public void loadLocalization()
        {
            list_localization_optionset.Add(new OptionSet() { Val = ("100000000"), Label = "Trong nước", });
            list_localization_optionset.Add(new OptionSet() { Val = ("100000001"), Label = "Nước ngoài", });
        }

        public async Task<OptionSet> loadOneLocalization(string id)
        {
            this.singleLocalization = list_localization_optionset.FirstOrDefault(x => x.Val == id); ;
            return singleLocalization;
        }


        ///////// ACCOUNT LOOKUP AREA
        /// ////////////

        public async Task loadAccountsLookup()
        {
            if (moreLooup_account)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_account + @"' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='account'>
                                        <attribute name='name' alias='Name'/>
                                        <attribute name='accountid' alias='Id'/>
                                    </entity>
                                </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("accounts", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    moreLooup_account = false;
                    return;
                }
                foreach (var x in result.value)
                {
                    x.Detail = "Account";
                    list_account_lookup.Add(x);
                }
            }
        }

        ///////// COUNTRY LOOKUP AREA
        /// ////////////

        public async Task LoadCountryForLookup()
        {
            if (morelookup_country)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_country + @"' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_country'>
                                    <attribute name='bsd_countryname' alias='Name'/>
                                    <attribute name='bsd_countryid' alias='Id'/>
                                    <attribute name='bsd_nameen' alias='Detail'/>
                                    <order attribute='bsd_countryname' descending='true' />
                                  </entity>
                                </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("bsd_countries", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    morelookup_country = false;
                    return;
                }
                foreach (var x in result.value)
                {
                    list_country_lookup.Add(x);
                }
            }
        }

        ///////// PROVINCE LOOKUP AREA
        /// ////////////

        public async Task loadProvincesForLookup(string countryId)
        {
            if (morelookup_province)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_province + @"' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='new_province'>
                                    <attribute name='bsd_provincename' alias='Name'/>
                                    <attribute name='new_provinceid' alias='Id'/>
                                    <attribute name='bsd_nameen' alias='Detail'/>
                                    <order attribute='bsd_provincename' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_country' operator='eq' value='" + countryId + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("new_provinces", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    morelookup_province = false;
                    return;
                }
                foreach (var x in result.value)
                {
                    list_province_lookup.Add(x);
                }
            }
        }

        public void resetProvince()
        {
            list_province_lookup = new ObservableCollection<LookUp>();

            pageLookup_province = 1;
            morelookup_province = true;
        }

        ///////// DISTRICT LOOKUP AREA
        /// ////////////

        public async Task loadDistrictForLookup(string provinceId)
        {
            if (morelookup_district)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_district + @"' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='new_district'>
                                <attribute name='new_name' alias='Name'/>
                                <attribute name='new_districtid' alias='Id'/>
                                <attribute name='bsd_nameen' alias='Detail'/>
                                <order attribute='new_name' descending='false' />
                                <filter type='and'>
                                  <condition attribute='new_province' operator='eq' value='" + provinceId + @"' />
                                </filter>
                              </entity>
                            </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("new_districts", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    morelookup_district = false;
                    return;
                }
                foreach (var x in result.value)
                {
                    list_district_lookup.Add(x);
                }
            }
        }
        public void resetDistrict()
        {
            list_district_lookup = new ObservableCollection<LookUp>();

            pageLookup_district = 1;
            morelookup_district = true;
        }

        ///////// CONTACT GROUP OPTION SET AREA
        /// /////////////////


        public void loadContacGroup()
        {
            list_contactgroup_optionset.Add(new OptionSet() { Val = ("100000000"), Label = "Ưu tiên (VIP)", });
            list_contactgroup_optionset.Add(new OptionSet() { Val = ("100000001"), Label = "An cư", });
            list_contactgroup_optionset.Add(new OptionSet() { Val = ("100000002"), Label = "Đầu tư", });
            list_contactgroup_optionset.Add(new OptionSet() { Val = ("100000003"), Label = "Đền bù", });
        }

        public async Task<OptionSet> loadOneContactGroup(string id)
        {
            this.singleContactgroup = list_contactgroup_optionset.FirstOrDefault(x => x.Val == id); ;
            return singleContactgroup;
        }

        ///////// DANH SACH DAT CHO AREA
        /// //////////////////
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
               // await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
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


        //////////// DANH SACH DAT COC AREA
        /// ////////////////////
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
               // await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
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

        ////////// DANH SACH HOP DONG AREA
        /// /////////////////////////

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
               // await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
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

        ///////////// CHAM SOC KHACH HANG
        /// ////////////////////////


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
                //await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
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

        //////// HINH ANH CMND SHAREPOINT AREA
        /// 
        //public async Task<GetTokenResponse> getSharePointToken()
        //{
        //    var client = BsdHttpClient.Instance();
        //    var request = new HttpRequestMessage(HttpMethod.Post, "https://login.microsoftonline.com/common/oauth2/token");
        //    var formContent = new FormUrlEncodedContent(new[]
        //        {
        //                new KeyValuePair<string, string>("client_id", "2ad88395-b77d-4561-9441-d0e40824f9bc"),
        //                new KeyValuePair<string, string>("username", OrgConfig.Username),
        //                new KeyValuePair<string, string>("password", OrgConfig.Password),
        //                new KeyValuePair<string, string>("grant_type", "password"),
        //                new KeyValuePair<string, string>("resource", OrgConfig.SharePointResource)
        //            });
        //    request.Content = formContent;
        //    var response = await client.SendAsync(request);
        //    var body = await response.Content.ReadAsStringAsync();
        //    GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
        //    return tokenData;
        //}

        public async Task GetImageCMND()
        {
            string token = (await CrmHelper.getSharePointToken()).access_token;
            var client = BsdHttpClient.Instance();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var front_request = new HttpRequestMessage(HttpMethod.Get, OrgConfig.SharePointResource
                            + "/sites/" + OrgConfig.SharePointSiteName + "/_api/web/GetFileByServerRelativeUrl('/sites/" + OrgConfig.SharePointSiteName + "/" + IMAGE_CMND_FOLDER + "/" + frontImage_name  + "')/$value");
            var front_result = await client.SendAsync(front_request);
            if(front_result.IsSuccessStatusCode)
            {
                singleContact.bsd_mattruoccmnd_base64 = Convert.ToBase64String(front_result.Content.ReadAsByteArrayAsync().Result);
            }

            var behind_request = new HttpRequestMessage(HttpMethod.Get, OrgConfig.SharePointResource
                            + "/sites/" + OrgConfig.SharePointSiteName + "/_api/web/GetFileByServerRelativeUrl('/sites/" + OrgConfig.SharePointSiteName + "/" + IMAGE_CMND_FOLDER + "/" + behindImage_name + "')/$value");
            var behind_result = await client.SendAsync(behind_request);
            if (behind_result.IsSuccessStatusCode)
            {
                singleContact.bsd_matsaucmnd_base64 = Convert.ToBase64String(behind_result.Content.ReadAsByteArrayAsync().Result);
            }
            //var front_result = ImageSource.FromUri(new Uri(OrgConfig.SharePointResource + "sites/Conasi/_layouts/15/download.aspx?SourceUrl=/sites/Conasi/" + IMAGE_CMND_FOLDER +  "/" + frontImage_name + "&access_token=" + token));
            //if(front_result != null)
            //{
            //    using (var memoryStream1 = new MemoryStream())
            //    {
            //        front_result.GetStream().CopyTo(memoryStream);
            //        file.Dispose();
            //        return memoryStream.ToArray();
            //    }
            //}

            //var behind_result = ImageSource.FromUri(new Uri(OrgConfig.SharePointResource + "sites/Conasi/_layouts/15/download.aspx?SourceUrl=/sites/Conasi/" + IMAGE_CMND_FOLDER + "/" + behindImage_name + "&access_token=" + token));
        }

        public async Task uploadImageCMND()
        {
            string token = (await CrmHelper.getSharePointToken()).access_token;

            using (var client = new HttpClient()) 
            { 
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                if(singleContact.bsd_mattruoccmnd_base64 != null)
                {
                    byte[] arrByteFront = Convert.FromBase64String(singleContact.bsd_mattruoccmnd_base64);

                    using (var response = client.PostAsync
                    (new Uri(OrgConfig.SharePointResource + "/sites/" + OrgConfig.SharePointSiteName + "/_api/web/GetFolderByServerRelativeUrl('/sites/" + OrgConfig.SharePointSiteName + "/" + IMAGE_CMND_FOLDER + "')/Files/add(url='" + frontImage_name + "',overwrite=true)")
                    , new StreamContent(new MemoryStream(arrByteFront))).Result)
                    {
                        if (!response.IsSuccessStatusCode) { await Application.Current.MainPage.DisplayAlert("Lỗi", "Cập nhật ảnh mặt trước CMND thất bại", "OK"); }
                    }
                }
                else
                {
                    await client.DeleteAsync(new Uri(OrgConfig.SharePointResource + "/sites/" + OrgConfig.SharePointSiteName + "/_api/web/GetFileByServerRelativeUrl('/sites/" + OrgConfig.SharePointSiteName + "/" + IMAGE_CMND_FOLDER + "/" + frontImage_name + "')"));
                }

                if (singleContact.bsd_matsaucmnd_base64 != null)
                {
                    byte[] arrByteBehind = Convert.FromBase64String(singleContact.bsd_matsaucmnd_base64);

                    using (var response = client.PostAsync
                    (new Uri(OrgConfig.SharePointResource + "/sites/" + OrgConfig.SharePointSiteName + "/_api/web/GetFolderByServerRelativeUrl('/sites/" + OrgConfig.SharePointSiteName + "/" + IMAGE_CMND_FOLDER + "')/Files/add(url='" + behindImage_name + "',overwrite=true)")
                    , new StreamContent(new MemoryStream(arrByteBehind))).Result)
                    {
                        if (!response.IsSuccessStatusCode) { await Application.Current.MainPage.DisplayAlert("Lỗi", "Cập nhật ảnh mặt sau CMND thất bại", "OK"); }
                    }
                }
                else
                {
                    await client.DeleteAsync(new Uri(OrgConfig.SharePointResource + "/sites/" + OrgConfig.SharePointSiteName + "/_api/web/GetFileByServerRelativeUrl('/sites/" + OrgConfig.SharePointSiteName + "/" + IMAGE_CMND_FOLDER + "/" + behindImage_name + "')"));
                }
            }
        }

        public async Task LoadAllProject()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_project'>
                                <attribute name='bsd_name' />
                                <attribute name='bsd_projectcode' />
                                <attribute name='bsd_landvalueofproject' />
                                <attribute name='bsd_esttopdate' />
                                <attribute name='bsd_acttopdate' />
                                <attribute name='bsd_projectid' />
                                <order attribute='bsd_name' descending='false' />
                                <filter type='and'>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ProjectList>>("bsd_projects", fetch);
            if (result == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }

            foreach (var x in result.value)
            {
                list_project_lookup.Add(x);
            }
        }

        public async Task checkdata_key(string passport, string identitycardnumber, string idcard, Guid contactid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='contact'>
                                <attribute name='fullname' />
                                <attribute name='statuscode' />
                                <attribute name='contactid' />
                                <order attribute='createdon' descending='true' />
                                  <filter type='or'>
                                    <filter type='and'>
                                      <condition attribute='bsd_passport' operator='eq' value='" + passport + @"' />
                                      <condition attribute='contactid' operator='eq' value='{" + contactid + @"}' />
                                    </filter>
                                    <filter type='and'>
                                      <condition attribute='bsd_identitycardnumber' operator='eq' value='" + identitycardnumber + @"' />
                                      <condition attribute='contactid' operator='ne' value='{" + contactid + @"}' />
                                    </filter>
                                    <filter type='and'>
                                      <condition attribute='bsd_idcard' operator='eq' value='" + idcard + @"' />
                                      <condition attribute='contactid' operator='ne' value='{" + contactid + @"}' />
                                    </filter>
                                  </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactFormModel>>("contacts", fetch);
            if(result.value != null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Trùng số CMND hoặc trùng passport", "OK");
                return;
            }
        }

        public async Task Load_DanhSachDuAn(string contactid)
        {
            string fetch = $@"<fetch version='1.0' count='3' page='{PageDuAnQuanTam}' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_project'>
                                <attribute name='bsd_name' />
                                <attribute name='bsd_projectcode' />
                                <attribute name='bsd_landvalueofproject' />
                                <attribute name='bsd_esttopdate' />
                                <attribute name='bsd_acttopdate' />
                                <attribute name='bsd_projectid' />
                                <order attribute='bsd_name' descending='false' />
                                <filter type='and'>
                                  <condition attribute='statecode' operator='eq' value='0' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ProjectList>>("contacts(" + contactid + @")/bsd_contact_bsd_project", fetch);
            if (result == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                return;
            }

            var data = result.value;

            if (data.Count < 3)
            {
                ShowMoreDuAnQuanTam = false;
            }
            else
            {
                ShowMoreDuAnQuanTam = true;
            }

            foreach (var x in result.value)
            {
                list_Duanquantam.Add(x);
            }
        }

        public async Task<Boolean> Add_DuAnQuanTam(string id, Guid Contactid)
        {
            string path = $"/contacts({Contactid})/bsd_contact_bsd_project/$ref";

            IDictionary<string, string> data = new Dictionary<string, string>();
            data["@odata.id"] = $"{OrgConfig.ApiUrl}/bsd_projects(" + id + ")";
            CrmApiResponse result = await CrmHelper.PostData(path, data);

            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                var mess = result.ErrorResponse?.error?.message ?? "Đã xảy ra lỗi. Vui lòng thử lại.";
                return false;
            }
        }

        public async Task<Boolean> Delete_DuAnQuanTam(string id, Guid Contactid)
        {
            string Token = App.Current.Properties["Token"] as string;

            var request = $"{OrgConfig.ApiUrl}/contacts(" + Contactid + ")/bsd_contact_bsd_project(" + id + ")/$ref";

            using (HttpClientHandler ClientHandler = new HttpClientHandler())
            using (HttpClient Client = new HttpClient(ClientHandler))
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                using (HttpRequestMessage RequestMessage = new HttpRequestMessage(new HttpMethod("DELETE"), request))
                {
                    using (HttpResponseMessage ResponseMessage = await Client.SendAsync(RequestMessage))
                    {
                        string result = await ResponseMessage.Content.ReadAsStringAsync();

                        if (ResponseMessage.StatusCode == HttpStatusCode.NoContent)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
        }

    }
}