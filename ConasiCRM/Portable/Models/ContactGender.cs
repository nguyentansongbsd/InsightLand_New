using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class ContactGender
    {
        public static List<OptionSet> GenderOptions;

        public static void GetGenders()
        {
            GenderOptions = new List<OptionSet>()
            {
                new OptionSet("1","Nam"),
                new OptionSet("2","Nữ"),
                new OptionSet("100000000","Khác")
            };
        }
        public static string GetGenderById(string listId)
        {
            GetGenders();
            if (listId != string.Empty)
            {
                List<string> list = new List<string>();
                var ids = listId.Split(',');
                foreach (var item in ids)
                {
                    OptionSet optionSet = GenderOptions.Single(x => x.Val == item);
                    list.Add(optionSet.Label);
                }
                return string.Join(", ", list);
            }
            return null;
        }
    }
}
