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
using ConasiCRM.Portable.Settings;

namespace ConasiCRM.Portable.ViewModels
{
    public class ReservationFormViewModel : BaseViewModel
    {
        public Guid ProjectId { get; set; }
        public Guid ProductId { get; set; }
        public string KeywordHandoverCondition { get; set; }
        public string KeywordPromotion { get; set; }
        public List<string> SelectedPromotionIds { get; set; }

        private QuoteModel _quote;
        public QuoteModel Quote { get => _quote; set { _quote = value; OnPropertyChanged(nameof(Quote)); } }

        private string _titleQuote;
        public string TitleQuote { get => _titleQuote; set { _titleQuote = value; OnPropertyChanged(nameof(TitleQuote)); } }
        private string _staffAgentQuote;
        public string StaffAgentQuote { get => _staffAgentQuote; set { _staffAgentQuote = value; OnPropertyChanged(nameof(StaffAgentQuote)); } }
        private string _descriptionQuote;
        public string DescriptionQuote { get => _descriptionQuote; set { _descriptionQuote = value; OnPropertyChanged(nameof(DescriptionQuote)); } }
        private string _waiverManaFee;
        public string WaiverManaFee { get => _waiverManaFee; set { _waiverManaFee = value; OnPropertyChanged(nameof(WaiverManaFee)); } }

        public ObservableCollection<DiscountChildOptionSet> DiscountChilds { get; set; } = new ObservableCollection<DiscountChildOptionSet>();
        public ObservableCollection<OptionSet> PromotionsSelected { get; set; } = new ObservableCollection<OptionSet>();
        public ObservableCollection<OptionSet> Promotions { get; set; } = new ObservableCollection<OptionSet>();

        private List<OptionSet> _paymentSchemes;
        public List<OptionSet> PaymentSchemes { get => _paymentSchemes; set { _paymentSchemes = value; OnPropertyChanged(nameof(PaymentSchemes)); } }
        private List<OptionSet> _discountLists;
        public List<OptionSet> DiscountLists { get => _discountLists; set { _discountLists = value; OnPropertyChanged(nameof(DiscountLists)); } }
        private List<HandoverConditionModel> _handoverConditions;
        public List<HandoverConditionModel> HandoverConditions { get => _handoverConditions; set { _handoverConditions = value; OnPropertyChanged(nameof(HandoverConditions)); } }

        private OptionSet _paymentScheme;
        public OptionSet PaymentScheme { get => _paymentScheme; set { _paymentScheme = value; OnPropertyChanged(nameof(PaymentScheme)); } }
        private OptionSet _discountList;
        public OptionSet DiscountList { get => _discountList; set { _discountList = value; OnPropertyChanged(nameof(DiscountList)); } }
        private HandoverConditionModel _handoverCondition;
        public HandoverConditionModel HandoverCondition { get => _handoverCondition; set { _handoverCondition = value; OnPropertyChanged(nameof(HandoverCondition)); } }

        private QuoteUnitInforModel _unitInfor;
        public QuoteUnitInforModel UnitInfor { get => _unitInfor; set { _unitInfor = value; OnPropertyChanged(nameof(UnitInfor)); } }

        private StatusCodeModel _statusUnit;
        public StatusCodeModel StatusUnit { get => _statusUnit; set { _statusUnit = value; OnPropertyChanged(nameof(StatusUnit)); } }

        private OptionSet _priceListPhasesLaunch;
        public OptionSet PriceListPhasesLaunch { get => _priceListPhasesLaunch; set { _priceListPhasesLaunch = value; OnPropertyChanged(nameof(PriceListPhasesLaunch)); } }

        private OptionSet _priceListApply;
        public OptionSet PriceListApply { get => _priceListApply; set { _priceListApply = value; OnPropertyChanged(nameof(PriceListApply)); } }

        #region CoOwner
        public ObservableCollection<CoOwnerFormModel> CoOwnerList { get; set; } = new ObservableCollection<CoOwnerFormModel>();

        private CoOwnerFormModel _coOwner;
        public CoOwnerFormModel CoOwner { get => _coOwner; set { _coOwner = value; OnPropertyChanged(nameof(CoOwner)); } }

