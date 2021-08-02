using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class CountUnit
    {
        public int Preparing { get; set; }
        public int Available { get; set; }
        public int Queueing { get; set; }
        public int Reserve { get; set; }
        public int Collected { get; set; }
        public int Deposited { get; set; }
        public int ThoaThuanDatCoc { get; set; }
        public int StInstallment { get; set; }
        public int Sold { get; set; }

        public int Total
        {
            get
            {
                return Preparing + Available + Queueing + Reserve + Collected + Deposited + ThoaThuanDatCoc + StInstallment + Sold;
            }
        }
    }
}
