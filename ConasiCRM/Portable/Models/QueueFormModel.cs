using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class QueueFormModel : BaseViewModel
    {
        public string bsd_queuenumber { get; set; }
        public string name { get; set; }

        /// <summary>
        /// Su dung contact_id + contact_name khi lay du lieu cua queue ve tu form update.
        /// khi tu direct sale qua thi ko su dug thong tin nay.
        /// tuong tu cho account.
        /// </summary>
        public Guid contact_id { get; set; }
        public string contact_name { get; set; }

        public Guid account_id { get; set; }
        public string account_name { get; set; }

        public Guid bsd_customerreferral_account_id { get; set; }
        public string bsd_customerreferral_name { get; set; }
        public Guid bsd_salesagentcompany_account_id { get; set; }
        public string bsd_salesagentcompany_name { get; set; }
        public Guid bsd_collaborator_contact_id { get; set; }
        public string bsd_collaborator_name { get; set; }

        public int statuscode { get; set; } // chi su dung trong form update.

        public DateTime createdon { get; set; } // Thời gian đặt chỗ 

        public DateTime bsd_queuingexpired { get; set; } // Thời gian hết hạn

        public Guid bsd_project_id { get; set; }
        public string bsd_project_name { get; set; } // dự án

        public Guid bsd_phaseslaunch_id { get; set; }
        public string bsd_phaseslaunch_name { get; set; }
        public Guid bsd_discountlist_id { get; set; } // lấy về kèm theo khi lấy phaselaucn mục đích dùng để đưa qua đặt cọc.

        public Guid bsd_block_id { get; set; }
        public string bsd_block_name { get; set; }

        public Guid bsd_floor_id { get; set; }
        public string bsd_floor_name { get; set; }

        public Guid bsd_units_id { get; set; }
        public string bsd_units_name { get; set; }

        public Guid pricelist_id { get; set; }
        public string pricelist_name { get; set; }

        public decimal constructionarea { get; set; } // diện tích xây dựng , tên gốc bsd_constructionarea => đổi lại tránh trùng khi trong form update khi lấy thông tin về.

        public decimal netsaleablearea { get; set; } // diện tích sử dụng  , tên gốc bsd_netsaleablearea => đổi lại tránh trùng khi trong form update khi lấy thông tin về.

        public bool bsd_collectedqueuingfee { get; set; } // Đã nhận tiền

        public decimal bsd_queuingfee { get; set; } // phí đặt chỗ

        public decimal landvalue { get; set; } // giá trị đất

        public decimal unit_price { get; set; } // Giá bán , tên gốc price => đổi lại tránh trùng khi trong form update khi lấy thông tin về.
    }
}
