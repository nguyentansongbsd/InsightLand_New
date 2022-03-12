using ConasiCRM.Portable.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class LeadStatusCodeData
    {
        public static List<StatusCodeModel> LeadStatusData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("0","","#FFFFFF"),
                new StatusCodeModel("1",Language.lead_new_sts,"#ffc43d"),//New
                new StatusCodeModel("2",Language.lead_contacted_sts,"#F43927"), //Contacted
                new StatusCodeModel("3",Language.lead_qualified_sts,"#8bce3d"), //Qualified
                new StatusCodeModel("4",Language.lead_lost_sts,"#808080"), //Lost
                new StatusCodeModel("5",Language.lead_cannot_contact_sts,"#808080"),//Cannot Contact
                new StatusCodeModel("6",Language.lead_no_longer_interested_sts,"#808080"), // No Longer Interested
                new StatusCodeModel("7",Language.lead_canceled_sts,"#808080"),//Canceled
            };
        }

        public static StatusCodeModel GetLeadStatusCodeById(string id)
        {
            return LeadStatusData().SingleOrDefault(x => x.Id == id);
        }
    }
}