        private string _titleCoOwner;
        public string TitleCoOwner { get => _titleCoOwner; set { _titleCoOwner = value; OnPropertyChanged(nameof(TitleCoOwner)); } }

        private List<OptionSet> _relationships;
        public List<OptionSet> Relationships { get => _relationships; set { _relationships = value; OnPropertyChanged(nameof(Relationships)); } }

        private OptionSet _relationship;
        public OptionSet Relationship { get => _relationship; set { _relationship = value; OnPropertyChanged(nameof(Relationship)); } }

        private OptionSet _customerCoOwner;
        public OptionSet CustomerCoOwner { get => _customerCoOwner; set { _customerCoOwner = value; OnPropertyChanged(nameof(CustomerCoOwner)); } }
        #endregion

        #region Thong tin ban hang
        private List<OptionSet> _queues;
        public List<OptionSet> Queues { get => _queues; set { _queues = value; OnPropertyChanged(nameof(Queues)); } }

        private OptionSet _queue;
        public OptionSet Queue { get => _queue; set { _queue = value; OnPropertyChanged(nameof(Queue)); } }

        private OptionSet _buyer;
        public OptionSet Buyer { get => _buyer; set { _buyer = value; OnPropertyChanged(nameof(Buyer)); } }
        #endregion

        #region Chi tiet
        private List<OptionSet> _contractTypes;
        public List<OptionSet> ContractTypes { get => _contractTypes; set { _contractTypes = value; OnPropertyChanged(nameof(ContractTypes)); } }

        private OptionSet _contractType;
        public OptionSet ContractType { get => _contractType; set { _contractType = value; OnPropertyChanged(nameof(ContractType)); } }

        private TaxCodeModel _taxCode;
        public TaxCodeModel TaxCode { get => _taxCode; set { _taxCode = value; OnPropertyChanged(nameof(TaxCode)); } }
        #endregion

        #region Thong tin bao gia
        private List<OptionSet> _salesAgents;
        public List<OptionSet> SalesAgents { get => _salesAgents; set { _salesAgents = value; OnPropertyChanged(nameof(SalesAgents)); } }

        private OptionSet _salesAgent;
        public OptionSet SalesAgent { get => _salesAgent; set { _salesAgent = value; OnPropertyChanged(nameof(SalesAgent)); } }
        #endregion

        #region Thong tin Gia
        private decimal _totalDiscount = 0;
        public decimal TotalDiscount { get => _totalDiscount; set { _totalDiscount = value; OnPropertyChanged(nameof(TotalDiscount)); SetNetSellingPrice(); } }

        private decimal _totalHandoverCondition = 0;
        public decimal TotalHandoverCondition { get => _totalHandoverCondition; set { _totalHandoverCondition = value; OnPropertyChanged(nameof(TotalHandoverCondition)); SetNetSellingPrice(); } }

        private decimal _netSellingPrice = 0;
        public decimal NetSellingPrice { get => _netSellingPrice; set { _netSellingPrice = value; OnPropertyChanged(nameof(NetSellingPrice)); SetTotalVatTax(); SetMaintenanceFee(); } }

        private decimal _landValueDeduction = 0;
        public decimal LandValueDeduction { get => _landValueDeduction; set { _landValueDeduction = value; OnPropertyChanged(nameof(LandValueDeduction)); } }

        private decimal _totalVATTax = 0;
        public decimal TotalVATTax { get => _totalVATTax; set { _totalVATTax = value; OnPropertyChanged(nameof(TotalVATTax)); } }

        private decimal _maintenanceFee = 0;
        public decimal MaintenanceFee { get => _maintenanceFee; set { _maintenanceFee = value; OnPropertyChanged(nameof(MaintenanceFee)); } }

        private decimal _totalAmount = 0;
        public decimal TotalAmount { get => _totalAmount; set { _totalAmount = value; OnPropertyChanged(nameof(TotalAmount)); } }
        #endregion

