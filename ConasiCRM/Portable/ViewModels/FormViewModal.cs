using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class FormViewModal : BaseViewModel
    {
        public ObservableCollection<Portable.Models.LookUp> LookUpData { get; set; }
        public Portable.Models.LookUpConfig CurrentLookUpConfig { get; set; }

        private bool _showLookUpModal;
        public bool ShowLookUpModal
        {
            get { return _showLookUpModal; }
            set
            {
                if (_showLookUpModal != value)
                {
                    _showLookUpModal = value;
                    OnPropertyChanged(nameof(ShowLookUpModal));
                }
            }
        }

        private int _lookUpPage;
        public int LookUpPage
        {
            get { return _lookUpPage; }
            set
            {
                if (_lookUpPage != value)
                {
                    _lookUpPage = value;
                    OnPropertyChanged(nameof(LookUpPage));
                }
            }
        }

        private bool _lookUpLoading;
        public bool LookUpLoading
        {
            get { return _lookUpLoading; }
            set
            {
                if (_lookUpLoading != value)
                {
                    _lookUpLoading = value;
                    OnPropertyChanged(nameof(LookUpLoading));
                }
            }
        }

        public ICommand CloseLookUpModal => new Command(() =>
        {
            LookUpData.Clear();
            ShowLookUpModal = false;
        });

        public async Task loadData()
        {
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>(CurrentLookUpConfig.EntityName, string.Format(CurrentLookUpConfig.FetchXml, _lookUpPage));
            var data = result.value;
            foreach (var item in data)
            {
                LookUpData.Add(item);
            }
        }

        public FormViewModal()
        {
            // lookup items
            LookUpData = new ObservableCollection<Portable.Models.LookUp>();

            LookUpPage = 1;
            IsBusy = true;
        }
    }
}
