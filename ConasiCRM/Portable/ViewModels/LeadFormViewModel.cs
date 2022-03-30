using System;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Helper;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using ConasiCRM.Portable.Settings;
using ConasiCRM.Portable.Resources;

namespace ConasiCRM.Portable.ViewModels
{
    public class LeadFormViewModel : BaseViewModel
    {
        public Guid LeadId { get; set; }

        private LeadFormModel _singleLead;
        public LeadFormModel singleLead { get => _singleLead; set { _singleLead = value; OnPropertyChanged(nameof(singleLead)); } }

        public ObservableCollection<OptionSet> list_currency_lookup { get; set; } = new ObservableCollection<OptionSet>();

        private OptionSet _selectedCurrency;
        public OptionSet SelectedCurrency { get => _selectedCurrency; set { _selectedCurrency = value; OnPropertyChanged(nameof(SelectedCurrency)); } }

        public ObservableCollection<OptionSet> list_industrycode_optionset { get; set; } = new ObservableCollection<OptionSet>();

        private OptionSet _industryCode;
        public OptionSet IndustryCode { get => _industryCode; set { _industryCode = value; OnPropertyChanged(nameof(IndustryCode)); } }

        public ObservableCollection<OptionSet> list_campaign_lookup { get; set; } = new ObservableCollection<OptionSet>();

        private OptionSet _campaign;
        public OptionSet Campaign { get => _campaign; set { _campaign = value; OnPropertyChanged(nameof(Campaign)); } }

        private OptionSet _gender;
        public OptionSet Gender { get => _gender; set { _gender = value; OnPropertyChanged(nameof(Gender)); } }

        private List<OptionSet> _genders;
        public List<OptionSet> Genders { get => _genders; set { _genders = value; OnPropertyChanged(nameof(Genders)); } }

        private OptionSet _leadSource;
        public OptionSet LeadSource { get => _leadSource; set { _leadSource = value;OnPropertyChanged(nameof(LeadSource)); } }

        private List<OptionSet> _leadSources;
        public List<OptionSet> LeadSources { get => _leadSources; set { _leadSources = value; OnPropertyChanged(nameof(LeadSources)); } }

        private OptionSet _rating;
        public OptionSet Rating { get => _rating; set { _rating = value; OnPropertyChanged(nameof(Rating)); } }

        private List<OptionSet> _ratings;
        public List<OptionSet> Ratings { get => _ratings; set { _ratings = value; OnPropertyChanged(nameof(Ratings)); } }

        private bool _isShowbtnClearAddress;
        public bool IsShowbtnClearAddress { get => _isShowbtnClearAddress; set { _isShowbtnClearAddress = value; OnPropertyChanged(nameof(IsShowbtnClearAddress)); } }

        private AddressModel _address;
        public AddressModel Address { get => _address; set { _address = value; OnPropertyChanged(nameof(Address)); } }

        public LeadFormViewModel()
        {
            singleLead = new LeadFormModel();
            this.Genders = new List<OptionSet>() { new OptionSet("1",Language.gender_male_sts), new OptionSet("2", Language.gender_female_sts), new OptionSet("100000000", Language.gender_other_sts) };
            this.loadIndustrycode();
        }

        public async Task LoadOneLead()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                          <entity name='lead'>
                            <attribute name='lastname' />
                            <attribute name='companyname' />
                            <attribute name='subject' alias='bsd_topic_label'/>
                            <attribute name='statuscode' />
                            <attribute name='mobilephone' />
                            <attribute name='telephone1' />
                            <attribute name='jobtitle' />
                            <attribute name='websiteurl' />
                            <attribute name='address1_composite' />
                            <attribute name='address1_line1' />
                            <attribute name='address1_city' />
                            <attribute name='address1_stateorprovince' />
                            <attribute name='address1_postalcode' />
                            <attribute name='address1_country' />
                            <attribute name='description' />
                            <attribute name='industrycode' />
                            <attribute name='revenue' />
                            <attribute name='numberofemployees' />
                            <attribute name='sic' />
                            <attribute name='donotsendmm' />
                            <attribute name='emailaddress1' />
                            <attribute name='createdon' />
                            <attribute name='leadid' />
                            <attribute name='leadqualitycode' />
                            <attribute name='new_gender' />
                            <attribute name='new_birthday' />
                            <attribute name='leadsourcecode' />
                            <order attribute='createdon' descending='true' />
                            <filter type='and'>
                                <condition attribute='leadid' operator='eq' value='{" + LeadId + @"}' />
                            </filter>
                            <link-entity name='transactioncurrency' from='transactioncurrencyid' to='transactioncurrencyid' visible='false' link-type='outer'>
                                <attribute name='currencyname'  alias='transactioncurrencyid_label'/>
                            </link-entity>
                            <link-entity name='campaign' from='campaignid' to='campaignid' visible='false' link-type='outer'>
                                <attribute name='name'  alias='campaignid_label'/>
                            </link-entity>
                            <filter type='and'>
                                     <condition attribute='bsd_employee' operator='eq' uitype='bsd_employee' value='" + UserLogged.Id + @"' />
                                </filter>
                          </entity>
                        </fetch>";

            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LeadFormModel>>("leads", fetch);
            var tmp = result.value.FirstOrDefault();
            if (result == null || tmp == null)
            {
                return;
            }

