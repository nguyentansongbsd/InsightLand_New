using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.ViewModels
{
    public class HoaHongGiaoDichFormViewModel : BaseViewModel
    {
        private HoaHongGiaoDichFormModel _HoaHongGiaoDichForm;
        public HoaHongGiaoDichFormModel HoaHongGiaoDich
        {
            get => _HoaHongGiaoDichForm;
            set
            {
                _HoaHongGiaoDichForm = value;
                OnPropertyChanged(nameof(HoaHongGiaoDich));
            }
        }
    }
}
