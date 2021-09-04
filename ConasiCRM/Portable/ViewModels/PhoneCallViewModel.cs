using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class PhoneCallViewModel : FormViewModal
    {
        public PhoneCellModel _phoneCellModel;
        public PhoneCellModel PhoneCellModel { get => _phoneCellModel; set { _phoneCellModel = value; OnPropertyChanged(nameof(PhoneCellModel)); } }
        public List<OptionSet> LeadsLookUp { get; set; }
        public List<OptionSet> ContactsLookUp { get; set; }
        public List<OptionSet> AccountsLookUp { get; set; }
        public List<List<OptionSet>> AllsLookUp { get; set; }
        public List<string> Tabs { get; set; }

        private LookUp _customer;
        public LookUp Customer { get => _customer; set { _customer = value; OnPropertyChanged(nameof(Customer)); } }

        private LookUp _callFrom;
        public LookUp CallFrom { get => _callFrom; set { _callFrom = value; OnPropertyChanged(nameof(CallFrom)); } }

        private OptionSet _callF;
        public OptionSet CallF { get => _callF; set { _callF = value; OnPropertyChanged(nameof(CallF)); } }

        public List<string> _callTo;
        public List<string> CallTo { get => _callTo; set { _callTo = value; OnPropertyChanged(nameof(CallTo)); } }

        public PhoneCallViewModel()
        {
            ContactsLookUp = new List<OptionSet>();
            LeadsLookUp = new List<OptionSet>();
            AccountsLookUp = new List<OptionSet>();
            AllsLookUp = new List<List<OptionSet>>();
            Tabs = new List<string>();
            Tabs.Add("KH Tiềm Năng");
            Tabs.Add("KH Cá Nhân");
            Tabs.Add("KH Doanh Nghiệp");
        }

        public async Task LoadLeadsLookUp()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='lead'>
                                <attribute name='fullname' alias='Label' />
                                <attribute name='leadid' alias='Val' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                    <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("leads", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value;
            foreach (var item in data)
            {
                LeadsLookUp.Add(item);
            }
        }

        public async Task LoadContactsLookUp()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='contactid' alias='Val' />
                    <attribute name='fullname' alias='Label' />
                    <order attribute='fullname' descending='false' />                   
                    <filter type='and'>
                        <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                    </filter>
                  </entity>
                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("contacts", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value;
            foreach (var item in data)
            {
                ContactsLookUp.Add(item);
            }
        }

        public async Task LoadAccountsLookUp()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='account'>
                                <attribute name='name' alias='Label'/>
                                <attribute name='accountid' alias='Val'/>
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                    <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("accounts", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value;
            foreach (var item in data)
            {
                AccountsLookUp.Add(item);
            }
        }

        public async Task LoadAllLookUp()
        {
            if (LeadsLookUp.Count <= 0 && ContactsLookUp.Count <= 0 && AccountsLookUp.Count <= 0)
            {
                await LoadLeadsLookUp();
                await LoadContactsLookUp();
                await LoadAccountsLookUp();
            }
            AllsLookUp.Add(LeadsLookUp);
            AllsLookUp.Add(ContactsLookUp);
            AllsLookUp.Add(AccountsLookUp);
        }
    }
}
