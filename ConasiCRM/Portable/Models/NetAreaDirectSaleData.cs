using ConasiCRM.Portable.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConasiCRM.Portable.Models
{
    public class NetAreaDirectSaleData
    {
        public static List<NetAreaDirectSaleModel> NetAreaData()
        {
            return new List<NetAreaDirectSaleModel>()
            {
                new NetAreaDirectSaleModel("1",Language.string_under + " 60m²","60"),
                new NetAreaDirectSaleModel("2","60m² -> 80m²","60","80"),
                new NetAreaDirectSaleModel("3","81m² -> 100m²","81","100"),
                new NetAreaDirectSaleModel("4","101m² -> 120m²","101","120"),
                new NetAreaDirectSaleModel("5","121m² -> 150m²","121","150"),
                new NetAreaDirectSaleModel("6","151m² -> 180m²","151","180"),
                new NetAreaDirectSaleModel("7","211m² -> 240m²","211","240"),
                new NetAreaDirectSaleModel("8","241m² -> 270m²","241","270"),
                new NetAreaDirectSaleModel("9","271m²-> 300m²","271","300"),
                new NetAreaDirectSaleModel("10","301m² -> 350m²","301","350"),
                new NetAreaDirectSaleModel("11",Language.string_more_than + " 350m²",null,"350"),
            };
        }
        public static NetAreaDirectSaleModel GetNetAreaById(string Id)
        {
            return NetAreaData().SingleOrDefault(x => x.Id == Id);
        }
    }
    public class NetAreaDirectSaleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public NetAreaDirectSaleModel(string id,string name,string from = null,string to = null)
        {
            Id = id;
            Name = name;
            From = from;
            To = to;
        }
    }
}
