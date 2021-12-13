﻿using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class ReservationDetailPageModel : BaseViewModel
    {
        public Guid quoteid { get; set; }

        private int _statuscode;
        public int statuscode { get => _statuscode; set { _statuscode = value; OnPropertyChanged(nameof(statuscode)); } }

        private int _statecode;
        public int statecode { get => _statecode; set { _statecode = value; OnPropertyChanged(nameof(statecode)); } }
        public string name { get; set; }
        public Guid purchaser_accountid { get; set; }  // id khach hang account
        public string purchaser_account_name { get; set; } // ten khach hang account
        public Guid purchaser_contactid { get; set; } // id khach hang contact
        public string purchaser_contact_name { get; set; } // ten khach hang contact
        public string bsd_reservationno { get; set; } // đặt cọc trang đầu
        public string bsd_quotationnumber { get; set; } // bảng tính giá trang đầu
        public string quotenumber { get; set; }
        public Guid unit_id { get; set; } // id unit trang đầu
        public string unit_name { get; set; } // tên unit trang đầu
        public string bsd_projectcode { get; set; }

        // chính sách
        public Guid bsd_discounttypeid { get; set; } // id discount

        private string _discountlist_name;// name discount 
        public string discountlist_name { get => _discountlist_name; set { _discountlist_name = value; OnPropertyChanged(nameof(discountlist_name)); } }
        public string bsd_discounts { get; set; } // id discounts 

        public Guid handovercondition_id; // id điều kiện bàn giao

        private string _handovercondition_name; // tên điều kiện bàn giao
        public string handovercondition_name { get => _handovercondition_name; set { _handovercondition_name = value; OnPropertyChanged(nameof(handovercondition_name)); } }
        public Guid paymentscheme_id { get; set; } // id phương thức thanh toán

        private string _paymentscheme_name; // tên phương thức thanh toán
        public string paymentscheme_name { get => _paymentscheme_name; set { _paymentscheme_name = value; OnPropertyChanged(nameof(paymentscheme_name)); } }

        // thông tin bán hàng 
        public Guid queue_id { get; set; } // id đặt chỗ
        public string queue_name { get; set; } // tên đặt chỗ
        public bool bsd_followuplist { get; set; } // danh sách theo dõi
        public string bsd_followuplist_format { get { return BoolToStringData.GetStringByBool(bsd_followuplist); } }
        // thông tin sản phẩm
        public int bsd_unitstatus { get; set; } // tình trạng sản phẩm
        public string bsd_unitstatus_format { get => StatusCodeUnit.GetStatusCodeById(bsd_unitstatus.ToString()).Name; }
        public decimal bsd_constructionarea { get; set; } // diện tích xây dựng
        public decimal bsd_netusablearea { get; set; } // diện tích sử dụng
        public decimal bsd_actualarea { get; set; } // diện tích thực

        // thông tin chi tiết
        public Guid project_id { get; set; } // id dự án
        public string project_name { get; set; } // tên dự án
        public int quotationvalidate { get; set; }
        public Guid phaseslaunch_id { get; set; } // id đợt mở bán
        public string phaseslaunch_name { get; set; } // tên đợt mở bán
        public Guid pricelevel_id_phaseslaunch { get; set; } // id bảng giá đmb
        public string pricelevel_name_phaseslaunch { get; set; } // tên bảng giá đmb
        public Guid pricelevel_id_apply { get; set; } // id bảng giá áp dụng
        public string pricelevel_name_apply { get; set; } // tên bảng giá áp dụng
        public Guid taxcode_id { get; set; } // id thuế
        public string taxcode_name { get; set; } // tên thuế
        public decimal bsd_bookingfee { get; set; } // phí giữ chỗ
        public decimal bsd_depositfee { get; set; } // phí đặt cọc 
        public int bsd_contracttypedescripton { get; set; } // loại hợp đồng 
        public string bsd_contracttypedescripton_format
        {
            get
            {
                var type = ContractTypeData.GetContractTypeById(bsd_contracttypedescripton.ToString());
                if (type != null)
                    return type.Label;
                else
                    return "";
            }
        }
        public decimal bsd_totalamountpaid { get; set; } // tổng tiền thanh toán 

        // thông tin báo giá
        public DateTime? _bsd_reservationtime; // thời gian đặt cọc
        public DateTime? bsd_reservationtime
        {
            get
            {
                if (_bsd_reservationtime.HasValue)
                    return _bsd_reservationtime.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_reservationtime = value;
                    OnPropertyChanged(nameof(bsd_reservationtime));
                }
            }
        }

        public DateTime? _bsd_deposittime; // ngày đặt cọc
        public DateTime? bsd_deposittime
        {
            get
            {
                if (_bsd_deposittime.HasValue)
                    return _bsd_deposittime.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_deposittime = value;
                    OnPropertyChanged(nameof(bsd_deposittime));
                }
            }
        }
        public Guid salescompany_accountid { get; set; }  // id đại lý/ sàn
        public string salescompany_account_name { get; set; } // tên đại lý/ sàn 
        public string bsd_nameofstaffagent { get; set; } // nhân viên đại lý/ sàn
        public string bsd_referral { get; set; } // giới thiệu

        // thông tin bảng tính giá
        public DateTime? _bsd_quotationprinteddate; // ngày in
        public DateTime? bsd_quotationprinteddate
        {
            get
            {
                if (_bsd_quotationprinteddate.HasValue)
                    return _bsd_quotationprinteddate.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_quotationprinteddate = value;
                    OnPropertyChanged(nameof(bsd_quotationprinteddate));
                }
            }
        }

        public DateTime? _bsd_expireddateofsigningqf; // ngày hết hạn ký
        public DateTime? bsd_expireddateofsigningqf
        {
            get
            {
                if (_bsd_expireddateofsigningqf.HasValue)
                    return _bsd_expireddateofsigningqf.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_expireddateofsigningqf = value;
                    OnPropertyChanged(nameof(bsd_expireddateofsigningqf));
                }
            }
        }

        public DateTime? _bsd_quotationsigneddate; // ngày ký
        public DateTime? bsd_quotationsigneddate
        {
            get
            {
                if (_bsd_quotationsigneddate.HasValue)
                    return _bsd_quotationsigneddate.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_quotationsigneddate = value;
                    OnPropertyChanged(nameof(bsd_quotationsigneddate));
                }
            }
        }

        // thông tin đặt cọc
        public int bsd_reservationformstatus { get; set; } // trạng thái pđc

        public DateTime? _bsd_reservationprinteddate; // ngày in
        public DateTime? bsd_reservationprinteddate
        {
            get
            {
                if (_bsd_reservationprinteddate.HasValue)
                    return _bsd_reservationprinteddate.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_reservationprinteddate = value;
                    OnPropertyChanged(nameof(bsd_reservationprinteddate));
                }
            }
        }
        public DateTime? _bsd_signingexpired; // ngày hết hạn ký
        public DateTime? bsd_signingexpired
        {
            get
            {
                if (_bsd_signingexpired.HasValue)
                    return _bsd_signingexpired.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_signingexpired = value;
                    OnPropertyChanged(nameof(bsd_signingexpired));
                }
            }
        }

        public DateTime? _bsd_rfsigneddate; // ngày ký
        public DateTime? bsd_rfsigneddate
        {
            get
            {
                if (_bsd_rfsigneddate.HasValue)
                    return _bsd_rfsigneddate.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_rfsigneddate = value;
                    OnPropertyChanged(nameof(bsd_rfsigneddate));
                }
            }
        }

        public DateTime? _bsd_reservationuploadeddate; // ngày tải lên pđc
        public DateTime? bsd_reservationuploadeddate
        {
            get
            {
                if (_bsd_reservationuploadeddate.HasValue)
                    return _bsd_reservationuploadeddate.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_reservationuploadeddate = value;
                    OnPropertyChanged(nameof(bsd_reservationuploadeddate));
                }
            }
        }

        // thông tin giá
        public decimal bsd_detailamount { get; set; } // giá gốc
        public decimal bsd_discount { get; set; } // chiết khấu
        public decimal bsd_packagesellingamount { get; set; } // đkbg
        public decimal bsd_totalamountlessfreight { get; set; } // giá bán thực
        public decimal bsd_landvaluededuction { get; set; } // giá trị qsdđ
        public decimal totaltax { get; set; } // thuế
        public decimal bsd_freightamount { get; set; } // phí bảo trì
        public decimal totalamount { get; set; } // tổng tiền

        // phí quản lý
        public decimal bsd_managementfee { get; set; } // phí quản lý 
        public int bsd_numberofmonthspaidmf { get; set; } // số tháng đóng phí
        public int bsd_waivermanafeemonth { get; set; } // miễn giảm

        // đã nhận tiền đặt cọc 
        public bool bsd_salesdepartmentreceiveddeposit { get; set; }  // nhận tiền đặt cọc 
        public string bsd_salesdepartmentreceiveddeposit_format { get { return BoolToStringData.GetStringByBool(bsd_salesdepartmentreceiveddeposit); } }

        public DateTime? _bsd_receiptdate; // ngày
        public DateTime? bsd_receiptdate
        {
            get
            {
                if (_bsd_receiptdate.HasValue)
                    return _bsd_receiptdate.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_receiptdate = value;
                    OnPropertyChanged(nameof(bsd_receiptdate));
                }
            }
        }
        public decimal bsd_depositfeereceived { get; set; } // số tiền

        // thông tin từ chối
        public DateTime? _bsd_rejectdate; // ngày
        public DateTime? bsd_rejectdate
        {
            get
            {
                if (_bsd_rejectdate.HasValue)
                    return _bsd_rejectdate.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_rejectdate = value;
                    OnPropertyChanged(nameof(bsd_rejectdate));
                }
            }
        }
        public string bsd_rejectreason { get; set; } // lý do

        // báo cáo bán hàng
        public DateTime? _bsd_calculatedforsalesreport; // ngày
        public DateTime? bsd_calculatedforsalesreport
        {
            get
            {
                if (_bsd_calculatedforsalesreport.HasValue)
                    return _bsd_calculatedforsalesreport.Value.AddHours(7);
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                {
                    _bsd_calculatedforsalesreport = value;
                    OnPropertyChanged(nameof(bsd_calculatedforsalesreport));
                }
            }
        }         
    }
}
