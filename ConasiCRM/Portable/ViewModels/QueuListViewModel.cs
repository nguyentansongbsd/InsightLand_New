using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using ConasiCRM.Portable.Settings;
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
    public class QueuListViewModel : ListViewBaseViewModel2<QueuesModel>
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
                        <attribute name='name'/>
                        <attribute name='statuscode' />
                        <attribute name='bsd_project' />
                        <attribute name='opportunityid' />
                        <attribute name='bsd_queuenumber' />
                        <attribute name='bsd_queuingexpired' />
                        <attribute name='createdon' />
                        <order attribute='createdon' descending='true' />
                        <filter type='and'>                          
                            <filter type='or'>
                                <condition attribute='name' operator='like' value='%25{Keyword}%25' />
                                <condition attribute='customeridname' operator='like' value='%25{Keyword}%25' />
                                <condition attribute='bsd_queuenumber' operator='like' value='%25{Keyword}%25' />
                            </filter>
                          <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}'/>
                        </filter>
                        <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer'>
                           <attribute name='fullname'  alias='contact_name'/>
                        </link-entity>
                        <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer'>
                           <attribute name='name'  alias='account_name'/>
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
