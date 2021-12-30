using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class EventModel : BaseViewModel
    {
        public Guid bsd_eventid { get; set; }

        private DateTime? _bsd_startdate;
        public DateTime? bsd_startdate
        {
            get => this._bsd_startdate;
            set
            {
                if (value.HasValue)
                {
                    _bsd_startdate = value;
                    OnPropertyChanged(nameof(bsd_startdate));
                }
            }
        }

        private DateTime? _bsd_enddate;
        public DateTime? bsd_enddate
        {
            get => this._bsd_enddate;
            set
            {
                if (value.HasValue)
                {
                    _bsd_enddate = value;
                    OnPropertyChanged(nameof(bsd_enddate));
                }
            }
        }
        public int statuscode { get; set; }
        public DateTime createdon { get; set; }
        public string bsd_name { get; set; }
        public string bsd_eventcode { get; set; }
        public Guid bsd_phaselaunch { get; set; }
        public string bsd_phaselaunch_name { get; set; }
    }
}
