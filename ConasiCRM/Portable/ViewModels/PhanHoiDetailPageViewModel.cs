using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class PhanHoiDetailPageViewModel : BaseViewModel
    {
        private PhanHoiFormModel _case;
        public PhanHoiFormModel Case { get => _case; set { _case = value; OnPropertyChanged(nameof(Case)); } }

        public PhanHoiDetailPageViewModel()
        {
            Case = new PhanHoiFormModel();
        }

        public async Task LoadCase(Guid CaseID)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='incident'>
                                    <all-attributes/>
                                  <order attribute='title' descending='false' />
                                  <filter type='and'>
                                      <condition attribute='incidentid' operator='eq'  value='{" + CaseID + @"}' />
                                  </filter>
                                  <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='accounts'>
                                  <attribute name='bsd_name' alias='case_nameaccount'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='contacts'>
                                  <attribute name='bsd_fullname' alias='case_namecontact'/>
                                </link-entity>
                                <link-entity name='product' from='productid' to='productid' visible='false' link-type='outer' alias='products'>
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
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhanHoiFormModel>>("incidents", fetch);
            if (result == null || result.value == null)
                return;
            Case = result.value.FirstOrDefault();
        }
    }
}
