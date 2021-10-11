using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;

namespace ConasiCRM.Portable.ViewModels
{
    public class DashboardViewModel :BaseViewModel
    {
        public ObservableCollection<ActivitiModel> Activities = new ObservableCollection<ActivitiModel>();

        public ObservableCollection<ChartModel> DataMonthQueue { get; set; } = new ObservableCollection<ChartModel>();
        public ObservableCollection<ChartModel> DataMonthQuote { get; set; } = new ObservableCollection<ChartModel>();
        public ObservableCollection<ChartModel> DataMonthOptionEntry { get; set; } = new ObservableCollection<ChartModel>();
        public ObservableCollection<ChartModel> DataMonthUnit { get; set; } = new ObservableCollection<ChartModel>();

        public ObservableCollection<ChartModel> LeadsChart { get; set; } = new ObservableCollection<ChartModel>();

        private int _numQueue;
        public int numQueue { get => _numQueue; set { _numQueue = value;OnPropertyChanged(nameof(numQueue)); } }
        private int _numQuote;
        public int numQuote { get => _numQuote; set { _numQuote = value; OnPropertyChanged(nameof(numQuote)); } }
        private int _numOptionEntry;
        public int numOptionEntry { get => _numOptionEntry; set { _numOptionEntry = value; OnPropertyChanged(nameof(numOptionEntry)); } }
        private int _numUnit;
        public int numUnit { get => _numUnit; set { _numUnit = value; OnPropertyChanged(nameof(numUnit)); } }

        private int _numKHMoi;
        public int numKHMoi { get => _numKHMoi; set { _numKHMoi = value;OnPropertyChanged(nameof(numKHMoi)); } }
        private int _numKHDaChuyenDoi;
        public int numKHDaChuyenDoi { get => _numKHDaChuyenDoi; set { _numKHDaChuyenDoi = value; OnPropertyChanged(nameof(numKHDaChuyenDoi)); } }
        private int _numKHKhongChuyenDoi;
        public int numKHKhongChuyenDoi { get => _numKHKhongChuyenDoi; set { _numKHKhongChuyenDoi = value; OnPropertyChanged(nameof(numKHKhongChuyenDoi)); } }

        private DateTime _dateBefor;
        public DateTime dateBefor { get=>_dateBefor; set { _dateBefor = value; OnPropertyChanged(nameof(dateBefor)); } }
        public DateTime dateAfter { get; set; }

        public DateTime firstMonth { get; set; }
        public DateTime secondMonth { get; set; }
        public DateTime thirdMonth { get; set; }
        public DateTime fourthMonth { get; set; }

        public DashboardViewModel()
        {
            dateBefor = DateTime.Now;
            DateTime threeMonthsAgo = DateTime.Now.AddMonths(-3);
            dateAfter = new DateTime(threeMonthsAgo.Year, threeMonthsAgo.Month, 1);
            firstMonth = dateAfter;
            secondMonth = dateAfter.AddMonths(1);
            thirdMonth = secondMonth.AddMonths(1);
            fourthMonth = dateBefor;
        }

        public async Task LoadQueueFourMonths()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='opportunity'>
                                    <attribute name='createdon' alias='Date' />
                                    <attribute name='opportunityid' alias='Id' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}'/>
                                      <condition attribute='createdon' operator='on-or-after' value='{dateAfter.ToString("yyyy-MM-dd")}' />
                                      <condition attribute='createdon' operator='on-or-before' value='{dateBefor.ToString("yyyy-MM-dd")}' />
                                      <condition attribute='statuscode' operator='in'>
                                         <value>100000004</value>
                                         <value>100000000</value>
                                         <value>100000002</value>
                                      </condition>
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DashboardChartModel>>("opportunities", fetchXml);
            if (result == null || result.value.Count == 0) return;

            var countQueueFr = result.value.Where(x => x.Date.Month == firstMonth.Month).Count();
            ChartModel chartFirstMonth = new ChartModel() { Category = firstMonth.ToString("MM/yyyy"), Value = countQueueFr };

            var countQueueSe = result.value.Where(x => x.Date.Month == secondMonth.Month).Count();
            ChartModel chartSecondMonth = new ChartModel() { Category = secondMonth.ToString("MM/yyyy"), Value = countQueueSe };

