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
                new StatusCodeModel("1","Active","#06CF79"),
                new StatusCodeModel("100000000","Not Paid","#03ACF5"), 
                new StatusCodeModel("100000001","Paid","#FDC206"), 
                new StatusCodeModel("2","Inactive","#FA7901"),
                new StatusCodeModel("0","","#f1f1f1")
            };
        }

        public static StatusCodeModel GetInstallmentsStatusCodeById(string id)
        {
            return InstallmentsStatusData().SingleOrDefault(x => x.Id == id);
        }
    }
}
