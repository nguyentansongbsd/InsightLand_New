using ConasiCRM.Portable.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class BoolToStringData
    {       
        public static string GetStringByBool(bool _bool)
        {
            if(_bool)
            {
                return Language.string_co_sts;
            }   
            else
            {
                return Language.string_khong_sts;
            }    
        }
    }
}
