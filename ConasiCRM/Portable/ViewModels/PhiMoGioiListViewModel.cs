using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class PhiMoGioiListViewModel : ListViewBaseViewModel2<PhiMoGioiListModel>
    {
        public string Keyword { get; set; }
        public PhiMoGioiListViewModel()
        {           
            PreLoadData = new Command(() =>
            {
                EntityName = "bsd_brokeragefeeses";
                FetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='15' page='{Page}'>
                            <entity name='bsd_brokeragefees'>
                              <all-attributes/>
                              <order attribute='createdon' descending='false' />
                              <filter type='and'>
                                  <condition attribute='bsd_name' operator='like' value='%{Keyword}%' />
                               </filter>
                              <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='project'>
                                <attribute name='bsd_name' alias='project_bsd_name'/>
                              </link-entity>                           
                            </entity>
                          </fetch>";
            });
        }
    }
}
