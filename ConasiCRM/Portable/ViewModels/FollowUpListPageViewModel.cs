using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class FollowUpListPageViewModel : ListViewBaseViewModel2<FollowUpListPageModel>
    {
        public string Keyword { get; set; }
        public FollowUpListPageViewModel()
        {
            PreLoadData = new Command(() =>
            {
                EntityName = "bsd_followuplists";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='bsd_followuplist'>
                                <attribute name='bsd_name' />
                                <attribute name='createdon' />
                                <attribute name='bsd_units' />
                                <attribute name='statuscode' />
                                <attribute name='bsd_reservation' />
                                <attribute name='bsd_optionentry' />
                                <attribute name='bsd_expiredate' />
                                <attribute name='bsd_date' />
                                <attribute name='bsd_type' />
                                <attribute name='bsd_installment' />
                                <attribute name='bsd_project' />
                                <attribute name='bsd_group' />
                                <attribute name='bsd_followuplistcode' />
                                <attribute name='bsd_followuplistid' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                  <condition attribute='bsd_name' operator='like' value='%{Keyword}%' />
                                </filter>
                                <link-entity name='quote' from='quoteid' to='bsd_reservation' visible='false' link-type='outer' alias='a_9fe1c29b064be61180ea3863bb367d40'>
                                  <attribute name='customerid' alias='customerid_quote'/>
                                </link-entity>
                                <link-entity name='salesorder' from='salesorderid' to='bsd_optionentry' visible='false' link-type='outer' alias='a_2ec267f5064be61180ea3863bb367d40'>
                                  <attribute name='customerid' />
                                </link-entity>
                                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='inner' alias='aa'>
                                  <attribute name='bsd_name' alias='bsd_name_project' />
                                </link-entity>
                                <link-entity name='product' from='productid' to='bsd_units' link-type='inner' alias='ac'>
                                  <attribute name='name' alias='name_unit'/>
                                </link-entity>
                              </entity>
                            </fetch>";
            });
        }
    }
}
