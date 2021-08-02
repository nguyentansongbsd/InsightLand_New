using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telerik.XamarinForms.Common;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class ListViewBaseViewModel<T> : BaseViewModel where T : class
    {
        public ObservableCollection<T> Data { get; set; }
        private int _page;
        public int Page
        {
            get
            {
                return _page;
            }
            set
            {
                if (_page != value)
                {
                    this._page = value;
                    OnPropertyChanged("Page");
                }
            }
        }

        private string _keyword;
        public string Keyword
        {
            get
            {
                return _keyword;
            }
            set
            {
                if (_keyword != value)
                {
                    this._keyword = value;
                    OnPropertyChanged(nameof(Keyword));
                }
            }
        }

        private LoadOnDemandMode _loadOnDemandMode;
        public LoadOnDemandMode LoadOnDemandMode
        {
            get
            {
                return _loadOnDemandMode;
            }
            set
            {
                if (_loadOnDemandMode != value)
                {
                    _loadOnDemandMode = value;
                    OnPropertyChanged(nameof(LoadOnDemandMode));
                }
            }
        }

        public ListViewBaseViewModel()
        {
            Data = new ObservableCollection<T>();
            this._page = 0;
            LoadOnDemandMode = LoadOnDemandMode.Automatic;
        }

        public ICommand RefreshCommand
        {
            get => new Command(ResetData);
        }

        public virtual void ResetData()
        {
            _page = 0;
            Data.Clear();
            LoadOnDemandMode = LoadOnDemandMode.Automatic;
        }
    }
}