            var countQueueTh = result.value.Where(x => x.Date.Month == thirdMonth.Month).Count();
            ChartModel chartThirdMonth = new ChartModel() { Category = thirdMonth.ToString("MM/yyyy"), Value = countQueueTh };

            var countQueueFo = this.numQueue = result.value.Where(x => x.Date.Month == fourthMonth.Month).Count();
            ChartModel chartFourthMonth = new ChartModel() { Category = fourthMonth.ToString("MM/yyyy"), Value = countQueueFo };

            this.DataMonthQueue.Add(chartFirstMonth);
            this.DataMonthQueue.Add(chartSecondMonth);
            this.DataMonthQueue.Add(chartThirdMonth);
            this.DataMonthQueue.Add(chartFourthMonth);
        }

        public async Task LoadQuoteFourMonths()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='quote'>
                                    <attribute name='createdon' alias='Date'/>
                                    <attribute name='quoteid' alias='Id'/>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}'/>
                                      <condition attribute='createdon' operator='on-or-before' value='{dateBefor.ToString("yyyy-MM-dd")}' />
                                      <condition attribute='createdon' operator='on-or-after' value='{dateAfter.ToString("yyyy-MM-dd")}' />
                                      <condition attribute='statuscode' operator='in'>
                                        <value>100000000</value>
                                        <value>4</value>
                                      </condition>
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DashboardChartModel>>("quotes", fetchXml);
            if (result == null || result.value.Count == 0) return;

            var countQuoteFr = result.value.Where(x => x.Date.Month == firstMonth.Month).Count();
            ChartModel chartFirstMonth = new ChartModel() { Category = firstMonth.ToString("MM/yyyy"), Value = countQuoteFr };

            var countQuoteSe = result.value.Where(x => x.Date.Month == secondMonth.Month).Count();
            ChartModel chartSecondMonth = new ChartModel() { Category = secondMonth.ToString("MM/yyyy"), Value = countQuoteSe };

            var countQuoteTh = result.value.Where(x => x.Date.Month == thirdMonth.Month).Count();
            ChartModel chartThirdMonth = new ChartModel() { Category = thirdMonth.ToString("MM/yyyy"), Value = countQuoteTh };

            var countQuoteFo = this.numQuote = result.value.Where(x => x.Date.Month == fourthMonth.Month).Count();
            ChartModel chartFourthMonth = new ChartModel() { Category = fourthMonth.ToString("MM/yyyy"), Value = countQuoteFo };

            this.DataMonthQuote.Add(chartFirstMonth);
            this.DataMonthQuote.Add(chartSecondMonth);
            this.DataMonthQuote.Add(chartThirdMonth);
            this.DataMonthQuote.Add(chartFourthMonth);
        }

        public async Task LoadOptionEntryFourMonths()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='salesorder'>
                                    <attribute name='createdon' alias='Date'/>
                                    <attribute name='salesorderid' alias='Id' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}'/>
                                      <condition attribute='statuscode' operator='ne' value='100000006' />
                                      <condition attribute='createdon' operator='on-or-after' value='{dateAfter.ToString("yyyy-MM-dd")}' />
                                      <condition attribute='createdon' operator='on-or-before' value='{dateBefor.ToString("yyyy-MM-dd")}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DashboardChartModel>>("salesorders", fetchXml);
            if (result == null || result.value.Count == 0) return;

            var countOptionEntryFr = result.value.Where(x => x.Date.Month == firstMonth.Month).Count();
            ChartModel chartFirstMonth = new ChartModel() { Category = firstMonth.ToString("MM/yyyy"), Value = countOptionEntryFr };

            var countOptionEntrySe = result.value.Where(x => x.Date.Month == secondMonth.Month).Count();
            ChartModel chartSecondMonth = new ChartModel() { Category = secondMonth.ToString("MM/yyyy"), Value = countOptionEntrySe };

            var countOptionEntryTh = result.value.Where(x => x.Date.Month == thirdMonth.Month).Count();
            ChartModel chartThirdMonth = new ChartModel() { Category = thirdMonth.ToString("MM/yyyy"), Value = countOptionEntryTh };

            var countOptionEntryFo = this.numOptionEntry = result.value.Where(x => x.Date.Month == fourthMonth.Month).Count();
            ChartModel chartFourthMonth = new ChartModel() { Category = fourthMonth.ToString("MM/yyyy"), Value = countOptionEntryFo };

            this.DataMonthOptionEntry.Add(chartFirstMonth);
            this.DataMonthOptionEntry.Add(chartSecondMonth);
            this.DataMonthOptionEntry.Add(chartThirdMonth);
            this.DataMonthOptionEntry.Add(chartFourthMonth);
        }

        public async Task LoadUnitFourMonths()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='product'>
                                    <attribute name='createdon' alias='Date'/>
                                    <attribute name='productid' alias='Id'/>
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}'/>
                                      <condition attribute='statuscode' operator='eq' value='100000002' />
                                      <condition attribute='createdon' operator='on-or-after' value='{dateAfter.ToString("yyyy-MM-dd")}' />
                                      <condition attribute='createdon' operator='on-or-before' value='{dateBefor.ToString("yyyy-MM-dd")}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DashboardChartModel>>("products", fetchXml);
            if (result == null || result.value.Count == 0) return;

            var countUnitFr = result.value.Where(x => x.Date.Month == firstMonth.Month).Count();
            ChartModel chartFirstMonth = new ChartModel() { Category = firstMonth.ToString("MM/yyyy"), Value = countUnitFr };

            var countUnitSe = result.value.Where(x => x.Date.Month == secondMonth.Month).Count();
            ChartModel chartSecondMonth = new ChartModel() { Category = secondMonth.ToString("MM/yyyy"), Value = countUnitSe };

            var countUnitTh = result.value.Where(x => x.Date.Month == thirdMonth.Month).Count();
            ChartModel chartThirdMonth = new ChartModel() { Category = thirdMonth.ToString("MM/yyyy"), Value = countUnitTh };

            var countUnitFo = this.numUnit = result.value.Where(x => x.Date.Month == fourthMonth.Month).Count();
            ChartModel chartFourthMonth = new ChartModel() { Category = fourthMonth.ToString("MM/yyyy"), Value = countUnitFo };

            this.DataMonthUnit.Add(chartFirstMonth);
            this.DataMonthUnit.Add(chartSecondMonth);
            this.DataMonthUnit.Add(chartThirdMonth);
            this.DataMonthUnit.Add(chartFourthMonth);
        }

        public async Task LoadLeads()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='lead'>
                                    <attribute name='statuscode' alias='Label'/>
                                    <attribute name='leadid' alias='Val' />
                                    <order attribute='createdon' descending='true' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='ne' value='2'/>
                                      <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='{UserLogged.Id}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("leads", fetchXml);
            if (result == null || result.value.Count == 0) return;

            numKHMoi = result.value.Where(x => x.Label == "1").Count();
            ChartModel chartKHMoi = new ChartModel() { Category = "Khách hàng mới", Value = numKHMoi };

            numKHDaChuyenDoi = result.value.Where(x => x.Label == "3").Count();
            ChartModel chartKHDaChuyenDoi = new ChartModel() { Category = "Đã chuyển đổi", Value = numKHDaChuyenDoi };

            numKHKhongChuyenDoi = result.value.Where(x => x.Label == "4" || x.Label == "5" || x.Label == "6" || x.Label == "7").Count();
            ChartModel chartKHKhongChuyenDoi = new ChartModel() { Category = "Không chuyển đổi", Value = numKHKhongChuyenDoi };

            this.LeadsChart.Add(chartKHMoi);
            this.LeadsChart.Add(chartKHDaChuyenDoi);
            this.LeadsChart.Add(chartKHKhongChuyenDoi);
        }

        public async Task LoadTasks()
        {
            string fetchXml = $@"<fetch version='1.0' count='5' page='1' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='task'>
                                    <attribute name='subject' />
                                    <attribute name='activityid' />
                                    <attribute name='scheduledstart' />
                                    <attribute name='scheduledend' />
                                    <attribute name='activitytypecode' />
                                    <attribute name='createdon' />
                                    <order attribute='modifiedon' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='scheduledstart' operator='today' />
                                      <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}'/>
                                    </filter>
                                    <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer' alias='a_48f82b1a8ad844bd90d915e7b3c4f263'>
                                        <attribute name='fullname' alias='contact_name'/>
                                        <attribute name='contactid' alias='contact_id'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='outer' alias='a_9cdbdceab5ee4a8db875050d455757bd'>
                                          <attribute name='accountid' alias='account_id'/>
                                          <attribute name='name' alias='account_name'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ActivitiModel>>("tasks", fetchXml);
            if (result == null || result.value.Count == 0) return;

            foreach (var item in result.value)
            {
                item.scheduledstart = item.scheduledstart.ToLocalTime();
                item.scheduledend = item.scheduledend.ToLocalTime();
                if (!string.IsNullOrWhiteSpace(item.contact_name))
                {
                    item.customer = item.contact_name;
                }
                if (!string.IsNullOrWhiteSpace(item.account_name))
                {
                    item.customer = item.account_name;
                }

                this.Activities.Add(item);
            }
        }

        public async Task LoadMettings()
        {
            string fetchXml = $@"<fetch version='1.0' count='5' page='1' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='appointment'>
                                    <attribute name='subject' />
                                    <attribute name='activityid' />
                                    <attribute name='scheduledstart' />
                                    <attribute name='scheduledend' />
                                    <attribute name='activitytypecode' />   
                                    <attribute name='createdon' />
                                    <order attribute='modifiedon' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='scheduledstart' operator='today' />
                                      <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}'/>
                                    </filter>
                                    <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer' alias='a_48f82b1a8ad844bd90d915e7b3c4f263'>
                                        <attribute name='fullname' alias='contact_name'/>
                                        <attribute name='contactid' alias='contact_id'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='outer' alias='a_9cdbdceab5ee4a8db875050d455757bd'>
                                          <attribute name='accountid' alias='account_id'/>
                                          <attribute name='name' alias='account_name'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ActivitiModel>>("appointments", fetchXml);
            if (result == null || result.value.Count == 0) return;

            foreach (var item in result.value)
            {
                item.scheduledstart = item.scheduledstart.ToLocalTime();
                item.scheduledend = item.scheduledend.ToLocalTime();
                if (!string.IsNullOrWhiteSpace(item.contact_name))
                {
                    item.customer = item.contact_name;
                }
                if (!string.IsNullOrWhiteSpace(item.account_name))
                {
                    item.customer = item.account_name;
                }

                this.Activities.Add(item);
            }
        }

        public async Task LoadPhoneCalls()
        {
            string fetchXml = $@"<fetch version='1.0' count='5' page='1' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='phonecall'>
                                    <attribute name='subject' />
                                    <attribute name='activityid' />
                                    <attribute name='scheduledstart' />
                                    <attribute name='scheduledend' />
                                    <attribute name='activitytypecode' />
                                    <attribute name='createdon' />
                                    <order attribute='modifiedon' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='scheduledstart' operator='today' />
                                      <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}'/>
                                    </filter>
                                    <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer' alias='a_48f82b1a8ad844bd90d915e7b3c4f263'>
                                        <attribute name='fullname' alias='contact_name'/>
                                        <attribute name='contactid' alias='contact_id'/>
                                    </link-entity>
                                    <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='outer' alias='a_9cdbdceab5ee4a8db875050d455757bd'>
                                          <attribute name='accountid' alias='account_id'/>
                                          <attribute name='name' alias='account_name'/>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ActivitiModel>>("phonecalls", fetchXml);
            if (result == null || result.value.Count == 0) return;

            foreach (var item in result.value)
            {
                item.scheduledstart = item.scheduledstart.ToLocalTime();
                item.scheduledend = item.scheduledend.ToLocalTime();
                if (!string.IsNullOrWhiteSpace(item.contact_name))
                {
                    item.customer = item.contact_name;
                }
                if (!string.IsNullOrWhiteSpace(item.account_name))
                {
                    item.customer = item.account_name;
                }

                this.Activities.Add(item);
            }
        }
    }
    public class DashboardChartModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
    }
}
