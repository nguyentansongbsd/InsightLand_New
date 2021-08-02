using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class Test
    {
        public string name { get; set; }
        public IList<Unit> Units { get; set; } = new ObservableCollection<Unit>();
    }
}
