using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class QueueFormViewModel : FormLookupViewModel
    {
        private QueueFormModel _queueFormModel;
        public QueueFormModel QueueFormModel
        {
            get => _queueFormModel;
            set
            {
                _queueFormModel = value;
                OnPropertyChanged(nameof(QueueFormModel));
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

        private List<Account> _daiLyOptions;
        public List<Account> DaiLyOptions { get => _daiLyOptions; set { _daiLyOptions = value; OnPropertyChanged(nameof(DaiLyOptions)); } }

        private Account _daiLyOption;
        public Account DailyOption { get => _daiLyOption; set { _daiLyOption = value;OnPropertyChanged(nameof(DailyOption)); } }

        private List<Contact> _collaboratorOptions;
        public List<Contact> CollaboratorOptions { get => _collaboratorOptions; set { _collaboratorOptions = value; OnPropertyChanged(nameof(CollaboratorOptions)); } }

        private Contact _collaboratorOption;
        public Contact CollaboratorOption { get => _collaboratorOption; set { _collaboratorOption = value; OnPropertyChanged(nameof(CollaboratorOption)); } }

        private List<Account> _customerReferralOptions;
        public List<Account> CustomerReferralOptions { get => _customerReferralOptions; set { _customerReferralOptions = value;OnPropertyChanged(nameof(CustomerReferralOptions)); } }

        private Account _customerReferralOption;
        public Account CustomerReferralOption { get => _customerReferralOption; set { _customerReferralOption = value;OnPropertyChanged(nameof(CustomerReferralOption)); } }

        private CustomerLookUp _khachHangGioiThieu;
        public CustomerLookUp KhachHangGioiThieu
        {
            get => _khachHangGioiThieu;
            set
            {
                if (_khachHangGioiThieu != value)
                {
                    _khachHangGioiThieu = value;
                    OnPropertyChanged(nameof(KhachHangGioiThieu));
                }
            }
        }

        public QueueFormViewModel()
        {
            ContactLookUpConfig = new LookUpConfig()
            {
                FetchXml = @"<fetch version='1.0' count='30' page='{0}' output-format='xml-platform' mapping='logical' distinct='false'>
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
                FetchXml = @"<fetch version='1.0' count='30' page='{0}' output-format='xml-platform' mapping='logical' distinct='false'>
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

            IsBusy = true;
        }

        public async Task LoadSalesAgent()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='account'>
                                <attribute name='bsd_name' />
                                <attribute name='name' />
                                <attribute name='bsd_businesstype' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                  <condition attribute='bsd_businesstype' operator='eq' value='100000002' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Account>>("accounts", fetchXml);
            if (result != null && result.value.Count > 0)
            {
                DaiLyOptions = result.value;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("", "Lỗi hệ thống vui lòng thử lại", "Đóng");
            }
        }

        public async Task LoadCollaborator()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='contact'>
                                <attribute name='bsd_fullname' />
                                <attribute name='contactid' />
                                <attribute name='bsd_type' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                  <condition attribute='bsd_type' operator='contain-values'>
                                    <value>100000001</value>
                                  </condition>
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Contact>>("contacts", fetchXml);
            if (result != null && result.value.Count > 0)
            {
                this.CollaboratorOptions = result.value;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("", "Lỗi hệ thống vui lòng thử lại", "Đóng");
            }
        }

        public async Task LoadCustomerReferral()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='account'>
                    <attribute name='accountid' alias='Id' />
                    <attribute name='bsd_name' alias='Name' />
                    <attribute name='createdon' alias='Detail' />
                    <order attribute='bsd_name' descending='false' />
                  </entity>
                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Account>>("accounts", fetchXml);
            if (result != null && result.value.Count > 0)
            {
                this.CustomerReferralOptions = result.value;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("", "Lỗi hệ thống vui lòng thử lại", "Đóng");
            }
        }
    }
}
