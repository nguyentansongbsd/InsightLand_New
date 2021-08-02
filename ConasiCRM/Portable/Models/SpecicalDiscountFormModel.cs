using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class SpecicalDiscountFormModel : BaseViewModel
    {
        private string _bsd_name;
        public string bsd_name
        {
            get => _bsd_name;
            set
            {
                _bsd_name = value;
                OnPropertyChanged(nameof(bsd_name));
            }
        }

        private string _bsd_reasons;
        public string bsd_reasons
        {
            get => _bsd_reasons;
            set
            {
                _bsd_reasons = value;
                OnPropertyChanged(nameof(bsd_reasons));
            }
        }

        private string _bsd_cchtnh;
        public string bsd_cchtnh
        {
            get => _bsd_cchtnh;
            set
            {
                _bsd_cchtnh = value;
                OnPropertyChanged(nameof(bsd_cchtnh));
            }
        }

        private decimal? _bsd_amountdiscount;
        public decimal? bsd_amountdiscount
        {
            get => _bsd_amountdiscount;
            set
            {
                _bsd_amountdiscount = value;
                OnPropertyChanged(nameof(bsd_amountdiscount));
            }
        }

        private decimal? _bsd_percentdiscount;
        public decimal? bsd_percentdiscount
        {
            get => _bsd_percentdiscount;
            set
            {
                _bsd_percentdiscount = value;
                OnPropertyChanged(nameof(bsd_percentdiscount));
            }
        }

        public Guid approver_id { get; set; }
        public string approver_name { get; set; }

        public Guid quote_id { get; set; }
        public string quote_name { get; set; }

        private DateTime? _bsd_approvaldate;
        public DateTime? bsd_approvaldate
        {
            get => _bsd_approvaldate;
            set
            {
                _bsd_approvaldate = value;
                OnPropertyChanged(nameof(bsd_approvaldate));
            }
        }

        private DateTime? _createdon;
        public DateTime? createdon
        {
            get => _createdon;
            set
            {
                if (_createdon != value)
                {
                    _createdon = value;
                    OnPropertyChanged(nameof(createdon));
                }
            }
        }
    }
}
