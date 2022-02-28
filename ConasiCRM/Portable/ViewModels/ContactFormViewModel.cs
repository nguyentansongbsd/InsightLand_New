using Azure.Identity;
using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class ContactFormViewModel : BaseViewModel
    {
        private ContactFormModel _singleContact;
        public ContactFormModel singleContact { get { return _singleContact; } set { _singleContact = value; OnPropertyChanged(nameof(singleContact)); } }

        private OptionSet _singleGender;
        public OptionSet singleGender { get => _singleGender; set { _singleGender = value; OnPropertyChanged(nameof(singleGender)); } }

        private OptionSet _singleLocalization;
        public OptionSet singleLocalization { get => _singleLocalization; set { _singleLocalization = value; OnPropertyChanged(nameof(singleLocalization)); } }

        public ObservableCollection<LookUp> list_account_lookup { get; set; }

        private LookUp _account;
        public LookUp Account { get => _account; set { _account = value; OnPropertyChanged(nameof(Account)); } }

        public ObservableCollection<LookUp> list_lookup { get; set; }

        

        public ObservableCollection<OptionSet> GenderOptions { get; set; }
        public ObservableCollection<OptionSet> LocalizationOptions { get; set; }

        private string IMAGE_CMND_FOLDER = "Contact_CMND";
        private string frontImage_name;
        private string behindImage_name;

        private string checkCMND;

        private AddressModel _contactAddress;
        public AddressModel ContactAddress { get => _contactAddress; set { _contactAddress = value; OnPropertyChanged(nameof(ContactAddress)); } }

        private AddressModel _permanentAddress;
        public AddressModel PermanentAddress { get => _permanentAddress; set { _permanentAddress = value; OnPropertyChanged(nameof(PermanentAddress)); } }

        public ContactFormViewModel()
        {
            singleContact = new ContactFormModel();
            
            list_lookup = new ObservableCollection<LookUp>();
            list_account_lookup = new ObservableCollection<LookUp>();
            GenderOptions = new ObservableCollection<OptionSet>();
            LocalizationOptions = new ObservableCollection<OptionSet>();
        }

        public async Task LoadOneContact(String id)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='contact'>
                                <attribute name='fullname' />
                                <attribute name='statuscode' />
                                <attribute name='ownerid' />
                                <attribute name='mobilephone' />
                                <attribute name='jobtitle' />
                                <attribute name='bsd_identitycardnumber' />
                                <attribute name='gendercode' />
                                <attribute name='emailaddress1' />
                                <attribute name='createdon' />
                                <attribute name='birthdate' />
                                <attribute name='address1_composite' />
                                <attribute name='bsd_fullname' />
                                <attribute name='contactid' />
                                <attribute name='telephone1' />
                                <attribute name='parentcustomerid' />
                                <attribute name='bsd_province' alias='_bsd_province_value'/>
                                <attribute name='bsd_placeofissuepassport' />
                                <attribute name='bsd_placeofissue' />
                                <attribute name='bsd_permanentprovince' alias='_bsd_permanentprovince_value'/>
                                <attribute name='bsd_permanentdistrict' alias='_bsd_permanentdistrict_value'/>
                                <attribute name='bsd_permanentcountry' alias='_bsd_permanentcountry_value'/>
                                <attribute name='bsd_permanentaddress1' />
                                <attribute name='bsd_permanentaddress' />
                                <attribute name='bsd_passport' />
                                <attribute name='bsd_localization' />
                                <attribute name='bsd_jobtitlevn' />
                                <attribute name='bsd_issuedonpassport' />
                                <attribute name='bsd_housenumberstreet' />
                                <attribute name='bsd_district' alias='_bsd_district_value'/>
                                <attribute name='bsd_dategrant' />
                                <attribute name='bsd_country' alias='_bsd_country_value'/>
                                <attribute name='bsd_postalcode' />
                                <attribute name='bsd_etag_behind' />
                                <attribute name='bsd_etag_front' />
                                <attribute name='bsd_contactaddress' />
                                    <link-entity name='account' from='accountid' to='parentcustomerid' visible='false' link-type='outer' alias='aa'>
                                          <attribute name='accountid' alias='_parentcustomerid_value' />
                                          <attribute name='bsd_name' alias='parentcustomerid_label' />
                                    </link-entity>
                                    <link-entity name='bsd_country' from='bsd_countryid' to='bsd_country' visible='false' link-type='outer'>
                                        <attribute name='bsd_countryname'  alias='bsd_country_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_country_label_en'/>
                                    </link-entity>
                                    <link-entity name='new_province' from='new_provinceid' to='bsd_province' visible='false' link-type='outer'>
                                        <attribute name='bsd_provincename'  alias='bsd_province_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_province_label_en'/>
                                    </link-entity>
                                    <link-entity name='new_district' from='new_districtid' to='bsd_district' visible='false' link-type='outer'>
                                        <attribute name='new_name'  alias='bsd_district_label'/>  
                                        <attribute name='bsd_nameen'  alias='bsd_district_label_en'/>
                                    </link-entity>
                                    <link-entity name='bsd_country' from='bsd_countryid' to='bsd_permanentcountry' visible='false' link-type='outer'>
                                        <attribute name='bsd_countryname'  alias='bsd_permanentcountry_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_permanentcountry_label_en'/>
                                    </link-entity>
                                    <link-entity name='new_province' from='new_provinceid' to='bsd_permanentprovince' visible='false' link-type='outer'>
                                        <attribute name='bsd_provincename'  alias='bsd_permanentprovince_label'/>                                        
                                        <attribute name='bsd_nameen'  alias='bsd_permanentprovince_label_en'/>
                                    </link-entity>
                                    <link-entity name='new_district' from='new_districtid' to='bsd_permanentdistrict' visible='false' link-type='outer'>
                                        <attribute name='new_name'  alias='bsd_permanentdistrict_label'/>
                                        <attribute name='bsd_nameen'  alias='bsd_permanentdistrict_label_en'/>
                                    </link-entity>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                     <condition attribute='contactid' operator='eq' value='" + id + @"' />
                                    </filter>
                              </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactFormModel>>("contacts", fetch);
            if (result == null || result.value == null)
            {
                return;
            }

            var tmp = result.value.FirstOrDefault();
            this.singleContact = tmp;

            checkCMND = tmp.bsd_identitycardnumber;
            ContactAddress = new AddressModel
            {
                country_id = singleContact._bsd_country_value,
                country_name = !string.IsNullOrWhiteSpace(singleContact.bsd_country_label_en) && UserLogged.Language == "en" ? singleContact.bsd_country_label_en : singleContact.bsd_country_label,
                province_id = singleContact._bsd_province_value,
                province_name = !string.IsNullOrWhiteSpace(singleContact.bsd_province_label_en) && UserLogged.Language == "en" ? singleContact.bsd_province_label_en : singleContact.bsd_province_label,
                district_id = singleContact._bsd_district_value,
                district_name = !string.IsNullOrWhiteSpace(singleContact.bsd_district_label_en) && UserLogged.Language == "en" ? singleContact.bsd_district_label_en : singleContact.bsd_district_label,
                address = singleContact.bsd_contactaddress,
                lineaddress = singleContact.bsd_housenumberstreet,
                address_en = singleContact.bsd_diachi,
                lineaddress_en = singleContact.bsd_housenumber,
            };
            PermanentAddress = new AddressModel
            {
                country_id = singleContact._bsd_permanentcountry_value,
                country_name = !string.IsNullOrWhiteSpace(singleContact.bsd_permanentcountry_label_en) && UserLogged.Language == "en" ? singleContact.bsd_permanentcountry_label_en : singleContact.bsd_permanentcountry_label,
                province_id = singleContact._bsd_permanentprovince_value,
                province_name = !string.IsNullOrWhiteSpace(singleContact.bsd_permanentprovince_label_en) && UserLogged.Language == "en" ? singleContact.bsd_permanentprovince_label_en : singleContact.bsd_permanentprovince_label,
                district_id = singleContact._bsd_permanentdistrict_value,
                district_name = !string.IsNullOrWhiteSpace(singleContact.bsd_permanentdistrict_label_en) && UserLogged.Language == "en" ? singleContact.bsd_permanentdistrict_label_en : singleContact.bsd_permanentdistrict_label,
                address = singleContact.bsd_permanentaddress1,
                lineaddress = singleContact.bsd_permanentaddress,
                address_en = singleContact.bsd_diachithuongtru,
                lineaddress_en = singleContact.bsd_permanenthousenumber,
            };
        }

        public async Task<Boolean> updateContact(ContactFormModel contact)
        {
            string path = "/contacts(" + contact.contactid + ")";
            var content = await this.getContent(contact);

            CrmApiResponse result = await CrmHelper.PatchData(path, content);

            if (result.IsSuccess)
            {
               // await UpLoadCMND();
                return true;
            }
            else
            {
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
               // await UpLoadCMND();
                return contact.contactid;
            }
            else
            {
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
            data["contactid"] = contact.contactid;
            data["lastname"] = contact.bsd_fullname;
            data["firstname"] = "";
            data["bsd_fullname"] = contact.bsd_fullname;
            data["emailaddress1"] = contact.emailaddress1;
            data["birthdate"] = contact.birthdate.HasValue ? (DateTime.Parse(contact.birthdate.ToString()).ToUniversalTime()).ToString("yyyy-MM-dd") : null;
            data["mobilephone"] = contact.mobilephone;
            data["gendercode"] = contact.gendercode;
            if (checkCMND != contact.bsd_identitycardnumber)
            {
                data["bsd_identitycardnumber"] = contact.bsd_identitycardnumber;
            }
            data["bsd_localization"] = contact.bsd_localization;
            data["bsd_dategrant"] = contact.bsd_dategrant.HasValue ? (DateTime.Parse(contact.bsd_dategrant.ToString()).ToLocalTime()).ToString("yyy-MM-dd") : null;
            data["bsd_placeofissue"] = contact.bsd_placeofissue;
            data["bsd_passport"] = contact.bsd_passport;
            data["bsd_issuedonpassport"] = contact.bsd_issuedonpassport.HasValue ? (DateTime.Parse(contact.bsd_issuedonpassport.ToString()).ToLocalTime()).ToString("yyyy-MM-dd") : null;
            data["bsd_placeofissuepassport"] = contact.bsd_placeofissuepassport;
            data["bsd_jobtitlevn"] = contact.bsd_jobtitlevn;
            data["telephone1"] = contact.telephone1;

          //  data["bsd_housenumberstreet"] = contact.bsd_housenumberstreet;
          //  data["bsd_contactaddress"] = contact.bsd_contactaddress;
          //  data["bsd_diachi"] = contact.bsd_diachi;
          ////  data["bsd_postalcode"] = contact.bsd_postalcode;
          //  data["bsd_housenumber"] = contact.bsd_housenumberstreet;

          //  data["bsd_permanentaddress1"] = contact.bsd_permanentaddress1;
          //  data["bsd_diachithuongtru"] = contact.bsd_diachithuongtru;
          //  data["bsd_permanenthousenumber"] = contact.bsd_permanentaddress;
          //  data["bsd_permanentaddress"] = contact.bsd_permanentaddress; //bsd_permanentaddress

            if (contact._parentcustomerid_value == null)
            {
                await DeletLookup("parentcustomerid_account", contact.contactid);
            }
            else
            {
                data["parentcustomerid_account@odata.bind"] = "/accounts(" + contact._parentcustomerid_value + ")"; /////Lookup Field

            }           

            if (ContactAddress != null && !string.IsNullOrWhiteSpace(ContactAddress.lineaddress))
            {
                data["bsd_contactaddress"] = ContactAddress.address;
                data["bsd_housenumberstreet"] = ContactAddress.lineaddress;

                data["bsd_housenumber"] = ContactAddress.lineaddress_en;
                data["bsd_diachi"] = ContactAddress.address_en;

                if (ContactAddress.country_id != Guid.Empty)
                    data["bsd_country@odata.bind"] = "/bsd_countries(" + ContactAddress.country_id + ")";
                else
                    await DeletLookup("bsd_country", contact.contactid);

                if (ContactAddress.province_id != Guid.Empty)
                    data["bsd_province@odata.bind"] = "/new_provinces(" + ContactAddress.province_id + ")";
                else
                    await DeletLookup("bsd_province", contact.contactid);

                if (ContactAddress.district_id != Guid.Empty)
                    data["bsd_district@odata.bind"] = "/new_districts(" + ContactAddress.district_id + ")";
                else
                    await DeletLookup("bsd_district", contact.contactid);

            }

            if (PermanentAddress != null && !string.IsNullOrWhiteSpace(PermanentAddress.lineaddress))
            {
                data["bsd_permanentaddress1"] = PermanentAddress.address;
                data["bsd_permanentaddress"] = PermanentAddress.lineaddress;

                data["bsd_permanenthousenumber"] = PermanentAddress.lineaddress_en;
                data["bsd_diachithuongtru"] = PermanentAddress.address_en;

                if (PermanentAddress.country_id != Guid.Empty)
                    data["bsd_permanentcountry@odata.bind"] = "/bsd_countries(" + PermanentAddress.country_id + ")";
                else
                    await DeletLookup("bsd_permanentcountry", contact.contactid);

                if (PermanentAddress.province_id != Guid.Empty)
                    data["bsd_permanentprovince@odata.bind"] = "/new_provinces(" + PermanentAddress.province_id + ")";
                else
                    await DeletLookup("bsd_permanentprovince", contact.contactid);

                if (PermanentAddress.district_id != Guid.Empty)
                    data["bsd_permanentdistrict@odata.bind"] = "/new_districts(" + PermanentAddress.district_id + ")";
                else
                    await DeletLookup("bsd_permanentdistrict", contact.contactid);

            }

            if (UserLogged.Id != null)
            {
                data["bsd_employee@odata.bind"] = "/bsd_employees(" + UserLogged.Id + ")";
            }
            if (UserLogged.ManagerId != Guid.Empty)
            {
                data["ownerid@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            }

            return data;
        }

        public async Task LoadAccountsLookup()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='account'>
                                        <attribute name='name' alias='Name'/>
                                        <attribute name='accountid' alias='Id'/>
                                    </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>("accounts", fetch);
            if (result == null)
            {
                return;
            }
            foreach (var x in result.value)
            {
                x.Detail = "Account";
                list_account_lookup.Add(x);
            }
        }

        public async Task GetImageCMND()
        {
            if (!string.IsNullOrWhiteSpace(singleContact.bsd_etag_behind) || !string.IsNullOrWhiteSpace(singleContact.bsd_etag_front))
            if (!string.IsNullOrWhiteSpace(singleContact.bsd_etag_behind))
            {
                var urlVideo = await CrmHelper.RetrieveImagesSharePoint<RetrieveMultipleApiResponse<GraphThumbnailsUrlModel>>($"{Config.OrgConfig.SharePointContact_CMNDId}/items/{singleContact.bsd_etag_behind}/driveItem/thumbnails");
                singleContact.bsd_matsaucmnd_source = urlVideo.value.SingleOrDefault().large.url;
            }
            if (!string.IsNullOrWhiteSpace(singleContact.bsd_etag_front))
            {
                var urlVideo = await CrmHelper.RetrieveImagesSharePoint<RetrieveMultipleApiResponse<GraphThumbnailsUrlModel>>($"{Config.OrgConfig.SharePointContact_CMNDId}/items/{singleContact.bsd_etag_front}/driveItem/thumbnails");
                singleContact.bsd_mattruoccmnd_source = urlVideo.value.SingleOrDefault().large.url;
            }
        }
        public async Task<bool> UpLoadCMNDFront()
        {
            var frontImage_name =  this.singleContact.contactid.ToString() + "_front.jpg";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (singleContact.bsd_mattruoccmnd_base64 != null)
                {
                    var url = "https://prod-12.southeastasia.logic.azure.com:443/workflows/ead81b97fd1c4e3c800f2614089508ef/triggers/manual/paths/invoke?api-version=2016-06-01&sp=/triggers/manual/run&sv=1.0&sig=e2vS46jv2ziDZCbhy32WbBLBjS5K5kEAOWs8KjXcG5M";
                    byte[] arrByteFront = Convert.FromBase64String(singleContact.bsd_mattruoccmnd_base64);

                    ImageCMNDModel data = new ImageCMNDModel {name= frontImage_name, value = arrByteFront };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var response = client.PostAsync(url, content))
                    {
                       var body = await response.Result.Content.ReadAsStringAsync();
                        singleContact.bsd_etag_front = body;
                        string a = response.Result.Content.ToString();
                        if (response.Result != null)
                            return true;
                        else
                            return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        public async Task<bool> UpLoadCMNDBehind()
        {
            behindImage_name = this.singleContact.contactid.ToString() + "_behind.jpg";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (singleContact.bsd_matsaucmnd_base64 != null)
                {
                    var url = "https://prod-12.southeastasia.logic.azure.com:443/workflows/ead81b97fd1c4e3c800f2614089508ef/triggers/manual/paths/invoke?api-version=2016-06-01&sp=/triggers/manual/run&sv=1.0&sig=e2vS46jv2ziDZCbhy32WbBLBjS5K5kEAOWs8KjXcG5M";
                    byte[] arrByteBehind = Convert.FromBase64String(singleContact.bsd_matsaucmnd_base64);

                    ImageCMNDModel data = new ImageCMNDModel { name = frontImage_name, value = arrByteBehind };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var response = client.PostAsync(url, content))
                    {
                        if (response.Result != null)
                            return true;
                        else
                            return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        public async Task UpLoadCMND()
        {
            var frontImage_name = this.singleContact.contactid.ToString() + "_front.jpg";
            var behindImage_name = this.singleContact.contactid.ToString() + "_behind.jpg";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var url = "https://prod-12.southeastasia.logic.azure.com:443/workflows/ead81b97fd1c4e3c800f2614089508ef/triggers/manual/paths/invoke?api-version=2016-06-01&sp=/triggers/manual/run&sv=1.0&sig=e2vS46jv2ziDZCbhy32WbBLBjS5K5kEAOWs8KjXcG5M";
                if (singleContact.bsd_mattruoccmnd_base64 != null)
                {
                    byte[] arrByteFront = Convert.FromBase64String(singleContact.bsd_mattruoccmnd_base64);
                    ImageCMNDModel data = new ImageCMNDModel { name = frontImage_name, value = arrByteFront };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var response = client.PostAsync(url, content))
                    {
                        if (response.Result != null)
                        {
                            var body = await response.Result.Content.ReadAsStringAsync();
                            singleContact.bsd_etag_front = body;
                            await UpdatContactEtag();
                        }
                    }
                }
                if (singleContact.bsd_matsaucmnd_base64 != null)
                {
                    byte[] arrByteBehind = Convert.FromBase64String(singleContact.bsd_matsaucmnd_base64);
                    ImageCMNDModel data = new ImageCMNDModel { name = behindImage_name, value = arrByteBehind };
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var response = client.PostAsync(url, content))
                    {
                        if (response.Result != null)
                        {
                            var body = await response.Result.Content.ReadAsStringAsync();
                            singleContact.bsd_etag_behind = body;
                            await UpdatContactEtag();
                        }
                    }
                }
            }
        }

        public async Task<bool> CheckCMND(string identitycardnumber, string contactid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='contact'>
                                    <attribute name='fullname' />
                                    <filter type='and'>
                                        <condition attribute='bsd_identitycardnumber' operator='eq' value='" + identitycardnumber + @"' />
                                        <condition attribute='contactid' operator='ne' value='" + contactid + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactFormModel>>("contacts", fetch);
            if (result != null && result.value.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> CheckPassport(string bsd_passport, string contactid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='contact'>
                                    <attribute name='fullname' />
                                    <filter type='and'>
                                        <condition attribute='bsd_passport' operator='eq' value='" + bsd_passport + @"' />
                                        <condition attribute='contactid' operator='ne' value='" + contactid + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactFormModel>>("contacts", fetch);
            if (result != null && result.value.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> CheckCCCD(string idcard, string contactid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='contact'>
                                    <attribute name='fullname' />
                                    <filter type='and'>
                                        <condition attribute='bsd_idcard' operator='eq' value='" + idcard + @"' />
                                        <condition attribute='contactid' operator='ne' value='" + contactid + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactFormModel>>("contacts", fetch);
            if (result != null && result.value.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> CheckEmail(string email, string contactid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='contact'>
                                    <attribute name='fullname' />
                                    <filter type='and'>
                                        <condition attribute='emailaddress1' operator='eq' value='" + email + @"' />
                                        <condition attribute='contactid' operator='ne' value='" + contactid + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContactFormModel>>("contacts", fetch);
            if (result != null && result.value.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public async Task<bool> UpdatContactEtag()
        {
            if (singleContact.contactid == Guid.Empty) return false;
            string path = "/contacts(" + singleContact.contactid + ")";

            IDictionary<string, object> data = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(singleContact.bsd_etag_front))
                data["bsd_etag_front"] = singleContact.bsd_etag_front;
            if (!string.IsNullOrWhiteSpace(singleContact.bsd_etag_behind))
                data["bsd_etag_behind"] = singleContact.bsd_etag_behind;
            CrmApiResponse result = await CrmHelper.PatchData(path, data);
            if (result.IsSuccess)
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