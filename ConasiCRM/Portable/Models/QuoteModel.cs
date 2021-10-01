using System;
namespace ConasiCRM.Portable.Models
{
    public class QuoteModel
    {
        public Guid quoteid { get; set; }
        public string name { get; set; }

        public Guid paymentscheme_id { get; set; }
        public string paymentscheme_name { get; set; }

        public Guid discountlist_id { get; set; }
        public string discountlist_name { get; set; }
        public string bsd_discounts { get; set; }

        public Guid unit_id { get; set; }
        public string unit_name { get; set; }
        public Guid _bsd_projectcode_value { get; set; }
        public Guid _bsd_phaseslaunchid_value { get; set; }
    }
}
