using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class TaskFormViewModel : FormViewModal
    {
        public bool FocusTimePickerStart = false;
        public bool FocusTimePickerEnd = false;
        public bool FocusDateTimeStart = false;
        public bool FocusDateTimeEnd = false;
        public TaskFormModel _taskFormModel;
        public TaskFormModel TaskFormModel
        {
            get => this._taskFormModel;
            set
            {
                _taskFormModel = value;
                OnPropertyChanged(nameof(TaskFormModel));
            }
        }
        public LookUpConfig ContactLookUpConfig { get; set; }
        public LookUpConfig AccountLookUpConfig { get; set; }
        public LookUpConfig LeadLookUpConfig { get; set; }
        public ObservableCollection<OptionSet> list_picker_durations { get; set; }
        public TaskFormViewModel()
        {

            list_picker_durations = new ObservableCollection<OptionSet>()
            {
                new OptionSet(){Val = "1", Label = "1 phút"},
                new OptionSet(){Val = "15", Label = "15 phút"},
                new OptionSet(){Val = "30", Label = "30 phút"},
                new OptionSet(){Val = "45", Label = "45 phút"},
                new OptionSet(){Val = "60", Label = "1 giờ"},
                new OptionSet(){Val = "90", Label = "1.5 giờ"},
                new OptionSet(){Val = "120", Label = "2 giờ"},
                new OptionSet(){Val = "150", Label = "2.5 giờ"},
                new OptionSet(){Val = "180", Label = "3 giờ"},
                new OptionSet(){Val = "210", Label = "3.5 giờ"},
                new OptionSet(){Val = "240", Label = "4 giờ"},
                new OptionSet(){Val = "270", Label = "4.5 giờ"},
                new OptionSet(){Val = "300", Label = "5 giờ"},
                new OptionSet(){Val = "330", Label = "5.5 giờ"},
                new OptionSet(){Val = "360", Label = "6 giờ"},
                new OptionSet(){Val = "390", Label = "6.5 giờ"},
                new OptionSet(){Val = "420", Label = "7 giờ"},
                new OptionSet(){Val = "450", Label = "7.5 giờ"},
                new OptionSet(){Val = "480", Label = "8 giờ"},
                new OptionSet(){Val = "1440", Label = "1 ngày"},
                new OptionSet(){Val = "2880", Label = "2 ngày"},
                new OptionSet(){Val = "4320", Label = "3 ngày"},
            };
            ContactLookUpConfig = new LookUpConfig()
            {
                FetchXml = @"<fetch version='1.0' count='15' page='{0}' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='contactid' alias='Id' />
                    <attribute name='fullname' alias='Name' />
                    <attribute name='createdon' alias='Detail' />
                    <order attribute='fullname' descending='false' />
                  </entity>
                </fetch>",
                EntityName = "contacts",
                PropertyName = "Contact"
            };

            AccountLookUpConfig = new LookUpConfig()
            {
                FetchXml = @"<fetch version='1.0' count='15' page='{0}' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='account'>
                    <attribute name='accountid' alias='Id' />
                    <attribute name='name' alias='Name' />
                    <attribute name='createdon' alias='Detail' />
                    <order attribute='name' descending='false' />
                  </entity>
                </fetch>",
                EntityName = "accounts",
                PropertyName = "Account"
            };

            LeadLookUpConfig = new LookUpConfig()
            {
                FetchXml = @"<fetch version='1.0' page='{0}' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='lead'>
                                <attribute name='fullname' alias='Name'/>
                                <attribute name='createdon' alias='Detail'/>
                                <attribute name='leadid' alias='Id'/>
                                <order attribute='createdon' descending='true' />
                              </entity>
                            </fetch>",
                EntityName = "leads",
                PropertyName = "Lead"
            };
        }
        public OptionSet setSelectedTime(string id)
        {
            return list_picker_durations.FirstOrDefault(x => x.Val == id);
        }
    }
}
