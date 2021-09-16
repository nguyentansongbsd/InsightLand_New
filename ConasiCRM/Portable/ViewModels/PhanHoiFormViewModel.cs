using ConasiCRM.Portable.ViewModels;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using ConasiCRM.Portable.Helper;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections;
using ConasiCRM.Portable.Config;
using System.Net.Http.Headers;
using System.Net;

using System.Diagnostics;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.ViewModels
{
    public class PhanHoiFormViewModel : FormViewModal
    {
        private PhanHoiFormModel _singlePhanHoi;
        public PhanHoiFormModel singlePhanHoi { get => _singlePhanHoi; set { _singlePhanHoi = value; OnPropertyChanged(nameof(singlePhanHoi)); } }
        public OptionSet _singleOrigin;
        public OptionSet singleOrigin { get => _singleOrigin; set { _singleOrigin = value; OnPropertyChanged(nameof(singleOrigin)); } }
        private ListSubjectCase _singleListSubject;
        public ListSubjectCase singleListSubject { get => _singleListSubject; set { _singleListSubject = value; OnPropertyChanged(nameof(singleListSubject)); } }
        private ListUnitCase _singleListUnitCase;
        public ListUnitCase singleListUnitCase { get => _singleListUnitCase; set { _singleListUnitCase = value; OnPropertyChanged(nameof(singleListUnitCase)); } }
        private ListLienHeCase _singleListLienHeCase;
        public ListLienHeCase singleListLienHeCase { get => _singleListLienHeCase; set { _singleListLienHeCase = value; OnPropertyChanged(nameof(singleListLienHeCase)); } }

        public ObservableCollection<OptionSet> list_picker_caseorigincode { get; set; }
        public ObservableCollection<ListSubjectCase> list_lookup_Subject { get; set; }
        public ObservableCollection<PhanHoiFormModel> list_lookup { get; set; }
        public ObservableCollection<PhanHoiFormModel> list_account_lookup { get; set; }
        public ObservableCollection<PhanHoiFormModel> list_contact_lookup { get; set; }
        public ObservableCollection<ListUnitCase> list_unit_lookup { get; set; }
        public ObservableCollection<ListLienHeCase> list_lookup_lienhe { get; set; }
        public ObservableCollection<PhanHoiFormModel> list_lookup_status { get; set; }

        public int pageLookup_subject;
        public bool morelookup_subject;
        public int pageLookup_account;
        public bool morelookup_account;
        public int pageLookup_contact;
        public bool morelookup_contact;
        public int pageLookup_unit;
        public bool morelookup_unit;
        public int pageLookup_lienhe;
        public bool morelookup_lienhe;

        public PhanHoiFormViewModel()
        {
            list_account_lookup = new ObservableCollection<PhanHoiFormModel>();
            list_contact_lookup = new ObservableCollection<PhanHoiFormModel>();
            list_lookup_Subject = new ObservableCollection<ListSubjectCase>();
            list_unit_lookup = new ObservableCollection<ListUnitCase>();
            list_lookup_lienhe = new ObservableCollection<ListLienHeCase>();

            list_lookup_status = new ObservableCollection<PhanHoiFormModel>()
            {
                new PhanHoiFormModel { Name="In Progress", Id="1"},
                new PhanHoiFormModel { Name="On Hold", Id="2"},
                new PhanHoiFormModel { Name="Waiting for Details", Id="3"},
                new PhanHoiFormModel { Name="Researching", Id="4"},
                new PhanHoiFormModel { Name="Canceled", Id="6"},
                new PhanHoiFormModel { Name="Merged", Id="2000"}
             };

            list_picker_caseorigincode = new ObservableCollection<OptionSet>()
            {
                new OptionSet { Val="1",Label = "Phone"},
                new OptionSet { Val="2",Label = "Email"},
                new OptionSet { Val="3",Label = "Web"},
                new OptionSet { Val="2483",Label = "Facebook"},
                new OptionSet { Val="3986",Label = "Twitter"},
             };

            pageLookup_subject = 1;
            morelookup_subject = true;
            pageLookup_account = 1;
            morelookup_account = true;
            pageLookup_contact = 1;
            morelookup_contact = true;
            pageLookup_unit = 1;
            morelookup_unit = true;
            pageLookup_lienhe = 1;
            morelookup_lienhe = true;
        }

        public OptionSet getOrigin(string id)
        {
            singleOrigin = list_picker_caseorigincode.FirstOrDefault(x => x.Val == id);
            return singleOrigin;
        }

        public async Task LoadOnePhanHoi(Guid incidentid)
        {
            //Debug.WriteLine("abc5" + accountid);
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='incident'>
                                    <all-attributes/>
                                  <order attribute='title' descending='false' />
                                  <filter type='and'>
                                      <condition attribute='incidentid' operator='eq'  value='{" + incidentid + @"}' />
                                  </filter>
                                  <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='accounts'>
                                  <attribute name='bsd_name' alias='case_nameaccount'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='contacts'>
                                  <attribute name='bsd_fullname' alias='case_namecontact'/>
                                </link-entity>
                                <link-entity name='product' from='productid' to='productid' visible='false' link-type='outer' alias='products'>
                                  <attribute name='name' alias='productname'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='primarycontactid' visible='false' link-type='outer'>
                                  <attribute name='fullname' alias='contactname'/>
                                </link-entity>
                                <link-entity name='contract' from='contractid' to='contractid' visible='false' link-type='outer' alias='contracts'>
                                  <attribute name='title' alias='contractname'/>
                                </link-entity>
                                <link-entity name='subject' from='subjectid' to='subjectid' visible='false' link-type='outer' >
                                  <attribute name='title' alias='subjecttitle'/>
                                </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhanHoiFormModel>>("incidents", fetch);
            var tmp = result.value.FirstOrDefault();
            if (tmp == null)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                await Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
            }

            this.singlePhanHoi = tmp;
            //System.Diagnostics.Debugger.Break();
        }
        public async Task LoadListSubject()
        {
            if (morelookup_subject)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_subject + @"' output-format='xml-platform' mapping='logical' distinct='false'>    
                                <entity name='subject'>      
                                    <all-attributes/>
                                    <order attribute='createdon' descending='true' />
                                </entity>  
                            </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListSubjectCase>>("subjects", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    morelookup_subject = false;
                    return;
                }

                var data = result.value;
                foreach (var item in data)
                {
                    list_lookup_Subject.Add(new ListSubjectCase { Id = item.subjectid, Name = item.title });
                }
            }
        }

        public async Task LoadListAcc()
        {
            if (morelookup_account)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_account + @"' output-format='xml-platform' mapping='logical' distinct='false'>    
                                <entity name='account'>      
                                    <all-attributes/>
                                    <order attribute='createdon' descending='true' />
                                </entity>  
                            </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhanHoiFormModel>>("accounts", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    morelookup_account = false;
                    return;
                }

                var data = result.value;
                foreach (var item in data)
                {
                    list_account_lookup.Add(new PhanHoiFormModel { Id = item.accountid, Name = item.bsd_name });
                }
            }
        }

        public async Task LoadListContact()
        {
            if (morelookup_contact)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_contact+ @"' output-format='xml-platform' mapping='logical' distinct='false'>    
                                <entity name='contact'>      
                                    <all-attributes/>
                                    <order attribute='createdon' descending='true' />
                                </entity>  
                            </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhanHoiFormModel>>("contacts", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    morelookup_contact = false;
                    return;
                }

                var data = result.value;
                foreach (var item in data)
                {
                    list_contact_lookup.Add(new PhanHoiFormModel { Id = item.contactid, Name = item.bsd_fullname });
                }
            }
        }

        public async Task LoadListUnit()
        {
            if (morelookup_unit)
            {
                string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_unit + @"' output-format='xml-platform' mapping='logical' distinct='false'>    
                                <entity name='product'>      
                                    <all-attributes/>
                                    <order attribute='createdon' descending='true' />
                                </entity>  
                            </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListUnitCase>>("products", fetch);
                if (result == null)
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                    return;
                }
                if (result.value.Count == 0)
                {
                    morelookup_unit = false;
                    return;
                }

                var data = result.value;
                foreach (var item in data)
                {
                    list_unit_lookup.Add(new ListUnitCase { Id = item.productid, Name = item.name });
                }
            }
        }

        public async Task LoadListLienHe(string _customerid_value)
        {
            if (morelookup_lienhe)
            {
                if (_customerid_value != null)
                {
                    string fetch = @"<fetch version='1.0' count='30' page='" + pageLookup_lienhe + @"' output-format='xml-platform' mapping='logical' distinct='false'>    
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
                                            <order attribute='createdon' descending='true' />
                                            <filter type='and'>
                                              <condition attribute='parentcustomerid' operator='eq' uitype='account' value='{" + _customerid_value + @"}' />
                                            </filter>
                                          </entity>
                                        </fetch>";
                    var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListLienHeCase>>("contacts", fetch);
                    if (result == null)
                    {
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
                        return;
                    }
                    if (result.value.Count == 0)
                    {
                        morelookup_lienhe = false;
                        return;
                    }

                    var data = result.value;
                    foreach (var item in data)
                    {
                        list_lookup_lienhe.Add(new ListLienHeCase { Id = item.contactid, Name = item.bsd_fullname });
                    }
                }
            }
        }
    }
}

