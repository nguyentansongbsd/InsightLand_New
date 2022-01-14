using ConasiCRM.Portable.Resources;
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
                new OptionSet("100000000",Language.co_owner_spouse_relationship), //Spouse co_owner_spouse_relationship
                new OptionSet("100000001",Language.co_owner_child_relationship), //Child  co_owner_child_relationship
                new OptionSet("100000002",Language.co_owner_parent_relationship), //Parent  co_owner_parent_relationship
                new OptionSet("100000003",Language.co_owner_friend_relationship), //Friend  co_owner_rriend_relationship
                new OptionSet("100000004",Language.co_owner_other_relationship),  //Other  co_owner_other_relationship
            };
        }
        public static OptionSet GetRelationshipById(string id)
        {
            return RelationshipData().SingleOrDefault(x => x.Val == id);
        }
    }
}
