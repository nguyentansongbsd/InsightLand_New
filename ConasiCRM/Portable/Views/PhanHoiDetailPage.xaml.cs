using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
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
    public partial class PhanHoiDetailPage : ContentPage
    {
        public Action<bool> OnCompleted;
        private Guid CaseId;
        PhanHoiDetailPageViewModel viewModel;
        public PhanHoiDetailPage(Guid id)
        {
            InitializeComponent();
            LoadingHelper.Show();
            CaseId = id;
            BindingContext = viewModel = new PhanHoiDetailPageViewModel();
            Tab_Tapped(1);
            Init();
        }

        public async void Init()
        {
            await LoadDataThongTin(CaseId);

            viewModel.ButtonCommandList.Add(new FloatButtonItem("Hủy phản hồi", "FontAwesomeRegular", "\uf273", null, CancelCase));
            viewModel.ButtonCommandList.Add(new FloatButtonItem("Giải quyết phản hồi", "FontAwesomeRegular", "\uf274", null, CompletedCase));
            viewModel.ButtonCommandList.Add(new FloatButtonItem("Chỉnh sửa", "FontAwesomeRegular", "\uf044", null,Update));

            if (viewModel.Case.incidentid != Guid.Empty)
                OnCompleted?.Invoke(true);
            else
                OnCompleted?.Invoke(false);
            LoadingHelper.Hide();
        }

        // tab thong tin
        private async Task LoadDataThongTin(Guid Id)
        {
            if (Id != Guid.Empty && viewModel.Case.incidentid == Guid.Empty)
            {
                await viewModel.LoadCase(Id);
            }
        }

        // phan hoi lien quan
        private async Task LoadDataPhanHoiLienQuan(Guid Id)
        {
            if (viewModel.ListCase != null && viewModel.ListCase.Count <= 0)
            {
                await viewModel.LoadListCase(Id);
            }
        }

        private async void ShowMoreCase_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageCase++;
            await viewModel.LoadListCase(CaseId);
            LoadingHelper.Hide();
        }

        private void Update(object sender, EventArgs e)
        {
        }

        private async void CancelCase(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            string options = await DisplayActionSheet("Hủy phản hồi", "Không", "Có", "Xác nhận hủy phản hồi");
            if (options == "Có")
            {

            }
            else if (options == "Không")
            {
                LoadingHelper.Hide();
            }
        }

        private void CompletedCase(object sender, EventArgs e)
        {
        }

        private void BtnSave_Clicked(object sender, EventArgs e)
        {

        }

        private void ThongTin_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(1);
        }

        private async void PhanHoiLienQuan_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(2);
            await LoadDataPhanHoiLienQuan(CaseId);
        }

        private void Tab_Tapped(int tab)
        {
            if (tab == 1)
            {
                VisualStateManager.GoToState(radBorderThongTin, "Selected");
                VisualStateManager.GoToState(lbThongTin, "Selected");
                TabThongTin.IsVisible = true;
            }
            else
            {
                VisualStateManager.GoToState(radBorderThongTin, "Normal");
                VisualStateManager.GoToState(lbThongTin, "Normal");
                TabThongTin.IsVisible = false;
            }
            if (tab == 2)
            {
                VisualStateManager.GoToState(radBorderPhanHoiLienQuan, "Selected");
                VisualStateManager.GoToState(lbPhanHoiLienQuan, "Selected");
                TabPhanHoiLienQuan.IsVisible = true;
            }
            else
            {
                VisualStateManager.GoToState(radBorderPhanHoiLienQuan, "Normal");
                VisualStateManager.GoToState(lbPhanHoiLienQuan, "Normal");
                TabPhanHoiLienQuan.IsVisible = false;
            }
        }       
    }
}