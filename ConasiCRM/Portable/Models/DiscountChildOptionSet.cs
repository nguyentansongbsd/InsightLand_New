using System;
namespace ConasiCRM.Portable.Models
{
    public class DiscountChildOptionSet :OptionSet
    {
        public decimal bsd_amount { get; set; }
        public decimal bsd_percentage { get; set; }
        public string new_type { get; set; }
    }
}
