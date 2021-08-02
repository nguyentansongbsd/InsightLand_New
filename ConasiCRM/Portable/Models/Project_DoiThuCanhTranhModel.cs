using ConasiCRM.Portable.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class Project_DoiThuCanhTranhModel
    {
        public string name { get; set; } // tiêu đề
        public string websiteurl { get; set; }
        public string weaknesses { get; set; } // điểm yếu
        public string strengths { get; set; } // điểm mạnh
        public DateTime? createdon { get; set; } // ngày tạo
        public string createdon_format
        {
            get => StringHelper.DateFormat(createdon);
        }

    }
}
