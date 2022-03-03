using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Resources;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QueueForm : ContentPage
    {
        public Action<bool> OnCompleted;
        public static bool? NeedToRefresh;
        public QueueFormViewModel viewModel;
        public Guid QueueId;
        private bool from;
        public QueueForm(Guid unitId, bool fromDirectSale) // Direct Sales (add)
        {
            InitializeComponent();   
            Init();
            viewModel.UnitId = unitId;
            from = fromDirectSale;
            Create();
        }

        public void Init()
        {          
            this.BindingContext = viewModel = new QueueFormViewModel();
            //centerModalKHTN.Body.BindingContext = viewModel;
            NeedToRefresh = false;
            SetPreOpen();            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefresh == true)
            {
                LoadingHelper.Show();
                Lookup_KhachHang.Refresh();
                NeedToRefresh = false;
                LoadingHelper.Hide();
            }
        }

        public async void SetPreOpen()
        {
            lookUpDaiLy.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadSalesAgentCompany();
                LoadingHelper.Hide();
            };
        }

        public async void Create()
        {
            btnSave.Text = Language.tao_giu_cho;
            btnSave.Clicked += Create_Clicked; ;
            this.Title = Language.tao_moi_giu_cho_title;
            if(from)
            {
                await viewModel.LoadFromUnit(viewModel.UnitId);
                string res = await viewModel.createQueueDraft(false, viewModel.UnitId);
                topic.Text = viewModel.QueueFormModel.bsd_units_name;
                if (viewModel.QueueFormModel.bsd_units_id != Guid.Empty)
                    OnCompleted?.Invoke(true);
                else
                {
                    OnCompleted?.Invoke(false);
                    ToastMessageHelper.ShortMessage(res);
                }
            }
            else
            {
                await viewModel.LoadFromProject(viewModel.UnitId);
                string res = await viewModel.createQueueDraft(true, viewModel.UnitId);
                topic.Text = viewModel.QueueFormModel.bsd_project_name +" - "+ DateTime.Now.ToString("dd/MM/yyyy");
                if (viewModel.QueueFormModel.bsd_project_id != Guid.Empty)
                    OnCompleted?.Invoke(true);
                else
                {
                    OnCompleted?.Invoke(false);
                    ToastMessageHelper.ShortMessage(res);
                }
            }            
        }

        private async void Create_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            btnSave.Text = Language.dang_tao_giu_cho;
            await SaveData(null);
        }

        private async Task SaveData(string id)
        {
            if (string.IsNullOrWhiteSpace(viewModel.QueueFormModel.name))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_tieu_de);
                LoadingHelper.Hide();
                btnSave.Text = Language.tao_giu_cho;
                return;
            }
            if (viewModel.Customer == null || string.IsNullOrWhiteSpace(viewModel.Customer.Val))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_khach_hang);
                LoadingHelper.Hide();
                btnSave.Text = Language.tao_giu_cho;
                return;
            }
            if (viewModel.DailyOption == null || viewModel.DailyOption.Id == null || viewModel.DailyOption.Id == Guid.Empty)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_dai_ly_ban_hang);
                LoadingHelper.Hide();
                btnSave.Text = Language.tao_giu_cho;
                return;
            }
            if (from)
            {
                if (!await viewModel.SetQueueTime())
                {
                    ToastMessageHelper.ShortMessage(Language.khach_hang_da_tham_gia_giu_cho_cho_du_an_nay);
                    LoadingHelper.Hide();
                    btnSave.Text = Language.tao_giu_cho;
                    return;
                }
            }
            if (viewModel.Customer != null && !string.IsNullOrWhiteSpace(viewModel.Customer.Val) && viewModel.DailyOption != null && viewModel.DailyOption.Id != Guid.Empty && viewModel.DailyOption.Id == Guid.Parse(viewModel.Customer.Val))
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_phai_khac_dai_ly_ban_hang);
                LoadingHelper.Hide();
                btnSave.Text = Language.tao_giu_cho;
                return;
            }
            var created = await viewModel.UpdateQueue(viewModel.idQueueDraft);
            if (created)
            {
                if (ProjectInfo.NeedToRefreshQueue.HasValue) ProjectInfo.NeedToRefreshQueue = true;
                if (DirectSaleDetail.NeedToRefreshDirectSale.HasValue) DirectSaleDetail.NeedToRefreshDirectSale = true;
                if (UnitInfo.NeedToRefreshQueue.HasValue) UnitInfo.NeedToRefreshQueue = true;
                if (Dashboard.NeedToRefreshQueue.HasValue) Dashboard.NeedToRefreshQueue = true;
                await Navigation.PopAsync();       
                ToastMessageHelper.ShortMessage(Language.tao_giu_cho_thanh_cong);
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                btnSave.Text = Language.tao_giu_cho;
                ToastMessageHelper.ShortMessage(Language.tao_giu_cho_that_bai);
            }
        }
    }
}