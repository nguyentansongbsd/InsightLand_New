using System;
using System.Collections.Generic;
using System.Linq;

namespace ConasiCRM.Portable.Models
{
    public class QuoteStatusCodeData
    {
        public static List<StatusCodeModel> QuoteStatusData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("100000007","Báo giá","#FF8F4F"),
                new StatusCodeModel("4","Won","#8bce3d"),
                new StatusCodeModel("100000001","Terminated","#F43927"), // đã thanh lý
                new StatusCodeModel("6","Canceled","#c4c4c4"), // đã hủy
                new StatusCodeModel("100000000","Reservation","#ffc43d"), // ~đặt cọc 
            };
        }

        public static StatusCodeModel GetQuoteStatusCodeById(string id)
        {
            return QuoteStatusData().SingleOrDefault(x => x.Id == id);
        }
    }
}
