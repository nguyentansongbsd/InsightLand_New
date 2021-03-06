using ConasiCRM.Portable.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class FollowUpGroup
    {
        public static List<StatusCodeModel> FollowUpGroupData()
        {
            return new List<StatusCodeModel>()
            {   
                new StatusCodeModel("100000001",Language.ful_ccr_group,"#FDC206"), //ful_ccr_group
                new StatusCodeModel("100000002",Language.ful_fin_group,"#06CF79"), //ful_fin_group
                new StatusCodeModel("100000000",Language.ful_s_m_group,"#06CF79"), //ful_s&m_group
                new StatusCodeModel("0","","#333333"),
            };
        }

        public static StatusCodeModel GetFollowUpGroupById(string id)
        {
            return FollowUpGroupData().SingleOrDefault(x => x.Id == id);
        }
    }
}
