using ConasiCRM.Portable.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class FollowUpListTakeOutMoney
    {
        public static List<StatusCodeModel> FollowUpListTakeOutMoneyData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("100000000", Language.ful_refund_takeoutmoney,"#03ACF5"),
                new StatusCodeModel("100000001",Language.ful_forfeiture_takeoutmoney,"#06CF79"),
                new StatusCodeModel("0","","#333333")
            };
        }

        public static StatusCodeModel GetFollowUpListTakeOutMoneyById(string id)
        {
            return FollowUpListTakeOutMoneyData().SingleOrDefault(x => x.Id == id);
        }
    }
}
