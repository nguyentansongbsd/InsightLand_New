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

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservationForm : ContentPage
    {
        public Action<bool> CheckReservation;
        public ReservationFormViewModel viewModel;

        public ReservationForm(Guid quoteId)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ReservationFormViewModel();
            centerModalPromotions.Body.BindingContext = viewModel;
            centerModalCoOwner.Body.BindingContext = viewModel;
            viewModel.QuoteId = quoteId;
            InitUpdate();
        }

        public ReservationForm(Guid productId , OptionSet queue)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ReservationFormViewModel();
            centerModalPromotions.Body.BindingContext = viewModel;
            centerModalCoOwner.Body.BindingContext = viewModel;
            viewModel.ProductId = productId;
            viewModel.Queue = queue;
            Init();
        }

        public async void Init()
        {
            await viewModel.LoadUnitInfor();
            if (viewModel.UnitInfor != null)
            {
                if (viewModel.Queue != null)
                {
                    lookupGiuCho.IsEnabled = false;
                }
                await viewModel.LoadTaxCode();
                SetPreOpen();
                CheckReservation?.Invoke(true);
            }
            else
            {
                CheckReservation?.Invoke(false);
            }
        }

        public async void InitUpdate()
        {
            await viewModel.LoadQuote();
            if (viewModel.Quote != null)
            {
                SetPreOpen();
                await viewModel.LoadDiscountChilds();
                await viewModel.LoadHandoverCondition();
                await viewModel.LoadPromotionsSelected();
                await viewModel.LoadPromotions();
                await viewModel.LoadCoOwners();
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
                this.CheckReservation?.Invoke(true);
            }
            else
            {
                this.CheckReservation?.Invoke(false);
            }
        }

        private void SetPreOpen()
        {
            lookupPhuongThucThanhToan.HideClearButton();
            lookupPhuongThucThanhToan.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadPaymentSchemes();
                LoadingHelper.Hide();
            };

            lookupDieuKienBanGiao.PreOpenAsync = async () => {
                LoadingHelper.Show();
                await viewModel.LoadHandoverConditions();
                LoadingHelper.Hide();
            };

            lookupChieuKhau.PreOpenOneTime = false;
            lookupChieuKhau.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                if (viewModel.DiscountLists == null)
                {
                    await viewModel.LoadDiscountList();
                }
                
                if (viewModel.DiscountLists == null) // dot mo ban khong co chieu khau
                {
                    ToastMessageHelper.ShortMessage("Không có chiết khấu");
                }
                LoadingHelper.Hide();
            };

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

        private void ChiTiet_Tapped(object sender, EventArgs e)
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
        }

        #region Handover Condition // Dieu kien ban giao
        private void HandoverCondition_SelectedItemChange(object sender, EventArgs e)
        {
            if (viewModel.HandoverCondition == null)
            {
                viewModel.TotalHandoverCondition = 0;
                viewModel.NetSellingPrice = 0;
                viewModel.TotalVATTax = 0;
                viewModel.MaintenanceFee = 0;
                viewModel.TotalAmount = 0;
                return;
            }
            if (viewModel.HandoverCondition.bsd_byunittype == true && (viewModel.HandoverCondition._bsd_unittype_value != viewModel.UnitInfor._bsd_unittype_value))
            {
                ToastMessageHelper.ShortMessage("Không thể thêm điều kiện bàn giao");
                viewModel.HandoverCondition = null;
                return;
            }
            viewModel.SetTotalHandoverCondition();
        }
        #endregion

        #region Discount list // Chiet Khau
        private async void DiscountListItem_Changed(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.DiscountChilds.Clear();
            await viewModel.LoadDiscountChilds();
            viewModel.TotalDiscount = 0;
            LoadingHelper.Hide();
        }

        private void DiscountChildItem_Tapped(object sender, EventArgs e)
        {
            var item = (DiscountChildOptionSet)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            item.Selected = !item.Selected;
            viewModel.SetTotalDiscount();
        }
        #endregion

        #region Promotion // Khuyen mai
        private async void Promotion_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
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

        private void PromotionItem_Tapped(object sender, EventArgs e)
        {
            var itemPromotion = (OptionSet)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            itemPromotion.Selected = !itemPromotion.Selected;
        }

        private async void SaveSelectedPromotion_CLicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
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
            await centerModalPromotions.Hide();
            LoadingHelper.Hide();
        }

        private void UnSelect_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = (OptionSet)((sender as Label).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            viewModel.PromotionsSelected.Remove(item);
            viewModel.SelectedPromotionIds.Remove(item.Val);
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

        private void UnSelectCoOwner_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = (CoOwnerFormModel)((sender as Label).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            viewModel.CoOwnerList.Remove(item);
            LoadingHelper.Hide();
        }

        private async void UpdateCoOwner_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = (CoOwnerFormModel)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            viewModel.CoOwner = new CoOwnerFormModel();
            viewModel.TitleCoOwner = null;
            viewModel.CustomerCoOwner = null;
            viewModel.Relationship = null;
            viewModel.CoOwner = item;
            if (viewModel.CoOwner.contact_id != Guid.Empty)
            {
                viewModel.CustomerCoOwner = new OptionSet(viewModel.CoOwner.contact_id.ToString(), viewModel.CoOwner.contact_name);
            }

            if (viewModel.CoOwner.account_id != Guid.Empty)
            {
                viewModel.CustomerCoOwner = new OptionSet(viewModel.CoOwner.account_id.ToString(), viewModel.CoOwner.account_name);
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
                ToastMessageHelper.ShortMessage("Vui lòng nhập tiêu đề");
                return;
            }

            if (viewModel.CustomerCoOwner == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn khách hàng");
                return;
            }

            if (viewModel.Relationship == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn quan hệ");
                return;
            }

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
            }
            else
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
            }
            await centerModalCoOwner.Hide();
        }
        #endregion

        private void Buyer_SelectedItemChange(System.Object sender, ConasiCRM.Portable.Models.LookUpChangeEvent e)
        {
            LoadingHelper.Show();
            if (viewModel.SalesAgent != null && (viewModel.SalesAgent == viewModel.Buyer))
            {
                ToastMessageHelper.LongMessage("Người mua không được trùng với Đại lý/Sàn giao dịch. Vui lòng chọn lại.");
                viewModel.Buyer = null;
            }
            LoadingHelper.Hide();
        }

        private void SalesAgent_SelectedItemChange(System.Object sender, ConasiCRM.Portable.Models.LookUpChangeEvent e)
        {
            LoadingHelper.Show();
            if (viewModel.SalesAgent == viewModel.Buyer)
            {
                ToastMessageHelper.LongMessage("Đại lý/Sàn giao dịch không được trùng với Người mua. Vui lòng chọn lại.");
                viewModel.SalesAgent = null;
            }
            LoadingHelper.Hide();
        }

        private async void SaveQuote_Clicked(object sender, EventArgs e)
        {
            if (viewModel.PaymentScheme == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn phương thức thanh toán");
                return;
            }
            if (viewModel.HandoverCondition == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn điều kiện bàn giao");
                return;
            }
            if (viewModel.Buyer == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn người mua");
                return;
            }
            if (viewModel.ContractType == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn loại hợp đồng");
                return;
            }
            
            if (string.IsNullOrWhiteSpace(viewModel.Quote.name))
            {
                ToastMessageHelper.ShortMessage("Vui lòng nhập mô tả");
                return;
            }
            if (viewModel.SalesAgent == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn Đại lý/Sàn giao dịch");
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.Quote.bsd_waivermanafeemonth))
            {
                ToastMessageHelper.ShortMessage("Vui lòng điền số tháng miễn giảm phí quản lý");
                return;
            }

            LoadingHelper.Show();

            if (viewModel.Quote.quoteid == Guid.Empty)
            {
                bool isSuccess = await viewModel.CreateQuote();
                if (isSuccess)
                {
                    await Task.WhenAll(
                        viewModel.AddCoOwer(),
                        viewModel.AddPromotion(),
                        viewModel.AddHandoverCondition(),
                        viewModel.CreateQuoteProduct()
                        );

                    await Navigation.PopAsync();
                    ToastMessageHelper.ShortMessage("Tạo bảng tính giá thành công");
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Tạo bảng tính giá thất bại");
                }
            }
            else
            {
                bool isSuccess = await viewModel.UpdateQuote();
                if (isSuccess)
                {
                    ToastMessageHelper.ShortMessage("Cập nhật bảng tính giá thành công");
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Cập nhật bảng tính giá thất bại");
                }
            }
        }
    }
}