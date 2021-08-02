using System;
using ConasiCRM.Portable.ViewModels;

namespace ConasiCRM.Portable.Models
{
    public class Case : BaseViewModel
    {
        private string _incidentid;
        public string incidentid { get => _incidentid; set { _incidentid = value; OnPropertyChanged(nameof(incidentid)); } }

        private string _title_label;
        public string title_label { get => _title_label; set { _title_label = value; OnPropertyChanged(nameof(title_label)); } }

        private string _ticketnumber;
        public string ticketnumber { get => _ticketnumber; set { _ticketnumber = value; OnPropertyChanged(nameof(ticketnumber)); } }

        private string _caseorigincode;
        public string caseorigincode { get => _caseorigincode; set { _caseorigincode = value; OnPropertyChanged(nameof(caseorigincode)); } }

        private string _prioritycode;
        public string prioritycode { get => _prioritycode; set { _prioritycode = value; OnPropertyChanged(nameof(prioritycode)); } }

        private DateTime? _createdon;
        public DateTime? createdon { get => _createdon; set { _createdon = value; OnPropertyChanged(nameof(createdon)); } }
        public string createdon_format { get => createdon.HasValue ? createdon.Value.ToString("dd/MM/yyyy") : null; }

        private string _statuscode;
        public string statuscode { get => _statuscode; set { _statuscode = value; OnPropertyChanged(nameof(statuscode)); } }
        public string statuscode_label {
            get
            {
                switch (statuscode)
                {
                    case "1":return "In Progress";
                    case "2":return "On Hold";
                    case "3":return "Waiting for Details";
                    case "4":return "Researching";
                    case "5":return "Problem Solved";
                    case "1000":return "Information Provided";
                    case "6":return "Cancelled";
                    case "2000":return "Merged";
                    default: return null;
                }
            }
        }

        private string _customerid_label_account;
        private string customerid_label_account { get => _customerid_label_account; set { _customerid_label_account = value; if (value != null) { customerid_label_account = null; customerid_label = value; } OnPropertyChanged(nameof(customerid_label_account)); } }

        private string _customerid_label_contact;
        public string customerid_label_contact { get => _customerid_label_contact; set { _customerid_label_contact = value; if (value != null) { customerid_label_account = null; customerid_label = value; } OnPropertyChanged(nameof(customerid_label_contact)); } }

        private string _customerid_label;
        public string customerid_label { get => _customerid_label; set { _customerid_label = value; OnPropertyChanged(nameof(customerid_label)); } }

        public Case()
        {
            this.title_label = " ";
        }
    }
}