        #region Tinh toan gia tien o bang chi tiet
        //Tinh (-)Chiet khau
        public void SetTotalDiscount()
        {
            this.TotalDiscount = 0;
            foreach (var item in this.DiscountChilds)
            {
                if (item.Selected == true && item.new_type == "100000000") // percent
                {
                    this.TotalDiscount += (item.bsd_percentage * this.UnitInfor.price) / 100;
                }
                if (item.Selected == true && item.new_type == "100000001") // amount
                {
                    this.TotalDiscount += item.bsd_amount;
                }
            }

            //var a = Math.Round(viewModel.TotalDiscount, 0); lam tron so thap phan
        }

        public void SetTotalHandoverCondition()
        {
            if (this.HandoverCondition.bsd_method == "100000000")// Price per sqm
            {
                this.TotalHandoverCondition = this.HandoverCondition.bsd_priceperm2 * this.UnitInfor.bsd_netsaleablearea;
            }
            else if (this.HandoverCondition.bsd_method == "100000001") //Amount
            {
                this.TotalHandoverCondition = this.HandoverCondition.bsd_amount;
            }
            else //Percent (%)
            {
                this.TotalHandoverCondition = (this.HandoverCondition.bsd_percent * this.UnitInfor.price) / 100;
            }
        }

        public void SetNetSellingPrice()
        {
            // Gia ban truoc thue = Gia ban san pham - Tong chiet khau + Tong dieu kien ban gia
            if (this.HandoverCondition == null) return;
            this.NetSellingPrice = 0;
            this.NetSellingPrice = UnitInfor.price - this.TotalDiscount + this.TotalHandoverCondition;
            SetTotalAmount();
        }

        public void SetLandValueDeduction()
        {
            // Tổng giá trị QSDĐ = = Land value of unit (sqm) * Net Usable Area
            this.LandValueDeduction = UnitInfor.bsd_landvalueofunit * UnitInfor.bsd_netsaleablearea;
            SetTotalAmount();
        }

        public void SetTotalVatTax()
        {
            //Tổng tiền thuế VAT = ((Gia ban truoc thue - Tổng giá trị QSDĐ) * Ma so thue)
            this.TotalVATTax = 0;
            this.TotalVATTax = ((this.NetSellingPrice - this.LandValueDeduction) * this.TaxCode.bsd_value) / 100;
            SetTotalAmount();
        }

        public void SetMaintenanceFee()
        {
            //Phí bảo trì = (Gia ban truoc thue * Maintenance fee% )/100
            this.MaintenanceFee = 0;
            this.MaintenanceFee = (this.NetSellingPrice * UnitInfor.bsd_maintenancefeespercent) / 100;
            SetTotalAmount();
        }

        public void SetTotalAmount()
        {
            this.TotalAmount = 0;
            if (this.NetSellingPrice > 0 && this.LandValueDeduction > 0 && this.TotalVATTax > 0 && this.MaintenanceFee > 0)
            {
                this.TotalAmount = this.NetSellingPrice - this.LandValueDeduction + this.TotalVATTax + this.MaintenanceFee;
            }
        }
        #endregion

        public ReservationFormViewModel()
        {
            SelectedPromotionIds = new List<string>();
            this.Quote = new QuoteModel();
        }

