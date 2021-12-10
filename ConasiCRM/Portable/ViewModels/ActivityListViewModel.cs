using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class ActivityListViewModel : ListViewBaseViewModel2<HoatDongListModel>
    {
        public string Keyword { get; set; }

        public PhoneCellModel _phoneCall;
        public PhoneCellModel PhoneCall { get => _phoneCall; set { _phoneCall = value; OnPropertyChanged(nameof(PhoneCall)); } }

        public TaskFormModel _task;
        public TaskFormModel Task { get => _task; set { _task = value; OnPropertyChanged(nameof(Task)); } }

        public MeetingModel _meet;
        public MeetingModel Meet { get => _meet; set { _meet = value; OnPropertyChanged(nameof(Meet)); } }

        private StatusCodeModel _activityStatusCode;
        public StatusCodeModel ActivityStatusCode { get => _activityStatusCode; set { _activityStatusCode = value; OnPropertyChanged(nameof(ActivityStatusCode)); } }

        public bool _showGridButton;
        public bool ShowGridButton { get => _showGridButton; set { _showGridButton = value; OnPropertyChanged(nameof(ShowGridButton)); } }

        private string _activityType;
        public string ActivityType { get => _activityType; set { _activityType = value; OnPropertyChanged(nameof(ActivityType)); } }

        private DateTime? _scheduledStartTask;
        public DateTime? ScheduledStartTask { get => _scheduledStartTask; set { _scheduledStartTask = value; OnPropertyChanged(nameof(ScheduledStartTask)); } }

        private DateTime? _scheduledEndTask;
        public DateTime? ScheduledEndTask { get => _scheduledEndTask; set { _scheduledEndTask = value; OnPropertyChanged(nameof(ScheduledEndTask)); } }

        public string CodeCompleted = "completed";

        public string CodeCancel = "cancel";

        public string entity { get; set; }

        public ActivityListViewModel()
        {
            PhoneCall = new PhoneCellModel();
            Task = new TaskFormModel();
            Meet = new MeetingModel();
            PreLoadData = new Command(() =>
            {
                string callto = null;
                if (entity == "phonecall")
                {
                    callto = @"<link-entity name='activityparty' from='activityid' to='activityid' link-type='outer' alias='aee'>
                                        <filter type='and'>
                                            <condition attribute='participationtypemask' operator='eq' value='2' />
                                        </filter>
                                        <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='aff'>
                                            <attribute name='fullname' alias='callto_contact_name'/>
                                        </link-entity>
                                        <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='agg'>
                                            <attribute name='bsd_name' alias='callto_account_name'/>
                                        </link-entity>
                                        <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='ahh'>
                                            <attribute name='fullname' alias='callto_lead_name'/>
                                        </link-entity>
                                    </link-entity>";
                }

                if(entity == "appointment")
                {
                    callto = @"<link-entity name='activityparty' from='activityid' to='activityid' link-type='outer' alias='aee'>
                                        <filter type='and'>
                                            <condition attribute='participationtypemask' operator='eq' value='5' />
                                        </filter>
                                        <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='aff'>
                                            <attribute name='fullname' alias='callto_contact_name'/>
                                        </link-entity>
                                        <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='agg'>
                                            <attribute name='bsd_name' alias='callto_accounts_name'/>
                                        </link-entity>
                                        <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='ahh'>
                                            <attribute name='fullname' alias='callto_lead_name'/>
                                        </link-entity>
                                    </link-entity>";
                }    

                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='true'>
                                  <entity name='{entity}'>
                                    <attribute name='activitytypecode' />
                                    <attribute name='subject' />
                                    <attribute name='statecode' />
                                    <attribute name='activityid' />
                                    <attribute name='scheduledstart' />
                                    <attribute name='scheduledend' />
                                    <order attribute='modifiedon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='subject' operator='like' value='%25{Keyword}%25' />
                                      <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='{UserLogged.Id}' />
                                    </filter>
                                    <link-entity name='account' from='accountid' to='regardingobjectid' link-type='outer' alias='ae'>
                                        <attribute name='bsd_name' alias='accounts_bsd_name'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='regardingobjectid' link-type='outer' alias='af'>
                                        <attribute name='fullname' alias='contact_bsd_fullname'/>
                                    </link-entity>
                                    <link-entity name='lead' from='leadid' to='regardingobjectid' link-type='outer' alias='ag'>
                                        <attribute name='fullname' alias='lead_fullname'/>
                                    </link-entity>
                                    {callto}
                                  </entity>
                                </fetch>";
            });
        }

        public override Task LoadOnRefreshCommandAsync()
        {
            var a = base.LoadOnRefreshCommandAsync();
            if (Data != null && Data.Count > 0)
            {
                List<HoatDongListModel> list = new List<HoatDongListModel>();
                foreach (var item in Data)
                {
                    var meet = list.FirstOrDefault(x => x.activityid == item.activityid);
                    if (meet != null)
                    {
                        if (!string.IsNullOrWhiteSpace(item.callto_contact_name))
                        {
                            string new_customer = ", " + item.callto_contact_name;
                            meet.customer += new_customer;
                        }
                        if (!string.IsNullOrWhiteSpace(item.callto_account_name))
                        {
                            string new_customer = ", " + item.callto_account_name;
                            meet.customer += new_customer;
                        }
                        if (!string.IsNullOrWhiteSpace(item.callto_lead_name))
                        {
                            string new_customer = ", " + item.callto_lead_name;
                            meet.customer += new_customer;
                        }
                    }
                    else
                    {
                        item.scheduledstart = item.scheduledstart.ToLocalTime();
                        item.scheduledend = item.scheduledend.ToLocalTime();
                        if (!string.IsNullOrWhiteSpace(item.callto_contact_name))
                        {
                            item.customer = item.callto_contact_name;
                        }
                        if (!string.IsNullOrWhiteSpace(item.callto_account_name))
                        {
                            item.customer = item.callto_account_name;
                        }
                        if (!string.IsNullOrWhiteSpace(item.callto_lead_name))
                        {
                            item.customer = item.callto_lead_name;
                        }
                        list.Add(item);
                    }
                }
                Data.Clear();
                foreach (var item in list)
                {
                    Data.Add(item);
                }
                //= new Xamarin.Forms.Extended.InfiniteScrollCollection<HoatDongListModel>(list);
            }
            return a;
        }
    }
}
