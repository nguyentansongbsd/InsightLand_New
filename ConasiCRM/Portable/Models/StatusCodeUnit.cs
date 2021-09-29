using System;
using System.Collections.Generic;
using System.Linq;

namespace ConasiCRM.Portable.Models
{
    public class StatusCodeUnit
    {
        public static StatusCodeModel GetStatusCodeById(string statusCodeId)
        {
            return StatusCodes().SingleOrDefault(x => x.Id == statusCodeId);
        }

        public static List<StatusCodeModel> StatusCodes()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("0","Nháp","#333333"),
                new StatusCodeModel("1","Chuẩn bị","#FDC206"),
                new StatusCodeModel("100000000","Sẵn sàng","#06CF79"),
                new StatusCodeModel("100000004","Giữ chỗ","#03ACF5"),
                new StatusCodeModel("100000006","Đặt cọc","#04A388"),
                new StatusCodeModel("100000005","Đồng ý chuyển cọc","#9A40AB"),
                new StatusCodeModel("100000003","Đã đủ tiền cọc","#FA7901"),
                new StatusCodeModel("100000001","Thanh toán đợt 1","#808080"),
                new StatusCodeModel("100000002","Đã bán","#D42A16"),
            };
        }
    }

    public class StatusCodeModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Background { get; set; }
        public StatusCodeModel(string id,string name,string background)
        {
            Id = id;
            Name = name;
            Background = background;
        }
    }
}
