using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MeetingForm : ContentPage
	{
        private bool isInit = true;
        public Action<bool> CheckMeeting;
        private Guid _idActivity;
        public MeetingViewModel viewModel;
        private List<StackLayout> _dataRequired;
        public List<StackLayout> dataRequired
        {
            get => _dataRequired;
            set
            {
                if (_dataRequired != value)
                {
                    _dataRequired = value;
                    OnPropertyChanged(nameof(dataRequired));
                }
            }
        }

        private List<StackLayout> _dataOption;
        public List<StackLayout> dataOption
        {
            get => _dataOption;
            set
            {
                if (_dataOption != value)
                {
                    _dataOption = value;
                    OnPropertyChanged(nameof(dataOption));
                }

            }
        }
        public MeetingForm()
        {
            InitializeComponent();
            BindingContext = viewModel = new MeetingViewModel();
            viewModel.Title = "Tạo Mới Cuộc Họp";
            isInit = false;
            viewModel.IsBusy = false;
            grid_createMeeting.IsVisible = true;
            grid_updateMeeting.IsVisible = false;
            dataRequired = new List<StackLayout>();
            dataOption = new List<StackLayout>();
            MeetingModel meetingModel = new MeetingModel();
            meetingModel.editable = true;
            meetingModel.scheduledstart = new DateTime(DateTime.Now.ToLocalTime().Year, DateTime.Now.ToLocalTime().Month, DateTime.Now.ToLocalTime().Day, DateTime.Now.ToLocalTime().Hour, DateTime.Now.ToLocalTime().Minute, 0);
            meetingModel.timeStart = new TimeSpan(DateTime.Now.ToLocalTime().Hour, DateTime.Now.ToLocalTime().Minute, 0);
            meetingModel.durationValue = viewModel.setSelectedTime(30 + "");
            viewModel.MeetingModel = meetingModel;
        }

        public MeetingForm(DateTime dateTimeNew)
        {
            InitializeComponent();
            BindingContext = viewModel = new MeetingViewModel();
            viewModel.Title = "Tạo Mới Cuộc Họp";
            isInit = false;
            viewModel.IsBusy = false;
            grid_createMeeting.IsVisible = true;
            grid_updateMeeting.IsVisible = false;
            dataRequired = new List<StackLayout>();
            dataOption = new List<StackLayout>();
            MeetingModel meetingModel = new MeetingModel();
            meetingModel.editable = true;
            meetingModel.scheduledstart = new DateTime(dateTimeNew.ToLocalTime().Year, dateTimeNew.ToLocalTime().Month, dateTimeNew.ToLocalTime().Day, dateTimeNew.ToLocalTime().Hour, dateTimeNew.Minute, 0);
            meetingModel.timeStart = new TimeSpan(dateTimeNew.Hour, dateTimeNew.ToLocalTime().Minute, 0);
            meetingModel.durationValue = viewModel.setSelectedTime("30");
            viewModel.MeetingModel = meetingModel;
        }

        public MeetingForm (Guid idActivity)
		{
			InitializeComponent ();
            BindingContext = viewModel = new MeetingViewModel();
            this._idActivity = idActivity;
            viewModel.IsBusy = true;
            Init();
        }
        // chuyển grid sang list view - đã test update - chưa test complete, cancel

        public async void Init()
        {
            await loadDataForm(this._idActivity);
            if (viewModel.MeetingModel != null)
            {
                CheckMeeting?.Invoke(true);
                isInit = false;
            }
            else
                CheckMeeting?.Invoke(false);
        }

        public async Task loadDataForm(Guid id)
        {
            dataRequired = new List<StackLayout>();
            dataOption = new List<StackLayout>();
            viewModel.listOptional = new ObservableCollection<PartyModel>();
            viewModel.listRequired = new ObservableCollection<PartyModel>();
            grid_createMeeting.IsVisible = false;
            grid_updateMeeting.IsVisible = true;

            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
            <entity name='appointment'>
                <attribute name='subject' />
                <attribute name='statecode' />
                <attribute name='createdby' />
                <attribute name='regardingobjectid' />
                <attribute name='instancetypecode' />
                <attribute name='statuscode' />
                <attribute name='requiredattendees' />
                <attribute name='prioritycode' />
                <attribute name='scheduledstart' />
                <attribute name='scheduledend' />
                <attribute name='scheduleddurationminutes' />
                <attribute name='bsd_mmeetingformuploaded' />
                <attribute name='optionalattendees' />
                <attribute name='isalldayevent' />
                <attribute name='ownerid' />
                <attribute name='location' />
                <attribute name='createdon' />
                <attribute name='activityid' />
                <attribute name='description' />
                <order attribute='createdon' descending='true' />
                <filter type='and'>
                    <condition attribute='activityid' operator='eq' uitype='appointment' value='" + id+ @"' />
                </filter>               
                <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer' alias='contacts'>
                  <attribute name='contactid' alias='contact_id' />                  
                  <attribute name='fullname' alias='contact_name'/>
                </link-entity>
                <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='outer' alias='accounts'>
                    <attribute name='accountid' alias='account_id' />                  
                    <attribute name='bsd_name' alias='account_name'/>
                </link-entity>
                <link-entity name='lead' from='leadid' to='regardingobjectid' visible='false' link-type='outer' alias='leads'>
                    <attribute name='leadid' alias='lead_id'/>                  
                    <attribute name='fullname' alias='lead_name'/>
                </link-entity>
                <link-entity name='bsd_systemsetup' from='bsd_systemsetupid' to='regardingobjectid' visible='false' link-type='outer' alias='users'>
                    <attribute name='bsd_name' alias='user_id'/>
                    <attribute name='bsd_systemsetupid' alias='user_name'/>
                </link-entity>
            </entity>
          </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<MeetingModel>>("appointments", xml);
            MeetingModel meeting = result.value.FirstOrDefault();

            MeetingModel meetingModel = new MeetingModel();
            meetingModel.activityid = meeting.activityid;
            meetingModel.subject = meeting.subject;
            meetingModel.statecode = meeting.statecode;
            meetingModel.statuscode = meeting.statuscode;
            //check statecode
            if (meetingModel.statecode == 1) // completed
            {
                meetingModel.editable = false;
                grid_updateMeeting.ColumnDefinitions[0].Width = new GridLength(0);
                grid_updateMeeting.ColumnDefinitions[1].Width = new GridLength(0);
            }
            else if (meetingModel.statecode == 2) // canceled
            {
                meetingModel.editable = false; // open
                grid_updateMeeting.IsVisible = false;
            }
            else
            {
                meetingModel.editable = true;
            }
            meetingModel.scheduledstart = meeting.scheduledstart.Value.ToLocalTime();
            meetingModel.timeStart = meeting.scheduledstart.Value.ToLocalTime().TimeOfDay;
            meetingModel.scheduledend = meeting.scheduledend.Value.ToLocalTime();
            meetingModel.timeEnd = meeting.scheduledend.Value.ToLocalTime().TimeOfDay;
            meetingModel.description = meeting.description;
            meetingModel.location = meeting.location;
            meetingModel.isalldayevent = meeting.isalldayevent;
            meetingModel.scheduleddurationminutes = meeting.scheduleddurationminutes;

            meetingModel.durationValue = viewModel.setSelectedTime(meeting.scheduleddurationminutes.ToString());
            if (meeting.contact_id != Guid.Empty)
            {
                meetingModel.Customer = new CustomerLookUp()
                {
                    Id = meeting.contact_id,
                    Name = meeting.contact_name,
                    Type = 1
                };
            }
            else if (meeting.account_id != Guid.Empty)
            {
                meetingModel.Customer = new CustomerLookUp()
                {
                    Id = meeting.account_id,
                    Name = meeting.account_name,
                    Type = 2
                };
            }
            else if (meeting.lead_id != Guid.Empty)
            {
                meetingModel.Customer = new CustomerLookUp()
                {
                    Id = meeting.lead_id,
                    Name = meeting.lead_name,
                    Type = 3
                };
            }
            else if (meeting.user_id != Guid.Empty)
            {
                meetingModel.Customer = new CustomerLookUp()
                {
                    Id = meeting.user_id,
                    Name = meeting.user_name
                };
            }
            meetingModel.createdon = meeting.createdon;
            viewModel.MeetingModel = meetingModel;
       
            // get date from fetch
            string xml_fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
            <entity name='appointment'>
                <attribute name='subject' />
                <attribute name='createdon' />
                <attribute name='activityid' />
                <order attribute='createdon' descending='false' />
                <filter type='and'>
                    <condition attribute='activityid' operator='eq' value='" + id + @"' />
                </filter>
                <link-entity name='activityparty' from='activityid' to='activityid' link-type='inner' alias='ab'>
                    <attribute name='partyid' alias='partyID'/>
                    <attribute name='participationtypemask' alias='typemask'/>
                  <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='partyaccount'>
                    <attribute name='bsd_name' alias='account_name'/>
                  </link-entity>
                  <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='partycontact'>
                    <attribute name='fullname' alias='contact_name'/>
                  </link-entity>
                  <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='partylead'>
                    <attribute name='fullname' alias='lead_name'/>
                  </link-entity>
                  <link-entity name='systemuser' from='systemuserid' to='partyid' link-type='outer' alias='partyuser'>
                    <attribute name='fullname' alias='user_name'/>
                  </link-entity>
                </link-entity>
            </entity>
          </fetch>";
            var _result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PartyModel>>("appointments", xml_fetch);
            var data = _result.value;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    if (item.typemask == 5) // from call
                    {
                        if (item.contact_name != null)
                        {
                            item.Customer = new CustomerLookUp()
                            {
                                Id = item.partyID,
                                Name = item.contact_name,
                                Type = 1
                            };
                        }
                        else if (item.account_name != null)
                        {
                            item.Customer = new CustomerLookUp()
                            {
                                Id = item.partyID,
                                Name = item.account_name,
                                Type = 2
                            };
                        }
                        else if (item.lead_name != null)
                        {
                            item.Customer = new CustomerLookUp()
                            {
                                Id = item.partyID,
                                Name = item.lead_name,
                                Type = 3
                            };
                        }
                        else
                        {
                            item.Customer = new CustomerLookUp()
                            {
                                Id = item.partyID,
                                Name = item.user_name,
                                Type = 4
                            };
                        }
                        viewModel.listRequired.Add(item);
                        dataRequired.Add(this.renderStack(item.Customer.Name, "Required"));
                    }
                    else if (item.typemask == 6)
                    {
                        if (item.contact_name != null)
                        {
                            item.Customer = new CustomerLookUp()
                            {
                                Id = item.partyID,
                                Name = item.contact_name,
                                Type = 1
                            };
                        }
                        else if (item.account_name != null)
                        {
                            item.Customer = new CustomerLookUp()
                            {
                                Id = item.partyID,
                                Name = item.account_name,
                                Type = 2
                            };
                        }
                        else if (item.lead_name != null)
                        {
                            item.Customer = new CustomerLookUp()
                            {
                                Id = item.partyID,
                                Name = item.lead_name,
                                Type = 3
                            };
                        }
                        else
                        {
                            item.Customer = new CustomerLookUp()
                            {
                                Id = item.partyID,
                                Name = item.user_name,
                                Type = 4
                            };
                        }
                         viewModel.listOptional.Add(item);
                        dataOption.Add(this.renderStack(item.Customer.Name, "Optional"));
                    }
                    required.renderStackLayout(dataRequired);
                    optional.renderStackLayout(dataOption);
                }
            }

            //if (dataRequired != null)
            //{
            //    required.Data = dataRequired;
            //}
            //else
            //{
            //    required.Data = new List<StackLayout>();
            //}

            //if (dataOption != null)
            //{
            //    optional.Data = dataOption;
            //}
            //else
            //{
            //    optional.Data = new List<StackLayout>();
            //}
            viewModel.Title = "Thông Tin Cuộc Họp";
            viewModel.IsBusy = false;
        }

        private StackLayout renderStack(string item, string check)
        {
            StackLayout stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Color.FromHex("#F3F3F3"),
                Padding = 2,
                Spacing = 6
            };
            var label = new Label()
            {
                Text = item,
                FontSize = 16,
                BackgroundColor = Color.FromHex("#F3F3F3"),
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Center

            };

            var clearButton = new Label
            {
                HeightRequest = 20,
                WidthRequest = 20,
                Text = "\uf057",
                FontFamily = "FontAwesomeSolid",
                FontSize = 16,
                VerticalTextAlignment = TextAlignment.Center
            };

            if (check == "Required")
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += CleanClickRequired;
                clearButton.GestureRecognizers.Add(tapGestureRecognizer);
             //   imageButton.Clicked += CleanClickRequired;
            }
            else if (check == "Optional")
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += CleanClickOption;
                clearButton.GestureRecognizers.Add(tapGestureRecognizer);
                //imageButton.Clicked += CleanClickOption;
            }
            stackLayout.Children.Add(label);
            stackLayout.Children.Add(clearButton);
            return stackLayout;
        }

        private void CleanClickRequired(object sender, EventArgs e)
        {
            var button = sender as Label;
            int index = dataRequired.IndexOf(dataRequired.FirstOrDefault(x => x.Children.Last() == button));
            this.deleteItem(index, "Required");

        }

        private void CleanClickOption(object sender, EventArgs e)
        {
            var button = sender as Label;
            int index = dataOption.IndexOf(dataOption.FirstOrDefault(x => x.Children.Last() == button));
            this.deleteItem(index, "Optional");

        }
        private void deleteItem(int index, string check)
        {
            if (check == "Required")
            {
                StackLayout item = dataRequired[index];
              //  viewModel.listRequired.FirstOrDefault()
                viewModel.listRequired.Remove(viewModel.listRequired[index]);
                dataRequired.Remove(item);               
                required.renderStackLayout(dataRequired);
            }
            else if (check == "Optional")
            {
                viewModel.listOptional.Remove(viewModel.listOptional[index]);
                dataOption.Remove(dataOption[index]);
                optional.renderStackLayout(dataOption);

            }
        }
        // Open list (contact, account, lead), update checkEvent = "Regarding"
        private void OpenModel_Clicked(object sender, EventArgs e)
        {
            // load data mặc định show popup
            var btnKHCN = FindByName("btnKHCN") as Button;
            viewModel.ShowLookUpModal = true;
            BtnKHCN_Clicked(btnKHCN, null);
            viewModel.check_open = "Lookup"; // event open lookup
        }
        private void ClearLookup_Clicked(object sender, EventArgs e)
        {
            viewModel.MeetingModel.Customer = null;
        }
        private void btnCloseLookUpModal(object sender, EventArgs e)
        {
            viewModel.ShowLookUpModal = false;
        }
        // event khách hàng cá nhân
        private async void BtnKHCN_Clicked(object sender, EventArgs e)
        {
            // styles button khách hàng cá nhân
            btnKHCN.BackgroundColor = Color.FromHex("#666666");
            btnKHCN.TextColor = Color.FromHex("#ffffff");
            btnKHDN.BackgroundColor = Color.FromHex("#eeeeee");
            btnKHDN.TextColor = Color.FromHex("#333333");
            btnKHTN.BackgroundColor = Color.FromHex("#eeeeee");
            btnKHTN.TextColor = Color.FromHex("#333333");
            // load data
            viewModel.CurrentLookUpConfig = viewModel.ContactLookUpConfig;
            viewModel.LookUpData.Clear();
            viewModel.LookUpLoading = true;
            viewModel.LookUpPage = 1;
            await viewModel.loadData();
            viewModel.LookUpLoading = false;

        }

        // event khách hang doanh nghiêp
        private async void BtnKHDN_Clicked(object sender, EventArgs e)
        {
            // styles button khách hàng doanh nghiệp
            btnKHDN.BackgroundColor = Color.FromHex("#666666");
            btnKHDN.TextColor = Color.FromHex("#ffffff");
            btnKHCN.BackgroundColor = Color.FromHex("#eeeeee");
            btnKHCN.TextColor = Color.FromHex("#333333");
            btnKHTN.BackgroundColor = Color.FromHex("#eeeeee");
            btnKHTN.TextColor = Color.FromHex("#333333");
            //load data theo KHDN
            viewModel.CurrentLookUpConfig = viewModel.AccountLookUpConfig;
            viewModel.LookUpData.Clear();
            viewModel.LookUpLoading = true;
            viewModel.LookUpPage = 1;
            await viewModel.loadData();
            viewModel.LookUpLoading = false;
        }

        // event khách hàng tiềm năng
        private async void BtnKHTN_Clicked(object sender, EventArgs e)
        {
            // styles button khách hàng tiềm năng
            btnKHTN.BackgroundColor = Color.FromHex("#666666");
            btnKHTN.TextColor = Color.FromHex("#ffffff");
            btnKHCN.BackgroundColor = Color.FromHex("#eeeeee");
            btnKHCN.TextColor = Color.FromHex("#333333");
            btnKHDN.BackgroundColor = Color.FromHex("#eeeeee");
            btnKHDN.TextColor = Color.FromHex("#333333");
            //load data theo KHDN
            viewModel.CurrentLookUpConfig = viewModel.LeadLookUpConfig;
            viewModel.LookUpData.Clear();
            viewModel.LookUpLoading = true;
            viewModel.LookUpPage = 1;
            await viewModel.loadData();
            viewModel.LookUpLoading = false;
        }
        // Open list (contact, account, lead), update checkEvent = "Required"
        private void Required_OnClicked(object sender, EventArgs e)
        {
            var btnKHCN = FindByName("btnKHCN") as Button;
            viewModel.ShowLookUpModal = true;
            BtnKHCN_Clicked(btnKHCN, null);
            viewModel.check_open = "Required"; // event open lookup
        }

        // Open list (contact, account, lead), update checkEvent = "Optional"
        private void Optional_OnClicked(object sender, EventArgs e)
        {
            var btnKHCN = FindByName("btnKHCN") as Button;
            viewModel.ShowLookUpModal = true;
            BtnKHCN_Clicked(btnKHCN, null);
            viewModel.check_open = "Optional"; // event open lookup
        }
       
        // option duration
        private void Actualduration_value_SelectedIndexChanged(object sender, EventArgs e)
        {
            int valueDate = int.Parse(viewModel.MeetingModel.durationValue.Val);
            if (viewModel.MeetingModel.scheduledstart == null)
            {
                viewModel.MeetingModel.scheduledstart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                viewModel.MeetingModel.timeStart = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                viewModel.MeetingModel.scheduledend = viewModel.MeetingModel.scheduledstart.Value.AddMinutes(valueDate);
                viewModel.MeetingModel.timeEnd = new TimeSpan(viewModel.MeetingModel.scheduledend.Value.Hour, viewModel.MeetingModel.scheduledend.Value.Minute, 0);
            }
            else
            {
                viewModel.MeetingModel.scheduledend = new DateTime(viewModel.MeetingModel.scheduledstart.Value.Year, viewModel.MeetingModel.scheduledstart.Value.Month, viewModel.MeetingModel.scheduledstart.Value.Day, viewModel.MeetingModel.scheduledstart.Value.Hour, viewModel.MeetingModel.scheduledstart.Value.Minute, 0).AddMinutes(valueDate);
                viewModel.MeetingModel.timeEnd = new TimeSpan(viewModel.MeetingModel.scheduledend.Value.Hour, viewModel.MeetingModel.scheduledend.Value.Minute, 0);
            }
            viewModel.MeetingModel.scheduleddurationminutes = valueDate;
        }

        // option scheduledstart
        private void DatePickerStart_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (viewModel.FocusDateTimeStart)
            {
                DateTime timeNew = e.NewDate;
                TimeSpan _timeStart = viewModel.MeetingModel.timeStart;
                var scheduledstart = new DateTime(timeNew.Year, timeNew.Month, timeNew.Day, _timeStart.Hours, _timeStart.Minutes, _timeStart.Seconds);
                //viewModel.FocusTimePickerStart = true;
                // check thời gian gian kết thúc
                if (this.compareDateTime(viewModel.MeetingModel.scheduledend.Value, viewModel.MeetingModel.scheduledstart.Value) != -1)
                {
                    viewModel.MeetingModel.scheduledstart = scheduledstart;
                    this.totalDuration();                    
                }
                else
                {
                    DisplayAlert("Thông Báo", "Vui lòng chọn thời gian bắt đầu nhỏ hơn thời gian kết thúc!", "Đồng ý");
                    int valueDate = int.Parse(viewModel.MeetingModel.durationValue.Val);
                    var time = viewModel.MeetingModel.scheduledend.Value.AddMinutes(-valueDate);
                    viewModel.MeetingModel.scheduledstart = new DateTime(time.Year, time.Month, time.Day, _timeStart.Hours, _timeStart.Minutes, _timeStart.Seconds);
                    viewModel.MeetingModel.timeStart = new TimeSpan(viewModel.MeetingModel.scheduledstart.Value.Hour, viewModel.MeetingModel.scheduledstart.Value.Minute, 0);
                }    
            }
        }
        // option scheduledend
        private void TimePickerStart_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
            {
                var time = timePickerStart.Time;
                if (viewModel.MeetingModel.scheduledend != null && viewModel.MeetingModel.scheduledstart != null)
                {
                    DateTime timeNew = viewModel.MeetingModel.scheduledstart.Value;
                    timeNew = new DateTime(timeNew.Year, timeNew.Month, timeNew.Day, time.Hours, time.Minutes, time.Seconds);
                    // 
                    if (this.compareDateTime(viewModel.MeetingModel.scheduledend.Value, viewModel.MeetingModel.scheduledstart.Value) != -1)
                    {
                        viewModel.MeetingModel.timeStart = time;
                        viewModel.MeetingModel.scheduledstart = timeNew;
                        this.totalDuration();
                    }
                    else
                    {
                        if (isInit != true)
                        {
                            DisplayAlert("Thông Báo", "Vui lòng chọn thời gian bắt đầu nhỏ hơn thời gian kết thúc!", "Đồng ý");
                            var time1 = viewModel.MeetingModel.scheduledend.Value;
                            int valueDate = int.Parse(viewModel.MeetingModel.durationValue.Val);
                            viewModel.MeetingModel.scheduledstart = viewModel.MeetingModel.scheduledend.Value.AddMinutes(-valueDate);
                            viewModel.MeetingModel.timeStart = new TimeSpan(viewModel.MeetingModel.scheduledstart.Value.Hour, viewModel.MeetingModel.scheduledstart.Value.Minute, 0);
                        }
                    }
                }
                else
                {
                    viewModel.MeetingModel.timeStart = time;
                }

            }
        }

        private void DatePicker_Focused(object sender, FocusEventArgs e)
        {
            viewModel.FocusDateTimeStart = true;
        }

        private void DatePickerEnd_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (viewModel.FocusDateTimeEnd)
            {
                if (viewModel.MeetingModel.scheduledstart != null)
                {
                    DateTime timeNew = e.NewDate;
                    TimeSpan time = viewModel.MeetingModel.timeEnd;
                    DateTime _scheduledend = new DateTime(timeNew.Year, timeNew.Month, timeNew.Day, time.Hours, time.Minutes, time.Seconds);

                    if (this.compareDateTime(viewModel.MeetingModel.scheduledend.Value, viewModel.MeetingModel.scheduledstart.Value) != -1)
                    {
                        viewModel.MeetingModel.scheduledend = _scheduledend;
                        //viewModel.FocusTimePickerEnd = true;
                        this.totalDuration();
                    }
                    else
                    {
                        DisplayAlert("Thông Báo", "Vui lòng chọn thời gian kết thúc lớn hơn thời gian bắt đầu!", "Đồng ý");
                        int valueDate = int.Parse(viewModel.MeetingModel.durationValue.Val);
                        viewModel.MeetingModel.scheduledend = viewModel.MeetingModel.scheduledstart.Value.AddMinutes(valueDate);
                        viewModel.MeetingModel.timeEnd = new TimeSpan(viewModel.MeetingModel.scheduledend.Value.Hour, viewModel.MeetingModel.scheduledend.Value.Minute, 0);
                    }
                }
                else
                {
                    DisplayAlert("Thông Báo", "Vui lòng chọn thời gian bắt đầu!", "Đồng ý");
                }

            }
        }

        private void DatePickerEnd_Focused(object sender, FocusEventArgs e)
        {
            viewModel.FocusDateTimeEnd = true;
        }

        private void TimePickerEnd_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
            {
                var time = timePickerEnd.Time;
                if (viewModel.MeetingModel.scheduledend != null)
                {
                    DateTime timeNew = viewModel.MeetingModel.scheduledend.Value;
                    timeNew = new DateTime(timeNew.Year, timeNew.Month, timeNew.Day, time.Hours, time.Minutes, time.Seconds);
                    if (this.compareDateTime(viewModel.MeetingModel.scheduledend.Value, viewModel.MeetingModel.scheduledstart.Value) != -1)
                    {
                        viewModel.MeetingModel.timeEnd = time;
                        viewModel.MeetingModel.scheduledend = timeNew;
                        this.totalDuration();
                    }
                    else
                    {
                        if (isInit != true)
                        {
                            DisplayAlert("Thông Báo", "Vui lòng chọn thời gian kết thúc lớn hơn thời gian bắt đầu!", "Đồng ý");
                            int valueDate = int.Parse(viewModel.MeetingModel.durationValue.Val);
                            viewModel.MeetingModel.scheduledend = viewModel.MeetingModel.scheduledstart.Value.AddMinutes(valueDate);
                            viewModel.MeetingModel.timeEnd = new TimeSpan(viewModel.MeetingModel.scheduledend.Value.Hour, viewModel.MeetingModel.scheduledend.Value.Minute, 0);
                        }
                    }
                }
                else
                {
                    viewModel.MeetingModel.timeEnd = time;
                }
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
            if (date == null && date1 !=null)
                return -1;
            if (date1 == null && date != null)
                return 1;
            return 0;
        }
        private void totalDuration()
        {
            if (viewModel.MeetingModel.scheduledstart != null && viewModel.MeetingModel.scheduledend != null)
            {
                TimeSpan difference = viewModel.MeetingModel.scheduledend.Value - viewModel.MeetingModel.scheduledstart.Value;
                double _minutes = Math.Round(difference.TotalMinutes);

                if (_minutes < 60)
                {
                    this.setDuration(_minutes, _minutes.ToString() + " phút");
                }
                else if (_minutes >= 60 && _minutes < 1440)
                {
                    double _hours = difference.TotalHours;
                    this.setDuration(_minutes, Math.Round(difference.TotalHours, 2).ToString() + " giờ");
                }
                else if (_minutes >= 1440)
                {
                    double _day = difference.TotalDays;
                    this.setDuration(_minutes, Math.Round(difference.TotalDays, 2).ToString() + " ngày");
                }
            }
        }

        // param: _minutes: (thời gian kết thúc - thời gian bắt đầu) theo số phút
        //        labelOption: hiển thị theo cấu trúc (< 60 : phút, >= 60 giờ, >= 1440 ngày)          
        private void setDuration(double _minutes, string labelOption)
        {
            bool exist = false;
            foreach (var item in viewModel.list_picker_durations)
            {
                if (item.Val == _minutes.ToString())
                {
                    exist = true;
                    break;
                }
            }
            if (exist)
            {
                viewModel.MeetingModel.durationValue = viewModel.setSelectedTime(_minutes.ToString());
            }
            else
            {
                viewModel.list_picker_durations.Add(new OptionSet() { Val = _minutes.ToString(), Label = labelOption });
                viewModel.MeetingModel.durationValue = viewModel.setSelectedTime(_minutes.ToString());
            }
        }

        // option one item in list customer (contact, account, lead )
        // Update data by selection (lookup, required, optional);
        private void LvLookUp_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as LookUp;
            var model = viewModel.MeetingModel;
            if (viewModel.check_open == "Lookup")
            {
                PropertyInfo prop = model.GetType().GetProperty(viewModel.CurrentLookUpConfig.PropertyName); // 
                prop.SetValue(model, item);
                viewModel.ShowLookUpModal = false; // được get mặc định từ viewModel kế thừa từ FormViewModel
            }
            else if (viewModel.check_open == "Required")
            {
                var customer = new CustomerLookUp() { Name = item.Name, Id = item.Id };
                PropertyInfo prop = model.GetType().GetProperty(viewModel.CurrentLookUpConfig.PropertyName); // 
                if (prop.Name == "Contact")
                {
                    customer.Type = 1;
                }
                else if (prop.Name == "Account")
                {
                    customer.Type = 2;
                }
                else if (prop.Name == "Lead")
                {
                    customer.Type = 3;
                }

                if (!this.check_Data(viewModel.listRequired, customer.Id))
                {
                    viewModel.listRequired.Add(new PartyModel() { typemask = 5, Customer = customer });
                    dataRequired.Add(this.renderStack(item.Name, viewModel.check_open));
                    required.renderStackLayout(dataRequired);
                }
                viewModel.ShowLookUpModal = false;
            }
            else if (viewModel.check_open == "Optional")
            {
                var customer = new CustomerLookUp() { Name = item.Name, Id = item.Id };
                PropertyInfo prop = model.GetType().GetProperty(viewModel.CurrentLookUpConfig.PropertyName); // 
                if (prop.Name == "Contact")
                {
                    customer.Type = 1;
                }
                else if (prop.Name == "Account")
                {
                    customer.Type = 2;
                }
                else if (prop.Name == "Lead")
                {
                    customer.Type = 3;
                }

                if (!this.check_Data(viewModel.listOptional, customer.Id))
                {
                    viewModel.listOptional.Add(new PartyModel() { typemask = 6, Customer = customer });
                    dataOption.Add(this.renderStack(item.Name, viewModel.check_open));
                    optional.renderStackLayout(dataOption);
                }

                viewModel.ShowLookUpModal = false;
            }
            
        }

        // check exits one item in listParty
        // exits: true ; else: false 
        private bool check_Data(ObservableCollection<PartyModel> listParty, Guid id)
        {
            var check = false;
            if (listParty != null && listParty.Count > 0)
            {
                foreach (var item in listParty)
                {
                    if (id == item.Customer.Id)
                    {
                        return check = true;
                    }
                }
            }
            return check;
        }

        // load more data
        private async void LvLookUp_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            viewModel.LookUpLoading = true;
            var itemAppearing = e.Item as Portable.Models.LookUp;
            var lastItem = viewModel.LookUpData.LastOrDefault();
            if (lastItem != null && itemAppearing.Id.Equals(lastItem.Id))
            {
                viewModel.LookUpPage += 1;
                await viewModel.loadData();
            }
            viewModel.LookUpLoading = false;
        }

        //onchange checkbox, set value durationValue
        private void Check_event_changeChecked(object sender, EventArgs e)
        {
            var durationValue = viewModel.MeetingModel.durationValue;
            var value = viewModel.MeetingModel.isalldayevent;
            if (value)
            {
                viewModel.MeetingModel.durationValue = viewModel.setSelectedTime("1440");
            }
            else
            {
                viewModel.MeetingModel.durationValue = viewModel.setSelectedTime("30");
            }
        }

        private async void saveMeeting()
        {
            LoadingHelper.Show();
            var check = await checkData(); // kiểm tra dư liệu
            // check create or update
            if (check)
            {
                if (viewModel.MeetingModel.activityid == Guid.Empty) // create
                {
                    var created = await createMeeting(viewModel, "create");
                    if (created != new Guid())
                    {
                        await this.loadDataForm(created);
                        LoadingHelper.Hide();
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "Tạo cuộc họp thành công", "OK");
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "Tạo cuộc họp thất bại", "OK");
                    }
                }
                else // update
                {
                    var update = await updateMeeting(viewModel, "update");
                    if (update)
                    {
                        await this.loadDataForm(viewModel.MeetingModel.activityid);
                        LoadingHelper.Hide();
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Thông báo", "Cập nhật thành công!", "OK");
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Thông báo", "Cập nhật thất bại!", "OK");
                    }
                }
            }
            else
            {
                LoadingHelper.Hide();
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Thông báo", "Vui lòng nhập các thông tin bắt buộc!", "OK");
            }
        }
        public async Task<Boolean> DeletLookup(string fieldName, Guid id)
        {
            var result = await CrmHelper.SetNullLookupField("appointments", id, fieldName);
            return result.IsSuccess;
        }

        // Check the required information (scheduledstart, scheduledend, )
        private async Task<bool> checkData()
        {
            var dataMeeting = viewModel.MeetingModel;
            if (dataMeeting != null)
            {
                var checkTime = compareDateTime(dataMeeting.scheduledend, dataMeeting.scheduledstart);
                if (dataMeeting.subject != null && checkTime == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        private async Task<object> getContent(MeetingViewModel meeting, string value)
        {
            var dataMeeting = meeting.MeetingModel;
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["activityid"] = dataMeeting.activityid.ToString();
            data["subject"] = dataMeeting.subject;
            data["isalldayevent"] = dataMeeting.isalldayevent;
            data["location"] = dataMeeting.location;
            data["description"] = dataMeeting.description ?? "";
            data["scheduledstart"] = dataMeeting.scheduledstart.Value.ToUniversalTime();
            data["scheduledend"] = dataMeeting.scheduledend.Value.ToUniversalTime();
            data["actualdurationminutes"] = dataMeeting.scheduleddurationminutes;
            if (value == "update")
            {
                data["statecode"] = dataMeeting.statecode;
                data["statuscode"] = dataMeeting.statuscode;
            }
            if (dataMeeting.Customer != null)
            {
                if (dataMeeting.Customer.Type == 1)
                {
                    data["regardingobjectid_contact_appointment@odata.bind"] = "/contacts(" + dataMeeting.Customer.Id.ToString() + ")";
                }
                else if (dataMeeting.Customer.Type == 2)
                {
                    data["regardingobjectid_account_appointment@odata.bind"] = "/accounts(" + dataMeeting.Customer.Id.ToString() + ")";
                }
                else if (dataMeeting.Customer.Type == 3)
                {
                    data["regardingobjectid_lead_appointment@odata.bind"] = "/leads(" + dataMeeting.Customer.Id.ToString() + ")";
                }
            }
            else
            {
                await DeletLookup("regardingobjectid_contact_appointment", dataMeeting.activityid);
                await DeletLookup("regardingobjectid_account_appointment", dataMeeting.activityid);
                await DeletLookup("regardingobjectid_lead_appointment", dataMeeting.activityid);
            }
            
            var dataRequired = meeting.listRequired;
            var dataOptional = meeting.listOptional;
            //check data FromCell and ToCell          
            List<object> arrayMeeting = new List<object>();

            // FromCell
            foreach (var item in dataRequired) // 2 
            {
                IDictionary<string, object> item_from = new Dictionary<string, object>();
                if (item.Customer.Type == 1) // contact
                {
                    item_from["partyid_contact@odata.bind"] = "/contacts(" + item.Customer.Id + ")";
                    item_from["participationtypemask"] = 5;
                }
                else if (item.Customer.Type == 2) // account
                {
                    item_from["partyid_account@odata.bind"] = "/accounts(" + item.Customer.Id + ")";
                    item_from["participationtypemask"] = 5;
                }
                else if (item.Customer.Type == 3) // lead
                {
                    item_from["partyid_lead@odata.bind"] = "/leads(" + item.Customer.Id + ")";
                    item_from["participationtypemask"] = 5;
                }
                else
                {
                    item_from["partyid_systemuser@odata.bind"] = "/systemusers(" + item.Customer.Id + ")";
                    item_from["participationtypemask"] = 5;
                }
                arrayMeeting.Add(item_from);
            }

            //ToCell
            foreach (var itemTo in dataOptional)
            {
                IDictionary<string, object> item_to = new Dictionary<string, object>();
                if (itemTo.Customer.Type == 1) // contact
                {
                    item_to["partyid_contact@odata.bind"] = "/contacts(" + itemTo.Customer.Id + ")";
                    item_to["participationtypemask"] = 6;
                }
                else if (itemTo.Customer.Type == 2) // account
                {
                    item_to["partyid_account@odata.bind"] = "/accounts(" + itemTo.Customer.Id + ")";
                    item_to["participationtypemask"] = 6;
                }
                else if (itemTo.Customer.Type == 3) // lead
                {
                    item_to["partyid_lead@odata.bind"] = "/leads(" + itemTo.Customer.Id + ")";
                    item_to["participationtypemask"] = 6;
                }
                else
                {
                    item_to["partyid_systemuser@odata.bind"] = "/systemusers(" + itemTo.Customer.Id + ")";
                    item_to["participationtypemask"] = 6;
                }
                arrayMeeting.Add(item_to);
            }
            data["appointment_activity_parties"] = arrayMeeting;
            return data;
        }

        public async Task<Guid> createMeeting(MeetingViewModel meeting, string value)
        {
            meeting.MeetingModel.activityid = Guid.NewGuid();
            string path = "/appointments";
            var content = await this.getContent(meeting,value);

            CrmApiResponse result = await CrmHelper.PostData(path, content);

            if (result.IsSuccess)
            {
                return meeting.MeetingModel.activityid; // return id meeting New
            }
            else
            {
                var mess = result.ErrorResponse?.error?.message ?? "Đã xảy ra lỗi. Vui lòng thử lại.";
                await App.Current.MainPage.DisplayAlert("", mess, "OK");
                return new Guid();
            }
        }
        public async Task<Boolean> updateMeeting(MeetingViewModel meeting, string value)
        {
            string path = "/appointments(" + meeting.MeetingModel.activityid + ")";
            var content = await this.getContent(meeting, value);
            CrmApiResponse result = await CrmHelper.PatchData(path, content);
            if (result.IsSuccess)
            {
                return true;
            }

            else
            {
                var mess = result.ErrorResponse?.error?.message ?? "Đã xảy ra lỗi. Vui lòng thử lại.";
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", mess, "OK");
                return false;
            }
        }

        // event update data
        private void UpdateMeeting_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            this.saveMeeting();
            viewModel.IsBusy = false;
        }

        private async void CompletedMeeting_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            string action = await DisplayActionSheet("Vui lòng chọn trạng thái cuộc họp", "Đóng", null, "Hoàn Thành", "Hủy");
            if (action == "Hoàn Thành")
            {
                viewModel.MeetingModel.statecode = 1;
                viewModel.MeetingModel.statuscode = 3;
                this.saveMeeting();
            }
            else if (action == "Hủy")
            {
                viewModel.MeetingModel.statecode = 2;
                viewModel.MeetingModel.statuscode = 4;
                this.saveMeeting();
            }
            viewModel.IsBusy = false;
        }

        private async void CanceledMeeting_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            bool check = await DisplayAlert("Thông báo", "Bạn có muốn hủy cuộc họp này không ?", "Đồng Ý", "Không Đồng Ý");
            if (check == true)
            {
                viewModel.MeetingModel.statecode = 2;
                viewModel.MeetingModel.statuscode = 4;
                this.saveMeeting(); ;
            }
            viewModel.IsBusy = false;
        }

        private void CreateNew_Clicked(object sender, EventArgs e)
        {
            this.saveMeeting();
        }
    }
}