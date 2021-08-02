using Conasi.Portable.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.Models
{

    public class MasterDetailPage1MenuItem
    {
        public MasterDetailPage1MenuItem()
        {
            TargetType = typeof(PageNotFound);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
        public string Icon { get; set; } = "contact.png";

        public string Group { get; set; }
    }
}