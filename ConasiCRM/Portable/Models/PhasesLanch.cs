using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class PhasesLanch
    {
        public string bsd_name { get; set; }
        public DateTime createdon { get; set; }
        public Guid bsd_phaseslaunchid { get; set; }

        public override string ToString()
        {
            return this.bsd_name;
        }
    }
}
