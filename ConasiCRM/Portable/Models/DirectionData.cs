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

        public static List<OptionSet> Directions()
        {
            var directions = new List<OptionSet>();
            directions.Add(new OptionSet("100000000", "Đông"));
            directions.Add(new OptionSet("100000001", "Tây"));
            directions.Add(new OptionSet("100000002", "Nam"));
            directions.Add(new OptionSet("100000003", "Bắc"));
            directions.Add(new OptionSet("100000004", "Tây bắc"));
            directions.Add(new OptionSet("100000005", "Đông bắc"));
            directions.Add(new OptionSet("100000006", "Tây nam"));
            directions.Add(new OptionSet("100000007", "Đông nam"));
            return directions;
        }
    }
}
