using ConasiCRM.Portable.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConasiCRM.Portable.Models
{
    public class CaseTypeData
    {
        public static List<OptionSet> CasesData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("1",Language.case_question_type), // case_question_type
                new OptionSet("2",Language.case_problem_type), // case_problem_type
                new OptionSet("3",Language.case_request_type), // case_request_type
                 new OptionSet("0",""),
            };
        }

        public static OptionSet GetCaseById(string id)
        {
            return CasesData().SingleOrDefault(x => x.Val == id);
        }
    }
}