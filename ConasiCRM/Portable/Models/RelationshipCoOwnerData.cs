using System;
using System.Collections.Generic;
using System.Linq;

namespace ConasiCRM.Portable.Models
{
    public class RelationshipCoOwnerData
    {
        public static List<OptionSet> RelationshipData()
        {
            return new List<OptionSet>()
            {
                new OptionSet("100000000","Spouse"),
                new OptionSet("100000001","Child"),
                new OptionSet("100000002","Parent"),
                new OptionSet("100000003","Friend"),
                new OptionSet("100000004","Other"),
            };
        }
        public static OptionSet GetRelationshipById(string id)
        {
            return RelationshipData().SingleOrDefault(x => x.Val == id);
        }
    }
}
