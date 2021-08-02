using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhiMoGioiGiaoDichList : ContentPage
    {
        public PhiMoGioiGiaoDichListViewModel viewModel;
        public PhiMoGioiGiaoDichList()
        {
            InitializeComponent();
            BindingContext = viewModel = new PhiMoGioiGiaoDichListViewModel();
            LoadingHelper.Show();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
            await loadTotalAmount();
            await loadTotalAmountReceived();
            LoadingHelper.Hide();
        }             
        public async Task loadTotalAmount()
        {

            string xml_total = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true'>
                                    <entity name='bsd_brokeragetransaction'>
                                        <attribute name='bsd_totalamount' alias='totalPMG' aggregate='sum' />
                                        <filter type='and'>
                                        </filter>
                                    </entity>
                                  </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhiMoGioiGiaoDichListViewModel>>("bsd_brokeragetransactions", xml_total);
            if (result != null)
            {
                viewModel.totalPMG = result.value[0].totalPMG;
                this.totalPMGConLai();
            }
        }
        public async Task loadTotalAmountReceived()
        {
            string xml_total = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true'>
                                    <entity name='bsd_brokeragetransaction'>
                                        <attribute name='bsd_totalamount' alias='totalPMGNhan' aggregate='sum' />
                                        <filter type='and'>
                                          <condition attribute='statuscode' operator='eq' value='100000001' />
                                        </filter>
                                    </entity>
                                  </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhiMoGioiGiaoDichListViewModel>>("bsd_brokeragetransactions", xml_total);

            if (result != null)
            {
                viewModel.totalPMGNhan = result.value[0].totalPMGNhan;
                this.totalPMGConLai();
            }

        }
        public void totalPMGConLai()
        {
            viewModel.totalPMGConLai = viewModel.totalPMG - viewModel.totalPMGNhan;
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            LoadingHelper.Show();
            PhiMoGioiGiaoDichListModel val = e.Item as PhiMoGioiGiaoDichListModel;
            PhiMoGioiGiaoDichForm newPage = new PhiMoGioiGiaoDichForm(val.bsd_brokeragetransactionid);
            newPage.CheckData = async (CheckData) =>
            {
                if (CheckData == true)
                {
                    await Navigation.PushAsync(newPage);
                }
                LoadingHelper.Hide();
            };
        }
    }
}