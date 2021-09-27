using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class BangTinhGiaDetailPageViewModel : BaseViewModel
    {
        private ReservationDetailPageModel _reservation;
        public ReservationDetailPageModel Reservation { get => _reservation; set { _reservation = value; OnPropertyChanged(nameof(Reservation)); } }

        public OptionSet _customer;
        public OptionSet Customer { get => _customer; set { _customer = value; OnPropertyChanged(nameof(Customer)); } }
        public ObservableCollection<ReservationCoownerModel> CoownerList { get; set; }

        private bool _showMoreNguoiDongSoHuu;
        public bool ShowMoreNguoiDongSoHuu { get => _showMoreNguoiDongSoHuu; set { _showMoreNguoiDongSoHuu = value; OnPropertyChanged(nameof(ShowMoreNguoiDongSoHuu)); } }
        public int PageNguoiDongSoHuu { get; set; } = 1;
        public ObservableCollection<ReservationInstallmentDetailPageModel> InstallmentList { get; set; }

        private int _numberInstallment;
        public int NumberInstallment { get => _numberInstallment; set { _numberInstallment = value; OnPropertyChanged(nameof(NumberInstallment)); } }

        public BangTinhGiaDetailPageViewModel()
        {
            CoownerList = new ObservableCollection<ReservationCoownerModel>();
            InstallmentList = new ObservableCollection<ReservationInstallmentDetailPageModel>();
            Reservation = new ReservationDetailPageModel();
            Customer = new OptionSet();
        }

        #region Chinh Sach
        public async Task LoadReservation(Guid ReservationId)
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='quote'>
                                    <attribute name='name' />
                                    <attribute name='statecode' />
                                    <attribute name='statuscode' />
                                    <attribute name='quoteid' />
                                    <attribute name='bsd_reservationno' />
                                    <attribute name='quotenumber' />
                                    <attribute name='bsd_numberofmonthspaidmf' />
                                    <attribute name='bsd_waivermanafeemonth' />
                                    <attribute name='bsd_managementfee' />
                                    <attribute name='bsd_rejectdate' />
                                    <attribute name='bsd_rejectreason' />
                                    <attribute name='bsd_salesdepartmentreceiveddeposit' />
                                    <attribute name='bsd_receiptdate' />
                                    <attribute name='bsd_depositfeereceived' />
                                    <attribute name='bsd_calculatedforsalesreport' />
                                    <attribute name='bsd_detailamount' />
                                    <attribute name='bsd_discount' />
                                    <attribute name='bsd_packagesellingamount' />
                                    <attribute name='bsd_totalamountlessfreight' />
                                    <attribute name='bsd_landvaluededuction' />
                                    <attribute name='totaltax' />
                                    <attribute name='bsd_freightamount' />
                                    <attribute name='totalamount' />
                                    <attribute name='bsd_constructionarea' />
                                    <attribute name='bsd_followuplist' />
                                    <attribute name='bsd_unitstatus' />
                                    <attribute name='bsd_netusablearea' />
                                    <attribute name='bsd_actualarea' alias='unit_bsd_actualarea'/>
                                    <attribute name='bsd_bookingfee' />
                                    <attribute name='bsd_depositfee' />
                                    <attribute name='bsd_contracttypedescripton' />
                                    <attribute name='bsd_totalamountpaid' />
                                    <attribute name='bsd_quotationprinteddate' />
                                    <attribute name='bsd_expireddateofsigningqf' />
                                    <attribute name='bsd_quotationsigneddate' />
                                    <attribute name='bsd_reservationtime' />
                                    <attribute name='bsd_deposittime' />
                                    <attribute name='bsd_nameofstaffagent' />
                                    <attribute name='bsd_referral' />
                                    <attribute name='bsd_reservationformstatus' />
                                    <attribute name='bsd_reservationprinteddate' />
                                    <attribute name='bsd_signingexpired' />
                                    <attribute name='bsd_rfsigneddate' />
                                    <attribute name='bsd_reservationuploadeddate' />
                                    <order attribute='createdon' descending='true' />
                                    <link-entity name='account' from='accountid' to='customerid' link-type='outer' alias='aa'>
      	                                <attribute name='bsd_name' alias='purchaser_account_name'/>
    	                                <attribute name='accountid' alias='purchaser_accountid'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='customerid' link-type='outer' alias='ab'>
     	                                <attribute name='bsd_fullname' alias='purchaser_contact_name'/>
    	                                <attribute name='contactid' alias='purchaser_contactid'/>
                                    </link-entity>
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' link-type='outer' alias='ac'>
                                        <attribute name='bsd_name' alias='project_name'/>
                                        <attribute name='bsd_projectid' alias='project_id' />
                                    </link-entity>
                                    <link-entity name='pricelevel' from='pricelevelid' to='pricelevelid' link-type='outer' alias='ad'>
                                        <attribute name='pricelevelid' alias='pricelevel_id_apply'/>
                                        <attribute name='name' alias='pricelevel_name_apply'/>
                                    </link-entity>
                                    <link-entity name='pricelevel' from='pricelevelid' to='bsd_pricelistphaselaunch' link-type='outer' alias='ae'>
                                        <attribute name='pricelevelid' alias='pricelevel_id_phaseslaunch'/>
                                        <attribute name='name' alias='pricelevel_name_phaseslaunch'/>
                                    </link-entity>
                                    <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaseslaunchid' link-type='inner' alias='ak'>
                                        <attribute name='bsd_phaseslaunchid' alias='phaseslaunch_id'/>
                                        <attribute name='bsd_name' alias='phaseslaunch_name'/>
                                    </link-entity>
                                    <link-entity name='bsd_taxcode' from='bsd_taxcodeid' to='bsd_taxcode' link-type='outer' alias='af'>
                                        <attribute name='bsd_taxcodeid' alias='taxcode_id'/>
                                        <attribute name='bsd_name' alias='taxcode_name'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='bsd_salessgentcompany' link-type='outer' alias='ag'>
                                        <attribute name='bsd_name' alias='salescompany_account_name'/>
                                        <attribute name='accountid' alias='salescompany_accountid'/>
                                    </link-entity>
                                    <link-entity name='opportunity' from='opportunityid' to='opportunityid' link-type='outer' alias='al'>
                                        <attribute name='name' alias='queue_name' />
                                        <attribute name='opportunityid' alias='queue_id' />
                                    </link-entity>
                                    <filter type='and'>
	                                    <condition attribute='quoteid' operator='eq' uitype='quote' value='" + ReservationId + @"' />
	                                </filter>
                                  </entity>
                                </fetch>";
            // join với contact + account de lấy name.
            // join với product để lấy unit name, unit status có sẵn trên quote.
            // join với queue để lấy mã phiếu đặt chô
            // joi project, dot mo ban, bang gia, lich thanh toan
            // join voi bsd_taxcode de lay phan tram thue.
            // join voi discount list dể lấy tên chiết khấu và mã, dugnf để lấy danh sách discounts..

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationDetailPageModel>>("quotes", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                return;
            }
            Reservation = result.value.SingleOrDefault();
            if(!string.IsNullOrEmpty(Reservation.purchaser_account_name))
            {
                Customer.Val = Reservation.purchaser_accountid.ToString();
                Customer.Label = Reservation.purchaser_account_name;
            }
            else
            {
                Customer.Val = Reservation.purchaser_contactid.ToString();
                Customer.Label = Reservation.purchaser_contact_name;
            }
        }

        public async Task LoadCoOwners(Guid ReservationId)
        {
            string xml = $@"<fetch version='1.0' count = '3' page = '{PageNguoiDongSoHuu}' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='bsd_coowner'>
                <attribute name='bsd_coownerid' />
                <attribute name='bsd_name' />
                <order attribute='bsd_name' descending='false' />
                <link-entity name='account' from='accountid' to='bsd_customer' visible='false' link-type='outer' alias='a_1324f6d5b214e911a97f000d3aa04914'>
                  <attribute name='bsd_name' alias='account_name' />
                </link-entity>
                <link-entity name='contact' from='contactid' to='bsd_customer' visible='false' link-type='outer' alias='a_6b0d05eeb214e911a97f000d3aa04914'>
                  <attribute name='bsd_fullname' alias='contact_name' />
                </link-entity>
                 <filter type='and'>
                      <condition attribute='bsd_reservation' operator='eq' uitype='quote' value='{ReservationId}' />
                  </filter>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationCoownerModel>>("bsd_coowners", xml);
            if (result != null)
            {
                var data = result.value;

                if (data.Count <= 3)
                {
                    ShowMoreNguoiDongSoHuu = false;
                }
                else
                {
                    ShowMoreNguoiDongSoHuu = true;
                }

                foreach (var x in result.value)
                {
                    if (!string.IsNullOrEmpty(x.account_name))
                    {
                        x.customer = x.account_name;
                    }
                    else
                    {
                        x.customer = x.contact_name;
                    }
                    CoownerList.Add(x);
                }
            }
        }

        #endregion

        #region Lich
        public async Task LoadInstallmentList(Guid ReservationId)
        {
            string xml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='bsd_paymentschemedetail'>
                <attribute name='bsd_paymentschemedetailid' />
                <attribute name='bsd_name' />
                <attribute name='bsd_duedate' />
                <attribute name='statuscode' />
                <attribute name='bsd_amountofthisphase' />
                <attribute name='bsd_amountwaspaid' />
                <attribute name='bsd_depositamount' />
                <order attribute='bsd_ordernumber' descending='false' />
                <filter type='and'>
                  <condition attribute='bsd_reservation' operator='eq' uitype='quote' value='{ReservationId}' />
                </filter>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationInstallmentDetailPageModel>>("bsd_paymentschemedetails", xml);
            if (result == null || result.value.Count == 0)
                return;

            foreach (var x in result.value)
            { 
                InstallmentList.Add(x);
            }
            NumberInstallment = InstallmentList.Count();
        }
        #endregion
    }
}
