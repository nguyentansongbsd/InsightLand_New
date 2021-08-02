using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
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
    public partial class HoaHongGiaoDichForm : ContentPage
    {
        public Action<bool> CheckData;
        private Guid _hoaHongGiaoDichId;
        public HoaHongGiaoDichFormViewModel viewModel;
        public HoaHongGiaoDichForm(Guid idHoaHongGD)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new HoaHongGiaoDichFormViewModel();
            this._hoaHongGiaoDichId = idHoaHongGD;
            viewModel.IsBusy = true;
            Init();
        }

        private async void Init()
        {
            await loadData();
            if (viewModel.HoaHongGiaoDich != null)
                CheckData(true);
            else
                CheckData(false);
        }
        public async Task loadData()
        {
            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='bsd_commissiontransaction'>
                                <all-attributes/>
                                <order attribute='createdon' descending='false' />
                                <filter type='and'>
                                    <condition attribute='bsd_commissiontransactionid' operator='eq' value='" + this._hoaHongGiaoDichId + @"' />
                                </filter>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='project'>
                                    <attribute name='bsd_name' alias='project_bsd_name'/>
                                </link-entity>
                                <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='products'>
                                  <attribute name='name' alias='products_name' />
                                </link-entity>
                                <link-entity name='account' from='accountid' to='bsd_saleagentcompany' visible='false' link-type='outer' alias='accounts'>
                                  <attribute name='bsd_name' alias='accounts_bsd_name' />
                                </link-entity>
                                <link-entity name='quote' from='quoteid' to='bsd_reservation' visible='false' link-type='outer' alias='quotes'>
                                  <attribute name='name' alias='quotes_name' />
                                </link-entity>
                                <link-entity name='salesorder' from='salesorderid' to='bsd_optionentry' visible='false' link-type='outer' alias='salesorder'>
                                  <attribute name='name' alias='salesorder_name' />
                                </link-entity>
                                <link-entity name='systemuser' from='systemuserid' to='bsd_approver' visible='false' link-type='outer' alias='systemusers'>
                                  <attribute name='fullname' alias='systemusers_name' />
                                </link-entity>
                                <link-entity name='systemuser' from='systemuserid' to='bsd_salestaff' visible='false' link-type='outer' alias='systemuser_sale'>
                                  <attribute name='fullname' alias='sale_name' />
                                </link-entity>
                                <link-entity name='bsd_paymentschemedetail' from='bsd_paymentschemedetailid' to='bsd_installment' visible='false' link-type='outer' alias='paymentschemedetail'>
                                  <attribute name='bsd_name' alias='paymentschemedetail_name' />
                                </link-entity>
                            </entity>
                          </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<HoaHongGiaoDichFormModel>>("bsd_commissiontransactions", xml);
            var data = result.value.FirstOrDefault();
            viewModel.HoaHongGiaoDich = data;
            viewModel.Title = "Thông Tin Hoa Hồng Giao Dịch ";
            viewModel.IsBusy = false;
        }
    }
}