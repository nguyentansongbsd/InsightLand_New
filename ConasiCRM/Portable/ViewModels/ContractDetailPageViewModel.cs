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
    public class ContractDetailPageViewModel : BaseViewModel
    {
        private ContractModel _contract;
        public ContractModel Contract { get => _contract; set { _contract = value; OnPropertyChanged(nameof(Contract)); } }
        public ObservableCollection<ReservationCoownerModel> CoownerList { get; set; }
        public ObservableCollection<ReservationInstallmentDetailPageModel> InstallmentList { get; set; }

        private int _numberInstallment;
        public int NumberInstallment { get => _numberInstallment; set { _numberInstallment = value; OnPropertyChanged(nameof(NumberInstallment)); } }

        private bool _showInstallmentList;
        public bool ShowInstallmentList { get => _showInstallmentList; set { _showInstallmentList = value; OnPropertyChanged(nameof(ShowInstallmentList)); } }

        public ContractDetailPageViewModel()
        {
            Contract = new ContractModel();
            CoownerList = new ObservableCollection<ReservationCoownerModel>();
            InstallmentList = new ObservableCollection<ReservationInstallmentDetailPageModel>();
        }

        public async Task LoadContract(Guid ContractId)
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='salesorder'>
                                    <attribute name='salesorderid' />
                                    <attribute name='ordernumber' />
                                    <attribute name='bsd_optionno' />
                                    <attribute name='customerid' />
                                    <attribute name='name' alias='salesorder_name'/>
                                    <attribute name='totalamount' />
                                    <attribute name='statuscode' />
                                    <attribute name='bsd_netusablearea' />
                                    <attribute name='bsd_constructionarea' />
                                    <attribute name='bsd_nameofstaffagent' alias='nameofstaffagent'/>
                                    <attribute name='bsd_referral' />
                                    <attribute name='bsd_queuingfee' />
                                    <attribute name='bsd_depositamount' />
                                    <attribute name='bsd_allowchangeunitsspec' />
                                    <order attribute='ordernumber' descending='false' />
                                    <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' link-type='outer' alias='aa'>
                                       <attribute name='bsd_projectid' alias='project_id'/>
                                       <attribute name='bsd_name' alias='project_name'/>
                                    </link-entity>
                                    <link-entity name='product' from='productid' to='bsd_unitnumber' link-type='outer' alias='ab'>
                                       <attribute name='productid' alias='unit_id'/>
                                       <attribute name='name' alias='unit_name'/>
                                       <attribute name='bsd_actualarea' alias='unit_actualarea' />
                                    </link-entity>
                                    <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaseslaunch' link-type='outer' alias='ac'>
                                        <attribute name='bsd_phaseslaunchid' alias='phaseslaunch_id'/>
                                        <attribute name='bsd_name' alias='phaseslaunch_name'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='customerid' link-type='outer' alias='ad'>
                                       <attribute name='name' alias='account_name'/>
                                    </link-entity>
                                    <link-entity name='contact' from='contactid' to='customerid' link-type='outer' alias='ae'>
                                      <attribute name='bsd_fullname' alias='contact_name'/>
                                    </link-entity>
                                    <link-entity name='opportunity' from='opportunityid' to='opportunityid' link-type='outer' alias='af'>
                                      <attribute name='name' alias='queue_name' />
                                      <attribute name='opportunityid' alias='queue_id'/>
                                    </link-entity>
                                    <link-entity name='bsd_taxcode' from='bsd_taxcodeid' to='bsd_taxcode' link-type='outer' alias='ag'>
                                      <attribute name='bsd_taxcodeid' alias='taxcode_id'/>
                                      <attribute name='bsd_name' alias='taxcode_name'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='bsd_salesagentcompany' link-type='outer' alias='ah'>
                                      <attribute name='name' alias='salesagentcompany_name'/>
                                      <attribute name='accountid' alias='salesagentcompany_id'/>
                                    </link-entity>
                                    <link-entity name='quote' from='quoteid' to='quoteid' link-type='outer' alias='ai'>
                                      <attribute name='name' alias='reservation_name' />
                                      <attribute name='quoteid' alias='reservation_id' />
                                    </link-entity>
                                    <link-entity name='pricelevel' from='pricelevelid' to='pricelevelid' link-type='outer' alias='aj'>
                                        <attribute name='pricelevelid'/>
                                        <attribute name='name' alias='pricelevel_name'/>
                                    </link-entity>
                                    <filter type='and'>
                                        <condition attribute='salesorderid' operator='eq' value='" + ContractId + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContractModel>>("salesorders", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                return;
            }

            Contract = result.value.SingleOrDefault();

            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='salesorder'>
                                    <attribute name='salesorderid' />
                                    <attribute name='ordernumber' />
                                    <link-entity name='bsd_unitsspecification' from='bsd_unitsspecificationid' to='bsd_unitsspecification' link-type='outer' alias='al'>
                                       <attribute name='bsd_name' alias='bsd_unitsspecification_name' />
                                      <attribute name='bsd_unitsspecificationid' alias='bsd_unitsspecification_id'/>
                                    </link-entity>
                                    <link-entity name='bsd_exchangeratedetail' from='bsd_exchangeratedetailid' to='bsd_applyingexchangerate' link-type='outer' alias='ak'>
                                      <attribute name='bsd_name' alias='bsd_exchangeratedetail_name' />
                                      <attribute name='bsd_exchangeratedetailid'/>
                                    </link-entity>
                                    <filter type='and'>
                                        <condition attribute='salesorderid' operator='eq' value='" + ContractId + @"' />
                                    </filter>
                                  </entity>
                                </fetch>";

            var result2 = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ContractModel>>("salesorders", fetch);
            if (result2 == null || result2.value.Count == 0)
            {
                return;
            }
            var data = result2.value.SingleOrDefault();
            Contract.bsd_unitsspecification_name = data.bsd_unitsspecification_name;
            Contract.bsd_unitsspecification_id = data.bsd_unitsspecification_id;
            Contract.bsd_exchangeratedetail_name = data.bsd_exchangeratedetail_name;
            Contract.bsd_exchangeratedetailid = data.bsd_exchangeratedetailid;
        }

        public async Task LoadCoOwners(Guid ContractId)
        {
            string xml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='bsd_coowner'>
                <attribute name='bsd_coownerid' />
                <attribute name='bsd_name' />
                <order attribute='bsd_name' descending='true' />
                <link-entity name='account' from='accountid' to='bsd_customer' visible='false' link-type='outer' alias='a_1324f6d5b214e911a97f000d3aa04914'>
                  <attribute name='bsd_name' alias='account_name' />
                </link-entity>
                <link-entity name='contact' from='contactid' to='bsd_customer' visible='false' link-type='outer' alias='a_6b0d05eeb214e911a97f000d3aa04914'>
                  <attribute name='bsd_fullname' alias='contact_name' />
                </link-entity>
                 <filter type='and'>
                      <condition attribute='bsd_optionentry' operator='eq' uitype='quote' value='{ContractId}' />
                  </filter>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ReservationCoownerModel>>("bsd_coowners", xml);
            if (result != null)
            {
                var data = result.value;
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

        public async Task LoadInstallmentList(Guid ContractId)
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
                    <condition attribute='bsd_optionentry' operator='eq' value='{ContractId}'/>
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
            if (NumberInstallment > 0)
            {
                ShowInstallmentList = true;
            }
            else
            {
                ShowInstallmentList = false;
            }
        }
    }
}
