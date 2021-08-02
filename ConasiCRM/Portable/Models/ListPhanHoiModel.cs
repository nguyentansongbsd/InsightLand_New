using System;

namespace ConasiCRM.Portable.Models
{
    public class ListPhanHoiModel
    {
        public string customerid { get; set; }
        public string _customerid_value { get; set; }
        public string subjecttitle { get; set; }

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
        public string _productid_value { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public DateTime createdon { get; set; }
        public string createdon_format
        {
            get
            {
                return this.createdon.ToString("dd/MM/yyyy");
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
                        return "In Progress";
                    case 2:
                        return "On Hold";
                    case 3:
                        return "Waiting for Details";
                    case 4:
                        return "Researching";
                    case 5:
                        return "Problem Solved";
                    case 1000:
                        return "Information Provided";
                    case 6:
                        return "Canceled";
                    case 2000:
                        return "Merged";
                    default:
                        return "";
                }
            }
        }
        public Guid incidentid { get; set; }
        public string case_nameaccount { get; set; }
        public string case_namecontact { get; set; }
        public string productname { get; set; }
        public string contactname
        {
            get
            {
                if (this.case_nameaccount != null)
                {
                    return this.case_nameaccount;
                }
                else if (this.case_namecontact != null)
                {
                    return this.case_namecontact;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}

