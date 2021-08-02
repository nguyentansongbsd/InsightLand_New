using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ConasiCRM.Portable.Models;

namespace ConasiCRM.Portable.ViewModels
{
    public class SMSViewModel : BaseViewModel
    {
        public ObservableCollection<SMSModel> lstAllSMS { get; set; }
        public ObservableCollection<SMSGroupedModel> lstGroupSMS { get; set; }

        public SMSViewModel()
        {
            lstAllSMS = new ObservableCollection<SMSModel>();
            lstGroupSMS = new ObservableCollection<SMSGroupedModel>();
        }

        public async Task GroupSMS()
        {
            foreach(var s in lstAllSMS)
            {
                if(lstGroupSMS.ToList().FirstOrDefault(x => x.phone == s.phone) == null)
                {
                    var tmp = new SMSGroupedModel()
                    {
                        phone = s.phone,
                        lastSMSTime = s.time,
                        lastSMSContent = s.msg,
                        lstSMS = new List<SMSModel>(),
                    };

                    tmp.addSMS(s);
                    if(s.name != null)
                    {
                        tmp.name = s.name;
                    }
                    lstGroupSMS.Add(tmp);
                }
                else
                {
                    var tmp = lstGroupSMS.ToList().FirstOrDefault(x => x.phone == s.phone);

                    tmp.addSMS(s);
                    if(tmp.lastSMSTime.CompareTo(s.time) < 0)
                    {
                        tmp.lastSMSTime = s.time;
                        tmp.lastSMSContent = s.msg;
                    }
                    if (tmp.name == null && s.name != null)
                    {
                        tmp.name = s.name;
                    }
                }
            }
            var result = lstGroupSMS.OrderByDescending(x => x.lastSMSTime).ToList();
            lstGroupSMS.Clear();
            foreach(var t in result)
            {
                t.lstSMS = t.lstSMS.OrderBy(x => x.time).ToList();
                lstGroupSMS.Add(t);
            }

        }
    }
}
