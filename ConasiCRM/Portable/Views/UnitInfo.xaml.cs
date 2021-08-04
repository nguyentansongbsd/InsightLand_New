using ConasiCRM.Portable.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnitInfo : ContentPage
    {
        public Action<bool> OnCompleted;
        private UnitInfoViewModel viewModel;

        public UnitInfo(Guid id)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new UnitInfoViewModel();
            viewModel.UnitId = id;
            Init();
        }
        public async void Init()
        {
            bool isSuccess = await viewModel.LoadUnit();
            
            if (isSuccess)
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

                btnGiuCho.IsVisible = viewModel.UnitInfo.bsd_vippriority ? false : true;
                
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
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

            LoadingHelper.Hide();
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