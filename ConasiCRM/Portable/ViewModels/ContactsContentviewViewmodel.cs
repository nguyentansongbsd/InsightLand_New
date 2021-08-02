using System;
using ConasiCRM.Portable.Models;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class ContactsContentviewViewmodel : ListViewBaseViewModel2<ContactListModel>
    {
        public string Keyword { get; set; }
        public ContactsContentviewViewmodel()
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
