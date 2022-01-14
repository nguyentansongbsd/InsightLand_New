using ConasiCRM.Portable.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class FollowUpStatusData
    {
        public static List<StatusCodeModel> FollowUpStatusCodeData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("1",Language.ful_active_sts,"#03ACF5"), // nháp Active
                new StatusCodeModel("100000000",Language.ful_complete_sts,"#06CF79"), // Complete
                new StatusCodeModel("2",Language.ful_inactive_sts,"#FDC206"), // Inactive
                new StatusCodeModel("0","","#333333")
                // ful_active_sts
                 // ful_complete_sts
                  // ful_inactive_sts
            };
        }

        public static StatusCodeModel GetFollowUpStatusCodeById(string id)
        {
            return FollowUpStatusCodeData().SingleOrDefault(x => x.Id == id);
        }
    }
}
