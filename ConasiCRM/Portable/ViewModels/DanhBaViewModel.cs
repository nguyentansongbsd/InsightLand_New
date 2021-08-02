using System;
using System.Collections.ObjectModel;
using ConasiCRM.Portable.Models;

namespace ConasiCRM.Portable.ViewModels
{
    public class DanhBaViewModel : BaseViewModel
    {
        public ObservableCollection<DanhBaItemModel> Contacts { get; set; }

        private bool _isCheckedAll;
        public bool isCheckedAll { get => _isCheckedAll; set { _isCheckedAll = value; OnPropertyChanged(nameof(isCheckedAll)); } }

        private int _numberChecked;
        public int numberChecked { get => _numberChecked; set { _numberChecked = value; totalChecked = value.ToString() + "/" + total.ToString(); OnPropertyChanged(nameof(numberChecked)); } }

        private int _total;
        public int total { get => _total; set { _total = value; totalChecked = numberChecked.ToString() + "/" + value.ToString(); OnPropertyChanged(nameof(total)); } }

        private string _totalChecked;
        public string totalChecked { get => _totalChecked; set { _totalChecked = "Đã chọn " + value; OnPropertyChanged(nameof(totalChecked)); } }

        public DanhBaViewModel()
        {
            Contacts = new ObservableCollection<DanhBaItemModel>();

            isCheckedAll = false;
            numberChecked = 0;
            total = 0;
        }

        public void reset()
        {
            Contacts.Clear();

            isCheckedAll = false;
            numberChecked = 0;
            total = 0;
        }
    }
}
