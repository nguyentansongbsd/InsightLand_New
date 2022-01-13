using ConasiCRM.Portable.Resources;
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
                new OptionSet("1",Language.gender_male_sts), //Male  gender_male_sts
                new OptionSet("2",Language.gender_female_sts), //Female  gender_female_sts
                new OptionSet("100000000",Language.gender_other_sts) //Other  gender_other_sts
            };
        }
        public static OptionSet GetGenderById(string Id)
        {
            GetGenders();
            if (Id != string.Empty)
            {
                OptionSet optionSet = GenderOptions.Single(x => x.Val == Id);
                return optionSet;
            }
            return null;
        }
    }
}
