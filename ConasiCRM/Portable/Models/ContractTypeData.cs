using ConasiCRM.Portable.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConasiCRM.Portable.Models
{
    public class ContractTypeData
    {
        public static List<OptionSet> ContractTypes()
        {
            return new List<OptionSet>()
            {
                new OptionSet("100000000",Language.contract_long_term_lease_type), //Long term lease
                new OptionSet("100000001",Language.contract_local_SPA_type), //Local SPA
                new OptionSet("100000002",Language.contract_foreigner_SPA_type), //Foreigner SPA
                // contract_long_term_lease_type
                // contract_local_SPA_type
                // contract_foreigner_SPA_type
            };
        }
        public static OptionSet GetContractTypeById(string id)
        {
            return ContractTypes().SingleOrDefault(x => x.Val == id);
        }
    }
}
