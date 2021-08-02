using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskForm : ContentPage
    {
        private bool isInit = true;
        public Action<bool> CheckTaskForm;
        private Guid _idActivity;
        public TaskFormViewModel viewModel;
        public TaskForm(Guid idActivity)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new TaskFormViewModel();
            this._idActivity = idActivity;
            viewModel.IsBusy = true;
            Init();
        }

        public async void Init()
        {
            await loadDataForm(this._idActivity);
            if (viewModel.TaskFormModel != null)
            {
                CheckTaskForm?.Invoke(true);
                isInit = false;
            }
            else
                CheckTaskForm?.Invoke(false);
        }

        public TaskForm()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new TaskFormViewModel();
            viewModel.Title = "Tạo Công Việc";
            viewModel.IsBusy = false;
            isInit = false;
            grid_create.IsVisible = true;
            grid_updateTask.IsVisible = false;
            // setDefault create task
            TaskFormModel taskFormModel = new TaskFormModel();
            taskFormModel.editable = true;
            taskFormModel.scheduledstart = new DateTime(DateTime.Now.ToLocalTime().Year, DateTime.Now.ToLocalTime().Month, DateTime.Now.ToLocalTime().Day, DateTime.Now.ToLocalTime().Hour, DateTime.Now.ToLocalTime().Minute, 0);
            taskFormModel.timeStart = new TimeSpan(DateTime.Now.ToLocalTime().Hour, DateTime.Now.ToLocalTime().Minute, 0);
            taskFormModel.durationValue = viewModel.setSelectedTime(30 + "");
            viewModel.TaskFormModel = taskFormModel; 
        }

        public TaskForm(DateTime dateTimeNew)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new TaskFormViewModel();
            viewModel.Title = "Tạo Công Việc";
            isInit = false;
            viewModel.IsBusy = false;
            grid_create.IsVisible = true;
            grid_updateTask.IsVisible = false;
            // setDefault create task
            TaskFormModel taskFormModel = new TaskFormModel();
            taskFormModel.editable = true;
            taskFormModel.scheduledstart = new DateTime(dateTimeNew.Year, dateTimeNew.Month, dateTimeNew.Day, dateTimeNew.Hour, dateTimeNew.Minute, 0);
            taskFormModel.timeStart = new TimeSpan(dateTimeNew.Hour, dateTimeNew.Minute, 0);
            taskFormModel.durationValue = viewModel.setSelectedTime(30 + "");
            viewModel.TaskFormModel = taskFormModel;
        }

        public async Task loadDataForm(Guid id)
        {
            grid_create.IsVisible = false;
            grid_updateTask.IsVisible = true;

            string xml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
            <entity name='task'>
                <attribute name='subject' />
                <attribute name='statecode' />
                <attribute name='prioritycode' />
                <attribute name='scheduledstart' />
                <attribute name='scheduledend' />
                <attribute name='createdby' />
                <attribute name='regardingobjectid' />
                <attribute name='description' />
                <attribute name='actualdurationminutes' />
                <attribute name='activityid' />
                <order attribute='subject' descending='false' />
                <filter type='and'>
                    <condition attribute='activityid' operator='eq' uitype='task' value='" + id + @"' />
                </filter>
                <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='outer' alias='accounts'>
                    <attribute name='accountid' alias='account_id' />                  
                    <attribute name='bsd_name' alias='account_name'/>
                </link-entity>
                <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer' alias='contacts'>
                  <attribute name='contactid' alias='contact_id' />                  
                  <attribute name='fullname' alias='contact_name'/>
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

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<TaskFormModel>>("tasks", xml);
            TaskFormModel taskForm = result.value.FirstOrDefault();

            TaskFormModel taskFormModel = new TaskFormModel();
            taskFormModel.activityid = taskForm.activityid;
            taskFormModel.subject = taskForm.subject;
            taskFormModel.statecode = taskForm.statecode;
            taskFormModel.statuscode = taskForm.statuscode;
            //check statecode
            if (taskForm.statecode == 1) // completed
            {
                taskFormModel.editable = false;
                grid_updateTask.ColumnDefinitions[0].Width = new GridLength(0);
                grid_updateTask.ColumnDefinitions[1].Width = new GridLength(0);
            }
            else if (taskForm.statecode == 2) // canceled
            {
                taskFormModel.editable = false; // open
                grid_updateTask.IsVisible = false;
            }
            else
            {
                taskFormModel.editable = true;
            }

            if(taskForm.scheduledend.HasValue)
            {
                taskFormModel.scheduledend = taskForm.scheduledend.Value.ToLocalTime();
                taskFormModel.timeEnd = taskForm.scheduledend.Value.ToLocalTime().TimeOfDay;
            }
            else
            {
                taskFormModel.scheduledend = DateTime.Now.Date;
                taskFormModel.timeEnd = DateTime.Now.TimeOfDay;
            }
            if (taskForm.scheduledstart.HasValue == true)
            {
                taskFormModel.scheduledstart = taskForm.scheduledstart.Value.ToLocalTime();
                taskFormModel.timeStart = taskForm.scheduledstart.Value.ToLocalTime().TimeOfDay;
            }
            else
            {
                taskFormModel.scheduledstart = null;
                taskFormModel.timeStart = DateTime.Now.TimeOfDay;
            }
            taskFormModel.description = taskForm.description;
            taskFormModel.actualdurationminutes = taskForm.actualdurationminutes;

            taskFormModel.durationValue = viewModel.setSelectedTime(taskForm.actualdurationminutes.ToString());
            if (taskForm.contact_id != Guid.Empty)
            {
                taskFormModel.Customer = new CustomerLookUp()
                {
                    Id = taskForm.contact_id,
                    Name = taskForm.contact_name,
                    Type = 1
                };
            }
            else if (taskForm.account_id != Guid.Empty)
            {
                taskFormModel.Customer = new CustomerLookUp()
                {
                    Id = taskForm.account_id,
                    Name = taskForm.account_name,
                    Type = 2
                };
            }
            else if (taskForm.lead_id != Guid.Empty)
            {
                taskFormModel.Customer = new CustomerLookUp()
                {
                    Id = taskForm.lead_id,
                    Name = taskForm.lead_name,
                    Type = 3
                };
            }
            else if (taskForm.user_id != Guid.Empty)
            {
                taskFormModel.Customer = new CustomerLookUp()
                {
                    Id = taskForm.user_id,
                    Name = taskForm.user_name
                };
            }
            taskFormModel.createdon = taskForm.createdon;
            viewModel.TaskFormModel = taskFormModel;
            viewModel.Title = "Thông Tin Công Việc";
            viewModel.IsBusy = false;
        }

        private void Actualduration_value_SelectedIndexChanged(object sender, EventArgs e)
        {
            int valueDate = int.Parse(viewModel.TaskFormModel.durationValue.Val);

            if (viewModel.TaskFormModel.scheduledstart == null)
            {
                var start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                var time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);

                viewModel.TaskFormModel.scheduledstart = start;
                viewModel.TaskFormModel.timeStart = time;
                viewModel.TaskFormModel.scheduledend = start.AddMinutes(valueDate);
                viewModel.TaskFormModel.timeEnd = new TimeSpan(viewModel.TaskFormModel.scheduledend.Value.Hour, viewModel.TaskFormModel.scheduledend.Value.Minute, 0);
            }
            else
            {
                viewModel.TaskFormModel.scheduledend = new DateTime(viewModel.TaskFormModel.scheduledstart.Value.Year, viewModel.TaskFormModel.scheduledstart.Value.Month, viewModel.TaskFormModel.scheduledstart.Value.Day, viewModel.TaskFormModel.scheduledstart.Value.Hour, viewModel.TaskFormModel.scheduledstart.Value.Minute, 0).AddMinutes(valueDate);
                viewModel.TaskFormModel.timeEnd = new TimeSpan(viewModel.TaskFormModel.scheduledend.Value.Hour, viewModel.TaskFormModel.scheduledend.Value.Minute, 0);
            }

            viewModel.TaskFormModel.actualdurationminutes = valueDate;
        }

        // close popup customer
        private void ClearLookup_Clicked(object sender, EventArgs e)
        {
            viewModel.TaskFormModel.Customer = null;
        }

        // opent popup customer and loading 
        private void OpenModel_Clicked(object sender, EventArgs e)
        {
            // load data mặc định show popup
            var btnKHCN = FindByName("btnKHCN") as Button;
            viewModel.ShowLookUpModal = true;
            BtnKHCN_Clicked(btnKHCN, null);
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

        // event item listView
        private void LvLookUp_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as LookUp;
            var model = viewModel.TaskFormModel;
            PropertyInfo prop = model.GetType().GetProperty(viewModel.CurrentLookUpConfig.PropertyName); // 
            prop.SetValue(model, item);
            viewModel.ShowLookUpModal = false; // được get mặc định từ viewModel kế thừa từ FormViewModel
        }

        private void btnCloseLookUpModal(object sender, EventArgs e)
        {
            viewModel.ShowLookUpModal = false;
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
        private void DatePickerStart_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (viewModel.FocusDateTimeStart)
            {
                DateTime timeNew = e.NewDate;
                TimeSpan _timeStart = viewModel.TaskFormModel.timeStart;
                var scheduledstart = new DateTime(timeNew.Year, timeNew.Month, timeNew.Day, _timeStart.Hours, _timeStart.Minutes, _timeStart.Seconds);
                //viewModel.FocusTimePickerStart = true;                
                // check thời gian gian kết thúc
                if (this.compareDateTime(viewModel.TaskFormModel.scheduledend, scheduledstart) != 1)
                {
                    DisplayAlert("Thông Báo", "Vui lòng chọn thời gian bắt đầu nhỏ hơn thời gian kết thúc!", "Đồng ý");
                    int valueDate = int.Parse(viewModel.TaskFormModel.durationValue.Val);
                    var time = viewModel.TaskFormModel.scheduledend.Value.AddMinutes(-valueDate);
                    viewModel.TaskFormModel.scheduledstart = new DateTime(time.Year, time.Month, time.Day, _timeStart.Hours, _timeStart.Minutes, _timeStart.Seconds);
                    viewModel.TaskFormModel.timeStart = new TimeSpan(viewModel.TaskFormModel.scheduledstart.Value.Hour, viewModel.TaskFormModel.scheduledstart.Value.Minute, 0);
                }
                else
                {
                    viewModel.TaskFormModel.scheduledstart = scheduledstart;
                    this.totalDuration();
                }
            }
        }

        // add thời gian bắt đầu
        private void TimePickerStart_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
            {
                var time = timePickerStart.Time;
                if (viewModel.TaskFormModel.scheduledend != null && viewModel.TaskFormModel.scheduledstart != null)
                {
                    DateTime timeNew = viewModel.TaskFormModel.scheduledstart.Value;
                    timeNew = new DateTime(timeNew.Year, timeNew.Month, timeNew.Day, time.Hours, time.Minutes, time.Seconds);
                    // 
                    if (this.compareDateTime(timeNew, viewModel.TaskFormModel.scheduledend.Value) == -1)
                    {
                        viewModel.TaskFormModel.timeStart = time;
                        viewModel.TaskFormModel.scheduledstart = timeNew;
                        this.totalDuration();
                    }
                    else
                    {
                        if (isInit != true)
                        {
                            DisplayAlert("Thông Báo", "Vui lòng chọn thời gian bắt đầu nhỏ hơn thời gian kết thúc!", "Đồng ý");
                            var time1 = viewModel.TaskFormModel.scheduledend.Value;
                            int valueDate = int.Parse(viewModel.TaskFormModel.durationValue.Val);
                            viewModel.TaskFormModel.scheduledstart = viewModel.TaskFormModel.scheduledend.Value.AddMinutes(-valueDate);
                            viewModel.TaskFormModel.timeStart = new TimeSpan(viewModel.TaskFormModel.scheduledstart.Value.Hour, viewModel.TaskFormModel.scheduledstart.Value.Minute, 0);
                        }
                    }
                }
                else
                {
                    viewModel.TaskFormModel.timeStart = time;
                }

            }
        }

        private void DatePickerEnd_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (viewModel.FocusDateTimeEnd)
            {
                if (viewModel.TaskFormModel.scheduledstart != null)
                {
                    DateTime timeNew = e.NewDate;
                    TimeSpan time = viewModel.TaskFormModel.timeEnd;
                    DateTime _scheduledend = viewModel.TaskFormModel.scheduledend.Value;
                    _scheduledend = new DateTime(timeNew.Year, timeNew.Month, timeNew.Day, time.Hours, time.Minutes, time.Seconds);

                    if (this.compareDateTime(_scheduledend, viewModel.TaskFormModel.scheduledstart) == 1)
                    {
                        viewModel.TaskFormModel.scheduledend = _scheduledend;
                        //viewModel.FocusTimePickerEnd = true;
                        this.totalDuration();
                    }
                    else
                    {
                        DisplayAlert("Thông Báo", "Vui lòng chọn thời gian kết thúc lớn hơn thời gian bắt đầu!", "Đồng ý");
                        int valueDate = int.Parse(viewModel.TaskFormModel.durationValue.Val);
                        viewModel.TaskFormModel.scheduledend = viewModel.TaskFormModel.scheduledstart.Value.AddMinutes(valueDate);
                        viewModel.TaskFormModel.timeEnd = new TimeSpan(viewModel.TaskFormModel.scheduledend.Value.Hour, viewModel.TaskFormModel.scheduledend.Value.Minute, 0);
                    }
                }
                else
                {
                    DisplayAlert("Thông Báo", "Vui lòng chọn thời gian bắt đầu!", "Đồng ý");
                }

            }

        }

        // add thời gian kết thúc
        private void TimePickerEnd_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
            {
                var time = timePickerEnd.Time;
                if (viewModel.TaskFormModel.scheduledend != null)
                {
                    DateTime timeNew = viewModel.TaskFormModel.scheduledend.Value;
                    timeNew = new DateTime(timeNew.Year, timeNew.Month, timeNew.Day, time.Hours, time.Minutes, time.Seconds);
                    if (this.compareDateTime(timeNew, viewModel.TaskFormModel.scheduledstart) == 1)
                    {
                        viewModel.TaskFormModel.timeEnd = time;
                        viewModel.TaskFormModel.scheduledend = timeNew;
                        this.totalDuration();
                    }
                    else
                    {
                        if (isInit != true)
                        {
                            DisplayAlert("Thông Báo", "Vui lòng chọn thời gian kết thúc lớn hơn thời gian bắt đầu!", "Đồng ý");
                            int valueDate = int.Parse(viewModel.TaskFormModel.durationValue.Val);
                            viewModel.TaskFormModel.scheduledend = viewModel.TaskFormModel.scheduledstart.Value.AddMinutes(valueDate);
                            viewModel.TaskFormModel.timeEnd = new TimeSpan(viewModel.TaskFormModel.scheduledend.Value.Hour, viewModel.TaskFormModel.scheduledend.Value.Minute, 0);
                        }
                    }
                }
                else
                {
                    viewModel.TaskFormModel.timeEnd = time;
                }
            }
        }

        private void DatePickerEnd_Focused(object sender, FocusEventArgs e)
        {
            viewModel.FocusDateTimeEnd = true;
        }

        private void DatePicker_Focused(object sender, FocusEventArgs e)
        {
            viewModel.FocusDateTimeStart = true;
        }

        // tinh thời lượng khi change (thời gian bắt đầu, thời gian kết thúc)
        private void totalDuration()
        {
            if (viewModel.TaskFormModel.scheduledstart != null && viewModel.TaskFormModel.scheduledend != null)
            {
                DateTime dateTimeStart = new DateTime(viewModel.TaskFormModel.scheduledstart.Value.Year, viewModel.TaskFormModel.scheduledstart.Value.Month, viewModel.TaskFormModel.scheduledstart.Value.Day, viewModel.TaskFormModel.timeStart.Hours, viewModel.TaskFormModel.timeStart.Minutes, viewModel.TaskFormModel.timeStart.Seconds);
                DateTime dateTimeEnd = new DateTime(viewModel.TaskFormModel.scheduledend.Value.Year, viewModel.TaskFormModel.scheduledend.Value.Month, viewModel.TaskFormModel.scheduledend.Value.Day, viewModel.TaskFormModel.timeEnd.Hours, viewModel.TaskFormModel.timeEnd.Minutes, viewModel.TaskFormModel.timeEnd.Seconds);

                TimeSpan difference = dateTimeEnd - dateTimeStart;
                
                double _minutes = Math.Round(difference.TotalMinutes);

                if(_minutes < 0)
                {
                    DisplayAlert("", "Vui lòng chọn thời gian kết thúc lớn hơn thời gian bắt đầu!", "Đồng ý");
                }
                else if (_minutes < 60 && _minutes > 0)
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
                viewModel.TaskFormModel.durationValue = viewModel.setSelectedTime(_minutes.ToString());
            }
            else
            {
                viewModel.list_picker_durations.Add(new OptionSet() { Val = _minutes.ToString(), Label = labelOption });
                viewModel.TaskFormModel.durationValue = viewModel.setSelectedTime(_minutes.ToString());
            }
        }
        private async void saveTask()
        {
            LoadingHelper.Show();
            DateTime dateTimeStart = new DateTime(viewModel.TaskFormModel.scheduledstart.Value.Year, viewModel.TaskFormModel.scheduledstart.Value.Month, viewModel.TaskFormModel.scheduledstart.Value.Day, viewModel.TaskFormModel.timeStart.Hours, viewModel.TaskFormModel.timeStart.Minutes, viewModel.TaskFormModel.timeStart.Seconds);
            DateTime dateTimeEnd = new DateTime(viewModel.TaskFormModel.scheduledend.Value.Year, viewModel.TaskFormModel.scheduledend.Value.Month, viewModel.TaskFormModel.scheduledend.Value.Day, viewModel.TaskFormModel.timeEnd.Hours, viewModel.TaskFormModel.timeEnd.Minutes, viewModel.TaskFormModel.timeEnd.Seconds);
            if (dateTimeStart > dateTimeEnd)
            {
                await DisplayAlert("", "Vui lòng chọn thời gian kết thúc lớn hơn thời gian bắt đầu!", "Đồng ý");
                return ;
            }
            var check = await checkData(); // kiểm tra dư liệu
            // check create or update
            if (check)
            {
                if (viewModel.TaskFormModel.activityid == Guid.Empty) // create
                {
                    var created = await createTask(viewModel);
                    if (created != new Guid())
                    {
                        await this.loadDataForm(created);
                        LoadingHelper.Hide();// load data according to new id
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "Tạo công việc thành công", "OK");
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "Tạo công việc thất bại", "OK");
                    }
                }
                else // update
                {
                    var update = await updateTask(viewModel);
                    if (update)
                    {
                        await this.loadDataForm(viewModel.TaskFormModel.activityid);
                        LoadingHelper.Hide();// loading data after update
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

        // Check the required information (scheduledstart, scheduledend, )
        private async Task<bool> checkData()
        {
            var dataTask = viewModel.TaskFormModel;
            if (!string.IsNullOrWhiteSpace(dataTask.subject) && dataTask.scheduledstart != null && dataTask.scheduledend != null && dataTask.Customer != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<object> getContent(TaskFormViewModel tasks)
        {
            var dataTask = tasks.TaskFormModel;
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["activityid"] = dataTask.activityid.ToString();
            data["subject"] = dataTask.subject;
            data["description"] = dataTask.description ?? "";
            data["scheduledstart"] = dataTask.scheduledstart.Value.ToUniversalTime();
            data["scheduledend"] = dataTask.scheduledend.Value.ToUniversalTime();
            data["actualdurationminutes"] = dataTask.actualdurationminutes;
            data["statecode"] = dataTask.statecode;
            if (dataTask.Customer.Type == 1)
            {
                data["regardingobjectid_contact_task@odata.bind"] = "/contacts(" + dataTask.Customer.Id.ToString() + ")";
            }
            else if (dataTask.Customer.Type == 2)
            {
                data["regardingobjectid_account_task@odata.bind"] = "/accounts(" + dataTask.Customer.Id.ToString() + ")";
            }
            else
            {
                data["regardingobjectid_lead_task@odata.bind"] = "/leads(" + dataTask.Customer.Id.ToString() + ")";
            }
            return data;
        }

        public async Task<Guid> createTask(TaskFormViewModel task)
        {
            task.TaskFormModel.activityid = Guid.NewGuid();
            string path = "/tasks";
            var content = await this.getContent(task);

            CrmApiResponse result = await CrmHelper.PostData(path, content);

            if (result.IsSuccess)
            {
                return task.TaskFormModel.activityid;
            }
            else
            {
                var mess = result.ErrorResponse?.error?.message ?? "Đã xảy ra lỗi. Vui lòng thử lại.";
                await App.Current.MainPage.DisplayAlert("", mess, "OK");
                return new Guid();
            }
        }
        public async Task<Boolean> updateTask(TaskFormViewModel task)
        {
            string path = "/tasks(" + task.TaskFormModel.activityid + ")";
            var content = await this.getContent(task);
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

        private void UpdateTask_Clicked(object sender, EventArgs e)
        {
            this.saveTask();
        }

        private void CreateNew_Clicked(object sender, EventArgs e)
        {
            this.saveTask();
        }

        private async void CompletedTask_Clicked(object sender, EventArgs e)
        {

            string action = await DisplayActionSheet("Vui lòng chọn trạng thái công việc", "Đóng", null, "Hoàn Thành", "Hủy");
            if (action == "Hoàn Thành")
            {
                viewModel.TaskFormModel.statecode = 1; // update statecode completed
                viewModel.TaskFormModel.statuscode = 5;
                this.saveTask();
            }
            else if (action == "Hủy")
            {
                viewModel.TaskFormModel.statecode = 2; // update statecode canceled
                viewModel.TaskFormModel.statuscode = 6;
                this.saveTask();
            }
        }

        private async void CanceledTask_Clicked(object sender, EventArgs e)
        {
            bool check = await DisplayAlert("Thông báo", "Bạn có muốn hủy công việc này không ?", "Đồng Ý", "Không Đồng Ý");
            if (check == true)
            {
                viewModel.TaskFormModel.statecode = 2; // update statecode canceled
                viewModel.TaskFormModel.statuscode = 6;
                this.saveTask();
            }
        }
    }
}