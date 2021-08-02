using ConasiCRM.Portable.Converters.Unit;
using ConasiCRM.Portable.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class ListInforDoithucanhtranhTLKD 
    {
        public string name { get; set; }
        public string websiteurl { get; set; }
        public string strengths { get; set; }
        public string weaknesses { get; set; }
        public DateTime createdon { get; set; }
        public string createdonformat
        {
            get => StringHelper.DateFormat(createdon);
        }
    }
}

