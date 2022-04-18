using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Resources;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BangTinhGiaDetailPage : ContentPage
    {
        private BangTinhGiaDetailPageViewModel viewModel;
        public Action<bool> OnCompleted;
        private Guid ReservationId;
        public static bool? NeedToRefresh = null; 
        public static bool? NeedToRefreshInstallment = null;

        public BangTinhGiaDetailPage(Guid id)
        {
            InitializeComponent();
            ReservationId = id;
            BindingContext = viewModel = new BangTinhGiaDetailPageViewModel();
            NeedToRefresh = false;
            NeedToRefreshInstallment = false;
            Tab_Tapped(1);
            Init();
        }

        public async void Init()
        {
            await Task.WhenAll(
                LoadDataChinhSach(ReservationId),
                viewModel.LoadCoOwners(ReservationId)
                );

            SetUpButtonGroup();
            if (viewModel.Reservation.quoteid != Guid.Empty)
            {
                OnCompleted?.Invoke(true);
            }
            else
                OnCompleted?.Invoke(false);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefresh == true)
            {
                LoadingHelper.Show();

                viewModel.CoownerList.Clear();
                viewModel.ListDiscount.Clear();
                viewModel.ListPromotion.Clear();
                // reload lịch
                viewModel.ShowInstallmentList = false;
                viewModel.NumberInstallment = 0;
                viewModel.InstallmentList.Clear();

                await Task.WhenAll(
                    LoadDataChinhSach(ReservationId),
                    viewModel.LoadCoOwners(ReservationId)
                );
                viewModel.ButtonCommandList.Clear();
                SetUpButtonGroup();
                if (QueuesDetialPage.NeedToRefreshBTG.HasValue) QueuesDetialPage.NeedToRefreshBTG = true;
                NeedToRefresh = false;

                LoadingHelper.Hide();
            }
            if (NeedToRefreshInstallment == true)
            {
                LoadingHelper.Show();

                viewModel.ShowInstallmentList = false;
                viewModel.NumberInstallment = 0;
                viewModel.InstallmentList.Clear();
                viewModel.ButtonCommandList.Clear();

                await viewModel.LoadInstallmentList(ReservationId);    
                SetUpButtonGroup();
                NeedToRefreshInstallment = false;

                LoadingHelper.Hide();
            }
        }

        //tab chinh sach

        private async Task LoadDataChinhSach(Guid id)
        {
            if (id != Guid.Empty)
            {
                await Task.WhenAll(
                    viewModel.LoadReservation(id),
                    viewModel.LoadPromotions(ReservationId),
                    viewModel.LoadSpecialDiscount(ReservationId),
                    viewModel.LoadInstallmentList(ReservationId)
                    );
                await viewModel.LoadHandoverCondition(ReservationId);
                await viewModel.LoadDiscounts();
               // SutUpSpecialDiscount();
            }
        }

        // tab lich
        private async void LoadInstallmentList(Guid id)
        {
            if (id != Guid.Empty && viewModel.InstallmentList != null && viewModel.InstallmentList.Count == 0)
            {
                LoadingHelper.Show();
                await viewModel.LoadInstallmentList(id);
                LoadingHelper.Hide();
            }
        }

        private void ChinhSach_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(1);
        }

        private void TongHop_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(2);
        }

        private void ChiTiet_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(3);
        }

        private void Lich_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(4);
            if (viewModel.InstallmentList.Count == 0)
            {
                LoadInstallmentList(ReservationId);
            }
        }

        private void Tab_Tapped(int tab)
        {
            if (tab == 1)
            {
                VisualStateManager.GoToState(radBorderChinhSach, "Selected");
                VisualStateManager.GoToState(lbChinhSach, "Selected");
                TabChinhSach.IsVisible = true;
            }
            else
            {
                VisualStateManager.GoToState(radBorderChinhSach, "Normal");
                VisualStateManager.GoToState(lbChinhSach, "Normal");
                TabChinhSach.IsVisible = false;
            }
            if (tab == 2)
            {
                VisualStateManager.GoToState(radBorderTongHop, "Selected");
                VisualStateManager.GoToState(lbTongHop, "Selected");
                TabTongHop.IsVisible = true;
            }
            else
            {
                VisualStateManager.GoToState(radBorderTongHop, "Normal");
                VisualStateManager.GoToState(lbTongHop, "Normal");
                TabTongHop.IsVisible = false;
            }
            if (tab == 3)
            {
                VisualStateManager.GoToState(radBorderChiTiet, "Selected");
                VisualStateManager.GoToState(lbChiTiet, "Selected");
                TabChiTiet.IsVisible = true;
            }
            else
            {
                VisualStateManager.GoToState(radBorderChiTiet, "Normal");
                VisualStateManager.GoToState(lbChiTiet, "Normal");
                TabChiTiet.IsVisible = false;
            }
            if (tab == 4)
            {
                VisualStateManager.GoToState(radBorderLich, "Selected");
                VisualStateManager.GoToState(lbLich, "Selected");
                TabLich.IsVisible = true;
            }
            else
            {
                VisualStateManager.GoToState(radBorderLich, "Normal");
                VisualStateManager.GoToState(lbLich, "Normal");
                TabLich.IsVisible = false;
            }
        }

        private void SutUpSpecialDiscount()
        {
            if (viewModel.ListSpecialDiscount != null && viewModel.ListSpecialDiscount.Count > 0)
            {
                stackLayoutSpecialDiscount.IsVisible = true;
                foreach (var item in viewModel.ListSpecialDiscount)
                {
                    if (!string.IsNullOrEmpty(item.Label))
                    {
                        stackLayoutSpecialDiscount.Children.Add(SetUpItem(item.Label));
                    }
                }
            }
            else
            {
                stackLayoutSpecialDiscount.IsVisible = false;
            }
        }

        private void SetUpButtonGroup()
        {
            if (viewModel.Reservation.statuscode == 100000000)// show khi statuscode  == 1000000 (Reservation)
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.huy_dat_coc, "FontAwesomeSolid", "\uf05e", null, CancelDeposit));
            }
            if (viewModel.Reservation.statuscode == 3)// show khi statuscode == 3(Deposited)
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.de_nghi_thanh_ly, "FontAwesomeSolid", "\uf560", null, FULTerminate));
            }

            if (viewModel.Reservation.statuscode == 100000007)
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.cap_nhat, "FontAwesomeRegular", "\uf044", null, EditQuotes));
                // chỉnh sửa menu
                if (viewModel.InstallmentList.Count == 0)
                {
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.tao_lich_thanh_toan, "FontAwesomeRegular", "\uf271", null, CreatePaymentScheme));
                }
                else
                { viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.xoa_lich_thanh_toan, "FontAwesomeRegular", "\uf1c3", null, CancelInstallment)); 
                }
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.xac_nhan_in, "FontAwesomeSolid", "\uf02f", null, ConfirmSigning));
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.huy_bang_tinh_gia, "FontAwesomeRegular", "\uf273", null, CancelQuotes));
                if (viewModel.InstallmentList.Count > 0 && viewModel.Reservation.bsd_quotationprinteddate != null)
                {
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.ky_bang_tinh_gia, "FontAwesomeRegular", "\uf274", null, SignQuotationClicked));
                }
            }

            if (viewModel.Reservation.bsd_reservationformstatus == 100000001 && viewModel.Reservation.bsd_reservationprinteddate != null && viewModel.Reservation.bsd_reservationuploadeddate == null && viewModel.Reservation.bsd_rfsigneddate == null)
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.xac_nhan_tai_pdc, "FontAwesomeRegular", "\uf15c", null, ConfirmReservation));
            }
            if (viewModel.Reservation.bsd_reservationformstatus == 100000001 && viewModel.Reservation.bsd_reservationprinteddate != null && viewModel.Reservation.bsd_reservationuploadeddate != null && viewModel.Reservation.bsd_rfsigneddate == null)
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.ky_phieu_dat_coc, "FontAwesomeRegular", "\uf274", null, CompletedReservation));
            }

            if (viewModel.ButtonCommandList.Count > 0)
            {
                floatingButtonGroup.IsVisible = true;
            }
            else
            {
                floatingButtonGroup.IsVisible = false;
            }
        }

        private async void FULTerminate(object sender, EventArgs e)
        {
            if (viewModel.Reservation != null && viewModel.Reservation.quoteid != Guid.Empty)
            {
                LoadingHelper.Show();
                var fulid = await viewModel.FULTerminate();
                if (fulid != Guid.Empty)
                {
                    FollowUpListForm newPage = new FollowUpListForm(fulid);
                    newPage.OnCompleted = async (OnCompleted) =>
                    {
                        if (OnCompleted == true)
                        {
                            await Navigation.PushAsync(newPage);
                            LoadingHelper.Hide();
                        }
                        else
                        {
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                        }
                    };
                    ToastMessageHelper.ShortMessage(Language.da_tao_danh_sach_theo_doi);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.de_nghi_thanh_ly_that_bai);
                }
            }
        }

        private async void CancelDeposit(object sender, EventArgs e)
        {
            if (viewModel.Reservation.quoteid != Guid.Empty)
            {
                LoadingHelper.Show();
                if (await viewModel.CancelDeposit())
                {
                    NeedToRefresh = true;
                    OnAppearing();
                    if (DirectSaleDetail.NeedToRefreshDirectSale.HasValue) DirectSaleDetail.NeedToRefreshDirectSale = true;
                    if (ReservationList.NeedToRefreshReservationList.HasValue) ReservationList.NeedToRefreshReservationList = true;
                    if (DatCocList.NeedToRefresh.HasValue) DatCocList.NeedToRefresh = true;
                    if (QueuesDetialPage.NeedToRefreshDC.HasValue) QueuesDetialPage.NeedToRefreshDC = true;
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.da_huy_dat_coc);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.huy_dat_coc_that_bai_vui_long_thu_lai);
                }
            }
        }

        private async void CancelInstallment(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.Reservation.quoteid != Guid.Empty)
            {
                if (await viewModel.DeactiveInstallment())
                {
                    NeedToRefreshInstallment = true;
                    OnAppearing();
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.da_xoa_lich_thanh_toan);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.xoa_lich_thanh_toan_that_bai_vui_long_thu_lai);
                }
            }
        }

        private async void ConfirmSigning(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.Reservation.bsd_quotationprinteddate.HasValue)
            {
                ToastMessageHelper.ShortMessage(Language.da_xac_nhan_in);
                LoadingHelper.Hide();
                return;
            }
            if (viewModel.InstallmentList.Count == 0)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_tao_lich_thanh_toan);
                LoadingHelper.Hide();
                return;
            }
            bool isSuccess = await viewModel.ConfirmSinging();
            if (isSuccess)
            {
                NeedToRefresh = true;
                NeedToRefreshInstallment = true;
                if (DirectSaleDetail.NeedToRefreshDirectSale.HasValue) DirectSaleDetail.NeedToRefreshDirectSale = true;
                OnAppearing();
                ToastMessageHelper.ShortMessage(Language.xac_nhan_in_thanh_cong);
            }
            else
            {
                ToastMessageHelper.ShortMessage(Language.xac_nhan_in_that_bai);
            }
            LoadingHelper.Hide();
        }

        private async void ConfirmReservation(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.Reservation.quoteid != Guid.Empty)
            {
                viewModel.Reservation.bsd_reservationuploadeddate = DateTime.Now;
                if (await viewModel.UpdateQuotes(viewModel.ConfirmReservation))
                {
                    NeedToRefresh = true;
                    OnAppearing();
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.da_xac_nhan_tai_phieu_dat_coc);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.xac_nhan_tai_phieu_dat_coc_that_bai_vui_long_thu_lai);
                }
            }
        }

        private void EditQuotes(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            ReservationForm reservation = new ReservationForm(this.ReservationId);
            reservation.CheckReservation = async (isSuccess) =>
            {
                if (isSuccess == 0)
                {
                    await Navigation.PushAsync(reservation);
                    LoadingHelper.Hide();
                }
                else if (isSuccess == 1)
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.san_pham_dang_o_trang_thai_reserve_khong_the_tao_bang_tinh_gia);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_co_thong_tin_bang_tinh_gia);
                }
            };
        }

        private async void CreatePaymentScheme(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            string IsSuccess = await viewModel.UpdatePaymentScheme();
            if (IsSuccess == "True")
            {
                NeedToRefreshInstallment = true;
                OnAppearing();
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(Language.tao_lich_thanh_toan_thanh_cong);
            }
            else
            {
                if (IsSuccess == "Localization")
                {
                    string asw = await App.Current.MainPage.DisplayActionSheet(Language.khach_hang_chua_chon_quoc_tich, Language.huy, Language.them_quoc_tich);
                    if (asw == Language.them_quoc_tich)
                    {
                        if (!string.IsNullOrEmpty(viewModel.Reservation.purchaser_contact_name))
                        {
                            await App.Current.MainPage.Navigation.PushAsync(new ContactForm(Guid.Parse(viewModel.Customer.Val)));
                        }
                        else
                        {
                            await App.Current.MainPage.Navigation.PushAsync(new AccountForm(Guid.Parse(viewModel.Customer.Val)));
                        }
                    }
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.tao_lich_thanh_toan_that_bai_vui_long_thu_lai);
                }    
            }
        }

        private async void CompletedReservation(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.Reservation.quoteid != Guid.Empty)
            {
                viewModel.Reservation.bsd_reservationformstatus = 100000002;
                viewModel.Reservation.bsd_rfsigneddate = DateTime.Now;
                if (await viewModel.UpdateQuotes(viewModel.UpdateReservation))
                {
                    NeedToRefresh = true;
                    OnAppearing();
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.phieu_dat_coc_da_duoc_ky);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.ky_phieu_dat_coc_that_bai_vui_long_thu_lai);
                }
            }
        }

        private async void SignQuotationClicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.InstallmentList.Count == 0)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_tao_lich_thanh_toan);
                LoadingHelper.Hide();
                return;
            }
            if (viewModel.Reservation.quoteid != Guid.Empty)
            {
                if (await viewModel.SignQuotation())
                {
                    NeedToRefresh = true;
                    OnAppearing();
                    if (DirectSaleDetail.NeedToRefreshDirectSale.HasValue) DirectSaleDetail.NeedToRefreshDirectSale = true;
                    if (ReservationList.NeedToRefreshReservationList.HasValue) ReservationList.NeedToRefreshReservationList = true;
                    if (QueuesDetialPage.NeedToRefreshBTG.HasValue) QueuesDetialPage.NeedToRefreshBTG = true;
                    if (QueuesDetialPage.NeedToRefreshDC.HasValue) QueuesDetialPage.NeedToRefreshDC = true;
                    this.Title = Language.dat_coc_title;
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.bang_tinh_gia_da_duoc_ky);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.ky_bang_tinh_gia_that_bai_vui_long_thu_lai);
                }
            }
        }

        private async void CancelQuotes(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            string options = await DisplayActionSheet(Language.huy_bang_tinh_gia, Language.dong, Language.xac_nhan);
            if (options == Language.xac_nhan)
            {
                viewModel.Reservation.statecode = 3;
                viewModel.Reservation.statuscode = 6;
                if (await viewModel.UpdateQuotes(viewModel.UpdateQuote))
                {
                    NeedToRefresh = true;
                    OnAppearing();
                    if (DirectSaleDetail.NeedToRefreshDirectSale.HasValue) DirectSaleDetail.NeedToRefreshDirectSale = true;
                    if (ReservationList.NeedToRefreshReservationList.HasValue) ReservationList.NeedToRefreshReservationList = true;
                    if (QueuesDetialPage.NeedToRefreshBTG.HasValue) QueuesDetialPage.NeedToRefreshBTG = true;
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.da_huy_bang_tinh_gia);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.huy_bang_tinh_gia_that_bai_vui_long_thu_lai);
                }
            }
            LoadingHelper.Hide();
        }

        private Grid SetUpItem(string content)
        {
            Grid grid = new Grid();
            Label lb = new Label();
            lb.Text = content;
            lb.FontSize = 15;
            lb.TextColor = Color.FromHex("1399D5");
            lb.VerticalOptions = LayoutOptions.Center;
            lb.HorizontalOptions = LayoutOptions.End;
            lb.FontAttributes = FontAttributes.Bold;
            grid.Children.Add(lb);
            TapGestureRecognizer item_tap = new TapGestureRecognizer();
            item_tap.SetBinding(TapGestureRecognizer.CommandParameterProperty, new Binding("."));
            item_tap.Tapped += SpecialDiscountItem_Tapped;
            return grid;
        }

        private void SpecialDiscountItem_Tapped(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Project_Tapped(object sender, EventArgs e)
        {
            if (viewModel.Reservation.project_id != Guid.Empty)
            {
                LoadingHelper.Show();
                ProjectInfo projectInfo = new ProjectInfo(viewModel.Reservation.project_id);
                projectInfo.OnCompleted = async (IsSuccess) =>
                {
                    if (IsSuccess == true)
                    {
                        await Navigation.PushAsync(projectInfo);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                    }
                };
            }
        }
        private void SalesCompany_Tapped(object sender, EventArgs e)
        {
            if (viewModel.Reservation.salescompany_accountid != Guid.Empty)
            {
                LoadingHelper.Show();
                AccountDetailPage newPage = new AccountDetailPage(viewModel.Reservation.salescompany_accountid);
                newPage.OnCompleted = async (OnCompleted) =>
                {
                    if (OnCompleted == true)
                    {
                        await Navigation.PushAsync(newPage);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                    }
                };
            }
        }
        private void Customer_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if(viewModel.Customer != null)
            {
                if(viewModel.Customer.Title == viewModel.CodeAccount)
                {
                    AccountDetailPage newPage = new AccountDetailPage(viewModel.Reservation.purchaser_accountid);
                    newPage.OnCompleted = async (OnCompleted) =>
                    {
                        if (OnCompleted == true)
                        {
                            await Navigation.PushAsync(newPage);
                            LoadingHelper.Hide();
                        }
                        else
                        {
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                        }
                    };
                }
                else if (viewModel.Customer.Title == viewModel.CodeContact)
                {
                    ContactDetailPage newPage = new ContactDetailPage(viewModel.Reservation.purchaser_contactid);
                    newPage.OnCompleted = async (OnCompleted) =>
                    {
                        if (OnCompleted == true)
                        {
                            await Navigation.PushAsync(newPage);
                            LoadingHelper.Hide();
                        }
                        else
                        {
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                        }
                    };
                }
            }
        }

        private void CloseContentPromotion_Tapped(object sender, EventArgs e)
        {
            ContentPromotion.IsVisible = false;
        }

        private async void stackLayoutPromotions_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = ((TapGestureRecognizer)((Label)sender).GestureRecognizers[0]).CommandParameter as OptionSet;
            if (item != null && item.Val != string.Empty)
            {
                if (viewModel.PromotionItem == null)
                {
                    await viewModel.LoadPromotionItem(item.Val);
                }
                else if (viewModel.PromotionItem.bsd_promotionid.ToString() != item.Val)
                {
                    await viewModel.LoadPromotionItem(item.Val);
                }
            }
            if (viewModel.PromotionItem != null)
                ContentPromotion.IsVisible = true;
            LoadingHelper.Hide();
        }

        private void ContentHandoverCondition_Tapped(object sender, EventArgs e)
        {
            ContentHandoverCondition.IsVisible = false;
        }

        private async void HandoverConditionItem_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.HandoverConditionItem == null && viewModel.Reservation.handovercondition_id != Guid.Empty)
            {
                await viewModel.LoadHandoverConditionItem(viewModel.Reservation.handovercondition_id);
            }
            if (viewModel.HandoverConditionItem != null)
                ContentHandoverCondition.IsVisible = true;
            LoadingHelper.Hide();
        }

        private void ContentSpecialDiscount_Tapped(object sender, EventArgs e)
        {
            ContentSpecialDiscount.IsVisible = false;
        }

        private async void stackLayoutSpecialDiscount_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = ((TapGestureRecognizer)((Label)sender).GestureRecognizers[0]).CommandParameter as OptionSet;
            if (item != null && item.Val != string.Empty)
            {
                if (viewModel.DiscountSpecialItem == null)
                {
                    await viewModel.LoadDiscountSpecialItem(item.Val);
                }
                else if (viewModel.DiscountSpecialItem.bsd_discountspecialid.ToString() != item.Val)
                {
                    await viewModel.LoadDiscountSpecialItem(item.Val);
                }
            }
            if (viewModel.DiscountSpecialItem != null)
                ContentSpecialDiscount.IsVisible = true;
            LoadingHelper.Hide();
        }

        private async void DiscountList_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = ((TapGestureRecognizer)((RadBorder)sender).GestureRecognizers[0]).CommandParameter as OptionSet;
            if (item != null && item.Val != string.Empty)
            {
                if (viewModel.DiscountSpecialItem == null)
                {
                    await viewModel.LoadDiscountList(item.Val);
                }
                else if (viewModel.DiscountSpecialItem.bsd_discountspecialid.ToString() != item.Val)
                {
                    await viewModel.LoadDiscountList(item.Val);
                }
            }
            if (viewModel.Discount != null)
                ContentDiscountList.IsVisible = true;
            LoadingHelper.Hide();
        }

        private void CloseContentDiscount_Tapped(object sender, EventArgs e)
        {
            ContentDiscountList.IsVisible = false;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = ((TapGestureRecognizer)((RadBorder)sender).GestureRecognizers[0]).CommandParameter as OptionSet;
            if (item != null && item.Val != string.Empty)
            {
                if (viewModel.DiscountSpecialItem == null)
                {
                    await viewModel.LoadDiscountList(item.Val);
                }
                else if (viewModel.DiscountSpecialItem.bsd_discountspecialid.ToString() != item.Val)
                {
                    await viewModel.LoadDiscountList(item.Val);
                }
            }
            if (viewModel.Discount != null)
                ContentDiscountList.IsVisible = true;
            LoadingHelper.Hide();
        }
    }
}