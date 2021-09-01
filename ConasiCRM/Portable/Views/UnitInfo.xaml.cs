using ConasiCRM.Portable.Helper;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using ConasiCRM.Portable.Helpers;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnitInfo : ContentPage
    {
        public Action<bool> OnCompleted;
        public static bool? NeedToRefreshQueue = null;
        private UnitInfoViewModel viewModel;

        public UnitInfo(Guid id)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new UnitInfoViewModel();
            NeedToRefreshQueue = false;
            viewModel.UnitId = id;
            Init();
        }
        public async void Init()
        {
            await Task.WhenAll(
                viewModel.LoadUnit(),
                viewModel.CheckShowBtnBangTinhGia()
                );
            
            if (viewModel.UnitInfo != null)
            {
                VisualStateManager.GoToState(radborderThongTin, "Active");
                VisualStateManager.GoToState(radborderGiaoDich, "InActive");
                VisualStateManager.GoToState(lblThongTin, "Active");
                VisualStateManager.GoToState(lblGiaoDich, "InActive");

                viewModel.StatusCode = StatusCodeUnit.GetStatusCodeById(viewModel.UnitInfo.statuscode.ToString());
                if (!string.IsNullOrWhiteSpace(viewModel.UnitInfo.bsd_direction))
                {
                    viewModel.Direction = DirectionData.GetDiretionById(viewModel.UnitInfo.bsd_direction);
                }

                if (!string.IsNullOrWhiteSpace(viewModel.UnitInfo.bsd_view))
                {
                    viewModel.View = ViewData.GetViewById(viewModel.UnitInfo.bsd_view);
                }

                if (viewModel.UnitInfo.statuscode == 1 || viewModel.UnitInfo.statuscode == 100000000 || viewModel.UnitInfo.statuscode == 100000004)
                {
                    btnGiuCho.IsVisible = viewModel.UnitInfo.bsd_vippriority ? false : true;
                    if (viewModel.UnitInfo.statuscode != 1 && viewModel.IsShowBtnBangTinhGia == true)
                    {
                        viewModel.IsShowBtnBangTinhGia = true;
                    }
                    else
                    {
                        viewModel.IsShowBtnBangTinhGia = false;
                    }
                }
                else
                {
                    btnGiuCho.IsVisible = false;
                    viewModel.IsShowBtnBangTinhGia = false;
                }
                SetButton();

                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if(NeedToRefreshQueue == true)
            {
                LoadingHelper.Show();
                viewModel.PageDanhSachDatCho = 1;
                viewModel.list_danhsachdatcho.Clear();
                await viewModel.LoadQueuesForContactForm();
                NeedToRefreshQueue = false;
                LoadingHelper.Hide();
            }    
        }

        public void SetButton()
        {
            if (btnGiuCho.IsVisible ==false && viewModel.IsShowBtnBangTinhGia ==false)
            {
                gridButton.IsVisible = false;
            }
            else if (btnGiuCho.IsVisible == true && viewModel.IsShowBtnBangTinhGia == true)
            {
                gridButton.IsVisible = true;
                Grid.SetColumn(btnGiuCho, 0);
                Grid.SetColumn(btnBangTinhGia, 1);
            }
            else if (btnGiuCho.IsVisible == true && viewModel.IsShowBtnBangTinhGia == false)
            {
                gridButton.IsVisible = true;
                Grid.SetColumn(btnGiuCho, 0);
                Grid.SetColumnSpan(btnGiuCho, 2);
                Grid.SetColumn(btnBangTinhGia, 0);
            }
            else if (btnGiuCho.IsVisible == false && viewModel.IsShowBtnBangTinhGia == true)
            {
                gridButton.IsVisible = true;
                Grid.SetColumn(btnGiuCho, 0);
                Grid.SetColumn(btnBangTinhGia, 0);
                Grid.SetColumnSpan(btnBangTinhGia, 2);
            }
        }

        private void ThongTin_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radborderThongTin, "Active");
            VisualStateManager.GoToState(radborderGiaoDich, "InActive");
            VisualStateManager.GoToState(lblThongTin, "Active");
            VisualStateManager.GoToState(lblGiaoDich, "InActive");
            stackThongTinCanHo.IsVisible = true;
            stackGiaoDich.IsVisible = false;
        }

        private async void GiaoDich_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            VisualStateManager.GoToState(radborderThongTin, "InActive");
            VisualStateManager.GoToState(radborderGiaoDich, "Active");
            VisualStateManager.GoToState(lblThongTin, "InActive");
            VisualStateManager.GoToState(lblGiaoDich, "Active");
            stackThongTinCanHo.IsVisible = false;
            stackGiaoDich.IsVisible = true;

            if (viewModel.IsLoaded == false)
            {
                await Task.WhenAll(
                    viewModel.LoadQueuesForContactForm(),
                    viewModel.LoadReservationForContactForm(),
                    viewModel.LoadOptoinEntryForContactForm()
                );
            }
            LoadingHelper.Hide();
        }

        private async void ShowMoreDanhSachDatCho_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDanhSachDatCho++;
            await viewModel.LoadQueuesForContactForm();
            LoadingHelper.Hide();
        }

        private async void ShowMoreDanhSachDatCoc_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDanhSachDatCoc++;
            await viewModel.LoadReservationForContactForm();
            LoadingHelper.Hide();
        }

        private async void ShowMoreDanhSachHopDong_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDanhSachHopDong++;
            await viewModel.LoadOptoinEntryForContactForm();
            LoadingHelper.Hide();
        }

        private void GiuCho_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            QueueForm queue = new QueueForm(viewModel.UnitId, true);
            queue.OnCompleted = async (IsSuccess) => {
                if (IsSuccess)
                {
                    await Navigation.PushAsync(queue);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Không tìm thấy sản phẩm");
                }
            };
            LoadingHelper.Hide();
        }

        private void BangTinhGia_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            ToastMessageHelper.ShortMessage("chua co page");
            LoadingHelper.Hide();
        }

        private void GiuChoItem_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var itemId = (Guid)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            QueuesDetialPage queuesDetialPage = new QueuesDetialPage(itemId);
            queuesDetialPage.OnCompleted = async (IsSuccess) => {
                if (IsSuccess)
                {
                    await Navigation.PushAsync(queuesDetialPage);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Không tìm thấy thông tin");
                }
            };
        }

        //private void Button_Clicked(object sender, EventArgs e)
        //{
        //    Navigation.PushAsync(new UnitImageGallery("Units", Id.ToString(), viewModel.UnitInfo.name, "Hình Ảnh Căn Hộ"));
        //}

        //private void Button_Clicked_Video(object sender, EventArgs e)
        //{
        //    Navigation.PushAsync(new UnitVideoGallery("Units",Id.ToString(),viewModel.UnitInfo.name,"Phim Căn Hộ"));
        //}
    }
}