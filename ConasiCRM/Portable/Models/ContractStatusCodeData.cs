using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class ContractStatusCodeData
    {
        public static List<StatusCodeModel> ContractStatusData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("100000001","1st Installment","#F43927"),
                new StatusCodeModel("100000003","Being Payment","#F43927"),
                new StatusCodeModel("4","Canceled","#8bce3d"),
                new StatusCodeModel("100001","Complete","#FF8F4F"),
                new StatusCodeModel("100000004","Complete Payment","#FF8F4F"),
                new StatusCodeModel("100000007","Converted","#FF8F4F"),
                new StatusCodeModel("100000005","Handover","#FF8F4F"),
                new StatusCodeModel("3","In Progress","#FF8F4F"),
                new StatusCodeModel("100003","Invoiced","#FF8F4F"),
                new StatusCodeModel("1","Open","#FF8F4F"),
                new StatusCodeModel("100000000","Option","#ffc43d"),
                new StatusCodeModel("100002","Partial","#FF8F4F"),
                new StatusCodeModel("2","Pending","#FF8F4F"),
                new StatusCodeModel("100000002","Signed Contract","#FF8F4F"),
                new StatusCodeModel("100000006","Terminated","#c4c4c4"),
                new StatusCodeModel("0","","#bfbfbf"),
            };
        }

        public static StatusCodeModel GetContractStatusCodeById(string id)
        {
            return ContractStatusData().SingleOrDefault(x => x.Id == id);
        }
    }
}
