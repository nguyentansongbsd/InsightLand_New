using System;
namespace ConasiCRM.Portable.Models
{
    public class HandoverConditionModel
    {
        public Guid bsd_packagesellingid { get; set; }
        public string bsd_name { get; set; }
        public Guid _bsd_unittype_value { get; set; }
        public bool bsd_byunittype { get; set; }

    }
}
