using ConasiCRM.Portable.Helper;
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
    public partial class MandatorySecondaryForm : ContentPage
    {
        private MandatorySecondaryFormViewModel viewModel;
        public MandatorySecondaryForm(Guid id)
        {
            InitializeComponent();
            Init(id.ToString());
        }

        private async void Init(string id)
        {
            this.BindingContext = viewModel = new MandatorySecondaryFormViewModel();
            SetPreOpen();
            await viewModel.GetOneAccountById(id);
        }

        public void SetPreOpen()
        {
            Lookup_Account.PreOpenAsync = async () =>
            {
                await viewModel.LoadContactsLookup();
            };
        }

        private async void AddMandatorySecondary_Clicked(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(viewModel.mandatorySecondary.bsd_name))
            {
                await DisplayAlert("Thông Báo", "Vui lòng nhập topic", "Đóng");
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.mandatorySecondary.bsd_descriptionsvn))
            {
                await DisplayAlert("Thông Báo", "Vui lòng nhập mô tả (VN)", "Đóng");
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.mandatorySecondary.bsd_descriptionsen))
            {
                await DisplayAlert("Thông Báo", "Vui lòng nhập mô tả (EN)", "Đóng");
                return;
            }
            if(viewModel.Contact.Id==null)
            {
                await DisplayAlert("Thông Báo", "Vui lòng chọn người ủy quyền", "Đóng");
                return;
            }
            LoadingHelper.Show();
            viewModel.mandatorySecondary.bsd_contactid = viewModel.Contact.Id;
            if(await viewModel.Save())
            {
                LoadingHelper.Hide();
                await Navigation.PopAsync();
                await DisplayAlert("Thông Báo", "Đã tạo người uỷ quyền thành công", "OK");
            }
            else
            {
                LoadingHelper.Hide();
                await DisplayAlert("Thông Báo", "Tạo người uỷ quyền thất bại", "OK");
            }
        }

        private async void Effectivedateto_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (viewModel.mandatorySecondary.bsd_effectivedatefrom == null)
                viewModel.mandatorySecondary.bsd_effectivedatefrom = DateTime.Now;
            if (this.compareDateTime(viewModel.mandatorySecondary.bsd_effectivedatefrom, viewModel.mandatorySecondary.bsd_effectivedateto) == -1)
            {
                viewModel.mandatorySecondary.bsd_effectivedateto = viewModel.mandatorySecondary.bsd_effectivedatefrom;
                await DisplayAlert("Thông Báo", "Ngày hết hiệu lực phải lớn hơn ngày bắt đầu", "Đóng");
            }
        }

        private async void Effectivedatefrom_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (viewModel.mandatorySecondary.bsd_effectivedateto == null)
                viewModel.mandatorySecondary.bsd_effectivedateto = DateTime.Now;
            if (this.compareDateTime(viewModel.mandatorySecondary.bsd_effectivedatefrom,viewModel.mandatorySecondary.bsd_effectivedateto) == -1)
            {
                viewModel.mandatorySecondary.bsd_effectivedatefrom = viewModel.mandatorySecondary.bsd_effectivedateto;
                await DisplayAlert("Thông Báo", "Ngày hết hiệu lực phải lớn hơn ngày bắt đầu", "Đóng");
            }    
        }

        private int compareDateTime(DateTime? date, DateTime? date1)
        {
            if (date != null && date != null)
            {
                int result = DateTime.Compare(date.Value, date1.Value);
                if (result < 0)
                    return -1;
                else if (result == 0)
                    return 0;
                else
                    return 1;
            }
            if (date == null && date1 != null)
                return -1;
            if (date1 == null && date != null)
                return 1;
            return 0;
        }
    }
}