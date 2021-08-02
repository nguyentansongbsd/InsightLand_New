using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ConasiCRM.Portable.ViewModels
{
    public class CoOwnerFormViewModel : FormLookupViewModel
    {
        public ObservableCollection<OptionSet> RelationShipOptionList { get; set; }

        private CoOwnerFormModel _coOwner;
        public CoOwnerFormModel CoOwner
        {
            get => _coOwner;
            set
            {
                if (_coOwner != value)
                {
                    _coOwner = value;
                    OnPropertyChanged(nameof(CoOwner));
                }
            }
        }

        public LookUpConfig ContactLookUpConfig { get; set; }
        public LookUpConfig AccountLookUpConfig { get; set; }

        public LookUp Contact
        {
            set
            {

                if (value != null)
                {
                    Customer = new CustomerLookUp()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Type = 1
                    };
                }
            }
        }
        public LookUp Account
        {
            set
            {
                if (value != null)
                {
                    Customer = new CustomerLookUp()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Type = 2
                    };

                }

            }
        }

        private CustomerLookUp _customer;
        public CustomerLookUp Customer
        {
            get => _customer;
            set
            {
                if (_customer != value)
                {
                    _customer = value;
                    OnPropertyChanged(nameof(Customer));
                }
            }
        }

        private OptionSet _relationShip;
        public OptionSet RelationShip
        {
            get { return _relationShip; }
            set
            {
                _relationShip = value;
                OnPropertyChanged(nameof(RelationShip));
            }
        }

        public CoOwnerFormViewModel()
        {
            RelationShipOptionList = new ObservableCollection<OptionSet>();
            RelationShipOptionList.Add(new OptionSet("100000000", "Vợ/chồng"));
            RelationShipOptionList.Add(new OptionSet("100000001", "Con"));
            RelationShipOptionList.Add(new OptionSet("100000002", "Cha/Mẹ"));
            RelationShipOptionList.Add(new OptionSet("100000003", "Bạn"));
            RelationShipOptionList.Add(new OptionSet("100000004", "Khác"));

            ContactLookUpConfig = new LookUpConfig()
            {
                FetchXml = @"<fetch version='1.0' count='15' page='{0}' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='contactid' alias='Id' />
                    <attribute name='bsd_fullname' alias='Name' />
                    <attribute name='createdon' alias='Detail' />
                    <order attribute='bsd_fullname' descending='false' />
                    <filter type='or'>
                          <condition attribute='bsd_fullname' operator='like' value='%{1}%' />
                     </filter>
                  </entity>
                </fetch>",
                EntityName = "contacts",
                PropertyName = "Contact",
                LookUpTitle = "Chọn khách hàng"
            };

            AccountLookUpConfig = new LookUpConfig()
            {
                FetchXml = @"<fetch version='1.0' count='15' page='{0}' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='account'>
                    <attribute name='accountid' alias='Id' />
                    <attribute name='bsd_name' alias='Name' />
                    <attribute name='createdon' alias='Detail' />
                    <order attribute='bsd_name' descending='false' />
                    <filter type='or'>
                          <condition attribute='bsd_name' operator='like' value='%{1}%' />
                    </filter>
                  </entity>
                </fetch>",
                EntityName = "accounts",
                PropertyName = "Account",
                LookUpTitle = "Chọn khách hàng"
            };
        }
    }
}
