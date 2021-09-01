using ConasiCRM.Portable.Helper;
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
            lookupNetArea.PreOpenAsync = async () => {
                LoadingHelper.Show();
                viewModel.NetAreas = NetAreaDirectSaleData.NetAreaData();
                LoadingHelper.Hide();
            };
            lookupPrice.PreOpenAsync = async () => {
                LoadingHelper.Show();
                viewModel.Prices = PriceDirectSaleData.PriceData();
                LoadingHelper.Hide();
            };
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

        private void SearchClicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            if (viewModel.Project == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn Dự án");
                LoadingHelper.Hide();
            }
            else
            {
                string phasesLanchId = string.Empty;
                if (viewModel.PhasesLaunch != null)
                {
                    phasesLanchId = viewModel.PhasesLaunch.Val;
                }
                string directions = (viewModel.SelectedDirections != null && viewModel.SelectedDirections.Count != 0) ? string.Join(",", viewModel.SelectedDirections) : null;
                string unitStatus = (viewModel.SelectedUnitStatus != null && viewModel.SelectedUnitStatus.Count != 0) ? string.Join(",", viewModel.SelectedUnitStatus) : null;

                DirectSaleSearchModel filter = new DirectSaleSearchModel(viewModel.Project.bsd_projectid, phasesLanchId, viewModel.IsEvent,viewModel.UnitCode, directions, unitStatus,viewModel.NetArea?.Val,viewModel.Price?.Id);

                DirectSaleDetail directSaleDetail = new DirectSaleDetail(filter);
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

        //private int CompareInt(string a, string b)
        //{
        //    if (a != string.Empty && b != string.Empty)
        //    {
        //        if (Int32.TryParse(a, out int value1) && Int32.TryParse(b, out int value2))
        //        {
        //            if (value1 > value2)
        //                return 1;
        //            if (value2 == value1)
        //                return 0;
        //            if (value1 < value2)
        //                return -1;
        //        }
        //        if (!Int32.TryParse(a, out int i) || !Int32.TryParse(b, out int j))
        //        {
        //            if (!Int32.TryParse(a, out int c))
        //                return -1;
        //            if (!Int32.TryParse(b, out int d))
        //                return 1;
        //            return 0;
        //        }
        //    }
        //    return 0;
        //}

        //private void MinPrice_Unfocused(object sender, FocusEventArgs e)
        //{
        //    if (e.IsFocused == false)
        //    {
        //        if (CompareInt(viewModel.minPrice.ToString(), viewModel.maxPrice.ToString()) == 1)
        //        {
        //            ToastMessageHelper.ShortMessage("Giá trị không hợp lệ. Vui lòng thử lại!");
        //        }
        //    }
        //}

        //private void MaxPrice_Unfocused(object sender, FocusEventArgs e)
        //{
        //    if (e.IsFocused == false)
        //    {
        //        if (CompareInt(viewModel.maxPrice.ToString(), viewModel.minPrice.ToString()) == -1)
        //        {
        //            ToastMessageHelper.ShortMessage("Giá trị không hợp lệ. Vui lòng thử lại!");
        //        }
        //    }
        //}

        //private void MinNetArea_Unfocused(object sender, FocusEventArgs e)
        //{
        //    if (e.IsFocused == false)
        //    {
        //        if (CompareInt(viewModel.minNetArea.ToString(), viewModel.maxNetArea.ToString()) == 1)
        //        {
        //            ToastMessageHelper.ShortMessage("Giá trị không hợp lệ. Vui lòng thử lại!");
        //        }
        //    }
        //}

        //private void MaxNetArea_Unfocused(object sender, FocusEventArgs e)
        //{
        //    if (e.IsFocused == false)
        //    {
        //        if (CompareInt(viewModel.maxNetArea.ToString(), viewModel.minNetArea.ToString()) == -1)
        //        {
        //            ToastMessageHelper.ShortMessage("Giá trị không hợp lệ. Vui lòng thử lại!");
        //        }
        //    }
        //}
    }
}