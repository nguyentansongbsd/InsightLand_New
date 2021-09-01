using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class OptionSet : BaseViewModel
    {
        public string Val { get; set; }

        public string _label;
        public string Label { get => _label; set { _label = value; OnPropertyChanged(nameof(Label)); } }

        public string _value;
        public string Value { get => _value; set { _value = value; OnPropertyChanged(nameof(Value)); } }

        private bool _selected;
        public bool Selected { get=>_selected; set { _selected = value;OnPropertyChanged(nameof(Selected)); } }
        public bool IsMultiple { get; set; }

        public OptionSet()
        {

        }

        public OptionSet(string val, string label,string value = null, bool selected = false)
        {
            Val = val;
            Label = label;
            Value = value;
            Selected = selected;
        }
    }
}
