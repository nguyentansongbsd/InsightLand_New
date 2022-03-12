using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class FollowUpModel : BaseViewModel
    {
        public Guid bsd_followuplistid { get; set; }

        private string _name;
        public string bsd_name { get => _name; set { _name = value; OnPropertyChanged(nameof(bsd_name)); } }
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

        public Guid bsd_optionentry_id { get; set; }
        public string name_optionentry { get; set; }

        public Guid contact_id_oe { get; set; } // id khách hàng Option Entry
        public Guid account_id_oe { get; set; } // id khách hàng Option Entry
        public Guid contact_id_re { get; set; } // id khách hàng Reservation
        public Guid account_id_re { get; set; } // id khách hàng Reservation
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
        public decimal bsd_depositfee { get; set; }
        public decimal bsd_sellingprice { get; set; } // giá bán
        public string bsd_sellingprice_format { get => StringFormatHelper.FormatCurrency(bsd_sellingprice); }
        public decimal bsd_totalamount { get; set; } // tổng tiền
        public string bsd_totalamount_format { get => StringFormatHelper.FormatCurrency(bsd_totalamount); }
        public decimal bsd_totalamountpaid { get; set; } // tổng tiền thanh toán 
        public string bsd_totalamountpaid_format { get => StringFormatHelper.FormatCurrency(bsd_totalamountpaid); }
        public decimal bsd_totalforfeitureamount { get; set; } // tổng tiền phạt
        public string bsd_totalforfeitureamount_format { get => StringFormatHelper.FormatCurrency(bsd_totalforfeitureamount); }
        public decimal bsd_forfeitureamount { get; set; } // hoàn tiền
        public string bsd_forfeitureamount_format { get => StringFormatHelper.FormatCurrency(bsd_forfeitureamount); }
        public int bsd_takeoutmoney { get; set; } // phương thức phạt
        public string bsd_takeoutmoney_format { get { return FollowUpListTakeOutMoney.GetFollowUpListTakeOutMoneyById(bsd_takeoutmoney.ToString()).Name; } }
        public decimal bsd_forfeiturepercent { get; set; } // hoàn tiền
        public string bsd_forfeiturepercent_format { get => StringFormatHelper.FormatCurrency(bsd_forfeiturepercent); }
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

        public bool _bsd_terminateletter;// thư thanh lý
        public bool bsd_terminateletter { get => _bsd_terminateletter; set { _bsd_terminateletter = value; OnPropertyChanged(nameof(bsd_terminateletter)); } }
        public string bsd_terminateletter_format { get { return BoolToStringData.GetStringByBool(bsd_terminateletter); } }

        public bool _bsd_termination; // thanh lý
        public bool bsd_termination { get => _bsd_termination; set { _bsd_termination = value; OnPropertyChanged(nameof(bsd_termination)); } }
        public string bsd_termination_format { get { return BoolToStringData.GetStringByBool(bsd_termination); } }

        public bool _bsd_resell; // bán lại
        public bool bsd_resell { get => _bsd_resell; set { _bsd_resell = value; OnPropertyChanged(nameof(bsd_resell)); } }
        public string bsd_resell_format { get { return BoolToStringData.GetStringByBool(bsd_resell); } }
        public Guid phaseslaunch_id { get; set; }
        public string phaseslaunch_name { get; set; } // đợt mở bán
        public Guid bsd_collectionmeeting_id { get; set; }
        public string bsd_collectionmeeting_subject { get; set; } // cuộc họp
        public string bsd_description { get; set; } //bình luận và quyết định nội dung
        public string project_code { get; set; }
        public decimal bsd_totalforfeitureamount_calculator // tổng tiền phạt
        {
            get
            {
                if (bsd_takeoutmoney == 100000000 && bsd_forfeitureamount != 0)
                {
                    var totalforfeiture = bsd_depositfee - bsd_forfeitureamount;
                    return totalforfeiture;
                }
                else
                {
                    if (bsd_takeoutmoney == 100000001 && bsd_forfeiturepercent != 0)
                    {

                        var totalforfeiture = (bsd_depositfee * bsd_forfeitureamount) / 100;
                        return totalforfeiture;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }
}
