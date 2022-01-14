using ConasiCRM.Portable.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.Models
{
    public class DirectionData
    {
        public static OptionSet GetDiretionById(string diretionId)
        {
            var diretion = Directions().Single(x=>x.Val == diretionId);
            return diretion;
        }

        public static List<OptionSetFilter> Directions()
        {
            var directions = new List<OptionSetFilter>();
            directions.Add(new OptionSetFilter {Val = "100000000",Label= Language.direction_east_sts });//East
            directions.Add(new OptionSetFilter { Val = "100000001", Label = Language.direction_west_sts });//West
            directions.Add(new OptionSetFilter { Val = "100000002", Label = Language.direction_south_sts });//South
            directions.Add(new OptionSetFilter { Val = "100000003", Label = Language.direction_north_sts });//North
            directions.Add(new OptionSetFilter { Val = "100000004", Label = Language.direction_north_west_sts });//North West
            directions.Add(new OptionSetFilter { Val = "100000005", Label = Language.direction_north_east_sts });//North East
            directions.Add(new OptionSetFilter { Val = "100000006", Label = Language.direction_south_west_sts });//South West
            directions.Add(new OptionSetFilter { Val = "100000007", Label = Language.direction_south_east_sts });//South East
            return directions;
            // direction_east_sts
            // direction_west_sts
            // direction_south_sts
            // direction_north_sts
            // direction_north_west_sts
            // direction_north_east_sts
            // direction_south_west_sts
            // direction_south_east_sts
        }
    }
}
