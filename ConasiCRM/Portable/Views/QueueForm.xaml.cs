using ConasiCRM.Portable.Helper;
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

        public QueueForm(Guid _queueId) // update
        {
            InitializeComponent();           
            QueueId = _queueId;          
            Init();
        }

        public void Init()
        {          
            this.BindingContext = viewModel = new QueueFormViewModel();
            viewModel.QueueFormModel.createdon = DateTime.Now;
            SetPreOpen();            
        }
        public async void Create()
        {
            btnSave.Text = "Lưu";
            btnSave.Clicked += Create_Clicked; ;
            this.Title = "Tạo Giữ Chỗ";
            if(from)
            {
                await viewModel.LoadFromUnit(this.UnitId);
                if (viewModel.QueueFormModel.bsd_units_id != Guid.Empty)
                    OnCompleted?.Invoke(true);
                else
                    OnCompleted?.Invoke(false);
            }
            else
            {
                await viewModel.LoadFromProject(this.UnitId);
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
                await DisplayAlert("Thông Báo", "Vui lòng nhập tiêu đề của giữ chỗ", "Đóng");
                return;
            }
            if (viewModel.Customer == null || viewModel.Customer.Id == null || viewModel.Customer.Id == Guid.Empty)
            {
                await DisplayAlert("Thông Báo", "Vui lòng chọn khách hàng tiềm năng", "Đóng");
                return;
            }
            if (viewModel.DailyOption == null || viewModel.DailyOption.Id == null || viewModel.DailyOption.Id == Guid.Empty)
            {
                await DisplayAlert("Thông Báo", "Vui lòng chọn đại lý", "Đóng");
                return;
            }
            if (from)
            {
               if (!await viewModel.SetQueueTime(this.UnitId))
                {
                    await DisplayAlert("Thông Báo", "Khách hàng tiềm năng đã tham gia giữ chỗ cho dự án này", "Đóng");
                    return;
                }
            }
            else
            {
                if (!await viewModel.SetQueueTimeProject(this.UnitId))
                {
                    await DisplayAlert("Thông Báo", "Khách hàng tiềm năng đã tham gia giữ chỗ cho dự án này", "Đóng");
                    return;
                }
            }

            LoadingHelper.Show();
            var created = await viewModel.createQueue();
            if (created)
            {
                if (DirectSaleDetail.NeedToRefreshQueues.HasValue) DirectSaleDetail.NeedToRefreshQueues = true;
                await Navigation.PopAsync(); 
                LoadingHelper.Hide();
                await DisplayAlert("Thông báo", "Tạo giữ chỗ thành công!", "OK");
            }
            else
            {
                LoadingHelper.Hide();
                await DisplayAlert("Thông báo", "Tạo giữ chỗ thất bại", "OK");
            }
        }

        private async void KhachHangTiemNang_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            Tab_Tapped(1);           
            LoadingHelper.Hide();
            await centerModalKHTN.Show();
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
                VisualStateManager.GoToState(radBorderContact, "Normal");
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

        public async void SetPreOpen()
        {
            lookUpDaiLy.PreOpenAsync = async () =>
            {
                await viewModel.LoadSalesAgent();
            };
            if (viewModel.AccountsLookUp.Count <= 0)
            {
                await viewModel.LoadAccountsLookUp();
            }
            if (viewModel.ContactsLookUp.Count <= 0)
            {
                await viewModel.LoadContactsLookUp();
            }
            LookUpAccount.SetList(viewModel.AccountsLookUp, "Name");
            LookUpContact.SetList(viewModel.ContactsLookUp, "Name");
            LookUpAccount.lookUpListView.ItemTapped += LookUpAccount_ItemTapped;
            LookUpContact.lookUpListView.ItemTapped += LookUpContac_ItemTapped;
        }

        private async void LookUpContac_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e != null)
            {
                var item = e.Item as LookUp;
                if(item != null)
                {
                    viewModel.Customer = new LookUp();
                    viewModel.Customer.Id = item.Id;
                    viewModel.Customer.Name = item.Name;
                    viewModel.Customer.Detail = "2";
                    viewModel.QueueFormModel.customer_name = item.Name;
                }    
            }
            await centerModalKHTN.Hide();
        }

        private async void LookUpAccount_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e != null)
            {
                var item = e.Item as LookUp;
                if (item != null)
                {
                    viewModel.Customer = new LookUp();
                    viewModel.Customer.Id = item.Id;
                    viewModel.Customer.Name = item.Name;
                    viewModel.Customer.Detail = "1";
                    viewModel.QueueFormModel.customer_name = item.Name;
                }
            }
            await centerModalKHTN.Hide();
        }      

        //private async void CancleQueue_Clicked(object sender, EventArgs e)
        //{
        //    bool confirm = await DisplayAlert("Xác nhận", "Bạn có muốn hủy đặt chỗ này không ?", "Đồng ý", "Hủy");
        //    if (confirm == false) return;

        //    viewModel.IsBusy = true;
        //    if (this.QueueId != Guid.Empty)
        //    {
        //        string url_action = $"/opportunities({this.QueueId})/Microsoft.Dynamics.CRM.bsd_Action_Queue_CancelQueuing";
        //        CrmApiResponse res = await CrmHelper.PostData(url_action, null);
        //        if (res.IsSuccess)
        //        {
        //            url_action = $"/opportunities({this.QueueId})/Microsoft.Dynamics.CRM.bsd_Action_Opportunity_HuyGiuChoCoTien";
        //            res = await CrmHelper.PostData(url_action, null);
        //            //await Navigation.PushAsync(new QueueForm(this.QueueId));
        //            Navigation.RemovePage(this);
        //        }
        //        else
        //        {
        //            await DisplayAlert("Thông báo", "Hủy báo giá thất bại." + res.GetErrorMessage(), "close");
        //        }
        //    }
        //    viewModel.IsBusy = false;
        //}

        //private async void BtnDatCoc_Clicked(object sender, EventArgs e)
        //{
        //    bool confirm = await DisplayAlert("Xác nhận", "Bạn có muốn tạo báo giá không ?", "Đồng ý", "Hủy");
        //    if (confirm == false) return;

        //    viewModel.IsBusy = true;
        //    var data = new
        //    {
        //        Command = "Reservation",
        //        Parameters = "[" + JsonConvert.SerializeObject(new
        //        {
        //            action = "Reservation",
        //            name = "opportunity",
        //            value = this.QueueId.ToString()
        //        }) + "]"
        //    };

        //    var res = await CrmHelper.PostData($"/products({viewModel.QueueFormModel.bsd_units_id})//Microsoft.Dynamics.CRM.bsd_Action_DirectSale", data);
        //    if (res.IsSuccess)
        //    {
        //        DirectSaleActionResponse directSaleActionResponse = JsonConvert.DeserializeObject<DirectSaleActionResponse>(res.Content);
        //        DirectSaleActionSubResponse subResponse = directSaleActionResponse.GetSubResponse();
        //        if (subResponse.type == "Success")
        //        {
        //            // tạo báo giá thành công, thì lấy Discount List từ PhasedLanch đưa vào Reservation.
        //            // tạo báo giá thành công thì cũng cập nhật lại field Queue trên đặt cọc.
        //            Guid CreateReservationId = Guid.Parse(subResponse.content);
        //            if (viewModel.QueueFormModel.bsd_discountlist_id != Guid.Empty)
        //            {
        //                // update lại reservation field discount list
        //                var updateData = new Dictionary<string, object>();
        //                updateData["bsd_discountlist@odata.bind"] = $"discounttypes({viewModel.QueueFormModel.bsd_discountlist_id})";
        //                var updateDiscountListResponse = await CrmHelper.PatchData($"/quotes({CreateReservationId})", updateData);
        //                if (updateDiscountListResponse.IsSuccess == false)
        //                {
        //                    await DisplayAlert("Thông báo", "Tạo báo giá thành công. Không cập nhật được Discount List từ Đợt mở bán", "Đóng");
        //                }
        //                else
        //                {
        //                    await DisplayAlert("Thông báo", "Tạo báo giá thành công. ", "Đóng");
        //                }
        //            }
        //            else
        //            {
        //                await DisplayAlert("Thông báo", "Tạo báo giá thành công. ", "Đóng");
        //            }
        //            await Navigation.PushAsync(new ReservationForm(CreateReservationId));
        //        }
        //        else await DisplayAlert("Thông báo", "Tạo báo giá thất bạn. " + subResponse.content, "Đóng");
        //    }
        //    else await DisplayAlert("Thông báo", "Tạo báo giá thất bại ." + res.GetErrorMessage(), "close");
        //    viewModel.IsBusy = false;
        //} 
    }
}