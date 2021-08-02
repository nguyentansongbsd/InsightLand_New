using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telerik.XamarinForms.Common;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class QueuListViewModel : ListViewBaseViewModel2<QueueListModel>
    {
        public string Keyword { get; set; }
        public ICommand PhoneCommand { get; }
        public QueuListViewModel()
        {
            PhoneCommand = new Command<string>(PhoneCommandAsync);
            PreLoadData = new Command(() =>
            {
                EntityName = "opportunities";
                FetchXml = $@"<fetch version='1.0' count='15' page='{Page}' output-format='xml-platform' mapping='logical' distinct='false'>
                  <entity name='opportunity'>
                    <attribute name='name' />
                    <attribute name='statecode' />
                    <attribute name='customerid' alias='customer_id'/>
                    <attribute name='emailaddress' />
                    <attribute name='bsd_queuenumber' />
                    <attribute name='statuscode' />
                    <attribute name='bsd_queuingfee' />
                    <attribute name='bsd_project' alias='project_id' />
                    <attribute name='bsd_phaselaunch' />
                    <attribute name='createdon' />
                    <attribute name='bsd_queuingexpired' />
                    <attribute name='statuscode' />
                    <attribute name='opportunityid' />                    
                    <order attribute='createdon' descending='true' />
                    <filter type='and'>
                      <condition attribute='name' operator='like' value='%{Keyword}%' />
                    </filter>
                    <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='a'>
                      <attribute name='bsd_name' alias='account_name' />
                      <attribute name='telephone1' alias='telephone' />
                    </link-entity>
                    <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='b'>
                      <attribute name='bsd_fullname'  alias='contact_name'/>
                    </link-entity>
                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='c'>
                      <attribute name='bsd_name' alias='project_name' />
                    </link-entity>
                    <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='a_b088efffb214e911a97f000d3aa04914'>
                        <attribute name='name' alias='unit_name' />
                    </link-entity>
                  </entity>
                </fetch>";
            });
        }

        private async void PhoneCommandAsync(string phone)
        {
            var checkVadate = PhoneNumberFormatVNHelper.CheckValidate(phone);
            if (checkVadate == true)
            {
                await Launcher.OpenAsync($"tel:{phone}");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Lỗi", "Không có số điện thoại", "OK");
            }
        }
    }
}
