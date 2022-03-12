using System;
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
        public ObservableCollection<ChartModel> CommissionTransactionChart2 { get; set; } = new ObservableCollection<ChartModel>();
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
        // tổng tiền hoa đồng format
        private string _totalCommission;
        public string TotalCommission { get => _totalCommission; set { _totalCommission = value; OnPropertyChanged(nameof(TotalCommission)); } }
        // tổng tiền thanh toán format
        private string _totalPaidCommission;
        public string TotalPaidCommission { get => _totalPaidCommission; set { _totalPaidCommission = value; OnPropertyChanged(nameof(TotalPaidCommission)); } }

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
                                        <attribute name='bsd_totalamountpaid' alias='CommissionTotalPaid'/>
                                        <attribute name='bsd_totalamount' alias='CommissionTotal'/>
                                        <attribute name='statuscode' />
                                        <order attribute='bsd_name' descending='false' />
                                        <filter type='and'>
                                            <condition attribute='createdon' operator='on-or-after' value='{dateAfter.ToString("yyyy-MM-dd")}' />
                                            <condition attribute='statecode' operator='eq' value='0' />
                                        </filter>
                                        <link-entity name='bsd_commissioncalculation' from='bsd_commissioncalculationid' to='bsd_commissioncalculation' link-type='inner'>
                                            <attribute name='statuscode' alias='statuscode_calculator'/>
                                            <filter type='and'>
                                                <condition attribute='statuscode' operator='eq' value='100000003' />
                                            </filter>
                                        </link-entity>
                                    </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DashboardChartModel>>("bsd_commissiontransactions", fetchXml);
            if (result == null) return;

            decimal countTotalCommissionFr = 0;
            decimal countTotalCommissionSe = 0;
            decimal countTotalCommissionTh = 0;
            decimal countTotalCommissionFo = 0;

            decimal countTotalPaidFr = 0;
            decimal countTotalPaidSe = 0;
            decimal countTotalPaidTh = 0;
            decimal countTotalPaidFo = 0;

            foreach (var item in result.value)
            {
                // danh sách các hhgd có sts = Accountant Confirmed lấy từ entity Commission Calculator trong 4 tháng từ ngày hiện tại trở về trước
                if (item.statuscode_calculator == 100000003 && item.Date.Month == firstMonth.Month)
                {
                    countTotalCommissionFr += item.CommissionTotal;
                    if (item.statuscode == 100000009)
                        countTotalPaidFr += item.CommissionTotalPaid;
                }
                if (item.statuscode_calculator == 100000003 && item.Date.Month == secondMonth.Month)
                {
                    countTotalCommissionSe += item.CommissionTotal;
                    if (item.statuscode == 100000009)
                        countTotalPaidSe += item.CommissionTotalPaid;
                }
                if (item.statuscode_calculator == 100000003 && item.Date.Month == thirdMonth.Month)
                {
                    countTotalCommissionTh += item.CommissionTotal;
                    if (item.statuscode == 100000009)
                        countTotalPaidTh += item.CommissionTotalPaid;
                }
                if (item.statuscode_calculator == 100000003 && item.Date.Month == fourthMonth.Month)
                {
                    // tổng hhgd và hhgd paid được tính ở tháng hiện tại (sử dụng cho 2 giá trị thống kê)
                    countTotalCommissionFo += item.CommissionTotal;
                    TotalCommissionAMonth += item.CommissionTotal;
                    if (item.statuscode == 100000009)
                    {
                        countTotalPaidFo += item.CommissionTotalPaid;
                        TotalPaidCommissionAMonth += item.CommissionTotalPaid;
                    }
                }
            }

            ChartModel chartFirstMonth = new ChartModel() { Category = firstMonth.ToString("MM/yyyy"), Value = await TotalAMonth(countTotalCommissionFr) };
            ChartModel chartSecondMonth = new ChartModel() { Category = secondMonth.ToString("MM/yyyy"), Value = await TotalAMonth(countTotalCommissionSe) };
            ChartModel chartThirdMonth = new ChartModel() { Category = thirdMonth.ToString("MM/yyyy"), Value = await TotalAMonth(countTotalCommissionTh) };
            ChartModel chartFourthMonth = new ChartModel() { Category = fourthMonth.ToString("MM/yyyy"), Value = await TotalAMonth(countTotalCommissionFo) };

            ChartModel chartFirstMonth2 = new ChartModel() { Category = firstMonth.ToString("MM/yyyy"), Value = await TotalAMonth(countTotalPaidFr) };
            ChartModel chartSecondMonth2 = new ChartModel() { Category = secondMonth.ToString("MM/yyyy"), Value = await TotalAMonth(countTotalPaidSe) };
            ChartModel chartThirdMonth2 = new ChartModel() { Category = thirdMonth.ToString("MM/yyyy"), Value = await TotalAMonth(countTotalPaidTh) };
            ChartModel chartFourthMonth2 = new ChartModel() { Category = fourthMonth.ToString("MM/yyyy"), Value = await TotalAMonth(countTotalPaidFo) };

            this.CommissionTransactionChart.Add(chartFirstMonth);
            this.CommissionTransactionChart.Add(chartSecondMonth);
            this.CommissionTransactionChart.Add(chartThirdMonth);
            this.CommissionTransactionChart.Add(chartFourthMonth);

            this.CommissionTransactionChart2.Add(chartFirstMonth2);
            this.CommissionTransactionChart2.Add(chartSecondMonth2);
            this.CommissionTransactionChart2.Add(chartThirdMonth2);
            this.CommissionTransactionChart2.Add(chartFourthMonth2);

            //format sau khi tính tổng
            TotalCommission = StringFormatHelper.FormatCurrency(TotalCommissionAMonth);
            TotalPaidCommission = StringFormatHelper.FormatCurrency(TotalPaidCommissionAMonth);
        }

        private async Task<double> TotalAMonth(decimal total)
        {
            if (total > 0 && total.ToString().Length > 6)
            {
                var _currency = decimal.ToDouble(total).ToString().Substring(0, decimal.ToDouble(total).ToString().Length - 6);
                return double.Parse(_currency);
            }
            else
            {
                return 0;
            }
        }

        public async Task LoadQueueFourMonths()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                    <entity name='opportunity'>
                                        <attribute name='bsd_bookingtime' alias='Date'/>
                                        <attribute name='opportunityid' alias='Id' />
                                            <filter type='and'>
                                                <condition attribute='bsd_bookingtime' operator='on-or-after' value='{dateAfter.ToString("yyyy-MM-dd")}' />
                                                <condition attribute='statuscode' operator='in'>
                                                    <value>100000002</value>
                                                    <value>100000000</value>
                                                </condition>
                                                <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}' />
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
                                    <attribute name='bsd_deposittime' alias='Date' />
                                    <attribute name='quoteid' alias='Id' />
                                    <attribute name='statuscode' alias='statuscode' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='in'>
                                         <value>3</value>
                                         <value>4</value>
                                      </condition>
                                      <condition attribute='bsd_deposittime' operator='on-or-after' value='{dateAfter.ToString("yyyy-MM-dd")}' />
                                      <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}' />
                                    </filter>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<DashboardChartModel>>("quotes", fetchXml);
            if (result == null) return;

            var countQuoteFr = result.value.Where(x => x.Date.Month == firstMonth.Month).Count();
            ChartModel chartFirstMonth = new ChartModel() { Category = firstMonth.ToString("MM/yyyy"), Value = countQuoteFr };

            var countQuoteSe = result.value.Where(x => x.Date.Month == secondMonth.Month).Count();
            ChartModel chartSecondMonth = new ChartModel() { Category = secondMonth.ToString("MM/yyyy"), Value = countQuoteSe };

            var countQuoteTh = result.value.Where(x => x.Date.Month == thirdMonth.Month).Count();
            ChartModel chartThirdMonth = new ChartModel() { Category = thirdMonth.ToString("MM/yyyy"), Value = countQuoteTh };

            var countQuoteFo = result.value.Where(x => x.Date.Month == fourthMonth.Month).Count();
            numQuote = result.value.Where(x => x.Date.Month == fourthMonth.Month && x.statuscode == 3).Count();
            ChartModel chartFourthMonth = new ChartModel() { Category = fourthMonth.ToString("MM/yyyy"), Value = countQuoteFo };

            this.DataMonthQuote.Add(chartFirstMonth);
            this.DataMonthQuote.Add(chartSecondMonth);
            this.DataMonthQuote.Add(chartThirdMonth);
            this.DataMonthQuote.Add(chartFourthMonth);
        }

        public async Task LoadOptionEntryFourMonths()
        {
            // ngoại trừ các sts Terminated , 1st Installment, Option, Qualify
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='salesorder'>
                                    <attribute name='createdon' alias='Date'/>
                                    <attribute name='quoteid' alias='Id'/>
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='not-in'>
                                        <value>100000001</value>
                                        <value>100000006</value>
                                        <value>100000000</value>
                                        <value>100000005</value>
                                      </condition>
                                      <condition attribute='createdon' operator='on-or-after' value='{dateAfter.ToString("yyyy-MM-dd")}' />
                                      <condition attribute='bsd_signedcontractdate' operator='null' />
                                      <condition attribute='bsd_employee' operator='eq' value='{UserLogged.Id}' />
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
                                    <attribute name='productid' alias='Id'/>
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='eq' value='100000002' />
                                    </filter>
                                    <link-entity name='salesorder' from='salesorderid' to='bsd_optionentry' link-type='inner'>
	                                <attribute name='bsd_signedcontractdate' alias='Date'/>
                                      <filter type='and'>
                                        <condition attribute='bsd_signedcontractdate' operator='on-or-after' value='{dateAfter.ToString("yyyy-MM-dd")}' />
                                      </filter>
                                    </link-entity>
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
                                    <filter type='and'>
                                      <condition attribute='createdon' operator='on-or-after' value='{dateAfter.ToString("yyyy-MM-dd")}' />
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
                                    <link-entity name='lead' from='leadid' to='regardingobjectid' visible='false' link-type='outer' alias='a_0a67d7c87cd1eb11bacc000d3a80021e'>
                                          <attribute name='leadid' alias='lead_id'/>
                                          <attribute name='lastname' alias='lead_name'/>
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
                if (!string.IsNullOrWhiteSpace(item.lead_name))
                {
                    item.customer = item.lead_name;
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
                                    <link-entity name='lead' from='leadid' to='regardingobjectid' visible='false' link-type='outer' alias='a_0a67d7c87cd1eb11bacc000d3a80021e'>
                                          <attribute name='leadid' alias='lead_id'/>
                                          <attribute name='lastname' alias='lead_name'/>
                                    </link-entity>
                                    <link-entity name='activityparty' from='activityid' to='activityid' link-type='outer' alias='aee'>
                                        <filter type='and'>
                                            <condition attribute='participationtypemask' operator='eq' value='5' />
                                        </filter>
                                        <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='aff'>
                                            <attribute name='fullname' alias='callto_contact_name'/>
                                        </link-entity>
                                        <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='agg'>
                                            <attribute name='bsd_name' alias='callto_accounts_name'/>
                                        </link-entity>
                                        <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='ahh'>
                                            <attribute name='fullname' alias='callto_lead_name'/>
                                        </link-entity>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ActivitiModel>>("appointments", fetchXml);
            if (result == null || result.value.Count == 0) return;

            foreach (var item in result.value)
            {
                var meet = Activities.FirstOrDefault(x => x.activityid == item.activityid);
                if (meet != null)
                {
                    if (!string.IsNullOrWhiteSpace(item.callto_contact_name))
                    {
                        string new_customer = ", " + item.callto_contact_name; 
                        meet.customer += new_customer;
                    }
                    if (!string.IsNullOrWhiteSpace(item.callto_accounts_name))
                    {
                        string new_customer = ", " + item.callto_accounts_name;
                        meet.customer += new_customer;
                    }
                    if (!string.IsNullOrWhiteSpace(item.callto_lead_name))
                    {
                        string new_customer = ", " + item.callto_lead_name;
                        meet.customer += new_customer;
                    }
                }
                else
                {
                    item.scheduledstart = item.scheduledstart.ToLocalTime();
                    item.scheduledend = item.scheduledend.ToLocalTime();
                    if (!string.IsNullOrWhiteSpace(item.callto_contact_name))
                    {
                        item.customer = item.callto_contact_name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.callto_accounts_name))
                    {
                        item.customer = item.callto_accounts_name;
                    }
                    if (!string.IsNullOrWhiteSpace(item.callto_lead_name))
                    {
                        item.customer = item.callto_lead_name;
                    }
                    this.Activities.Add(item);
                }
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
                                    <link-entity name='lead' from='leadid' to='regardingobjectid' visible='false' link-type='outer' alias='a_0a67d7c87cd1eb11bacc000d3a80021e'>
                                          <attribute name='leadid' alias='lead_id'/>
                                          <attribute name='lastname' alias='lead_name'/>
                                    </link-entity>
                                    <link-entity name='activityparty' from='activityid' to='activityid' link-type='outer' alias='aee'>
                                        <filter type='and'>
                                            <condition attribute='participationtypemask' operator='eq' value='2' />
                                        </filter>
                                        <link-entity name='contact' from='contactid' to='partyid' link-type='outer' alias='aff'>
                                            <attribute name='fullname' alias='callto_contact_name'/>
                                        </link-entity>
                                        <link-entity name='account' from='accountid' to='partyid' link-type='outer' alias='agg'>
                                            <attribute name='bsd_name' alias='callto_accounts_name'/>
                                        </link-entity>
                                        <link-entity name='lead' from='leadid' to='partyid' link-type='outer' alias='ahh'>
                                            <attribute name='fullname' alias='callto_lead_name'/>
                                        </link-entity>
                                    </link-entity>
                                  </entity>
                                </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ActivitiModel>>("phonecalls", fetchXml);
            if (result == null || result.value.Count == 0) return;

            foreach (var item in result.value)
            {
                item.scheduledstart = item.scheduledstart.ToLocalTime();
                item.scheduledend = item.scheduledend.ToLocalTime();
                if (!string.IsNullOrWhiteSpace(item.callto_contact_name))
                {
                    item.customer = item.callto_contact_name;
                }
                if (!string.IsNullOrWhiteSpace(item.callto_accounts_name))
                {
                    item.customer = item.callto_accounts_name;
                }
                if (!string.IsNullOrWhiteSpace(item.callto_lead_name))
                {
                    item.customer = item.callto_lead_name;
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

            this.TotalCommissionAMonth = 0;
            this.TotalPaidCommissionAMonth = 0;

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
        }

        public void LoadTotalCommission()
        {

        }
    }
    public class DashboardChartModel
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal CommissionTotal { get; set; }
        public string CommissionStatus { get; set; }
        public decimal CommissionTotalPaid { get; set; }
        public int statuscode_calculator { get; set; }
        public int statuscode { get; set; }
    }
}
