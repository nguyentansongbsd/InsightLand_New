using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class CaseStatusCodeData
    {
        public static List<OptionSet> CaseStatusData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("1", "In Progress"),
                new OptionSet("2", "On Hold"),
                new OptionSet("3", "Waiting for Details"),
                new OptionSet("4", "Researching"),
                new OptionSet("5", "Problem Solved"),
                new OptionSet("1000", "Information Provided"),
                new OptionSet("6", "Canceled"),
                new OptionSet("2000", "Merged"),
                 new OptionSet("0", "")

            };
        }

        public static OptionSet GetCaseStatusCodeById(string id)
        {
            return CaseStatusData().SingleOrDefault(x => x.Val == id);
        }
    }
}
