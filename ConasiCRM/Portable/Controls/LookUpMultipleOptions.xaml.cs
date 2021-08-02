using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using ConasiCRM.Portable.Models;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Controls
{
    public partial class LookUpMultipleOptions : Grid
    {
        public CenterModal CenterModal { get; set; }
        public event EventHandler OnSave;
        public event EventHandler OnDelete;

        public Func<Task> PreShow;

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(List<OptionSet>), typeof(LookUpMultipleOptions), null, BindingMode.TwoWay, null, propertyChanged: ItemSourceChange);
        public List<OptionSet> ItemsSource { get => (List<OptionSet>)GetValue(ItemsSourceProperty); set { SetValue(ItemsSourceProperty, value); } }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(LookUpMultipleOptions), null, BindingMode.TwoWay);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }

        public static readonly BindableProperty SelectedIdsProperty = BindableProperty.Create(nameof(SelectedIds), typeof(List<string>), typeof(LookUpMultipleOptions), null, BindingMode.TwoWay, null, propertyChanged: ItemSourceChange);
        public List<string> SelectedIds { get => (List<string>)GetValue(SelectedIdsProperty); set { SetValue(SelectedIdsProperty, value); } }

        private string _text;
        public string Text { get => _text; set { _text = value; OnPropertyChanged(nameof(Text)); } }

        private ListView lookUpListView;
        private Grid gridMain;
        private Grid gridButton;
        private SearchBar searchBar;
        private Button saveButton, cancelButton;
        public LookUpMultipleOptions()
        {
            InitializeComponent();
            this.Entry.SetBinding(EntryNoneBorder.PlaceholderProperty, new Binding("Placeholder") { Source = this });
            this.Entry.SetBinding(EntryNoneBorder.TextProperty, new Binding("Text") { Source = this });
        }

        private bool init = false;

        private async void OpenLookUp_Tapped(object sender1, EventArgs e1)
        {
            await Show();
        }

        public bool PreOpenOneTime { get; set; } = true;
        public async Task Show()
        {
            if (PreShow != null)
            {
                await PreShow();
                if (PreOpenOneTime)
                {
                    PreShow = null;
                }
            }

            if (init == false)
            {
                SetUpGridButton();
                SetUpListView();
                init = true;
            }
            else
            {
                if (searchBar.Text != null && searchBar.Text.Length > 0)
                {
                    searchBar.Text = "";
                }
            }

            CenterModal.CustomCloseButton(CancelButton_Clicked);
            CenterModal.Title = Placeholder;
            CenterModal.Footer = gridButton;
            CenterModal.Body = gridMain;
            await CenterModal.Show();
        }

        public async Task Hide() => await CenterModal.Hide();

        private void SetUpGridButton()
        {
            saveButton = new Button()
            {
                Text = "Lưu",
                BackgroundColor = (Color)App.Current.Resources["NavigationPrimary"],
                TextColor = Color.White,
                Padding = new Thickness(10, 5)
            };
            saveButton.Clicked += SaveButton_Clicked;


            Button deleteButton = new Button()
            {
                Text = "Xoá",
                TextColor = (Color)App.Current.Resources["NavigationPrimary"],
                BackgroundColor = Color.White,
                BorderColor = (Color)App.Current.Resources["NavigationPrimary"],
                Padding = new Thickness(10, 5),
                BorderWidth = 1
            };
            deleteButton.Clicked += async (object sender, EventArgs e) =>
            {
                this.ClearData();
                await this.Hide();
                OnSave?.Invoke(this, EventArgs.Empty);
            };


            cancelButton = new Button()
            {
                Text = "Đóng",
                TextColor = (Color)App.Current.Resources["NavigationPrimary"],
                BackgroundColor = Color.White,
                BorderColor = (Color)App.Current.Resources["NavigationPrimary"],
                Padding = new Thickness(10, 5),
                BorderWidth = 1
            };
            cancelButton.Clicked += CancelButton_Clicked;

            gridButton = new Grid()
            {
                ColumnSpacing = 2,
                Margin = new Thickness(5, 0, 5, 5)
            };

            gridButton.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(35) });
            gridButton.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            gridButton.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            gridButton.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            gridButton.Children.Add(cancelButton);
            gridButton.Children.Add(deleteButton);
            gridButton.Children.Add(saveButton);
            Grid.SetRow(cancelButton, 0);
            Grid.SetRow(deleteButton, 0);
            Grid.SetRow(saveButton, 0);

            Grid.SetColumn(cancelButton, 0);
            Grid.SetColumn(deleteButton, 1);
            Grid.SetColumn(saveButton, 2);
        }

        private void SetUpListView()
        {
            gridMain = new Grid();
            gridMain.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            gridMain.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

            StackLayout stSearchBar = new StackLayout();
            stSearchBar.Padding = new Thickness(10, 10, 10, 0);

            searchBar = new SearchBar();
            searchBar.Placeholder = "Từ khoá";
            searchBar.TextChanged += SearchBar_TextChangedEventArgs;

            SearchBarFrame searchBarFrame = new SearchBarFrame();
            searchBarFrame.Content = searchBar;

            stSearchBar.Children.Add(searchBarFrame);

            gridMain.Children.Add(stSearchBar);
            Grid.SetRow(stSearchBar, 0);

            lookUpListView = new ListView(ListViewCachingStrategy.RecycleElement);
            lookUpListView.BackgroundColor = Color.White;
            lookUpListView.HasUnevenRows = true;
            lookUpListView.SelectionMode = ListViewSelectionMode.None;
            lookUpListView.SeparatorVisibility = SeparatorVisibility.None;

            var dataTemplate = new DataTemplate(() =>
            {
                RadBorder st = new RadBorder();
                st.BorderThickness = new Thickness(0, 0, 0, 1);
                st.BorderColor = Color.FromHex("#eeeeee");
                st.Padding = 10;
                st.Margin = new Thickness(5, 0);

                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
                st.Content = grid;

                Label lb = new Label();
                lb.TextColor = Color.Black;
                lb.FontSize = 16;
                lb.SetBinding(Label.TextProperty, "Name");
                Grid.SetColumn(lb, 0);
                grid.Children.Add(lb);


                Label labelCheck = new Label();
                Grid.SetColumn(labelCheck, 1);
                labelCheck.Text = "\uf00c";
                labelCheck.TextColor = Color.DarkGreen;
                labelCheck.FontFamily = "FontAwesomeSolid";
                labelCheck.SetBinding(Label.IsVisibleProperty, "Selected");
                grid.Children.Add(labelCheck);

                return new ViewCell { View = st };
            });
            lookUpListView.ItemTemplate = dataTemplate;
            lookUpListView.ItemsSource = ItemsSource;

            lookUpListView.ItemTapped += LookUpListView_ItemTapped;

            gridMain.Children.Add(lookUpListView);
            Grid.SetRow(lookUpListView, 1);
        }

        private void SearchBar_TextChangedEventArgs(object sender, TextChangedEventArgs e)
        {
            var text = e.NewTextValue;
            if (string.IsNullOrWhiteSpace(text))
            {
                lookUpListView.ItemsSource = this.ItemsSource;
            }
            else
            {
                lookUpListView.ItemsSource = this.ItemsSource.Where(x => x.Label.ToString().ToLower().Contains(text.ToLower()));
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            ItemsSource.Where(x => x.Selected == true).ToList().ForEach(x => x.Selected = false);
            if (SelectedIds != null)
            {
                ItemsSource.Where(x => SelectedIds.Any(val => val == x.Val)).ToList().ForEach(x => x.Selected = true);
            }
            await CenterModal.Hide();
        }

        public async void SaveButton_Clicked(object sender, EventArgs e)
        {

            var checkedItems = ItemsSource.Where(x => x.Selected).ToList();
            if (checkedItems.Any())
            {
                string[] names = checkedItems.Select(x => x.Label).ToArray();
                SelectedIds = checkedItems.Select(x => x.Val).ToList();
                this.Text = string.Join(", ", names);
                SetList(checkedItems);
            }
            else
            {
                SelectedIds = null;
                this.Text = null;
                ClearFlexLayout();
            }

            await CenterModal.Hide();
            OnSave?.Invoke(this, EventArgs.Empty);
        }

        private void LookUpListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as OptionSet;
            item.Selected = !item.Selected;
        }

        private void Clear_Clicked(object sender, EventArgs e) => ClearData();

        public void ClearData()
        {
            if (ItemsSource == null || ItemsSource.Any() == false) return;
            this.Text = null;
            ItemsSource.ForEach(x => x.Selected = false);
            SelectedIds = null;
            OnDelete?.Invoke(this, EventArgs.Empty);
        }

        public void setData()
        {
            if (this.SelectedIds != null && this.SelectedIds.Any() && ItemsSource != null)
            {
                var selectedInSource = ItemsSource.Where(x => SelectedIds.Any(s => s == x.Val)).ToList();
                foreach (var item in selectedInSource)
                {
                    item.Selected = true;
                }
                this.Text = string.Join(", ", selectedInSource.Select(x => x.Label).ToArray());

                SetList(selectedInSource);
            }
            else
            {
                this.Text = null;
                ClearFlexLayout();
            }
        }
        public void SetList(List<OptionSet> selectedInSource)
        {
            this.Entry.IsVisible = false;
            this.flexLayout.IsVisible = true;
            selectedInSource.Add(new OptionSet()
            {
                Val = "0"
            });

            BindableLayout.SetItemsSource(flexLayout, selectedInSource);
            var last = flexLayout.Children.Last() as StackLayout;
            //var radBorder = flexLayout.Children.Last() as RadBorder;
            var radBorder = last.Children[0] as RadBorder;
            radBorder.BackgroundColor = Color.Gray;
            (radBorder.Content as Label).Text = "\uf00d";
            (radBorder.Content as Label).FontSize = Device.RuntimePlatform == Device.iOS ? 16 : 17;
            (radBorder.Content as Label).HorizontalTextAlignment = TextAlignment.Center;
            (radBorder.Content as Label).VerticalTextAlignment = TextAlignment.Center;
            (radBorder.Content as Label).FontFamily = "FontAwesomeSolid";
            (radBorder.Content as Label).TextColor = Color.White;

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += Clear_Clicked;

            radBorder.GestureRecognizers.Add(tap);
        }
        public void ClearFlexLayout()
        {
            flexLayout.IsVisible = false;
            Entry.IsVisible = true;
        }

        private static void ItemSourceChange(BindableObject bindable, object oldValue, object value)
        {
            LookUpMultipleOptions control = (LookUpMultipleOptions)bindable;
            control.setData();
        }
    }
}
