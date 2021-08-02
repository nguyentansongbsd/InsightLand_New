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
    public class DirectSaleViewModel : FormLookupViewModel
    {

        public ObservableCollection<OptionSet> DirectionOptions { get; set; }
        public ObservableCollection<OptionSet> ViewOptions { get; set; }
        public ObservableCollection<OptionSet> UnitStatusOptions { get; set; }

        private ObservableCollection<string> _selectedDirections;

        public ObservableCollection<string> SelectedDirections { get => _selectedDirections; set { _selectedDirections = value; OnPropertyChanged(nameof(SelectedDirections)); } }
        public ObservableCollection<string> SelectedViews { get; set; }
        public ObservableCollection<string> SelectedUnitStatus { get; set; }

        private string _unitCode;
        public string UnitCode { get =>_unitCode; set { _unitCode = value;OnPropertyChanged(nameof(UnitCode)); } }

        private decimal? _minNetArea;
        public decimal? minNetArea { get =>_minNetArea; set { _minNetArea = value; OnPropertyChanged(nameof(minNetArea)); } }

        private decimal? _maxNetArea;
        public decimal? maxNetArea { get => _maxNetArea; set { _maxNetArea = value; OnPropertyChanged(nameof(maxNetArea)); } }

        private decimal? _minPrice;
        public decimal? minPrice { get => _minPrice; set { _minPrice = value;OnPropertyChanged(nameof(minPrice)); } }

        private decimal? _maxPrice;
        public decimal? maxPrice { get => _maxPrice; set { _maxPrice = value; OnPropertyChanged(nameof(maxPrice)); } }

        private ConasiCRM.Portable.Models.LookUp _project;
        public Models.LookUp Project
        {
            get => _project;
            set
            {
                if (_project != value)
                {
                    this.PhasesLanch = null;
                    this._project = value;
                    OnPropertyChanged(nameof(Project));
                }
            }
        }

        private bool _isEvent;
        public bool IsEvent
        {
            get
            {
                return _isEvent;
            }
            set
            {
                if (_isEvent != value)
                {
                    this._isEvent = value;
                    OnPropertyChanged(nameof(IsEvent));
                }
            }
        }

        private bool _isCollapse;
        public bool IsCollapse
        {
            get
            {
                return _isCollapse;
            }
            set
            {
                if (_isCollapse != value)
                {
                    this._isCollapse = value;
                    OnPropertyChanged(nameof(IsCollapse));
                }
            }
        }

        private Models.LookUp _phasesLanch;
        public Models.LookUp PhasesLanch
        {
            get
            {
                return _phasesLanch;
            }
            set
            {
                if (_phasesLanch != value)
                {
                    this._phasesLanch = value;
                    OnPropertyChanged(nameof(PhasesLanch));
                }
            }
        }

        public LookUpConfig ProjectConfig { get; set; }

        public LookUpConfig PhasesLanchConfig { get; set; }

        public DirectSaleViewModel()
        {
            SelectedViews = new ObservableCollection<string>();
            SelectedDirections = new ObservableCollection<string>();
            SelectedUnitStatus = new ObservableCollection<string>();

            this.IsBusy = true;
            DirectionOptions = new ObservableCollection<OptionSet>()
            {
                new OptionSet("100000000", "Đông"),
                new OptionSet("100000001", "Tây"),
                new OptionSet("100000002", "Nam"),
                new OptionSet("100000003", "Bắc"),
                new OptionSet("100000004", "Tây Bắc"),
                new OptionSet("100000005", "Đông Bắc"),
                new OptionSet("100000006", "Tây Nam"),
                new OptionSet("100000007", "Đông Nam"),
                new OptionSet("100000008", "Apartment"),
                new OptionSet("100000009", "Condotel"),
            };
            ViewOptions = new ObservableCollection<OptionSet>()
            {
                new OptionSet("100000000","Sông/biển/hồ"),
                new OptionSet("100000001","Núi"),
                new OptionSet("100000004","Thành phố"),
                new OptionSet("100000005","Hồ bơi"),
                new OptionSet("100000002","Đông"),
                new OptionSet("100000003","Tây"),
                new OptionSet("100000006","Biển"),
            };
            UnitStatusOptions = new ObservableCollection<OptionSet>()
            {
                new OptionSet("1","Preparing"),
                new OptionSet("100000000","Available"),
                new OptionSet("100000004","Queuing"),
                new OptionSet("100000001","1st Installment"),
                new OptionSet("100000005","Collected"),
                new OptionSet("100000003","Deposited"),
                new OptionSet("100000006","Reserve"),
                new OptionSet("100000002","Sold"),
            };

            ProjectConfig = new LookUpConfig()
            {
                FetchXml = @"<fetch version='1.0' count='10' page='{0}' output-format='xml-platform' mapping='logical' distinct='false'>
                    <entity name='bsd_project'>
                        <attribute name='bsd_projectid' alias='Id' />
                        <attribute name='bsd_name' alias='Name' />
                        <attribute name='createdon' />
                        <order attribute='bsd_name' descending='false' />
                        <filter type='and'>
                          <condition attribute='bsd_name' operator='like' value='%{1}%' />
                        </filter>
                      </entity>
                </fetch>",
                EntityName = "bsd_projects",
                PropertyName = "Project",
                LookUpTitle = "Chọn dự án"
            };

            PhasesLanchConfig = new LookUpConfig()
            {
                EntityName = "bsd_phaseslaunchs",
                PropertyName = "PhasesLanch",
                LookUpTitle = "Chọn đợt mở bán"
            };
        }
    }
}
