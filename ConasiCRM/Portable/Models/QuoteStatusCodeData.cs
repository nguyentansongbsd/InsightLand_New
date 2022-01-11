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
                new StatusCodeModel("0","","#FFFFFF"),
                new StatusCodeModel("1","Đang xử lý","#808080"),//In Progress
                new StatusCodeModel("2","Đang xử lý","#808080"),
                new StatusCodeModel("3","Đã đủ tiền cọc","#808080"), //Deposited
                new StatusCodeModel("4","Thành công","#8bce3d"),
                new StatusCodeModel("5","Mất khách hàng","#808080"),//Lost
                new StatusCodeModel("6","Đã hủy","#808080"), // đã hủy
                new StatusCodeModel("7","Revised","#808080"),//Revised

                new StatusCodeModel("100000000","Đặt cọc","#ffc43d"), // Reservation
                new StatusCodeModel("100000001","Đã thanh lý","#F43927"), // Terminated
                new StatusCodeModel("100000002","Đang chờ hủy bỏ tiền gửi","#808080"), //Pending Cancel Deposit
                new StatusCodeModel("100000003","Từ chối","#808080"),//Reject
                new StatusCodeModel("100000004","Signed RF","#808080"), //Signed RF
                new StatusCodeModel("100000005","Expired of signing RF","#808080"),//Expired of signing RF
                new StatusCodeModel("100000006","Đồng ý chuyển cọc","#808080"), //Collected
                new StatusCodeModel("100000007","Báo giá","#FF8F4F"), //Quotation
                new StatusCodeModel("100000008","Expired Quotation","#808080"),//Expired Quotation
                new StatusCodeModel("100000009","Hết hạn","#B3B3B3"), // ~ Het han 
            };
        }

        public static StatusCodeModel GetQuoteStatusCodeById(string id)
        {
            return QuoteStatusData().SingleOrDefault(x => x.Id == id);
        }
    }
}
