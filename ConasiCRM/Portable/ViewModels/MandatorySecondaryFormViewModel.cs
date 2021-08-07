using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class MandatorySecondaryFormViewModel : FormLookupViewModel
    {
        private MandatorySecondaryModel _mandatorySecondary;
        public MandatorySecondaryModel mandatorySecondary { get => _mandatorySecondary; set { _mandatorySecondary = value; OnPropertyChanged(nameof(mandatorySecondary)); } }
        public MandatorySecondaryFormViewModel()
        {
            mandatorySecondary = new MandatorySecondaryModel();
        }
        public async Task GetOneAccountById( string accountid)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='account'>
                                <attribute name='bsd_name' />                                
                                <attribute name='accountid' />
                                <attribute name='bsd_businesstypesys' alias='bsd_businesstype' />
                                <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='accountid' operator='eq' value='" + accountid + @"' />
                                    </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<AccountFormModel>>("accounts", fetch);
            if (result == null)
                return;
            var tmp = result.value.FirstOrDefault();
            if (tmp == null)
            {
                return;
            }
            mandatorySecondary.bsd_developeraccount = tmp.bsd_name;
            mandatorySecondary._bsd_developeraccount_value = tmp.accountid.ToString();
        }

        private async void GetContentMandatorySecondary()
        {
        //    IDictionary<string, object> data = new Dictionary<string, object>();
        //    data["bsd_fullname"] = mandatorySecondary.bsd_contact_name;
        //    data["statuscode"] = mandatorySecondary.statuscode;
        //    data["bsd_developeraccount"] = mandatorySecondary.bsd_developeraccount;
        //    data["mobilephone"] = contact.mobilephone;
        //    data["gendercode"] = contact.gendercode;
        //    data["bsd_localization"] = contact.bsd_localization;
        //    data["bsd_dategrant"] = contact.bsd_dategrant.HasValue ? (DateTime.Parse(contact.bsd_dategrant.ToString()).ToLocalTime()).ToString("yyy-MM-dd") : null;
        //    data["bsd_placeofissue"] = contact.bsd_placeofissue;
        //    data["bsd_passport"] = contact.bsd_passport;
        //    data["bsd_issuedonpassport"] = contact.bsd_issuedonpassport.HasValue ? (DateTime.Parse(contact.bsd_issuedonpassport.ToString()).ToLocalTime()).ToString("yyyy-MM-dd") : null;
        //    data["bsd_placeofissuepassport"] = contact.bsd_placeofissuepassport;
        //    data["bsd_jobtitlevn"] = contact.bsd_jobtitlevn;
        //    data["telephone1"] = contact.telephone1;
        //    data["bsd_housenumberstreet"] = contact.bsd_housenumberstreet;
        //    data["bsd_permanentaddress"] = contact.bsd_permanentaddress;
        //    data["bsd_contactaddress"] = contact.bsd_contactaddress;
        //    data["bsd_permanentaddress1"] = contact.bsd_permanentaddress1;
        //    data["bsd_postalcode"] = contact.bsd_postalcode;

        //    if (mandatorySecondary.bsd_contactid == null)
        //    {
        //        await DeletLookup("bsd_contact", mandatorySecondary.bsd_contactid);
        //    }
        //    else
        //    {
        //        data["bsd_contact@odata.bind"] = "/contacts(" + mandatorySecondary.bsd_contactid + ")"; /////Lookup Field
        //    }

        //    return data;
        //}
        //public async Task<Boolean> DeletLookup(string fieldName, Guid contactId)
        //{
        //    var result = await CrmHelper.SetNullLookupField("bsd_mandatorysecondarys", contactId, fieldName);
        //    return result.IsSuccess;
        }
    }
}
