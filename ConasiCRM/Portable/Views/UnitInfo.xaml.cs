using ConasiCRM.Portable.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UnitInfo : ContentPage
    {
        public Action<bool> OnCompleted;
        private UnitInfoViewModel viewModel;
        private Guid Id;
        public UnitInfo(Guid id)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new UnitInfoViewModel();
            this.Id = id;
            Load();
        }
        public async void Load()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='product'>
                <attribute name='productid' />
                <attribute name='bsd_units' />
                <attribute name='name' />
                <attribute name='productnumber' />
                <attribute name='bsd_queuingfee' />
                <attribute name='bsd_depositamount' />
                <attribute name='bsd_vippriority' />
                <attribute name='statuscode' />
                <attribute name='bsd_areavariance' />
                <attribute name='bsd_constructionarea' />
                <attribute name='bsd_netsaleablearea' />
                <attribute name='price' />
                <attribute name='bsd_landvalueofunit' />
                <attribute name='bsd_landvalue' />
                <attribute name='bsd_maintenancefeespercent' />
                <attribute name='bsd_maintenancefees' />
                <attribute name='bsd_taxpercent' />
                <attribute name='bsd_vat' />
                <attribute name='bsd_totalprice' />
                <attribute name='bsd_estimatehandoverdate' />
                <attribute name='bsd_numberofmonthspaidmf' />
                <attribute name='bsd_managementamountmonth' />
                <attribute name='bsd_handovercondition' />
                <filter type='and'>
                  <condition attribute='productid' operator='eq' uitype='product' value='" + Id.ToString() + @"' />
                </filter>
                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectcode' visible='false' link-type='outer' alias='a_a77d98e66ce2e811a94e000d3a1bc2d1'>
                  <attribute name='bsd_name' alias='bsd_project_name' />
                </link-entity>
                <link-entity name='bsd_floor' from='bsd_floorid' to='bsd_floor' visible='false' link-type='outer' alias='a_4d73a1e06ce2e811a94e000d3a1bc2d1'>
                  <attribute name='bsd_name' alias='bsd_floor_name' />
                </link-entity>
                <link-entity name='bsd_block' from='bsd_blockid' to='bsd_blocknumber' visible='false' link-type='outer' alias='a_290ca3da6ce2e811a94e000d3a1bc2d1'>
                  <attribute name='bsd_name' alias='bsd_block_name' />
                </link-entity>
                <link-entity name='bsd_unittype' from='bsd_unittypeid' to='bsd_unittype' visible='false' link-type='outer' alias='a_493690ec6ce2e811a94e000d3a1bc2d1'>
                  <attribute name='bsd_name'  alias='bsd_unittype_name'/>
                </link-entity>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<UnitInfoModel>>("products", fetchXml);
            if (result.value.Count != 0)
            {
                viewModel.UnitInfo = result.value.FirstOrDefault();
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UnitImageGallery("Units", Id.ToString(), viewModel.UnitInfo.name, "Hình Ảnh Căn Hộ"));
        }

        private void Button_Clicked_Video(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UnitVideoGallery("Units",Id.ToString(),viewModel.UnitInfo.name,"Phim Căn Hộ"));
        }
    }
}