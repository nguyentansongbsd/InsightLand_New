using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class ListPhanHoiViewModel : ListViewBaseViewModel2<ListPhanHoiModel>
    {
        public string Keyword { get; set; }
        public ListPhanHoiViewModel()
        {
            PreLoadData = new Command(() =>
            {
                EntityName = "incidents";
                FetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='15' page='{Page}'>
                            <entity name='incident'>
                                <all-attributes/>
                                <order attribute='createdon' descending='true' />
                                <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer'>
                                <attribute name='bsd_name' alias='case_nameaccount'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' >
                                  <attribute name='bsd_fullname' alias='case_namecontact'/>
                                </link-entity>
                                <link-entity name='product' from='productid' to='productid' visible='false' link-type='outer' >
                                  <attribute name='name' alias='productname'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='primarycontactid' visible='false' link-type='outer'>
                                  <attribute name='fullname' alias='contactname'/>
                                </link-entity>
                                <link-entity name='contract' from='contractid' to='contractid' visible='false' link-type='outer' alias='contracts'>
                                  <attribute name='title' alias='contractname'/>
                                </link-entity>
                                <link-entity name='subject' from='subjectid' to='subjectid' visible='false' link-type='outer' >
                                  <attribute name='title' alias='subjecttitle'/>
                                </link-entity>
                                <filter type='and'>
                                  <condition attribute='title' operator='like' value='%{Keyword}%' />
                                </filter>
                                </entity>
                            </fetch>";
            });
        }
    }
}

