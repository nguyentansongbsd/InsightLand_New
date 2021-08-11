using System;
using System.Collections.Generic;
using System.Linq;

namespace ConasiCRM.Portable.Models
{
    public class QueuesStatusCodeData
    {
        public static OptionSet GetQueuesById(string id)
        {
            return GetQueuesData().SingleOrDefault(x => x.Val == id);
        }
        public static List<OptionSet> GetQueuesData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("1","Draft"),
                new OptionSet("2","On Hold"),
                new OptionSet("100000000","Queuing"),
                new OptionSet("100000001","Collected Queuing Fee"),
                new OptionSet("100000002","Waiting"),
                new OptionSet("100000003","Expired"),
                new OptionSet("100000004","Completed")
            };
        }
    }
}
