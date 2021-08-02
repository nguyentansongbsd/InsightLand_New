using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class PhiMoGioiGiaoDichListViewModel : ListViewBaseViewModel2<PhiMoGioiGiaoDichListModel>

    {
        private decimal _totalPMG;
        public decimal totalPMG
        {
            get => _totalPMG;
            set
            {
                if (_totalPMG != value)
                {
                    _totalPMG = value;
                    OnPropertyChanged(nameof(totalPMG));
                }
            }
        }
        private decimal _totalPMGNhan;
        public decimal totalPMGNhan
        {
            get => _totalPMGNhan;
            set
            {
                if (_totalPMGNhan != value)
                {
                    _totalPMGNhan = value;
                    OnPropertyChanged(nameof(totalPMGNhan));
                }
            }
        }
        private decimal _totalPMGConLai;
        public decimal totalPMGConLai
        {
            get => _totalPMGConLai;
            set
            {
                if (_totalPMGConLai != value)
                {
                    _totalPMGConLai = value;
                    OnPropertyChanged(nameof(totalPMGConLai));
                }
            }
        }
        public PhiMoGioiGiaoDichListViewModel()
        {
            PreLoadData = new Command(() =>
            {
                EntityName = "bsd_brokeragetransactions";
                FetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='15' page='{Page}'>
                <entity name='bsd_brokeragetransaction'>
                    <all-attributes/>
                    <order attribute='createdon' descending='false' />
                    <filter type='and'>
                    </filter>
                    <link-entity name='quote' from='quoteid' to='bsd_reservation' visible='false' link-type='outer' alias='quote'>
                      <attribute name='name' alias='quote_name'/>
                    </link-entity>
                    <link-entity name='bsd_brokeragefees' from='bsd_brokeragefeesid' to='bsd_brokeragefees' visible='false' link-type='outer' alias='brokeragefees'>
                      <attribute name='bsd_name' alias='brokeragefees_name'/>
                    </link-entity>
                  <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='product'>
                  <attribute name='name' alias='product_name'/>
                </link-entity>
                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer'>
                <attribute name='bsd_name' alias='project_bsd_name'/> 
                </link-entity>
                <link-entity name='account' from='accountid' to='bsd_customer' visible='false' link-type='outer' >
                <attribute name='bsd_name' alias='account_bsd_name'/>
                </link-entity>
                <link-entity name='contact' from='contactid' to='bsd_customer' visible='false' link-type='outer' >
                <attribute name='bsd_fullname' alias='contact_bsd_fullname'/>
                </link-entity>
                <link-entity name='contact' from='contactid' to='bsd_collaborator' visible='false' link-type='outer' > 
                <attribute name='bsd_fullname' alias='sales_name'/>
                </link-entity>
                </entity>
            </fetch>";
            });
        }
    }
}
