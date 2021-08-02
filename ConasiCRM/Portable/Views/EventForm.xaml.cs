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
    public partial class EventForm : ContentPage
    {
        public Action<bool> CheckEventData;
        private Guid eventIDForm;
        public EventFormViewModel viewModel;
        public EventForm(Guid eventID)
        {
            InitializeComponent();
            BindingContext = viewModel = new EventFormViewModel();
            this.eventIDForm = eventID;
            viewModel.IsBusy = true; // khi load vào form chạy loading
            Init();
        }

        public async void Init()
        {
            await loadData();
            if(viewModel.Event != null)
                CheckEventData(true);
            else
                CheckEventData(false);
        }

        public async Task loadData()
        {
            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='bsd_event'>
                              <attribute name='bsd_name' />
                              <attribute name='createdon' />
                              <attribute name='statuscode' />
                              <attribute name='bsd_startdate' />
                              <attribute name='bsd_project' />
                              <attribute name='bsd_eventcode' />
                              <attribute name='bsd_enddate' />
                              <attribute name='bsd_description' />
                              <attribute name='bsd_phaselaunch' />
                              <attribute name='bsd_eventid' />
                              <order attribute='createdon' descending='true' />
                              <filter type='and'>
                                <condition attribute='bsd_eventid' operator='eq' uitype='bsd_event' value='"+ eventIDForm + @"' />
                              </filter>
                              <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='project'>
                                <attribute name='bsd_name' alias='bsd_project_name'/>
                              </link-entity>
                              <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaselaunch' link-type='inner' alias='phaselaunch'>
                                <attribute name='bsd_name' alias ='phaselaunch_bsd_name' />
                                <attribute name='bsd_pricelistid' />
                                <attribute name='statuscode' alias ='phaselaunch_statuscode' />
                                <attribute name='bsd_startdate' alias ='phaselaunch_startdate' />
                                <attribute name='bsd_projectid' />
                                <attribute name='bsd_enddate' alias ='phaselaunch_enddate' />
                                <link-entity name='pricelevel' from='pricelevelid' to='bsd_pricelistid' visible='false' link-type='outer' alias='pricelevel'>
                                  <attribute name='name' alias='pricelevel_name' />
                                </link-entity>
                              </link-entity>
                            </entity>
                          </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<EventFormModel>>("bsd_events", xml);
            var eventData = result.value.FirstOrDefault();
            if(eventData == null)
            {               
                await DisplayAlert("Thông báo", "Thông tin chi tiết của sự kiện không tìm thấy !", "Đóng");
                return;
            }          
            viewModel.Event = eventData;
            viewModel.IsBusy = false;
        }
    }
    
}