using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QueueForm : ContentPage
    {
        public Action<bool> OnCompleted;
        public static bool? NeedToRefreshContactList = null;
        public static bool? NeedToRefreshAccountList = null;
        public QueueFormViewModel viewModel;
        public Guid UnitId;
        public Guid QueueId;
        private bool from;
        public QueueForm(Guid unitId, bool fromDirectSale) // Direct Sales (add)
        {
            InitializeComponent();         
            UnitId = unitId;
            from = fromDirectSale;
            Init();
            Create();
        }

        public void Init()
        {          
            this.BindingContext = viewModel = new QueueFormViewModel();
            centerModalKHTN.Body.BindingContext = viewModel;
            NeedToRefreshAccountList = false;
            NeedToRefreshContactList = false;
            SetPreOpen();            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (NeedToRefreshAccountList == true)
            {
                LoadingHelper.Show();
                viewModel.AccountsLookUp.Clear();
                await viewModel.LoadAccountsLookUp();
                NeedToRefreshAccountList = false;
                LoadingHelper.Hide();
            }

            if (NeedToRefreshContactList == true)
            {
                LoadingHelper.Show();
                viewModel.ContactsLookUp.Clear();
                await viewModel.LoadContactsLookUp();
                NeedToRefreshContactList = false;
                LoadingHelper.Hide();
            }
        }

        public async void SetPreOpen()
        {
            lookUpDaiLy.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadSalesAgent();
                LoadingHelper.Hide();
            };
            if (viewModel.AccountsLookUp.Count <= 0)
            {
                LoadingHelper.Show();
                await viewModel.LoadAccountsLookUp();
                LoadingHelper.Hide();
            }
            if (viewModel.ContactsLookUp.Count <= 0)
            {
                LoadingHelper.Show();
                await viewModel.LoadContactsLookUp();
                LoadingHelper.Hide();
            }
        }

        public async void Create()
        {
            btnSave.Text = "Tạo Giữ Chỗ";
            btnSave.Clicked += Create_Clicked; ;
            this.Title = "Tạo Giữ Chỗ";
            if(from)
            {
                await viewModel.LoadFromUnit(this.UnitId);
                topic.Text = viewModel.QueueFormModel.bsd_units_name;
                if (viewModel.QueueFormModel.bsd_units_id != Guid.Empty)
                    OnCompleted?.Invoke(true);
                else
                    OnCompleted?.Invoke(false);
            }
            else
            {
                await viewModel.LoadFromProject(this.UnitId);
                topic.Text = viewModel.QueueFormModel.bsd_project_name +" - "+ DateTime.Now.ToString("dd/MM/yyyyy");
                if (viewModel.QueueFormModel.bsd_project_id != Guid.Empty)
                    OnCompleted?.Invoke(true);
                else
                    OnCompleted?.Invoke(false);
            }            
        }

        private void Create_Clicked(object sender, EventArgs e)
        {
            SaveData(null);
        }

        private async void SaveData(string id)
        {
            if (string.IsNullOrWhiteSpace(viewModel.QueueFormModel.name))
            {
                ToastMessageHelper.ShortMessage("Vui lòng nhập tiêu đề của giữ chỗ");
                return;
            }
            if (viewModel.Customer == null || viewModel.Customer.Id == null || viewModel.Customer.Id == Guid.Empty)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn khách hàng tiềm năng");
                return;
            }
            if (viewModel.DailyOption == null || viewModel.DailyOption.Id == null || viewModel.DailyOption.Id == Guid.Empty)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn đại lý bán hàng");
                return;
            }
            if (from)
            {
                if (!await viewModel.SetQueueTime())
                {
                    ToastMessageHelper.ShortMessage("Khách hàng tiềm năng đã tham gia giữ chỗ cho dự án này");
                    return;
                }
            }
            if (viewModel.Customer != null && viewModel.Customer.Id != Guid.Empty && viewModel.DailyOption != null && viewModel.DailyOption.Id != Guid.Empty && viewModel.DailyOption.Id == viewModel.Customer.Id)
            {
                ToastMessageHelper.ShortMessage("Khách hàng tiềm năng phải khác Đại lý bán hàng");
                return;
            }

            LoadingHelper.Show();
            var created = await viewModel.createQueue();
            if (created)
            {
                if (ProjectInfo.NeedToRefreshQueue.HasValue) ProjectInfo.NeedToRefreshQueue = true;
                if (DirectSaleDetail.NeedToRefreshDirectSale.HasValue) DirectSaleDetail.NeedToRefreshDirectSale = true;
                if (DirectSaleDetail.NeedToRefreshQueues.HasValue) DirectSaleDetail.NeedToRefreshQueues = true;
                if (UnitInfo.NeedToRefreshQueue.HasValue) UnitInfo.NeedToRefreshQueue = true;
                await Navigation.PopAsync();       
                ToastMessageHelper.ShortMessage("Tạo giữ chỗ thành công");
                LoadingHelper.Hide();
            }
            else
            {
                LoadingHelper.Hide();
                ToastMessageHelper.ShortMessage("Tạo giữ chỗ thất bại");
            }
        }

        private async void KhachHangTiemNang_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            Tab_Tapped(1);           
            await centerModalKHTN.Show();
            LoadingHelper.Hide();
        }

        private void Contact_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(1);
        }

        private void Account_Tapped(object sender, EventArgs e)
        {
            Tab_Tapped(2);
        }

        private async void CloseKHTN_Clicked(object sender, EventArgs e)
        {
            await centerModalKHTN.Hide();
        }       

        private void Tab_Tapped(int tab)
        {
            if (tab == 1)
            {
                VisualStateManager.GoToState(radBorderContact, "Selected");
                VisualStateManager.GoToState(lbContact, "Selected");
                LookUpContact.IsVisible = true;
            }
            else
            {
                VisualStateManager.GoToState(radBorderContact, "Normal");
                VisualStateManager.GoToState(lbContact, "Normal");
                LookUpContact.IsVisible = false;
            }
            if (tab == 2)
            {
                VisualStateManager.GoToState(radBorderAccount, "Selected");
                VisualStateManager.GoToState(lbAccount, "Selected");
                LookUpAccount.IsVisible = true;
            }
            else
            {
                VisualStateManager.GoToState(radBorderAccount, "Normal");
                VisualStateManager.GoToState(lbAccount, "Normal");
                LookUpAccount.IsVisible = false;
            }
        }

        private async void LookUpContact_ItemTapped(object sender, EventArgs e)
        {
            var item = (LookUp)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item != null)
            {
                viewModel.Customer = new LookUp();
                viewModel.Customer.Id = item.Id;
                viewModel.Customer.Name = item.Name;
                viewModel.Customer.Detail = "2";
                viewModel.QueueFormModel.customer_name = item.Name;
            }
            await centerModalKHTN.Hide();
        }

        private async void LookUpAccount_ItemTapped(object sender, EventArgs e)
        {
            var item = (LookUp)((sender as StackLayout).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
            if (item != null)
            {
                viewModel.Customer = new LookUp();
                viewModel.Customer.Id = item.Id;
                viewModel.Customer.Name = item.Name;
                viewModel.Customer.Detail = "1";
                viewModel.QueueFormModel.customer_name = item.Name;
            }
            await centerModalKHTN.Hide();
        }      

        private async void AddContact_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Navigation.PushAsync(new ContactForm());
            LoadingHelper.Hide();
        }

        private async void AddAccount_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Navigation.PushAsync(new AccountForm());
            LoadingHelper.Hide();
        }
    }
}