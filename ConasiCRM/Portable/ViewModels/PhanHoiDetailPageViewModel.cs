using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class PhanHoiDetailPageViewModel : BaseViewModel
    {
        public ObservableCollection<FloatButtonItem> ButtonCommandList { get; set; } = new ObservableCollection<FloatButtonItem>();

        private PhanHoiFormModel _case;
        public PhanHoiFormModel Case { get => _case; set { _case = value; OnPropertyChanged(nameof(Case)); } }

        public ObservableCollection<ListPhanHoiModel> _listCase;
        public ObservableCollection<ListPhanHoiModel> ListCase { get => _listCase; set { _listCase = value; OnPropertyChanged(nameof(ListCase)); } }
        public int PageCase { get; set; } = 1;

        private bool _showMoreCase;
        public bool ShowMoreCase { get => _showMoreCase; set { _showMoreCase = value; OnPropertyChanged(nameof(ShowMoreCase)); } }

        public bool _showButton;
        public bool ShowButton { get => _showButton; set { _showButton = value; OnPropertyChanged(nameof(ShowButton)); } }

        public PhanHoiDetailPageViewModel()
        {
            Case = new PhanHoiFormModel();
            ListCase = new ObservableCollection<ListPhanHoiModel>();
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
                                <link-entity name='incident' from='incidentid' to='parentcaseid' link-type='outer' alias='aa'>    
                                    <attribute name='title' alias='parentcase_title' />
                                </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhanHoiFormModel>>("incidents", fetch);
            if (result == null || result.value == null)
                return;
            Case = result.value.FirstOrDefault();
        }

        public async Task LoadListCase(Guid CaseId)
        {
            string fetchXml = $@"<fetch version='1.0' count='3' page='{PageCase}' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='incident'>
                                    <attribute name='title' />
                                    <attribute name='casetypecode' />
                                  <order attribute='title' descending='false' />                               
                                  <link-entity name='account' from='accountid' to='customerid' visible='false' link-type='outer' alias='accounts'>
                                  <attribute name='bsd_name' alias='case_nameaccount'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='customerid' visible='false' link-type='outer' alias='contacts'>
                                  <attribute name='bsd_fullname' alias='case_namecontact'/>
                                </link-entity>                               
                                <filter type='and'>
                                    <condition attribute='parentcaseid' operator='eq' uitype='incident' value='" + CaseId + @"' />
                                    <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='"+UserLogged.Id+@"' />
                                </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ListPhanHoiModel>>("incidents", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                ShowMoreCase = false;
                return;
            }
            var data = result.value;
            if (data.Count < 3)
            {
                ShowMoreCase = false;
            }
            else
            {
                ShowMoreCase = true;
            }
            foreach (var item in data)
            {
                ListCase.Add(item);
            }
        }
    }
}
