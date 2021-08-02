using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    class AccountContentViewViewModel : ListViewBaseViewModel2<AccountListModel>
    {
        public string Keyword { get; set; }
        public AccountContentViewViewModel()
        {
            PreLoadData = new Command(() =>
            {
                EntityName = "accounts";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='account'>
                    <attribute name='bsd_name' />
                    <attribute name='telephone1' />
                    <attribute name='accountid' />
                    <attribute name='bsd_address' />
                    <attribute name='bsd_companycode' />
                    <attribute name='bsd_registrationcode' />
                    <attribute name='bsd_vatregistrationnumber' />
                    <order attribute='createdon' descending='true' />
                    <filter type='and'>
                      <condition attribute='name' operator='like' value='%{Keyword}%' />
                    </filter>
                    <link-entity name='contact' from='contactid' to='primarycontactid' visible='false' link-type='outer' alias='a'>
                         <attribute name='bsd_fullname' alias='primarycontact_name' />
                    </link-entity>
                  </entity>
                </fetch>";
            });
        }
    }
}
