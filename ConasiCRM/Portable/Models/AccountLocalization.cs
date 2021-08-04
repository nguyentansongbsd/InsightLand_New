using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class AccountLocalization
    {
        public static List<OptionSet> LocalizationOptions;

        public static void Localizations()
        {
            LocalizationOptions = new List<OptionSet>()
            {
                new OptionSet("100000000", "Trong nước"),
                new OptionSet("100000001", "Nước ngoài")
            };
        }
        public static string GetLocalizationById(string listId)
        {
            Localizations();
            if (listId != string.Empty)
            {
                List<string> list = new List<string>();
                var ids = listId.Split(',');
                foreach (var item in ids)
                {
                    OptionSet optionSet = LocalizationOptions.Single(x => x.Val == item);
                    list.Add(optionSet.Label);
                }
                return string.Join(", ", list);
            }
            return null;
        }
    }
}
