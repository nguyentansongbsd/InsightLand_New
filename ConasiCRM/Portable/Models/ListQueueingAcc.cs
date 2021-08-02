using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Models
{
    public class ListQueueingAcc 
    {
        public string customerid { get; set; }
        public string bsd_queuenumber { get; set; }
        public string name { get; set; }
        public decimal? estimatedvalue { get; set; }
        public string estimatedvalueformat => estimatedvalue.HasValue ? string.Format("{0:#,0.#}", estimatedvalue.Value) + " đ" : null;

        public int statuscode { get; set; }
        public string statuscodevalue
        {
            get
            {
                switch (statuscode)
                {
                    case 1:
                        return "Draft";
                    case 2:
                        return "On Hold";
                    case 100000000:
                        return "Queuing";
                    case 100000001:
                        return "Collected Queuing Fee";
                    case 100000002:
                        return "Waiting";
                    case 100000003:
                        return "Expired";
                    case 100000004:
                        return "Completed";
                    case 3:
                        return "Won";
                    case 4:
                        return "Canceled";
                    case 5:
                        return "Out-Sold";
                    default:
                        return "";
                }
            }
        }
        public DateTime createdon { get; set; }
        public string createdonformat 
        {
            get => StringHelper.DateFormat(createdon);
        }
        public string que_nameproject { get; set; }
        public string que_nameaccount { get; set; }
        public string que_namecontact { get; set; }

    }
}

