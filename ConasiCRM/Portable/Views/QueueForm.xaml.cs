using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QueueForm : ContentPage
    {
        public Action<bool> CheckQueueInfo;
        public QueueFormViewModel viewModel;
        public Guid UnitId;
        public Guid QueueId;
        public QueueForm(Guid unitId, bool fromDirectSale) // Direct Sales (add)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new QueueFormViewModel();

            QueueNumberLabel.IsVisible = false;
            QueueNumberValue.IsVisible = false;
            btnDatCho.IsVisible = true;
            UnitId = unitId;

            viewModel.ModalLookUp = modalLookUp;
            viewModel.InitializeModal();
            viewModel.AfterLookUpClose += AfterLookUpClose;
            Init();
            LoadFromUnit();
        }

        public QueueForm(Guid _queueId) // update
        {
            InitializeComponent();
            QueueNumberLabel.IsVisible = true;
            QueueNumberValue.IsVisible = true;

            this.BindingContext = viewModel = new QueueFormViewModel();
            QueueId = _queueId;
            this.cb_bsd_collectedqueuingfee.IsVisible = true;
            this.cb_bsd_collectedqueuingfeeLabel.IsVisible = true;

            viewModel.ModalLookUp = modalLookUp;
            viewModel.InitializeModal();
            viewModel.AfterLookUpClose += AfterLookUpClose;
            Init();
            InitUpdate();
        }

        public async void Init()
        {
            lookUpDaiLy.PreOpenAsync = viewModel.LoadSalesAgent;
            lookUpCollaborator.PreOpenAsync = viewModel.LoadCollaborator;
            lookUpCustomerReferral.PreOpenAsync = viewModel.LoadCustomerReferral;
        }

        public async void InitUpdate()
        {
            await Load();
            if (viewModel.QueueFormModel != null)
                CheckQueueInfo(true);
            else
                CheckQueueInfo(false);
        }

        private void BsdLookUp_OpenClicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.InitCustomerLookUpHeader();
            viewModel.BtnContact.Clicked += ContactOpen;
            viewModel.BtnAccount.Clicked += AccountOpen;
            ContactOpen(viewModel.BtnContact, EventArgs.Empty);
            LoadingHelper.Hide();
        }

        public void ContactOpen(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.gridCustomer.IsVisible = true;
            if (viewModel.AccountLookUpConfig.ListView != null) viewModel.AccountLookUpConfig.ListView.IsVisible = false;
            OnSwitch();
            viewModel.BtnContact.BackgroundColor = Color.FromHex("#999999");
            viewModel.BtnAccount.BackgroundColor = Color.Transparent;
            viewModel.CurrentLookUpConfig = viewModel.ContactLookUpConfig;
            viewModel.ProcessLookup(nameof(viewModel.ContactLookUpConfig));
            LoadingHelper.Hide();
        }

        public void AccountOpen(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.gridCustomer.IsVisible = true;
            viewModel.ContactLookUpConfig.ListView.IsVisible = false;
            OnSwitch();
            viewModel.BtnContact.BackgroundColor = Color.Transparent;
            viewModel.BtnAccount.BackgroundColor = Color.FromHex("#999999");
            viewModel.CurrentLookUpConfig = viewModel.AccountLookUpConfig;
            viewModel.ProcessLookup(nameof(viewModel.AccountLookUpConfig));
            LoadingHelper.Hide();
        }

        public void OnSwitch()
        {
            if (viewModel.searchBar.Text != null && viewModel.searchBar.Text.Length > 0)
            {
                viewModel.CurrentLookUpConfig.LookUpData.Clear();
                viewModel.searchBar.Text = null;
            }
        }

        public void AfterLookUpClose(object sender, EventArgs e)
        {
            viewModel.BtnContact.Clicked -= ContactOpen;
            viewModel.BtnAccount.Clicked -= AccountOpen;
            viewModel.gridCustomer.IsVisible = false;
        }

        public async void LoadFromUnit()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='product'>
                <attribute name='productid' />
                <attribute name='bsd_units' />
                <attribute name='name' />
                <attribute name='productnumber' />
                <attribute name='bsd_queuingfee' />
                <attribute name='bsd_depositamount' />
                <attribute name='bsd_vippriority' />
                <attribute name='statuscode' />
                <attribute name='bsd_areavariance' />
                <attribute name='bsd_constructionarea'/>
                <attribute name='bsd_netsaleablearea'/>
                <attribute name='price'/>
                <attribute name='bsd_landvalueofunit' />
                <attribute name='bsd_landvalue'/>
                <attribute name='bsd_maintenancefeespercent' />
                <attribute name='bsd_maintenancefees' />
                <attribute name='bsd_taxpercent' />
                <attribute name='bsd_vat' />
                <attribute name='bsd_totalprice' />
                <attribute name='bsd_estimatehandoverdate' />
                <attribute name='bsd_numberofmonthspaidmf' />
                <attribute name='bsd_managementamountmonth' />
                <attribute name='bsd_handovercondition' />
                <attribute name='bsd_handovercondition' />
                <filter type='and'>
                  <condition attribute='productid' operator='eq' uitype='product' value='" + UnitId.ToString() + @"' />
                </filter>
                <link-entity name='bsd_project' from='bsd_projectid' to='bsd_projectcode' visible='false' link-type='outer' alias='a_a77d98e66ce2e811a94e000d3a1bc2d1'>
                  <attribute name='bsd_name' alias='bsd_project_name' />
                  <attribute name='bsd_projectid' alias='bsd_project_id' />
                </link-entity>
                <link-entity name='bsd_floor' from='bsd_floorid' to='bsd_floor' visible='false' link-type='outer' alias='a_4d73a1e06ce2e811a94e000d3a1bc2d1'>
                  <attribute name='bsd_name' alias='bsd_floor_name' />
                  <attribute name='bsd_floorid' alias='bsd_floor_id' />
                </link-entity>
                <link-entity name='bsd_block' from='bsd_blockid' to='bsd_blocknumber' visible='false' link-type='outer' alias='a_290ca3da6ce2e811a94e000d3a1bc2d1'>
                  <attribute name='bsd_name' alias='bsd_block_name' />
                  <attribute name='bsd_blockid' alias='bsd_block_id' />
                </link-entity>
                <link-entity name='bsd_unittype' from='bsd_unittypeid' to='bsd_unittype' visible='false' link-type='outer' alias='a_493690ec6ce2e811a94e000d3a1bc2d1'>
                  <attribute name='bsd_name'  alias='bsd_unittype_name'/>
                </link-entity>
                <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaseslaunchid' visible='false' link-type='outer' alias='a_8d7b98e66ce2e811a94e000d3a1bc2d1'>
                      <attribute name='bsd_name' alias='bsd_phaseslaunch_name' />
                      <attribute name='bsd_phaseslaunchid' alias='bsd_phaseslaunch_id' />
                      <link-entity name='pricelevel' from='pricelevelid' to='bsd_pricelistid' visible='false' link-type='outer' alias='aa'>
                           <attribute name='name' alias='pricelist_name' />
                           <attribute name='pricelevelid' alias='pricelist_id' />
                      </link-entity>
                 </link-entity>
              </entity>
            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<UnitInfoModel>>("products", fetchXml);
            if (result == null || result.value.Count == 0)
            {
                CheckQueueInfo?.Invoke(false);
            }
            else
            {
                viewModel.Title = "Tạo đặt chỗ";
                UnitInfoModel unitInfo = result.value.FirstOrDefault();

                QueueFormModel queueFormModel = new QueueFormModel();
                queueFormModel.name = unitInfo.name;

                queueFormModel.bsd_project_id = unitInfo.bsd_project_id;
                queueFormModel.bsd_project_name = unitInfo.bsd_project_name;

                queueFormModel.bsd_phaseslaunch_id = unitInfo.bsd_phaseslaunch_id;
                queueFormModel.bsd_phaseslaunch_name = unitInfo.bsd_phaseslaunch_name;


                queueFormModel.bsd_block_id = unitInfo.bsd_block_id;
                queueFormModel.bsd_block_name = unitInfo.bsd_block_name;

                queueFormModel.bsd_floor_id = unitInfo.bsd_floor_id;
                queueFormModel.bsd_floor_name = unitInfo.bsd_floor_name;

                queueFormModel.bsd_units_id = UnitId;
                queueFormModel.bsd_units_name = unitInfo.name;

                queueFormModel.pricelist_id = unitInfo.pricelist_id;
                queueFormModel.pricelist_name = unitInfo.pricelist_name;

                queueFormModel.constructionarea = unitInfo.bsd_constructionarea;
                queueFormModel.netsaleablearea = unitInfo.bsd_netsaleablearea;

                queueFormModel.bsd_queuingfee = unitInfo.bsd_queuingfee;

                queueFormModel.landvalue = unitInfo.bsd_landvalue;

                queueFormModel.unit_price = unitInfo.price;

                viewModel.QueueFormModel = queueFormModel;
                gridBtnGroup.IsVisible = btnDatCho.IsVisible = true;
                CheckQueueInfo?.Invoke(true);
            }
            
        }

        public async Task Load()
        {
            string fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='opportunity'>
                   <attribute name='name' />
                    <attribute name='statecode' />
                    <attribute name='customerid' alias='customer_id'/>
                    <attribute name='emailaddress' />
                    <attribute name='bsd_queuenumber' />
                    <attribute name='statuscode' />
                    <attribute name='bsd_queuingfee' />
                    <attribute name='bsd_project' alias='project_id' />
                    <attribute name='bsd_phaselaunch' />
                    <attribute name='createdon' />
                    <attribute name='bsd_queuingexpired' />
                    <attribute name='statuscode' />
                    <attribute name='opportunityid' />
                    <attribute name='bsd_customerreferral' />
                    <attribute name='bsd_salesagentcompany' />
                    <attribute name='bsd_collaborator' />
                    <order attribute='createdon' descending='true' />
                   <link-entity name='contact' from='contactid' to='parentcontactid' visible='false' link-type='outer' alias='a_7eff24578704e911a98b000d3aa2e890'>
                         <attribute name='contactid' alias='contact_id' />
                         <attribute name='bsd_fullname' alias='contact_name' />
                   </link-entity>
                   <link-entity name='account' from='accountid' to='parentaccountid' visible='false' link-type='outer' alias='a_77ff24578704e911a98b000d3aa2e890'>
                          <attribute name='accountid' alias='account_id' />
                           <attribute name='bsd_name' alias='account_name' />
                   </link-entity>
                   <link-entity name='bsd_project' from='bsd_projectid' to='bsd_project' visible='false' link-type='outer' alias='a_910b05eeb214e911a97f000d3aa04914'>
                         <attribute name='bsd_name' alias='bsd_project_name' />
                   </link-entity>
                   <link-entity name='bsd_phaseslaunch' from='bsd_phaseslaunchid' to='bsd_phaselaunch' visible='false' link-type='outer' alias='a_bfb1e1e7b214e911a97f000d3aa04914'>
                          <attribute name='bsd_name' alias='bsd_phaseslaunch_name' />
                          <attribute name='bsd_discountlist' alias='bsd_discountlist_id' />
                   </link-entity>
                   <link-entity name='pricelevel' from='pricelevelid' to='pricelevelid' visible='false' link-type='outer' alias='a_fcff24578704e911a98b000d3aa2e890'>
                        <attribute name='name' alias='pricelist_name' />
                   </link-entity>
                    <link-entity name='product' from='productid' to='bsd_units' visible='false' link-type='outer' alias='a_b088efffb214e911a97f000d3aa04914'>
                        <attribute name='productid' alias='bsd_units_id' />
                          <attribute name='name' alias='bsd_units_name' />
                          <attribute name='bsd_constructionarea' alias='constructionarea' />
                          <attribute name='bsd_netsaleablearea' alias='netsaleablearea' />
                          <attribute name='bsd_landvalue' alias='landvalue' />
                          <attribute name='price' alias='unit_price' />
                          <link-entity name='bsd_block' from='bsd_blockid' to='bsd_blocknumber' visible='false' link-type='outer' alias='aj'>
                                <attribute name='bsd_name' alias='bsd_block_name' />
                          </link-entity>
                          <link-entity name='bsd_floor' from='bsd_floorid' to='bsd_floor' visible='false' link-type='outer' alias='ak'>
                                <attribute name='bsd_name' alias='bsd_floor_name' />
                          </link-entity>
                    </link-entity>
                    <filter type='and'>
                       <condition attribute='opportunityid' operator='eq' uitype='opportunity' value='" + QueueId.ToString() + @"' />
                    </filter>
              </entity>
            </fetch>";


            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueueFormModel>>("opportunities", fetchXml);
            var queueInfo = result.value.FirstOrDefault();

            //vi gioi han toi da chi duoc 10 entity. nen lay 2 laan api
            string fetchXml2 = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
              <entity name='opportunity'>
                    <attribute name='opportunityid' />
                    <order attribute='createdon' descending='true' />
                    <link-entity name='contact' from='contactid' to='bsd_collaborator' visible='false' link-type='outer' alias='a_4913879edf72e911a83a000d3a80e651'>
                        <attribute name='contactid' alias='bsd_collaborator_contact_id' />
                        <attribute name='bsd_fullname' alias='bsd_collaborator_name'/>
                    </link-entity>
                    <link-entity name='account' from='accountid' to='bsd_salesagentcompany' visible='false' link-type='outer' alias='a_d6e4a386df72e911a83a000d3a80e651'>
                        <attribute name='accountid' alias='bsd_salesagentcompany_account_id' />
                        <attribute name='bsd_name' alias='bsd_salesagentcompany_name'/>
                    </link-entity>
                    <link-entity name='account' from='accountid' to='bsd_customerreferral' visible='false' link-type='outer' alias='a_cee4a386df72e911a83a000d3a80e651'>
                        <attribute name='accountid' alias='bsd_customerreferral_account_id' />
                        <attribute name='bsd_name' alias='bsd_customerreferral_name'/>
                    </link-entity>
                    <filter type='and'>
                       <condition attribute='opportunityid' operator='eq' uitype='opportunity' value='" + QueueId.ToString() + @"' />
                    </filter>
              </entity>
            </fetch>";
            var result2 = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<QueueFormModel>>("opportunities", fetchXml2);
            var queueInfo2 = result2.value.FirstOrDefault();

            if (queueInfo != null && queueInfo2 != null)
            {
                QueueFormModel queueFormModel = new QueueFormModel();
                queueFormModel.bsd_queuenumber = queueInfo.bsd_queuenumber;
                queueFormModel.name = queueInfo.name;
                queueFormModel.statuscode = queueInfo.statuscode;

                Contact contactCollaborator = new Contact() { contactid = queueInfo2.bsd_collaborator_contact_id, bsd_fullname = queueInfo2.bsd_collaborator_name };
                viewModel.CollaboratorOption = contactCollaborator;

                Account accountsalesagentcompany = new Account() { accountid = queueInfo2.bsd_salesagentcompany_account_id, name = queueInfo2.bsd_salesagentcompany_name };
                viewModel.DailyOption = accountsalesagentcompany;

                Account accountcustomerreferral = new Account() { accountid = queueInfo2.bsd_customerreferral_account_id, Name = queueInfo2.bsd_customerreferral_name };
                viewModel.CustomerReferralOption = accountcustomerreferral;

                if (queueInfo.contact_id != Guid.Empty)
                {
                    viewModel.Customer = new CustomerLookUp()
                    {
                        Id = queueInfo.contact_id,
                        Name = queueInfo.contact_name,
                        Type = 1
                    };
                }
                else if (queueInfo.account_id != Guid.Empty)
                {
                    viewModel.Customer = new CustomerLookUp()
                    {
                        Id = queueInfo.account_id,
                        Name = queueInfo.account_name,
                        Type = 2
                    };
                }

                queueFormModel.createdon = queueInfo.createdon;
                queueFormModel.bsd_queuingexpired = queueInfo.bsd_queuingexpired;


                //queueFormModel.bsd_project_id = queueInfo.bsd_project_id;
                queueFormModel.bsd_project_name = queueInfo.bsd_project_name;

                //queueFormModel.bsd_phaseslaunch_id = queueInfo.bsd_phaseslaunch_id;
                queueFormModel.bsd_phaseslaunch_name = queueInfo.bsd_phaseslaunch_name;
                queueFormModel.bsd_discountlist_id = queueInfo.bsd_discountlist_id; // lấy để đưa qua đặt cọc. 

                //queueFormModel.bsd_block_id = queueInfo.bsd_block_id;
                queueFormModel.bsd_block_name = queueInfo.bsd_block_name;

                //queueFormModel.bsd_floor_id = queueInfo.bsd_floor_id;
                queueFormModel.bsd_floor_name = queueInfo.bsd_floor_name;

                queueFormModel.bsd_units_id = queueInfo.bsd_units_id; // lay id de dat coc.
                queueFormModel.bsd_units_name = queueInfo.bsd_units_name;

                //queueFormModel.pricelist_id = queueInfo.pricelist_id;
                queueFormModel.pricelist_name = queueInfo.pricelist_name;

                queueFormModel.constructionarea = queueInfo.constructionarea;
                queueFormModel.netsaleablearea = queueInfo.netsaleablearea;
                queueFormModel.bsd_queuingfee = queueInfo.bsd_queuingfee;
                queueFormModel.bsd_collectedqueuingfee = queueInfo.bsd_collectedqueuingfee;// đa nhan tien
                queueFormModel.landvalue = queueInfo.landvalue;
                queueFormModel.unit_price = queueInfo.unit_price;
                viewModel.QueueFormModel = queueFormModel;
                InitBtn();
            }

            viewModel.Title = "Thông tin đặt chỗ";
            viewModel.IsBusy = false;
        }

        public void InitBtn()
        {
            int status = viewModel.QueueFormModel.statuscode;
            // ẩn hiện nút hủy đặt chỗ.
            if (status == 1 || status == 3 || status == 4 || status == 100000003 || status == 100000004 || status == 100000008 || status == 100000009 || status == 100000010) // cancel;expired;completed
            {
                btnHuyDatCho.IsVisible = false;
            }
            else
            {
                gridBtnGroup.IsVisible = btnHuyDatCho.IsVisible = true;
            }


            if (status == 1) // draft van cho datcho.
            {
                gridBtnGroup.IsVisible = btnDatCho.IsVisible = true;
            }
            else
            {
                btnDatCho.IsVisible = false;
            }

            // trang thai la queuing va phi dat cho = 0 hoac da tra tien phi dat cho.
            if ((status == 100000000) && (viewModel.QueueFormModel.bsd_queuingfee == 0 || viewModel.QueueFormModel.bsd_collectedqueuingfee))
            {
                gridBtnGroup.IsVisible = btnDatCoc.IsVisible = true;
            }
            else
            {
                btnDatCoc.IsVisible = false;
            }
            InitButtonGroup();
        }

        // set width cho button.
        private void InitButtonGroup()
        {
            gridBtnGroup.ColumnDefinitions.Clear();
            var buttons = gridBtnGroup.Children.Where(x => x is RadButton && x.IsVisible == true).ToList();
            var count = buttons.Count();
            for (int i = 0; i < count; i++)
            {
                gridBtnGroup.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star),
                });
                Grid.SetColumn(buttons[i], i);
            }
        }

        private async void Queue_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            if (viewModel.Customer == null)
            {
                await DisplayAlert("Thông báo", "Vui lòng chọn khách hàng", "Đóng");
                viewModel.IsBusy = false;
                return;
            }

            bool confirm = await DisplayAlert("Xác nhận", "Bạn có muốn đặt chỗ không ?", "Đồng ý", "Hủy");
            if (confirm == false) return;

            if (QueueId == Guid.Empty) // new
            {
                string url_action = "/opportunities";
                var data = await getContent();

                CrmApiResponse res = await CrmHelper.PostData(url_action, data);
                if (res.IsSuccess)
                {
                    ActionDirectSaleMoble_Result actionDirectSaleMoble_Result = JsonConvert.DeserializeObject<ActionDirectSaleMoble_Result>(res.Content);
                    //await Navigation.PushAsync(new QueueForm(Guid.Parse(actionDirectSaleMoble_Result.ReturnId)));
                    Navigation.RemovePage(this);
                }
                else
                {
                    await DisplayAlert("Thông báo", "Đặt chỗ thất bại." + res.GetErrorMessage(), "Đóng");
                }
            }
            else
            {
                IDictionary<string, object> data = new Dictionary<string, object>();
                CrmApiResponse clearLookupResponse = new CrmApiResponse()
                {
                    IsSuccess = true
                };
                if (viewModel.Customer.Type == 1)
                {
                    data["customerid_contact@odata.bind"] = $"/contacts({viewModel.Customer.Id})";
                    clearLookupResponse = await CrmHelper.SetNullLookupField("opportunities", this.QueueId, "customerid_account");
                }
                else
                {
                    data["customerid_account@odata.bind"] = $"/accounts({viewModel.Customer.Id})";
                    clearLookupResponse = await CrmHelper.SetNullLookupField("opportunities", this.QueueId, "customerid_contact");
                }

                if (viewModel.DailyOption == null || viewModel.DailyOption.accountid == Guid.Empty)
                {
                    clearLookupResponse = await CrmHelper.SetNullLookupField("opportunities", this.QueueId, "bsd_salesagentcompany");
                }
                else
                {
                    data["bsd_salesagentcompany@odata.bind"] = $"/accounts({viewModel.DailyOption.accountid})";
                }

                if (viewModel.CollaboratorOption == null || viewModel.CollaboratorOption.contactid == Guid.Empty)
                {
                    clearLookupResponse = await CrmHelper.SetNullLookupField("opportunities", this.QueueId, "bsd_collaborator");
                }
                else
                {
                    data["bsd_collaborator@odata.bind"] = $"/contacts({viewModel.CollaboratorOption.contactid})";
                }

                if (viewModel.CustomerReferralOption == null || viewModel.CustomerReferralOption.accountid == Guid.Empty)
                {
                    clearLookupResponse = await CrmHelper.SetNullLookupField("opportunities", this.QueueId, "bsd_customerreferral_account");
                }
                else
                {
                    data["bsd_customerreferral_account@odata.bind"] = $"/accounts({viewModel.CustomerReferralOption.accountid})";
                }

                if (clearLookupResponse.IsSuccess)
                {
                    CrmApiResponse res = await CrmHelper.PatchData($"/opportunities({this.QueueId})", data);
                    if (res.IsSuccess)
                    {
                        await DisplayAlert("Thông báo", "Cập nhật thành công", "Đóng");
                        await Load();
                    }
                    else
                    {
                        await DisplayAlert("Thông báo", res.GetErrorMessage(), "close");
                    }
                }
                else
                {
                    await DisplayAlert("Thông báo", clearLookupResponse.GetErrorMessage(), "close");
                }
            }
            viewModel.IsBusy = false;
        }

        private async Task<object> getContent()
        {
            IDictionary<string, object> data = new Dictionary<string, object>();
            CrmApiResponse clearLookupResponse = new CrmApiResponse();
            data["opportunityid"] = Guid.NewGuid();
            data["name"] = viewModel.QueueFormModel.name;
            data["bsd_queuingfee"] = viewModel.QueueFormModel.bsd_queuingfee;
            data["estimatedvalue"] = viewModel.QueueFormModel.unit_price;

            data["pricelevelid@odata.bind"] = $"/pricelevels({viewModel.QueueFormModel.pricelist_id})";

            data["bsd_units@odata.bind"] = $"/products({viewModel.QueueFormModel.bsd_units_id})";

            data["bsd_Project@odata.bind"] = $"/bsd_projects({viewModel.QueueFormModel.bsd_project_id})";

            if (viewModel.QueueFormModel.bsd_phaseslaunch_id != Guid.Empty)
            {
                data["bsd_phaselaunch@odata.bind"] =$"/bsd_phaseslaunchs({viewModel.QueueFormModel.bsd_phaseslaunch_id})";
            }
            else
            {
                await CrmHelper.SetNullLookupField("opportunities", this.QueueId, "bsd_phaseslaunch");
            }

            if (viewModel.Customer.Type == 1)
            {
                data["customerid_contact@odata.bind"] = $"/contacts({viewModel.Customer.Id})";
                clearLookupResponse = await CrmHelper.SetNullLookupField("opportunities", this.QueueId, "customerid_account");
            }
            else
            {
                data["customerid_account@odata.bind"] = $"/accounts({viewModel.Customer.Id})";
                clearLookupResponse = await CrmHelper.SetNullLookupField("opportunities", this.QueueId, "customerid_contact");
            }

            if (viewModel.DailyOption == null || viewModel.DailyOption.accountid == Guid.Empty)
            {
                clearLookupResponse = await CrmHelper.SetNullLookupField("opportunities", this.QueueId, "bsd_salesagentcompany");
            }
            else
            {
                data["bsd_salesagentcompany@odata.bind"] = $"/accounts({viewModel.DailyOption.accountid})";
            }

            if (viewModel.CollaboratorOption == null || viewModel.CollaboratorOption.contactid == Guid.Empty)
            {
                clearLookupResponse = await CrmHelper.SetNullLookupField("opportunities", this.QueueId, "bsd_collaborator");
            }
            else
            {
                data["bsd_collaborator@odata.bind"] = $"/contacts({viewModel.CollaboratorOption.contactid})";
            }

            if (viewModel.CustomerReferralOption == null || viewModel.CustomerReferralOption.accountid == Guid.Empty)
            {
                clearLookupResponse = await CrmHelper.SetNullLookupField("opportunities", this.QueueId, "bsd_customerreferral_account");
            }
            else
            {
                data["bsd_customerreferral_account@odata.bind"] = $"/accounts({viewModel.CustomerReferralOption.accountid})";
            }

            return data;
        }

        private async void CancleQueue_Clicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Xác nhận", "Bạn có muốn hủy đặt chỗ này không ?", "Đồng ý", "Hủy");
            if (confirm == false) return;

            viewModel.IsBusy = true;
            if (this.QueueId != Guid.Empty)
            {
                string url_action = $"/opportunities({this.QueueId})/Microsoft.Dynamics.CRM.bsd_Action_Queue_CancelQueuing";
                CrmApiResponse res = await CrmHelper.PostData(url_action, null);
                if (res.IsSuccess)
                {
                    url_action = $"/opportunities({this.QueueId})/Microsoft.Dynamics.CRM.bsd_Action_Opportunity_HuyGiuChoCoTien";
                    res = await CrmHelper.PostData(url_action, null);
                    //await Navigation.PushAsync(new QueueForm(this.QueueId));
                    Navigation.RemovePage(this);
                }
                else
                {
                    await DisplayAlert("Thông báo", "Hủy báo giá thất bại." + res.GetErrorMessage(), "close");
                }
            }
            viewModel.IsBusy = false;
        }

        private async void BtnDatCoc_Clicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Xác nhận", "Bạn có muốn tạo báo giá không ?", "Đồng ý", "Hủy");
            if (confirm == false) return;

            viewModel.IsBusy = true;
            var data = new
            {
                Command = "Reservation",
                Parameters = "[" + JsonConvert.SerializeObject(new
                {
                    action = "Reservation",
                    name = "opportunity",
                    value = this.QueueId.ToString()
                }) + "]"
            };

            var res = await CrmHelper.PostData($"/products({viewModel.QueueFormModel.bsd_units_id})//Microsoft.Dynamics.CRM.bsd_Action_DirectSale", data);
            if (res.IsSuccess)
            {
                DirectSaleActionResponse directSaleActionResponse = JsonConvert.DeserializeObject<DirectSaleActionResponse>(res.Content);
                DirectSaleActionSubResponse subResponse = directSaleActionResponse.GetSubResponse();
                if (subResponse.type == "Success")
                {
                    // tạo báo giá thành công, thì lấy Discount List từ PhasedLanch đưa vào Reservation.
                    // tạo báo giá thành công thì cũng cập nhật lại field Queue trên đặt cọc.
                    Guid CreateReservationId = Guid.Parse(subResponse.content);
                    if (viewModel.QueueFormModel.bsd_discountlist_id != Guid.Empty)
                    {
                        // update lại reservation field discount list
                        var updateData = new Dictionary<string, object>();
                        updateData["bsd_discountlist@odata.bind"] = $"discounttypes({viewModel.QueueFormModel.bsd_discountlist_id})";
                        var updateDiscountListResponse = await CrmHelper.PatchData($"/quotes({CreateReservationId})", updateData);
                        if (updateDiscountListResponse.IsSuccess == false)
                        {
                            await DisplayAlert("Thông báo", "Tạo báo giá thành công. Không cập nhật được Discount List từ Đợt mở bán", "Đóng");
                        }
                        else
                        {
                            await DisplayAlert("Thông báo", "Tạo báo giá thành công. ", "Đóng");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Thông báo", "Tạo báo giá thành công. ", "Đóng");
                    }
                    await Navigation.PushAsync(new ReservationForm(CreateReservationId));
                }
                else await DisplayAlert("Thông báo", "Tạo báo giá thất bạn. " + subResponse.content, "Đóng");
            }
            else await DisplayAlert("Thông báo", "Tạo báo giá thất bại ." + res.GetErrorMessage(), "close");
            viewModel.IsBusy = false;
        }

        private async void KhachHangGioiThieuLookUp_OpenClicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.InitCustomerLookUpHeader();
            viewModel.BtnContact.Clicked += ContactOpen;
            viewModel.BtnAccount.Clicked += AccountOpen;
            ContactOpen(viewModel.BtnContact, EventArgs.Empty);
        }

        private void lookUpDaiLy_SelectedItemChange(System.Object sender, ConasiCRM.Portable.Models.LookUpChangeEvent e)
        {
            viewModel.CollaboratorOption = null;
        }

        private void lookUpCollaborator_SelectedItemChange(System.Object sender, ConasiCRM.Portable.Models.LookUpChangeEvent e)
        {
            viewModel.DailyOption = null;
        }
    }

    class ActionDirectSaleMoble_Result
    {
        public int status { get; set; }
        public string ReturnId { get; set; }
        public string Message { get; set; }
    }
}