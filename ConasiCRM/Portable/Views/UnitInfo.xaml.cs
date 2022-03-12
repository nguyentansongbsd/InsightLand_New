using ConasiCRM.Portable.Helper;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using ConasiCRM.Portable.Helpers;
using FFImageLoading.Forms;
using System.Collections.Generic;
using Stormlion.PhotoBrowser;
using System.Linq;
using MediaManager;
using MediaManager.Forms;
using System.Collections.ObjectModel;
using ConasiCRM.Portable.Resources;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnitInfo : ContentPage
    {
        public Action<bool> OnCompleted;
        public static bool? NeedToRefreshQueue = null;
        public static bool? NeedToRefreshQuotation = null;
        private UnitInfoViewModel viewModel;

        public UnitInfo(Guid id)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new UnitInfoViewModel();
            NeedToRefreshQueue = false;
            NeedToRefreshQuotation = false;
            viewModel.UnitId = id;
            Init();
        }
        public async void Init()
        {
            await Task.WhenAll(
                viewModel.LoadUnit(),
                viewModel.LoadAllCollection(),
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
                await viewModel.LoadQueues();
                NeedToRefreshQueue = false;
                LoadingHelper.Hide();
            }
            if (NeedToRefreshQuotation == true)
            {
                LoadingHelper.Show();
                viewModel.PageBangTinhGia = 1;
                viewModel.BangTinhGiaList.Clear();
                await viewModel.LoadDanhSachBangTinhGia();
                NeedToRefreshQuotation = false;
                LoadingHelper.Hide();
            }
            await CrossMediaManager.Current.Stop();
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
                viewModel.BangTinhGiaList = new ObservableCollection<ReservationListModel>();
                await Task.WhenAll(
                    viewModel.LoadQueues(),
                    viewModel.LoadDanhSachDatCoc(),
                    viewModel.LoadDanhSachBangTinhGia(),
                    viewModel.LoadOptoinEntry()
                );
            }
            LoadingHelper.Hide();
        }

        private async void ShowMoreDanhSachDatCho_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDanhSachDatCho++;
            await viewModel.LoadQueues();
            LoadingHelper.Hide();
        }

        private async void ShowMoreDanhSachDatCoc_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDanhSachDatCoc++;
            await viewModel.LoadDanhSachDatCoc();
            LoadingHelper.Hide();
        }

        private async void ShowMoreDanhSachHopDong_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDanhSachHopDong++;
            await viewModel.LoadOptoinEntry();
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
                  //  ToastMessageHelper.ShortMessage(Language.khong_tim_thay_san_pham);
                }
            };
        }

        private void BangTinhGia_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            ReservationForm reservationForm = new ReservationForm(Guid.Parse(viewModel.UnitInfo.productid), null, null, null,null);
            reservationForm.CheckReservation = async (isSuccess) => {
                if (isSuccess == 0)
                {
                    await Navigation.PushAsync(reservationForm);
                    LoadingHelper.Hide();
                }
                else if (isSuccess == 1)
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.san_pham_khong_the_tao_bang_tinh_gia);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_san_pham);
                }
            };
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
                   // ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }

        private void ChiTietDatCoc_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var itemId = (Guid)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            BangTinhGiaDetailPage bangTinhGiaDetail = new BangTinhGiaDetailPage(itemId);
            bangTinhGiaDetail.OnCompleted = async (isSuccess) =>
            {
                if (isSuccess)
                {
                    await Navigation.PushAsync(bangTinhGiaDetail);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }

        private async void ShowMoreBangTinhGia_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageBangTinhGia++;
            await viewModel.LoadDanhSachBangTinhGia();
            LoadingHelper.Hide();
        }

        private void ItemHopDong_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var itemId = (Guid)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            ContractDetailPage contractDetail = new ContractDetailPage(itemId);
            contractDetail.OnCompleted = async (isSuccess) =>
            {
                if (isSuccess)
                {
                    await Navigation.PushAsync(contractDetail);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
        }

        private void ItemSlider_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = (CollectionData)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item.SharePointType == SharePointType.Image)
            {
                var img = viewModel.Photos.SingleOrDefault(x => x.URL == item.ImageSource);
                var index = viewModel.Photos.IndexOf(img);

                new PhotoBrowser()
                {
                    Photos = viewModel.Photos,
                    StartIndex = index,
                    EnableGrid = true
                }.Show();
            }
            else if (item.SharePointType == SharePointType.Video)
            {
                ShowMedia showMedia = new ShowMedia(Config.OrgConfig.SharePointProjectId, item.MediaSourceId);
                showMedia.OnCompleted = async (isSuccess) => {
                    if (isSuccess)
                    {
                        await Navigation.PushAsync(showMedia);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.khong_lay_duoc_video);
                    }
                };
            }
            LoadingHelper.Hide();
        }

        private void ScollTo_Video_Tapped(object sender, EventArgs e)
        {
            var index = viewModel.Collections.IndexOf(viewModel.Collections.FirstOrDefault(x => x.SharePointType == SharePointType.Video));
            carouseView.ScrollTo(index, position: ScrollToPosition.End);
        }

        private void ScollTo_Image_Tapped(object sender, EventArgs e)
        {
            var index = viewModel.Collections.IndexOf(viewModel.Collections.FirstOrDefault(x => x.SharePointType == SharePointType.Image));
            carouseView.ScrollTo(index, position: ScrollToPosition.End);
        }
        private void CloseContentEvent_Tapped(object sender, EventArgs e)
        {
            ContentEvent.IsVisible = false;
        }

        private async void OpenEvent_Tapped(object sender, EventArgs e)
        {
            if (viewModel.Event == null)
                await viewModel.LoadDataEvent();
            ContentEvent.IsVisible = true;
        }
    }
}