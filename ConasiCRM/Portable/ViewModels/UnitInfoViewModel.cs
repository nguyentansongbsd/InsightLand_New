using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.ViewModels
{
    public class UnitInfoViewModel : BaseViewModel
    {
        private UnitInfoModel _unitInfo;
        public UnitInfoModel UnitInfo
        {
            get => _unitInfo;
            set
            {
                _unitInfo = value;
                OnPropertyChanged(nameof(UnitInfo));
            }
        }
    }
}
