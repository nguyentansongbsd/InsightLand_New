using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class DirectSaleSearchModel
    {
        public Guid ProjectId { get; set; }
        public Guid PhasesLanchId { get; set; }
        public bool IsEvent { get; set; }
        public bool IsCollapse { get; set; }
        public string View { get; set; }
        public string UnitCode { get; set; }
        public ObservableCollection<string> Directions { get; set; }
        public ObservableCollection<string> Views { get; set; }
        public ObservableCollection<string> UnitStatuses { get; set; }
        public decimal? minNetArea { get; set; }
        public decimal? maxNetArea { get; set; }
        public decimal? minPrice { get; set; }
        public decimal? maxPrice { get; set; }

        public DirectSaleSearchModel(Guid projectId, Guid phasesLanchId, 
            bool isEvent,bool isCollapse, string view,string UnitCode, ObservableCollection<string> Directions,
            ObservableCollection<string> Views, ObservableCollection<string> UnitStatuses, 
            decimal? minNetArea, decimal? maxNetArea, decimal? minPrice, decimal? maxPrice)
        {
            ProjectId = projectId;
            PhasesLanchId = phasesLanchId;
            IsEvent = isEvent;
            IsCollapse = isCollapse;
            View = view;
            this.UnitCode = UnitCode;
            this.Directions = Directions;
            this.Views = Views;
            this.UnitStatuses = UnitStatuses;
            this.minNetArea = minNetArea;
            this.maxNetArea = maxNetArea;
            this.minPrice = minPrice;
            this.maxPrice = maxPrice;
        }
    }
}