            this.singleLead = tmp;
            Address = new AddressModel
            {
                country_name = singleLead.address1_country,
                province_name = singleLead.address1_stateorprovince,
                district_name = singleLead.address1_city,
                address = singleLead.address1_composite,
                lineaddress = singleLead.address1_line1,
            };
        }

        public async Task<bool> updateLead()
        {
            string path = "/leads(" + LeadId + ")";
            singleLead.leadid = LeadId;
            var content = await this.getContent();
            CrmApiResponse result = await CrmHelper.PatchData(path, content);

            if (result.IsSuccess)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public async Task<CrmApiResponse> createLead()
        {
            string path = "/leads";
            singleLead.leadid = Guid.NewGuid();
            var content = await this.getContent();
            CrmApiResponse result = await CrmHelper.PostData(path, content);
            return result;
        }

        public async Task<Boolean> DeletLookup(string fieldName, Guid leadId)
        {
            var result = await CrmHelper.SetNullLookupField("leads", leadId, fieldName);
            return result.IsSuccess;
        }

        private async Task<object> getContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            data["leadid"] = singleLead.leadid;
            data["subject"] = singleLead.bsd_topic_label;
            data["lastname"] = singleLead.lastname;
            data["mobilephone"] = singleLead.mobilephone;
            data["telephone1"] = singleLead.telephone1;
            data["jobtitle"] = singleLead.jobtitle;
            data["emailaddress1"] = singleLead.emailaddress1;
            data["companyname"] = singleLead.companyname;
            data["websiteurl"] = singleLead.websiteurl;
            data["description"] = singleLead.description;
            data["industrycode"] = singleLead.industrycode;
            data["revenue"] = singleLead?.revenue;
            data["leadqualitycode"] = Rating.Val;
            data["new_gender"] = this.Gender?.Val;
            data["new_birthday"] = singleLead.new_birthday;
            data["leadsourcecode"] = this.LeadSource?.Val;

            if (Address != null && !string.IsNullOrWhiteSpace(Address.lineaddress))
            {
                data["address1_composite"] = Address.address;
                data["address1_line1"] = Address.lineaddress;

                if (Address.country_id != Guid.Empty)
                    data["address1_country"] = Address.country_name;
                if (Address.province_id != Guid.Empty)
                    data["address1_stateorprovince"] = Address.province_name;
                if (Address.district_id != Guid.Empty)
                    data["address1_city"] = Address.district_name;
            }
            if (!string.IsNullOrWhiteSpace(singleLead.numberofemployees))
            {
                data["numberofemployees"] = int.Parse(singleLead.numberofemployees);
            }
            else
            {
                data["numberofemployees"] = null;
            }
            data["sic"] = singleLead.sic;
            data["donotsendmm"] = singleLead.donotsendmm.ToString();
            data["lastusedincampaign"] = singleLead.lastusedincampaign.HasValue ? (DateTime.Parse(singleLead.lastusedincampaign.ToString()).ToLocalTime()).ToString("yyyy-MM-dd\"T\"HH:mm:ss\"Z\"") : null;
            if (singleLead._transactioncurrencyid_value == null)
            {
                await DeletLookup("transactioncurrencyid", singleLead.leadid);
            }
            else
            {
                data["transactioncurrencyid@odata.bind"] = "/transactioncurrencies(" + singleLead._transactioncurrencyid_value + ")"; /////Lookup Field
            }
            if (singleLead._campaignid_value == null)
            {
                await DeletLookup("CampaignId", singleLead.leadid);
            }
            else
            {
                data["campaignid@odata.bind"] = "/campaigns(" + singleLead._campaignid_value + ")"; /////Lookup Field
            }
            if (UserLogged.Id != Guid.Empty)
            {
                data["bsd_employee@odata.bind"] = "/bsd_employees(" + UserLogged.Id + ")";
            }
            if (UserLogged.ManagerId != Guid.Empty)
            {
                data["ownerid@odata.bind"] = "/systemusers(" + UserLogged.ManagerId + ")";
            }
            return data;
        }

        ///////////// CURRENCY LOOKUP AREA
        public async Task LoadCurrenciesForLookup()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='transactioncurrency'>
                                    <attribute name='transactioncurrencyid' alias='Val'/>
                                    <attribute name='currencyname' alias='Label'/>
                                    <order attribute='currencyname' descending='false' />
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("transactioncurrencies", fetch);
            if (result == null || result.value.Count == 0)
            {
                return;
            }

