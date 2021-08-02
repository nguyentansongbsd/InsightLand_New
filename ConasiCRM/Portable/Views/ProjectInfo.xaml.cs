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
    public partial class ProjectInfo : ContentPage
    {
        public ProjectInfoViewModel viewModel;
        private Guid ProjectId;
        public Action<bool> OnCompleted;
        public ProjectInfo(Guid Id)
        {
            InitializeComponent();
            labeDuAnNghienCu.Text = "Dự án nghiên cứu (R&D)";
            ProjectId = Id;
            this.BindingContext = viewModel = new ProjectInfoViewModel();
            viewModel.IsBusy = true;         
            LoadData();
        }
        
        public async Task LoadData()
        {
            string FetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='bsd_project'>
                <attribute name='bsd_projectid' />
                <attribute name='bsd_name' />
                <attribute name='bsd_loaiduan' />
                <attribute name='createdon' />
                <attribute name='bsd_projectcode' />
                <attribute name='bsd_address' />
                <attribute name='bsd_addressen' />
                <attribute name='bsd_depositpercentda' />
                <attribute name='bsd_esttopdate' />
                <attribute name='bsd_estimatehandoverdate' />
                <attribute name='bsd_landvalueofproject' />
                <attribute name='bsd_maintenancefeespercent' />
                <attribute name='bsd_numberofmonthspaidmf' />
                <attribute name='bsd_managementamount' />
                <attribute name='bsd_bookingfee' />
                <attribute name='bsd_depositamount' />
                <attribute name='bsd_description' />
                <filter type='and'>
                  <condition attribute='bsd_projectid' operator='eq' uitype='bsd_project' value='" + ProjectId.ToString() + @"' />
                </filter>
                <link-entity name='account' from='accountid' to='bsd_investor' visible='false' link-type='outer' alias='a_8924f6d5b214e911a97f000d3aa04914'>
                  <attribute name='bsd_name' alias='bsd_investor_name' />
                </link-entity>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ProjectInfoModel>>("bsd_projects", FetchXml);
            if (result == null || result.value.Any() == false)
            {
                await DisplayAlert("Thông báo", "Không tìm thấy dự án.", "Đóng");
                return;
            }

            var project = result.value.FirstOrDefault();
            viewModel.Project = project;

            if (viewModel.Project != null)
                OnCompleted?.Invoke(true);
            else
                OnCompleted?.Invoke(false);


            var tasks = new Task[2]
               {
                    LoadDuAnCanhTranh(),
                    LoadDoiThuCanhTrang()
               };
            await Task.WhenAll(tasks);
            viewModel.IsBusy = false;
        }
        public async Task LoadDuAnCanhTranh()
        {
            viewModel.IsBusy = true;
            await viewModel.LoadDuAnCanhTranh(ProjectId);           
            viewModel.IsBusy = false;
        }
        public async Task LoadDoiThuCanhTrang()
        {
            viewModel.IsBusy = true;
            await viewModel.LoadDoiThuCanhTrang(ProjectId);          
            viewModel.IsBusy = false;
        }

        private async void ShowMoreDuAnCanhTranh_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.PageDuAnCanhTranh++;
            await viewModel.LoadDuAnCanhTranh(ProjectId);
            viewModel.IsBusy = false;
        }

        private async void ShowMoreDoiThuCanhTranh_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.PageDoiThuCanhTranh++;
            await viewModel.LoadDoiThuCanhTrang(ProjectId);
            viewModel.IsBusy = false;
        }
    }
}