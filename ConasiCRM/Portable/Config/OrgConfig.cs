using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Config
{
    public class OrgConfig
    {
        public static int RecordPerPage = 30;

        public const string ApiUrl = "https://conasicrm.api.crm5.dynamics.com/api/data/v9.1";
        //public const string ApiUrl = "https://cnssb.api.crm5.dynamics.com/api/data/v9.1";

        public const string SharePointResource = "https://conasivn.sharepoint.com";
        public const string SharePointSiteName = "Conasi";

        public const string LinkLogin = "https://login.microsoftonline.com/b8ff1d2e-28ba-44e6-bf5b-c96188196711/oauth2/token";
        //public const string Resource = "https://cnssb.crm5.dynamics.com";
        public const string Resource = "https://conasicrm.crm5.dynamics.com/";
        public const string ClientId = "bbdc1207-6048-415a-a21c-02a734872571";
        public const string ClientSecret = "_~~NDM9PVbrSD22Ef-.qRnxioPHcG5xsJ8";
        public const string UserName = "bsddev@conasi.vn";
        public const string Password = "admin123$5";
    }
}
