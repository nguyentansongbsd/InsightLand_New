using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class Project
    {
        public Guid bsd_projectid { get; set; }
        public string bsd_name { get; set; }
        public DateTime createdon { get; set; }

        public override string ToString()
        {
            return this.bsd_name;
        }
    }
}
