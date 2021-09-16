using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhanHoiForm : ContentPage
    {
        public Action<bool> CheckPhanHoi;
        private Guid IncidentId;
        public PhanHoiFormViewModel viewModel;

        public ICRMService<Account> accountService;
        Label required_field = new Label()
        {
            HorizontalTextAlignment = TextAlignment.End,
            VerticalOptions = LayoutOptions.Center,
            TextColor = Color.FromHex("#fb0000"),
            FontSize = 18,
            Text = "*",
        };

        public PhanHoiForm()
        {
            InitializeComponent();
            IncidentId = Guid.Empty;
            Init();
        }
        public PhanHoiForm(Guid incidentid)
        {
            InitializeComponent();
            IncidentId = incidentid;
            CheckSinglePhanHoi();
        }

        private async void CheckSinglePhanHoi()
        {
            await Init();
            if (viewModel.singlePhanHoi != null)
                CheckPhanHoi(true);
            else
                CheckPhanHoi(false);
        }

        public async Task Init()
        {
            accountService = new CRMService<Account>();
            this.BindingContext = viewModel = new PhanHoiFormViewModel();

            if (IncidentId != Guid.Empty)
            {
                viewModel.Title = "Cập Nhật Phản Hồi";
                buttonUpdate.IsVisible=true;
                buttonCreate.IsVisible = false;
                await Start(IncidentId);
            }
            else
            {
                viewModel.Title = "Thêm Phản Hồi";
                buttonCreate.IsVisible = true;
                buttonUpdate.IsVisible = false;

                bsd_status_label.IsVisible = false;
                bsd_status_text.IsVisible = false;
                viewModel.singlePhanHoi = new PhanHoiFormModel();

                //viewModel.singleCustomergroup = viewModel.list_picker_bsd_customergroup.SingleOrDefault(x => x.Val == "");
                viewModel.singleOrigin = viewModel.list_picker_caseorigincode.SingleOrDefault(x => x.Val == "");

                await viewModel.LoadListSubject();
                await viewModel.LoadListAcc();
                await viewModel.LoadListContact();
                await viewModel.LoadListUnit();
                await viewModel.LoadListLienHe(viewModel.singlePhanHoi._customerid_value);

                viewModel.IsBusy = false;
            }
        }

        public async Task Start(Guid IncidentId)
        {
            viewModel.IsBusy = true;
            viewModel.Title = "Cập Nhật Phản Hồi";
            buttonUpdate.IsVisible = true;
            buttonCreate.IsVisible = false;

            bsd_status_label.IsVisible = true;
            bsd_status_text.IsVisible = true;

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
            origin_value.SelectedItem = null;
            btn_origin.IsVisible = false;
            viewModel.singlePhanHoi.caseorigincode = 0;
        }

        void Selectvalue_origin(object sender, System.EventArgs e)
        {
            btn_origin.IsVisible = true;

            viewModel.singlePhanHoi.caseorigincode = int.Parse(viewModel.singleOrigin == null ? "0" : viewModel.singleOrigin.Val);

        }

        void show_popup_subject(object sender, System.EventArgs e)
        {
            popup_list_viewSubject.IsVisible = true;
        }

        public async void ItemAppearingSubject(object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
        {
            viewModel.morelookup_subject = true;
            var itemAppearing = e.Item as Portable.Models.ListSubjectCase;
            var lastItem = viewModel.list_lookup_Subject.LastOrDefault();
            if (lastItem != null && itemAppearing.Id.Equals(lastItem.Id))
            {
                if (viewModel.morelookup_subject == true)
                {
                    viewModel.pageLookup_subject += 1;
                    await viewModel.LoadListSubject();
                }
            }
            viewModel.morelookup_subject = false;
        }

        void show_popup_customer(object sender, System.EventArgs e)
        {
            popup_list_view.IsVisible = true;
        }

        void show_popup_Unit(object sender, System.EventArgs e)
        {
            popup_list_viewUnit.IsVisible = true;
        }

        void show_popup_Contact(object sender, System.EventArgs e)
        {
            popup_list_viewContact.IsVisible = true;
        }

        void show_popup_Entilement(object sender, System.EventArgs e)
        {
            popup_list_viewEntilemnent.IsVisible = true;
        }

        void OnSelectItem_Subject(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {          
            viewModel.singlePhanHoi.subjecttitle = (e.Item as ListSubjectCase).Name;
            viewModel.singlePhanHoi._subjectid_value = (e.Item as ListSubjectCase).Id;
            //bsd_subject_text.text = (e.item as listsubjectcase).name;
            //bsd_subject_text.text = (e.item as customercase).name;
            popup_list_viewSubject.IsVisible = false;
        }

        void Clearvalue_subject(object sender, System.EventArgs e)
        {
            //bsd_subject_text.IsVisible = false;
            //bsd_subject_default.IsVisible = true;
            //btn_subject.IsVisible = false;

            //bsd_subject_text.Text = "";
            viewModel.singlePhanHoi.subjecttitle = null;
            viewModel.singlePhanHoi._subjectid_value = null;
        }

        void show_popup_multiselect(object sender, System.EventArgs e)
        {

            header_popup_listview.Children.Clear();

            Grid tab_grid = new Grid() { ColumnSpacing = 1, HorizontalOptions = LayoutOptions.FillAndExpand };
            tab_grid.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition() { Width = GridLength.Star},
                new ColumnDefinition() { Width = GridLength.Star},
            };

            StackLayout tab_Account = new StackLayout();
            tab_Account.Children.Add(new Label() { Text = "KH Doanh Nghiệp", FontAttributes = FontAttributes.Bold, FontSize = 15, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand });

            StackLayout tab_Contact = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
            tab_Contact.Children.Add(new Label() { Text = "KH Cá Nhân", FontAttributes = FontAttributes.Bold, FontSize = 15, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand });

            tab_grid.Children.Add(tab_Account, 0, 0);
            tab_grid.Children.Add(tab_Contact, 1, 0);

            header_popup_listview.Children.Add(tab_grid);
            this.show_tab_listview_popup_required(tab_Account);

            popup_list_view.IsVisible = true;
        }

        //POPUP REQUIRED
        public bool checkacc;
        private async void show_tab_listview_popup_required(StackLayout active)
        {

            var tablename = (active.Children[0] as Label).Text;
            if (tablename == "KH Doanh Nghiệp")
            {
                if (viewModel.list_account_lookup.Count == 0)
                {
                    //await viewModelAccount.loadAccountsLookup();
                }
                viewModel.list_lookup = viewModel.list_account_lookup;
                checkacc = true;
                viewModel.singlePhanHoi.logicalname = "accounts";
            }
            else if (tablename == "KH Cá Nhân")
            {
                if (viewModel.list_contact_lookup.Count == 0)
                {
                    //await viewModel.LoadContactsForLookup();
                }
                viewModel.list_lookup = viewModel.list_contact_lookup;
                checkacc = false;
                viewModel.singlePhanHoi.logicalname = "contacts";
            }

            active.BackgroundColor = Color.White;
            active.Margin = 0;
            active.GestureRecognizers.Clear();

            var inactive = (header_popup_listview.Children[0] as Grid).Children.ToList().Where(x => (x as StackLayout) != active).ToList();

            foreach (var s in inactive)
            {
                s.BackgroundColor = Color.LightSlateGray;
                s.GestureRecognizers.Clear();
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += Tab_Tapped;
                s.GestureRecognizers.Add(tapGestureRecognizer);
                (s as StackLayout).Margin = new Thickness(0, 5, 0, 0);
            }

            source_listviewpopup.Children.Clear();

            ListView tmp = new ListView();

            ///// Render List Topic to Popup
            tmp.SetBinding(ListView.ItemsSourceProperty, new Binding("list_lookup", source: viewModel));
            tmp.ItemTemplate = null;
            tmp.ItemTemplate = new DataTemplate(() => {
                // Create views with bindings for displaying each property.
                Label nameLabel = new Label();
                nameLabel.SetBinding(Label.TextProperty, "Name");
                // Return an assembled ViewCell.
                return new ViewCell
                {
                    View = new StackLayout
                    {
                        Children = { nameLabel }
                    }
                };
            });
            tmp.ItemTapped += popup_required_ItemTapped;
            tmp.ItemAppearing += ItemAppearingcutomer;

            source_listviewpopup.Children.Add(tmp);

            ////show Popup
            SearchBarListView.Text = "";
            popup_list_view.IsVisible = true;
            //viewModel.isBusy = false;
        }

        private void Tab_Tapped(object sender, EventArgs e)
        {
            var active = (sender as StackLayout);

            this.show_tab_listview_popup_required(active);
        }

        public async void ItemAppearingcutomer(object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
        {
            if (checkacc == true)
            {
                viewModel.morelookup_account = true;
                var itemAppearing = e.Item as Portable.Models.PhanHoiFormModel;
                var lastItem = viewModel.list_lookup.LastOrDefault();
                if (lastItem != null && itemAppearing.Id.Equals(lastItem.Id))
                {
                    if (viewModel.morelookup_account == true)
                    {
                        viewModel.pageLookup_account += 1;
                        await viewModel.LoadListAcc();
                    }
                }
                viewModel.morelookup_account = false;
            }
            else
            {
                viewModel.morelookup_contact = true;
                var itemAppearing = e.Item as Portable.Models.PhanHoiFormModel;
                var lastItem = viewModel.list_lookup.LastOrDefault();
                if (lastItem != null && itemAppearing.Id.Equals(lastItem.Id))
                {
                    if (viewModel.morelookup_contact == true)
                    {
                        viewModel.pageLookup_contact += 1;
                        await viewModel.LoadListContact();
                    }
                }
                viewModel.morelookup_contact = false;
            }
        }

        private async void popup_required_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.list_lookup_lienhe.Clear();
            viewModel.morelookup_lienhe = true;
            viewModel.pageLookup_lienhe = 1;
            viewModel.singlePhanHoi.contactname = null;
            viewModel.singlePhanHoi._primarycontactid_value = null;
            //bsd_contact_text.Text = null;

            //btn_customer.IsVisible = true;
            //bsd_customer_text.IsVisible = true;
            //bsd_customer_default.IsVisible = false;
            //bsd_customer_text.Text= (e.Item as PhanHoiFormModel).Name;
            viewModel.singlePhanHoi.customerid = (e.Item as PhanHoiFormModel).Name;
            viewModel.singlePhanHoi._customerid_value = (e.Item as PhanHoiFormModel).Id;
            await viewModel.LoadListLienHe((e.Item as PhanHoiFormModel).Id);

            viewModel.morelookup_lienhe = false;
            this.hide_listview_popup(null, null);
        }

        void hide_listview_popup(object sender, System.EventArgs e)
        {
            popup_list_view.IsVisible = false;
        }

        void SearchBarListView_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {

        }

        void Clearvalue_customer(object sender, System.EventArgs e)
        {
            //bsd_customer_text.IsVisible = false;
            //bsd_customer_default.IsVisible = true;
            //btn_customer.IsVisible = false;

            //EntryMultiSelect_regarding.Children.Clear();
            Label label = new Label { Text = "", TextColor = Color.Black };
            StackLayout stacklayout = new StackLayout { Orientation = StackOrientation.Horizontal, Spacing = 7, Padding = 3, BackgroundColor = Color.White };
            stacklayout.Children.Add(label);
            viewModel.singlePhanHoi.customerid = null;
            viewModel.singlePhanHoi._customerid_value = null;

            viewModel.singlePhanHoi.contactname = null;
            viewModel.singlePhanHoi._primarycontactid_value = null;
            //bsd_contact_text.Text = null;
            //EntryMultiSelect_regarding.Children.Add(stacklayout);

            //viewModel.list_lookup_required = new ObservableCollection<PhanHoiFormModel>();
        }

        //void OnSelectItem_Customer(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        //{
        //    bsd_customer_text.IsVisible = true;
        //    bsd_customer_default.IsVisible = false;
        //    btn_customer.IsVisible = true;

        //    //bsd_customer_text.Text = (e.Item as CustomerCase).Name;
        //    popup_list_view.IsVisible = false;
        //}

        //void Clearvalue_customer(object sender, System.EventArgs e)
        //{
        //    bsd_customer_text.IsVisible = false;
        //    bsd_customer_default.IsVisible = true;
        //    btn_customer.IsVisible = false;

        //    bsd_customer_text.Text = "";
        //}

        public async void ItemAppearingUnit(object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
        {
            viewModel.morelookup_unit = true;
            var itemAppearing = e.Item as Portable.Models.ListUnitCase;
            var lastItem = viewModel.list_unit_lookup.LastOrDefault();
            if (lastItem != null && itemAppearing.Id.Equals(lastItem.Id))
            {
                if (viewModel.morelookup_unit == true)
                {
                    viewModel.pageLookup_unit += 1;
                    await viewModel.LoadListUnit();
                }
            }
            viewModel.morelookup_unit = false;
        }

        void OnSelectItem_Unit(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            //bsd_unit_text.IsVisible = true;
            //bsd_unit_default.IsVisible = false;
            //btn_unit.IsVisible = true;

            viewModel.singlePhanHoi.productname = (e.Item as ListUnitCase).Name;
            viewModel.singlePhanHoi._productid_value = (e.Item as ListUnitCase).Id;
            //bsd_unit_text.Text = (e.Item as ListUnitCase).Name;

            //bsd_unit_text.Text = (e.Item as CustomerCase).Name;
            popup_list_viewUnit.IsVisible = false;
        }

        void Clearvalue_unit(object sender, System.EventArgs e)
        {
            //bsd_unit_text.IsVisible = false;
            //bsd_unit_default.IsVisible = true;
            //btn_unit.IsVisible = false;

            //bsd_unit_text.Text = "";
            viewModel.singlePhanHoi.productname = null;
            viewModel.singlePhanHoi._productid_value = null;
        }

        public async void ItemAppearingLienhe(object sender, Xamarin.Forms.ItemVisibilityEventArgs e)
        {
            viewModel.morelookup_lienhe = true;
            var itemAppearing = e.Item as Portable.Models.ListLienHeCase;
            var lastItem = viewModel.list_lookup_lienhe.LastOrDefault();
            if (lastItem != null && itemAppearing.Id.Equals(lastItem.Id))
            {
                if (viewModel.morelookup_lienhe == true)
                {
                    viewModel.pageLookup_lienhe += 1;
                    await viewModel.LoadListLienHe(viewModel.singlePhanHoi._customerid_value);
                }
            }
            viewModel.morelookup_lienhe = false;
        }

        void OnSelectItem_Contact(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            //bsd_contact_text.IsVisible = true;
            //bsd_contact_default.IsVisible = false;
            //btn_contact.IsVisible = true;

            viewModel.singlePhanHoi.contactname = (e.Item as ListLienHeCase).Name;
            viewModel.singlePhanHoi._primarycontactid_value = (e.Item as ListLienHeCase).Id;
            //bsd_contact_text.Text = (e.Item as ListLienHeCase).Name;

            //bsd_contact_text.Text = (e.Item as CustomerCase).Name;
            popup_list_viewContact.IsVisible = false;
        }

        void Clearvalue_contact(object sender, System.EventArgs e)
        {
            //bsd_contact_text.IsVisible = false;
            //bsd_contact_default.IsVisible = true;
            //btn_contact.IsVisible = false;

            //bsd_contact_text.Text = "";
            viewModel.singlePhanHoi.contactname = null;
            viewModel.singlePhanHoi._primarycontactid_value = null;
        }

        void OnSelectItem_Entilement(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            //bsd_entilement_text.IsVisible = true;
            //bsd_entilement_default.IsVisible = false;
            //btn_entilement.IsVisible = true;

            ////bsd_entilement_text.Text = (e.Item as CustomerCase).Name;
            //popup_list_viewEntilemnent.IsVisible = false;
        }

        void Clearvalue_entilement(object sender, System.EventArgs e)
        {
            //bsd_entilement_text.IsVisible = false;
            //bsd_entilement_default.IsVisible = true;
            //btn_entilement.IsVisible = false;

            //bsd_entilement_text.Text = "";
        }

        void SearchBar_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {

        }

        void Button_Clicked(object sender, System.EventArgs e)
        {
            popup_list_viewSubject.IsVisible = false;
            popup_list_view.IsVisible = false;
            popup_list_viewUnit.IsVisible = false;
            popup_list_viewContact.IsVisible = false;
            popup_list_viewEntilemnent.IsVisible = false;
            popup_list_viewStatus.IsVisible = false;
        }

        private async Task<String> checkData()
        {

            if (string.IsNullOrWhiteSpace(viewModel.singlePhanHoi.title)|| viewModel.singlePhanHoi.customerid == null 
             || viewModel.singlePhanHoi.customerid == "" )
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
            if (cases.singlePhanHoi.logicalname=="accounts")
            {
                IDictionary<string, object> data = new Dictionary<string, object>();
                data["incidentid"] = cases.singlePhanHoi.incidentid.ToString();
                data["title"] = cases.singlePhanHoi.title ?? "";
                data["description"] = cases.singlePhanHoi.description ?? "";
                //data["new_solution"] = cases.singlePhanHoi.new_solution ?? "";

                data["caseorigincode"] = cases.singlePhanHoi.caseorigincode != 0 ? cases.singlePhanHoi.caseorigincode.ToString() : null;

                if (checkcreate == false) {
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

        void Show_popup_status(object sender, System.EventArgs e)
        {
            popup_list_viewStatus.IsVisible = true;
        }

        public int? valueid { get; set; }
        public string valuename { get; set; }

        void OnSelectItem_Status(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            valueid = int.Parse((e.Item as PhanHoiFormModel).Id);
            valuename = (e.Item as PhanHoiFormModel).Name;
        }

        void Accept_Status(object sender, System.EventArgs e)
        {
            if (valueid.HasValue && !string.IsNullOrWhiteSpace(valuename))
            {
                viewModel.singlePhanHoi.statuscode = valueid.Value;
                bsd_status_text.Text = valuename;
            }
            
            popup_list_viewStatus.IsVisible = false;
        }


    }
}
