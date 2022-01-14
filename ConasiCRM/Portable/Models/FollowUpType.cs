using ConasiCRM.Portable.Resources;
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
                new StatusCodeModel("100000002",Language.ful_option_entry_1st_installment_type,"#FDC206"),
                new StatusCodeModel("100000003",Language.ful_option_entry_contract_type,"#06CF79"),
                new StatusCodeModel("100000004",Language.ful_option_entry_installments_type,"#03ACF5"),
                new StatusCodeModel("100000006",Language.ful_option_entry_terminate_type,"#04A388"),
                new StatusCodeModel("100000001",Language.ful_reservation_deposited_type,"#9A40AB"),
                new StatusCodeModel("100000000",Language.ful_reservation_sign_off_RF_type,"#FA7901"),
                new StatusCodeModel("100000005",Language.ful_reservation_terminate_type,"#808080"),
                new StatusCodeModel("100000007",Language.ful_units_type,"#D42A16"),
                new StatusCodeModel("0","","#333333"),
            };
        }

        public static StatusCodeModel GetFollowUpTypeById(string id)
        {
            return FollowUpTypeData().SingleOrDefault(x => x.Id == id);
        }
    }
}
