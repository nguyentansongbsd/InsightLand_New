using ConasiCRM.Portable.Controls;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Services;
using ConasiCRM.Portable.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Common.DataAnnotations;
using Telerik.XamarinForms.DataGrid;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using permissionType = Plugin.Permissions.Abstractions.Permission;
using permissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;
using System.Text.RegularExpressions;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactForm : ContentPage
    {
        public Action<bool> CheckSingleContact;
        ContactFormViewModel viewModel;
        LeadFormViewModel viewModel_lead;

        TapGestureRecognizer tab_tapped;

        Label required_field = new Label()
        {
            HorizontalTextAlignment = TextAlignment.End,
            VerticalOptions = LayoutOptions.Center,
            TextColor = Color.FromHex("#fb0000"),
            FontSize = 18,
            Text = "*",
        };

        private bool isShowingPopup;

        public ContactForm()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ContactFormViewModel();
            viewModel_lead = new LeadFormViewModel();
            this.constructor();
            this.loadData(null);
            lookUpContact.PreOpenAsync= async () => {
                await viewModel.loadAccountsLookup();
            };
        }

        public ContactForm(Guid contactId)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new ContactFormViewModel();
            this.constructor();
            Init(contactId);
        }

        public async void Init(Guid Id)
        {
            await loadData(Id.ToString());
            if (viewModel.singleContact != null)
                CheckSingleContact(true);
            else
                CheckSingleContact(false);
        }

        public void constructor()
        {
            viewModel.singleContact = new ContactFormModel();
            tab_tapped = new TapGestureRecognizer();
            tab_tapped.Tapped += Tab_Tapped;
            isShowingPopup = false;
        }

        public async Task loadData(string contactId)
        {
            viewModel.IsBusy = true;

            if (contactId != null)
            {
                await viewModel.loadOneContact(contactId);
                await viewModel.Load_DanhSachDuAn(contactId);
                if (viewModel.singleContact.gendercode != null) { await viewModel.loadOneGender(viewModel.singleContact.gendercode); }
                if (viewModel.singleContact.bsd_localization != null) { await viewModel.loadOneLocalization(viewModel.singleContact.bsd_localization); }
                if (viewModel.singleContact.bsd_customergroup != null) { await viewModel.loadOneContactGroup(viewModel.singleContact.bsd_customergroup); }
                await viewModel.Load_NhuCauVeDiaDiem(contactId);
                await viewModel.LoadQueuesForContactForm(contactId);
                await viewModel.LoadReservationForContactForm(contactId);
                await viewModel.LoadOptoinEntryForContactForm(contactId);
                await viewModel.LoadCaseForContactForm(contactId);

                //if (viewModel.list_nhucauvediadiem.Count < 3)
                //{
                //    for (int i = viewModel.list_nhucauvediadiem.Count; i < 3; i++)
                //    {
                //        viewModel.list_nhucauvediadiem.Add(new Provinces());
                //    }
                //}
                //if (viewModel.list_danhsachdatcho.Count < 3)
                //{
                //    for (int i = viewModel.list_danhsachdatcho.Count; i < 3; i++)
                //    {
                //        viewModel.list_danhsachdatcho.Add(new QueueListModel());
                //    }
                //}
                //if (viewModel.list_danhsachdatcoc.Count < 3)
                //{
                //    for (int i = viewModel.list_danhsachdatcoc.Count; i < 3; i++)
                //    {
                //        viewModel.list_danhsachdatcoc.Add(new QuotationReseravtion());
                //    }
                //}
                //if (viewModel.list_danhsachhopdong.Count < 3)
                //{
                //    for (int i = viewModel.list_danhsachhopdong.Count; i < 3; i++)
                //    {
                //        viewModel.list_danhsachhopdong.Add(new OptionEntry());
                //    }
                //}
                //if (viewModel.list_chamsockhachhang.Count < 3)
                //{
                //    for (int i = viewModel.list_chamsockhachhang.Count; i < 3; i++)
                //    {
                //        viewModel.list_chamsockhachhang.Add(new Case());
                //    }
                //}
                //if (viewModel.list_Duanquantam.Count < 3)
                //{
                //    for (int i = viewModel.list_Duanquantam.Count; i < 3; i++)
                //    {
                //        viewModel.list_Duanquantam.Add(new ProjectList());
                //    }
                //}

                if (!string.IsNullOrEmpty(viewModel.singleContact.bsd_type) && !string.IsNullOrWhiteSpace(viewModel.singleContact.bsd_type))
                {
                    var text_type = viewModel.singleContact.bsd_type.Split(',');
                    foreach (var x in text_type)
                    {
                        multipleSelectView.addSelectedItem(x);
                    }
                }
                await viewModel.GetImageCMND();
            }
            this.render(contactId);
            viewModel.IsBusy = false;
        }

        public void render(string id)
        {
            if (id != null)
            {
                this.Title = "Cập Nhật Khách Hàng Cá Nhân";
                btn_save_contact.Text = "Cập Nhật";
                btn_save_contact.Clicked += UpdateContact_Clicked;
                selectionPhongThuy.IsVisible = true;
                label_du_an_quan_tam.IsVisible = true;

                //datagrid_danhsachdatcho.SetBinding(RadDataGrid.ItemsSourceProperty, new Binding("list_danhsachdatcho", source: viewModel));
                //datagrid_danhsachdatcoc.SetBinding(RadDataGrid.ItemsSourceProperty, new Binding("list_danhsachdatcoc", source: viewModel));
                //datagrid_danhsachhopdong.SetBinding(RadDataGrid.ItemsSourceProperty, new Binding("list_danhsachhopdong", source: viewModel));
                //datagrid_chamsockhachhang.SetBinding(RadDataGrid.ItemsSourceProperty, new Binding("list_chamsockhachhang", source: viewModel));

                //birthdate_picker.SetBinding(MyNewDatePicker.NullableDateProperty, new Binding("singleContact.birthdate", source: viewModel));
                //bsd_dategrant_picker.SetBinding(MyNewDatePicker.NullableDateProperty, new Binding("singleContact.bsd_dategrant", source: viewModel));
                //bsd_issuedonpassport_picker.SetBinding(MyNewDatePicker.NullableDateProperty, new Binding("singleContact.bsd_issuedonpassport", source: viewModel));

                label_nhu_cau_ve_dia_diem.IsVisible = true;
                stacklayout_nhucauvediadiem.IsVisible = true;
                layout_danhsachdatcho.IsVisible = true;
                layout_danhsachdatcoc.IsVisible = true;
                layout_danhsachhopdong.IsVisible = true;
                layout_chamsockhachhang.IsVisible = true;
            }
            else
            {
                this.Title = "Tạo Mới Khách Hàng Cá Nhân";
                btn_save_contact.Text = "Tạo Mới";
                selectionPhongThuy.IsVisible = false;
                label_du_an_quan_tam.IsVisible = false;
                btn_save_contact.Clicked += AddContact_Clicked;

                label_nhu_cau_ve_dia_diem.IsVisible = false;
                stacklayout_nhucauvediadiem.IsVisible = false;
                layout_danhsachdatcho.IsVisible = false;
                layout_danhsachdatcoc.IsVisible = false;
                layout_danhsachhopdong.IsVisible = false;
                layout_chamsockhachhang.IsVisible = false;
            }

        }

        private void reload(Guid created)
        {
            viewModel.reset();
            btn_save_contact.Clicked -= AddContact_Clicked;
            btn_save_contact.Clicked -= UpdateContact_Clicked;

            this.constructor();
            this.loadData(created.ToString());
        }

        /// This event for update Heightreqest of Image CMND
        void Handle_LayoutChanged(object sender, System.EventArgs e)
        {
            var width = ((DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density) - 35) / 2;
            var tmpHeight = width * 2 / 3;
            this.matTruocCMND_field.HeightRequest = tmpHeight;
            this.matSauCMND_field.HeightRequest = tmpHeight;
        }

        /////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////
        ///
        /// POPUP FUllNAME
        private void show_popup_fullname(object sender, EventArgs e)
        {
            popupfullname_firstname.Text = viewModel.singleContact.bsd_firstname;
            popupfullname_lastname.Text = viewModel.singleContact.bsd_lastname;
            popup_fullname.IsVisible = true;

            isShowingPopup = true;
        }

        private void hide_popup_fullname(object sender, EventArgs e)
        {
            popup_fullname.IsVisible = false;
            isShowingPopup = false;
        }

        private async void save_fullname(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(popupfullname_lastname.Text))
            {
                if (!string.IsNullOrWhiteSpace(popupfullname_firstname.Text))
                {
                    viewModel.singleContact.bsd_firstname = "";
                    viewModel.singleContact.firstname = "";
                }
                else
                {
                    viewModel.singleContact.bsd_firstname = popupfullname_firstname.Text.Trim();
                    viewModel.singleContact.firstname = popupfullname_firstname.Text.Trim();
                }
                viewModel.singleContact.bsd_lastname = popupfullname_lastname.Text.Trim();
                viewModel.singleContact.lastname = popupfullname_lastname.Text.Trim();

                var tmp = new List<String>();
            if (viewModel.singleContact.bsd_firstname != null) { tmp.Add(viewModel.singleContact.bsd_firstname); }
            if (viewModel.singleContact.bsd_lastname != null) { tmp.Add(viewModel.singleContact.bsd_lastname); }


                viewModel.singleContact.bsd_fullname = string.Join(" ", tmp);
                //viewModel.singleContact.bsd_fullname = viewModel.singleContact.fullname;
                this.hide_popup_fullname(null, null);
            }
            else
            {
                await DisplayAlert("", "Vui lòng nhập các trường bắt buộc (trường có gắn dấu *)", "OK");
            }
        }

        /// GENDER PICKER
        private void gendercode_picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (viewModel.singleGender.Val != null)
            {
                viewModel.singleContact.gendercode = viewModel.singleGender.Val;
                viewModel.PhongThuy.gioi_tinh = Int32.Parse(viewModel.singleContact.gendercode);
                viewModel.PhongThuy.nam_sinh = viewModel.singleContact.birthdate.HasValue ? viewModel.singleContact.birthdate.Value.Year : 0;
            }
            else
            {
                viewModel.singleContact.gendercode = null;
                viewModel.PhongThuy.gioi_tinh = 0;
                viewModel.PhongThuy.nam_sinh = 0;
            }
        }

        /// CONTACTGROUP PICKER

        private void contact_group_picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            viewModel.singleContact.bsd_customergroup = viewModel.singleContactgroup == null ? null : viewModel.singleContactgroup.Val;
        }

        /// LOCALIZATION PICKER
        private void bsd_localization_picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            viewModel.singleContact.bsd_localization = viewModel.singleLocalization == null ? null : viewModel.singleLocalization.Val;
        }

        //////////////////////////////////////////////////////////// <summary>
        ///
        ///        LISTVIEW POPUP (All POPUP USE LISTVIEW AND SEARCHBAR VIEW)
        ///
        //////////////////////////////////////////////////////////// </summary>

        private void hide_listview_popup(object sender, EventArgs e)
        {
            popup_list_view.IsVisible = false;
            isShowingPopup = false;
        }

        private void SearchBarListView_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (source_listviewpopup.Children.FirstOrDefault() != null)
            {
                if (e.NewTextValue == null)
                {
                    (source_listviewpopup.Children.FirstOrDefault() as ListView).ItemsSource = viewModel.list_lookup;
                    return;
                }
               (source_listviewpopup.Children.FirstOrDefault() as ListView).ItemsSource = viewModel.list_lookup.Where(x => x.Name.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        /// POPUPLISTVIEW PROTECTOR
        private async void show_popup_listview_protector(object sender, EventArgs e)
        {
            isShowingPopup = true;

            header_popup_listview.Children.Clear();
            ////<Label x:Name="title_popuplistview" Font="Bold,18" HorizontalTextAlignment="Center"/>
            //title_popup_list_view.Text = "Người bảo hộ";
            StackLayout layout_title_listview_popup = new StackLayout() { BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.FillAndExpand };
            Label title_popuplistview = new Label() { Text = "Người bảo hộ", FontAttributes = FontAttributes.Bold, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand };
            StackLayout underline = new StackLayout() { BackgroundColor = Color.Black, HeightRequest = .9, Padding = new Thickness(10, 0) };
            layout_title_listview_popup.Children.Add(title_popuplistview);
            layout_title_listview_popup.Children.Add(underline);
            header_popup_listview.Children.Add(layout_title_listview_popup);

            viewModel.IsBusy = true;
            source_listviewpopup.Children.Clear();

            if (viewModel.list_contact_lookup.Count == 0)
            {
                await viewModel.LoadContactsForLookup();
            }
            viewModel.list_lookup = viewModel.list_contact_lookup;


            ListView tmp = new ListView();

            ///// Render List Topic to Popup
            tmp.SetBinding(ListView.ItemsSourceProperty, new Binding("list_lookup", source: viewModel));
            tmp.ItemTemplate = null;
            tmp.ItemTemplate = new DataTemplate(() =>
            {
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
            tmp.ItemTapped += popupProtector_ItemTapped;
            tmp.ItemAppearing += loadMoreContactLookup;

            source_listviewpopup.Children.Add(tmp);

            #region show Popup
            SearchBarListView.Text = "";
            popup_list_view.IsVisible = true;

            viewModel.IsBusy = false;
            #endregion
        }

        private void popupProtector_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selected = e.Item as Models.LookUp;

            viewModel.singleContact._bsd_protecter_value = selected.Id.ToString();
            viewModel.singleContact.bsd_protecter_label = selected.Name;

            this.hide_listview_popup(null, null);
        }

        /// POPUPLISTVIEW PARENTCUSTOMERID ------ POPUP HAVE 2 TABS -------
        private void show_popup_listview_parentcustomerid(object sender, EventArgs e)
        {
            isShowingPopup = true;

            header_popup_listview.Children.Clear();

            Grid tab_grid = new Grid() { ColumnSpacing = 1, HorizontalOptions = LayoutOptions.FillAndExpand };
            tab_grid.ColumnDefinitions = new ColumnDefinitionCollection
            {
            new ColumnDefinition() { Width = GridLength.Star},
            new ColumnDefinition() { Width = GridLength.Star},
            };

            StackLayout tab_Account = new StackLayout();
            tab_Account.Children.Add(new Label() { Text = "KH Doanh Nghiệp", FontAttributes = FontAttributes.Bold, FontSize = 15, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand });

            StackLayout tab_Contact = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.LightSlateGray,
                Margin = new Thickness(0, 5, 0, 0),
            };
            tab_Contact.GestureRecognizers.Add(tab_tapped);
            tab_Contact.Children.Add(new Label() { Text = "KH Cá Nhân", FontAttributes = FontAttributes.Bold, FontSize = 15, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand });

            tab_grid.Children.Add(tab_Account, 0, 0);
            tab_grid.Children.Add(tab_Contact, 1, 0);

            header_popup_listview.Children.Add(tab_grid);

            this.show_tab_listview_popup_parentcustomerid(tab_Account);
        }

        private async void show_tab_listview_popup_parentcustomerid(StackLayout active)
        {
            //popup_list_view.IsVisible = false;

            viewModel.IsBusy = true;

            var inactive = (header_popup_listview.Children.Last() as Grid).Children.FirstOrDefault(x => ((x as StackLayout) != active) && (x as StackLayout).BackgroundColor == Color.White);
            if (inactive != null)
            {
                inactive.BackgroundColor = Color.LightSlateGray;
                inactive.GestureRecognizers.Add(tab_tapped);
                inactive.Margin = new Thickness(0, 5, 0, 0);
            }

            active.Margin = 0;
            active.GestureRecognizers.Clear();

            var tablename = (active.Children.Last() as Label).Text;
            if (tablename == "KH Doanh Nghiệp")
            {
                if (viewModel.list_account_lookup.Count == 0)
                {
                    await viewModel.loadAccountsLookup();
                }
                viewModel.list_lookup = viewModel.list_account_lookup;
            }
            else
            {
                if (viewModel.list_contact_lookup.Count == 0)
                {
                    await viewModel.LoadContactsForLookup();
                }
                viewModel.list_lookup = viewModel.list_contact_lookup;
            }

            active.BackgroundColor = Color.White;
            source_listviewpopup.Children.Clear();

            ListView tmp = new ListView();

            ///// Render List Topic to Popup
            tmp.SetBinding(ListView.ItemsSourceProperty, new Binding("list_lookup", source: viewModel));
            tmp.ItemTemplate = null;
            tmp.ItemTemplate = new DataTemplate(() =>
            {
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
            if (tablename == "KH Doanh Nghiệp")
            {
                tmp.ItemAppearing += loadMoreAccountLookup;
            }
            else
            {
                tmp.ItemAppearing += loadMoreContactLookup;
            }
            tmp.ItemTapped += popup_parentcustomerid_ItemTapped;

            source_listviewpopup.Children.Add(tmp);

            ////show Popup
            SearchBarListView.Text = "";
            popup_list_view.IsVisible = true;
            viewModel.IsBusy = false;
        }

        private void popup_parentcustomerid_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selected = e.Item as Models.LookUp;
            viewModel.singleContact._parentcustomerid_value = selected.Id.ToString();
            viewModel.singleContact.parentcustomerid_label = selected.Name;

            if (selected.Detail == "Contact")
            {
                viewModel.singleContact.parentcustomerid_label_contact = selected.Name;
            }
            else if (selected.Detail == "Account")
            {
                viewModel.singleContact.parentcustomerid_label_account = selected.Name;
            }

            this.hide_listview_popup(null, null);
        }

        private void Tab_Tapped(object sender, EventArgs e)
        {
            var active = (sender as StackLayout);

            this.show_tab_listview_popup_parentcustomerid(active);
        }

        private async void loadMoreContactLookup(object sender, ItemVisibilityEventArgs e)
        {
            //var bottom = e.Item as Option;
            ListView listview = source_listviewpopup.Children.LastOrDefault() as ListView;
            var bottom = listview != null ? listview.ItemsSource.Cast<Models.LookUp>().Last() : null;

            if (viewModel.moreLookup_contact && (listview.ItemsSource.Cast<Models.LookUp>().Count() == 0 || (e.Item as Models.LookUp) == bottom))
            {
                viewModel.looking_up = true;
                viewModel.pageLookup_contact++;
                await viewModel.LoadContactsForLookup();
                viewModel.list_lookup = viewModel.list_contact_lookup;
                listview.ItemsSource = viewModel.list_lookup.Where(x => x.Name.IndexOf(SearchBarListView.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                viewModel.looking_up = false;
            }
        }

        private async void loadMoreAccountLookup(object sender, ItemVisibilityEventArgs e)
        {
            //var bottom = e.Item as Option;
            ListView listview = source_listviewpopup.Children.LastOrDefault() as ListView;
            var bottom = listview != null ? listview.ItemsSource.Cast<Models.LookUp>().Last() : null;

            if (viewModel.moreLooup_account && (listview.ItemsSource.Cast<Models.LookUp>().Count() == 0 || (e.Item as Models.LookUp) == bottom))
            {
                viewModel.looking_up = true;
                viewModel.pageLookup_account++;
                await viewModel.loadAccountsLookup();
                viewModel.list_lookup = viewModel.list_account_lookup;
                listview.ItemsSource = viewModel.list_lookup.Where(x => x.Name.IndexOf(SearchBarListView.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                viewModel.looking_up = false;
            }
        }

        //// POPUPLISTVIEW COUNTRY

        private async void show_popup_listview_country(object sender, EventArgs e)
        {
            isShowingPopup = true;

            header_popup_listview.Children.Clear();
            StackLayout layout_title_listview_popup = new StackLayout() { BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.FillAndExpand };
            Label title_popuplistview = new Label() { Text = "Quốc gia", FontAttributes = FontAttributes.Bold, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand };
            StackLayout underline = new StackLayout() { BackgroundColor = Color.Black, HeightRequest = .9, Padding = new Thickness(10, 0) };
            layout_title_listview_popup.Children.Add(title_popuplistview);
            layout_title_listview_popup.Children.Add(underline);
            header_popup_listview.Children.Add(layout_title_listview_popup);

            viewModel.IsBusy = true;
            source_listviewpopup.Children.Clear();

            if (viewModel.list_country_lookup.Count == 0)
            {
                await viewModel.LoadCountryForLookup();
            }
            viewModel.list_lookup = viewModel.list_country_lookup;


            ListView tmp = new ListView();

            ///// Render List Topic to Popup
            tmp.SetBinding(ListView.ItemsSourceProperty, new Binding("list_lookup", source: viewModel));
            tmp.ItemTemplate = null;
            tmp.ItemTemplate = new DataTemplate(() =>
            {
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
            tmp.ItemTapped += popupCountry_ItemTapped;
            tmp.ItemAppearing += loadMoreCountryLookup;

            source_listviewpopup.Children.Add(tmp);

            ////show Popup
            SearchBarListView.Text = "";
            popup_list_view.IsVisible = true;

            viewModel.IsBusy = false;
        }

        private void popupCountry_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selected = e.Item as Models.LookUp;

            viewModel.resetProvince();
            this.clear_entry_province(null, null);
            viewModel.resetDistrict();
            this.clear_entry_district(null, null);

            popupcontactaddress_country.Text = selected.Name;
            popupcontactaddress_country_id.Text = selected.Id.ToString();
            popupcontactaddress_country_en.Text = selected.Detail;
            popupcontactaddress_country.HasClearButton = true;

            this.hide_listview_popup(null, null);
        }

        private async void loadMoreCountryLookup(object sender, ItemVisibilityEventArgs e)
        {
            ListView listview = source_listviewpopup.Children.LastOrDefault() as ListView;
            var bottom = listview != null ? listview.ItemsSource.Cast<Models.LookUp>().Last() : null;

            if (viewModel.morelookup_country && (listview.ItemsSource.Cast<Models.LookUp>().Count() == 0 || (e.Item as Models.LookUp) == bottom))
            {
                viewModel.looking_up = true;
                viewModel.pageLookup_country++;
                await viewModel.LoadCountryForLookup();
                viewModel.list_lookup = viewModel.list_country_lookup;
                listview.ItemsSource = viewModel.list_lookup.Where(x => x.Name.IndexOf(SearchBarListView.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                viewModel.looking_up = false;

            }
        }

        //// POPUPLISTVIEW PROVINCE

        private async void show_popup_listview_province(object sender, EventArgs e)
        {
            isShowingPopup = true;

            if (string.IsNullOrEmpty(popupcontactaddress_country.Text))
            {
                await DisplayAlert("", "Vui lòng chọn quốc gia", "OK");
                return;
            }
            header_popup_listview.Children.Clear();
            StackLayout layout_title_listview_popup = new StackLayout() { BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.FillAndExpand };
            Label title_popuplistview = new Label() { Text = "Tỉnh/Thành", FontAttributes = FontAttributes.Bold, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand };
            StackLayout underline = new StackLayout() { BackgroundColor = Color.Black, HeightRequest = .9, Padding = new Thickness(10, 0) };
            layout_title_listview_popup.Children.Add(title_popuplistview);
            layout_title_listview_popup.Children.Add(underline);
            header_popup_listview.Children.Add(layout_title_listview_popup);

            viewModel.IsBusy = true;
            source_listviewpopup.Children.Clear();

            if (viewModel.list_province_lookup.Count == 0)
            {
                await viewModel.loadProvincesForLookup(popupcontactaddress_country_id.Text);
            }
            viewModel.list_lookup = viewModel.list_province_lookup;


            ListView tmp = new ListView();

            ///// Render List Topic to Popup
            tmp.SetBinding(ListView.ItemsSourceProperty, new Binding("list_lookup", source: viewModel));
            tmp.ItemTemplate = null;
            tmp.ItemTemplate = new DataTemplate(() =>
            {
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
            tmp.ItemTapped += popupProvince_ItemTapped;
            tmp.ItemAppearing += loadMoreProvinceLookup;

            source_listviewpopup.Children.Add(tmp);

            ////show Popup
            SearchBarListView.Text = "";
            popup_list_view.IsVisible = true;

            viewModel.IsBusy = false;
        }

        private void popupProvince_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selected = e.Item as Models.LookUp;

            viewModel.resetDistrict();
            this.clear_entry_district(null, null);

            popupcontactaddress_province.Text = selected.Name;
            popupcontactaddress_province_id.Text = selected.Id.ToString();
            popupcontactaddress_province_en.Text = selected.Detail;
            popupcontactaddress_province.HasClearButton = true;

            this.hide_listview_popup(null, null);
        }

        private async void loadMoreProvinceLookup(object sender, ItemVisibilityEventArgs e)
        {
            ListView listview = source_listviewpopup.Children.LastOrDefault() as ListView;
            var bottom = listview != null ? listview.ItemsSource.Cast<Models.LookUp>().Last() : null;

            if (viewModel.morelookup_province && (listview.ItemsSource.Cast<Models.LookUp>().Count() == 0 || (e.Item as Models.LookUp) == bottom))
            {
                viewModel.looking_up = true;
                viewModel.pageLookup_province++;
                await viewModel.loadProvincesForLookup(popupcontactaddress_country_id.Text);
                viewModel.list_lookup = viewModel.list_province_lookup;
                listview.ItemsSource = viewModel.list_lookup.Where(x => x.Name.IndexOf(SearchBarListView.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                viewModel.looking_up = false;

            }
        }

        //// POPUPLISTVIEW DISTRIC

        private async void show_popup_listview_district(object sender, EventArgs e)
        {
            isShowingPopup = true;

            if (string.IsNullOrEmpty(popupcontactaddress_province.Text))
            {
                await DisplayAlert("", "Vui lòng chọn tỉnh/thành", "OK");
                return;
            }
            header_popup_listview.Children.Clear();
            StackLayout layout_title_listview_popup = new StackLayout() { BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.FillAndExpand };
            Label title_popuplistview = new Label() { Text = "Quận/Huyện", FontAttributes = FontAttributes.Bold, FontSize = 18, HorizontalTextAlignment = TextAlignment.Center, HorizontalOptions = LayoutOptions.FillAndExpand };
            StackLayout underline = new StackLayout() { BackgroundColor = Color.Black, HeightRequest = .9, Padding = new Thickness(10, 0) };
            layout_title_listview_popup.Children.Add(title_popuplistview);
            layout_title_listview_popup.Children.Add(underline);
            header_popup_listview.Children.Add(layout_title_listview_popup);

            viewModel.IsBusy = true;
            source_listviewpopup.Children.Clear();

            if (viewModel.list_district_lookup.Count == 0)
            {
                await viewModel.loadDistrictForLookup(popupcontactaddress_province_id.Text);
            }
            viewModel.list_lookup = viewModel.list_district_lookup;


            ListView tmp = new ListView();

            ///// Render List Topic to Popup
            tmp.SetBinding(ListView.ItemsSourceProperty, new Binding("list_lookup", source: viewModel));
            tmp.ItemTemplate = null;
            tmp.ItemTemplate = new DataTemplate(() =>
            {
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
            tmp.ItemTapped += popupDistrict_ItemTapped;
            tmp.ItemAppearing += loadMoreDistrictLookup;

            source_listviewpopup.Children.Add(tmp);

            ////show Popup
            SearchBarListView.Text = "";
            popup_list_view.IsVisible = true;

            viewModel.IsBusy = false;
        }

        private void popupDistrict_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selected = e.Item as Models.LookUp;

            popupcontactaddress_district.Text = selected.Name;
            popupcontactaddress_district_id.Text = selected.Id.ToString();
            popupcontactaddress_district_en.Text = selected.Detail;
            popupcontactaddress_district.HasClearButton = true;

            this.hide_listview_popup(null, null);
        }

        private async void loadMoreDistrictLookup(object sender, ItemVisibilityEventArgs e)
        {
            ListView listview = source_listviewpopup.Children.LastOrDefault() as ListView;
            var bottom = listview != null ? listview.ItemsSource.Cast<Models.LookUp>().Last() : null;

            if (viewModel.morelookup_district && (listview.ItemsSource.Cast<Models.LookUp>().Count() == 0 || (e.Item as Models.LookUp) == bottom))
            {
                viewModel.looking_up = true;
                viewModel.pageLookup_district++;
                await viewModel.loadDistrictForLookup(popupcontactaddress_province_id.Text);
                viewModel.list_lookup = viewModel.list_district_lookup;
                listview.ItemsSource = viewModel.list_lookup.Where(x => x.Name.IndexOf(SearchBarListView.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                viewModel.looking_up = false;
            }
        }

        //////////////////////////////////////////////////////////// <summary>
        //////////////////////////////////////////////////////////// </summary>


        /// POPUP CONTACADRESS
        void show_popup_contactaddress(object sender, System.EventArgs e)
        {
            isShowingPopup = true;

            requiredform.Children.Add(required_field);

            viewModel.resetProvince();
            viewModel.resetDistrict();

            popupcontact_footer.Children.Remove(popupcontact_footer.Children.Last());
            var button = new Button()
            {
                Text = "Xác nhận",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Silver,
            };
            button.Clicked += save_popup_contacaddress;
            popupcontact_footer.Children.Add(button, 1, 0);

            popupcontactaddress_housenumberstreet.Text = viewModel.singleContact.bsd_housenumberstreet;
            popupcontactaddress_housenumber.Text = viewModel.singleContact.bsd_housenumber;

            popupcontactaddress_country.Text = viewModel.singleContact.bsd_country_label;
            popupcontactaddress_country.HasClearButton = string.IsNullOrEmpty(viewModel.singleContact.bsd_country_label) ? false : true;
            popupcontactaddress_country_id.Text = viewModel.singleContact._bsd_country_value;
            popupcontactaddress_country_en.Text = viewModel.singleContact.bsd_country_en;

            popupcontactaddress_province.Text = viewModel.singleContact.bsd_province_label;
            popupcontactaddress_province.HasClearButton = string.IsNullOrEmpty(viewModel.singleContact.bsd_province_label) ? false : true;
            popupcontactaddress_province_id.Text = viewModel.singleContact._bsd_province_value;
            popupcontactaddress_province_en.Text = viewModel.singleContact.bsd_province_en;

            popupcontactaddress_district.Text = viewModel.singleContact.bsd_district_label;
            popupcontactaddress_district.HasClearButton = string.IsNullOrEmpty(viewModel.singleContact.bsd_district_label) ? false : true;
            popupcontactaddress_district_id.Text = viewModel.singleContact._bsd_district_value;
            popupcontactaddress_district_en.Text = viewModel.singleContact.bsd_district_en;


            popup_contact_address.IsVisible = true;
        }

        void hide_popup_contactaddress(object sender, System.EventArgs e)
        {
            requiredform.Children.Remove(required_field);
            popup_contact_address.IsVisible = false;
        }

        async void save_popup_contacaddress(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(popupcontactaddress_housenumberstreet.Text))
            {
                if (string.IsNullOrWhiteSpace(popupcontactaddress_housenumber.Text))
                {
                    viewModel.singleContact.bsd_housenumber = "";
                }
                else
                {
                    viewModel.singleContact.bsd_housenumber = popupcontactaddress_housenumber.Text.Trim();
                }                  
                viewModel.singleContact.bsd_housenumberstreet = popupcontactaddress_housenumberstreet.Text.Trim();

                viewModel.singleContact.bsd_country_label = popupcontactaddress_country.Text;
                viewModel.singleContact._bsd_country_value = popupcontactaddress_country_id.Text;
                viewModel.singleContact.bsd_country_en = popupcontactaddress_country_en.Text;

                viewModel.singleContact.bsd_province_label = popupcontactaddress_province.Text;
                viewModel.singleContact._bsd_province_value = popupcontactaddress_province_id.Text;
                viewModel.singleContact.bsd_province_en = popupcontactaddress_province_en.Text;

                viewModel.singleContact.bsd_district_label = popupcontactaddress_district.Text;
                viewModel.singleContact._bsd_district_value = popupcontactaddress_district_id.Text;
                viewModel.singleContact.bsd_district_en = popupcontactaddress_district_en.Text;

                var tmp = new List<string>();
                if (!string.IsNullOrEmpty(viewModel.singleContact.bsd_housenumberstreet)) { tmp.Add(viewModel.singleContact.bsd_housenumberstreet); }
                if (!string.IsNullOrEmpty(viewModel.singleContact.bsd_district_label)) { tmp.Add(viewModel.singleContact.bsd_district_label); }
                if (!string.IsNullOrEmpty(viewModel.singleContact.bsd_province_label)) { tmp.Add(viewModel.singleContact.bsd_province_label); }
                if (!string.IsNullOrEmpty(viewModel.singleContact.bsd_country_label)) { tmp.Add(viewModel.singleContact.bsd_country_label); }

                viewModel.singleContact.bsd_contactaddress = string.Join(", ", tmp);

                var tmpEN = new List<string>();

                if (!string.IsNullOrEmpty(viewModel.singleContact.bsd_housenumber)) { tmpEN.Add(viewModel.singleContact.bsd_housenumber); }
                if (!string.IsNullOrEmpty(viewModel.singleContact.bsd_district_en)) { tmpEN.Add(viewModel.singleContact.bsd_district_en); }
                if (!string.IsNullOrEmpty(viewModel.singleContact.bsd_province_en)) { tmpEN.Add(viewModel.singleContact.bsd_province_en); }
                if (!string.IsNullOrEmpty(viewModel.singleContact.bsd_country_en)) { tmpEN.Add(viewModel.singleContact.bsd_country_en); }

                viewModel.singleContact.bsd_diachi = string.Join(", ", tmpEN);

                this.hide_popup_contactaddress(null, null);
            }
            else
            {
                await DisplayAlert("", "Vui lòng nhập các trường bắt buộc (trường có gắn dấu *)", "OK");
            }
        }

        /// POPUP PERMANENT CONTACTADDRESS
        void show_popup_permanent_contactaddress(object sender, System.EventArgs e)
        {
            isShowingPopup = true;

            viewModel.resetProvince();
            viewModel.resetDistrict();

            popupcontact_footer.Children.Remove(popupcontact_footer.Children.Last());
            var button = new Button()
            {
                Text = "Xác nhận",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Silver,
            };
            button.Clicked += save_popup_permanent_contacaddress;
            popupcontact_footer.Children.Add(button, 1, 0);

            popupcontactaddress_housenumberstreet.Text = viewModel.singleContact.bsd_permanentaddress;
            popupcontactaddress_housenumber.Text = viewModel.singleContact.bsd_permanenthousenumber;

            popupcontactaddress_country.Text = viewModel.singleContact.bsd_permanentcountry_label;
            popupcontactaddress_country.HasClearButton = viewModel.singleContact.bsd_permanentcountry_label == null ? false : true;
            popupcontactaddress_country_id.Text = viewModel.singleContact._bsd_permanentcountry_value;

            popupcontactaddress_province.Text = viewModel.singleContact.bsd_permanentprovince_label;
            popupcontactaddress_province.HasClearButton = viewModel.singleContact.bsd_permanentprovince_label == null ? false : true;
            popupcontactaddress_province_id.Text = viewModel.singleContact._bsd_permanentprovince_value;

            popupcontactaddress_district.Text = viewModel.singleContact.bsd_permanentdistrict_label;
            popupcontactaddress_district.HasClearButton = viewModel.singleContact.bsd_permanentdistrict_label == null ? false : true;
            popupcontactaddress_district_id.Text = viewModel.singleContact._bsd_permanentdistrict_value;


            popup_contact_address.IsVisible = true;
        }

        void hide_popup_permanent_contactaddress(object sender, System.EventArgs e)
        {
            popup_contact_address.IsVisible = false;
            isShowingPopup = false;
        }

        void save_popup_permanent_contacaddress(object sender, System.EventArgs e)
        {
            viewModel.singleContact.bsd_permanenthousenumber = popupcontactaddress_housenumber.Text;
            viewModel.singleContact.bsd_permanentaddress = popupcontactaddress_housenumberstreet.Text;

            viewModel.singleContact.bsd_permanentcountry_label = popupcontactaddress_country.Text;
            viewModel.singleContact._bsd_permanentcountry_value = popupcontactaddress_country_id.Text;
            viewModel.singleContact.bsd_permanentcountry_en = popupcontactaddress_country_en.Text;

            viewModel.singleContact.bsd_permanentprovince_label = popupcontactaddress_province.Text;
            viewModel.singleContact._bsd_permanentprovince_value = popupcontactaddress_province_id.Text;
            viewModel.singleContact.bsd_permanentprovince_en = popupcontactaddress_province_en.Text;

            viewModel.singleContact.bsd_permanentdistrict_label = popupcontactaddress_district.Text;
            viewModel.singleContact._bsd_permanentdistrict_value = popupcontactaddress_district_id.Text;
            viewModel.singleContact.bsd_permanentdistrict_en = popupcontactaddress_district_en.Text;

            var tmp = new List<string>();
            if (!string.IsNullOrEmpty(viewModel.singleContact.bsd_permanentaddress)) { tmp.Add(viewModel.singleContact.bsd_permanentaddress); }
            if (!string.IsNullOrEmpty(viewModel.singleContact.bsd_permanentdistrict_label)) { tmp.Add(viewModel.singleContact.bsd_permanentdistrict_label); }
            if (!string.IsNullOrEmpty(viewModel.singleContact.bsd_permanentprovince_label)) { tmp.Add(viewModel.singleContact.bsd_permanentprovince_label); }
            if (!string.IsNullOrEmpty(viewModel.singleContact.bsd_permanentcountry_label)) { tmp.Add(viewModel.singleContact.bsd_permanentcountry_label); }

            viewModel.singleContact.bsd_permanentaddress1 = string.Join(", ", tmp);

            var tmpEN = new List<string>();
            if (!string.IsNullOrEmpty(viewModel.singleContact.bsd_permanenthousenumber)) { tmpEN.Add(viewModel.singleContact.bsd_permanenthousenumber); }
            if (!string.IsNullOrEmpty(viewModel.singleContact.bsd_permanentdistrict_en)) { tmpEN.Add(viewModel.singleContact.bsd_permanentdistrict_en); }
            if (!string.IsNullOrEmpty(viewModel.singleContact.bsd_permanentprovince_en)) { tmpEN.Add(viewModel.singleContact.bsd_permanentprovince_en); }
            if (!string.IsNullOrEmpty(viewModel.singleContact.bsd_permanentcountry_en)) { tmpEN.Add(viewModel.singleContact.bsd_permanentcountry_en); }

            viewModel.singleContact.bsd_diachithuongtru = string.Join(", ", tmpEN);

            this.hide_popup_contactaddress(null, null);
        }

        #region Nhu cau dia diem
        private async void BtnAddNhuCauDiaDiem_Clicked(object sender, EventArgs e)
        {
            isShowingPopup = true;
            viewModel.IsBusy = true;

            if (viewModel.list_provinceefornhucaudiadiem.Count == 0)
            {
                await viewModel.LoadAllProvincesForNhuCauVeDiaDiem();
            }

            listviewProvinces.SetBinding(ListView.ItemsSourceProperty, new Binding("list_provinceefornhucaudiadiem", source: viewModel));
            popup_province.IsVisible = true;

            viewModel.IsBusy = false;
        }

        private async void popupprovinces_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Provinces selected = e.Item as Provinces;
            if (viewModel.list_nhucauvediadiem.ToList().FirstOrDefault(x => x.new_provinceid == selected.new_provinceid) == null)
            {
                this.hide_popup_province(null, null);
                viewModel.IsBusy = true;
                await viewModel.Add_NhuCauDiaDiem(selected.new_provinceid.ToString(), viewModel.singleContact.contactid);

                if (viewModel.list_nhucauvediadiem.FirstOrDefault(x => x.new_id == null) != null)
                {
                    var index = viewModel.list_nhucauvediadiem.IndexOf(viewModel.list_nhucauvediadiem.FirstOrDefault(x => x.new_id == null));
                    viewModel.list_nhucauvediadiem[index] = selected;
                }
                else
                {
                    viewModel.list_nhucauvediadiem.Add(selected);
                }
                viewModel.IsBusy = false;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("", "Địa điểm đã tồn tại", "OK");
            }
        }

        private async void DeleteNhuCauVeDiaDiem_Tapped(object sender, EventArgs e)
        {
            Label lblClicked = (Label)sender;
            var item = (TapGestureRecognizer)lblClicked.GestureRecognizers[0];
            Provinces tmp = item.CommandParameter as Provinces;
            if (tmp != null && tmp.new_id != null)
            {
                bool x = await App.Current.MainPage.DisplayAlert("", "Bạn có chắc chắn muốn xoá?", "Xoá", "Huỷ");
                if (x)
                {
                    viewModel.IsBusy = true;
                    await viewModel.Delete_NhuCauDiaDiem(tmp.new_provinceid.ToString(), viewModel.singleContact.contactid);
                    viewModel.list_nhucauvediadiem.Remove(tmp);
                    //if (viewModel.list_nhucauvediadiem.Count < 3)
                    //{
                    //    viewModel.list_nhucauvediadiem.Add(new Provinces());
                    //}
                    viewModel.IsBusy = false;
                }
            }
        }

        private async void ShowMoreNhuCauDiaDiem_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.PageNhuCauDiaDiem++;
            await viewModel.Load_NhuCauVeDiaDiem(viewModel.singleContact.contactid.ToString());
            viewModel.IsBusy = false;
        }
        //private async void BtnRemoveNhuCauDiaDiem_Clicked(object sender, EventArgs e)
        //{
        //    var tmp = datagrid_nhucavediadiem.SelectedItem as Provinces;
        //    if (tmp != null && tmp.new_id != null)
        //    {
        //        bool x = await App.Current.MainPage.DisplayAlert("", "Bạn có chắc chắn muốn xoá?", "Xoá", "Huỷ");
        //        if (x)
        //        {
        //            viewModel.IsBusy = true;
        //            await viewModel.Delete_NhuCauDiaDiem(tmp.new_provinceid.ToString(), viewModel.singleContact.contactid);
        //            viewModel.list_nhucauvediadiem.Remove(tmp);
        //            if (viewModel.list_nhucauvediadiem.Count < 3)
        //            {
        //                viewModel.list_nhucauvediadiem.Add(new Provinces());
        //            }
        //            viewModel.IsBusy = false;
        //        }
        //    }
        //}

        private void hide_popup_province(object sender, EventArgs e)
        {
            isShowingPopup = false;
            popup_province.IsVisible = false;
        }

        private void SearchBarProvince_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == null)
            {
                listviewProvinces.ItemsSource = viewModel.list_provinceefornhucaudiadiem;
                return;
            }
            listviewProvinces.ItemsSource = viewModel.list_provinceefornhucaudiadiem.Where(x => x.new_name.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        #endregion

        #region Danh sach dat cho
        private async void ShowMoreDanhSachDatCho_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.PageDanhSachDatCho++;
            await viewModel.LoadQueuesForContactForm(viewModel.singleContact.contactid.ToString());
            viewModel.IsBusy = false;
        }
        #endregion

        #region Danh sach dat coc
        private async void ShowMoreDanhSachDatCoc_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.PageDanhSachDatCoc++;
            await viewModel.LoadReservationForContactForm(viewModel.singleContact.contactid.ToString());
            viewModel.IsBusy = false;
        }
        #endregion

        #region Dah sach hop dong
        private async void ShowMoreDanhSachHopDong_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.PageDanhSachDatCoc++;
            await viewModel.LoadOptoinEntryForContactForm(viewModel.singleContact.contactid.ToString());
            viewModel.IsBusy = false;
        }
        #endregion

        #region Cham soc khach hang
        private async void ShowMoreChamSocKhachHang_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.PageChamSocKhachHang++;
            await viewModel.LoadCaseForContactForm(viewModel.singleContact.contactid.ToString());
            viewModel.IsBusy = false;
        }
        #endregion

        /// CLEARBUTTON CLICKED
        private void clear_entry_parentcustomerid(object sender, EventArgs e)
        {
            viewModel.singleContact._parentcustomerid_value = null;
            viewModel.singleContact.parentcustomerid_label = null;
        }

        private void clear_entry_protector(object sender, EventArgs e)
        {
            viewModel.singleContact._bsd_protecter_value = null;
            viewModel.singleContact.bsd_protecter_label = null;
        }

        private void clear_entry_country(object sender, EventArgs e)
        {
            popupcontactaddress_country.Text = null;
            popupcontactaddress_country_id.Text = null;
            popupcontactaddress_country_en.Text = null;
            popupcontactaddress_country.HasClearButton = false;

            this.clear_entry_province(null, null);
            this.clear_entry_district(null, null);
        }

        private void clear_entry_province(object sender, EventArgs e)
        {
            popupcontactaddress_province.Text = null;
            popupcontactaddress_province_id.Text = null;
            popupcontactaddress_province_en.Text = null;
            popupcontactaddress_province.HasClearButton = false;

            this.clear_entry_district(null, null);
        }

        private void clear_entry_district(object sender, EventArgs e)
        {
            popupcontactaddress_district.Text = null;
            popupcontactaddress_district_id.Text = null;
            popupcontactaddress_district_en.Text = null;
            popupcontactaddress_district.HasClearButton = false;
        }
        private void Clear_dategrant_picker_Clicked(object sender, EventArgs e)
        {
            viewModel.singleContact.bsd_dategrant = null;
        }
        private void Clear_issuedonpassport_picker_Clicked(object sender, EventArgs e)
        {
            viewModel.singleContact.bsd_issuedonpassport = null;
        }
        private void Clear_contact_group_picker_Clicked(object sender, EventArgs e)
        {
            viewModel.singleContactgroup = null;
        }

        ////////////////////////////// SEND FORM TO CRM
        private async void AddContact_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            var check = await checkData();
            if (check == "Sucesses")
            {
                await viewModel.uploadImageCMND();
                var created = await viewModel.createContact(viewModel.singleContact);

                if (created != new Guid())
                {
                    Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "Đã tạo khách hàng cá nhân thành công", "OK");
                    Xamarin.Forms.Application.Current.Properties["update"] = "1";

                    this.reload(created);
                    //Xamarin.Forms.Application.Current.MainPage.Navigation.InsertPageBefore(new LeadForm(created), Xamarin.Forms.Application.Current.MainPage);
                    //Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "Tạo khách hàng cá nhân thất bại", "OK");
                }
            }
            else
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", check, "OK");
            }

            viewModel.IsBusy = false;
        }

        private async void UpdateContact_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            var check = await checkData();
            if (check == "Sucesses")
            {
                await viewModel.uploadImageCMND();
                var updated = await viewModel.updateContact(viewModel.singleContact);

                if (updated)
                {
                    Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "Đã cập nhật thành công", "OK");
                    Xamarin.Forms.Application.Current.Properties["update"] = "1";

                    this.reload(viewModel.singleContact.contactid);

                    //Xamarin.Forms.Application.Current.MainPage.Navigation.InsertPageBefore(new LeadForm(viewModel.singleLead.leadid), Xamarin.Forms.Application.Current.MainPage);
                    //Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", "Cập nhật thất bại", "OK");
                }
            }
            else
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("", check, "OK");
            }
            viewModel.PageDanhSachDatCho = 1;
            viewModel.PageChamSocKhachHang = 1;
            viewModel.PageDanhSachDatCoc = 1;
            viewModel.PageDanhSachHopDong = 1;
            viewModel.PageDuAnQuanTam = 1;
            viewModel.PageNhuCauDiaDiem = 1;
            viewModel.IsBusy = false;
        }

        private async Task<String> checkData()
        {
            if (string.IsNullOrWhiteSpace(viewModel.singleContact.bsd_lastname) || string.IsNullOrWhiteSpace(viewModel.singleContact.mobilephone) ||
                string.IsNullOrEmpty(viewModel.singleContact.gendercode) || string.IsNullOrEmpty(viewModel.singleContact.bsd_localization) || 
                (!viewModel.singleContact.bsd_loingysinh && viewModel.singleContact.birthdate == null) ||
                string.IsNullOrEmpty(viewModel.singleContact.bsd_identitycardnumber) ||
                string.IsNullOrEmpty(viewModel.singleContact.emailaddress1) || string.IsNullOrEmpty(viewModel.singleContact.bsd_housenumberstreet) ||
                viewModel.SelectedTypes.Count == 0 ||
                (viewModel.singleContact.bsd_loingysinh && string.IsNullOrEmpty(viewModel.singleContact.bsd_nmsinh)))
            {

                return "Vui lòng nhập các trường bắt buộc (trường có gắn dấu *)";

            }
            if (viewModel.singleContact.bsd_haveprotector && viewModel.singleContact._bsd_protecter_value == null)
            {
                return "Vui lòng chọn người bảo hộ";
            }
            if (!viewModel.singleContact.bsd_haveprotector && (DateTime.Now.Year - DateTime.Parse(viewModel.singleContact.birthdate.ToString()).Year < 18))
            {
                return "Khách hàng phải từ 18 tuổi";
            }
            if(!PhoneNumberFormatVNHelper.CheckValidate(viewModel.singleContact.mobilephone))
                return "Số điện thoại sai định dạng";

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!string.IsNullOrEmpty(viewModel.singleContact.emailaddress1) && !string.IsNullOrWhiteSpace(viewModel.singleContact.emailaddress1))
            {

                Match match = regex.Match(viewModel.singleContact.emailaddress1);
                if (!match.Success) { viewModel.IsBusy = false; return "Địa chỉ mail sai. Vui lòng nhập lại!"; }
            }

            return "Sucesses";
        }

        private void MyNewDatePicker_DateChanged(object sender, EventArgs e)
        {
            if (viewModel.singleContact.birthdate != null && (viewModel.singleContact.birthdate.Value.AddYears(18).CompareTo(DateTime.Now) > 0))
            {
                if (!viewModel.singleContact.bsd_haveprotector)
                {
                    //if(DateTime.Now.Year - DateTime.Parse(viewModel.singleContact.birthdate.ToString()).Year < 18)

                    Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Lỗi", "Khách hàng phải từ 18 tuổi", "OK");
                    viewModel.singleContact.birthdate = null;
                    viewModel.singleContact.bsd_nmsinh = null;
                }

            }
            viewModel.PhongThuy.gioi_tinh = viewModel.singleContact.gendercode != null ? Int32.Parse(viewModel.singleContact.gendercode) : 0;
            viewModel.PhongThuy.nam_sinh = viewModel.singleContact.birthdate.HasValue ? viewModel.singleContact.birthdate.Value.Year : 0;
        }

        void HaveProtecter_changeChecked(object sender, System.EventArgs e)
        {
            this.clear_entry_protector(null, null);
            viewModel.singleContact.birthdate = null;
            viewModel.singleContact.bsd_nmsinh = null;
        }

        protected override bool OnBackButtonPressed()
        {
            if (this.isShowingPopup)
            {
                this.hide_popup_fullname(null, null);
                this.hide_popup_contactaddress(null, null);
                this.hide_listview_popup(null, null);
                this.hide_popup_province(null, null);
                this.hide_popup_permanent_contactaddress(null, null);
                this.BtnCloseModalImage_Clicked(null, null);
                return true;
            }
            return base.OnBackButtonPressed();
        }

        void MatTruocCMND_Tapped(object sender, System.EventArgs e)
        {
            List<OptionSet> menuItem = new List<OptionSet>();
            if (viewModel.singleContact.bsd_mattruoccmnd_base64 != null)
            {
                menuItem.Add(new OptionSet { Label = "Xem ảnh mặt trước cmnd", Val = "Front" });
            }
            menuItem.Add(new OptionSet { Label = "Chụp ảnh", Val = "Front" });
            menuItem.Add(new OptionSet { Label = "Chọn ảnh từ thư viện", Val = "Front" });
            this.showMenuImageCMND(menuItem);
        }

        void MatSauCMND_Tapped(object sender, System.EventArgs e)
        {
            List<OptionSet> menuItem = new List<OptionSet>();
            if (viewModel.singleContact.bsd_matsaucmnd_base64 != null)
            {
                menuItem.Add(new OptionSet { Label = "Xem ảnh mặt sau cmnd", Val = "Behind" });
            }
            menuItem.Add(new OptionSet { Label = "Chụp ảnh", Val = "Behind" });
            menuItem.Add(new OptionSet { Label = "Chọn ảnh từ thư viện", Val = "Behind" });
            this.showMenuImageCMND(menuItem);
        }

        void showMenuImageCMND(List<OptionSet> listItem)
        {
            popup_menu_imageCMND.ItemSource = listItem;

            popup_menu_imageCMND.focus();
        }

        async void MenuItem_Tapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var item = e.Item as OptionSet;
            popup_menu_imageCMND.unFocus();

            Stream resultStream;
            byte[] arrByte;
            string base64String;

            switch (item.Label)
            {
                case "Chụp ảnh":
                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await DisplayAlert("No Camera", ":( No camera available.", "OK");
                        return;
                    }
                    if ((await PermissionHelper.CheckPermissions(permissionType.Camera)) == permissionStatus.Granted
                        && await PermissionHelper.CheckPermissions(permissionType.Storage) == permissionStatus.Granted)
                    {
                        var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                        {
                            Directory = "Inland",
                            SaveToAlbum = true,
                            //CompressionQuality = 75,
                            CustomPhotoSize = 50,
                            PhotoSize = PhotoSize.MaxWidthHeight,
                            MaxWidthHeight = 2000,
                            DefaultCamera = CameraDevice.Front
                        });

                        if (file == null)
                            return;

                        resultStream = file.GetStream();
                        using (var memoryStream = new MemoryStream())
                        {
                            resultStream.CopyTo(memoryStream);
                            arrByte = memoryStream.ToArray();
                        }
                        base64String = Convert.ToBase64String(arrByte);
                        var tmp1 = base64String.Length;
                        if (item.Val == "Front") { viewModel.singleContact.bsd_mattruoccmnd_base64 = base64String; }
                        else if (item.Val == "Behind") { viewModel.singleContact.bsd_matsaucmnd_base64 = base64String; }
                    }

                    break;
                case "Chọn ảnh từ thư viện":
                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                        return;
                    }
                    if (await PermissionHelper.CheckPermissions(permissionType.Storage) == permissionStatus.Granted)
                    {
                        var file2 = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                        {
                            PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                            //CompressionQuality = 50,
                        });


                        if (file2 == null)
                            return;
                        Stream result = file2.GetStream();
                        if (result != null)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                result.CopyTo(memoryStream);
                                arrByte = memoryStream.ToArray();
                            }
                            base64String = Convert.ToBase64String(arrByte);
                            var tmp = base64String.Length;
                            if (item.Val == "Front") { viewModel.singleContact.bsd_mattruoccmnd_base64 = base64String; }
                            else if (item.Val == "Behind") { viewModel.singleContact.bsd_matsaucmnd_base64 = base64String; }
                        }
                    }
                    break;
                default:
                    if (item.Val == "Front") { image_detailCMNDImage.Source = viewModel.singleContact.bsd_mattruoccmnd_source; }
                    else if (item.Val == "Behind") { image_detailCMNDImage.Source = viewModel.singleContact.bsd_matsaucmnd_source; }
                    this.showDetailCMNDImage();
                    break;

            }
        }

        private void showDetailCMNDImage()
        {
            this.isShowingPopup = true;
            NavigationPage.SetHasNavigationBar(this, false);
            popup_detailCMNDImage.IsVisible = true;
        }

        void BtnCloseModalImage_Clicked(object sender, System.EventArgs e)
        {
            this.isShowingPopup = false;
            NavigationPage.SetHasNavigationBar(this, true);
            popup_detailCMNDImage.IsVisible = false;
        }

        private void check_birthyear(object sender, EventArgs e)
        {
            checkbox_birthd.IsChecked = false;
            viewModel.singleContact.birthdate = null;
            viewModel.singleContact.bsd_loingysinh = true;
            //birthdate_picker.IsVisible = false;
            //Namsinh_txt.IsVisible = true;
        }

        private void check_birthday(object sender, EventArgs e)
        {
            checkbox_birthy.IsChecked = false;
            viewModel.singleContact.birthdate = null;
            viewModel.singleContact.bsd_nmsinh = null;
            viewModel.singleContact.bsd_loingysinh = false;
            //Namsinh_txt.IsVisible = false;
            //birthdate_picker.IsVisible = true;
        }

        private void text_yearfocus(object sender, FocusEventArgs e)
        {
            var yeartxt = (sender as Entry).Text;
            if (!string.IsNullOrEmpty(yeartxt))
            {

                DateTime yeartmp = new DateTime(int.Parse(yeartxt), 1, 1);

                viewModel.singleContact.birthdate = yeartmp;
                //if (!viewModel.singleContact.bsd_haveprotector)
                //{
                //    if (yeartmp.AddYears(18).CompareTo(DateTime.Now) > 0)
                //    {
                //        Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Lỗi", "Khách hàng phải từ 18 tuổi", "OK");
                //        viewModel.singleContact.bsd_nmsinh = null;

                //    }
                //}

            }
        }

        private void check_year(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != null)
            {
                try
                {
                    var yeartmp = int.Parse(e.NewTextValue);
                    if (yeartmp > DateTime.Now.Year)
                    {
                        (sender as Entry).Text = e.OldTextValue;
                    }
                }
                catch
                {
                    (sender as Entry).Text = "";
                }
            }
        }

        #region Du an quan tam
        private async void BtnAddDuanquantam_Clicked(object sender, EventArgs e)
        {

            isShowingPopup = true;

            viewModel.IsBusy = true;

            if (viewModel.list_project_lookup.Count == 0)
            {
                await viewModel.LoadAllProject();
            }

            listviewProject.SetBinding(ListView.ItemsSourceProperty, new Binding("list_project_lookup", source: viewModel));
            popup_project.IsVisible = true;

            viewModel.IsBusy = false;
        }

        //private async void BtnRemoveDuanquantam_Clicked(object sender, EventArgs e)
        //{
        //    var tmp = datagrid_duanquantam.SelectedItem as ProjectList;

        //    if (tmp != null)
        //    {
        //        if (tmp.bsd_projectid != null)
        //        {
        //            bool x = await App.Current.MainPage.DisplayAlert("", "Bạn có chắc chắn muốn xoá?", "Xoá", "Huỷ");
        //            if (x)
        //            {
        //                viewModel.IsBusy = true;
        //                await viewModel.Delete_DuAnQuanTam(tmp.bsd_projectid, viewModel.singleContact.contactid);
        //                viewModel.list_Duanquantam.Remove(tmp);
        //                if (viewModel.list_Duanquantam.Count < 3)
        //                {
        //                    viewModel.list_Duanquantam.Add(new ProjectList());
        //                }
        //                viewModel.IsBusy = false;
        //            }
        //        }
        //    }
        //}
        private async void DeleteDuAnQuanTam_Tapped(object sender, EventArgs e)
        {
            Label lblClicked = (Label)sender;
            var item = (TapGestureRecognizer)lblClicked.GestureRecognizers[0];
            ProjectList tmp = item.CommandParameter as ProjectList;
            if (tmp != null)
            {
                if (tmp.bsd_projectid != null)
                {
                    bool x = await App.Current.MainPage.DisplayAlert("", "Bạn có chắc chắn muốn xoá?", "Xoá", "Huỷ");
                    if (x)
                    {
                        viewModel.IsBusy = true;
                        await viewModel.Delete_DuAnQuanTam(tmp.bsd_projectid, viewModel.singleContact.contactid);
                        viewModel.list_Duanquantam.Remove(tmp);
                        //if (viewModel.list_Duanquantam.Count < 3)
                        //{
                        //    viewModel.list_Duanquantam.Add(new ProjectList());
                        //}
                        viewModel.IsBusy = false;
                    }
                }
            }
        }

        private async void ShowMoreDuAnQuanTam_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            viewModel.PageDuAnQuanTam++;
            await viewModel.Load_DanhSachDuAn(viewModel.singleContact.contactid.ToString());
            viewModel.IsBusy = false;
        }

        private void SearchBarProject_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == null)
            {
                listviewProject.ItemsSource = viewModel.list_project_lookup;
                return;
            }
            listviewProject.ItemsSource = viewModel.list_project_lookup.Where(x => x.bsd_name.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private async void project_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ProjectList selected = e.Item as ProjectList;
            if (viewModel.list_Duanquantam.ToList().FirstOrDefault(x => x.bsd_projectid == selected.bsd_projectid) == null)
            {
                this.hide_popup_project(null, null);
                viewModel.IsBusy = true;
                await viewModel.Add_DuAnQuanTam(selected.bsd_projectid, viewModel.singleContact.contactid);
                if (viewModel.list_Duanquantam.FirstOrDefault(x => x.bsd_projectid == null) != null)
                {
                    var index = viewModel.list_Duanquantam.IndexOf(viewModel.list_Duanquantam.FirstOrDefault(x => x.bsd_projectid == null));
                    viewModel.list_Duanquantam[index] = selected;
                }
                else
                {
                    viewModel.list_Duanquantam.Add(selected);
                }
                viewModel.IsBusy = false;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("", "Dự án đã tồn tại", "OK");
            }
        }

        private void hide_popup_project(object sender, EventArgs e)
        {
            isShowingPopup = false;

            popup_project.IsVisible = false;
        }

        private void Clear_issuedateidcard_picker_Clicked(object sender, EventArgs e)
        {
            viewModel.singleContact.bsd_issuedateidcard = null;
        }

        #endregion      
    }
}