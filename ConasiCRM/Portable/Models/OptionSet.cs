using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class OptionSet : BaseViewModel
    {
        public string Val { get; set; }
        public string Label { get; set; }
        public bool Selected { get; set; }
        public bool IsMultiple { get; set; }

        public OptionSet()
        {

        }

        public OptionSet(string val, string label, bool selected = false)
        {
            Val = val;
            Label = label;
            Selected = selected;
        }
    }
}
