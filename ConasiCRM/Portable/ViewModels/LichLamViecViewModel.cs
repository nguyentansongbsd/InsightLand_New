using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ConasiCRM.Portable.Controls;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class LichLamViecViewModel : BaseViewModel
    {
        public ObservableCollection<CalendarEvent> lstEvents { get; set; }
        public ObservableCollection<CalendarEvent> selectedDateEvents { get; set; }
        public ObservableCollection<Grouping<DateTime, CalendarEvent>> selectedDateEventsGrouped { get; set; }


        private DateTime? _selectedDate;
        public DateTime? selectedDate { get => _selectedDate; set { _selectedDate = value; DayLabel = ""; OnPropertyChanged(nameof(selectedDate)); } }

        private string _DayLabel;
        public string DayLabel
        {
            get
            {
                return _DayLabel;
            }
            set
            {
                if (this.selectedDate.HasValue)
                {
                    var date = selectedDate.Value;
                    var result = string.Format("{0}, {1} {2}", CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(date.DayOfWeek), CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(date.Month), date.Day);

                    if (Device.RuntimePlatform == Device.Android)
                    {
                        result = result.ToUpper();
                    }

                    _DayLabel = result;
                }
                OnPropertyChanged(nameof(DayLabel));
            }
        }

        public LichLamViecViewModel()
        {
            lstEvents = new ObservableCollection<CalendarEvent>();
            selectedDateEvents = new ObservableCollection<CalendarEvent>();
            selectedDateEventsGrouped = new ObservableCollection<Grouping<DateTime, CalendarEvent>>();

            selectedDate = DateTime.Today;
        }

        public void reset()
        {
            lstEvents.Clear();
            selectedDate = null;
            selectedDateEvents.Clear();
            selectedDateEventsGrouped.Clear();
        }

        public async Task loadAllActivities()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='activitypointer'>
                                <attribute name='subject' />
                                <attribute name='regardingobjectid' />
                                <attribute name='activitytypecode' />
                                <attribute name='statecode' />
                                <attribute name='scheduledstart' />
                                <attribute name='scheduledend' />
                                <attribute name='activityid' />
                                <attribute name='statuscode' />
                                <order attribute='scheduledstart' descending='false' />
                                <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer'>
                                    <attribute name='fullname'  alias='regardingobjectid_label_contact'/>
                                </link-entity>
                                <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='outer'>
                                    <attribute name='name'  alias='regardingobjectid_label_account'/>
                                </link-entity>
                                <link-entity name='lead' from='leadid' to='regardingobjectid' visible='false' link-type='outer'>
                                    <attribute name='fullname'  alias='regardingobjectid_label_lead'/>
                                </link-entity>
                                <filter type='and'>
                                  <condition attribute='isregularactivity' operator='eq' value='1' />
                                </filter>
                              </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListActivitiesAcc>>("activitypointers", fetch);
            //if (result == null)
            //{
            //    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", "Đã có lỗi xảy ra. Vui lòng thử lại sau.", "OK");
            //    await Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
            //}
            var data = result.value;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    lstEvents.Add(new CalendarEvent(item));
                }
            }
        }

        public void UpdateSelectedEvents(DateTime value)
        {
            this.selectedDateEvents.Clear();
            foreach (CalendarEvent item in this.lstEvents)
            {
                var date = value;
                var recurrenceRule = item.RecurrenceRule;
                if ((recurrenceRule == null && item.StartDate.CompareTo(date) >= 0 && item.StartDate.CompareTo(date.AddDays(1)) < 0) 
                    || (recurrenceRule == null && item.EndDate.CompareTo(date) >= 0 && item.EndDate.CompareTo(date.AddDays(1)) < 0) 
                    || (recurrenceRule == null && item.StartDate.CompareTo(date) < 0 && item.EndDate.CompareTo(date.AddDays(1)) >= 0) 
                    || (recurrenceRule != null && recurrenceRule.Pattern.GetOccurrences(date, date, date).Any()))
                {
                    this.selectedDateEvents.Add(item);
                }
            }
        }

        public void UpdateSelectedEventsForWeekView(DateTime value)
        {
            this.selectedDateEvents.Clear();
            this.selectedDateEventsGrouped.Clear();

            var dayOfWeek = (int)value.DayOfWeek;
            for(int i = 1; i < 8; i++)
            {
                var currentDayOfWeek = i;
                var balance = currentDayOfWeek - dayOfWeek;
                var currentDate = value.AddDays(balance);
                var checkHasValue = false;

                foreach (CalendarEvent item in this.lstEvents)
                {
                    var date = currentDate;
                    var recurrenceRule = item.RecurrenceRule;
                    if ((recurrenceRule == null && item.StartDate.CompareTo(date) >= 0 && item.StartDate.CompareTo(date.AddDays(1)) < 0)
                        || (recurrenceRule == null && item.EndDate.CompareTo(date) >= 0 && item.EndDate.CompareTo(date.AddDays(1)) < 0)
                        || (recurrenceRule == null && item.StartDate.CompareTo(date) < 0 && item.EndDate.CompareTo(date.AddDays(1)) >= 0)
                        || (recurrenceRule != null && recurrenceRule.Pattern.GetOccurrences(date, date, date).Any()))
                    {
                        CalendarEvent newItem = new CalendarEvent(item.Activity) { dateForGroupingWeek = date };
                        this.selectedDateEvents.Add(newItem);
                        checkHasValue = true;
                    }
                }

                if (!checkHasValue)
                {
                    selectedDateEvents.Add(new CalendarEvent() { dateForGroupingWeek = currentDate, activitytype_label = "No Activity" });
                }

            }

            var sorted = from eventCalendar in selectedDateEvents
                         group eventCalendar by eventCalendar.dateForGroupingWeek.Value into dateGrouped
                         select new Grouping<DateTime, CalendarEvent>(dateGrouped.Key, dateGrouped);
            this.selectedDateEventsGrouped = new ObservableCollection<Grouping<DateTime, CalendarEvent>>(sorted);
        }
    }
}
