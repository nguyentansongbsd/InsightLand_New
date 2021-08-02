using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class ReservationDiscountOptionSet : OptionSet
    {
        private bool _isExpired;
        public bool IsExpired
        {
            get => _isExpired;
            set
            {
                _isExpired = value;
                OnPropertyChanged(nameof(IsExpired));
            }
        }

        private DateTime _bsd_startdate;
        public DateTime bsd_startdate
        {
            get => _bsd_startdate;
            set
            {
                _bsd_startdate = value;
                OnPropertyChanged(nameof(bsd_startdate));
            }
        }

        private DateTime _bsd_enddate;
        public DateTime bsd_enddate
        {
            get => _bsd_enddate;
            set
            {
                _bsd_enddate = value;
                OnPropertyChanged(nameof(bsd_enddate));
            }
        }
    }
}
