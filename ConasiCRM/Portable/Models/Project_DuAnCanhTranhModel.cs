using ConasiCRM.Portable.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class Project_DuAnCanhTranhModel
    {
        public string bsd_name { get; set; } // tên dự án
        public string bsd_projectcode { get; set; } // mã dự án
        public string bsd_investor_name { get; set; } // chủ đầu tư

        public string bsd_strength { get; set; } // điểm mạnh
        public string bsd_weakness { get; set; } // điểm yếu

        public DateTime? createdon { get; set; }
        public string createdon_format
        {
            get => StringHelper.DateFormat(createdon);
        }
    }
}
