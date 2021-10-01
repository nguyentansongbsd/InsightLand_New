using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public BangTinhGiaDetailPage(Guid id)
        {
            InitializeComponent();
            ReservationId = id;
            BindingContext = viewModel = new BangTinhGiaDetailPageViewModel();
            NeedToRefresh = false;
            LoadingHelper.Show();
            Tab_Tapped(1);
            Init();
        }

        public async void Init()
        {
            await LoadDataChinhSach(ReservationId);
            await viewModel.LoadCoOwners(ReservationId);
            SetUpButtonGroup();

            if (viewModel.Reservation.quoteid != Guid.Empty)
                OnCompleted?.Invoke(true);
            else
                OnCompleted?.Invoke(false);
            LoadingHelper.Hide();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefresh == true)
            {
                LoadingHelper.Show();
                await LoadDataChinhSach(ReservationId);
                await viewModel.LoadCoOwners(ReservationId);
                viewModel.ButtonCommandList.Clear();
                SetUpButtonGroup();
                LoadingHelper.Hide();
                NeedToRefresh = false;
            }
        }

        //tab chinh sach

        private async Task LoadDataChinhSach(Guid id)
        {
            if(id != Guid.Empty)
            {
                LoadingHelper.Show();
                await viewModel.LoadReservation(id);
                SutUpPromotions();
                SutUpSpecialDiscount();
                SetUpDiscount(viewModel.Reservation.bsd_discounts);
                LoadingHelper.Hide();
            }
        }

        // tab lich
        private async void LoadInstallmentList(Guid id)
        {
            if (id != Guid.Empty &&viewModel.InstallmentList != null && viewModel.InstallmentList.Count == 0)
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
            LoadInstallmentList(ReservationId);
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

        private async void SetUpDiscount(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                scrolltDiscount.IsVisible = true;
                if (viewModel.Reservation.bsd_discounttypeid != Guid.Empty)
                {
                    await viewModel.LoadDiscounts(viewModel.Reservation.bsd_discounttypeid);

                    var list_id = ids.Split(',');

                    foreach (var id in list_id)
                    {
                        OptionSet item = viewModel.ListDiscount.Single(x => x.Val == id);
                        if(item != null && !string.IsNullOrEmpty(item.Val))
                        {
                            stackLayoutDiscount.Children.Add(SetUpItemBorder(item.Label));
                        }
                    }
                }
            }
            else
            {
                scrolltDiscount.IsVisible = false;
            }    
        }

        private void SutUpPromotions()
        {
            if(viewModel.ListPromotion != null && viewModel.ListPromotion.Count>0)
            {
                stackLayoutPromotions.IsVisible = true;
                foreach(var item in viewModel.ListPromotion)
                {
                    if(!string.IsNullOrEmpty(item.Label))
                    {
                        stackLayoutPromotions.Children.Add(SetUpItem(item.Label));
                    }    
                }    
            }    
            else
            {
                stackLayoutPromotions.IsVisible = false;
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
            if (viewModel.Reservation.statuscode == 100000007)
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem("Hủy Bảng Tính Giá", "FontAwesomeRegular", "\uf273", null, CancelQuotes));
            }
            if (viewModel.Reservation.statuscode == 100000007)
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem("Cập Nhật Bảng Tính Giá", "FontAwesomeRegular", "\uf044", null, EditQuotes));
            }
            if (viewModel.Reservation.statuscode == 100000007 && viewModel.Reservation.bsd_quotationprinteddate == null && viewModel.Reservation.bsd_quotationsigneddate == null)
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem("Tạo Lịch Thanh Toán", "FontAwesomeRegular", "\uf271", null, CreatePaymentScheme));
            }
            if (viewModel.Reservation.statuscode == 100000007 && viewModel.Reservation.bsd_quotationprinteddate != null && viewModel.Reservation.bsd_quotationsigneddate == null)
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem("Ký Bảng Tính Giá", "FontAwesomeRegular", "\uf274", null, CompletedQuotation));
            }
            if (viewModel.Reservation.bsd_reservationformstatus == 100000001 && viewModel.Reservation.bsd_reservationprinteddate != null && viewModel.Reservation.bsd_reservationuploadeddate == null && viewModel.Reservation.bsd_rfsigneddate == null)
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem("Xác Nhận Tải PĐC", "FontAwesomeRegular", "\uf15c", null, ConfirmReservation));
            }
            if (viewModel.Reservation.bsd_reservationformstatus == 100000001 && viewModel.Reservation.bsd_reservationprinteddate != null && viewModel.Reservation.bsd_reservationuploadeddate != null && viewModel.Reservation.bsd_rfsigneddate == null)
            {
                viewModel.ButtonCommandList.Add(new FloatButtonItem("Ký Phiếu Đặt Cọc", "FontAwesomeRegular", "\uf274", null, CompletedReservation));
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
                    ToastMessageHelper.ShortMessage("Đã xác nhận tải phiếu đặt cọc");
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Xác nhận tải phiếu đặt cọc thất bại. Vui lòng thử lại");
                }
            }
        }

        private void EditQuotes(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            ReservationForm reservation = new ReservationForm(new Guid("1AE3B00B-404F-4382-BB6F-56739D9BE0CF"));
            reservation.CheckReservation = async (isSuccess) => {
                if (isSuccess)
                {
                    await Navigation.PushAsync(reservation);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Không có thông tin bảng tính giá");
                }
            };
        }

        private async void CreatePaymentScheme(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (await viewModel.UpdatePaymentScheme())
            {
                NeedToRefresh = true;
                OnAppearing();
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage("Tạo lịch thanh toán thành công");
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage("Tạo lịch thanh toán thất bại. Vui lòng thử lại");
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
                    ToastMessageHelper.ShortMessage("Phiếu đặt cọc đã được ký");
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Ký phiếu đặt cọc thất bại. Vui lòng thử lại");
                }
            }
        }

        private async void CompletedQuotation(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.Reservation.quoteid != Guid.Empty)
            {
                viewModel.Reservation.statecode = 0;
                viewModel.Reservation.statuscode = 100000000;
                viewModel.Reservation.bsd_quotationsigneddate = DateTime.Now;
                if (await viewModel.UpdateQuotes(viewModel.UpdateQuotation))
                {
                    NeedToRefresh = true;
                    OnAppearing();
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Bảng tính giá đã được ký");
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Ký bảng tính giá thất bại. Vui lòng thử lại");
                }
            }
        }

        private async void CancelQuotes(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            string options = await DisplayActionSheet("Hủy Bảng Tính Giá", "Không", "Có", "Xác nhận hủy bảng tính giá");
            if (options == "Có")
            {
                viewModel.Reservation.statecode = 3;
                viewModel.Reservation.statuscode = 6;
                if (await viewModel.UpdateQuotes(viewModel.UpdateQuote))
                {
                    NeedToRefresh = true;
                    OnAppearing();
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Đã hủy bảng tính giá");
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Hủy bảng tính giá thất bại. Vui lòng thử lại");
                }
            }
            else if (options == "Không")
            {
                LoadingHelper.Hide();
            }
        }

        private RadBorder SetUpItemBorder(string content)
        {
            RadBorder rd = new RadBorder();
            rd.Padding = 5;
            rd.BorderColor = Color.FromHex("f1f1f1");
            rd.BorderThickness = 1;
            rd.CornerRadius = 5;
            Label lb = new Label();
            lb.Text = content;
            lb.FontSize = 15;
            lb.TextColor = Color.FromHex("1399D5");
            lb.VerticalOptions = LayoutOptions.Center;
            lb.HorizontalOptions = LayoutOptions.Center;
            lb.FontAttributes = FontAttributes.Bold;
            rd.Content = lb;
            return rd;
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
            return grid;
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
                        ToastMessageHelper.ShortMessage("Không tìm thấy thông tin dự án");
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
                        ToastMessageHelper.ShortMessage("Không tìm thấy thông tin đại lý/sàn giao dịch");
                    }
                };
            }
        }
    }
}