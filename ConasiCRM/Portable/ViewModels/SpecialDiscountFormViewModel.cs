using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ConasiCRM.Portable.ViewModels
{
    public class SpecialDiscountFormViewModel : FormLookupViewModel
    {
        public SpecicalDiscountFormModel _specialDiscount;
        public SpecicalDiscountFormModel SpecialDiscount
        {
            get => _specialDiscount;
            set
            {
                if (_specialDiscount != value)
                {
                    _specialDiscount = value;
                    OnPropertyChanged(nameof(SpecialDiscount));
                }
            }
        }

        public List<OptionSet> CachTinh_Options { get; set; }

        private OptionSet _cachTinh;
        public OptionSet CachTinh
        {
            get => _cachTinh;
            set
            {
                if (_cachTinh != value)
                {
                    _cachTinh = value;
                    OnPropertyChanged(nameof(CachTinh));
                }
            }
        }

        public LookUpConfig CreatedByConfig { get; set; }

        private LookUp _approver;
        public LookUp Approver
        {
            get => _approver;
            set
            {
                if (_approver != value)
                {
                    this._approver = value;
                    OnPropertyChanged(nameof(Approver));
                }
            }
        }

        public SpecialDiscountFormViewModel()
        {
            CreatedByConfig = new LookUpConfig();
            CreatedByConfig.EntityName = "systemusers";
            CreatedByConfig.FetchXml = @"<fetch version='1.0' count='10' page='{0}' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='systemuser'>
                <attribute name='fullname' alias='Name' />
                <attribute name='systemuserid' alias='Id' />
                <attribute name='createdon' alias='Detail' />            
                <order attribute='fullname' descending='false' />
                <filter type='and'>
                  <condition attribute='fullname' operator='like' value='%{1}%' />
                </filter>
              </entity>
            </fetch>";
            CreatedByConfig.LookUpTitle = "Chọn người xác nhận";
            CreatedByConfig.PropertyName = "Approver";

            CachTinh_Options = new List<OptionSet>()
            {
                new OptionSet("100000000","Số tiền"),
                new OptionSet("100000001","Phần trăm")
            };
        }
    }
}
