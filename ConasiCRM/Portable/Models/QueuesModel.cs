﻿using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class QueuesModel : BaseViewModel, IComparer<QueuesModel> //, IComparable<QueuesModel>
    {
        public Guid opportunityid { get; set; }
        public string name { get; set; }
        public Guid customer_id { get; set; }
        public Guid project_id { get; set; }
        public string project_name { get; set; }
        public string contact_name { get; set; }
        public string account_name { get; set; }

        private DateTime _createdon;
        public DateTime createdon { get => _createdon.AddHours(7); set { _createdon = value; OnPropertyChanged(nameof(createdon)); } }

        private DateTime _bsd_queuingexpired;
        public DateTime bsd_queuingexpired { get => _bsd_queuingexpired.AddHours(7); set { _bsd_queuingexpired = value; OnPropertyChanged(nameof(bsd_queuingexpired)); } }
        public int statuscode { get; set; }
        public string statuscode_format { get { return QueuesStatusCodeData.GetQueuesById(statuscode.ToString()).Name; } }
        public string statuscode_color { get { return QueuesStatusCodeData.GetQueuesById(statuscode.ToString()).BackGroundColor; } }

        public string bsd_queuenumber { get; set; }
        public string customername
        {
            get { return contact_name ?? account_name ?? ""; }
        }
        public string telephone { get; set; }

        private DateTime _bsd_bookingtime;
        public DateTime bsd_bookingtime { get => _bsd_bookingtime.AddHours(7); set { _bsd_bookingtime = value; OnPropertyChanged(nameof(bsd_bookingtime)); } }
        public int compare_sts
        {
            get
            {
                if (statuscode == 100000000)
                    return 0;
                else if (statuscode == 100000002)
                    return 1;
                else return 2;
            }
        }

        //public int CompareTo(QueuesModel other)
        //{
        //    if (other == null)
        //    {
        //        // check sts
        //        if (this.compare_sts < other.compare_sts)
        //            return 1;
        //        else if (this.compare_sts > other.compare_sts)
        //            return -1;
        //        else
        //        {// check bookingtime if sts = sts
        //            if (this.bsd_bookingtime > other.bsd_bookingtime)
        //                return 1;
        //            else if (this.bsd_bookingtime < other.bsd_bookingtime)
        //                return -1;
        //            else
        //                return 0;
        //        }
        //    }
        //    else
        //        return 0;
        //}

        public int Compare(QueuesModel x, QueuesModel y)
        {
            if (x == null)
                return -1;
            if (y == null)
                return 1;
            // check sts
            if (x.compare_sts < y.compare_sts)
                return 1;
            else if (x.compare_sts > y.compare_sts)
                return -1;
            else
            {// check bookingtime if sts = sts
                if (x.bsd_bookingtime > y.bsd_bookingtime)
                    return 1;
                else if (x.bsd_bookingtime < y.bsd_bookingtime)
                    return -1;
                else
                    return 0;
            }
        }
    }
}
