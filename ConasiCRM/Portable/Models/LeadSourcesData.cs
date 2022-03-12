using ConasiCRM.Portable.Resources;
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
                new OptionSet("1",Language.lead_advertisement_source),//Advertisement
                new OptionSet("2",Language.lead_employee_referral_source),//Employee Referral
                new OptionSet("3",Language.lead_external_referral_source),//External Referral
                new OptionSet("4",Language.lead_partner_source),//Partner
                new OptionSet("5",Language.lead_public_relations_source),//Public Relations
                new OptionSet("6",Language.lead_seminar_source),//Seminar
                new OptionSet("7",Language.lead_trade_show_source),//Trade Show
                new OptionSet("8",Language.lead_web_source),//Web
                new OptionSet("9",Language.lead_word_of_mouth_source),//Word of Mouth
                new OptionSet("10",Language.lead_other_source),//Other
                //lead_advertisement_source
                //lead_employee_referral_source
                //lead_external_referral_source
                //lead_partner_source
                //lead_public_relations_source
                //lead_seminar_source
                //lead_trade_show_source
                //lead_web_source
                //lead_word_of_mouth_source
                //lead_other_source
            };
        }

        public static OptionSet GetLeadSourceById(string Id)
        {
            return GetListSources().SingleOrDefault(x => x.Val == Id);
        }
    }
}
