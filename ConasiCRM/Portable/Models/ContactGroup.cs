using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class ContactGroup
    {
        private static List<OptionSet> GroupOptions;

        private static void GetContactGroups()
        {
            GroupOptions = new List<OptionSet>()
            {
                new OptionSet("100000000","Ưu tiên (VIP)"),
                new OptionSet("100000001","An cư"),
                new OptionSet("100000002","Đầu tư"),
                new OptionSet("100000003","Đền bù")
            };
        }
        public static string GetContactGroupById(string listId)
        {
            GetContactGroups();          
            if (listId != string.Empty)
            {
                List<string> listType = new List<string>();
                var ids = listId.Split(',');
                foreach (var item in ids)
                {
                    OptionSet optionSet = GroupOptions.Single(x => x.Val == item);
                    listType.Add(optionSet.Label);
                }
                return string.Join(", ", listType);
            }
            return null;
        }
    }
}
