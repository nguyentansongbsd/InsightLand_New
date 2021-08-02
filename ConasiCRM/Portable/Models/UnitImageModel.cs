using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class UnitImageModel
    {
        public string ImageUrl { get; set; }

        public UnitImageModel(string url)
        {
            this.ImageUrl = url;
        }
    }
}
