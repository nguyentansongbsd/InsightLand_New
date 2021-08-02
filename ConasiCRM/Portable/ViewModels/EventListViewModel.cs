using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class EventListViewModel : ListViewBaseViewModel2<EventListModel>
    {
        public string Keyword { get; set; }
        public EventListViewModel()
        {          
            PreLoadData = new Command(() =>
            {
                EntityName = "bsd_events";
                FetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='15' page='{Page}' >
                <entity name='bsd_event'>
                <attribute name='bsd_name' />
                <attribute name='createdon' />
                <attribute name='statuscode' />
                <attribute name='bsd_startdate' />
                <attribute name='bsd_project' />
                <attribute name='bsd_phaselaunch' />
                <attribute name='bsd_eventcode' />
                <attribute name='bsd_enddate' />
                <attribute name='bsd_description' />
                <attribute name='bsd_projectname' />
                <attribute name='bsd_eventid' />
                <order attribute='createdon' descending='true' />
                <filter type='and'>
                   <condition attribute='bsd_name' operator='like' value='%{Keyword}%' />
                </filter>
                <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaselaunch' visible='false' link-type='outer' alias='phaseslaunch'>
                    <attribute name='bsd_name' alias='bsd_phaseslaunch_name'/>
                </link-entity>
                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='project'>
                    <attribute name='bsd_name' alias='bsd_project_name'/>
                </link-entity>  
                </entity>
            </fetch>";
            });
        }
    }

}
