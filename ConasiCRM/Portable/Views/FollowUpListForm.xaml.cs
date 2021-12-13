﻿using ConasiCRM.Portable.Helper;
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
    public partial class FollowUpListForm : ContentPage
    {
        public FollowUpListFormViewModel viewModel;
        public Action<bool> OnCompleted;
        public FollowUpListForm(Guid fulid)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new FollowUpListFormViewModel();
            this.Title = "Cập Nhật Thông Tin";
            Init(fulid);
        }

        public async void Init(Guid fulid)
        {
            await viewModel.LoadFUL(fulid);
            SetPreOpen();

            if (viewModel.FULDetail != null)
            {
                if (!string.IsNullOrWhiteSpace(viewModel.FULDetail.bsd_type.ToString()))
                    viewModel.Type = FollowUpType.GetFollowUpTypeById(viewModel.FULDetail.bsd_type.ToString());

                if (!string.IsNullOrWhiteSpace(viewModel.FULDetail.bsd_terminationtype.ToString()) && viewModel.FULDetail.bsd_terminationtype != 0)
                    viewModel.TypeTerminateletter = FollowUpTerminationType.GetFollowUpTerminationTypeById(viewModel.FULDetail.bsd_terminationtype.ToString());

                if (!string.IsNullOrWhiteSpace(viewModel.FULDetail.bsd_group.ToString()) && viewModel.FULDetail.bsd_group != 0)
                    viewModel.Group = FollowUpGroup.GetFollowUpGroupById(viewModel.FULDetail.bsd_group.ToString());

                if (!string.IsNullOrWhiteSpace(viewModel.FULDetail.bsd_takeoutmoney.ToString()) && viewModel.FULDetail.bsd_takeoutmoney != 0)
                    viewModel.TakeOutMoney = FollowUpListTakeOutMoney.GetFollowUpListTakeOutMoneyById(viewModel.FULDetail.bsd_takeoutmoney.ToString());
            }

            if (viewModel.FULDetail != null && viewModel.FULDetail.bsd_followuplistid != Guid.Empty)
                OnCompleted?.Invoke(true);
            else
                OnCompleted?.Invoke(false);
        }

        public void SetPreOpen()
        {
            Lookup_Type.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.ListType = FollowUpType.FollowUpTypeData();
                LoadingHelper.Hide();
            };
            Lookup_TypeTerminateletter.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.ListTypeTerminateletter = FollowUpTerminationType.FollowUpTerminationTypeData();
                LoadingHelper.Hide();
            };
            Lookup_Group.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.ListGroup = FollowUpGroup.FollowUpGroupData();
                LoadingHelper.Hide();
            };
            Lookup_TakeOut.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.ListTakeOutMoney = FollowUpListTakeOutMoney.FollowUpListTakeOutMoneyData();
                LoadingHelper.Hide();
            };
            Lookup_PhaseLaunch.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                if(viewModel.FULDetail != null && viewModel.FULDetail.project_id != Guid.Empty)
                {
                   await viewModel.LoadPhaseLaunch(viewModel.FULDetail.project_id);
                }    
                LoadingHelper.Hide();
            };
            Lookup_Meeting.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                if (viewModel.FULDetail != null && viewModel.FULDetail.bsd_followuplistid != Guid.Empty)
                {
                    await viewModel.LoadMeeting(viewModel.FULDetail.bsd_followuplistid);
                }
                LoadingHelper.Hide();
            };
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            if(viewModel.Type == null || string.IsNullOrWhiteSpace(viewModel.Type.Id))
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn loại");
                return;
            }
            if (viewModel.TypeTerminateletter == null || string.IsNullOrWhiteSpace(viewModel.TypeTerminateletter.Id))
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn loại thanh lý");
                return;
            }
            if (viewModel.Group == null || string.IsNullOrWhiteSpace(viewModel.Group.Id))
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn nhóm");
                return;
            }
            if (viewModel.TakeOutMoney == null || string.IsNullOrWhiteSpace(viewModel.TakeOutMoney.Id))
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn phương thức phạt");
                return;
            }
            if (viewModel.Refund <=0 )
            {
                ToastMessageHelper.ShortMessage("Vui lòng nhập số tiền hoàn lại");
                return;
            }
            if (viewModel.TakeOutMoney.Id == "100000001")
            {
                ToastMessageHelper.ShortMessage("Vui lòng nhập giá trị từ 0 đến 100");
                return;
            }

            if(viewModel.FULDetail != null && viewModel.FULDetail.bsd_resell)
            {
                if(viewModel.PhaseLaunch == null || viewModel.PhaseLaunch.Id == Guid.Empty)
                {
                    ToastMessageHelper.ShortMessage("Vui lòng chọn đợt mở bán");
                    return;
                }    
            }

            LoadingHelper.Show();
            var updated = await viewModel.updateFUL();
            if (updated)
            {
                if (FollowDetailPage.NeedToRefresh.HasValue) FollowDetailPage.NeedToRefresh = true;
                await Navigation.PopAsync();
                ToastMessageHelper.ShortMessage("Cập nhật thông tin thành công");
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage("Cập nhật thông tin thất bại");
            }
        }
    }
}