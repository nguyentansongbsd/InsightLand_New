using ConasiCRM.Portable.Converters;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SpecialDiscountForm : ContentPage
    {
        public SpecialDiscountFormViewModel viewModel;

        // form thêm mới.
        public SpecialDiscountForm(LookUp QuoteReservation)
        {
            InitializeComponent();
            Init();
            viewModel.Title = "Tạo chiết khấu đặc biệt";
            viewModel.SpecialDiscount = new SpecicalDiscountFormModel()
            {
                bsd_approvaldate = DateTime.Now,
                quote_id = QuoteReservation.Id,
                quote_name = QuoteReservation.Name
            };
        }

        // from view/chỉnh sửa.
        private Guid DiscountId;
        public SpecialDiscountForm(Guid discountId)
        {
            InitializeComponent();
            lblCreatedOn.IsVisible = true;
            entryCreatedOn.IsVisible = true;
            Init();
            viewModel.Title = "Thông tin chiết khấu";
            DiscountId = discountId;
            LoadData();
        }

        private async void LoadData()
        {
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<SpecicalDiscountFormModel>>("bsd_discountspecials", @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='bsd_discountspecial'>
                    <attribute name='bsd_discountspecialid' />
                    <attribute name='bsd_name' />
                    <attribute name='createdon' />
                    <attribute name='bsd_reasons' />
                    <attribute name='bsd_quote' alias='quote_id' />
                    <attribute name='bsd_percentdiscount' />
                    <attribute name='bsd_cchtnh' />
                    <attribute name='bsd_approverrejecter' alias='approver_id' />
                    <attribute name='bsd_approvaldate' />
                    <attribute name='bsd_amountdiscount' />
                    <order attribute='bsd_name' descending='false' />
                    <filter type='and'>
                      <condition attribute='bsd_discountspecialid' operator='eq' uitype='bsd_discountspecial' value='" + DiscountId + @"' />
                    </filter>
                    <link-entity name='systemuser' from='systemuserid' to='bsd_approverrejecter' visible='false' link-type='outer' alias='a_ee35ee0bb314e911a97f000d3aa04914'>
                      <attribute name='fullname' alias='approver_name' />
                    </link-entity>
                    <link-entity name='quote' from='quoteid' to='bsd_quote' visible='false' link-type='outer' alias='a_2d89efffb214e911a97f000d3aa04914'>
                      <attribute name='name' alias='quote_name' />
                    </link-entity>
                  </entity>
                </fetch>");

            SpecicalDiscountFormModel discountSpecial = result?.value.FirstOrDefault();
            if (discountSpecial != null)
            {
                viewModel.CachTinh = viewModel.CachTinh_Options.SingleOrDefault(x => x.Val == discountSpecial.bsd_cchtnh);
                viewModel.Approver = new LookUp()
                {
                    Id = discountSpecial.approver_id,
                    Name = discountSpecial.approver_name
                };
                viewModel.SpecialDiscount = discountSpecial;
            }
        }

        public void Init()
        {
            this.BindingContext = viewModel = new SpecialDiscountFormViewModel();

            entrySoTienGiamGia.SetBinding(Entry.TextProperty, new Binding("SpecialDiscount.bsd_amountdiscount", converter: new EntryDecimalConverter()));
            entryPhanTramGiamGia.SetBinding(Entry.TextProperty, new Binding("SpecialDiscount.bsd_percentdiscount", converter: new EntryDecimalConverter()));

            viewModel.ModalLookUp = modalLookUp;
            viewModel.InitializeModal();
            viewModel.IsBusy = false;
        }

        private void NguoiXacNhan_OpenClicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.CurrentLookUpConfig = viewModel.CreatedByConfig;
            viewModel.ProcessLookup(nameof(viewModel.CreatedByConfig));
        }

        private void PickerCachTinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (viewModel.CachTinh.Val == "100000000")
            {
                lblSoTienGiamGia.IsVisible = true;
                entrySoTienGiamGia.IsVisible = true;

                lblPhanTramGiamGia.IsVisible = false;
                entryPhanTramGiamGia.IsVisible = false;
            }
            else if (viewModel.CachTinh.Val == "100000001")
            {
                lblSoTienGiamGia.IsVisible = false;
                entrySoTienGiamGia.IsVisible = false;

                lblPhanTramGiamGia.IsVisible = true;
                entryPhanTramGiamGia.IsVisible = true;
            }
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(viewModel.SpecialDiscount.bsd_name))
            {
                await DisplayAlert("", "Vui lòng nhập tên chiết khấu", "Đóng");
                return;
            }
            else if (string.IsNullOrWhiteSpace(viewModel.SpecialDiscount.bsd_reasons))
            {
                await DisplayAlert("", "Vui lòng nhập lý do chiết khấu", "Đóng");
                return;
            }
            else if (viewModel.CachTinh == null)
            {
                await DisplayAlert("", "Vui lòng chọn cách tính", "Đóng");
                return;
            }
            else if (viewModel.CachTinh.Val == "100000000" && viewModel.SpecialDiscount.bsd_amountdiscount.HasValue == false)
            {
                await DisplayAlert("", "Vui lòng nhập số tiền chiết khấu", "Đóng");
                return;
            }
            else if (viewModel.CachTinh.Val == "100000001" && viewModel.SpecialDiscount.bsd_percentdiscount.HasValue == false)
            {
                await DisplayAlert("", "Vui lòng nhập phần trăm chiết khấu", "Đóng");
                return;
            }
            else if (viewModel.Approver == null)
            {
                await DisplayAlert("", "Vui lòng chọn người chấp nhận", "Đóng");
                return;
            }

            viewModel.IsBusy = true;

            var data = new Dictionary<string, object>();
            data["bsd_name"] = viewModel.SpecialDiscount.bsd_name;
            data["bsd_reasons"] = viewModel.SpecialDiscount.bsd_reasons;
            data["bsd_cchtnh"] = viewModel.CachTinh.Val;
            data["bsd_approvaldate"] = viewModel.SpecialDiscount.bsd_approvaldate;
            data["bsd_amountdiscount"] = viewModel.SpecialDiscount.bsd_amountdiscount;
            data["bsd_percentdiscount"] = viewModel.SpecialDiscount.bsd_percentdiscount;
            data["bsd_approverrejecter@odata.bind"] = $"/systemusers({viewModel.Approver.Id})";
            CrmApiResponse createResponse;
            if (DiscountId == Guid.Empty)
            {
                DiscountId = Guid.NewGuid();
                data["bsd_quote@odata.bind"] = $"/quotes({viewModel.SpecialDiscount.quote_id})";
                data["bsd_discountspecialid"] = DiscountId;
                createResponse = await CrmHelper.PostData("/bsd_discountspecials", data);
            }
            else
            {
                createResponse = await CrmHelper.PatchData($"/bsd_discountspecials({DiscountId})", data);
            }

            if (createResponse.IsSuccess)
            {
                MessagingCenter.Send<SpecialDiscountForm, bool>(this, "LoadSpecialDiscounts", true);
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Thông báo", createResponse.GetErrorMessage(), "Đóng");
            }
        }

        private async void BtnCancle_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void EntryPhanTramGiamGia_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (viewModel != null && e.PropertyName == "IsVisible" && ((Entry)sender).IsVisible == false)
            {
                viewModel.SpecialDiscount.bsd_percentdiscount = null;
            }
        }

        private void EntrySoTienGiamGia_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (viewModel != null && e.PropertyName == "IsVisible" && ((Entry)sender).IsVisible == false)
            {
                viewModel.SpecialDiscount.bsd_amountdiscount = null;
            }
        }
    }
}