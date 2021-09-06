using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DirectSaleDetail : ContentPage
    {
        public static bool? NeedToRefreshDirectSale = null;
        public static bool? NeedToRefreshQueues = null;
        public Action<int> OnComplete;
        private DirectSaleDetailViewModel viewModel;
        private int currentBlock = 0;

        public DirectSaleDetail()
        {
            InitializeComponent();
        }

        public DirectSaleDetail(DirectSaleSearchModel filter)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new DirectSaleDetailViewModel(filter);
            NeedToRefreshQueues = false;
            NeedToRefreshDirectSale = false;
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
                await viewModel.LoadQueues(viewModel.Unit.productid);
                await viewModel.LoadUnitById(viewModel.Unit.productid);
                viewModel.UnitStatusCode = StatusCodeUnit.GetStatusCodeById(viewModel.Unit.statuscode.ToString());
                NeedToRefreshQueues = false;
                LoadingHelper.Hide();
            }

            if (NeedToRefreshDirectSale == true)
            {
                LoadingHelper.Show();
                viewModel.Floors.Clear();
                await viewModel.LoadTotalDirectSale();
                var firstBlock = viewModel.DirectSaleResult.SingleOrDefault(x=>x.ID == viewModel.Unit.blockid.ToString());
                viewModel.ResetDirectSale(firstBlock);

                await viewModel.LoadUnitByFloor(viewModel.Unit.floorid);
                int indexFloor = viewModel.Floors.IndexOf(viewModel.Floors.SingleOrDefault(x => x.bsd_floorid == viewModel.Unit.floorid));
                SetContentFloor(indexFloor);
                LoadingHelper.Hide();
            }
        }
        public async void Init()
        {
            await viewModel.LoadTotalDirectSale();
            SetBlocks();

            var firstBlock = viewModel.DirectSaleResult.FirstOrDefault();
            viewModel.ResetDirectSale(firstBlock);

            if (viewModel.Floors.Count != 0)
            {
                currentBlock = viewModel.Blocks.FindIndex(x => x.bsd_blockid == viewModel.blockId);
                SetActiveBlock();

                var floorId = viewModel.Floors.FirstOrDefault().bsd_floorid;
                await viewModel.LoadUnitByFloor(floorId);
                SetContentFloor();

                OnComplete?.Invoke(0);
            }
            else
            {
                OnComplete?.Invoke(1);
            }
        }

        private void SetBlocks()
        {
            List<Block> blocks = new List<Block>();
            foreach (var item in viewModel.DirectSaleResult)
            {
                Block block = new Block();
                block.bsd_blockid = Guid.Parse(item.ID);
                block.bsd_name = item.name;
                blocks.Add(block);
            }
            viewModel.Blocks = blocks;
        }

        private void SetContentFloor(int index = 0)
        {
            var content = ((stackFloors.Children[index] as RadBorder).Content as StackLayout).Children[3] as FlexLayout;
            BindableLayout.SetItemsSource(content, viewModel.Floors[index].Units);
            content.IsVisible = true;
        }

        private async void ItemFloor_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            var item = sender as RadBorder;
            int CurrentFloor = stackFloors.Children.IndexOf(item);
            FlexLayout units = ((stackFloors.Children[CurrentFloor] as RadBorder).Content as StackLayout).Children[3] as FlexLayout;
            if (viewModel.Floors[CurrentFloor].Units.Count == 0)
            {
                var floorId = (Guid)(item.GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
                await viewModel.LoadUnitByFloor(floorId);
                BindableLayout.SetItemsSource(units, viewModel.Floors[CurrentFloor].Units);
            }
            
            units.IsVisible = !units.IsVisible ;
            LoadingHelper.Hide();
        }

        public async void Block_Tapped(object sender,EventArgs e)
        {
            LoadingHelper.Show();
            var blockChoosed = sender as RadBorder;
            if (stackBlocks.Children.IndexOf(blockChoosed) == currentBlock) return;
            SetInActiveBlock();
            this.currentBlock = stackBlocks.Children.IndexOf(blockChoosed);
            SetActiveBlock();
            await Task.Delay(1);

            var item = (Block)(blockChoosed.GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            viewModel.blockId = item.bsd_blockid;
            viewModel.Floors.Clear();
            viewModel.NumChuanBiInBlock = "0";
            viewModel.NumSanSangInBlock = "0";
            viewModel.NumGiuChoInBlock = "0";
            viewModel.NumDatCocInBlock = "0";
            viewModel.NumDongYChuyenCoInBlock = "0";
            viewModel.NumDaDuTienCocInBlock = "0";
            viewModel.NumThanhToanDot1InBlock = "0";
            viewModel.NumDaBanInBlock = "0";

            var block = viewModel.DirectSaleResult.SingleOrDefault(x => x.ID == viewModel.blockId.ToString());
            if (block != null)
            {
                viewModel.ResetDirectSale(block);
                
                var floorId = viewModel.Floors.FirstOrDefault().bsd_floorid;
                await viewModel.LoadUnitByFloor(floorId);

                var content = ((stackFloors.Children[0] as RadBorder).Content as StackLayout).Children[3] as FlexLayout;
                BindableLayout.SetItemsSource(content, viewModel.Floors[0].Units);
                content.IsVisible = true;
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
            var unitId = (Guid)((sender as RadBorder).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;

            viewModel.PageDanhSachDatCho = 1;
            viewModel.QueueList.Clear();
            await Task.WhenAll(
                viewModel.LoadQueues(unitId),
                viewModel.CheckShowBtnBangTinhGia(unitId),
                viewModel.LoadUnitById(unitId)
                );

            viewModel.UnitStatusCode = StatusCodeUnit.GetStatusCodeById(viewModel.Unit.statuscode.ToString());
            if (!string.IsNullOrWhiteSpace(viewModel.Unit.bsd_direction))
            {
                viewModel.UnitDirection = DirectionData.GetDiretionById(viewModel.Unit.bsd_direction);
            }
            if (!string.IsNullOrWhiteSpace(viewModel.Unit.bsd_view))
            {
                viewModel.UnitView = ViewData.GetViewById(viewModel.Unit.bsd_view);
            }

            if (viewModel.UnitStatusCode.Id == "1" || viewModel.UnitStatusCode.Id == "100000000" || viewModel.UnitStatusCode.Id == "100000004")
            {
                btnGiuCho.IsVisible = true;
                if (viewModel.UnitStatusCode.Id != "1" && viewModel.IsShowBtnBangTinhGia == true)
                {
                    viewModel.IsShowBtnBangTinhGia = true;
                }
                else
                {
                    viewModel.IsShowBtnBangTinhGia = false;
                }    
            }
            else
            {
                btnGiuCho.IsVisible = false;
                viewModel.IsShowBtnBangTinhGia = false;
            }

            SetButton();
            
            contentUnitInfor.IsVisible = true;
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
            await viewModel.LoadQueues(viewModel.Unit.productid);
            LoadingHelper.Hide();
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

        private void Question_CLicked(object sender, EventArgs e)
        {
            stackQuestion.IsVisible = !stackQuestion.IsVisible;
        }

        private void CloseQuestion_Tapped(object sender, EventArgs e)
        {
            stackQuestion.IsVisible = !stackQuestion.IsVisible;
        }
    }
}
