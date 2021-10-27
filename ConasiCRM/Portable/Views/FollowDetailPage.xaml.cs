﻿using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
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
    public partial class FollowDetailPage : ContentPage
    {
        public FollowDetailPageViewModel viewModel;
        public Action<bool> OnLoaded;

        public FollowDetailPage(Guid id)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new FollowDetailPageViewModel();
            Init(id);            
        }

        private async void Init(Guid id)
        {
            await viewModel.Load(id);

            //if (viewModel.FollowDetail.name_quote != null)
            //{
            //    //nameWork.Text = "Phiếu đặt cọc: ";
            //}
            //else if (viewModel.FollowDetail.name_salesorder != null)
            //{
            //   // nameWork.Text = "Hợp đồng: ";
            //}

            if (viewModel.FollowDetail != null && viewModel.FollowDetail.bsd_followuplistid != Guid.Empty)
            {
                OnLoaded?.Invoke(true);
            }
            else
            {
                OnLoaded?.Invoke(false);
            }
        }

        private void Project_Tapped(object sender, EventArgs e)
        {
            if (viewModel.FollowDetail.project_id != Guid.Empty)
            {
                LoadingHelper.Show();
                ProjectInfo projectInfo = new ProjectInfo(viewModel.FollowDetail.project_id);
                projectInfo.OnCompleted = async (IsSuccess) =>
                {
                    if (IsSuccess == true)
                    {
                        await Navigation.PushAsync(projectInfo);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        ToastMessageHelper.ShortMessage("Không tìm thấy thông tin");
                        LoadingHelper.Hide();
                    }
                };
            }    
        }

        private void Unit_Tapped(object sender, EventArgs e)
        {
            if (viewModel.FollowDetail.product_id != Guid.Empty)
            {
                LoadingHelper.Show();
                UnitInfo unitInfo = new UnitInfo(viewModel.FollowDetail.product_id);
                unitInfo.OnCompleted = async (IsSuccess) => {
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
        }

        private void Reservation_Tapped(object sender, EventArgs e)
        {
            if(viewModel.FollowDetail.bsd_reservation_id != Guid.Empty)
            {
                LoadingHelper.Show();
                BangTinhGiaDetailPage bangTinhGiaDetail = new BangTinhGiaDetailPage(viewModel.FollowDetail.bsd_reservation_id);
                bangTinhGiaDetail.OnCompleted = async (OnCompleted) =>
                {
                    if (OnCompleted == true)
                    {
                        await Navigation.PushAsync(bangTinhGiaDetail);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage("Không tìm thấy thông tin bảng tính giá");
                    }

                };
            }    
        }

        private void CollectionMeeting_Tapped(object sender, EventArgs e)
        {
            if (viewModel.FollowDetail.bsd_collectionmeeting_id != Guid.Empty)
            {
                LoadingHelper.Show();
                MeetingForm newPage = new MeetingForm(viewModel.FollowDetail.bsd_collectionmeeting_id);
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
                        ToastMessageHelper.ShortMessage("Không tìm thấy thông tin. Vui lòng thử lại");
                    }
                };
            }
        }
    }
}