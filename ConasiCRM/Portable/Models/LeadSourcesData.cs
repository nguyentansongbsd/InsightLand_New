using System;
using System.Collections.Generic;
using System.Linq;

namespace ConasiCRM.Portable.Models
{
    public class LeadSourcesData
    {
        public static List<OptionSet> GetListSources()
        {
            return new List<OptionSet>()
            {
                new OptionSet("1","Advertisement"),
                new OptionSet("2","Employee Referral"),
                new OptionSet("3","External Referral"),
                new OptionSet("4","Partner"),
                new OptionSet("5","Public Relations"),
                new OptionSet("6","Seminar"),
                new OptionSet("7","Trade Show"),
                new OptionSet("8","Web"),
                new OptionSet("9","Word of Mouth"),
                new OptionSet("10","Other"),
            };
        }

        public static OptionSet GetLeadSourceById(string Id)
        {
            return GetListSources().SingleOrDefault(x => x.Val == Id);
        }
    }
}
