using ConasiCRM.Portable.Controls;
using ConasiCRM.Portable.Helper;
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
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class DirectSaleViewModel : BaseViewModel
    {
        public ObservableCollection<ProjectList> Projects { get; set; } = new ObservableCollection<ProjectList>();
        public ObservableCollection<OptionSet> PhasesLaunchs { get; set; } = new ObservableCollection<OptionSet>();

        private List<OptionSet> _viewOptions;
        public List<OptionSet> ViewOptions { get => _viewOptions; set { _viewOptions = value;OnPropertyChanged(nameof(ViewOptions)); } }

        private List<string> _selectedViews;
        public List<string> SelectedViews { get=>_selectedViews; set { _selectedViews = value;OnPropertyChanged(nameof(SelectedViews)); } }

        private List<OptionSet> _directionOptions;
        public List<OptionSet> DirectionOptions { get=>_directionOptions; set { _directionOptions = value;OnPropertyChanged(nameof(DirectionOptions)); } }

        private List<string> _selectedDirections;
        public List<string> SelectedDirections { get => _selectedDirections; set { _selectedDirections = value; OnPropertyChanged(nameof(SelectedDirections)); } }

        private List<OptionSet> _unitStatusOptions;
        public List<OptionSet> UnitStatusOptions { get=>_unitStatusOptions; set { _unitStatusOptions = value;OnPropertyChanged(nameof(UnitStatusOptions)); } }

        private List<string> _selectedUnitStatus;
        public List<string> SelectedUnitStatus { get => _selectedUnitStatus; set { _selectedUnitStatus = value; OnPropertyChanged(nameof(SelectedUnitStatus)); } }

        private OptionSet _phasesLaunch;
        public OptionSet PhasesLaunch { get => _phasesLaunch; set { _phasesLaunch = value; OnPropertyChanged(nameof(PhasesLaunch)); } }

        private string _unitCode;
        public string UnitCode { get => _unitCode; set { _unitCode = value; OnPropertyChanged(nameof(UnitCode)); } }

        private decimal? _minNetArea;
        public decimal? minNetArea { get => _minNetArea; set { _minNetArea = value; OnPropertyChanged(nameof(minNetArea)); } }

        private decimal? _maxNetArea;
        public decimal? maxNetArea { get => _maxNetArea; set { _maxNetArea = value; OnPropertyChanged(nameof(maxNetArea)); } }

        private decimal? _minPrice;
        public decimal? minPrice { get => _minPrice; set { _minPrice = value; OnPropertyChanged(nameof(minPrice)); } }

        private decimal? _maxPrice;
        public decimal? maxPrice { get => _maxPrice; set { _maxPrice = value; OnPropertyChanged(nameof(maxPrice)); } }

        private ProjectList _project;
        public ProjectList Project
        {
            get => _project;
            set
            {
                if (_project != value)
                {
                    PhasesLaunch = null;
                    PhasesLaunchs.Clear();
                    _project = value;
                    OnPropertyChanged(nameof(Project));
                    
                }
            }
        }

        private bool _isEvent;
        public bool IsEvent
        {
            get => _isEvent;
            set
            {
                if (_isEvent != value)
                {
                    this._isEvent = value;
                    OnPropertyChanged(nameof(IsEvent));
                }
            }
        }

        public DirectSaleViewModel()
        {
        }

        public async Task LoadProject()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_project'>
                                    <attribute name='bsd_projectid'/>
                                    <attribute name='bsd_projectcode'/>
                                    <attribute name='bsd_name'/>
                                    <attribute name='createdon' />
                                    <order attribute='bsd_name' descending='false' />
                                  </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ProjectList>>("bsd_projects", fetchXml);
            if (result == null || result.value.Any() == false) return;

             var data = result.value;
            foreach (var item in data)
            {
                Projects.Add(item);
            }
        }

        public async Task LoadPhasesLanch()
        {
            if (Project == null) return;
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                        <entity name='bsd_phaseslaunch'>
                        <attribute name='bsd_name' alias='Label' />
                        <attribute name='bsd_phaseslaunchid' alias='Val' />
                        <order attribute='createdon' descending='true' />
                        <filter type='and'>
                          <condition attribute='statecode' operator='eq' value='0' />
                          <condition attribute='statuscode' operator='eq' value='100000000' />
                          <condition attribute='bsd_projectid' operator='eq' uitype='bsd_project' value='" + Project.bsd_projectid + @"' />
                        </filter>
                      </entity>
                    </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_phaseslaunchs", fetchXml);
            if (result == null || result.value.Any() == false) return;

            var data = result.value;
            foreach (var item in data)
            {
                PhasesLaunchs.Add(item);
            }
        }
    }
}
