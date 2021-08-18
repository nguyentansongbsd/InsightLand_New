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
        public string bsd_name { get; set; }
        
        public int UnitCount { get; set; }

        public IList<Unit> Units { get; set; } = new ObservableCollection<Unit>();

        public new string Title
        {
            get
            {
                return $"{this.bsd_name} ({CountUnit.Total})";
            }
        }

        public CountUnit CountUnit { get; set; } = new CountUnit();

        public int NumChuanBiInFloor { get; set; }
        public int NumSanSangInFloor { get; set; }
        public int NumGiuChoInFloor { get; set; }
        public int NumDatCocInFloor { get; set; }
        public int NumDongYChuyenCoInFloor { get; set; }
        public int NumDaDuTienCocInFloor { get; set; }
        public int NumThanhToanDot1InFloor { get; set; }
        public int NumDaBanInFloor { get; set; }
    }
}
