﻿using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class FollowUpModel : BaseViewModel
    {
        public Guid bsd_followuplistid { get; set; }
        public string bsd_name { get; set; }
        public DateTime createdon { get; set; }

        public DateTime? _bsd_expiredate; // ngày hết hạn
        public DateTime? bsd_expiredate
        {
            get
            {
                if (_bsd_expiredate.HasValue)
                    return _bsd_expiredate.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_expiredate = value;
                    OnPropertyChanged(nameof(bsd_expiredate));
                }
            }
        } 
        public int statuscode { get; set; }
        public string bsd_followuplistcode { get; set; }
        public Guid product_id { get; set; }
        public string bsd_units { get; set; }
        public Guid bsd_reservation_id { get; set; }
        public string name_reservation { get; set; } // đặt cọc
        public string contact_name_oe { get; set; } // khách hàng Option Entry
        public string account_name_oe { get; set; } // khách hàng Option Entry
        public string contact_name_re { get; set; } // khách hàng Reservation
        public string account_name_re { get; set; } // khách hàng Reservation
        public string customer 
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(contact_name_oe))
                    return contact_name_oe;
                else if (!string.IsNullOrWhiteSpace(account_name_oe))
                    return account_name_oe;
                else if (!string.IsNullOrWhiteSpace(contact_name_re))
                    return contact_name_re;
                else
                    return account_name_re;
            }
        }
        public string statuscode_format { get { return FollowUpStatusData.GetFollowUpStatusCodeById(statuscode.ToString()).Name; } }
        public string statuscode_color { get { return FollowUpStatusData.GetFollowUpStatusCodeById(statuscode.ToString()).Background; } }
        public int bsd_type { get; set; }
        public string bsd_type_format { get { return FollowUpType.GetFollowUpTypeById(bsd_type.ToString()).Name; } }
        public int bsd_terminationtype { get; set; }
        public string bsd_terminationtype_format { get { return FollowUpTerminationType.GetFollowUpTerminationTypeById(bsd_terminationtype.ToString()).Name; } }
        public int bsd_group { get; set; }
        public string bsd_group_format { get { return FollowUpGroup.GetFollowUpGroupById(bsd_group.ToString()).Name; } }
        public Guid project_id { get; set; }
        public string project_name { get; set; }
        public decimal bsd_sellingprice { get; set; } // giá bán
        public decimal bsd_totalamount { get; set; } // tổng tiền
        public decimal bsd_totalamountpaid { get; set; } // tổng tiền thanh toán 
        public decimal bsd_totalforfeitureamount { get; set; } // tổng tiền phạt
        public decimal bsd_forfeitureamount { get; set; } // hoàn tiền
        public int bsd_takeoutmoney { get; set; } // phương thức phạt
        public string bsd_takeoutmoney_format 
        { get 
            {
                if (bsd_takeoutmoney == 100000001)
                    return "Forfeiture";
                else if (bsd_takeoutmoney == 100000000)
                    return "Refund";
                else
                    return "";
            } 
        }
        public decimal bsd_forfeiturepercent { get; set; } // hoàn tiền
        public bool isRefund
        {
            get
            {
                if (bsd_forfeitureamount != 0)
                    return true;
                else
                    return false;
            }
        }
        public bool isForfeiture
        {
            get
            {
                if (bsd_forfeiturepercent != 0)
                    return true;
                else
                    return false;
            }
        }
        public bool bsd_terminateletter { get; set; } // thư thanh lý
        public string bsd_terminateletter_format { get { return BoolToStringData.GetStringByBool(bsd_terminateletter); } }
        public bool bsd_termination { get; set; } // thanh lý
        public string bsd_termination_format { get { return BoolToStringData.GetStringByBool(bsd_termination); } }
        public bool bsd_resell { get; set; } // bán lại
        public string bsd_resell_format { get { return BoolToStringData.GetStringByBool(bsd_resell); } }
        public string phaseslaunch_name { get; set; } // đợt mở bán
        public Guid bsd_collectionmeeting_id { get; set; }
        public string bsd_collectionmeeting_subject { get; set; } // cuộc họp
        public string bsd_description { get; set; } //bình luận và quyết định nội dung
    }
}