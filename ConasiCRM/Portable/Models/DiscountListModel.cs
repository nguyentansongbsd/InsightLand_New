using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class DiscountListModel : BaseViewModel
    {
        public Guid bsd_discountid { get; set; }
        public string bsd_name { get; set; }
        public int new_type { get; set; }
        public DateTime bsd_startdate { get; set; }
        public DateTime bsd_enddate { get; set; }
        public Guid bsd_project { get; set; }
        public string project_name { get; set; }
        public Guid bsd_phaseslaunch { get; set; }
        public string phaseslaunch_name { get; set; }
        public decimal bsd_highquantity { get; set; }
        public string bsd_highquantity_format { get => StringFormatHelper.FormatCurrency(bsd_highquantity); }
        public decimal bsd_lowquantity { get; set; }
        public string bsd_lowquantity_format { get => StringFormatHelper.FormatCurrency(bsd_lowquantity); }
        public string bsd_discountnumber { get; set; }
        public decimal bsd_amount { get; set; }
        public decimal bsd_percentage { get; set; }
        public string bsd_discounttype { get; set; }
        public string discounttype_format { get { return bsd_discounttype != string.Empty ? DiscountType.GetDiscountTypeById(bsd_discounttype)?.Name : null; } }
        public bool hide_discounttype { get => bsd_discounttype == "100000000" ? false : true; }
        public string value_format
        {
            get
            {
                if (new_type == 100000001)
                    return StringFormatHelper.FormatCurrency(bsd_amount) + " đ";
                else if (new_type == 100000000)
                    return StringFormatHelper.FormatPercent(bsd_percentage) + " %";
                else
                    return null;
            }
        }
        public string bsd_method_format
        {
            get
            {
                if (new_type == 100000001)
                    return "Amount";
                else if (new_type == 100000000)
                    return "Percent";
                else
                    return null;
            }
        }
        private ObservableCollection<OptionSet> _distcount_list;
        public ObservableCollection<OptionSet> distcount_list { get => _distcount_list; set { _distcount_list = value; OnPropertyChanged(nameof(distcount_list)); } }
    }
    public class DiscountType
    {
        public static List<StatusCodeModel> DiscountTypeData()
        {
            return new List<StatusCodeModel>()
            {
                new StatusCodeModel("100000000","General Discount","#FDC206"),
                new StatusCodeModel("100000001","Bulk Discount","#f1f1f1"),
                new StatusCodeModel("0","","#f1f1f1")
            };
        }

        public static StatusCodeModel GetDiscountTypeById(string id)
        {
            return DiscountTypeData().SingleOrDefault(x => x.Id == id);
        }
    }
}
