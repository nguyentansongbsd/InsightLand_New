using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using ConasiCRM.Portable.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telerik.XamarinForms.Common;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class QueuListViewModel : ListViewBaseViewModel2<QueuesModel>
    {
        public ObservableCollection<OptionSet> FiltersStatus { get; set; } = new ObservableCollection<OptionSet>();

        public OptionSet _filterStatus;
        public OptionSet FilterStatus { get => _filterStatus; set { _filterStatus = value; OnPropertyChanged(nameof(FilterStatus)); } }
        public ObservableCollection<OptionSet> FiltersProject { get; set; } = new ObservableCollection<OptionSet>();

        public OptionSet _filterProject;
        public OptionSet FilterProject { get => _filterProject; set { _filterProject = value; OnPropertyChanged(nameof(FilterProject)); } }
        public ObservableCollection<OptionSet> FiltersUnit { get; set; } = new ObservableCollection<OptionSet>();

        public OptionSet _filterUnit;
        public OptionSet FilterUnit { get => _filter; set { _filter = value; OnPropertyChanged(nameof(FilterUnit)); } }
        public ObservableCollection<OptionSet> FilterList { get; set; } = new ObservableCollection<OptionSet>();

        public OptionSet _filter;
        public OptionSet Filter { get => _filter; set { _filter = value; OnPropertyChanged(nameof(Filter)); } }
        public string Keyword { get; set; }
        public ICommand PhoneCommand { get; }
        public QueuListViewModel()
        {
            PhoneCommand = new Command<string>(PhoneCommandAsync);
            PreLoadData = new Command(() =>
            {
                string filter = string.Empty;
                if (Filter != null)
                {
                    if (Filter.Val == "1")
                        filter = $@"<order attribute='statuscode' descending='true' />";
                    else if (Filter.Val == "2")
                        filter = $@"<order attribute='statuscode' descending='false' />";
                    else if (Filter.Val == "3")
                        filter = $@"<order attribute='bsd_project' descending='true' />";
                    else if (Filter.Val == "4")
                        filter = $@"<order attribute='bsd_project' descending='false' />";
                    else if (Filter.Val == "5")
                        filter = $@"<order attribute='bsd_units' descending='true' />";
                    else if (Filter.Val == "6")
                        filter = $@"<order attribute='bsd_units' descending='false' />";
                    else
                        filter = $@"<order attribute='statuscode' descending='true' />";
                }
                else
                {
                    filter = $@"<order attribute='statuscode' descending='true' />";
                }

                EntityName = "opportunities";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='opportunity'>
                        <attribute name='name'/>
                        <attribute name='statuscode' />
                        <attribute name='bsd_project' />
                        <attribute name='opportunityid' />
                        <attribute name='bsd_queuenumber' />
                        <attribute name='bsd_queuingexpired' />
                        <attribute name='createdon' />
                        <filter type='and'>                          
                            <filter type='or'>
                                <condition attribute='name' operator='like' value='%25{Keyword}%25' />
                                <condition attribute='customeridname' operator='like' value='%25{Keyword}%25' />
                                <condition attribute='bsd_queuenumber' operator='like' value='%25{Keyword}%25' />
                                <condition attribute='bsd_unitsname' operator='like' value='%25{Keyword}%25' /> 
                            </filter>
                          <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}'/>
                        </filter>
                        <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer'>
                           <attribute name='fullname'  alias='contact_name'/>
                        </link-entity>
                        <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer'>
                           <attribute name='name'  alias='account_name'/>
                        </link-entity>
                        " + filter + @"
                      </entity>
                    </fetch>";
            });
        }

        private async void PhoneCommandAsync(string phone)
        {
            var checkVadate = PhoneNumberFormatVNHelper.CheckValidate(phone);
            if (checkVadate == true)
            {
                await Launcher.OpenAsync($"tel:{phone}");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Lỗi", "Không có số điện thoại", "OK");
            }
        }
    }
}