        // Load thong tin san pham
        public async Task LoadUnitInfor()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='product'>
                                    <attribute name='name' />
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_constructionarea' />
                                    <attribute name='bsd_netsaleablearea' />
                                    <attribute name='bsd_actualarea' />
                                    <attribute name='bsd_projectcode' alias='_bsd_projectcode_value'/>
                                    <attribute name='bsd_phaseslaunchid' alias='_bsd_phaseslaunchid_value'/>
                                    <attribute name='bsd_unittype' alias='_bsd_unittype_value'/>
                                    <attribute name='bsd_taxpercent' />
                                    <attribute name='bsd_queuingfee' />
                                    <attribute name='bsd_depositamount' />
                                    <attribute name='price' />
                                    <attribute name='bsd_landvalueofunit' />
                                    <attribute name='bsd_maintenancefeespercent' />
                                    <attribute name='bsd_numberofmonthspaidmf' />
                                    <attribute name='bsd_managementamountmonth' />
                                    <attribute name='productid' />
                                    <attribute name='defaultuomid' alias='_defaultuomid_value'/>
                                    <order attribute='bsd_constructionarea' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='productid' operator='eq' uitype='product' value='{ProductId}' />
                                    </filter>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectcode' visible='false' link-type='outer' alias='a_9a5e44d019dbeb11bacb002248168cad'>
                                      <attribute name='bsd_name' alias='project_name'/>
                                      <attribute name='bsd_projectid'/>
                                    </link-entity>
                                    <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaseslaunchid' visible='false' link-type='outer' alias='a_645347ca19dbeb11bacb002248168cad'>
                                        <attribute name='bsd_name' alias='phaseslaunch_name'/>
                                        <link-entity name='pricelevel' from='pricelevelid' to='bsd_pricelistid' visible='false' link-type='outer' alias='a_f07b3be219dbeb11bacb002248168cad'>
                                          <attribute name='name' alias='pricelist_name_phaseslaunch'/>
                                          <attribute name='pricelevelid' alias='pricelist_id_phaseslaunch'/>
                                        </link-entity>
                                    </link-entity>
                                    <link-entity name='pricelevel' from='pricelevelid' to='pricelevelid' visible='false' link-type='outer' alias='a_af3c62ff7dd1eb11bacc000d3a80021e'>
                                      <attribute name='name' alias='pricelist_name_unit'/>
                                      <attribute name='pricelevelid' alias='pricelist_id_unit'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QuoteUnitInforModel>>("products", fetchXml);
            if (result == null || result.value.Any() == false) return;

            this.UnitInfor = result.value.FirstOrDefault();
            this.StatusUnit = StatusCodeUnit.GetStatusCodeById(UnitInfor.statuscode);

            if (UnitInfor._bsd_phaseslaunchid_value != Guid.Empty)
            {
                this.PriceListPhasesLaunch = new OptionSet(UnitInfor.pricelist_id_phaseslaunch.ToString(), UnitInfor.pricelist_name_phaseslaunch);
            }
            else
            {
                this.PriceListPhasesLaunch = new OptionSet(UnitInfor.pricelist_id_unit.ToString(), UnitInfor.pricelist_name_unit);
            }

            this.PriceListApply = this.PriceListPhasesLaunch;
            SetLandValueDeduction();
        }

        // Load tax code
        public async Task LoadTaxCode()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_taxcode'>
                                    <attribute name='bsd_taxcodeid'/>
                                    <attribute name='bsd_value'/>
                                    <order attribute='bsd_name' descending='false' />
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<TaxCodeModel>>("bsd_taxcodes", fetchXml);
            if (result == null || result.value.Any() == false) return;

