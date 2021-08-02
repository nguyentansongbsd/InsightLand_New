using ConasiCRM.Portable.Helper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class ReservationCoowner
    {
        public Guid bsd_coownerid { get; set; }
        public string bsd_name { get; set; }

        public int bsd_relationship { get; set; } // moi quan he

        public string ReservationName { get; set; }

        public string bsd_relationship_format
        {
            get
            {
                switch (bsd_relationship)
                {
                    case 100000000:
                        return "Vợ/chồng";
                    case 100000001:
                        return "Con";
                    case 100000002:
                        return "Cha/mẹ";
                    case 100000003:
                        return "Bạn";
                    case 100000004:
                        return "Khác";
                    default:
                        return "";
                }
            }
        }

        public string account_name { get; set; }
        public string contact_name { get; set; }

        public string customer
        {
            get
            {
                if (!string.IsNullOrEmpty(account_name))
                {
                    return account_name;
                }
                else
                {
                    return contact_name;
                }
            }
        }

        public DateTime createdon { get; set; }
        public string createdon_format
        {
            get => StringHelper.DateFormat(createdon);
        }
    }
}
