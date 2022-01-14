using ConasiCRM.Portable.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConasiCRM.Portable.Models
{
    public class ViewData
    {
        public static OptionSet GetViewById(string viewId)
        {
            var view = Views().SingleOrDefault(x => x.Val == viewId);
            return view;
        }

        public static List<OptionSet> Views()
        {
            return new List<OptionSet>() {
                new OptionSet("100000000",Language.view_welcome_court_sts),//Welcome court view_welcome_court_sts
                new OptionSet("100000001",Language.view_outdoor_children_playground_sts),//Outdoor Children Playground  view_outdoor_children_playground_sts
                new OptionSet("100000002",Language.view_sea_sts),//Sea view_sea_sts
                new OptionSet("100000003",Language.view_garden_sts),//Garden view_garden_sts
                new OptionSet("100000004",Language.view_pool_sts)//Pool view_pool_sts
                //100.000.005 //City
                //100.000.006 //City
                //100.000.007 //100.000.009 //River
                //100.000.008 //City
            };
        }
    }
}
