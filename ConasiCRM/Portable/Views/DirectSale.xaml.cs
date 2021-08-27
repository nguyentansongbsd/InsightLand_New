﻿using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DirectSale : ContentPage
    {
        public DirectSaleViewModel viewModel;
        public DirectSale()
        {
            LoadingHelper.Show();
            InitializeComponent();
            this.BindingContext = viewModel = new DirectSaleViewModel();
            Init();
            LoadingHelper.Hide();
        }

        public async void Init()
        {
            //lookupMultipleView.PreShow = async () =>
            //{
            //    LoadingHelper.Show();
            //    viewModel.ViewOptions = ViewData.Views();
            //    LoadingHelper.Hide();
            //};
            lookupMultipleDirection.PreShow = async () => {
                LoadingHelper.Show();
                viewModel.DirectionOptions = DirectionData.Directions();
                LoadingHelper.Hide();
            };
            lookupMultipleUnitStatus.PreShow= async () => {
                LoadingHelper.Show();
                var unitStatus = StatusCodeUnit.StatusCodes();
                viewModel.UnitStatusOptions = new List<OptionSet>();
                foreach (var item in unitStatus)
                {
                    viewModel.UnitStatusOptions.Add(new OptionSet(item.Id, item.Name));
                }
                LoadingHelper.Hide();
            };
            
        }

        private async void LoadProject_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.Projects.Count == 0)
            {
                await viewModel.LoadProject();
                listviewProject.ItemsSource = viewModel.Projects;
            }
            
            await bottomModalProject.Show();
            LoadingHelper.Hide();
        }

        private void SearchBar_SearchButtonPressed(object sender,EventArgs e)
        {
            LoadingHelper.Show();
            listviewProject.ItemsSource = viewModel.Projects.Where(x=>x.bsd_name.ToLower().Contains(searchProject.Text.Trim().ToLower()) || x.bsd_projectcode.ToLower().Contains(searchProject.Text.Trim().ToLower()));
            LoadingHelper.Hide();
        }

        private async void SearchBar_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchProject.Text))
            {
                LoadingHelper.Show();
                viewModel.Projects.Clear();
                await viewModel.LoadProject();
                listviewProject.ItemsSource = viewModel.Projects;
                LoadingHelper.Hide();
            }
        }

        private async void ProjectItem_Tapped(object sender, ItemTappedEventArgs e)
        {
            LoadingHelper.Show();
            var item = e.Item as ProjectList;
            viewModel.Project = item;
            await Task.WhenAll(
                //viewModel.LoadBlocks(),
                viewModel.LoadPhasesLanch()
                );
            await bottomModalProject.Hide();
            LoadingHelper.Hide();
        }

        private async void SearchClicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.Project == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn Dự án");
                LoadingHelper.Hide();
            }
            //else if(viewModel.Blocks == null)
            //{
            //    ToastMessageHelper.ShortMessage("Dự án không có blocks");
            //    LoadingHelper.Hide();
            //}
            else
            {
                DirectSaleSearchModel model = new DirectSaleSearchModel();

                if (viewModel.PhasesLaunch != null)
                {
                    model.PhasesLanchId = viewModel.PhasesLaunch.Val;
                }
                model.ProjectId = viewModel.Project.bsd_projectid;
                model.IsEvent = viewModel.IsEvent;
                model.UnitCode = viewModel.UnitCode;
                model.Directions = viewModel.SelectedDirections;
                //model.Views = viewModel.SelectedViews;
                model.UnitStatuses = viewModel.SelectedUnitStatus;
                model.minNetArea = viewModel.minNetArea;
                model.maxNetArea = viewModel.maxNetArea;
                model.minPrice = viewModel.minPrice;
                model.maxPrice = viewModel.maxPrice;
                //model.Blocks = viewModel.Blocks;
                
                DirectSaleDetail directSaleDetail = new DirectSaleDetail(model);
                directSaleDetail.OnComplete = async (Success) =>
                {
                    if (Success == 0)
                    {
                        await Navigation.PushAsync(directSaleDetail);
                        LoadingHelper.Hide();
                    }
                    else if (Success == 1)
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.LongMessage("Không có sản phẩm");
                    }
                };
            }
        }

        private void ShowInfo(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.Project == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn Dự án");
                LoadingHelper.Hide();
                return;
            }

            ProjectInfo projectInfo = new ProjectInfo(Guid.Parse(viewModel.Project.bsd_projectid));
            projectInfo.OnCompleted = async (IsSuccess) =>
            {
                if (IsSuccess == true)
                {
                    await Navigation.PushAsync(projectInfo);
                    LoadingHelper.Hide();
                }
                else
                {
                    await DisplayAlert("", "Không tìm thấy thông tin.", "Đóng");
                    LoadingHelper.Hide();
                }
            };
        }

        private int CompareInt(string a, string b)
        {
            if (a != string.Empty && b != string.Empty)
            {
                if (Int32.TryParse(a, out int value1) && Int32.TryParse(b, out int value2))
                {
                    if (value1 > value2)
                        return 1;
                    if (value2 == value1)
                        return 0;
                    if (value1 < value2)
                        return -1;
                }
                if (!Int32.TryParse(a, out int i) || !Int32.TryParse(b, out int j))
                {
                    if (!Int32.TryParse(a, out int c))
                        return -1;
                    if (!Int32.TryParse(b, out int d))
                        return 1;
                    return 0;
                }
            }
            return 0;
        }

        private void MinPrice_Unfocused(object sender, FocusEventArgs e)
        {
            if (e.IsFocused == false)
            {
                if (CompareInt(viewModel.minPrice.ToString(), viewModel.maxPrice.ToString()) == 1)
                {
                    ToastMessageHelper.ShortMessage("Giá trị không hợp lệ. Vui lòng thử lại!");
                }
            }
        }

        private void MaxPrice_Unfocused(object sender, FocusEventArgs e)
        {
            if (e.IsFocused == false)
            {
                if (CompareInt(viewModel.maxPrice.ToString(), viewModel.minPrice.ToString()) == -1)
                {
                    ToastMessageHelper.ShortMessage("Giá trị không hợp lệ. Vui lòng thử lại!");
                }
            }
        }

        private void MinNetArea_Unfocused(object sender, FocusEventArgs e)
        {
            if (e.IsFocused == false)
            {
                if (CompareInt(viewModel.minNetArea.ToString(), viewModel.maxNetArea.ToString()) == 1)
                {
                    ToastMessageHelper.ShortMessage("Giá trị không hợp lệ. Vui lòng thử lại!");
                }
            }
        }

        private void MaxNetArea_Unfocused(object sender, FocusEventArgs e)
        {
            if (e.IsFocused == false)
            {
                if (CompareInt(viewModel.maxNetArea.ToString(), viewModel.minNetArea.ToString()) == -1)
                {
                    ToastMessageHelper.ShortMessage("Giá trị không hợp lệ. Vui lòng thử lại!");
                }
            }
        }
    }
}