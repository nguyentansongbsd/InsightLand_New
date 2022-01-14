using ConasiCRM.Portable.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConasiCRM.Portable.Models
{
    public class PropertyUsageTypeData
    {
        public static OptionSet GetPropertyUsageTypeById(string Id)
        {
            return PropertyUsageTypes().SingleOrDefault(x => x.Val == Id);
        }
        public static List<OptionSet> PropertyUsageTypes()
        {
            return new List<OptionSet>()
            {
                new OptionSet("1",Language.project_condo_usagetype), //Condo project_condo_usagetype
                new OptionSet("2",Language.project_apartment_usagetype), //Apartment project_apartment_usagetype
                new OptionSet("3",Language.project_townhouse_usagetype), //Townhouse project_townhouse_usagetype
            };
        }
    }
}
