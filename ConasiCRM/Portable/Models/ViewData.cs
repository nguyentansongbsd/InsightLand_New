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
                new OptionSet("100000000","Tòa chào mừng"),
                new OptionSet("100000001","Sân chơi trẻ em ngoài trời"),
                new OptionSet("100000002","Biển"),
                new OptionSet("100000003","Garden"),
                new OptionSet("100000004","Pool")
            };
        }
    }
}
