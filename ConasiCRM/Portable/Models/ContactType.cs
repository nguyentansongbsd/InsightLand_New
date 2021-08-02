using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.Models
{
    public class ContactType
    {
        private static List<OptionSet> TypeOptions;

        private static void GetTypes()
        {
            TypeOptions = new List<OptionSet>()
            {
                new OptionSet("100000000","Customer"),
                new OptionSet("100000001","Collaborator"),
                new OptionSet("100000002","Authorized"),
                new OptionSet("100000003","Legal Representative")
            };
        }
        public static string GetTypeById(string listId)
        {
            GetTypes();          
            if (listId != string.Empty)
            {
                List<string> listType = new List<string>();
                var ids = listId.Split(',');
                foreach (var item in ids)
                {
                    OptionSet optionSet = TypeOptions.Single(x => x.Val == item);
                    listType.Add(optionSet.Label);
                }
                return string.Join(", ", listType);
            }
            return null;
        }
    }
}
