using ConasiCRM.Portable.Resources;
using System;

using Xamarin.Forms;

namespace ConasiCRM.Portable.Models
{
    public class ListCaseAcc : ContentView
    {
        public string customerid { get; set; }
        public string title { get; set; }
        public string ticketnumber { get; set; }
        public int prioritycode { get; set; }
        public string prioritycodevalue
        {
            get
            {
                switch (prioritycode)
                {
                    case 1:
                        return "High";
                    case 2:
                        return "Normal";
                    case 3:
                        return "Low";
                    default:
                        return "";
                }
            }
        }
        public int caseorigincode { get; set; }
        public string caseorigincodevalue
        {
            get
            {
                switch (caseorigincode)
                {
                    case 1:
                        return "Phone";
                    case 2:
                        return "Email";
                    case 3:
                        return "Web";
                    case 2483:
                        return "Facebook";
                    case 3986:
                        return "Twitter";
                    default:
                        return "";
                }
            }
        }
        public int statuscode { get; set; }
        public string statuscodevalue
        {
            get
            {
                switch (statuscode)
                {
                    case 1:
                        return Language.case_in_progress_sts;
                    case 2:
                        return Language.case_on_hold_sts;
                    case 3:
                        return Language.case_waiting_for_details_sts;
                    case 4:
                        return Language.case_researching_sts;
                    case 5:
                        return Language.case_problem_solved_sts; //Vấn đề đã được giải quyết
                    case 1000:
                        return Language.case_information_provided_sts;  //Cung cấp thông tin
                    case 6:
                        return Language.case_cancelled_sts;
                    case 2000:
                        return Language.case_merged_sts;
                    default:
                        return "";
                }
            }
        }

        public string case_nameaccount { get; set; }
        public string case_nameaccontact { get; set; }
    }
}

