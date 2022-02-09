using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Linq;
using ConasiCRM.Portable.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ConasiCRM.Portable.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConasiCRM.Portable.Resources;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservationForm : ContentPage
    {
        public Action<int> CheckReservation;
        public ReservationFormViewModel viewModel;
        private bool isSetTotal;
        private List<string> newSelectedPromotionIds;
        private List<string> needDeletedPromotionIds;
        private bool _isEnableCheck { get; set; }

        public ReservationForm(Guid quoteId)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ReservationFormViewModel();
            centerModalPromotions.Body.BindingContext = viewModel;
            centerModalCoOwner.Body.BindingContext = viewModel;
            viewModel.QuoteId = quoteId;
            InitUpdate();
        }

        public ReservationForm(Guid productId, OptionSet queue, OptionSet saleAgentCompany, string nameOfStaffAgent, OptionSet customer)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ReservationFormViewModel();
            centerModalPromotions.Body.BindingContext = viewModel;
            centerModalCoOwner.Body.BindingContext = viewModel;
            viewModel.ProductId = productId;
            viewModel.Queue = queue;

            if (viewModel.Queue == null)
            {
                lblGiuCho.IsVisible = false;
                lookupGiuCho.IsVisible = false;
            }

            if (saleAgentCompany != null)
            {
                viewModel.SalesAgent = saleAgentCompany;
                lookupDaiLySanGiaoDich.IsEnabled = false;
            }
            if (!string.IsNullOrWhiteSpace(nameOfStaffAgent))
            {
                viewModel.Quote.bsd_nameofstaffagent = nameOfStaffAgent;
                entryNhanVienDaiLy.IsEnabled = false;
            }
            if (customer != null)
            {
                viewModel.Buyer = customer;
                lookupNguoiMua.IsEnabled = false;
                lookupNguoiMua.HideClearButton();
            }

            Init();
        }

        public async void Init()
        {
            await viewModel.LoadUnitInfor();
            if (viewModel.UnitInfor != null)
            {
                if (viewModel.UnitInfor.statuscode == "100000006") // 100000006 :  Reserve
                {
                    CheckReservation?.Invoke(1);
                    return;
                }
                if (viewModel.Queue != null)
                {
                    lookupGiuCho.IsEnabled = false;
                    lookupGiuCho.HideClearButton();
                }
                await viewModel.LoadTaxCode();
                SetPreOpen();
                CheckReservation?.Invoke(0);
            }
            else
            {
                CheckReservation?.Invoke(2);
            }
        }

        public async void InitUpdate()
        {
            await viewModel.LoadQuote();
            if (viewModel.Quote != null)
            {
                this.Title = Language.cap_nhat_bang_tinh_gia_title;
                buttonSave.Text = Language.cap_nhat_bang_tinh_gia;
                lookupNguoiMua.IsEnabled = false;
                lookupGiuCho.IsEnabled = false;
                lookupDaiLySanGiaoDich.IsEnabled = false;
                entryNhanVienDaiLy.IsEnabled = false;
                _isEnableCheck = true;

                if (viewModel.Queue == null)
                { 
                    lblGiuCho.IsVisible = false;
                    lookupGiuCho.IsVisible = false;
                }

                this.isSetTotal = true;// set = true de khong nhay vao ham SetTotal
                await viewModel.CheckTaoLichThanhToan();
                await Task.WhenAll(
                    viewModel.LoadDiscountChilds(),
                    viewModel.LoadHandoverCondition(),
                    viewModel.LoadPromotionsSelected(),
                    viewModel.LoadPromotions(),
                    viewModel.LoadCoOwners()
                    );
                SetPreOpen();
                
                if (viewModel.IsHadLichThanhToan == true)
                {
                    lookupDieuKienBanGiao.IsEnabled = true;
                    lookupPhuongThucThanhToan.IsEnabled = true;
                    lookupChieuKhau.IsEnabled = true;
                }
                if (!string.IsNullOrWhiteSpace(viewModel.Quote.bsd_discounts))
                {
                    List<string> arrDiscounts = viewModel.Quote.bsd_discounts.Split(',').ToList();
                    for (int i = 0; i < viewModel.DiscountChilds.Count; i++)
                    {
                        for (int j = 0; j < arrDiscounts.Count; j++)
                        {
                            if (viewModel.DiscountChilds[i].Val == arrDiscounts[j])
                            {
                                viewModel.DiscountChilds[i].Selected = true;
                            }
                        }
                    }
                }
                this.CheckReservation?.Invoke(0);
                _isEnableCheck = false;
            }
            else
            {
                this.CheckReservation?.Invoke(2);
            }
        }

        private void SetPreOpen()
        {
            lookupDieuKienBanGiao.HideClearButton();
            lookupPhuongThucThanhToan.HideClearButton();
            lookupChieuKhau.PreOpenOneTime = false;

            if (viewModel.IsHadLichThanhToan)
            {
                lookupDieuKienBanGiao.PreOpenOneTime = false;
                lookupPhuongThucThanhToan.PreOpenOneTime = false;
                lookupChieuKhau.HideClearButton();

                lookupDieuKienBanGiao.PreOpen = () =>
                {
                    ToastMessageHelper.ShortMessage(Language.da_co_lich_thanh_toan_khong_duoc_chinh_sua);
                };

                lookupPhuongThucThanhToan.PreOpen = () =>
                {
                    ToastMessageHelper.ShortMessage(Language.da_co_lich_thanh_toan_khong_duoc_chinh_sua);
                };

                lookupChieuKhau.PreOpen = () =>
                {
                    ToastMessageHelper.ShortMessage(Language.da_co_lich_thanh_toan_khong_duoc_chinh_sua);
                };
            }
            else
            {
                lookupDieuKienBanGiao.PreOpenAsync = async () =>
                {
                    LoadingHelper.Show();
                    await viewModel.LoadHandoverConditions();
                    LoadingHelper.Hide();
                };

                lookupPhuongThucThanhToan.PreOpenAsync = async () =>
                {
                    LoadingHelper.Show();
                    await viewModel.LoadPaymentSchemes();
                    LoadingHelper.Hide();
                };

                lookupChieuKhau.PreOpenAsync = async () =>
                {
                    LoadingHelper.Show();
                    if (viewModel.DiscountLists == null)
                    {
                        await viewModel.LoadDiscountList();
                    }

                    if (viewModel.DiscountLists == null) // dot mo ban khong co chieu khau
                    {
                        ToastMessageHelper.ShortMessage(Language.khong_co_chiet_khau);
                    }
                    LoadingHelper.Hide();
                };
            }

            lookupQuanHe.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.Relationships = RelationshipCoOwnerData.RelationshipData();
                LoadingHelper.Hide();
            };

            lookupGiuCho.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadQueues();
                LoadingHelper.Hide();
            };

            lookupLoaiHopDong.PreOpenAsync = async () =>
            {
                viewModel.ContractTypes = ContractTypeData.ContractTypes();
            };

            lookupDaiLySanGiaoDich.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadSalesAgents();
                LoadingHelper.Hide();
            };
        }

        private void ChinhSach_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radBorderChinhSach, "Active");
            VisualStateManager.GoToState(radBorderTongHop, "InActive");
            VisualStateManager.GoToState(radBorderChiTiet, "InActive");
            VisualStateManager.GoToState(lblChinhSach, "Active");
            VisualStateManager.GoToState(lblTongHop, "InActive");
            VisualStateManager.GoToState(lblChiTiet, "InActive");
            contentChinhSach.IsVisible = true;
            contentTongHop.IsVisible = false;
            contentChiTiet.IsVisible = false;
        }

        private void TongHop_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radBorderChinhSach, "InActive");
            VisualStateManager.GoToState(radBorderTongHop, "Active");
            VisualStateManager.GoToState(radBorderChiTiet, "InActive");
            VisualStateManager.GoToState(lblChinhSach, "InActive");
            VisualStateManager.GoToState(lblTongHop, "Active");
            VisualStateManager.GoToState(lblChiTiet, "InActive");
            contentChinhSach.IsVisible = false;
            contentTongHop.IsVisible = true;
            contentChiTiet.IsVisible = false;
        }

        private async void ChiTiet_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radBorderChinhSach, "InActive");
            VisualStateManager.GoToState(radBorderTongHop, "InActive");
            VisualStateManager.GoToState(radBorderChiTiet, "Active");
            VisualStateManager.GoToState(lblChinhSach, "InActive");
            VisualStateManager.GoToState(lblTongHop, "InActive");
            VisualStateManager.GoToState(lblChiTiet, "Active");
            contentChinhSach.IsVisible = false;
            contentTongHop.IsVisible = false;
            contentChiTiet.IsVisible = true;
            await SetTotal();
        }

        #region Handover Condition // Dieu kien ban giao
        private async void HandoverCondition_SelectedItemChange(object sender, EventArgs e)
        {
            if (viewModel.IsHadLichThanhToan == true)
            {
                ToastMessageHelper.ShortMessage(Language.da_co_lich_thanh_toan_khong_duoc_chinh_sua);
                return;
            }
            if (viewModel.HandoverCondition == null)
            {
                viewModel.TotalHandoverCondition = 0;
                viewModel.NetSellingPrice = 0;
                viewModel.TotalVATTax = 0;
                viewModel.MaintenanceFee = 0;
                viewModel.TotalAmount = 0;
                return;
            }
            if (viewModel.HandoverCondition.bsd_byunittype == true && (viewModel.HandoverCondition._bsd_unittype_value != viewModel.UnitType))
            {
                ToastMessageHelper.ShortMessage(Language.khong_the_them_dieu_kien_ban_giao); //Không thể thêm điều kiện bàn giao
                viewModel.HandoverCondition = null;
                return;
            }

            await viewModel.SetTotalHandoverCondition();
            isSetTotal = false;
        }
        #endregion

        #region Discount list // Chiet Khau
        private async void DiscountListItem_Changed(object sender, EventArgs e)
        {
            LoadingHelper.Show();

            if (viewModel.DiscountList == null)
            {
                viewModel.DiscountChilds.Clear();
                isSetTotal = false;
            }
            if (viewModel.DiscountChilds.Count == 0)
            {
                viewModel.DiscountChilds.Clear();
                await viewModel.LoadDiscountChilds();
            }

            viewModel.TotalDiscount = 0;
            LoadingHelper.Hide();
        }

        private async void DiscountChildItem_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            if (_isEnableCheck) return;
            if (viewModel.IsHadLichThanhToan == true)
            {
                ToastMessageHelper.ShortMessage(Language.da_co_lich_thanh_toan_khong_duoc_chinh_sua);
                return;
            }
            await viewModel.SetTotalDiscount();
            isSetTotal = false;
        }

        private void DiscountChildItem_Tapped(object sender, EventArgs e)
        {
            var item = (DiscountChildOptionSet)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item.IsExpired == true || item.IsNotApplied == true) return;

            item.Selected = !item.Selected;
        }
        #endregion

        #region Promotion // Khuyen mai
        private async void Promotion_Tapped(object sender, EventArgs e)
        {
            //if (viewModel.IsHadLichThanhToan == true)
            //{
            //    ToastMessageHelper.ShortMessage("Đã có lịch thanh toán, không được chỉnh sửa");
            //    return;
            //}
            LoadingHelper.Show();
            this.needDeletedPromotionIds = new List<string>();
            this.newSelectedPromotionIds = new List<string>();
            if (viewModel.Promotions.Count == 0)
            {
                await viewModel.LoadPromotions();
            }
            else
            {
                foreach (var itemPromotion in viewModel.Promotions)
                {
                    if (viewModel.SelectedPromotionIds.Count != 0 && viewModel.SelectedPromotionIds.Any(x => x == itemPromotion.Val))
                    {
                        itemPromotion.Selected = true;
                    }
                    else
                    {
                        itemPromotion.Selected = false;
                    }
                }
            }
            await centerModalPromotions.Show();
            LoadingHelper.Hide();
        }

        private async void PromotionItem_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var itemPromotion = (OptionSet)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            itemPromotion.Selected = !itemPromotion.Selected;
            LoadingHelper.Hide();
        }

        private async void SaveSelectedPromotion_CLicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.QuoteId == Guid.Empty)
            {
                viewModel.PromotionsSelected.Clear();
                viewModel.SelectedPromotionIds.Clear();

                foreach (var itemPromotion in viewModel.Promotions)
                {
                    if (itemPromotion.Selected)
                    {
                        viewModel.PromotionsSelected.Add(itemPromotion);
                        viewModel.SelectedPromotionIds.Add(itemPromotion.Val);
                    }
                }
            }
            else
            {
                foreach (var item in viewModel.Promotions)
                {
                    if (item.Selected == true)
                    {
                        if (viewModel.SelectedPromotionIds.Count != 0 && viewModel.SelectedPromotionIds.Any(x => x != item.Val))
                        {
                            this.newSelectedPromotionIds.Add(item.Val);
                        }
                        else
                        {
                            this.newSelectedPromotionIds.Add(item.Val);
                        }
                    }
                    else if (item.Selected == false)
                    {
                        if (viewModel.SelectedPromotionIds.Any(x => x == item.Val))
                        {
                            this.needDeletedPromotionIds.Add(item.Val);
                        }
                    }
                }

                if (this.needDeletedPromotionIds.Count != 0)
                {
                    await DeletedPromotions();
                    if (BangTinhGiaDetailPage.NeedToRefresh.HasValue) BangTinhGiaDetailPage.NeedToRefresh = true;
                    viewModel.PromotionsSelected.Clear();

                    foreach (var itemPromotion in viewModel.Promotions)
                    {
                        if (itemPromotion.Selected)
                        {
                            viewModel.PromotionsSelected.Add(itemPromotion);
                        }
                    }
                }

                if (this.newSelectedPromotionIds.Count != 0)
                {
                    bool IsSuccess = await viewModel.AddPromotion(this.newSelectedPromotionIds);
                    if (IsSuccess)
                    {
                        this.newSelectedPromotionIds.Clear();
                        viewModel.PromotionsSelected.Clear();

                        foreach (var itemPromotion in viewModel.Promotions)
                        {
                            if (itemPromotion.Selected)
                            {
                                viewModel.PromotionsSelected.Add(itemPromotion);
                            }
                        }
                        if (BangTinhGiaDetailPage.NeedToRefresh.HasValue) BangTinhGiaDetailPage.NeedToRefresh = true;
                        ToastMessageHelper.ShortMessage(Language.them_khuyen_mai_thanh_cong);
                    }
                    else
                    {
                        ToastMessageHelper.ShortMessage(Language.them_khuyen_mai_that_bai);
                    }
                }
            }
            await centerModalPromotions.Hide();
            LoadingHelper.Hide();
        }

        private async void UnSelect_Clicked(object sender, EventArgs e)
        {
            //if (viewModel.IsHadLichThanhToan == true)
            //{
            //    ToastMessageHelper.ShortMessage("Đã có lịch thanh toán, không được chỉnh sửa");
            //    return;
            //}
            LoadingHelper.Show();
            var item = (OptionSet)((sender as Label).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (viewModel.QuoteId != Guid.Empty)
            {
                DeletedPromotion(item);
            }
            else
            {
                viewModel.PromotionsSelected.Remove(item);
                viewModel.SelectedPromotionIds.Remove(item.Val);
            }
            LoadingHelper.Hide();
        }

        private async Task DeletedPromotions()
        {
            foreach (var item in needDeletedPromotionIds)
            {
                var deleteResponse = await CrmHelper.DeleteRecord($"/quotes({viewModel.QuoteId})/bsd_quote_bsd_promotion({item})/$ref");
                if (deleteResponse.IsSuccess)
                {
                    viewModel.PromotionsSelected.Remove(viewModel.PromotionsSelected.SingleOrDefault(x => x.Val == item));
                    viewModel.SelectedPromotionIds.Remove(item);
                }
            }
        }

        private async void DeletedPromotion(OptionSet item)
        {
            var conform = await DisplayAlert(Language.xac_nhan, Language.ban_co_muon_xoa_khuyen_mai_khong, Language.dong_y, Language.huy);
            if (conform == false) return;
            LoadingHelper.Show();
            var deleteResponse = await CrmHelper.DeleteRecord($"/quotes({viewModel.QuoteId})/bsd_quote_bsd_promotion({item.Val})/$ref");
            if (deleteResponse.IsSuccess)
            {
                if (BangTinhGiaDetailPage.NeedToRefresh.HasValue) BangTinhGiaDetailPage.NeedToRefresh = true;
                viewModel.PromotionsSelected.Remove(item);
                viewModel.SelectedPromotionIds.Remove(item.Val);
                ToastMessageHelper.ShortMessage(Language.xoa_khuyen_mai_thanh_cong);
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.xoa_khuyen_mai_that_bai);
            }
            LoadingHelper.Hide();
        }

        private void SearchPromotion_Pressed(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var data = viewModel.Promotions.Where(x => x.Label.ToLower().Contains(viewModel.KeywordPromotion.ToLower())).ToList();
            viewModel.Promotions.Clear();
            foreach (var item in data)
            {
                viewModel.Promotions.Add(item);
            }
            LoadingHelper.Hide();
        }

        private async void SearchPromotion_TexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.KeywordPromotion))
            {
                LoadingHelper.Show();
                viewModel.Promotions.Clear();
                await viewModel.LoadPromotions();
                LoadingHelper.Hide();
            }
        }
        #endregion

        #region Co Owner // Dong so huu
        private async void CoOwner_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.CoOwner = new CoOwnerFormModel();
            viewModel.TitleCoOwner = null;
            viewModel.CustomerCoOwner = null;
            viewModel.Relationship = null;
            await centerModalCoOwner.Show();
            LoadingHelper.Hide();
        }

        private async void UnSelectCoOwner_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = (CoOwnerFormModel)((sender as Label).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;

            if (viewModel.QuoteId != Guid.Empty)
            {
                var conform = await DisplayAlert(Language.xac_nhan, Language.ban_co_muon_xoa_nguoi_dong_so_huu_nay_khong, Language.dong_y, Language.huy);
                if (conform == false)
                {
                    LoadingHelper.Hide();
                    return;
                }
                var deleteResponse = await CrmHelper.DeleteRecord($"/bsd_coowners({item.bsd_coownerid})");
                if (deleteResponse.IsSuccess)
                {
                    viewModel.CoOwnerList.Remove(item);
                    ToastMessageHelper.ShortMessage(Language.xoa_nguoi_dong_so_huu_thanh_cong);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.xoa_nguoi_dong_so_huu_that_bai);
                    return;
                }
            }
            else
            {
                viewModel.CoOwnerList.Remove(item);
            }
            LoadingHelper.Hide();
        }

        private async void UpdateCoOwner_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            centerModalCoOwner.Title = Language.cap_nhat_dong_so_huu_title;
            var item = (CoOwnerFormModel)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            viewModel.CoOwner = new CoOwnerFormModel();
            viewModel.TitleCoOwner = null;
            viewModel.CustomerCoOwner = null;
            viewModel.Relationship = null;
            viewModel.CoOwner = item;
            if (viewModel.CoOwner.contact_id != Guid.Empty)
            {
                viewModel.CustomerCoOwner = new OptionSet(viewModel.CoOwner.contact_id.ToString(), viewModel.CoOwner.contact_name) { Title = "2" };
            }

            if (viewModel.CoOwner.account_id != Guid.Empty)
            {
                viewModel.CustomerCoOwner = new OptionSet(viewModel.CoOwner.account_id.ToString(), viewModel.CoOwner.account_name) { Title = "3" };
            }

            viewModel.TitleCoOwner = viewModel.CoOwner.bsd_name;
            viewModel.Relationship = new OptionSet(viewModel.CoOwner.bsd_relationshipId.ToString(), viewModel.CoOwner.bsd_relationship);
            await centerModalCoOwner.Show();
            LoadingHelper.Hide();
        }

        private async void SaveCoOwner_CLicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.TitleCoOwner))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_tieu_de);
                return;
            }

            if (viewModel.CustomerCoOwner == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_khach_hang);
                return;
            }

            if (viewModel.Relationship == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_quan_he);
                return;
            }

            if (viewModel.CustomerCoOwner?.Val == viewModel.Buyer?.Val)
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_khong_duoc_trung_voi_nguoi_mua);
                viewModel.CustomerCoOwner = null;
                return;
            }

            if (viewModel.CoOwner.bsd_coownerid == Guid.Empty && viewModel.CoOwnerList.Any(x => x.contact_id == Guid.Parse(viewModel.CustomerCoOwner?.Val) || x.account_id == Guid.Parse(viewModel.CustomerCoOwner?.Val)))
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_da_duoc_chon);
                return;
            }

            LoadingHelper.Show();
            if (viewModel.CustomerCoOwner.Title == "2")
            {
                viewModel.CoOwner.contact_id = Guid.Parse(viewModel.CustomerCoOwner.Val);
                viewModel.CoOwner.contact_name = viewModel.CustomerCoOwner.Label;
            }

            if (viewModel.CustomerCoOwner.Title == "3")
            {
                viewModel.CoOwner.account_id = Guid.Parse(viewModel.CustomerCoOwner.Val);
                viewModel.CoOwner.account_name = viewModel.CustomerCoOwner.Label;
            }
            viewModel.CoOwner.bsd_name = viewModel.TitleCoOwner;
            viewModel.CoOwner.bsd_relationshipId = viewModel.Relationship.Val;
            viewModel.CoOwner.bsd_relationship = viewModel.Relationship.Label;

            if (viewModel.CoOwner.bsd_coownerid == Guid.Empty)
            {
                viewModel.CoOwner.bsd_coownerid = Guid.NewGuid();

                viewModel.CoOwnerList.Add(viewModel.CoOwner);
                if (viewModel.QuoteId != Guid.Empty)
                {
                    bool IsSuccess = await viewModel.AddCoOwer();
                    if (IsSuccess)
                    {
                        if (BangTinhGiaDetailPage.NeedToRefresh.HasValue) BangTinhGiaDetailPage.NeedToRefresh = true;
                        ToastMessageHelper.ShortMessage(Language.them_dong_so_huu_thanh_cong);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.them_dong_so_huu_that_bai);
                    }
                }
                await centerModalCoOwner.Hide();
                LoadingHelper.Hide();
            }
            else
            {
                if (viewModel.QuoteId == Guid.Empty)
                {
                    List<CoOwnerFormModel> coOwnerList = new List<CoOwnerFormModel>();
                    foreach (var item in viewModel.CoOwnerList)
                    {
                        if (viewModel.CoOwner.bsd_coownerid == item.bsd_coownerid)
                        {
                            item.bsd_name = viewModel.CoOwner.bsd_name;
                            item.contact_id = viewModel.CoOwner.contact_id;
                            item.contact_name = viewModel.CoOwner.contact_name;
                            item.account_id = viewModel.CoOwner.account_id;
                            item.account_name = viewModel.CoOwner.account_name;
                            item.bsd_relationshipId = viewModel.CoOwner.bsd_relationshipId;
                            item.bsd_relationship = viewModel.CoOwner.bsd_relationship;
                        }
                        coOwnerList.Add(item);
                    }
                    viewModel.CoOwnerList.Clear();
                    coOwnerList.ForEach(x => viewModel.CoOwnerList.Add(x));
                    await centerModalCoOwner.Hide();
                    LoadingHelper.Hide();
                }
                else
                {
                    bool IsSuccess = await viewModel.UpdateCoOwner();
                    if (IsSuccess)
                    {
                        viewModel.CoOwnerList.Clear();
                        await viewModel.LoadCoOwners();
                        await centerModalCoOwner.Hide();
                        ToastMessageHelper.ShortMessage(Language.cap_nhat_dong_so_huu_thanh_cong);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.cap_nhat_dong_so_huu_that_bai);
                    }
                }
            }
        }
        #endregion

        private void Buyer_SelectedItemChange(System.Object sender, ConasiCRM.Portable.Models.LookUpChangeEvent e)
        {
            LoadingHelper.Show();
            if (viewModel.SalesAgent != null && (viewModel.SalesAgent == viewModel.Buyer))
            {
                ToastMessageHelper.LongMessage(Language.nguoi_mua_khong_duoc_trung_voi_dai_ly_san_giao_dich_vui_long_chon_lai);
                viewModel.Buyer = null;
            }
            LoadingHelper.Hide();
        }

        private void SalesAgent_SelectedItemChange(System.Object sender, ConasiCRM.Portable.Models.LookUpChangeEvent e)
        {
            LoadingHelper.Show();
            if (viewModel.SalesAgent == viewModel.Buyer)
            {
                ToastMessageHelper.LongMessage(Language.dai_ly_san_giao_dich_khong_duoc_trung_voi_nguoi_mua_vui_long_chon_lai);
                viewModel.SalesAgent = null;
            }
            LoadingHelper.Hide();
        }

        private async Task SetTotal()
        {
            if (this.isSetTotal == false)
            {
                await viewModel.SetNetSellingPrice();
                await viewModel.SetTotalVatTax();
                await viewModel.SetMaintenanceFee();
                await viewModel.SetTotalAmount();
                isSetTotal = true;
            }
        }

        private async void SaveQuote_Clicked(object sender, EventArgs e)
        {
            //if (viewModel.IsHadLichThanhToan == true)
            //{
            //    ToastMessageHelper.ShortMessage("Đã có lịch thanh toán, Vui lòng xoá lịch thanh toán để cập nhật");
            //    return;
            //}

            if (viewModel.PaymentScheme == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_phuong_thuc_thanh_toan);
                return;
            }
            if (viewModel.HandoverCondition == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_dieu_kien_ban_giao);
                return;
            }
            if (viewModel.Buyer == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_khach_hang);
                return;
            }
            if (viewModel.ContractType == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_loai_hop_dong);
                return;
            }

            if (string.IsNullOrWhiteSpace(viewModel.Quote.name))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_tieu_de);
                return;
            }
            if (viewModel.SalesAgent == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_dai_ly_san_giao_dich);
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.Quote.bsd_waivermanafeemonth))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_so_thang_mien_giam_phi_quan_ly);
                return;
            }
            if (int.Parse(viewModel.Quote.bsd_waivermanafeemonth) > 100)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_so_thang_mien_giam_phi_quan_ly_khong_qua_100);
                return;
            }

            if (viewModel.CustomerCoOwner?.Val == viewModel.Buyer?.Val)
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_co_owner_va_khach_hang_khong_duoc_trung);
                return;
            }



            LoadingHelper.Show();
            if (viewModel.QuoteId == Guid.Empty)
            {
                await SetTotal();
                bool isSuccess = await viewModel.CreateQuote();
                if (isSuccess)
                {
                    await Task.WhenAll(
                        viewModel.AddCoOwer(),
                        viewModel.AddPromotion(viewModel.SelectedPromotionIds),
                        viewModel.AddHandoverCondition(),
                        viewModel.CreateQuoteProduct()
                        );
                    if (QueuesDetialPage.NeedToRefreshBTG.HasValue) QueuesDetialPage.NeedToRefreshBTG = true;
                    if (ReservationList.NeedToRefreshReservationList.HasValue) ReservationList.NeedToRefreshReservationList = true;
                    if (UnitInfo.NeedToRefreshQuotation.HasValue) UnitInfo.NeedToRefreshQuotation = true;
                    await Navigation.PopAsync();
                    ToastMessageHelper.ShortMessage(Language.tao_bang_tinh_gia_thanh_cong);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.tao_bang_tinh_gia_that_bai);
                }
            }
            else
            {
                await SetTotal();
                if (viewModel.HandoverCondition_Update != null && (viewModel.HandoverCondition_Update?.Val != viewModel.HandoverCondition.Val))
                {
                    var response = await CrmHelper.DeleteRecord($"/quotes({viewModel.QuoteId})/bsd_quote_bsd_packageselling({viewModel.HandoverCondition_Update.Val})/$ref");
                    if (response.IsSuccess)
                    {
                        bool isSuccess_Update = await viewModel.AddHandoverCondition();
                        if (isSuccess_Update)
                        {
                            viewModel.HandoverCondition_Update = viewModel.HandoverCondition;
                        }
                        else
                        {
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage(Language.cap_nhat_dieu_kien_ban_giao_that_bai);
                            return;
                        }
                    }
                    else
                    {
                        viewModel.HandoverCondition = viewModel.HandoverCondition_Update;
                    }
                }

                var isSuccess = await viewModel.UpdateQuote();
                if (isSuccess != null && isSuccess.IsSuccess)
                {
                    if (QueuesDetialPage.NeedToRefreshBTG.HasValue) QueuesDetialPage.NeedToRefreshBTG = true;
                    if (BangTinhGiaDetailPage.NeedToRefresh.HasValue) BangTinhGiaDetailPage.NeedToRefresh = true;
                    if (ReservationList.NeedToRefreshReservationList.HasValue) ReservationList.NeedToRefreshReservationList = true;
                    if (UnitInfo.NeedToRefreshQuotation.HasValue) UnitInfo.NeedToRefreshQuotation = true;
                    await Navigation.PopAsync();
                    ToastMessageHelper.ShortMessage(Language.cap_nhat_bang_tinh_gia_thanh_cong);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(isSuccess.GetErrorMessage());
                }
            }
        }
    }
}