using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class DirectSaleSearchModel
    {
        public string ProjectId { get; set; }
        public string PhasesLanchId { get; set; }
        public bool? IsEvent { get; set; }
        public string UnitCode { get; set; }
        public List<string> Directions { get; set; }
        public List<string> Views { get; set; }
        public List<string> UnitStatuses { get; set; }
        public decimal? minNetArea { get; set; }
        public decimal? maxNetArea { get; set; }
        public decimal? minPrice { get; set; }
        public decimal? maxPrice { get; set; }
    }
}
