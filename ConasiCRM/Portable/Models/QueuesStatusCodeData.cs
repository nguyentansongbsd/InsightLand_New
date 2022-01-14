using ConasiCRM.Portable.Resources;
using System;
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
        public static List<QueuesStatusCodeModel> GetQueuesByIds(string ids)
        {
            List<QueuesStatusCodeModel> listQueue = new List<QueuesStatusCodeModel>();
            string[] Ids = ids.Split(',');
            foreach (var item in Ids)
            {
                listQueue.Add(GetQueuesById(item));
            }
            return listQueue;
        }
        public static List<QueuesStatusCodeModel> GetQueuesData()
        {
            return new List<QueuesStatusCodeModel>()
            {
                new QueuesStatusCodeModel("1",Language.queue_draft_sts,"#808080"), // Draft
                new QueuesStatusCodeModel("2",Language.queue_on_hold_sts,"#808080"), //On Hold
                new QueuesStatusCodeModel("3",Language.queue_won_sts,"#808080"), //Won
                new QueuesStatusCodeModel("4",Language.queue_canceled_sts,"#808080"), //Canceled
                new QueuesStatusCodeModel("5",Language.queue_out_sold_sts,"#808080"), //Out-Sold
                new QueuesStatusCodeModel("100000000",Language.queue_queuing_sts,"#00CF79"), //Queuing
                new QueuesStatusCodeModel("100000001",Language.queue_collected_queuing_fee_sts,"#808080"), //Collected queuing fee
                new QueuesStatusCodeModel("100000002",Language.queue_waiting_sts,"#FDC206"),//Waiting
                new QueuesStatusCodeModel("100000003",Language.queue_expired_sts,"#B3B3B3"),//Expired
                new QueuesStatusCodeModel("100000004",Language.queue_completed_sts,"#C50147"),//Completed
                new QueuesStatusCodeModel("0","","#808080")
                //queue_draft_sts
                //queue_on_hold_sts
                //queue_won_sts
                //queue_canceled_sts
                //queue_out_sold_sts
                //queue_queuing_sts
                //queue_collected_queuing_fee_sts
                //queue_waiting_sts
                //queue_expired_sts
                //queue_completed_sts
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
