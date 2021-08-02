using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class Floor : BaseViewModel
    {
        public Guid bsd_floorid { get; set; }
        private string _bsd_name;
        public string bsd_name
        {
            get
            {
                try
                {
                    if (_bsd_name != null)
                    {
                        int index = _bsd_name.LastIndexOf('-');
                        return _bsd_name.Substring(index + 1);
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception ex)
                {
                    string a = ex.Message;
                    Debug.Write("substring floor name : " + a);
                    return _bsd_name;
                }
            }
            set
            {
                _bsd_name = value;
            }
        }

        public int UnitCount { get; set; }

        public IList<Test> Tests { get; set; } = new ObservableCollection<Test>() { };

        public IList<Unit> Units { get; set; } = new ObservableCollection<Unit>();

        public new string Title
        {
            get
            {
                return $"{this.bsd_name} ({CountUnit.Total})";
            }
        }

        public CountUnit CountUnit { get; set; } = new CountUnit();
    }
}