            this.TaxCode = result.value.SingleOrDefault();
        }

        // load phuong thuc thanh toan vs status code = confirm va theo du an
        public async Task LoadPaymentSchemes()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_paymentscheme'>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='bsd_paymentschemeid' alias='Val' />
                                    <order attribute='createdon' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='eq' value='100000000' />
                                      <condition attribute='bsd_project' operator='eq' uitype='bsd_project' value='{this.UnitInfor._bsd_projectcode_value}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_paymentschemes", fetchXml);
            if (result == null || result.value.Count == 0) return;
            this.PaymentSchemes = result.value;
        }

        // Load Dieu kien ban giao
        public async Task LoadHandoverCondition()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_packageselling'>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='bsd_unittype' alias='_bsd_unittype_value'/>
                                    <attribute name='bsd_byunittype' />
                                    <attribute name='bsd_packagesellingid' alias='Val'/>
                                    <attribute name='bsd_method' />
                                    <attribute name='bsd_priceperm2' />
                                    <attribute name='bsd_amount' />
                                    <attribute name='bsd_percent' />
                                    <order attribute='bsd_name' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_phaseslaunch' operator='eq' uitype='bsd_phaseslaunch' value='{this.UnitInfor._bsd_phaseslaunchid_value}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<HandoverConditionModel>>("bsd_packagesellings", fetchXml);
            if (result == null || result.value.Count == 0) return;
            this.HandoverConditions = result.value;
        }

        // Load Chieu khau
        public async Task LoadDiscountList()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_phaseslaunch'>
                                    <attribute name='bsd_phaseslaunchid' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_phaseslaunchid' operator='eq' value='{this.UnitInfor._bsd_phaseslaunchid_value}'/>
                                    </filter>
                                    <link-entity name='bsd_discounttype' from='bsd_discounttypeid' to='bsd_discountlist' visible='false' link-type='outer' alias='a_d55241be19dbeb11bacb002248168cad'>
                                        <attribute name='bsd_name' alias='Label'/>
                                        <attribute name='bsd_discounttypeid' alias='Val'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_phaseslaunchs", fetchXml);
            if (result == null || result.value.Any() == false) return;
            this.DiscountLists = result.value;
        }

        // Load Chieu khau con
        public async Task LoadDiscountChilds()
        {
            // new_type -> loai cua discounts (precent:100000000 or amount:100000001)
            if (DiscountList == null) return;
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_discount'>
                                    <attribute name='bsd_discountid' alias='Val'/>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='bsd_amount'/>
                                    <attribute name='bsd_percentage'/>
                                    <attribute name='new_type'/>
                                    <order attribute='bsd_name' descending='false' />
                                    <link-entity name='bsd_bsd_discounttype_bsd_discount' from='bsd_discountid' to='bsd_discountid' visible='false' intersect='true'>
                                      <link-entity name='bsd_discounttype' from='bsd_discounttypeid' to='bsd_discounttypeid' alias='ab'>
                                        <filter type='and'>
                                          <condition attribute='bsd_discounttypeid' operator='eq' uitype='bsd_discounttype' value='" + this.DiscountList.Val + @"' />
                                        </filter>
                                      </link-entity>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DiscountChildOptionSet>>("bsd_discounts", fetchXml);
            if (result == null || result.value.Any() == false) return;

            foreach (var item in result.value)
            {
                this.DiscountChilds.Add(item);
            }
        }

        // Load Khuyen mai
        public async Task LoadPromotions()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_promotion'>
                                    <attribute name='bsd_name' alias='Label'/>
                                    <attribute name='bsd_values'/>
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_startdate' />
                                    <attribute name='bsd_phaselaunch' />
                                    <attribute name='ownerid' />
                                    <attribute name='bsd_enddate' />
                                    <attribute name='bsd_description' />
                                    <attribute name='bsd_promotionid' alias='Val' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_phaselaunch' operator='eq' uitype='bsd_phaseslaunch' value='{this.UnitInfor._bsd_phaseslaunchid_value}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("bsd_promotions", fetchXml);
            if (result == null || result.value.Any() == false) return;

            foreach (var item in result.value)
            {
                this.Promotions.Add(item);
            }
        }

        // Load Giu cho
        public async Task LoadQueues()
        {
            string fectchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='opportunity'>
                                    <attribute name='name' alias='Label'/>
                                    <attribute name='opportunityid' alias='Val'/>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='{UserLogged.Id}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("opportunities", fectchXml);
            if (result == null || result.value.Any() == false) return;
            this.Queues = result.value;
        }

        // Load danh sach dai ly/ san giao dich
        public async Task LoadSalesAgents()
        {
            //Load account co field bsd_businesstypesys la sales agent(100000002)
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='account'>
                                    <attribute name='name' alias='Label'/>
                                    <attribute name='accountid' alias='Val' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_businesstypesys' operator='contain-values'>
                                        <value>100000002</value>
                                      </condition>
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("accounts", fetchXml);
            if (result == null || result.value.Any() == false) return;
            this.SalesAgents = result.value;
        }

        public async Task<bool> AddPromotion()
        {
            if (this.SelectedPromotionIds.Count == 0) return false;
            string path = $"/quotes({this.Quote.quoteid})/bsd_quote_bsd_promotion/$ref";
            IDictionary<string, string> data = new Dictionary<string, string>();
            CrmApiResponse apiResponse = new CrmApiResponse();
            foreach (var item in this.SelectedPromotionIds)
            {
                data["@odata.id"] = $"{OrgConfig.ApiUrl}/bsd_promotions({item})";
                apiResponse = await CrmHelper.PostData(path, data);
            }
            if (apiResponse.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> AddHandoverCondition()
        {
            string path = $"/quotes({this.Quote.quoteid})/bsd_quote_bsd_packageselling/$ref";

            IDictionary<string, string> data = new Dictionary<string, string>();
            data["@odata.id"] = $"{OrgConfig.ApiUrl}/bsd_packagesellings({this.HandoverCondition.Val})";
            CrmApiResponse result = await CrmHelper.PostData(path, data);

            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> AddCoOwer()
        {
            if (this.CoOwnerList == null || this.CoOwnerList.Count == 0) return false;
            string path = "/bsd_coowners";
            CrmApiResponse apiResponse = new CrmApiResponse();
            foreach (var item in this.CoOwnerList)
            {
                var content = await GetContentCoOwer(item);
                apiResponse = await CrmHelper.PostData(path, content);
            }
            if (apiResponse.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private async Task<object> GetContentCoOwer(CoOwnerFormModel coOwner)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["bsd_coownerid"] = coOwner.bsd_coownerid;
            data["bsd_name"] = coOwner.bsd_name;
            data["bsd_relationship"] = coOwner.bsd_relationshipId;
            data["bsd_reservation@odata.bind"] = $"quotes({this.Quote.quoteid})";

            if (this.CustomerCoOwner.Title == "2")
            {
                data["bsd_customer_contact@odata.bind"] = $"/contacts({coOwner.contact_id})";
                await CrmHelper.SetNullLookupField("bsd_coowners", coOwner.bsd_coownerid, "bsd_customer_contact");
            }
            else
            {
                data["bsd_customer_account@odata.bind"] = $"/accounts({coOwner.account_id})";
                await CrmHelper.SetNullLookupField("bsd_coowners", coOwner.bsd_coownerid, "bsd_customer_account");
            }
            return data;
        }

        public async Task<bool> CreateQuote()
        {
            string path = "/quotes";
            Quote.quoteid = Guid.NewGuid();
            var content = await GetContent();
            CrmApiResponse response = await CrmHelper.PostData(path, content);
            if (response.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<object> GetContent()
        {
            List<string> discounts = new List<string>();
            foreach (var item in this.DiscountChilds)
            {
                if (item.Selected == true)
                {
                    discounts.Add(item.Val);
                }
            }

            Dictionary<string, object> data = new Dictionary<string, object>();
            data["quoteid"] = this.Quote.quoteid;
            data["name"] = this.TitleQuote;
            data["bsd_discounts"] = string.Join(",", discounts);

            data["bsd_unitstatus"] = this.UnitInfor.statuscode;
            data["bsd_constructionarea"] = this.UnitInfor.bsd_constructionarea;
            data["bsd_netusablearea"] = this.UnitInfor.bsd_netsaleablearea;
            data["bsd_actualarea"] = this.UnitInfor.bsd_actualarea;
            data["bsd_depositfee"] = this.UnitInfor.bsd_queuingfee;
            data["bsd_bookingfee"] = this.UnitInfor.bsd_depositamount;
            data["bsd_contracttypedescripton"] = this.ContractType.Val;
            data["bsd_nameofstaffagent"] = this.StaffAgentQuote;
            data["bsd_referral"] = this.DescriptionQuote;

            data["bsd_detailamount"] = this.UnitInfor.price;
            data["bsd_discount"] = this.TotalDiscount;
            data["bsd_packagesellingamount"] = this.TotalHandoverCondition;
            data["bsd_totalamountlessfreight"] = this.NetSellingPrice;
            data["bsd_landvaluededuction"] = this.LandValueDeduction;
            data["totaltax"] = this.TotalVATTax;
            data["bsd_freightamount"] = this.MaintenanceFee;
            data["totalamount_base"] = this.TotalAmount;

            data["bsd_numberofmonthspaidmf"] = this.UnitInfor.bsd_numberofmonthspaidmf;
            data["bsd_managementfee"] = this.UnitInfor.bsd_managementamountmonth;
            data["bsd_waivermanafeemonth"] = this.WaiverManaFee;

            data["bsd_paymentscheme@odata.bind"] = $"/bsd_paymentschemes({this.PaymentScheme.Val})";
            data["bsd_unitno@odata.bind"] = $"/products({this.UnitInfor.productid})";
            data["bsd_projectid@odata.bind"] = $"/bsd_projects({this.UnitInfor._bsd_projectcode_value})";
            data["bsd_salessgentcompany@odata.bind"] = $"/accounts({this.SalesAgent.Val})";
            data["bsd_taxcode@odata.bind"] = $"/bsd_taxcodes({this.TaxCode.bsd_taxcodeid})";

            if (this.UnitInfor._bsd_phaseslaunchid_value != Guid.Empty)
            {
                data["bsd_phaseslaunchid@odata.bind"] = $"/bsd_phaseslaunchs({this.UnitInfor._bsd_phaseslaunchid_value})";
            }

            if (this.UnitInfor.pricelist_id_phaseslaunch != Guid.Empty)
            {
                data["bsd_pricelistphaselaunch@odata.bind"] = $"/pricelevels({this.UnitInfor.pricelist_id_phaseslaunch})";
            }

            if (this.UnitInfor.pricelist_id_unit != Guid.Empty)
            {
                data["pricelevelid@odata.bind"] = $"/pricelevels({this.UnitInfor.pricelist_id_unit})";
            }

            if (this.DiscountList != null)
            {
                data["bsd_discountlist@odata.bind"] = $"/bsd_discounttypes({this.DiscountList.Val})";
            }
            else
            {
                await CrmHelper.SetNullLookupField("quotes", this.Quote.quoteid, "bsd_discountlist");
            }

            if (this.Queue != null)
            {
                data["opportunityid@odata.bind"] = $"/opportunities({this.Queue.Val})";
            }
            else
            {
                await CrmHelper.SetNullLookupField("quotes", this.Quote.quoteid, "opportunityid");
            }

            if (this.Buyer.Title == "2")
            {
                data["customerid_contact@odata.bind"] = $"/contacts({this.Buyer.Val})";
                await CrmHelper.SetNullLookupField("quotes", this.Quote.quoteid, "customerid_account");
            }
            else
            {
                data["customerid_account@odata.bind"] = $"/accounts({this.Buyer.Val})";
                await CrmHelper.SetNullLookupField("quotes", this.Quote.quoteid, "customerid_contact");
            }

            if (UserLogged.Id != Guid.Empty)
            {
                data["bsd_employee@odata.bind"] = "/bsd_employees(" + UserLogged.Id + ")";
            }
            if (UserLogged.ManagerId != Guid.Empty)
            {
                data["ownerid@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            }

            return data;
        }

        public async Task<bool> CreateQuoteProduct()
        {
            string path = "/quotedetails";
            var content = await GetContentQuoteProduct();
            CrmApiResponse apiResponse = await CrmHelper.PostData(path, content);
            if (apiResponse.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<object> GetContentQuoteProduct()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["quotedetailid"] = Guid.NewGuid();
            data["isproductoverridden"] = false;
            data["ispriceoverridden"] = false;
            data["priceperunit"] = this.UnitInfor.price;
            data["quantity"] = 1;
            data["quotedetailname"] = this.TitleQuote;
            data["tax"] = this.TotalVATTax;
            data["manualdiscountamount"] = this.TotalDiscount;
            data["extendedamount"] = this.UnitInfor.price + this.TotalVATTax - this.TotalDiscount;
            
            data["quoteid@odata.bind"] = $"/quotes({this.Quote.quoteid})";
            data["uomid@odata.bind"] = $"/products({this.UnitInfor._defaultuomid_value})";
            data["productid@odata.bind"] = $"/products({this.UnitInfor.productid})";

            //if (UserLogged.ManagerId != Guid.Empty)
            //{
            //    data["OwnerId@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            //}
            return data;
        }

    }
}
