using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using System.Linq;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class ReservationFormViewModel : FormLookupViewModel
    {
        private ReservationFormModel _reservation;
        public ReservationFormModel Reservation
        {
            get => _reservation;
            set
            {
                _reservation = value;
                OnPropertyChanged(nameof(Reservation));
            }
        }

        public LookUpConfig PaymentschemeConfig { get; set; }
        public LookUpConfig HandoverConditionConfig { get; set; }
        public LookUpConfig PromotionConfig { get; set; }
        public LookUpConfig ContactLookUpConfig { get; set; }
        public LookUpConfig AccountLookUpConfig { get; set; }
        private LookUp _paymentScheme;
        public LookUp PaymentScheme
        {
            get => _paymentScheme;
            set
            {
                if (value != _paymentScheme)
                {
                    _paymentScheme = value;
                    OnPropertyChanged(nameof(PaymentScheme));
                }
            }
        }

        // khai báo để hứng dữ liệu khi chọn handover condition.
        private LookUp _bsd_packagesellings;
        public LookUp bsd_packagesellings
        {
            get => _bsd_packagesellings;
            set
            {
                if (value != null)
                {
                    IsBusy = true;
                    _bsd_packagesellings = value;
                    AddHandoverCondition();
                }
                else
                {
                    _bsd_packagesellings = value;
                }

            }
        }

        // khai báo để hứng dữ liệu khi chọn Promotion.
        private LookUp _bsd_promotion;
        public LookUp bsd_promotion
        {
            get => _bsd_promotion;
            set
            {
                if (value != null)
                {
                    IsBusy = true;
                    _bsd_promotion = value;
                    AddPromotion();
                }
                else
                {
                    _bsd_promotion = value;
                }

            }
        }

        // contact && account
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

        public bool DaTaoLichThanhToan { get; set; }

        // end
        // danh sacsh discount cua thong ctin chiet khau
        public ObservableCollection<ReservationDiscountOptionSet> Discounts { get; set; }
        public ObservableCollection<ReservationDiscountOptionSet> InternelDiscounts { get; set; }
        public ObservableCollection<ReservationDiscountOptionSet> WholesaleDiscounts { get; set; }

        public ObservableCollection<ReservationInstallmentModel> InstallmentList { get; set; }
        public ObservableCollection<ReservationCoowner> CoownerList { get; set; }
        public ObservableCollection<ReservationHandoverCondition> HandoverConditionList { get; set; }
        public ObservableCollection<ReservationPromotionModel> PromotionList { get; set; }
        public ObservableCollection<ReservationSpecialDiscountListModel> SpecialDiscountList { get; set; }

        private bool _showMoreDieuKienBanGiao;
        public bool ShowMoreDieuKienBanGiao { get => _showMoreDieuKienBanGiao; set { _showMoreDieuKienBanGiao = value; OnPropertyChanged(nameof(ShowMoreDieuKienBanGiao)); } }

        public int PageDieuKienBanGiao { get; set; } = 1;

        private bool _showMoreKhuyenMai;
        public bool ShowMoreKhuyenMai { get => _showMoreKhuyenMai; set { _showMoreKhuyenMai = value; OnPropertyChanged(nameof(ShowMoreKhuyenMai)); } }

        public int PageKhuyenMai { get; set; } = 1;

        private bool _showMoreChietKhauDacBiet;
        public bool ShowMoreChietKhauDacBiet { get => _showMoreChietKhauDacBiet; set { _showMoreChietKhauDacBiet = value; OnPropertyChanged(nameof(ShowMoreChietKhauDacBiet)); } }

        public int PageChietKhauDacBiet { get; set; } = 1;

        private bool _showMoreNguoiDongSoHuu;
        public bool ShowMoreNguoiDongSoHuu { get => _showMoreNguoiDongSoHuu; set { _showMoreNguoiDongSoHuu = value; OnPropertyChanged(nameof(ShowMoreNguoiDongSoHuu)); } }

        public int PageNguoiDongSoHuu { get; set; } = 1;

        private bool _showMoreLichThanhToan;
        public bool ShowMoreLichThanhToan { get => _showMoreLichThanhToan; set { _showMoreLichThanhToan = value; OnPropertyChanged(nameof(ShowMoreLichThanhToan)); } }

        public int PageLichThanhToan { get; set; } = 1;

        public ReservationFormViewModel()
        {
            InstallmentList = new ObservableCollection<ReservationInstallmentModel>();
            CoownerList = new ObservableCollection<ReservationCoowner>();
            HandoverConditionList = new ObservableCollection<ReservationHandoverCondition>();
            PromotionList = new ObservableCollection<ReservationPromotionModel>();
            SpecialDiscountList = new ObservableCollection<ReservationSpecialDiscountListModel>();

            Discounts = new ObservableCollection<ReservationDiscountOptionSet>();
            InternelDiscounts = new ObservableCollection<ReservationDiscountOptionSet>();
            WholesaleDiscounts = new ObservableCollection<ReservationDiscountOptionSet>();

            #region contact va account lookup cònig
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
            #endregion

            PaymentschemeConfig = new LookUpConfig();
            PaymentschemeConfig.LookUpTitle = "Chọn lịch thanh toán";
            PaymentschemeConfig.EntityName = "bsd_paymentschemes";
            PaymentschemeConfig.PropertyName = "PaymentScheme";
            PaymentschemeConfig.FetchXml = @"<fetch count='20' page='{0}' version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='bsd_paymentscheme'>
                <attribute name='bsd_name' alias='Name' />
                <attribute name='createdon' alias='Detail' /> 
                <attribute name='bsd_paymentschemeid' alias='Id' />
                <order attribute='bsd_name' descending='false' />
                <filter type='and'>
                      <condition attribute='bsd_name' operator='like' value='%{1}%' />
                 </filter>
              </entity>
            </fetch>";

            // handover condition
            HandoverConditionConfig = new LookUpConfig();
            HandoverConditionConfig.LookUpTitle = "Chọn điều kiện bàn giao";
            HandoverConditionConfig.EntityName = "bsd_packagesellings";
            HandoverConditionConfig.PropertyName = "bsd_packagesellings";

            // promotion
            PromotionConfig = new LookUpConfig();
            PromotionConfig.LookUpTitle = "Chọn khuyến mại";
            PromotionConfig.EntityName = "bsd_promotions";
            PromotionConfig.PropertyName = "bsd_promotion";
        }       

        public async void AddHandoverCondition()
        {
            var isExist = this.HandoverConditionList.Any(x => x.bsd_packagesellingid == bsd_packagesellings.Id);
            if (isExist)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", bsd_packagesellings.Name + " đã tồn tại.", "Đóng");
                IsBusy = false; // tren ham set da set thanh true, o day phai set lai thanh false.
                return;
            }

            string path = $"/quotes({Reservation.quoteid})/bsd_quote_bsd_packageselling/$ref";

            IDictionary<string, string> data = new Dictionary<string, string>();
            data["@odata.id"] = $"{OrgConfig.ApiUrl}/bsd_packagesellings({bsd_packagesellings.Id})";
            CrmApiResponse result = await CrmHelper.PostData(path, data);

            if (result.IsSuccess)
            {
                MessagingCenter.Send<ReservationFormViewModel, bool>(this, "LoadHandoverConditions", true);
                // ben day chay qua ben kia roi ko can is busy = false nua.
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", result.GetErrorMessage(), "Đóng");
                IsBusy = false;
            }
        }

        private async void AddPromotion()
        {
            var isExist = this.PromotionList.Any(x => x.bsd_promotionid == bsd_promotion.Id);
            if (isExist)
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", bsd_promotion.Name + " đã tồn tại.", "Đóng");
                IsBusy = false; // tren ham set da set thanh true, o day phai set lai thanh false.
                return;
            }

            string path = $"/quotes({Reservation.quoteid})/bsd_quote_bsd_promotion/$ref";

            IDictionary<string, string> data = new Dictionary<string, string>();
            data["@odata.id"] = $"{OrgConfig.ApiUrl}/bsd_promotions({bsd_promotion.Id})";
            CrmApiResponse result = await CrmHelper.PostData(path, data);

            if (result.IsSuccess)
            {
                MessagingCenter.Send<ReservationFormViewModel, bool>(this, "LoadPromotions", true);
                // ben day chay qua ben kia roi ko can is busy = false nua.
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Thông báo", result.GetErrorMessage(), "Đóng");
                IsBusy = false;
            }
        }

        #region Dieu Kien Ban Giao
        public async Task LoadhandoverConditions(Guid ReservationId)
        {
            string fetch = $@"<fetch version='1.0' count = '3' page = '{PageDieuKienBanGiao}' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='bsd_packageselling'>
                <attribute name='bsd_name' />
                <attribute name='createdon' />
                <attribute name='bsd_type' />
                <attribute name='bsd_priceperm2' />
                <attribute name='bsd_amount' />
                <attribute name='bsd_percent' />
                <attribute name='bsd_method' />
                <attribute name='bsd_packagesellingid' />
                <order attribute='createdon' descending='true' />
                <link-entity name='bsd_quote_bsd_packageselling' from='bsd_packagesellingid' to='bsd_packagesellingid' visible='false' intersect='true'>
                <link-entity name='quote' from='quoteid' to='quoteid' alias='ab'>
                     <filter type='and'>
                          <condition attribute='quoteid' operator='eq' uitype='quote' value='{ReservationId}' />
                     </filter>
                </link-entity>
            </link-entity>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationHandoverCondition>>("bsd_packagesellings", fetch);

            if (result != null)
            {
                var data = result.value;

                if (data.Count <= 3)
                {
                    ShowMoreDieuKienBanGiao = false;
                }
                else
                {
                     ShowMoreDieuKienBanGiao = true;
                }

                foreach (var x in result.value)
                {
                    HandoverConditionList.Add(x);
                }
            }
        }
        #endregion

        #region Khuyen Mai
        public async Task LoadPromotions(Guid ReservationId)
        {
            string xml = $@"<fetch version='1.0' count = '3' page = '{PageKhuyenMai}' output-format='xml-platform' mapping='logical' distinct='true'>
              <entity name='bsd_promotion'>
                <attribute name='bsd_name' />
                <attribute name='createdon' />
                <attribute name='bsd_values' />
                <attribute name='statuscode' />
                <attribute name='bsd_startdate' />
                <attribute name='ownerid' />
                <attribute name='bsd_enddate' />
                <attribute name='bsd_description' />
                <attribute name='bsd_promotionid' />
                <order attribute='createdon' descending='true' />
                <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaselaunch' link-type='inner' alias='a_c72b74f6fa82e61180f23863bb367d40'>
                      <attribute name='bsd_name' alias='phaseslaunch_name' />
                      <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' link-type='inner' alias='ad'>
                           <attribute name='bsd_name' alias='project_name' />                        
                        </link-entity>
                </link-entity>
                <link-entity name='bsd_quote_bsd_promotion' from='bsd_promotionid' to='bsd_promotionid' visible='false' intersect='true'>
                      <link-entity name='quote' from='quoteid' to='quoteid' alias='ae'>
                             <filter type='and'>
                                    <condition attribute='quoteid' operator='eq' uiname='A,20.10' uitype='quote' value='{ReservationId}' />
                              </filter>
                      </link-entity>
                </link-entity>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationPromotionModel>>("bsd_promotions", xml);
            if (result != null)
            {
                var data = result.value;

                if (data.Count < 3)
                {
                    ShowMoreKhuyenMai = false;
                }
                else
                {
                    ShowMoreKhuyenMai = true;
                }

                foreach (var x in result.value)
                {
                    PromotionList.Add(x);
                }
            }          
        }

        #endregion

        #region Chiet Khau Dac Biet
        public async Task LoadSpecialDiscounts(Guid ReservationId)
        {
            var xml = $@"<fetch version='1.0' count = '3' page = '{PageChietKhauDacBiet}' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='bsd_discountspecial'>
                <attribute name='bsd_discountspecialid' />
                <attribute name='bsd_name' />
                <attribute name='createdon' />
                <attribute name='bsd_reasons' />
                <attribute name='statuscode' />
                <attribute name='bsd_percentdiscount' />
                <attribute name='bsd_amountdiscount' />
                <order attribute='bsd_name' descending='false' />
                <filter type='and'>
                  <condition attribute='bsd_quote' operator='eq' uiname='A,20.10' uitype='quote' value='{ReservationId}' />
                </filter>
                <link-entity name='systemuser' from='systemuserid' to='createdby' visible='false' link-type='outer' alias='a_769c3b2db214e911a97f000d3aa04914'>
                  <attribute name='fullname' alias='createdby_name' />
                </link-entity>
              </entity>
            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationSpecialDiscountListModel>>("bsd_discountspecials", xml);
            if (result != null)
            {
                var data = result.value;

                if (data.Count <= 3)
                {
                    ShowMoreChietKhauDacBiet = false;
                }
                else
                {
                    ShowMoreChietKhauDacBiet = true;
                }

                foreach (var x in result.value)
                {
                    SpecialDiscountList.Add(x);
                }
            }          
        }

        #endregion

        #region Nguoi Dong So Huu
        public async Task LoadCoOwners(Guid ReservationId)
        {
            string xml = $@"<fetch version='1.0' count = '3' page = '{PageNguoiDongSoHuu}' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='bsd_coowner'>
                <attribute name='bsd_coownerid' />
                <attribute name='bsd_name' />
                <attribute name='bsd_relationship' />
                <attribute name='createdon' />
                <order attribute='bsd_name' descending='false' />
                <link-entity name='account' from='accountid' to='bsd_customer' visible='false' link-type='outer' alias='a_1324f6d5b214e911a97f000d3aa04914'>
                  <attribute name='bsd_name' alias='account_name' />
                </link-entity>
                <link-entity name='contact' from='contactid' to='bsd_customer' visible='false' link-type='outer' alias='a_6b0d05eeb214e911a97f000d3aa04914'>
                  <attribute name='bsd_fullname' alias='contact_name' />
                </link-entity>
                 <filter type='and'>
                      <condition attribute='bsd_reservation' operator='eq' uitype='quote' value='{ReservationId}' />
                  </filter>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationCoowner>>("bsd_coowners", xml);
            if (result != null)
            {
                var data = result.value;

                if (data.Count <= 3)
                {
                    ShowMoreNguoiDongSoHuu = false;
                }
                else
                {
                    ShowMoreNguoiDongSoHuu = true;
                }

                foreach (var x in result.value)
                {
                    CoownerList.Add(x);
                }
            }           
        }

        #endregion

        #region Lich Thanh Toan
        public async Task LoadInstallments(Guid ReservationId)
        {
            string xml = $@"<fetch version='1.0' count = '3' page = '{PageLichThanhToan}' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='bsd_paymentschemedetail'>
                <attribute name='bsd_paymentschemedetailid' />
                <attribute name='bsd_ordernumber' />
                <attribute name='bsd_name' />
                <attribute name='bsd_duedate' />
                <attribute name='statuscode' />
                <attribute name='bsd_amountofthisphase' />
                <attribute name='bsd_amountwaspaid' />
                <attribute name='bsd_depositamount' />
                <attribute name='bsd_waiveramount' />
                <order attribute='bsd_ordernumber' descending='false' />
                <filter type='and'>
                  <condition attribute='bsd_reservation' operator='eq' uitype='quote' value='{ReservationId}' />
                </filter>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationInstallmentModel>>("bsd_paymentschemedetails", xml);
            if (result != null)
            {
                var data = result.value.OrderBy(x => x.bsd_ordernumber).ToList();
                if (data.Count < 3)
                {
                    ShowMoreLichThanhToan = false;
                }
                else
                {
                    ShowMoreLichThanhToan = true;
                }
                foreach (var x in result.value)
                {
                    InstallmentList.Add(x);
                }               
            }         
        }

        #endregion
    }
}
