using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class FollowUpType
    {
        public static List<StatusCodeModel> FollowUpTypeData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("100000002","Option Entry - 1st installment","#FDC206"),
                new StatusCodeModel("100000003","Option Entry - Contract","#06CF79"),
                new StatusCodeModel("100000004","Option Entry - Installments","#03ACF5"),
                new StatusCodeModel("100000006","Option Entry - Terminate","#04A388"),
                new StatusCodeModel("100000001","Reservation - Deposited","#9A40AB"),
                new StatusCodeModel("100000000","Reservation - Sign off RF","#FA7901"),
                new StatusCodeModel("100000005","Reservation - Terminate","#808080"),
                new StatusCodeModel("100000007","Units","#D42A16"),
                new StatusCodeModel("0","","#333333"),
            };
        }

        public static StatusCodeModel GetFollowUpTypeById(string id)
        {
            return FollowUpTypeData().SingleOrDefault(x => x.Id == id);
        }
    }
}
