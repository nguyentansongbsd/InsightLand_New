using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class Block : BaseViewModel
    {
        public Guid bsd_blockid { get; set; }
        public string bsd_name { get; set; }
        public List<UnitCount> UnitCounts { get; set; }

        public IList<Floor> Floors { get; set; } = new ObservableCollection<Floor>();

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
