using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ConasiCRM.Portable.Models;

namespace ConasiCRM.Portable.Controls
{
    public interface IAccessService
    {
        ObservableCollection<CallLogModel> GetCallLogs();
        ObservableCollection<SMSModel> GetSMS();
    }
}
