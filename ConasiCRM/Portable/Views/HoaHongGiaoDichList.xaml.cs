using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ConasiCRM.Portable.ViewModels;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Helper;
using Telerik.XamarinForms.Common;
using ConasiCRM.Portable.Config;

namespace ConasiCRM.Portable.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HoaHongGiaoDichList : ContentPage
	{
        public HoaHongGiaoDichListViewModel viewModel;
		public HoaHongGiaoDichList ()
		{
			InitializeComponent ();           
            BindingContext = viewModel = new HoaHongGiaoDichListViewModel();
            LoadingHelper.Show();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
            await loadTongTienHoaHong();
            await loadTongTienHoaHongNhan();
            LoadingHelper.Hide();
        }             
        public async Task loadTongTienHoaHong()
        {
            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true'>
                      <entity name='bsd_commissiontransaction'>
                          <attribute name='bsd_totalcommission' alias='totalHoaHong' aggregate='sum' />
                          <filter type='and'>
                          </filter>
                      </entity>
                    </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<HoaHongGiaoDichListViewModel>>("bsd_commissiontransactions", xml);
            if (result != null)
            {
                viewModel.totalHoaHong = result.value[0].totalHoaHong;
                this.totalHoaHongConLai();
            }
        }
        public async Task loadTongTienHoaHongNhan()
        {
            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true'>
                            <entity name='bsd_commissiontransaction'>
                                <attribute name='bsd_totalcommission' alias='totalHoaHongNhan' aggregate='sum' />
                                <filter type='and'>
                                    <condition attribute='statuscode' operator='eq' value='100000001' />
                                </filter>
                            </entity>
                          </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<HoaHongGiaoDichListViewModel>>("bsd_commissiontransactions", xml);
            if (result != null)
            {
                viewModel.totalHoaHongNhan = result.value[0].totalHoaHongNhan;
                this.totalHoaHongConLai();
            }
        }
        public void totalHoaHongConLai()
        {
            viewModel.totalHoaHongConLai = viewModel.totalHoaHong - viewModel.totalHoaHongNhan;
        }
        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            LoadingHelper.Show();
            HoaHongGiaoDichListModel val = e.Item as HoaHongGiaoDichListModel;
            HoaHongGiaoDichForm newPage = new HoaHongGiaoDichForm(val.bsd_commissiontransactionid);
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