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
                new StatusCodeModel("1","Active","#03ACF5"), // nháp
                new StatusCodeModel("100000000","Complete","#06CF79"), // hoàn thành
                new StatusCodeModel("2","Inactive","#FDC206"), // vô hiệu lực
                new StatusCodeModel("0","","#333333")
            };
        }

        public static StatusCodeModel GetFollowUpStatusCodeById(string id)
        {
            return FollowUpStatusCodeData().SingleOrDefault(x => x.Id == id);
        }
    }
}
