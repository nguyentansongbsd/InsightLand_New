using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{   
    public class LeadsContentViewViewModel : ListViewBaseViewModel2<LeadListModel>
    {
        public string Keyword { get; set; }

        public LeadsContentViewViewModel()
        {                   
            PreLoadData = new Command(() =>
            {
                string filter = string.Empty;
                if (!string.IsNullOrWhiteSpace(Keyword))
                {
                    filter = $@"<condition attribute='lastname' operator='like' value='%{Keyword}%' />";
                }
                EntityName = "leads";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                      <entity name='lead'>
                        <attribute name='lastname' />
                        <attribute name='subject' />
                        <attribute name='mobilephone'/>
                        <attribute name='emailaddress1' />
                        <attribute name='createdon' />
                        <attribute name='leadid' />
                        <attribute name='leadqualitycode' />
                        <order attribute='createdon' descending='true' />
                        <filter type='and'>
                             <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                             '" + filter + @"'   
                        </filter>
                      </entity>
                    </fetch>";
            });
        }
    }
}
