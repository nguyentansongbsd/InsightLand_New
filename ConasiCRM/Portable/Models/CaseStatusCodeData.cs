using ConasiCRM.Portable.Resources;
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
                new OptionSet("1", Language.case_in_progress_sts), //In Progress case_in_progress_sts
                new OptionSet("2", Language.case_on_hold_sts), //On hold case_on_hold_sts
                new OptionSet("3", Language.case_waiting_for_details_sts), //Waiting for details case_waiting_for_details_sts
                new OptionSet("4", Language.case_researching_sts), //Researching case_researching_sts
                new OptionSet("5", Language.case_problem_solved_sts), //Problem Solved case_problem_solved_sts
                new OptionSet("1000", Language.case_information_provided_sts), //Information Provided case_information_provided_sts
                new OptionSet("6", Language.case_cancelled_sts), //Cancelled case_cancelled_sts
                new OptionSet("2000", Language.case_merged_sts), //Merged case_merged_sts
                 new OptionSet("0", "")

            };
        }

        public static OptionSet GetCaseStatusCodeById(string id)
        {
            return CaseStatusData().SingleOrDefault(x => x.Val == id);
        }
    }
}
