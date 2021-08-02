using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class ReservationFormModel : BaseViewModel
    {
        public Guid quoteid { get; set; }
        public string name { get; set; } // quotation description
        public string quotenumber { get; set; } // mã báo giá
        public string bsd_quotationnumber { get; set; } // số báo giá
        public int statuscode { get; set; }
        public bool bsd_xacnhanckmuasi { get; set; }

        // thong tin can ho
        public string unit_name { get; set; }
        public int unit_statuscode { get; set; }
        public decimal unit_netsaleablearea { get; set; }
        public decimal unit_constructionarea { get; set; }

        // thong tin ban hang
        public string queuenumber { get; set; } // ma phieu dat cho

        public Guid contact_id { get; set; }
        public string contact_name { get; set; }
        public Guid account_id { get; set; }
        public string account_name { get; set; }
        public string customer_name
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(contact_name))
                {
                    return contact_name;
                }
                else
                {
                    return account_name ?? "";
                }
            }
        }

        // chi tiet
        public Guid project_id { get; set; } // lay ra de lam dieu kien load handover condition
        public string project_name { get; set; } // du an
        public Guid phaseslaunch_id { get; set; }
        public string phaseslaunch_name { get; set; } // dotmoban
        public string pricelevel_name { get; set; } // bang gia

        public Guid paymentscheme_id { get; set; }
        public string paymentscheme_name { get; set; } //lich thanh toan

        public decimal tax { get; set; }//phan tram thue.
        public decimal bsd_bookingfee { get; set; } // phi dat cho
        public decimal bsd_depositfee { get; set; } // tien dat coc

        // thong tin gia tri.
        public decimal bsd_detailamount { get; set; } // gia ban
        public decimal bsd_discount { get; set; } // chiet khau
        public decimal bsd_packagesellingamount { get; set; } // phi ban giao Handover Condition Amount
        public decimal bsd_totalamountlessfreight { get; set; } // gia ban truoc thue 
        public decimal bsd_landvaluededuction { get; set; } // gia dat Land Value Deduction
        public decimal totaltax { get; set; } // thue Total VAT Tax
        public decimal bsd_freightamount { get; set; }// phi bao tri . Maintenance Fee
        public decimal totalamount { get; set; } //Total Amount

        // thong tin phi quan ly
        public int bsd_numberofmonthspaidmf { get; set; } // so thang tra phi quan ly
        public decimal bsd_managementfee { get; set; }// phi quan ly

        // thong tin chiet khau
        public Guid discountlist { get; set; }
        private string _bsd_discounts; // property nay duoc dung de luu lai thong tin khi chon discounts,cach nhau dau phay. luu ID. Id nao co thi checked.
        public string bsd_discounts
        {
            get => _bsd_discounts;
            set
            {
                _bsd_discounts = value;
                OnPropertyChanged(nameof(bsd_discounts));
            }
        }

        public Guid internaldiscountlist { get; set; }
        public string bsd_interneldiscount { get; set; }

        public Guid discountswholesalelist { get; set; }
        public string bsd_chietkhaumausiid { get; set; }

    }
}
