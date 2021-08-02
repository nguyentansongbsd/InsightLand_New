using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class OptionSet : BaseViewModel
    {
        private string _val;
        public string Val
        {
            get { return _val; }
            set
            {
                if (_val != value)
                {
                    _val = value;
                    OnPropertyChanged(nameof(Val));
                }
            }
        }
        private string _label;
        public string Label
        {
            get => _label;
            set
            {
                if (_label != value)
                {
                    _label = value;
                    OnPropertyChanged(nameof(Label));
                }
            }
        }

        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    OnPropertyChanged(nameof(Selected));
                }
            }
        }

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
