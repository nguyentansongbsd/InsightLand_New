using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
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
    public partial class ProjectInfo : ContentPage
    {
        public Action<bool> OnCompleted;
        public ProjectInfoViewModel viewModel;
        
        public ProjectInfo(Guid Id)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ProjectInfoViewModel();
            viewModel.ProjectId = Id;
            Init();
        }

        public async void Init()
        {
            VisualStateManager.GoToState(radborderThongKe, "Active");
            VisualStateManager.GoToState(radborderThongTin, "InActive");
            VisualStateManager.GoToState(radborderGiuCho, "InActive");
            VisualStateManager.GoToState(lblThongKe, "Active");
            VisualStateManager.GoToState(lblThongTin, "InActive");
            VisualStateManager.GoToState(lblGiuCho, "InActive");

            await Task.WhenAll(
                viewModel.LoadData(),
                viewModel.CheckEvent(),
                viewModel.LoadThongKe(),
                viewModel.LoadThongKeGiuCho(),
                viewModel.LoadThongKeHopDong(),
                viewModel.LoadThongKeBangTinhGia()
            );

            if (viewModel.Project != null)
            {
                viewModel.ProjectType = ProjectTypeData.GetProjectType(viewModel.Project.bsd_projecttype);
                viewModel.PropertyUsageType = PropertyUsageTypeData.GetPropertyUsageTypeById(viewModel.Project.bsd_propertyusagetype.ToString());
                if (viewModel.Project.bsd_handoverconditionminimum.HasValue)
                {
                    viewModel.HandoverCoditionMinimum = HandoverCoditionMinimumData.GetHandoverCoditionMinimum(viewModel.Project.bsd_handoverconditionminimum.Value.ToString());
                }
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
        }
        
        private async void ThongKe_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radborderThongKe, "Active");
            VisualStateManager.GoToState(radborderThongTin, "InActive");
            VisualStateManager.GoToState(radborderGiuCho, "InActive");
            VisualStateManager.GoToState(lblThongKe, "Active");
            VisualStateManager.GoToState(lblThongTin, "InActive");
            VisualStateManager.GoToState(lblGiuCho, "InActive");
            stackThongKe.IsVisible = true;
            stackThongTin.IsVisible = false;
            stackGiuCho.IsVisible = false;
        }

        private async void ThongTin_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radborderThongKe, "InActive");
            VisualStateManager.GoToState(radborderThongTin, "Active");
            VisualStateManager.GoToState(radborderGiuCho, "InActive");
            VisualStateManager.GoToState(lblThongKe, "InActive");
            VisualStateManager.GoToState(lblThongTin, "Active");
            VisualStateManager.GoToState(lblGiuCho, "InActive");
            stackThongKe.IsVisible = false;
            stackThongTin.IsVisible = true;
            stackGiuCho.IsVisible = false;
        }

        private async void GiuCho_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            VisualStateManager.GoToState(radborderThongKe, "InActive");
            VisualStateManager.GoToState(radborderThongTin, "InActive");
            VisualStateManager.GoToState(radborderGiuCho, "Active");
            VisualStateManager.GoToState(lblThongKe, "InActive");
            VisualStateManager.GoToState(lblThongTin, "InActive");
            VisualStateManager.GoToState(lblGiuCho, "Active");
            stackThongKe.IsVisible = false;
            stackThongTin.IsVisible = false;
            stackGiuCho.IsVisible = true;
            if (viewModel.IsLoadedGiuCho == false)
            {
                await viewModel.LoadGiuCho();
            }
            
            LoadingHelper.Hide();
        }

        private void GiuCho_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            QueueForm queue = new QueueForm(viewModel.ProjectId, false);
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

        private async void ShowMoreListDatCho_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageListGiuCho++;
            await viewModel.LoadGiuCho();
            LoadingHelper.Hide();
        }

        private void ChuDauTu_Tapped(System.Object sender, System.EventArgs e)
        {
            LoadingHelper.Show();
            var id = (Guid)((sender as Label).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            AccountDetailPage accountDetailPage = new AccountDetailPage(id);
            accountDetailPage.OnCompleted= async (IsSuccess) => {
                if (IsSuccess)
                {
                    await Navigation.PushAsync(accountDetailPage);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Không tìm thấy thông tin chủ đầu tư");
                }
            };
        }
    }
}