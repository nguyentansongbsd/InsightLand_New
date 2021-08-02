using System;
using System.Collections.ObjectModel;
using ConasiCRM.Portable.Models;

namespace ConasiCRM.Portable.ViewModels
{
    public class CallLogViewModel : BaseViewModel
    {
        private ObservableCollection<CallLogModel> _CallLogs;
        public ObservableCollection<CallLogModel> CallLogs { get => _CallLogs;set { _CallLogs = value; OnPropertyChanged(nameof(CallLogs)); } }

        public CallLogViewModel()
        {
            CallLogs = new ObservableCollection<CallLogModel>();
        }
    }
}
