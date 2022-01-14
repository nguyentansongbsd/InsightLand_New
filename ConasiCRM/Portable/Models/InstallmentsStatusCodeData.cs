using ConasiCRM.Portable.Resources;
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
                new StatusCodeModel("1",Language.installments_active_sts,"#06CF79"), // installments_active_sts //Active
                new StatusCodeModel("100000000",Language.installments_not_paid_sts,"#03ACF5"),  // installments_not_paid_sts //Not Paid
                new StatusCodeModel("100000001",Language.installments_paid_sts,"#FDC206"),  // installments_paid_sts //Paid
                new StatusCodeModel("2",Language.installments_inactive_sts,"#FA7901"), // installments_inactive_sts
                new StatusCodeModel("0","","#f1f1f1")
            };
        }

        public static StatusCodeModel GetInstallmentsStatusCodeById(string id)
        {
            return InstallmentsStatusData().SingleOrDefault(x => x.Id == id);
        }
    }
}
