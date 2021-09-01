using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class DirectSaleSearchModel
    {
        public string Project { get; set; }
        public string Phase { get; set; }
        public bool? Event { get; set; }
        public string Unit { get; set; }
        public string Direction { get; set; }
        public string stsUnit { get; set; }
        public NetAreaDirectSaleModel Area { get; set; }
        public PriceDirectSaleModel Price { get; set; }
        public DirectSaleSearchModel(string projectId, string phasesLanchId, bool? isEvent = null, string unitCode = null, string directions = null, string unitStatuses = null, NetAreaDirectSaleModel netArea = null, PriceDirectSaleModel price = null)
        {
            Project = projectId;
            Phase = phasesLanchId;
            Event = isEvent;
            Unit = unitCode;
            Direction = directions;
            stsUnit = unitStatuses;
            Area = netArea;
            Price = price;
        }
    }
}
