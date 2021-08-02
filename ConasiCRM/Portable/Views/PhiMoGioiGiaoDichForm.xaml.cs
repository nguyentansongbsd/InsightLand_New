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
    public partial class PhiMoGioiGiaoDichForm : ContentPage
    {
        public Action<bool> CheckData;
        public PhiMoGioiGiaoDichFormViewModel viewModel;
        public Guid idPMGGD;
        public PhiMoGioiGiaoDichForm(Guid _idPMGGD)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new PhiMoGioiGiaoDichFormViewModel();
            viewModel.IsBusy = true;
            this.idPMGGD = _idPMGGD;
            Init();
        }

        private async void Init()
        {
            await loadData();
            if (viewModel.PhiMoGioiGD != null)
                CheckData(true);
            else
                CheckData(false);
        }


        public async Task loadData()
        {
            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='bsd_brokeragetransaction'>
                  <all-attributes/>
                  <order attribute='createdon' descending='false' />
                  <filter type='and'>
                      <condition attribute='bsd_brokeragetransactionid' operator='eq' value='" + this.idPMGGD + @"' />
                  </filter>
                  <link-entity name='quote' from='quoteid' to='bsd_reservation' visible='false' link-type='outer' alias='quote'>
                      <attribute name='name' alias='quote_name'/>
                    </link-entity>
                    <link-entity name='bsd_brokeragefees' from='bsd_brokeragefeesid' to='bsd_brokeragefees' visible='false' link-type='outer' alias='brokeragefees'>
                      <attribute name='bsd_name' alias='brokeragefees_name'/>
                    </link-entity>
                    <link-entity name='contact' from='contactid' to='bsd_customer' visible='false' link-type='outer' alias='contact'>
                    <attribute name='bsd_fullname' alias='contact_bsd_fullname'/>
                  </link-entity>
                  <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='product'>
                  <attribute name='name' alias='product_name'/>
                </link-entity>
                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='project'>
                <attribute name='bsd_name' alias='project_bsd_name'/>
              </link-entity>
              <link-entity name='account' from='accountid' to='bsd_customer' visible='false' link-type='outer' alias='account'>
              <attribute name='bsd_name' alias='account_bsd_name'/>
            </link-entity>
            <link-entity name='contact' from='contactid' to='bsd_collaborator' visible='false' link-type='outer' alias='contact_ct'> 
              <attribute name='bsd_fullname' alias='sales_name'/>
            </link-entity>
              </entity>
          </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhiMoGioiGiaoDichFormModel>>("bsd_brokeragetransactions", xml);
            var data = result.value.FirstOrDefault();
            viewModel.PhiMoGioiGD = data;
            viewModel.Title = "Thông Tin Phí Mô Giới Giao Dịch ";
            viewModel.IsBusy = false;
        }
    }
}