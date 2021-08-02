using ConasiCRM.Portable.Controls;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;

namespace ConasiCRM.Portable.ViewModels
{
    /// <summary>
    /// Form nao co su dung lookup thi xai base nay.
    /// </summary>
    public class FormLookupViewModel : BaseViewModel
    {
        public LookUpConfig CurrentLookUpConfig { get; set; }

        private bool _showLookUpModal;
        public bool ShowLookUpModal
        {
            get { return _showLookUpModal; }
            set
            {
                if (_showLookUpModal != value)
                {
                    _showLookUpModal = value;
                    OnPropertyChanged(nameof(ShowLookUpModal));
                }
            }
        }

        private bool _lookUpLoading;
        public bool LookUpLoading
        {
            get { return _lookUpLoading; }
            set
            {
                if (_lookUpLoading != value)
                {
                    _lookUpLoading = value;
                    OnPropertyChanged(nameof(LookUpLoading));
                }
            }
        }

        public event EventHandler AfterLookUpClose;

        public ICommand CloseLookUpModal => new Command(OnCloseLookUp);

        public ModalContentView ModalLookUp { get; set; }
        public StackLayout Header { get; set; }
        public ImageButton BtnClose { get; set; }
        public StackLayout ModalContent { get; set; }
        public StackLayout stackLayoutModalLookUp { get; set; }
        public Label ModalTitle { get; set; }
        public Xamarin.Forms.SearchBar searchBar { get; set; }

        public StackLayout CustomerLookUpHeader { get; set; }

        public async Task FetchData()
        {
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<Models.LookUp>>(CurrentLookUpConfig.EntityName, string.Format(CurrentLookUpConfig.FetchXml, CurrentLookUpConfig.LookUpPage, CurrentLookUpConfig.Keyword));
            if (result == null || result.value.Count == 0) return;
            var data = result.value;
            var count = data.Count;
            for (int i = 0; i < count; i++)
            {
                CurrentLookUpConfig.LookUpData.Add(data[i]);
            }
        }

        public void InitializeModal()
        {
            ModalLookUp.SetBinding(ContentView.IsVisibleProperty, new Binding("ShowLookUpModal"));

            ModalContent = new StackLayout() { BackgroundColor = Color.White };

            #region Modal Title
            Header = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.FromHex("#F3F3F3"),
                Padding = 10,
            };

            ModalTitle = new Label()
            {
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 18
            };

            BtnClose = new ImageButton()
            {
                Source = "close.png",
                Padding = 5,
                BackgroundColor = Color.White,
                HeightRequest = 32,
                WidthRequest = 32,
                Aspect = Aspect.AspectFit
            };
            BtnClose.SetBinding(ImageButton.CommandProperty, new Binding("CloseLookUpModal"));

            Header.Children.Add(ModalTitle);
            Header.Children.Add(BtnClose);
            #endregion

            #region Search bar
            searchBar = new Xamarin.Forms.SearchBar()
            {
                FontSize = 16,
                Placeholder = "Nhập từ khóa tìm kiếm..."
            };
            searchBar.SetBinding(Xamarin.Forms.SearchBar.IsEnabledProperty, new Binding("LookUpLoading")
            {
                Converter = new Converters.BoolToBoolConverter()
            });
            searchBar.SearchButtonPressed += Search;
            searchBar.TextChanged += (sender, e) =>
            {
                if (e.OldTextValue != null && e.NewTextValue == "")
                    Search(sender, e);
            };
            #endregion

            #region Border search
            RadBorder searchBorder = new RadBorder()
            {
                Margin = 5,
                Content = searchBar
            };
            if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.UWP)
                searchBorder.BorderColor = Color.FromHex("#eeeeee");
            else
                searchBorder.BorderColor = Color.Transparent;
            #endregion

            stackLayoutModalLookUp = new StackLayout()
            {
                Padding = new Thickness(10, 0, 10, 0)
            };

            ModalContent.Children.Add(Header);
            ModalContent.Children.Add(searchBorder);
            ModalContent.Children.Add(stackLayoutModalLookUp);

