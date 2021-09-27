using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public BangTinhGiaDetailPage(Guid id)
        {
            InitializeComponent();
            ReservationId = id;
            BindingContext = viewModel = new BangTinhGiaDetailPageViewModel();
            LoadingHelper.Show();
            Tab_Tapped(1);
            Init();
        }
        public async void Init()
        {
            await LoadDataChinhSach(ReservationId);
            await viewModel.LoadCoOwners(ReservationId);
            if (viewModel.Reservation.quoteid != Guid.Empty)
                OnCompleted?.Invoke(true);
            else
                OnCompleted?.Invoke(false);
            LoadingHelper.Hide();
        }

        //tab chinh sach

        private async Task LoadDataChinhSach(Guid id)
        {
            if(id != Guid.Empty)
            {
                LoadingHelper.Show();
                await viewModel.LoadReservation(id);
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

        private async void ShowMoreNguoiDongSoHuu_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageNguoiDongSoHuu++;
            await viewModel.LoadCoOwners(ReservationId);
            LoadingHelper.Hide();
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
    }
}