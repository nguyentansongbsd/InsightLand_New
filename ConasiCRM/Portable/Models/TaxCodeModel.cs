using ConasiCRM.Portable.Helper;
using System;
namespace ConasiCRM.Portable.Models
{
    public class TaxCodeModel
    {
        public Guid bsd_taxcodeid { get; set; }
        public string bsd_taxcode { get; set; }
        public decimal bsd_value { get; set; }
        public string bsd_value_format { get => StringFormatHelper.FormatCurrency(bsd_value); }
    }
}
