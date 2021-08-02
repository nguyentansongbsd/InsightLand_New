using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Common;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class ContactListViewModel : ListViewBaseViewModel2<ContactListModel>
    {
        public string Keyword { get; set; }
        public ContactListViewModel()
        {
            PreLoadData = new Command(() =>
            {
                EntityName = "contacts";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='contact'>
                    <attribute name='bsd_fullname' />
                    <attribute name='mobilephone' />
                    <attribute name='birthdate' />
                    <attribute name='emailaddress1' />
                    <attribute name='bsd_diachithuongtru' />
                    <attribute name='createdon' />
                    <attribute name='contactid' />
                    <order attribute='fullname' descending='false' />
                    <filter type='and'>
                      <condition attribute='bsd_fullname' operator='like' value='%{Keyword}%' />
                    </filter>
                  </entity>
                </fetch>";
            });
        }
    }
}
