using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class InstallmentsStatusCodeData
    {
        public static List<StatusCodeModel> InstallmentsStatusData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("1","Active","#06CF79"), // nháp
                new StatusCodeModel("100000000","Not Paid","#03ACF5"),  // chưa thnah toán
                new StatusCodeModel("100000001","Paid","#FDC206"),  // đã thah toán
                new StatusCodeModel("2","Inactive","#FA7901"), // vô hiệu lực
                new StatusCodeModel("0","","#f1f1f1")
            };
        }

        public static StatusCodeModel GetInstallmentsStatusCodeById(string id)
        {
            return InstallmentsStatusData().SingleOrDefault(x => x.Id == id);
        }
    }
}