            ModalLookUp.Content = ModalContent;
        }

        public ListView CreateNewListView(string ConfigName)
        {
            ActivityIndicator activityIndicator = new ActivityIndicator()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Color = Color.Black,
                HeightRequest = 20
            };
            activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, new Binding("LookUpLoading"));
            ListView listView = new ListView(cachingStrategy: ListViewCachingStrategy.RecycleElement)
            {
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(() =>
                {
                    var itemLayout = new StackLayout()
                    {
                        BackgroundColor = Color.White,
                        Padding = new Thickness(10)
                    };
                    Label label = new Label()
                    {
                        TextColor = Color.FromHex("#333333"),
                        FontSize = 16
                    };
                    label.SetBinding(Label.TextProperty, new Binding("Name"));
                    itemLayout.Children.Add(label);
                    return new ViewCell { View = itemLayout };
                }),
                Footer = activityIndicator
            };
            listView.ItemTapped += (sender, e) =>
            {
                var item = e.Item as Models.LookUp;
                string itemid = item.Id.ToString();
                var model = this;
                PropertyInfo prop = model.GetType().GetProperty(CurrentLookUpConfig.PropertyName);
                prop.SetValue(model, item);
                listView.SelectedItem = null;

                this.OnCloseLookUp();
            };
            listView.ItemAppearing += async (sender, e) =>
            {
                LookUpLoading = true;

                if (e.Item == CurrentLookUpConfig.LookUpData[CurrentLookUpConfig.LookUpData.Count -1])
                {
                    CurrentLookUpConfig.LookUpPage += 1;
                    await FetchData();
                }

                //var itemAppearing = e.Item as Portable.Models.LookUp;
                //var lastItem = CurrentLookUpConfig.LookUpData.LastOrDefault();
                //if (lastItem != null && itemAppearing.Id.Equals(lastItem.Id))
                //{
                //    CurrentLookUpConfig.LookUpPage += 1;
                //    await FetchData();
                //}
                LookUpLoading = false;
            };
            listView.SetBinding(ListView.ItemsSourceProperty, new Binding(ConfigName + ".LookUpData"));
            return listView;
        }

        public async void ProcessLookup(string ConfigName, bool Reset = false)
        {
            // nếu chưa có listview.
            if (CurrentLookUpConfig.ListView == null)
            {
                var listView = CreateNewListView(ConfigName);

                stackLayoutModalLookUp.Children.Add(listView);

                CurrentLookUpConfig.ListView = listView;
                CurrentLookUpConfig.LookUpPage = 1;
                await FetchData();
                //LookUpLoading = false;
            }
            else // hướng này đã có rồi.
            {
                CurrentLookUpConfig.ListView.IsVisible = true;
                if (Reset || CurrentLookUpConfig.LookUpData.Count == 0)
                {
                    CurrentLookUpConfig.LookUpData.Clear();
                    CurrentLookUpConfig.LookUpPage = 1;
                    await FetchData();
                }
            }
            CurrentLookUpConfig.ListView.ScrollTo(CurrentLookUpConfig.LookUpData.FirstOrDefault(), ScrollToPosition.Start, false);
            ModalTitle.Text = CurrentLookUpConfig.LookUpTitle;
            ShowLookUpModal = true;
            LoadingHelper.Hide();
        }

        public async void Search(object sender, EventArgs e)
        {
            LookUpLoading = true;
            CurrentLookUpConfig.LookUpData.Clear();
            CurrentLookUpConfig.LookUpPage = 1;
            CurrentLookUpConfig.Keyword = searchBar.Text ?? "";
            await FetchData();
            LookUpLoading = false;
        }

        public void OnCloseLookUp()
        {
            if (CurrentLookUpConfig.ListView != null)
            {
                // truong hop ma phaseslauchner, ma chua chon project hti se null.
                CurrentLookUpConfig.ListView.IsVisible = false;
            }

            if (searchBar.Text != null && searchBar.Text.Length > 0)
            {
                searchBar.Text = "";
            }
            ShowLookUpModal = false;
            if (AfterLookUpClose != null)
            {
                AfterLookUpClose.Invoke(this, EventArgs.Empty);
            }
        }

        // Phần này chỉ khi có customer lookup.
        public Grid gridCustomer { get; set; }
        public Button BtnContact { get; set; }
        public Button BtnAccount { get; set; }
        public void InitCustomerLookUpHeader()
        {
            if (gridCustomer != null)
            {
                return;
            }
            gridCustomer = new Grid()
            {
                ColumnSpacing = 0,
                Padding = new Thickness(10, 0, 10, 0),
                IsVisible = true
            };
            gridCustomer.RowDefinitions.Add(new RowDefinition() { Height = 40 });
            gridCustomer.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });

            gridCustomer.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(5, GridUnitType.Star) });
            gridCustomer.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(5, GridUnitType.Star) });

            BtnContact = new Button()
            {
                Text = "Khách hàng cá nhân",
                BorderColor = Color.Black,
                BorderWidth = 1,
            };
            Grid.SetColumn(BtnContact, 0);
            Grid.SetRow(BtnContact, 0);

            BtnAccount = new Button()
            {
                Text = "Khách hàng doanh nghiệp",
                BorderColor = Color.Black,
                BorderWidth = 1,
            };
            Grid.SetColumn(BtnAccount, 1);
            Grid.SetRow(BtnAccount, 0);
            gridCustomer.Children.Add(BtnContact);
            gridCustomer.Children.Add(BtnAccount);

            ModalContent.Children.Insert(1, gridCustomer);
        }

        public void OnSwitch()
        {
            if (searchBar.Text != null && searchBar.Text.Length > 0)
            {
                searchBar.Text = null;
                if (CurrentLookUpConfig != null)
                {
                    CurrentLookUpConfig.Keyword = "";
                    CurrentLookUpConfig.LookUpData.Clear();
                    CurrentLookUpConfig.LookUpPage = 1;
                }
            }
        }
    }
}
