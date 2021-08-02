using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class ReservationListViewModel : ListViewBaseViewModel2<ReservationListModel>
    {
        public string Keyword { get; set; }
        public ReservationListViewModel()
        {            
            PreLoadData = new Command(() =>
            {
                EntityName = "quotes";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='quote'>
                <attribute name='name' />
                <attribute name='quotenumber' />
                <attribute name='totalamount' />
                <attribute name='statecode' />
                <attribute name='customerid' />
                <attribute name='bsd_unitno' alias='bsd_unitno_id' />
                <attribute name='statuscode' />
                <attribute name='bsd_reservationno' />
                <attribute name='bsd_projectid' alias='bsd_project_id' />
                <attribute name='bsd_quotationnumber' />
                <attribute name='quoteid' />
                <order attribute='createdon' descending='true' />
                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' visible='false' link-type='outer' alias='a'>
                  <attribute name='bsd_name' alias='bsd_projectid_name' />
                </link-entity>
                <link-entity name='product' from='productid' to='bsd_unitno' visible='false' link-type='outer' alias='b'>
                  <attribute name='name' alias='bsd_unitno_name' />
                </link-entity>
                <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='c'>
                  <attribute name='bsd_name' alias='purchaser_accountname' />
                </link-entity>
                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='d'>
                  <attribute name='bsd_fullname' alias='purchaser_contactname' />
                </link-entity>
                <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaseslaunchid' visible='false' link-type='outer' alias='e'>
                  <attribute name='bsd_name' alias='phaseslaunch_name' />
                </link-entity>
                <link-entity name='bsd_paymentscheme' from='bsd_paymentschemeid' to='bsd_paymentscheme' visible='false' link-type='outer' alias='a_8524eae1b214e911a97f000d3aa04914'>
                      <attribute name='bsd_name'  alias='paymentscheme_name' />
                    </link-entity>
                <filter type='and'>
                   <condition attribute='customeridname' operator='like' value='%{Keyword}%' />
                </filter>
              </entity>
            </fetch>";
            });
        }
    }
}
