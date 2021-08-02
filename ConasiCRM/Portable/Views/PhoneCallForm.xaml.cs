using ConasiCRM.Portable.Controls;
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
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("FontAwesome5Solid.otf", Alias = "FontAwesomeSolid")]
namespace ConasiCRM.Portable.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PhoneCallForm : ContentPage
	{
        private bool isInit = true;
        public Action<bool> CheckPhoneCell;
        private Guid _idActivity;
        public PhoneCellViewModel viewModel;
        private List<StackLayout> _dataStackTo;
        public List<StackLayout> dataStackTo
        {
            get => _dataStackTo;
            set
            {
                if(_dataStackTo != value)
                {
                    _dataStackTo = value;
                    OnPropertyChanged(nameof(dataStackTo));
                }
                    
            }
        }

        private List<StackLayout> _dataStackFrom;
        public List<StackLayout> dataStackFrom
        {
            get => _dataStackFrom;
            set
            {
                if (_dataStackFrom != value)
                {
                    _dataStackFrom = value;
                    OnPropertyChanged(nameof(dataStackFrom));
                }

            }
        }

        public PhoneCallForm()
        {
            InitializeComponent();
            BindingContext = viewModel = new PhoneCellViewModel();
            viewModel.Title = "Tạo mới cuộc gọi";
            viewModel.IsBusy = false;
            isInit = false;
            grid_createPhone.IsVisible = true;
            grid_updatePhone.IsVisible = false;
            dataStackTo = new List<StackLayout>();
            dataStackFrom = new List<StackLayout>();
            PhoneCellModel phoneCellModel = new PhoneCellModel();
            phoneCellModel.editable = true;
            phoneCellModel.scheduledstart = new DateTime(DateTime.Now.ToLocalTime().Year, DateTime.Now.ToLocalTime().Month, DateTime.Now.ToLocalTime().Day, DateTime.Now.ToLocalTime().Hour, DateTime.Now.ToLocalTime().Minute, 0);
            phoneCellModel.timeStart = new TimeSpan(DateTime.Now.ToLocalTime().Hour, DateTime.Now.ToLocalTime().Minute, 0);
            phoneCellModel.durationValue = viewModel.setSelectedTime(30 + "");
            viewModel.PhoneCellModel = phoneCellModel;
        }

        public PhoneCallForm(DateTime dateTimeNew)
        {
            InitializeComponent();
            BindingContext = viewModel = new PhoneCellViewModel();
            viewModel.Title = "Tạo mới cuộc gọi";
            isInit = false;
            viewModel.IsBusy = false;
            grid_createPhone.IsVisible = true;
            grid_updatePhone.IsVisible = false;
            dataStackTo = new List<StackLayout>();
            dataStackFrom = new List<StackLayout>();
            PhoneCellModel phoneCellModel = new PhoneCellModel();
            phoneCellModel.editable = true;
            phoneCellModel.scheduledstart = new DateTime(dateTimeNew.Year, dateTimeNew.Month, dateTimeNew.Day, dateTimeNew.Hour, dateTimeNew.Minute, 0);
            phoneCellModel.timeStart = new TimeSpan(dateTimeNew.Hour, dateTimeNew.Minute, 0);
            phoneCellModel.durationValue = viewModel.setSelectedTime("30");
            viewModel.PhoneCellModel = phoneCellModel;
        }

        public PhoneCallForm (Guid idActivity)
		{
			InitializeComponent ();
            BindingContext = viewModel = new PhoneCellViewModel();
            this._idActivity = idActivity;
            viewModel.IsBusy = true;
            Init();
            //fromCell.SetBinding(MyNewEntryPartyList.DataProperty, new Binding("dataStack"));
            //fromCell.BindingContext = this;
        }
        public async void Init()
        {
            await loadDataForm(this._idActivity);
            if (viewModel.PhoneCellModel != null)
            {
                CheckPhoneCell?.Invoke(true);
                isInit = false;
            }
            else
                CheckPhoneCell?.Invoke(false); 
        }
        public async Task loadDataForm(Guid id)
        {
            viewModel.listFromCell.Clear();
            viewModel.listToCell.Clear();
            dataStackTo = new List<StackLayout>();
            dataStackFrom = new List<StackLayout>();
            toCell.Data = dataStackTo;
            fromCell.Data = dataStackFrom;

            grid_createPhone.IsVisible = false;
            grid_updatePhone.IsVisible = true;

            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
            <entity name='phonecall'>
                <attribute name='subject' />
                <attribute name='to' /> 
                <attribute name='from' /> 
                <attribute name='statecode' />
                <attribute name='prioritycode' />
                <attribute name='scheduledstart' />
                <attribute name='scheduledend' />
                <attribute name='createdby' />
                <attribute name='regardingobjectid' />
                <attribute name='actualdurationminutes' />
                <attribute name='description' />
                <attribute name='phonenumber' />
                <attribute name='directioncode' />
                <attribute name='activityid' />
                <order attribute='subject' descending='false' />
                <filter type='and'>
                    <condition attribute='activityid' operator='eq' uitype='phonecall' value='" + id + @"' />
                </filter>
                <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='outer' alias='a_9b4f4019bdc24dd79b1858c2d087a27d'>
                    <attribute name='accountid' alias='account_id' />                  
                    <attribute name='bsd_name' alias='account_name'/>
                </link-entity>
                <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer' alias='a_66b6d0af970a40c9a0f42838936ea5ce'>
                    <attribute name='contactid' alias='contact_id' />                  
                    <attribute name='fullname' alias='contact_name'/>
                </link-entity>
                <link-entity name='lead' from='leadid' to='regardingobjectid' visible='false' link-type='outer' alias='a_fb87dbfd8304e911a98b000d3aa2e890'>
                    <attribute name='leadid' alias='lead_id'/>                  
                    <attribute name='fullname' alias='lead_name'/>
                </link-entity>
            </entity>
          </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhoneCellModel>>("phonecalls", xml);
            PhoneCellModel phoneCell = result.value.FirstOrDefault();            

            PhoneCellModel phoneCellModel = new PhoneCellModel();
            phoneCellModel.activityid = phoneCell.activityid;
            phoneCellModel.subject = phoneCell.subject;
            phoneCellModel.statecode = phoneCell.statecode;
            //check statecode
            if (phoneCell.statecode == 1) // completed
            {
                phoneCellModel.editable = false;
                grid_updatePhone.ColumnDefinitions[0].Width = new GridLength(0);
                grid_updatePhone.ColumnDefinitions[1].Width = new GridLength(0);
            }
            else if (phoneCell.statecode == 2) // canceled
            {
                phoneCellModel.editable = false; // open
                grid_updatePhone.IsVisible = false;
            }
            else
            {
                phoneCellModel.editable = true;
            }
            phoneCellModel.statuscode = phoneCell.statuscode;
            phoneCellModel.scheduledstart = phoneCell.scheduledstart.Value.ToLocalTime();
            phoneCellModel.timeStart = phoneCell.scheduledstart.Value.ToLocalTime().TimeOfDay;
            phoneCellModel.scheduledend = phoneCell.scheduledend.Value.ToLocalTime();
            phoneCellModel.timeEnd = phoneCell.scheduledend.Value.ToLocalTime().TimeOfDay;
            phoneCellModel.description = phoneCell.description;
            phoneCellModel.actualdurationminutes = phoneCell.actualdurationminutes;
            phoneCellModel.phonenumber = phoneCell.phonenumber;
            phoneCellModel.durationValue = viewModel.setSelectedTime(phoneCell.actualdurationminutes.ToString());
            phoneCellModel.statecodeValue = viewModel.setSelectedState("1");
            phoneCellModel.statuscodeValue = viewModel.setSelectedStatus("2");
            // Regarding
            if (phoneCell.contact_id != Guid.Empty)
            {
                phoneCellModel.Customer = new CustomerLookUp()
                {
                    Id = phoneCell.contact_id,
                    Name = phoneCell.contact_name,
                    Type = 1
                };
            }
            else if (phoneCell.account_id != Guid.Empty)
            {
                phoneCellModel.Customer = new CustomerLookUp()
                {
                    Id = phoneCell.account_id,
                    Name = phoneCell.account_name,
                    Type = 2
                };
            }
            else if (phoneCell.lead_id != Guid.Empty)
            {
                phoneCellModel.Customer = new CustomerLookUp()
                {
                    Id = phoneCell.lead_id,
                    Name = phoneCell.lead_name,
                    Type = 3
                };
            }
            else if (phoneCell.user_id != Guid.Empty)
            {
                phoneCellModel.Customer = new CustomerLookUp()
                {
                    Id = phoneCell.user_id,
                    Name = phoneCell.user_name
                };
            }

            // call from

            phoneCellModel.createdon = phoneCell.createdon;
            viewModel.PhoneCellModel = phoneCellModel;

            // get date from fetch
            string xml_fromcell = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true' >
                                    <entity name='phonecall' >
                                        <attribute name='subject' />
                                        <attribute name='activityid' />
                                        <order attribute='subject' descending='false' />
                                        <filter type='and' >
                                            <condition attribute='activityid' operator='eq' value='" +id+ @"' />
                                        </filter>
                                        <link-entity name='activityparty' from='activityid' to='activityid' link-type='inner' alias='ab' >
                                            <attribute name='partyid' alias='partyID'/>
                                            <attribute name='participationtypemask' alias='typemask' />
                                            <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='partyaccount' >
                                                <attribute name='bsd_name' alias='account_name'/>
                                            </link-entity>
                                            <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='partycontact' >
                                                <attribute name='fullname' alias='contact_name'/>
                                            </link-entity>
                                            <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='partylead' >
                                                <attribute name='fullname' alias='lead_name'/>
                                            </link-entity>
                                            <link-entity name='systemuser' from='systemuserid' to='partyid' link-type='outer' alias='partyuser' >
                                                <attribute name='fullname' alias='user_name'/>
                                            </link-entity>
                                        </link-entity>
                                    </entity>
                                </fetch>";
            var _result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PartyModel>>("phonecalls", xml_fromcell);
            var data = _result.value;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    if(item.typemask == 1) // from call
                    {
                        if(item.contact_name != null)
                        {
                            item.Customer = new CustomerLookUp()
                            {
                                Id = item.partyID,
                                Name = item.contact_name,
                                Type = 1
                            };
                        }
                        else if(item.account_name != null)
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
                        viewModel.listFromCell.Add(item);
                        dataStackFrom.Add(this.renderStack(item.Customer.Name, "FromCell"));
                    }
                    else if (item.typemask == 2)
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
                        viewModel.listToCell.Add(item);
                        dataStackTo.Add(this.renderStack(item.Customer.Name, "ToCell"));

                    }
                    toCell.Data = dataStackTo;
                    fromCell.Data = dataStackFrom;
                }
            }

            viewModel.Title = "Thông Tin Cuộc Gọi";
            viewModel.IsBusy = false;
        }

        private StackLayout renderStack(string item, string check)
        {           
            StackLayout stackLayout = new StackLayout
            {                
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions= LayoutOptions.Center,
                BackgroundColor = Color.FromHex("#F3F3F3"),
                Padding = 2,
                Spacing = 6
            };           
            var label = new Label()
            {
                Text = item,
                FontSize = 16,               
                TextColor = Color.Black,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            var clearButton = new Label
            {
                HeightRequest = 20,
                WidthRequest = 20,
                Text = "\uf057",
                FontFamily = "FontAwesomeSolid",
                FontSize=16,
                VerticalTextAlignment = TextAlignment.Center
            };            
            //var imageButton = new ImageButton
            //{
            //    BackgroundColor = Color.White,
            //   // Source = "clear.png",
            //    HeightRequest = 20,
            //    WidthRequest = 20,
            //    Margin = 2
                
            //};
            //imageButton.Source = new FontImageSource
            //{
            //    Glyph = "\uf30c",
            //    FontFamily = "FontAwesomeSolid",
            //    Size = 20
            //};

            if (check == "ToCell")
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += CleanClickToCell;
                clearButton.GestureRecognizers.Add(tapGestureRecognizer);
                //imageButton.Clicked += CleanClickToCell;
            }
            else if(check == "FromCell")
            {
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += CleanClickFromCell;
                clearButton.GestureRecognizers.Add(tapGestureRecognizer);
                //imageButton.Clicked += CleanClickFromCell;
            }          

            stackLayout.Children.Add(label);
            stackLayout.Children.Add(clearButton);
            return stackLayout;
        }

        private void CleanClickToCell(object sender, EventArgs e)
        {
            var button = sender as Label;
            int index = dataStackTo.IndexOf(dataStackTo.FirstOrDefault(x => x.Children.Last() == button));
            this.deleteItem(index, "ToCell");

        }

        private void CleanClickFromCell(object sender, EventArgs e)
        {
            var button = sender as Label;
            int index = dataStackFrom.IndexOf(dataStackFrom.FirstOrDefault(x => x.Children.Last() == button));
            this.deleteItem(index, "FromCell");

        }
        private void deleteItem(int index, string check)
        {
            if (check == "ToCell")
            {
                viewModel.listToCell.Remove(viewModel.listToCell[index]);
                dataStackTo.Remove(dataStackTo[index]);
                toCell.Data = dataStackTo;
            }
            else if (check == "FromCell")
            {
                viewModel.listFromCell.Remove(viewModel.listFromCell[index]);
                dataStackFrom.Remove(dataStackFrom[index]);
                fromCell.Data = dataStackFrom;
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

        private void DatePicker_Focused(object sender, FocusEventArgs e)
        {
            viewModel.FocusDateTimeStart = true;
        }

        private void DatePickerStart_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (viewModel.FocusDateTimeStart)
            {
                DateTime timeNew = e.NewDate;
                TimeSpan _timeStart = viewModel.PhoneCellModel.timeStart;
                var scheduledstart = new DateTime(timeNew.Year, timeNew.Month, timeNew.Day, _timeStart.Hours, _timeStart.Minutes, _timeStart.Seconds);
                //viewModel.FocusTimePickerStart = true;
                
                // check thời gian gian kết thúc
                if (this.compareDateTime(viewModel.PhoneCellModel.scheduledend, viewModel.PhoneCellModel.scheduledstart) != 1)
                {
                    DisplayAlert("Thông Báo", "Vui lòng chọn thời gian bắt đầu nhỏ hơn thời gian kết thúc!", "Đồng ý");
                    int valueDate = int.Parse(viewModel.PhoneCellModel.durationValue.Val);
                    var time = viewModel.PhoneCellModel.scheduledend.Value.AddMinutes(-valueDate);
                    viewModel.PhoneCellModel.scheduledstart = new DateTime(time.Year, time.Month, time.Day, _timeStart.Hours, _timeStart.Minutes, _timeStart.Seconds);
                    viewModel.PhoneCellModel.timeStart = new TimeSpan(viewModel.PhoneCellModel.scheduledstart.Value.Hour, viewModel.PhoneCellModel.scheduledstart.Value.Minute, 0);
                }
                else
                {
                    viewModel.PhoneCellModel.scheduledstart = scheduledstart;
                    this.totalDuration();
                }
            }
        }

        private void TimePickerStart_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
            {
                var time = timePickerStart.Time;
                if (viewModel.PhoneCellModel.scheduledend != null && viewModel.PhoneCellModel.scheduledstart != null)
                {
                    DateTime timeNew = viewModel.PhoneCellModel.scheduledstart.Value;
                    timeNew = new DateTime(timeNew.Year, timeNew.Month, timeNew.Day, time.Hours, time.Minutes, time.Seconds);
                    // 
                    if (this.compareDateTime(timeNew, viewModel.PhoneCellModel.scheduledend.Value) == -1)
                    {
                        viewModel.PhoneCellModel.timeStart = time;
                        viewModel.PhoneCellModel.scheduledstart = timeNew;
                        this.totalDuration();
                    }
                    else
                    {
                        if (isInit != true)
                        {
                            DisplayAlert("Thông Báo", "Vui lòng chọn thời gian bắt đầu nhỏ hơn thời gian kết thúc!", "Đồng ý");
                            var time1 = viewModel.PhoneCellModel.scheduledend.Value;
                            int valueDate = int.Parse(viewModel.PhoneCellModel.durationValue.Val);
                            viewModel.PhoneCellModel.scheduledstart = viewModel.PhoneCellModel.scheduledend.Value.AddMinutes(-valueDate);
                            viewModel.PhoneCellModel.timeStart = new TimeSpan(viewModel.PhoneCellModel.scheduledstart.Value.Hour, viewModel.PhoneCellModel.scheduledstart.Value.Minute, 0);
                        }
                    }
                }
                else
                {
                    viewModel.PhoneCellModel.timeStart = time;
                }

            }
        }

        private void DatePickerEnd_Focused(object sender, FocusEventArgs e)
        {
            viewModel.FocusDateTimeEnd = true;
        }

        private void DatePickerEnd_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (viewModel.FocusDateTimeEnd)
            {
                if (viewModel.PhoneCellModel.scheduledstart != null)
                {
                    DateTime timeNew = e.NewDate;
                    TimeSpan time = viewModel.PhoneCellModel.timeEnd;
                    DateTime _scheduledend = viewModel.PhoneCellModel.scheduledend.Value;
                    _scheduledend = new DateTime(timeNew.Year, timeNew.Month, timeNew.Day, time.Hours, time.Minutes, time.Seconds);

                    if (this.compareDateTime(_scheduledend, viewModel.PhoneCellModel.scheduledstart) == 1)
                    {
                        viewModel.PhoneCellModel.scheduledend = _scheduledend;
                        //viewModel.FocusTimePickerEnd = true;
                        this.totalDuration();
                    }
                    else
                    {
                        DisplayAlert("Thông Báo", "Vui lòng chọn thời gian kết thúc lớn hơn thời gian bắt đầu!", "Đồng ý");
                        int valueDate = int.Parse(viewModel.PhoneCellModel.durationValue.Val);
                        viewModel.PhoneCellModel.scheduledend = viewModel.PhoneCellModel.scheduledstart.Value.AddMinutes(valueDate);
                        viewModel.PhoneCellModel.timeEnd = new TimeSpan(viewModel.PhoneCellModel.scheduledend.Value.Hour, viewModel.PhoneCellModel.scheduledend.Value.Minute, 0);
                    }
                }
                else
                {
                    DisplayAlert("Thông Báo", "Vui lòng chọn thời gian bắt đầu!", "Đồng ý");
                }

            }
        }

        private void TimePickerEnd_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
            {
                var time = timePickerEnd.Time;
                if (viewModel.PhoneCellModel.scheduledend != null)
                {
                    DateTime timeNew = viewModel.PhoneCellModel.scheduledend.Value;
                    timeNew = new DateTime(timeNew.Year, timeNew.Month, timeNew.Day, time.Hours, time.Minutes, time.Seconds);
                    if (this.compareDateTime(timeNew, viewModel.PhoneCellModel.scheduledstart) == 1)
                    {
                        viewModel.PhoneCellModel.timeEnd = time;
                        viewModel.PhoneCellModel.scheduledend = timeNew;
                        this.totalDuration();
                    }
                    else
                    {
                        if (isInit != true)
                        {
                            DisplayAlert("Thông Báo", "Vui lòng chọn thời gian kết thúc lớn hơn thời gian bắt đầu!", "Đồng ý");
                            int valueDate = int.Parse(viewModel.PhoneCellModel.durationValue.Val);
                            viewModel.PhoneCellModel.scheduledend = viewModel.PhoneCellModel.scheduledstart.Value.AddMinutes(valueDate);
                            viewModel.PhoneCellModel.timeEnd = new TimeSpan(viewModel.PhoneCellModel.scheduledend.Value.Hour, viewModel.PhoneCellModel.scheduledend.Value.Minute, 0);
                        }
                    }
                }
                else
                {
                    viewModel.PhoneCellModel.timeEnd = time;
                }
            }
        }

        private void Actualduration_value_SelectedIndexChanged(object sender, EventArgs e)
        {
            int valueDate = int.Parse(viewModel.PhoneCellModel.durationValue.Val);
            if (viewModel.PhoneCellModel.scheduledstart == null)
            {
                viewModel.PhoneCellModel.scheduledstart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                viewModel.PhoneCellModel.timeStart = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
                viewModel.PhoneCellModel.scheduledend = viewModel.PhoneCellModel.scheduledstart.Value.AddMinutes(valueDate);
                viewModel.PhoneCellModel.timeEnd = new TimeSpan(viewModel.PhoneCellModel.scheduledend.Value.Hour, viewModel.PhoneCellModel.scheduledend.Value.Minute, 0);
            }
            else
            {
                viewModel.PhoneCellModel.scheduledend = new DateTime(viewModel.PhoneCellModel.scheduledstart.Value.Year, viewModel.PhoneCellModel.scheduledstart.Value.Month, viewModel.PhoneCellModel.scheduledstart.Value.Day, viewModel.PhoneCellModel.scheduledstart.Value.Hour, viewModel.PhoneCellModel.scheduledstart.Value.Minute, 0).AddMinutes(valueDate);
                viewModel.PhoneCellModel.timeEnd = new TimeSpan(viewModel.PhoneCellModel.scheduledend.Value.Hour, viewModel.PhoneCellModel.scheduledend.Value.Minute, 0);
            }
            viewModel.PhoneCellModel.actualdurationminutes = valueDate;
        }

        private void totalDuration()
        {
            if (viewModel.PhoneCellModel.scheduledstart != null && viewModel.PhoneCellModel.scheduledend != null)
            {
                TimeSpan difference = viewModel.PhoneCellModel.scheduledend.Value - viewModel.PhoneCellModel.scheduledstart.Value;
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
                viewModel.PhoneCellModel.durationValue = viewModel.setSelectedTime(_minutes.ToString());
            }
            else
            {
                viewModel.list_picker_durations.Add(new OptionSet() { Val = _minutes.ToString(), Label = labelOption });
                viewModel.PhoneCellModel.durationValue = viewModel.setSelectedTime(_minutes.ToString());
            }
        }

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

        private void LvLookUp_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as Models.LookUp;
            if (viewModel.check_open == "Lookup")
            {
                var model = viewModel.PhoneCellModel;
                PropertyInfo prop = model.GetType().GetProperty(viewModel.CurrentLookUpConfig.PropertyName); // 
                prop.SetValue(model, item);
                viewModel.ShowLookUpModal = false; // được get mặc định từ viewModel kế thừa từ FormViewModel
            }
            else if(viewModel.check_open == "ToCell")
            {
                var customer = new CustomerLookUp() { Name = item.Name, Id = item.Id };
                var model = viewModel.PhoneCellModel;
                PropertyInfo prop = model.GetType().GetProperty(viewModel.CurrentLookUpConfig.PropertyName); // 
                if (prop.Name == "Contact")
                {
                    customer.Type = 1;
                }
                else if(prop.Name == "Account")
                {
                    customer.Type = 2;
                }
                else if(prop.Name == "Lead")
                {
                    customer.Type = 3;
                }

                if (!this.check_Data(viewModel.listToCell, customer.Id))
                {
                    viewModel.listToCell.Add(new PartyModel() { typemask = 2, Customer = customer });
                    dataStackTo.Add(this.renderStack(item.Name, viewModel.check_open));
                    toCell.Data = dataStackTo;
                }              
                viewModel.ShowLookUpModal = false;
            }
            else if(viewModel.check_open == "FromCell")
            {
                var customer = new CustomerLookUp() { Name = item.Name, Id = item.Id };
                var model = viewModel.PhoneCellModel;
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
                // tạm thời delete data listFromCell and dataStackFrom sau đó add item mới (vi CRM đang chỉ chọn được 1 item)
                if (viewModel.listFromCell.Count > 0)
                {
                    this.deleteItem(0, viewModel.check_open);
                }
                viewModel.listFromCell.Add(new PartyModel() { typemask = 1, Customer = customer });
                dataStackFrom.Add(this.renderStack(item.Name, viewModel.check_open));
                fromCell.Data = dataStackFrom;
                viewModel.ShowLookUpModal = false;
            }
        }

        private bool check_Data(ObservableCollection<PartyModel> listParty, Guid id)
        {
            var check = false;
            foreach (var item in listParty)
            {
                if(id == item.Customer.Id)
                {
                    return check = true;
                }
            }

            return check;

        }
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

        private void btnCloseLookUpModal(object sender, EventArgs e)
        {
            viewModel.ShowLookUpModal = false;
        }

        // onclick open popup
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
            viewModel.PhoneCellModel.Customer = null;
        }

        private void ToCell_OnClicked(object sender, EventArgs e)
        {
            viewModel.check_open = "ToCell"; // event open ToCell
            var btnKHCN = FindByName("btnKHCN") as Button;
            viewModel.ShowLookUpModal = true;
            BtnKHCN_Clicked(btnKHCN, null);
        }

        private void FromCell_OnClicked(object sender, EventArgs e)
        {
            viewModel.check_open = "FromCell"; // event open ToCell
            var btnKHCN = FindByName("btnKHCN") as Button;
            viewModel.ShowLookUpModal = true;
            BtnKHCN_Clicked(btnKHCN, null);
        }

        // check data
        private async void savePhone()
        {
            LoadingHelper.Show();
            var check = await checkData(); // kiểm tra dư liệu
            // check create or update
            if (check)
            {
                if (viewModel.PhoneCellModel.activityid == Guid.Empty) // create
                {
                    var created = await createPhoneCell(viewModel, "create");
                    if (created != new Guid())
                    {                       
                       await this.loadDataForm(created);
                        LoadingHelper.Hide();
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "Tạo cuộc gọi thành công", "OK");
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "Tạo cuộc gọi thất bại", "OK");                       
                    }
                }
                else // update
                {
                    var update = await updatePhoneCell(viewModel, "update");
                    if (update)
                    {                      
                       await this.loadDataForm(viewModel.PhoneCellModel.activityid);
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
        }

        // Check the required information (scheduledstart, scheduledend, )
        private async Task<bool> checkData()
        {
            var dataPhone = viewModel.PhoneCellModel;
            if (!string.IsNullOrWhiteSpace(dataPhone.subject) && viewModel.listFromCell.Count > 0 && viewModel.listToCell.Count > 0 && dataPhone.scheduledstart != null && dataPhone.scheduledend != null)
            {
                if(!string.IsNullOrWhiteSpace(dataPhone.phonenumber))
                {
                    if(!PhoneNumberFormatVNHelper.CheckValidate(dataPhone.phonenumber))
                    {
                        await DisplayAlert("Thông Báo", "Số điện thoại sai định dạng!", "Đồng ý");
                        return false;
                    }
                }
                return true;
            }
            else
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Thông báo", "Vui lòng nhập các thông tin bắt buộc!", "OK");
                return false;
            }
        }
        public async Task<Boolean> DeletLookup(string fieldName, Guid id)
        {
            var result = await CrmHelper.SetNullLookupField("phonecalls", id, fieldName);
            return result.IsSuccess;
        }
        private async Task<object> getContent(PhoneCellViewModel phoneCell, string value)
        {
            var dataPhone = phoneCell.PhoneCellModel;
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["activityid"] = dataPhone.activityid.ToString();
            data["subject"] = dataPhone.subject;
            data["description"] = dataPhone.description ?? "";
            data["scheduledstart"] = dataPhone.scheduledstart.Value.ToUniversalTime();
            data["scheduledend"] = dataPhone.scheduledend.Value.ToUniversalTime();
            data["actualdurationminutes"] = dataPhone.actualdurationminutes;
            if ( value == "update")
            {
                data["statecode"] = dataPhone.statecode;
                data["statuscode"] = dataPhone.statuscode;
            }
            data["phonenumber"] = dataPhone.phonenumber ?? "";
            if (dataPhone.Customer != null)
            {
                if (dataPhone.Customer.Type == 1)
                {
                    data["regardingobjectid_contact_phonecall@odata.bind"] = "/contacts(" + dataPhone.Customer.Id.ToString() + ")";
                }
                else if (dataPhone.Customer.Type == 2)
                {
                    data["regardingobjectid_account_phonecall@odata.bind"] = "/accounts(" + dataPhone.Customer.Id.ToString() + ")";
                }
                else if (dataPhone.Customer.Type == 3)
                {
                    data["regardingobjectid_lead_phonecall@odata.bind"] = "/leads(" + dataPhone.Customer.Id.ToString() + ")";
                }
            }
            else
            {
                await DeletLookup("regardingobjectid_contact_phonecall", dataPhone.activityid);
                await DeletLookup("regardingobjectid_account_phonecall", dataPhone.activityid);
                await DeletLookup("regardingobjectid_lead_phonecall", dataPhone.activityid);
            }
            

            var dataFromCell = phoneCell.listFromCell;
            var dataToCell = phoneCell.listToCell;
            //check data FromCell and ToCell          
            List<object> dataFromTo = new List<object>();

            // FromCell
            foreach (var item in dataFromCell) // 2 
            {
                IDictionary<string, object> item_from = new Dictionary<string, object>();
                if (item.Customer.Type == 1) // contact
                {
                    item_from["partyid_contact@odata.bind"] = "/contacts(" + item.Customer.Id + ")";
                    item_from["participationtypemask"] = 1;
                }
                else if (item.Customer.Type == 2) // account
                {
                    item_from["partyid_account@odata.bind"] = "/accounts(" + item.Customer.Id + ")";
                    item_from["participationtypemask"] = 1;
                }
                else if (item.Customer.Type == 3) // lead
                {
                    item_from["partyid_lead@odata.bind"] = "/leads(" + item.Customer.Id + ")";
                    item_from["participationtypemask"] = 1;
                }
                else
                {
                    item_from["partyid_systemuser@odata.bind"] = "/systemusers(" + item.Customer.Id + ")";
                    item_from["participationtypemask"] = 1;
                }
                dataFromTo.Add(item_from);
            }

            //ToCell
            foreach(var itemTo in dataToCell)
            {
                IDictionary<string, object> item_to = new Dictionary<string, object>();
                if (itemTo.Customer.Type == 1) // contact
                {
                    item_to["partyid_contact@odata.bind"] = "/contacts(" + itemTo.Customer.Id + ")";
                    item_to["participationtypemask"] = 2;
                }
                else if (itemTo.Customer.Type == 2) // account
                {
                    item_to["partyid_account@odata.bind"] = "/accounts(" + itemTo.Customer.Id + ")";
                    item_to["participationtypemask"] = 2;
                }
                else if (itemTo.Customer.Type == 3) // lead
                {
                    item_to["partyid_lead@odata.bind"] = "/leads(" + itemTo.Customer.Id + ")";
                    item_to["participationtypemask"] = 2;
                }
                else
                {
                    item_to["partyid_systemuser@odata.bind"] = "/systemusers(" + itemTo.Customer.Id + ")";
                    item_to["participationtypemask"] = 2;
                }
                dataFromTo.Add(item_to);

            }

            data["phonecall_activity_parties"] = dataFromTo;
            return  data;
        }

        public async Task<Guid> createPhoneCell(PhoneCellViewModel phoneCell, string value)
        {
            phoneCell.PhoneCellModel.activityid = Guid.NewGuid();
            string path = "/phonecalls";
            var content = await this.getContent(phoneCell, value);

            CrmApiResponse result = await CrmHelper.PostData(path, content);

            if (result.IsSuccess)
            {
                return phoneCell.PhoneCellModel.activityid; // return id PhoneCell New
            }
            else
            {
                var mess = result.ErrorResponse?.error?.message ?? "Đã xảy ra lỗi. Vui lòng thử lại.";
                await App.Current.MainPage.DisplayAlert("", mess, "OK");
                return new Guid();
            }
        }
        public async Task<Boolean> updatePhoneCell(PhoneCellViewModel phoneCell, string value)
        {
            string path = "/phonecalls(" + phoneCell.PhoneCellModel.activityid + ")";
            var content = await this.getContent(phoneCell, value);
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

        private void UpdatePhone_Clicked(object sender, EventArgs e)
        {        
            this.savePhone();         
        }

        private void CompletedPhone_Clicked(object sender, EventArgs e)
        {
            //viewModel.PhoneCellModel.statecode = 1;
            //viewModel.PhoneCellModel.statecode = 1;          
            viewModel.ShowStatus = true;          
        }

        private async void CanceledPhone_Clicked(object sender, EventArgs e)
        {
            bool check = await DisplayAlert("Thông báo", "Bạn có muốn hủy cuộc gọi này không ?", "Đồng Ý", "Không Đồng Ý");            
            if (check == true)
            {
                viewModel.PhoneCellModel.statecode = 2; // update statecode canceled
                viewModel.PhoneCellModel.statuscode = 3;
                this.savePhone();
            }         
        }

        private void CreateNew_Clicked(object sender, EventArgs e)
        {        
            this.savePhone();         
        }

        private void State_SelectedIndexChanged(object sender, EventArgs e)
        {
            var state = viewModel.PhoneCellModel.statecodeValue.Val;
            if (state == "1") // completed 
            {
                Console.WriteLine("update status");
                txtStatus.IsVisible = true;
                Status_value.IsVisible = true;
                viewModel.PhoneCellModel.statecode = int.Parse(state);
                viewModel.PhoneCellModel.statuscode = int.Parse(viewModel.PhoneCellModel.statuscodeValue.Val);
            }
            else
            {
                // Canceled : state = 2, status = 3
                viewModel.PhoneCellModel.statecode = int.Parse(state); 
                viewModel.PhoneCellModel.statuscode = 3;
                txtStatus.IsVisible = false;
                Status_value.IsVisible = false;
            }
        }

        // option : if state == completed ==> seleted item status: made(thực hiên), received(nhận)
        private void Status_SelectedIndexChanged(object sender, EventArgs e)
        {
            var state = viewModel.PhoneCellModel.statecodeValue.Val;
            var status = viewModel.PhoneCellModel.statuscodeValue.Val;
            if (state == "1") // completed
            {
                viewModel.PhoneCellModel.statuscode = int.Parse(status);
            }
        }

        private void btnClose(object sender, EventArgs e)
        {
            viewModel.ShowStatus = false;
        }

        private void btnAccept(object sender, EventArgs e)
        {         
            viewModel.ShowStatus = false;
            this.savePhone();          
        }
    }
}