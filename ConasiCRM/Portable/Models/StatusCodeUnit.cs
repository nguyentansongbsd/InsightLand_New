using ConasiCRM.Portable.Resources;
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
                new StatusCodeModel("0",Language.unit_draft_sts,"#333333"), //Draft unit_draft_sts
                new StatusCodeModel("1",Language.unit_preparing_sts,"#FDC206"),//Preparing unit_preparing_sts
                new StatusCodeModel("100000000",Language.unit_available_sts,"#06CF79"),//Available unit_available_sts
                new StatusCodeModel("100000004",Language.unit_queuing_sts,"#03ACF5"),//Queuing unit_queuing_sts
                new StatusCodeModel("100000006",Language.unit_reserve_sts,"#04A388"),//Reserve unit_reserve_sts
                new StatusCodeModel("100000005",Language.unit_collected_sts,"#9A40AB"),//Collected unit_collected_sts
                new StatusCodeModel("100000003",Language.unit_deposited_sts,"#FA7901"),//Deposited unit_deposited_sts
                new StatusCodeModel("100000001",Language.unit_1st_installment_sts,"#808080"),//1st Installment unit_1st_installment_sts
                new StatusCodeModel("100000002",Language.unit_sold_sts,"#D42A16"),//Sold unit_sold_sts
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
