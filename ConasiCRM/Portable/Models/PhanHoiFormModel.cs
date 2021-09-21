using ConasiCRM.Portable.ViewModels;
using System;

namespace ConasiCRM.Portable.Models
{
    public class PhanHoiFormModel
    {
        public Guid incidentid { get; set; }
        public string title { get; set; }
        public string caseorigincode { get; set; }
        public string casetypecode { get; set; }
        public string description { get; set; }

        public string subjectId { get; set; }
        public string subjectTitle { get; set; }

        public string parentCaseId { get; set; }
        public string parentCaseTitle { get; set; }

        public string accountId { get; set; }
        public string accountName { get; set; }

        public string contactId { get; set; }
        public string contactName { get; set; }

        public int _caseorigincode;
        public int caseorigincode { get { return _caseorigincode; } set { _caseorigincode = value; OnPropertyChanged(nameof(caseorigincode)); } }

        public string _description;
        public string description { get { return _description; } set { _description = value; OnPropertyChanged(nameof(description)); } }

        public string _title;
        public string title { get { return _title; } set { _title = value; OnPropertyChanged(nameof(title)); } }

        public DateTime _createdon;
        public DateTime createdon { get { return _createdon; } set { _createdon = value; OnPropertyChanged(nameof(createdon)); } }

        public int _statuscode;
        public int statuscode { get { return _statuscode; } set { _statuscode = value; OnPropertyChanged(nameof(statuscode)); } }

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

        public string _customerid;
        public string customerid { get { return _customerid; } set { _customerid = value; OnPropertyChanged(nameof(customerid)); } }

        public string _logicalname;
        public string logicalname { get { return _logicalname; } set { _logicalname = value; OnPropertyChanged(nameof(logicalname)); } }

        public string __customerid_value;
        public string _customerid_value { get { return __customerid_value; } set { __customerid_value = value; OnPropertyChanged(nameof(_customerid_value)); } }

        public string _case_nameaccount;
        public string case_nameaccount
        {
            get { return _case_nameaccount; }
            set
            {
                _case_nameaccount = value;
                if (value != null)
                {
                    customerid = value;
                    logicalname = "accounts";
                }
                OnPropertyChanged(nameof(case_nameaccount));
            }
        }

        public string _case_namecontact;
        public string case_namecontact
        {
            get { return _case_namecontact; }
            set
            {
                _case_namecontact = value;
                if (value != null)
                {
                    customerid = value;
                    logicalname = "contacts";
                }
                OnPropertyChanged(nameof(case_namecontact));
            }
        }

        public string __productid_value;
        public string _productid_value { get { return __productid_value; } set { __productid_value = value; OnPropertyChanged(nameof(_productid_value)); } }

        public string _productname;
        public string productname { get { return _productname; } set { _productname = value; OnPropertyChanged(nameof(productname)); } }

        public string _contactname;
        public string contactname { get { return _contactname; } set { _contactname = value; OnPropertyChanged(nameof(contactname)); } }

        public string __primarycontactid_value;
        public string _primarycontactid_value { get { return __primarycontactid_value; } set { __primarycontactid_value = value; OnPropertyChanged(nameof(_primarycontactid_value)); } }


        public string _contractname;
        public string contractname { get { return _contractname; } set { _contractname = value; OnPropertyChanged(nameof(contractname)); } }

        public string _new_solution;
        public string new_solution { get { return _new_solution; } set { _new_solution = value; OnPropertyChanged(nameof(new_solution)); } }

        private string _bsd_name;
        public string bsd_name { get { return _bsd_name; } set { _bsd_name = value; OnPropertyChanged(nameof(bsd_name)); } }

        private string _accountid;
        public string accountid { get { return _accountid; } set { _accountid = value; OnPropertyChanged(nameof(accountid)); } }

        private string _bsd_fullname;
        public string bsd_fullname { get { return _bsd_fullname; } set { _bsd_fullname = value; OnPropertyChanged(nameof(bsd_fullname)); } }

        private string _contactid;
        public string contactid { get { return _contactid; } set { _contactid = value; OnPropertyChanged(nameof(contactid)); } }

        private string _productid;
        public string productid { get { return _productid; } set { _productid = value; OnPropertyChanged(nameof(productid)); } }

        private string _name;
        public string name { get { return _name; } set { _name = value; OnPropertyChanged(nameof(name)); } }

        public string customername
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

        public int casetypecode { get; set; }

        public string casetypecodevalue
        {
            get
            {
                switch (casetypecode)
                {
                    case 1:
                        return "Question";
                    case 2:
                        return "Problem";
                    case 3:
                        return "Request";
                    default:
                        return "";
                }
            }
        }
        public string parentcase_title { get; set; }
        
    }
}

