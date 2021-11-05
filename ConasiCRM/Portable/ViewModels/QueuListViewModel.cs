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

        public List<string> _filterStatus;
        public List<string> FilterStatus { get => _filterStatus; set { _filterStatus = value; OnPropertyChanged(nameof(FilterStatus)); } }
        public ObservableCollection<OptionSet> FiltersProject { get; set; } = new ObservableCollection<OptionSet>();

        public OptionSet _filterProject;
        public OptionSet FilterProject { get => _filterProject; set { _filterProject = value; OnPropertyChanged(nameof(FilterProject)); } }
        public ObservableCollection<OptionSet> FiltersUnit { get; set; } = new ObservableCollection<OptionSet>();

        public List<string> _filterUnit;
        public List<string> FilterUnit { get => _filterUnit; set { _filterUnit = value; OnPropertyChanged(nameof(FilterUnit)); } }

        public string Keyword { get; set; }
        public ICommand PhoneCommand { get; }
        public QueuListViewModel()
        {
            PhoneCommand = new Command<string>(PhoneCommandAsync);
            PreLoadData = new Command(() =>
            {
                string project = null;
                string status = null;
                string units = null;
                if (FilterStatus != null && FilterStatus.Count > 0)
                {
                    if (string.IsNullOrWhiteSpace(FilterStatus.Where(x => x == "-1").FirstOrDefault()))
                    {
                        string sts = string.Empty;
                        foreach (var item in FilterStatus)
                        {
                            sts += $@"<value>{item}</value>";
                        }
                        status = @"<condition attribute='statuscode' operator='in'>" + sts + "</condition>";
                    }
                    else
                    {
                        status = null;
                    }
                }
                else
                {
                    status = null;
                }
                if (FilterProject != null && FilterProject.Val != "-1")
                {
                    project = $@"<condition attribute='bsd_project' operator='eq' value='{FilterProject.Val}' />";
                }
                else
                {
                    project = null;
                }
                if (FilterUnit != null && FilterUnit.Count > 0)
                {
                    if (string.IsNullOrWhiteSpace(FilterUnit.Where(x => x == "-1").FirstOrDefault()))
                    {
                        string unit = string.Empty;
                        foreach (var item in FilterUnit)
                        {
                            unit += $@"<value>{item}</value>";
                        }
                        units = @"<condition attribute='bsd_units' operator='in'>" + unit + "</condition>";
                    }
                    else
                    {
                        units = null;
                    }
                }
                else
                {
                    units = null;
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
                          "+ status + @"
                            " + project + @"
                            " + units + @"
                        </filter>
                        <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer'>
                           <attribute name='fullname'  alias='contact_name'/>
                        </link-entity>
                        <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer'>
                           <attribute name='name'  alias='account_name'/>
                        </link-entity>
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

        public void LoadStatus()
        {
            if (FiltersStatus != null && FiltersStatus.Count == 0)
            {
                FiltersStatus.Add(new OptionSet("-1", "Tất cả"));
                var list = QueuesStatusCodeData.GetQueuesData();
                foreach (var item in list)
                {
                    FiltersStatus.Add(new OptionSet(item.Id, item.Name));
                }
            }
        }

        public async Task LoadProject()
        {
            if (FiltersStatus != null && FiltersStatus.Count == 0)
            {
                string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_project'>
                                    <attribute name='bsd_projectid' alias='Val'/>
                                    <attribute name='bsd_projectcode'/>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='createdon' />
                                    <order attribute='bsd_name' descending='false' />
                                  </entity>
                            </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_projects", fetchXml);
                if (result == null || result.value.Any() == false) return;

                FiltersProject.Add(new OptionSet("-1", "Tất cả"));
                var data = result.value;
                foreach (var item in data)
                {
                    FiltersProject.Add(item);
                }
            }
        }

        public async Task LoadUnit()
        {
            if (!string.IsNullOrWhiteSpace(FilterProject.Val) &&  FilterProject.Val != "-1")
            {
                string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='product'>
                                        <attribute name='name' alias='Label'/>
                                        <attribute name='productid' alias='Val'/>
                                        <order attribute='createdon' descending='true' />
                                        <filter type='and'>
                                             <condition attribute='bsd_projectcode' operator='eq' value='{FilterProject.Val}' />
                                        </filter>
                                    </entity>
                                </fetch>";
                var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("products", fetchXml);
                if (result == null || result.value.Any() == false) return;

                FiltersUnit.Add(new OptionSet("-1","Tất cả"));
                var data = result.value;
                foreach (var item in data)
                {
                    FiltersUnit.Add(item);
                }
            }
        }
    }
}
