using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    /// <summary>
    /// Model khi goi action Direct Sale tra ve.
    /// </summary>
    public class DirectSaleActionResponse
    {
        public string Result { get; set; }

        public DirectSaleActionSubResponse GetSubResponse()
        {
            return JsonConvert.DeserializeObject<DirectSaleActionSubResponse>(Result.Replace("tmp=", ""));
        }
    }

    public class DirectSaleActionSubResponse
    {
        public string type { get; set; }
        public string content { get; set; }
    }
}
