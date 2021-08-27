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

        public List<Unit> Units { get; set; } = new List<Unit>();

        public new string Title
        {
            get
            {
                return $"{this.bsd_name} ({CountUnit.Total})";
            }
        }

        public CountUnit CountUnit { get; set; } = new CountUnit();

        public string NumChuanBiInFloor { get; set; }
        public string NumSanSangInFloor { get; set; }
        public string NumGiuChoInFloor { get; set; }
        public string NumDatCocInFloor { get; set; }
        public string NumDongYChuyenCoInFloor { get; set; }
        public string NumDaDuTienCocInFloor { get; set; }
        public string NumThanhToanDot1InFloor { get; set; }
        public string NumDaBanInFloor { get; set; }
    }
}
