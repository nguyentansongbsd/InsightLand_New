﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ConasiCRM.Portable.Models
{
    public class QueuesStatusCodeData
    {
        public static QueuesStatusCodeModel GetQueuesById(string id)
        {
            return GetQueuesData().SingleOrDefault(x => x.Id == id);
        }
        public static List<QueuesStatusCodeModel> GetQueuesData()
        {
            return new List<QueuesStatusCodeModel>()
            {
                new QueuesStatusCodeModel("1","Nháp","#808080"), // Draft
                new QueuesStatusCodeModel("2","On Hold","#808080"), //?????
                new QueuesStatusCodeModel("3","Won","#808080"), //?????
                new QueuesStatusCodeModel("4","Đã hủy","#808080"),
                new QueuesStatusCodeModel("5","Out-Sold","#808080"), //?????
                new QueuesStatusCodeModel("100000000","Giữ chỗ","#00CF79"),
                new QueuesStatusCodeModel("100000001","Collected Queuing Fee","#808080"), //?????
                new QueuesStatusCodeModel("100000002","Đang đợi","#FDC206"),
                new QueuesStatusCodeModel("100000003","Hết hạn","#B3B3B3"),
                new QueuesStatusCodeModel("100000004","Hoàn thành","#C50147"),
                new QueuesStatusCodeModel("0","","#808080")
            };
        }
    }

    public class QueuesStatusCodeModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BackGroundColor { get; set; }
        public QueuesStatusCodeModel(string id,string name,string backGroundColor)
        {
            Id = id;
            Name = name;
            BackGroundColor = backGroundColor;
        }
    }
}
