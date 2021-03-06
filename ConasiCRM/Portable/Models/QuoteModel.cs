using System;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.ViewModels;

namespace ConasiCRM.Portable.Models
{
    public class QuoteModel : BaseViewModel
    {
        public Guid quoteid { get; set; }

        private string _name;
        public string name { get=>_name; set { _name = value; OnPropertyChanged(nameof(name)); } }

        public Guid paymentscheme_id { get; set; }
        public string paymentscheme_name { get; set; }

        private decimal _bsd_constructionarea;
        public decimal bsd_constructionarea { get=> _bsd_constructionarea; set { _bsd_constructionarea = value;OnPropertyChanged(nameof(bsd_constructionarea)); } }
        public string bsd_constructionarea_format { get => StringFormatHelper.FormatCurrency(bsd_constructionarea); }

        private decimal _bsd_netusablearea;
        public decimal bsd_netusablearea { get=> _bsd_netusablearea; set { _bsd_netusablearea = value;OnPropertyChanged(nameof(bsd_netusablearea)); } }
        public string bsd_netusablearea_format { get => StringFormatHelper.FormatCurrency(bsd_netusablearea); }

        private decimal _bsd_actualarea;
        public decimal bsd_actualarea { get=> _bsd_actualarea; set { _bsd_actualarea = value;OnPropertyChanged(nameof(bsd_actualarea)); } }
        public string bsd_actualarea_format { get => StringFormatHelper.FormatCurrency(bsd_actualarea); }
        public string bsd_unitstatus { get; set; }

        public Guid discountlist_id { get; set; }
        public string discountlist_name { get; set; }
        public string bsd_discounts { get; set; }
        public string bsd_contracttypedescripton { get; set; }

        private decimal _bsd_depositfee;
        public decimal bsd_depositfee { get=> _bsd_depositfee; set { _bsd_depositfee = value;OnPropertyChanged(nameof(bsd_depositfee)); } }
        public string bsd_depositfee_format { get => StringFormatHelper.FormatCurrency(bsd_depositfee); }

        private decimal _bsd_bookingfee;
        public decimal bsd_bookingfee { get=> _bsd_bookingfee; set { _bsd_bookingfee = value;OnPropertyChanged(nameof(bsd_bookingfee)); } }
        public string bsd_bookingfee_format { get => StringFormatHelper.FormatCurrency(bsd_bookingfee); }

        private string _bsd_nameofstaffagent;
        public string bsd_nameofstaffagent { get=>_bsd_nameofstaffagent; set { _bsd_nameofstaffagent = value;OnPropertyChanged(nameof(bsd_nameofstaffagent)); } }
        public string bsd_referral { get; set; }

        private decimal _bsd_detailamount;
        public decimal bsd_detailamount { get=> _bsd_detailamount; set { _bsd_detailamount = value;OnPropertyChanged(nameof(bsd_detailamount)); } }
        public string bsd_detailamount_format { get => StringFormatHelper.FormatCurrency(bsd_detailamount); }

        private int _bsd_numberofmonthspaidmf;
        public int bsd_numberofmonthspaidmf { get=> _bsd_numberofmonthspaidmf; set { _bsd_numberofmonthspaidmf = value;OnPropertyChanged(nameof(bsd_numberofmonthspaidmf)); } }

        private decimal _bsd_managementfee;
        public decimal bsd_managementfee { get=> _bsd_managementfee; set { _bsd_managementfee = value; OnPropertyChanged(nameof(bsd_managementfee)); } }
        public string bsd_managementfee_format { get => StringFormatHelper.FormatCurrency(bsd_managementfee); }
        public string bsd_waivermanafeemonth { get; set; }

        public decimal bsd_discount { get; set; }
        public decimal bsd_packagesellingamount { get; set; }
        public decimal bsd_totalamountlessfreight { get; set; }
        public decimal bsd_landvaluededuction { get; set; }
        public decimal totaltax { get; set; }
        public decimal bsd_freightamount { get; set; }
        public decimal totalamount { get; set; }

        public Guid queue_id { get; set; }
        public string queue_name { get; set; }

        public Guid contact_id { get; set; }
        public string contact_name { get; set; }

        public Guid account_id { get; set; }
        public string account_name { get; set; }

        public Guid unit_id { get; set; }

        private string _unit_name;
        public string unit_name { get=>_unit_name; set { _unit_name = value; OnPropertyChanged(nameof(unit_name)); } }
        public string unit_statuscode { get; set; }

        private decimal _unit_price;
        public decimal unit_price { get=> _unit_price; set { _unit_price = value;OnPropertyChanged(nameof(unit_price)); } }
        public decimal bsd_landvalueofunit { get; set; }
        public decimal maintenancefreespercent { get; set; }
        public Guid _bsd_projectcode_value { get; set; }
        public Guid _bsd_phaseslaunchid_value { get; set; }
        public Guid _bsd_unittype_value { get; set; }

        public Guid project_id { get; set; }

        private string _project_name;
        public string project_name { get => _project_name; set { _project_name = value;OnPropertyChanged(nameof(project_name)); } }

        private string _phaseslaunch_name;
        public string phaseslaunch_name { get=> _phaseslaunch_name; set { _phaseslaunch_name = value;OnPropertyChanged(nameof(phaseslaunch_name)); } }
        public Guid pricelist_apply_id { get; set; }
        public string pricelist_apply_name { get; set; }
        public Guid pricelist_phaselaunch_id { get; set; }
        public string pricelist_phaselaunch_name { get; set; }

        public decimal tax_value { get; set; }
        public Guid tax_id { get; set; }

        public Guid saleagentcompany_id { get; set; }
        public string saleagentcompany_name { get; set; }
    }
}
