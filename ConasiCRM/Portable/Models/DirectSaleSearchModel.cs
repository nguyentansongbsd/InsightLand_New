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
        public string Directions { get; set; }
        public string UnitStatuses { get; set; }
        public string NetArea { get; set; }
        public string Price { get; set; }
        public DirectSaleSearchModel(string projectId, string phasesLanchId, bool? isEvent = null, string unitCode = null, string directions = null, string unitStatuses = null, string netArea = null, string price = null)
        {
            ProjectId = projectId;
            PhasesLanchId = phasesLanchId;
            IsEvent = isEvent;
            UnitCode = unitCode;
            Directions = directions;
            UnitStatuses = unitStatuses;
            NetArea = netArea;
            Price = price;
        }
    }
}
