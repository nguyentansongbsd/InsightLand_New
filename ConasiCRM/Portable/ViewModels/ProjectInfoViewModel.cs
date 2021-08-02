using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class ProjectInfoViewModel : BaseViewModel
    {
        private ProjectInfoModel _project;
        public ProjectInfoModel Project
        {
            get => _project;
            set
            {
                if (_project != value)
                {
                    _project = value;
                    OnPropertyChanged(nameof(Project));
                }
            }
        }

        private bool _showMoreDuAnCanhTranh;
        public bool ShowMoreDuAnCanhTranh { get => _showMoreDuAnCanhTranh; set { _showMoreDuAnCanhTranh = value; OnPropertyChanged(nameof(ShowMoreDuAnCanhTranh)); } }

        public int PageDuAnCanhTranh { get; set; } = 1;

        private bool _showMoreDoiThuCanhTranh;
        public bool ShowMoreDoiThuCanhTranh { get => _showMoreDoiThuCanhTranh; set { _showMoreDoiThuCanhTranh = value; OnPropertyChanged(nameof(ShowMoreDoiThuCanhTranh)); } }

        public int PageDoiThuCanhTranh { get; set; } = 1;

        public ObservableCollection<Project_DuAnCanhTranhModel> DuAnCanhTranh_List { get; set; }
        public ObservableCollection<Project_DoiThuCanhTranhModel> DoiThuCanhTranh_List { get; set; }

        public ProjectInfoViewModel()
        {
            IsBusy = true;
            DuAnCanhTranh_List = new ObservableCollection<Project_DuAnCanhTranhModel>();
            DoiThuCanhTranh_List = new ObservableCollection<Project_DoiThuCanhTranhModel>();
        }

        public async Task LoadDuAnCanhTranh(Guid ProjectId)
        {
            string FetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
              <entity name='bsd_competitiveproject'>
                <attribute name='bsd_competitiveprojectid' />
                <attribute name='bsd_name' />
                <attribute name='createdon' />
                <attribute name='bsd_projectcode' />
                <attribute name='bsd_weakness' />
                <attribute name='bsd_strength' />
                <order attribute='bsd_name' descending='false' />
                <link-entity name='bsd_bsd_competitiveproject_bsd_project' from='bsd_competitiveprojectid' to='bsd_competitiveprojectid' visible='false' intersect='true'>
                  <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' alias='ab'>
                    <filter type='and'>
                      <condition attribute='bsd_projectid' operator='eq' uitype='bsd_project' value='" + ProjectId.ToString() + @"' />
                    </filter>
                  </link-entity>
                </link-entity>
                <link-entity name='account' from='accountid' to='bsd_investor' visible='false' link-type='outer' alias='a_0a24f6d5b214e911a97f000d3aa04914'>
                  <attribute name='bsd_name' alias='bsd_investor_name' />
                </link-entity>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Project_DuAnCanhTranhModel>>("bsd_competitiveprojects", FetchXml);
            if (result == null)
            {
                await App.Current.MainPage.DisplayAlert("Lỗi", "Không load được danh sách dự án cạnh tranh", "Đóng");
                return;
            }
            else
            {
                var data = result.value;

                if (data.Count <= 3)
                {
                    ShowMoreDuAnCanhTranh = false;
                }
                else
                {
                    ShowMoreDuAnCanhTranh = true;
                }

                foreach (var x in result.value)
                {
                    DuAnCanhTranh_List.Add(x);
                }
            }
        }
        public async Task LoadDoiThuCanhTrang(Guid ProjectId)
        {
            string FetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
              <entity name='competitor'>
                <attribute name='name' />
                <attribute name='websiteurl' />
                <attribute name='competitorid' />
                <attribute name='weaknesses' />
                <attribute name='strengths' />
                <attribute name='createdon' />
                <order attribute='name' descending='false' />
                <link-entity name='bsd_competitor_bsd_project' from='competitorid' to='competitorid' visible='false' intersect='true'>
                  <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectid' alias='ac'>
                    <filter type='and'>
                      <condition attribute='bsd_projectid' operator='eq' uiname='ARIYANA' uitype='bsd_project' value='" + ProjectId.ToString() + @"' />
                    </filter>
                  </link-entity>
                </link-entity>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Project_DoiThuCanhTranhModel>>("competitors", FetchXml);
            if (result == null)
            {
                await App.Current.MainPage.DisplayAlert("Lỗi", "Không load được đối thủ cạnh tranh.", "Đóng");
                return;
            }
            else
            {
                var data = result.value;

                if (data.Count <= 3)
                {
                    ShowMoreDoiThuCanhTranh = false;
                }
                else
                {
                    ShowMoreDoiThuCanhTranh = true;
                }

                foreach (var x in result.value)
                {
                    DoiThuCanhTranh_List.Add(x);
                }
            }
        }
    }
}
