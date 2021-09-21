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

        public string unitId { get; set; }
        public string unitName { get; set; }

        public DateTime createdon { get; set; }
    }
}