            foreach (var x in result.value)
            {
                list_currency_lookup.Add(x);
            }
        }

        //////// INDUSTRYCODE OPTIONSET AREA
        public void loadIndustrycode()
        {
            list_industrycode_optionset.Add(new OptionSet() { Val = ("1"), Label = Language.lead_1_industry, });
            //Accounting
            list_industrycode_optionset.Add(new OptionSet() { Val = ("2"), Label = Language.lead_2_industry, });
            //Agriculture and Non-petrol natural resource extraction
            list_industrycode_optionset.Add(new OptionSet() { Val = ("3"), Label = Language.lead_3_industry, });
            //Broadcasting printing and Publishing
            list_industrycode_optionset.Add(new OptionSet() { Val = ("4"), Label = Language.lead_4_industry, });
            //Brokers
            list_industrycode_optionset.Add(new OptionSet() { Val = ("5"), Label = Language.lead_5_industry, });
            //Building supply retail
            list_industrycode_optionset.Add(new OptionSet() { Val = ("6"), Label = Language.lead_6_industry, });
            //Business services
            list_industrycode_optionset.Add(new OptionSet() { Val = ("7"), Label = Language.lead_7_industry, });
            //Consulting
            list_industrycode_optionset.Add(new OptionSet() { Val = ("8"), Label = Language.lead_8_industry, });
            //Consumer services
            list_industrycode_optionset.Add(new OptionSet() { Val = ("9"), Label = Language.lead_9_industry, });
            //Design, direction and creative management
            list_industrycode_optionset.Add(new OptionSet() { Val = ("10"), Label = Language.lead_10_industry, });
            //Distributors, dispatchers and processors
            list_industrycode_optionset.Add(new OptionSet() { Val = ("11"), Label = Language.lead_11_industry, });
            //Doctor's offices and clinics
            list_industrycode_optionset.Add(new OptionSet() { Val = ("12"), Label = Language.lead_12_industry, });
            //Durable manufacturing
            list_industrycode_optionset.Add(new OptionSet() { Val = ("13"), Label = Language.lead_13_industry, });
            //Eating and drinking places
            list_industrycode_optionset.Add(new OptionSet() { Val = ("14"), Label = Language.lead_14_industry, });
            //Entertainment retail
            list_industrycode_optionset.Add(new OptionSet() { Val = ("15"), Label = Language.lead_15_industry, });
            //Equipment rental and leasing
            list_industrycode_optionset.Add(new OptionSet() { Val = ("16"), Label = Language.lead_16_industry, });
            //Financial
            list_industrycode_optionset.Add(new OptionSet() { Val = ("17"), Label = Language.lead_17_industry, });
            //Food and tobacco processing
            list_industrycode_optionset.Add(new OptionSet() { Val = ("18"), Label = Language.lead_18_industry, });
            //Inbound capital intensive processing
            list_industrycode_optionset.Add(new OptionSet() { Val = ("19"), Label = Language.lead_19_industry, });
            //Inbound repair and services
            list_industrycode_optionset.Add(new OptionSet() { Val = ("20"), Label = Language.lead_20_industry, });
            //Insurance
            list_industrycode_optionset.Add(new OptionSet() { Val = ("21"), Label = Language.lead_21_industry, });
            //Legal services
            list_industrycode_optionset.Add(new OptionSet() { Val = ("22"), Label = Language.lead_22_industry, });
            //Non-Durable merchandise retail
            list_industrycode_optionset.Add(new OptionSet() { Val = ("23"), Label = Language.lead_23_industry, });
            //Outbound consumer service
            list_industrycode_optionset.Add(new OptionSet() { Val = ("24"), Label = Language.lead_24_industry, });
            //Petrochemical extraction and distribution
            list_industrycode_optionset.Add(new OptionSet() { Val = ("25"), Label = Language.lead_25_industry, });
            //Service retail
            list_industrycode_optionset.Add(new OptionSet() { Val = ("26"), Label = Language.lead_26_industry, });
            //SIG affiliations
            list_industrycode_optionset.Add(new OptionSet() { Val = ("27"), Label = Language.lead_27_industry, });
            //Social services
            list_industrycode_optionset.Add(new OptionSet() { Val = ("28"), Label = Language.lead_28_industry, });
            //Special outbound trade contractors
            list_industrycode_optionset.Add(new OptionSet() { Val = ("29"), Label = Language.lead_29_industry, });
            //Specialty realty
            list_industrycode_optionset.Add(new OptionSet() { Val = ("30"), Label = Language.lead_30_industry, });
            //Transportation
            list_industrycode_optionset.Add(new OptionSet() { Val = ("31"), Label = Language.lead_31_industry, });
            //Utility creation and distribution
            list_industrycode_optionset.Add(new OptionSet() { Val = ("32"), Label = Language.lead_32_industry, });
            //Vehicle retail
            list_industrycode_optionset.Add(new OptionSet() { Val = ("33"), Label = Language.lead_33_industry, });
            //Wholesale
        }

        ////////// CAMPAIGN LOOKP AREA
        public async Task LoadCampainsForLookup()
        {
            string fetch = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='campaign'>
                                    <attribute name='name' alias='Label'/>
                                    <attribute name='campaignid' alias='Val'/>
                                    <order attribute='name' descending='true' />
                                </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<OptionSet>>("campaigns", fetch);
            if (result == null || result.value.Count == 0) return;

            foreach (var x in result.value)
            {
                list_campaign_lookup.Add(x);
            }
        }
    }
}