using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.ViewModels
{
    public class PhiMoGioiGiaoDichFormViewModel : BaseViewModel
    {
        private PhiMoGioiGiaoDichFormModel _phiMoGioiGiaoDichFormModel;
        public PhiMoGioiGiaoDichFormModel PhiMoGioiGD
        {
            get => _phiMoGioiGiaoDichFormModel;
            set
            {
                _phiMoGioiGiaoDichFormModel = value;
                OnPropertyChanged(nameof(PhiMoGioiGD));
            }
        }
    }
}
