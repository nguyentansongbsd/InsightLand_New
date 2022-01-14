using ConasiCRM.Portable.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConasiCRM.Portable.Models
{
    public class ProjectTypeData
    {
        public static OptionSet GetProjectType(string projectType)
        {
            return ProjectTypes().SingleOrDefault(x => x.Val == projectType);
        }
        public static List<OptionSet> ProjectTypes()
        {
            return new List<OptionSet>()
            {
                new OptionSet("false",Language.project_residential_type), //Residential project_residential_type
                new OptionSet("true",Language.project_commercial_type), //Commercial  project_commercial_type
            };
        }
    }
}
