using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhanHoiForm : ContentPage
    {
        public Action<bool> CheckPhanHoi;
        public PhanHoiFormViewModel viewModel;

        public PhanHoiForm()
        {
            InitializeComponent();
            Init();
        }
        public PhanHoiForm(Guid incidentid)
        {
            InitializeComponent();
            Init();
        }

        public async void Init()
        {
            this.BindingContext = viewModel = new PhanHoiFormViewModel();
            viewModel.TabsDoiTuong = new List<string>() { "Giữ chỗ,", "Bảng Tính Giá", "Hợp đồng" };
            SerPreOpen();


        }

        private void SerPreOpen()
        {
            lookupCaseType.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.CaseTypes = CaseTypeData.CasesData();
                LoadingHelper.Hide();
            };

            lookupSubjects.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadSubjects();
                LoadingHelper.Hide();
            };

            lookupCaseLienQuan.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadCaseLienQuan();
                LoadingHelper.Hide();
            };

            lookupCaseOrigin.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.CaseOrigins = OriginData.Origins();
                LoadingHelper.Hide();
            };           

            lookupProjects.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadProjects();
                LoadingHelper.Hide();
            };

            //multiTabsDoiTuong.PreOpenOneTime = false;
            //multiTabsDoiTuong.PreOpen = () =>
            //{
            //    LoadingHelper.Show();
            //    if (viewModel.Customer == null)
            //    {
            //        LoadingHelper.Hide();
            //        ToastMessageHelper.ShortMessage("Vui lòng chọn khách hàng");
            //        return;
            //    }
            //    LoadingHelper.Hide();
            //};
        }

        private async void CustomerItem_Changed(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.DoiTuong = null;
            viewModel.Queues = null;
            viewModel.Quotes = null;
            viewModel.OptionEntries = null;
            if (viewModel.AllItemSourceDoiTuong != null)
            {
                viewModel.AllItemSourceDoiTuong.Clear();
            }
            
            await Task.WhenAll(
                    viewModel.LoadQueues(),
                    viewModel.LoadQuotes(),
                    viewModel.LoadOptionEntries()
                    );
            viewModel.AllItemSourceDoiTuong = new List<List<OptionSet>>() { viewModel.Queues, viewModel.Quotes, viewModel.OptionEntries };
            LoadingHelper.Hide();
        }

        private async void ProjectItem_Changed(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.Unit = null;
            viewModel.Units = null;
            await viewModel.LoadUnits();
            LoadingHelper.Hide();
        }

        private async void SaveCase_Clicked(object sender, EventArgs e)
        {
            if (viewModel.CaseType == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn loại phản hồi");
                return;
            }

            if (string.IsNullOrWhiteSpace(viewModel.singlePhanHoi.title))
            {
                ToastMessageHelper.ShortMessage("Vui lòng nhập tiêu đề");
                return;
            }

            if (viewModel.Customer == null)
            {
                ToastMessageHelper.ShortMessage("Vui lòng chọn khách hàng");
                return;
            }

            LoadingHelper.Show();
            if (viewModel.singlePhanHoi.incidentid == Guid.Empty)
            {
                bool isSuccess = await viewModel.CreateCase();
                if (isSuccess)
                {
                    await Navigation.PopAsync();
                    ToastMessageHelper.ShortMessage("Tạo phản hồi thành công");
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage("Tạo phản hồi thất bại");
                }
            }
        }






        public async Task Start(Guid IncidentId)
        {
            viewModel.IsBusy = true;
            viewModel.Title = "Cập Nhật Phản Hồi";
            //buttonUpdate.IsVisible = true;
            //buttonCreate.IsVisible = false;

            //bsd_status_label.IsVisible = true;
            //bsd_status_text.IsVisible = true;

            if (IncidentId != null) { await viewModel.LoadOnePhanHoi(IncidentId); }
            await viewModel.LoadListSubject();
            await viewModel.LoadListAcc();
            await viewModel.LoadListContact();
            await viewModel.LoadListUnit();
            await viewModel.LoadListLienHe(viewModel.singlePhanHoi._customerid_value);


            //if (viewModel.singlePhanHoi._customerid_value != null)
            //{             
            //    bsd_customer.IsVisible = true;              
            //}
            //if (viewModel.singlePhanHoi._subjectid_value != null)
            //{              
            //    bsd_subject.IsVisible = true;               
            //}
            //if (viewModel.singlePhanHoi._productid_value != null)
            //{
            //    btn_unit.IsVisible = true;
            //    bsd_unit_text.IsVisible = true;
            //    bsd_unit_default.IsVisible = false;
            //}
            //if (viewModel.singlePhanHoi._primarycontactid_value != null)
            //{
            //    btn_contact.IsVisible = true;
            //    bsd_contact_text.IsVisible = true;
            //    bsd_contact_default.IsVisible = false;
            //}

            if (viewModel.singlePhanHoi.caseorigincode != null) { viewModel.getOrigin((viewModel.singlePhanHoi.caseorigincode).ToString()); }

            //if (viewModel.singleAccount.bsd_customergroup != null) { viewModel.getCustomergroup(viewModel.singleAccount.bsd_customergroup); }

            viewModel.IsBusy = false;
        }

        void Clearvalue_origin(object sender, System.EventArgs e)
        {
            //origin_value.SelectedItem = null;
            //btn_origin.IsVisible = false;
            viewModel.singlePhanHoi.caseorigincode = 0;
        }

        void Selectvalue_origin(object sender, System.EventArgs e)
        {
            //btn_origin.IsVisible = true;

            viewModel.singlePhanHoi.caseorigincode = int.Parse(viewModel.singleOrigin == null ? "0" : viewModel.singleOrigin.Val);

        }

        private async Task<String> checkData()
        {

            if (string.IsNullOrWhiteSpace(viewModel.singlePhanHoi.title) || viewModel.singlePhanHoi.customerid == null
             || viewModel.singlePhanHoi.customerid == "")
            {
                return "Vui lòng nhập các thông tin bắt buộc!";
            }
            else
            {
                return "Sucesses";
            }
        }

        private async void BtnUpdate(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            int Mode = 1;
            var check = await checkData();
            if (viewModel.singlePhanHoi.incidentid == Guid.Empty)
            {
                viewModel.singlePhanHoi.incidentid = Guid.NewGuid();
                Mode = 1;
                if (check == "Sucesses")
                {
                    var created = await createCase(viewModel);

                    if (created != new Guid())
                    {
                        if (ListPhanHoi.NeedToRefresh.HasValue) ListPhanHoi.NeedToRefresh = true;
                        await Navigation.PopAsync();
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Thông báo", "Tạo phản hồi thành công!", "OK");
                        //Xamarin.Forms.Application.Current.Properties["update"] = "1";
                        //this.Start(viewModel.singlePhanHoi.incidentid);
                    }
                    else
                    {
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Thông báo", "Tạo phản hồi thất bại", "OK");
                    }
                }
                else
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Thông báo", check, "OK");
                }
            }
            else
            {
                Mode = 2;
                if (check == "Sucesses")
                {
                    var updated = await updateCase(viewModel);
                    if (updated)
                    {
                        if (ListPhanHoi.NeedToRefresh.HasValue) ListPhanHoi.NeedToRefresh = true;
                        await Navigation.PopAsync();
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Thông báo", "Cập nhật thành công!", "OK");
                        //Xamarin.Forms.Application.Current.Properties["update"] = "1";
                        //this.Start(viewModel.singlePhanHoi.incidentid);
                    }
                    else
                    {
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Thông báo", "Cập nhật thất bại!", "OK");
                    }
                }
                else
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Thông báo", check, "OK");
                }
            }
            viewModel.IsBusy = false;
        }

        public bool checkcreate { get; set; }

        public async Task<Guid> createCase(PhanHoiFormViewModel viewModel)
        {
            checkcreate = true;
            string path = "/incidents";
            viewModel.singlePhanHoi.incidentid = Guid.NewGuid();
            var content = await this.getContent(viewModel);

            CrmApiResponse result = await CrmHelper.PostData(path, content);

            if (result.IsSuccess)
            {
                return viewModel.singlePhanHoi.incidentid;
            }
            else
            {
                var mess = result.ErrorResponse?.error?.message ?? "Đã xảy ra lỗi. Vui lòng thử lại.";
                await App.Current.MainPage.DisplayAlert("Thông báo", mess, "OK");
                return new Guid();
            }

        }

        public async Task<Boolean> updateCase(PhanHoiFormViewModel cases)
        {
            checkcreate = false;
            string path = "/incidents(" + cases.singlePhanHoi.incidentid + ")";
            var content = await this.getContent(cases);
            CrmApiResponse result = await CrmHelper.PatchData(path, content);
            if (result.IsSuccess)
            {
                return true;
            }

            else
            {
                var mess = result.ErrorResponse?.error?.message ?? "Đã xảy ra lỗi. Vui lòng thử lại.";
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", mess, "OK");
                return false;
            }

        }

        public async Task<Boolean> DeletLookup(string fieldName, Guid IncidentId)
        {
            var result = await CrmHelper.SetNullLookupField("incidents", IncidentId, fieldName);
            return result.IsSuccess;
        }

        private async Task<object> getContent(PhanHoiFormViewModel cases)
        {
            if (cases.singlePhanHoi.logicalname == "accounts")
            {
                IDictionary<string, object> data = new Dictionary<string, object>();
                data["incidentid"] = cases.singlePhanHoi.incidentid.ToString();
                data["title"] = cases.singlePhanHoi.title ?? "";
                data["description"] = cases.singlePhanHoi.description ?? "";
                //data["new_solution"] = cases.singlePhanHoi.new_solution ?? "";

                data["caseorigincode"] = cases.singlePhanHoi.caseorigincode != 0 ? cases.singlePhanHoi.caseorigincode.ToString() : null;

                if (checkcreate == false)
                {
                    data["statuscode"] = cases.singlePhanHoi.statuscode;
                }

                if (cases.singlePhanHoi._customerid_value == null)
                {
                    await DeletLookup("customerid_account", cases.singlePhanHoi.incidentid);
                }
                else
                {
                    data["customerid_account@odata.bind"] = "/accounts(" + cases.singlePhanHoi._customerid_value + ")"; /////Lookup Field
                }

                if (cases.singlePhanHoi._subjectid_value == null)
                {
                    await DeletLookup("subjectid", cases.singlePhanHoi.incidentid);
                }
                else
                {
                    data["subjectid@odata.bind"] = "/subjects(" + cases.singlePhanHoi._subjectid_value + ")"; /////Lookup Field
                }

                if (cases.singlePhanHoi._productid_value == null)
                {
                    await DeletLookup("productid", cases.singlePhanHoi.incidentid);
                }
                else
                {
                    data["productid@odata.bind"] = "/products(" + cases.singlePhanHoi._productid_value + ")"; /////Lookup Field
                }
                if (cases.singlePhanHoi._primarycontactid_value == null)
                {
                    await DeletLookup("primarycontactid", cases.singlePhanHoi.incidentid);
                }
                else
                {
                    data["primarycontactid@odata.bind"] = "/contacts(" + cases.singlePhanHoi._primarycontactid_value + ")"; /////Lookup Field
                }
                return data;
            }
            else
            {
                IDictionary<string, object> data = new Dictionary<string, object>();
                data["incidentid"] = cases.singlePhanHoi.incidentid.ToString();
                data["title"] = cases.singlePhanHoi.title ?? "";
                data["description"] = cases.singlePhanHoi.description ?? "";
                //data["new_solution"] = cases.singlePhanHoi.new_solution ?? "";

                if (cases.singlePhanHoi.caseorigincode != 0)
                {
                    data["caseorigincode"] = cases.singlePhanHoi.caseorigincode;
                }

                if (checkcreate == false)
                {
                    data["statuscode"] = cases.singlePhanHoi.statuscode;
                }

                if (cases.singlePhanHoi._customerid_value == null)
                {
                    await DeletLookup("customerid_contact", cases.singlePhanHoi.incidentid);
                }
                else
                {
                    data["customerid_contact@odata.bind"] = "/contacts(" + cases.singlePhanHoi._customerid_value + ")"; /////Lookup Field
                }

                if (cases.singlePhanHoi._subjectid_value == null)
                {
                    await DeletLookup("subjectid", cases.singlePhanHoi.incidentid);
                }
                else
                {
                    data["subjectid@odata.bind"] = "/subjects(" + cases.singlePhanHoi._subjectid_value + ")"; /////Lookup Field
                }

                if (cases.singlePhanHoi._productid_value == null)
                {
                    await DeletLookup("productid", cases.singlePhanHoi.incidentid);
                }
                else
                {
                    data["productid@odata.bind"] = "/products(" + cases.singlePhanHoi._productid_value + ")"; /////Lookup Field
                }

                if (cases.singlePhanHoi._primarycontactid_value == null)
                {
                    await DeletLookup("primarycontactid", cases.singlePhanHoi.incidentid);
                }
                else
                {
                    data["primarycontactid@odata.bind"] = "/contacts(" + cases.singlePhanHoi._primarycontactid_value + ")"; /////Lookup Field
                }

                return data;
            }

        }


    }
}
