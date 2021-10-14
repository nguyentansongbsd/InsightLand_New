﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        public ObservableCollection<ActivitiModel> Activities { get; set; } = new ObservableCollection<ActivitiModel>();

        public ObservableCollection<ChartModel> DataMonthQueue { get; set; } = new ObservableCollection<ChartModel>();
        public ObservableCollection<ChartModel> DataMonthQuote { get; set; } = new ObservableCollection<ChartModel>();
        public ObservableCollection<ChartModel> DataMonthOptionEntry { get; set; } = new ObservableCollection<ChartModel>();
        public ObservableCollection<ChartModel> DataMonthUnit { get; set; } = new ObservableCollection<ChartModel>();

        public ObservableCollection<ChartModel> CommissionTransactionChart { get; set; } = new ObservableCollection<ChartModel>();
        public ObservableCollection<ChartModel> LeadsChart { get; set; } = new ObservableCollection<ChartModel>();

        private bool _isRefreshing;
        public bool IsRefreshing { get => _isRefreshing; set { _isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); } }

        private decimal _totalCommissionAMonth;
        public decimal TotalCommissionAMonth { get => _totalCommissionAMonth; set { _totalCommissionAMonth = value; OnPropertyChanged(nameof(TotalCommissionAMonth)); } }
        private decimal _totalPaidCommissionAMonth;
        public decimal TotalPaidCommissionAMonth { get => _totalPaidCommissionAMonth; set { _totalPaidCommissionAMonth = value; OnPropertyChanged(nameof(TotalPaidCommissionAMonth)); } }

        private int _numQueue;
        public int numQueue { get => _numQueue; set { _numQueue = value; OnPropertyChanged(nameof(numQueue)); } }
        private int _numQuote;
        public int numQuote { get => _numQuote; set { _numQuote = value; OnPropertyChanged(nameof(numQuote)); } }
        private int _numOptionEntry;
        public int numOptionEntry { get => _numOptionEntry; set { _numOptionEntry = value; OnPropertyChanged(nameof(numOptionEntry)); } }
        private int _numUnit;
        public int numUnit { get => _numUnit; set { _numUnit = value; OnPropertyChanged(nameof(numUnit)); } }

        private int _numKHMoi;
        public int numKHMoi { get => _numKHMoi; set { _numKHMoi = value; OnPropertyChanged(nameof(numKHMoi)); } }
        private int _numKHDaChuyenDoi;
        public int numKHDaChuyenDoi { get => _numKHDaChuyenDoi; set { _numKHDaChuyenDoi = value; OnPropertyChanged(nameof(numKHDaChuyenDoi)); } }
        private int _numKHKhongChuyenDoi;
        public int numKHKhongChuyenDoi { get => _numKHKhongChuyenDoi; set { _numKHKhongChuyenDoi = value; OnPropertyChanged(nameof(numKHKhongChuyenDoi)); } }

        private PhoneCellModel _phoneCall;
        public PhoneCellModel PhoneCall { get => _phoneCall; set { _phoneCall = value; OnPropertyChanged(nameof(PhoneCall)); } }
        private TaskFormModel _taskDetail;
        public TaskFormModel TaskDetail { get => _taskDetail; set { _taskDetail = value; OnPropertyChanged(nameof(TaskDetail)); } }
        private MeetingModel _meet;
        public MeetingModel Meet { get => _meet; set { _meet = value; OnPropertyChanged(nameof(Meet)); } }

        #region Hoat dong
        private StatusCodeModel _activityStatusCode;
        public StatusCodeModel ActivityStatusCode { get => _activityStatusCode; set { _activityStatusCode = value; OnPropertyChanged(nameof(ActivityStatusCode)); } }
        private string _activityType;
        public string ActivityType { get => _activityType; set { _activityType = value; OnPropertyChanged(nameof(ActivityType)); } }
        public bool _showGridButton;
        public bool ShowGridButton { get => _showGridButton; set { _showGridButton = value; OnPropertyChanged(nameof(ShowGridButton)); } }
        private DateTime? _scheduledStartTask;
        public DateTime? ScheduledStartTask { get => _scheduledStartTask; set { _scheduledStartTask = value; OnPropertyChanged(nameof(ScheduledStartTask)); } }
        private DateTime? _scheduledEndTask;
        public DateTime? ScheduledEndTask { get => _scheduledEndTask; set { _scheduledEndTask = value; OnPropertyChanged(nameof(ScheduledEndTask)); } }
        public string CodeCompleted = "completed";
        public string CodeCancel = "cancel";
        #endregion

        private DateTime _dateBefor;
        public DateTime dateBefor { get => _dateBefor; set { _dateBefor = value; OnPropertyChanged(nameof(dateBefor)); } }
        public DateTime dateAfter { get; set; }

        public DateTime firstMonth { get; set; }
        public DateTime secondMonth { get; set; }
        public DateTime thirdMonth { get; set; }
        public DateTime fourthMonth { get; set; }

        public ICommand RefreshCommand => new Command(async () =>
        {
            IsRefreshing = true;
            await RefreshDashboard(); 
            IsRefreshing = false;
        });

        public DashboardViewModel()
        {
            dateBefor = DateTime.Now;
            DateTime threeMonthsAgo = DateTime.Now.AddMonths(-3);
            dateAfter = new DateTime(threeMonthsAgo.Year, threeMonthsAgo.Month, 1);
            firstMonth = dateAfter;
            secondMonth = dateAfter.AddMonths(1);
            thirdMonth = secondMonth.AddMonths(1);
            fourthMonth = dateBefor;

            PhoneCall = new PhoneCellModel();
            TaskDetail = new TaskFormModel();
            Meet = new MeetingModel();
        }

        public async Task LoadCommissionTransactions()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='bsd_commissiontransaction'>
                                    <attribute name='bsd_commissiontransactionid' alias='Id'/>
                                    <attribute name='createdon' alias='Date'/>
                                    <attribute name='bsd_totalcommission' alias='CommissionTotal'/>
                                    <attribute name='statuscode' alias='CommissionStatus'/>
                                    <order attribute='createdon' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}'/>
                                      <condition attribute='statuscode' operator='in'>
                                        <value>100000007</value>
                                        <value>100000006</value>
                                        <value>100000004</value>
                                        <value>100000005</value>
                                      </condition>
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DashboardChartModel>>("bsd_commissiontransactions", fetchXml);
            if (result == null) return;

            foreach (var item in result.value)
            {
                //this.TotalCommissionAMonth += item.CommissionTotal;
                //if (item.CommissionStatus == "100000007")
                //{
                //    this.TotalPaidCommissionAMonth += item.CommissionTotal;
                //}
            }

            var countCommissionFr = result.value.Where(x => x.Date.Month == firstMonth.Month).Count();
            ChartModel chartFirstMonth = new ChartModel() { Category = firstMonth.ToString("MM/yyyy"), Value = countCommissionFr };

            var countCommissionSe = result.value.Where(x => x.Date.Month == secondMonth.Month).Count();
            ChartModel chartSecondMonth = new ChartModel() { Category = secondMonth.ToString("MM/yyyy"), Value = countCommissionSe };

            var countCommissionTh = result.value.Where(x => x.Date.Month == thirdMonth.Month).Count();
            ChartModel chartThirdMonth = new ChartModel() { Category = thirdMonth.ToString("MM/yyyy"), Value = countCommissionTh };

            var countCommissionFo = result.value.Where(x => x.Date.Month == fourthMonth.Month).Count();
            ChartModel chartFourthMonth = new ChartModel() { Category = fourthMonth.ToString("MM/yyyy"), Value = countCommissionFo };

            this.CommissionTransactionChart.Add(chartFirstMonth);
            this.CommissionTransactionChart.Add(chartSecondMonth);
            this.CommissionTransactionChart.Add(chartThirdMonth);
            this.CommissionTransactionChart.Add(chartFourthMonth);
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
            if (result == null) return;

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
            if (result == null ) return;

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
            if (result == null) return;

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
            if (result == null) return;

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

        public async Task RefreshDashboard()
        {
            this.Activities.Clear();
            this.DataMonthQueue.Clear();
            this.DataMonthQuote.Clear();
            this.DataMonthOptionEntry.Clear();
            this.DataMonthUnit.Clear();
            this.CommissionTransactionChart.Clear();
            this.LeadsChart.Clear();

            await Task.WhenAll(
                 this.LoadTasks(),
                 this.LoadMettings(),
                 this.LoadPhoneCalls(),
                 this.LoadQueueFourMonths(),
                 this.LoadQuoteFourMonths(),
                 this.LoadOptionEntryFourMonths(),
                 this.LoadUnitFourMonths(),
                 this.LoadLeads(),
                 this.LoadCommissionTransactions()
                );
            var activities = new ObservableCollection<ActivitiModel>(this.Activities.Take(5));
            this.Activities = activities;
        }

        public async Task loadPhoneCall(Guid id)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='phonecall'>
                                <attribute name='subject' />
                                <attribute name='statecode' />
                                <attribute name='prioritycode' />
                                <attribute name='scheduledend' alias='scheduledend' />
                                <attribute name='createdby' />
                                <attribute name='regardingobjectid' />
                                <attribute name='activityid' />
                                <attribute name='statuscode' />
                                <attribute name='scheduledstart' alias='scheduledstart' />
                                <attribute name='actualdurationminutes' />
                                <attribute name='description' />
                                <attribute name='activitytypecode' />
                                <attribute name='phonenumber' />
                                <order attribute='subject' descending='false' />
                                <filter type='and'>
                                    <condition attribute='activityid' operator='eq' uitype='phonecall' value='" + id + @"' />
                                </filter>
                                <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='outer' alias='a_9b4f4019bdc24dd79b1858c2d087a27d'>
                                    <attribute name='accountid' alias='account_id' />                  
                                    <attribute name='bsd_name' alias='account_name'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer' alias='a_66b6d0af970a40c9a0f42838936ea5ce'>
                                    <attribute name='contactid' alias='contact_id' />                  
                                    <attribute name='fullname' alias='contact_name'/>
                                </link-entity>
                                <link-entity name='lead' from='leadid' to='regardingobjectid' visible='false' link-type='outer' alias='a_fb87dbfd8304e911a98b000d3aa2e890'>
                                    <attribute name='leadid' alias='lead_id'/>                  
                                    <attribute name='fullname' alias='lead_name'/>
                                </link-entity>
                                <link-entity name='bsd_employee' from='bsd_employeeid' to='bsd_employee' link-type='outer' alias='aa'>
                                    <attribute name='bsd_name' alias='user_name'/>
                                    <attribute name='bsd_employeeid' alias='user_id'/>
                                </link-entity>
                            </entity>
                          </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PhoneCellModel>>("phonecalls", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value.FirstOrDefault();
            PhoneCall = data;

            if (data.scheduledend != null && data.scheduledstart != null)
            {
                PhoneCall.scheduledend = data.scheduledend.Value.ToLocalTime();
                PhoneCall.scheduledstart = data.scheduledstart.Value.ToLocalTime();
            }

            if (PhoneCall.contact_id != Guid.Empty)
            {
                PhoneCall.Customer = new CustomerLookUp
                {
                    Name = PhoneCall.contact_name
                };
            }
            else if (PhoneCall.account_id != Guid.Empty)
            {
                PhoneCall.Customer = new CustomerLookUp
                {
                    Name = PhoneCall.account_name
                };
            }
            else if (PhoneCall.lead_id != Guid.Empty)
            {
                PhoneCall.Customer = new CustomerLookUp
                {
                    Name = PhoneCall.lead_name
                };
            }

            if (PhoneCall.statecode == 0)
                ShowGridButton = true;
            else
                ShowGridButton = false;
        }

        public async Task loadFromTo(Guid id)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true' >
                                    <entity name='phonecall' >
                                        <attribute name='subject' />
                                        <attribute name='activityid' />
                                        <order attribute='subject' descending='false' />
                                        <filter type='and' >
                                            <condition attribute='activityid' operator='eq' value='" + id + @"' />
                                        </filter>
                                        <link-entity name='activityparty' from='activityid' to='activityid' link-type='inner' alias='ab' >
                                            <attribute name='partyid' alias='partyID'/>
                                            <attribute name='participationtypemask' alias='typemask' />
                                            <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='partyaccount' >
                                                <attribute name='bsd_name' alias='account_name'/>
                                            </link-entity>
                                            <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='partycontact' >
                                                <attribute name='fullname' alias='contact_name'/>
                                            </link-entity>
                                            <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='partylead' >
                                                <attribute name='fullname' alias='lead_name'/>
                                            </link-entity>
                                            <link-entity name='systemuser' from='systemuserid' to='partyid' link-type='outer' alias='partyuser' >
                                                <attribute name='fullname' alias='user_name'/>
                                            </link-entity>
                                        </link-entity>
                                    </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PartyModel>>("phonecalls", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value;
            if (data.Any())
            {
                foreach (var item in data)
                {
                    if (item.typemask == 1) // from call
                    {
                        PhoneCall.call_from = item.user_name;
                    }
                    else if (item.typemask == 2) // to call
                    {
                        if (item.contact_name != null && item.contact_name != string.Empty)
                        {
                            PhoneCall.call_to = item.contact_name;
                        }
                        else if (item.account_name != null && item.account_name != string.Empty)
                        {
                            PhoneCall.call_to = item.account_name;
                        }
                        else if (item.lead_name != null && item.lead_name != string.Empty)
                        {
                            PhoneCall.call_to = item.lead_name;
                        }
                    }
                }
            }
        }

        public async Task loadTask(Guid id)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                              <entity name='task'>
                                <attribute name='subject' />
                                <attribute name='statecode' />
                                <attribute name='scheduledend' />
                                <attribute name='activityid' />
                                <attribute name='statuscode' />
                                <attribute name='scheduledstart' />
                                <attribute name='description' />
                                <order attribute='subject' descending='false' />
                                <filter type='and' >
                                    <condition attribute='activityid' operator='eq' value='" + id + @"' />
                                </filter>
                                <link-entity name='account' from='accountid' to='regardingobjectid' link-type='outer' alias='ah'>
    	                            <attribute name='accountid' alias='account_id' />                  
    	                            <attribute name='bsd_name' alias='account_name'/>
                                </link-entity>
                                <link-entity name='contact' from='contactid' to='regardingobjectid' link-type='outer' alias='ai'>
	                            <attribute name='contactid' alias='contact_id' />                  
                                    <attribute name='fullname' alias='contact_name'/>
                                </link-entity>
                                <link-entity name='lead' from='leadid' to='regardingobjectid' link-type='outer' alias='aj'>
	                            <attribute name='leadid' alias='lead_id'/>                  
                                    <attribute name='fullname' alias='lead_name'/>
                                </link-entity>
                              </entity>
                            </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<TaskFormModel>>("tasks", fetch);
            if (result == null || result.value == null) return;
            TaskDetail = result.value.FirstOrDefault();

            this.ScheduledStartTask = TaskDetail.scheduledstart.Value.ToLocalTime();
            this.ScheduledEndTask = TaskDetail.scheduledend.Value.ToLocalTime();

            if (TaskDetail.contact_id != Guid.Empty)
            {
                TaskDetail.Customer = new CustomerLookUp
                {
                    Name = TaskDetail.contact_name
                };
            }
            else if (TaskDetail.account_id != Guid.Empty)
            {
                TaskDetail.Customer = new CustomerLookUp
                {
                    Name = TaskDetail.account_name
                };
            }
            else if (TaskDetail.lead_id != Guid.Empty)
            {
                TaskDetail.Customer = new CustomerLookUp
                {
                    Name = TaskDetail.lead_name
                };
            }

            if (TaskDetail.statecode == 0)
                ShowGridButton = true;
            else
                ShowGridButton = false;
        }

        public async Task loadMeet(Guid id)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='appointment'>
                                <attribute name='subject' />
                                <attribute name='statecode' />
                                <attribute name='createdby' />
                                <attribute name='statuscode' />
                                <attribute name='requiredattendees' />
                                <attribute name='prioritycode' />
                                <attribute name='scheduledstart' />
                                <attribute name='scheduledend' />
                                <attribute name='scheduleddurationminutes' />
                                <attribute name='bsd_mmeetingformuploaded' />
                                <attribute name='optionalattendees' />
                                <attribute name='isalldayevent' />
                                <attribute name='location' />
                                <attribute name='activityid' />
                                <attribute name='description' />
                                <order attribute='createdon' descending='true' />
                                <filter type='and'>
                                    <condition attribute='activityid' operator='eq' uitype='appointment' value='" + id + @"' />
                                </filter>               
                                <link-entity name='contact' from='contactid' to='regardingobjectid' visible='false' link-type='outer' alias='contacts'>
                                  <attribute name='contactid' alias='contact_id' />                  
                                  <attribute name='fullname' alias='contact_name'/>
                                </link-entity>
                                <link-entity name='account' from='accountid' to='regardingobjectid' visible='false' link-type='outer' alias='accounts'>
                                    <attribute name='accountid' alias='account_id' />                  
                                    <attribute name='bsd_name' alias='account_name'/>
                                </link-entity>
                                <link-entity name='lead' from='leadid' to='regardingobjectid' visible='false' link-type='outer' alias='leads'>
                                    <attribute name='leadid' alias='lead_id'/>                  
                                    <attribute name='fullname' alias='lead_name'/>
                                </link-entity>
                            </entity>
                          </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<MeetingModel>>("appointments", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value.FirstOrDefault();
            Meet = data;

            if (data.scheduledend != null && data.scheduledstart != null)
            {
                Meet.scheduledend = data.scheduledend.Value.ToLocalTime();
                Meet.scheduledstart = data.scheduledstart.Value.ToLocalTime();
            }

            if (Meet.contact_id != Guid.Empty)
            {
                Meet.Customer = new CustomerLookUp
                {
                    Name = Meet.contact_name
                };
            }
            else if (Meet.account_id != Guid.Empty)
            {
                Meet.Customer = new CustomerLookUp
                {
                    Name = Meet.account_name
                };
            }
            else if (Meet.lead_id != Guid.Empty)
            {
                Meet.Customer = new CustomerLookUp
                {
                    Name = Meet.lead_name
                };
            }

            if (Meet.statecode == 0)
                ShowGridButton = true;
            else
                ShowGridButton = false;
        }

        public async Task loadFromToMeet(Guid id)
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>
                                <entity name='appointment'>
                                    <attribute name='subject' />
                                    <attribute name='createdon' />
                                    <attribute name='activityid' />
                                    <order attribute='createdon' descending='false' />
                                    <filter type='and'>
                                        <condition attribute='activityid' operator='eq' value='" + id + @"' />
                                    </filter>
                                    <link-entity name='activityparty' from='activityid' to='activityid' link-type='inner' alias='ab'>
                                        <attribute name='partyid' alias='partyID'/>
                                        <attribute name='participationtypemask' alias='typemask'/>
                                      <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='partyaccount'>
                                        <attribute name='bsd_name' alias='account_name'/>
                                      </link-entity>
                                      <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='partycontact'>
                                        <attribute name='fullname' alias='contact_name'/>
                                      </link-entity>
                                      <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='partylead'>
                                        <attribute name='fullname' alias='lead_name'/>
                                      </link-entity>
                                      <link-entity name='systemuser' from='systemuserid' to='partyid' link-type='outer' alias='partyuser'>
                                        <attribute name='fullname' alias='user_name'/>
                                      </link-entity>
                                    </link-entity>
                                </entity>
                              </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<PartyModel>>("appointments", fetch);
            if (result == null || result.value == null)
                return;
            var data = result.value;
            if (data.Any())
            {
                List<string> required = new List<string>();
                List<string> optional = new List<string>();
                foreach (var item in data)
                {
                    if (item.typemask == 5) // from call
                    {
                        if (item.contact_name != null && item.contact_name != string.Empty)
                        {
                            required.Add(item.contact_name);
                        }
                        else if (item.account_name != null && item.account_name != string.Empty)
                        {
                            required.Add(item.account_name);
                        }
                        else if (item.lead_name != null && item.lead_name != string.Empty)
                        {
                            required.Add(item.lead_name);
                        }
                    }
                    else if (item.typemask == 6) // to call
                    {
                        if (item.contact_name != null && item.contact_name != string.Empty)
                        {
                            optional.Add(item.contact_name);
                        }
                        else if (item.account_name != null && item.account_name != string.Empty)
                        {
                            optional.Add(item.account_name);
                        }
                        else if (item.lead_name != null && item.lead_name != string.Empty)
                        {
                            optional.Add(item.lead_name);
                        }
                    }
                }
                Meet.required = string.Join(", ", required);
                Meet.optional = string.Join(", ", optional);
            }
        }

        public async Task<bool> UpdateStatusPhoneCall(string update)
        {
            if (update == CodeCompleted)
            {
                PhoneCall.statecode = 1;
                PhoneCall.statuscode = 2;
            }
            else if (update == CodeCancel)
            {
                PhoneCall.statecode = 2;
                PhoneCall.statuscode = 3;
            }

            IDictionary<string, object> data = new Dictionary<string, object>();
            data["statecode"] = PhoneCall.statecode;
            data["statuscode"] = PhoneCall.statuscode;

            string path = "/phonecalls(" + PhoneCall.activityid + ")";
            CrmApiResponse result = await CrmHelper.PatchData(path, data);
            if (result.IsSuccess)
            {
                ShowGridButton = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateStatusTask(string update)
        {
            if (update == CodeCompleted)
            {
                TaskDetail.statecode = 1;
            }
            else if (update == CodeCancel)
            {
                TaskDetail.statecode = 2;
            }

            IDictionary<string, object> data = new Dictionary<string, object>();
            data["statecode"] = TaskDetail.statecode;

            string path = "/tasks(" + TaskDetail.activityid + ")";
            CrmApiResponse result = await CrmHelper.PatchData(path, data);
            if (result.IsSuccess)
            {
                ShowGridButton = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateStatusMeet(string update)
        {
            if (update == CodeCompleted)
            {
                Meet.statecode = 1;
            }
            else if (update == CodeCancel)
            {
                Meet.statecode = 2;
            }

            IDictionary<string, object> data = new Dictionary<string, object>();
            data["statecode"] = Meet.statecode;

            string path = "/appointments(" + Meet.activityid + ")";
            CrmApiResponse result = await CrmHelper.PatchData(path, data);
            if (result.IsSuccess)
            {
                ShowGridButton = false;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public class DashboardChartModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal CommissionTotal { get; set; }
        public string CommissionStatus { get; set; }
    }
}
