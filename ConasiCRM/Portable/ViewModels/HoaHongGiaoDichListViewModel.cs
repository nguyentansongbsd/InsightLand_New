using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class HoaHongGiaoDichListViewModel : ListViewBaseViewModel2<HoaHongGiaoDichListModel>
    {
        private decimal _totalHoaHong;
        public decimal totalHoaHong
        {
            get => _totalHoaHong;
            set
            {
                if (_totalHoaHong != value)
                {
                    _totalHoaHong = value;
                    OnPropertyChanged(nameof(totalHoaHong));
                }
            }
        }
        private decimal _totalHoaHongNhan;
        public decimal totalHoaHongNhan
        {
            get => _totalHoaHongNhan;
            set
            {
                if (_totalHoaHongNhan != value)
                {
                    _totalHoaHongNhan = value;
                    OnPropertyChanged(nameof(totalHoaHongNhan));
                }
            }
        }
        private decimal _totalHoaHongConLai;
        public decimal totalHoaHongConLai
        {
            get => _totalHoaHongConLai;
            set
            {
                if (_totalHoaHongConLai != value)
                {
                    _totalHoaHongConLai = value;
                    OnPropertyChanged(nameof(totalHoaHongConLai));
                }
            }
        }

        public HoaHongGiaoDichListViewModel()
        {
            PreLoadData = new Command(() =>
            {
                EntityName = "bsd_commissiontransactions";
                FetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' count='15' page='{Page}'>
              <entity name='bsd_commissiontransaction'>
                <all-attributes/>
                <order attribute='bsd_name' descending='false' />
                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='project'>
                  <attribute name='bsd_name' alias='project_bsd_name'/>
                </link-entity>
                <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='products'>
                  <attribute name='name' alias='products_name'/>
                </link-entity>
                <link-entity name='account' from='accountid' to='bsd_saleagentcompany' visible='false' link-type='outer' alias='accounts'>
                  <attribute name='bsd_name' alias='accounts_bsd_name'/>
                </link-entity>
                <link-entity name='quote' from='quoteid' to='bsd_reservation' visible='false' link-type='outer' alias='quotes'>
                  <attribute name='name' alias='quotes_name' />
                </link-entity>
              </entity>
            </fetch>";
            });
        }
    }
}
