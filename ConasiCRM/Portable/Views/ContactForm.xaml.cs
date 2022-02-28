using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Text.RegularExpressions;
using ConasiCRM.Portable.Helpers;
using ConasiCRM.Portable.Resources;

namespace ConasiCRM.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactForm : ContentPage
    {
        public Action<bool> OnCompleted;      
        private ContactFormViewModel viewModel;
        private Guid Id;

        public ContactForm()
        {
            InitializeComponent();
            this.Id = Guid.Empty;
            Init();
            Create();
        }

        public ContactForm(Guid contactId)
        {
            InitializeComponent();
            this.Id = contactId;
            Init();
            Update();
        }

        public async void Init()
        {
            this.BindingContext = viewModel = new ContactFormViewModel();
            SetPreOpen();
        }

        private void Create()
        {
            this.Title = Language.tao_moi_khach_hang_ca_nhan_title;
            btn_save_contact.Text = Language.tao_khach_hang;
            btn_save_contact.Clicked += CreateContact_Clicked;
        }

        private void CreateContact_Clicked(object sender, EventArgs e)
        {
            SaveData(null);
        }

        private async void Update()
        {
            await loadData(this.Id.ToString());
            this.Title = Language.cap_nhat_khach_hang_ca_nhan_title;
            btn_save_contact.Text = Language.cap_nhat_khach_hang;
            btn_save_contact.Clicked += UpdateContact_Clicked;
            if (viewModel.singleContact.contactid != Guid.Empty)
                OnCompleted?.Invoke(true);
            else
                OnCompleted?.Invoke(false);
        }

        private void UpdateContact_Clicked(object sender, EventArgs e)
        {
            SaveData(this.Id.ToString());
        }

        public async Task loadData(string contactId)
        {
            LoadingHelper.Show();

            if (contactId != null)
            {
                await viewModel.LoadOneContact(contactId);
                await viewModel.GetImageCMND();
                if (viewModel.singleContact.gendercode != null)
                {
                    viewModel.singleGender = ContactGender.GetGenderById(viewModel.singleContact.gendercode);
                }
                if (viewModel.singleContact.bsd_localization != null)
                {
                    viewModel.singleLocalization = AccountLocalization.GetLocalizationById(viewModel.singleContact.bsd_localization);
                }
                if (viewModel.singleContact._parentcustomerid_value != null)
                {
                    viewModel.Account = new Models.LookUp
                    {
                        Name = viewModel.singleContact.parentcustomerid_label,
                        Id = Guid.Parse(viewModel.singleContact._parentcustomerid_value)
                    };
                }
            }
            LoadingHelper.Hide();
        }

        private async void SaveData(string id)
        {
            if (string.IsNullOrWhiteSpace(viewModel.singleContact.bsd_fullname))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_ho_ten);
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.singleContact.mobilephone))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_sdt);               
                return;
            }
            if (viewModel.singleGender == null || viewModel.singleGender.Val == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_gioi_tinh);
                return;
            }
            if (viewModel.singleLocalization == null || viewModel.singleLocalization.Val == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_quoc_tich);
                return;
            }

            if (viewModel.singleContact.birthdate == null)
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_ngay_sinh);
                return;
            }
            if (DateTime.Now.Year - DateTime.Parse(viewModel.singleContact.birthdate.ToString()).Year < 18)
            {
                ToastMessageHelper.ShortMessage(Language.khach_hang_phai_tu_18_tuoi);
                return;
            }
            if (!string.IsNullOrWhiteSpace(viewModel.singleContact.emailaddress1))
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(viewModel.singleContact.emailaddress1);
                if (!match.Success)
                {
                    ToastMessageHelper.ShortMessage(Language.email_sai_dinh_dang);
                    return;
                }

                if (!await viewModel.CheckEmail(viewModel.singleContact.emailaddress1, id))
                {
                    ToastMessageHelper.ShortMessage(Language.email_da_duoc_su_dung);
                    return;
                }
            }
            if (string.IsNullOrWhiteSpace(viewModel.ContactAddress.address))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_dia_chi_lien_lac);
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.PermanentAddress.address))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_chon_dia_chi_thuong_tru);
                return;
            }
            if (string.IsNullOrWhiteSpace(viewModel.singleContact.bsd_identitycardnumber))
            {
                ToastMessageHelper.ShortMessage(Language.vui_long_nhap_so_cmnd);
                return;
            }
            if (!await viewModel.CheckCMND(viewModel.singleContact.bsd_identitycardnumber, id))
            {
                ToastMessageHelper.ShortMessage(Language.so_cmnd_da_duoc_su_dung);
                return;
            }
            if (!string.IsNullOrWhiteSpace(viewModel.singleContact.bsd_identitycardnumber) && !await viewModel.CheckPassport(viewModel.singleContact.bsd_passport, id))
            {
                ToastMessageHelper.ShortMessage(Language.so_ho_chieu_da_duoc_su_dung);
                return;
            }

            viewModel.singleContact.bsd_localization = viewModel.singleLocalization != null && viewModel.singleLocalization.Val != null ? viewModel.singleLocalization.Val : null;
            viewModel.singleContact.gendercode = viewModel.singleGender != null && viewModel.singleGender.Val != null ? viewModel.singleGender.Val : null;
            viewModel.singleContact._parentcustomerid_value = viewModel.Account != null && viewModel.Account.Id != null ? viewModel.Account.Id.ToString() : null;

            if (id == null)
            {
                LoadingHelper.Show();               
                var created = await viewModel.createContact(viewModel.singleContact);

                if (created != new Guid())
                {
                    if (CustomerPage.NeedToRefreshContact.HasValue) CustomerPage.NeedToRefreshContact = true;
                    if (QueueForm.NeedToRefresh.HasValue) QueueForm.NeedToRefresh = true;
                    await viewModel.UpLoadCMND();
                    await Navigation.PopAsync();
                    ToastMessageHelper.ShortMessage(Language.tao_moi_thanh_cong);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_them_duoc_khach_hang_vui_long_thu_lai);
                }
            }
            else
            {
                LoadingHelper.Show();               
                var updated = await viewModel.updateContact(viewModel.singleContact);

                if (updated)
                {
                    LoadingHelper.Hide();
                    if (CustomerPage.NeedToRefreshContact.HasValue) CustomerPage.NeedToRefreshContact = true;
                    if (ContactDetailPage.NeedToRefresh.HasValue) ContactDetailPage.NeedToRefresh = true;

                    await viewModel.UpLoadCMND();
                    await Navigation.PopAsync();
                    ToastMessageHelper.ShortMessage(Language.cap_nhat_thanh_cong);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.khong_cap_nhat_duoc_khach_hang_vui_long_thu_lai);
                }
            }
        }

        public void SetPreOpen()
        {
            Lookup_GenderOptions.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                ContactGender.GetGenders();
                foreach (var item in ContactGender.GenderOptions)
                {
                    viewModel.GenderOptions.Add(item);
                }
                LoadingHelper.Hide();
            };
            Lookup_LocalizationOptions.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                AccountLocalization.Localizations();
                foreach (var item in AccountLocalization.LocalizationOptions)
                {
                    viewModel.LocalizationOptions.Add(item);
                }
                LoadingHelper.Hide();                
            }; 
            Lookup_Account.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadAccountsLookup();
                LoadingHelper.Hide();
            };
        }

        private void Handle_LayoutChanged(object sender, System.EventArgs e)
        {
            var width = ((DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density) - 35) / 2;
            var tmpHeight = width * 2 / 3;
            MatTruocCMND.HeightRequest = tmpHeight;
            MatSauCMND.HeightRequest = tmpHeight;
        }    

        public void MatTruocCMND_Tapped(object sender, System.EventArgs e)
        {
            List<OptionSet> menuItem = new List<OptionSet>();
            if (viewModel.singleContact.bsd_mattruoccmnd_base64 != null)
            {
                menuItem.Add(new OptionSet { Label = Language.xem_anh_mat_truoc_cmnd, Val = "Front" });
            }
            menuItem.Add(new OptionSet { Label = Language.chup_hinh, Val = "Front" });
            menuItem.Add(new OptionSet { Label = Language.chon_anh_tu_thu_vien, Val = "Front" });
            this.showMenuImageCMND(menuItem);
        }

        private void MatSauCMND_Tapped(object sender, System.EventArgs e)
        {
            List<OptionSet> menuItem = new List<OptionSet>();
            if (viewModel.singleContact.bsd_matsaucmnd_base64 != null)
            {
                menuItem.Add(new OptionSet { Label = Language.xem_anh_mat_sau_cmnd, Val = "Behind" });
            }
            menuItem.Add(new OptionSet { Label = Language.chup_hinh, Val = "Behind" });
            menuItem.Add(new OptionSet { Label = Language.chon_anh_tu_thu_vien, Val = "Behind" });
            this.showMenuImageCMND(menuItem);
        }

        private void showMenuImageCMND(List<OptionSet> listItem)
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
                case "Chụp hình":
                    
                    PermissionStatus cameraStatus = await PermissionHelper.RequestCameraPermission();
                    if (cameraStatus == PermissionStatus.Granted)
                    { 
                        var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                        {
                            SaveToAlbum = false,
                            PhotoSize = Plugin.Media.Abstractions.PhotoSize.MaxWidthHeight,
                            MaxWidthHeight = 600,
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

                    PermissionStatus storageStatus = await PermissionHelper.RequestPhotosPermission();
                    if (storageStatus == PermissionStatus.Granted)
                    {
                        var file2 = await MediaPicker.PickPhotoAsync();
                        if (file2 == null)
                            return;

                        Stream result = await file2.OpenReadAsync();
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

            NavigationPage.SetHasNavigationBar(this, false);
            popup_detailCMNDImage.IsVisible = true;
        }

        void BtnCloseModalImage_Clicked(object sender, System.EventArgs e)
        {
            NavigationPage.SetHasNavigationBar(this, true);
            popup_detailCMNDImage.IsVisible = false;
        }
    }
}