using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.ViewModels
{
    public class EventFormViewModel : BaseViewModel
    {
        private EventFormModel _eventForm;
        public EventFormModel Event
        {
            get => _eventForm;
            set
            {
                _eventForm = value;
                OnPropertyChanged(nameof(Event));
            }
            
        }
    }
}
