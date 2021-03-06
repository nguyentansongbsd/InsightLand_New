using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.ViewModels;
using System;
namespace ConasiCRM.Portable.Models
{
    public class QueuesDetailModel : BaseViewModel
    {
        public string opportunityid { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public Guid _bsd_units_value { get; set; }
        public string unit_name { get; set; }

        public Guid _bsd_project_value { get; set; }
        public string project_name { get; set; }

        public string bsd_queuenumber { get; set; }

        public Guid contact_id { get; set; }
        public string contact_name { get; set; }
        public string PhoneContact { get; set; }

        public Guid account_id { get; set; }
        public string account_name { get; set; }
        public string PhoneAccount { get; set; }

        public Guid _bsd_salesagentcompany_value { get; set; }
        public string salesagentcompany_name { get; set; }

        public decimal? bsd_queuingfee { get; set; }
        public string bsd_queuingfee_format { get => StringFormatHelper.FormatCurrency(bsd_queuingfee); }
        public decimal? budgetamount { get; set; }
        public string budgetamount_format { get => StringFormatHelper.FormatCurrency(budgetamount); }
        public Guid _bsd_phaselaunch_value { get; set; }
        public string phaselaunch_name { get; set; }

        public string bsd_nameofstaffagent { get; set; }

        public int statuscode { get; set; }

        public DateTime _bsd_bookingtime;
        public DateTime bsd_bookingtime { get => _bsd_bookingtime.AddHours(7); set { _bsd_bookingtime = value; OnPropertyChanged(nameof(bsd_bookingtime)); } }

        public DateTime _createdon;
        public DateTime createdon { get => _createdon.AddHours(7); set { _createdon = value; OnPropertyChanged(nameof(createdon)); } }

        public DateTime _bsd_queuingexpired;
        public DateTime bsd_queuingexpired { get => _bsd_queuingexpired.AddHours(7); set { _bsd_queuingexpired = value; OnPropertyChanged(nameof(bsd_queuingexpired)); } }
    }
}
