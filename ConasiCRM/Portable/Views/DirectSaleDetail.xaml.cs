using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.XamarinForms.Primitives;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DirectSaleDetail : ContentPage
    {
        public static bool? NeedToRefreshQueues = null;
        public Action<int> OnComplete;
        private DirectSaleDetailViewModel viewModel;
        public Unit CurrentUnit;
        private int currentBlock = 0;

        public DirectSaleDetail()
        {
            InitializeComponent();
        }

        public DirectSaleDetail(DirectSaleSearchModel model)
        {
            InitializeComponent();
            BindingContext = viewModel = new DirectSaleDetailViewModel(model);
            NeedToRefreshQueues = false;
            Init();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefreshQueues == true)
            {
                LoadingHelper.Show();
                viewModel.PageDanhSachDatCho = 1;
                viewModel.QueueList.Clear();
                await viewModel.LoadQueues();
                NeedToRefreshQueues = false;
                LoadingHelper.Hide();
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            //var a = (((stackFloors.Children as List<RadBorder>).Content as RadExpander).Content as FlexLayout).Children as RadBorder;





        }

        public async void Init()
        {
            await viewModel.LoadBlocks();
            if (viewModel.Blocks != null && viewModel.Blocks.Count != 0)
            {
                if (string.IsNullOrWhiteSpace(viewModel.PhasesLanchId) && string.IsNullOrWhiteSpace(viewModel.UnitCode))
                {
                    viewModel.blockId = viewModel.Blocks.FirstOrDefault().bsd_blockid;
                    SetActiveBlock();
                }
            }
            else
            {
                OnComplete?.Invoke(1);// loi khong co blocks
                return;
            }

            await viewModel.LoadUnit();
            //OnComplete?.Invoke(0);
            if (viewModel.Floors != null && viewModel.Floors.Count > 0)
            {
                ((stackFloors.Children[0] as RadBorder).Content as RadExpander).IsExpanded = true;
                if (!string.IsNullOrWhiteSpace(viewModel.PhasesLanchId) || !string.IsNullOrWhiteSpace(viewModel.UnitCode))
                {
                    for (int i = 0; i < viewModel.Blocks.Count; i++)
                    {
                        if (viewModel.Blocks[i].bsd_blockid == viewModel.blockId)
                        {
                            currentBlock = i;
                        }
                    }
                    SetActiveBlock();
                }
                OnComplete?.Invoke(0);
            }
            else
            {
                OnComplete?.Invoke(2); // loi khong co unit
            }
        }

        private void test_tapped(object sender, EventArgs e)
        {
            var b= stackFloors.TabIndex;
            var a = (stackFloors.Children[1] as RadBorder).Content as RadExpander;
            a.IsExpanded = true;
        }

        private void Question_CLicked(object sender,EventArgs e)
        {
            stackQuestion.IsVisible = !stackQuestion.IsVisible;
        }

        private void CloseQuestion_Tapped(object sender, EventArgs e)
        {
            stackQuestion.IsVisible = !stackQuestion.IsVisible;
        }

        public async void Block_Tapped(object sender,EventArgs e)
        {
            var blockChoosed = sender as RadBorder;
            if (stackBlocks.Children.IndexOf(blockChoosed) == currentBlock) return;
            SetInActiveBlock();
            this.currentBlock = stackBlocks.Children.IndexOf(blockChoosed);
            SetActiveBlock();

            LoadingHelper.Show();
            var block = (Block)(blockChoosed.GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            viewModel.blockId = block.bsd_blockid;
            viewModel.Floors.Clear();
            viewModel.NumChuanBiInBlock = 0;
            viewModel.NumSanSangInBlock = 0;
            viewModel.NumGiuChoInBlock = 0;
            viewModel.NumDatCocInBlock = 0;
            viewModel.NumDongYChuyenCoInBlock = 0;
            viewModel.NumDaDuTienCocInBlock = 0;
            viewModel.NumThanhToanDot1InBlock = 0;
            viewModel.NumDaBanInBlock = 0;
            await viewModel.LoadUnit();
            if (viewModel.Floors.Count != 0)
            {
                ((stackFloors.Children[0] as RadBorder).Content as RadExpander).IsExpanded = true;
            }
            LoadingHelper.Hide();
        }

        public void SetActiveBlock()
        {
            var radblock = (RadBorder)stackBlocks.Children[currentBlock];
            Label lblblock = (radblock.Content as Label);
            VisualStateManager.GoToState(radblock, "Active");
            VisualStateManager.GoToState(lblblock, "Active");
        }

        public void SetInActiveBlock()
        {
            var radblock = (RadBorder)stackBlocks.Children[currentBlock];
            Label lblblock = (radblock.Content as Label);
            VisualStateManager.GoToState(radblock, "InActive");
            VisualStateManager.GoToState(lblblock, "InActive");
        }

        private async void UnitItem_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = ((sender as RadBorder).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter as Unit;

            viewModel.UnitStatusCode = StatusCodeUnit.GetStatusCodeById(item.statuscode.ToString());          

            if (!string.IsNullOrWhiteSpace(item.bsd_direction))
            {
                viewModel.UnitDirection = DirectionData.GetDiretionById(item.bsd_direction);
            }
            if (!string.IsNullOrWhiteSpace(item.bsd_view))
            {
                viewModel.UnitView = ViewData.GetViewById(item.bsd_view);
            }
            viewModel.Unit = item;
            viewModel.PageDanhSachDatCho = 1;
            viewModel.QueueList.Clear();
            await viewModel.LoadQueues();
            await viewModel.CheckShowBtnBangTinhGia();

            if (viewModel.UnitStatusCode.Id == "1" || viewModel.UnitStatusCode.Id == "100000000" || viewModel.UnitStatusCode.Id == "100000004")
            {
                btnGiuCho.IsVisible = true;
            }
            else
            {
                btnGiuCho.IsVisible = false;
            }

            SetButton();
            
            contentUnitInfor.IsVisible = true;
            LoadingHelper.Hide();
        }

        private void UnitInfor_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            UnitInfo unitInfo = new UnitInfo(viewModel.Unit.productid);
            unitInfo.OnCompleted= async (IsSuccess) => {
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

        private void CloseUnintInfor_Tapped(object sender,EventArgs e)
        {
            contentUnitInfor.IsVisible = false;
        }

        private async void ShowMoreDanhSachDatCho_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageDanhSachDatCho++;
            await viewModel.LoadQueues();
            LoadingHelper.Hide();
        }

        public void SetButton()
        {
            if (btnGiuCho.IsVisible == false && viewModel.IsShowBtnBangTinhGia == false)
            {
                gridButton.IsVisible = false;
            }
            else if (btnGiuCho.IsVisible == true && viewModel.IsShowBtnBangTinhGia == true)
            {
                gridButton.IsVisible = true;
                btnGiuCho.IsVisible = true;
                btnBangTinhGia.IsVisible = viewModel.IsShowBtnBangTinhGia;
                Grid.SetColumn(btnGiuCho, 0);
                Grid.SetColumnSpan(btnGiuCho, 1);
                Grid.SetColumn(btnBangTinhGia, 1);
                Grid.SetColumnSpan(btnBangTinhGia, 1);
            }
            else if (btnGiuCho.IsVisible == true && viewModel.IsShowBtnBangTinhGia == false)
            {
                gridButton.IsVisible = true;
                btnGiuCho.IsVisible = true;
                btnBangTinhGia.IsVisible = viewModel.IsShowBtnBangTinhGia;
                Grid.SetColumn(btnGiuCho, 0);
                Grid.SetColumnSpan(btnGiuCho, 2);
                Grid.SetColumn(btnBangTinhGia, 0);
            }
            else if (btnGiuCho.IsVisible == false && viewModel.IsShowBtnBangTinhGia == true)
            {
                gridButton.IsVisible = true;
                btnGiuCho.IsVisible = false;
                btnBangTinhGia.IsVisible = viewModel.IsShowBtnBangTinhGia;
                Grid.SetColumn(btnGiuCho, 0);
                Grid.SetColumn(btnBangTinhGia, 0);
                Grid.SetColumnSpan(btnBangTinhGia, 2);
            }
        }

        private void GiuCho_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            QueueForm queue = new QueueForm(viewModel.Unit.productid, true);
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

        private void BangTinhGia_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();

            LoadingHelper.Hide();
        }

        private void GiuChoItem_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var itemId = (Guid)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
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
                    ToastMessageHelper.ShortMessage("Không tìm thấy thông tin");
                }
            };
        }

        private void ScreenChange_SizeChanged(System.Object sender, System.EventArgs e)
        {
            //WidthRequest = "{Binding Source={Reference flexContentUnits},Path=Padding, Converter={StaticResource UnitItemWidthConverter}}"
            //((stackFloors.Children.Content as RadExpander).Content as FlexLayout).Children as RadBorder;
            // a.WidthRequest = 100;

            //List<RadBorder> radBorders = new List<RadBorder>();
            //foreach (var item in stackFloors.Children)
            //{
            //    radBorders.Add((RadBorder)item);
            //}

            //foreach (var item in radBorders)
            //{
            //    var a = item.Content as RadExpander;

            //}



        }
    }
}
