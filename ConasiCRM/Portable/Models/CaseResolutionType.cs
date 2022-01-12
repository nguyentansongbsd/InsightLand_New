using ConasiCRM.Portable.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class CaseResolutionType
    {
        public static List<OptionSet> CaseResolutionTypeData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("5", Language.case_problemsolved_sts),
                new OptionSet("1000", Language.case_informationprovided_sts),
            };
        }
    }
}
