using System;
using System.Collections.Generic;
using System.Linq;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Views
{
    public partial class QueuesDetialPage : ContentPage
    {
        public Action<bool> OnCompleted;
        public QueuesDetialPageViewModel viewModel;
        public QueuesDetialPage(Guid queueId)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new QueuesDetialPageViewModel();
            viewModel.QueueId = queueId;
            Init();
        }

        public async void Init()
        {
            await viewModel.LoadQueue();
            SetButtons();
            if (viewModel.Queue != null)
            {
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
        }

        private void SetButtons()
        {
            viewModel.ShowButtons = (viewModel.ShowBtnHuyGiuCho == false && viewModel.ShowBtnBangTinhGia == false) ? false : true;
            gridButtons.ColumnDefinitions.Clear();
            var btns = gridButtons.Children.Where(x => x.IsVisible == true).ToList();
            for (int i = 0; i < btns.Count(); i++)
            {
                gridButtons.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star),
                });
                Grid.SetColumn(btns[i], i);
            }
        }

        private void GoToProject_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            ProjectInfo projectInfo = new ProjectInfo(viewModel.Queue._bsd_project_value);
            projectInfo.OnCompleted = async (IsSuccess) =>
            {
                if (IsSuccess)
                {
                    await Navigation.PushAsync(projectInfo);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Không tìm thấy dự án");
                }
            };
        }

        private void GoToPhaseLaunch_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();

            LoadingHelper.Hide();
        }

        private void GoToUnit_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            UnitInfo unitInfo = new UnitInfo(viewModel.Queue._bsd_units_value);
            unitInfo.OnCompleted = async (IsSuccess) =>
            {
                if (IsSuccess)
                {
                    await Navigation.PushAsync(unitInfo);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Không tìm thấy sản phẩm");
                }
            };
        }

        private void GoToAcount_Tapped(System.Object sender, System.EventArgs e)
        {
            LoadingHelper.Show();
            AccountDetailPage accountDetail = new AccountDetailPage(viewModel.Queue._bsd_salesagentcompany_value);
            accountDetail.OnCompleted = async (IsSuccess) =>
            {
                if (IsSuccess)
                {
                    await Navigation.PushAsync(accountDetail);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Không tìm thấy đại lý bán hàng ");
                }
            };
        }

        private async void NhanTin_Tapped(System.Object sender, System.EventArgs e)
        {
            LoadingHelper.Show();
            try
            {
                if (!string.IsNullOrWhiteSpace(viewModel.NumPhone))
                {
                    LoadingHelper.Hide();
                    SmsMessage sms = new SmsMessage(null, viewModel.NumPhone);
                    await Sms.ComposeAsync(sms);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Không có số điện thoại");
                }
            }
            catch (Exception ex)
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(ex.Message);
            }
        }

        private async void GoiDien_Tapped(System.Object sender, System.EventArgs e)
        {
            LoadingHelper.Show();
            try
            {
                if (!string.IsNullOrWhiteSpace(viewModel.NumPhone))
                {
                    await Launcher.OpenAsync($"tel:{viewModel.NumPhone}");
                }
                else
                {
                    ToastMessageHelper.ShortMessage("Không có số điện thoại");
                }
                LoadingHelper.Hide();
            }
            catch (Exception ex)
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage(ex.Message);
            }
        }

        private async void HuyGiuCho_Clicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Xác nhận", "Bạn có muốn hủy giữ chỗ này không ?", "Đồng ý", "Hủy");
            if (confirm == false) return;

            LoadingHelper.Show();
            string url_action = $"/opportunities({this.viewModel.QueueId})/Microsoft.Dynamics.CRM.bsd_Action_Queue_CancelQueuing";
            CrmApiResponse res = await CrmHelper.PostData(url_action, null);
            if (res.IsSuccess)
            {
                await viewModel.LoadQueue();
                SetButtons();
                if (DirectSaleDetail.NeedToRefreshQueues.HasValue) DirectSaleDetail.NeedToRefreshQueues = true;
                if (ProjectInfo.NeedToRefreshQueue.HasValue) ProjectInfo.NeedToRefreshQueue = true;
                if (UnitInfo.NeedToRefreshQueue.HasValue) UnitInfo.NeedToRefreshQueue = true;
                if (AccountDetailPage.NeedToRefreshQueues.HasValue) AccountDetailPage.NeedToRefreshQueues = true;
                if (ContactDetailPage.NeedToRefreshQueues.HasValue) ContactDetailPage.NeedToRefreshQueues = true;
                ToastMessageHelper.ShortMessage("Huỷ giữ chổ thành công");
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage("Huỷ giữ chổ thất bại");
            }
        }

    }
}
