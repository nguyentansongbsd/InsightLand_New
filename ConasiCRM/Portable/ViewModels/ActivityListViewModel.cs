using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class ActivityListViewModel : ListViewBaseViewModel2<HoatDongListModel>
    {
        public string Keyword { get; set; }

        public ActivityListViewModel()
        {
            PreLoadData = new Command(() =>
            {
                EntityName = "activitypointers";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='true'>
                                  <entity name='activitypointer'>
                                    <attribute name='activitytypecode' />
                                    <attribute name='subject' />
                                    <attribute name='statecode' />
                                    <attribute name='prioritycode' />
                                    <attribute name='modifiedon' />
                                    <attribute name='activityid' />
                                    <attribute name='instancetypecode' />
                                    <attribute name='community' />
                                    <attribute name='regardingobjectid' />
                                    <attribute name='scheduledstart' />
                                    <attribute name='scheduledend' />
                                    <attribute name='createdon' />
                                    <attribute name='actualdurationminutes' />
                                    <attribute name='description' />
                                    <order attribute='modifiedon' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='activitytypecode' operator='in'>
                                        <value>4210</value>
                                        <value>4201</value>
                                        <value>4212</value>
                                      </condition>
                                    </filter>
                                    <filter type='and'>
                                        <condition attribute='subject' operator='like' value='%{Keyword}%' />
                                     </filter>
                                    <link-entity name='account' from='accountid' to='regardingobjectid' link-type='outer' alias='ae'>
                                        <attribute name='bsd_name' alias='accounts_bsd_name'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='regardingobjectid' link-type='outer' alias='af'>
                                        <attribute name='fullname' alias='contact_bsd_fullname'/>
                                    </link-entity>
                                    <link-entity name='lead' from='leadid' to='regardingobjectid' link-type='outer' alias='ag'>
                                        <attribute name='fullname' alias='lead_fullname'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            });
        }
    }
}
