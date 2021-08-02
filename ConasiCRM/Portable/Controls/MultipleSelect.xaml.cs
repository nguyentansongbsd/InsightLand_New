using ConasiCRM.Portable.Converters;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MultipleSelect : ContentView
    {
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(MultipleSelect), null, BindingMode.TwoWay);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }

        public static readonly BindableProperty ModalContentViewProperty = BindableProperty.Create(nameof(ModalContentView), typeof(ContentView), typeof(MultipleSelect), null, BindingMode.TwoWay);
        public ContentView ModalContentView { get => (ContentView)GetValue(ModalContentViewProperty); set => SetValue(ModalContentViewProperty, value); }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(ObservableCollection<OptionSet>), typeof(MultipleSelect), null, BindingMode.TwoWay, null);
        public ObservableCollection<OptionSet> ItemsSource { get => (ObservableCollection<OptionSet>)GetValue(ItemsSourceProperty); set { SetValue(ItemsSourceProperty, value); setValueMultiEntry(); } }

        public static readonly BindableProperty SelectedItemsProperty = BindableProperty.Create(nameof(SelectedItems), typeof(ObservableCollection<string>), typeof(MultipleSelect), null, BindingMode.Default);
        public ObservableCollection<string> SelectedItems { get => (ObservableCollection<string>)GetValue(SelectedItemsProperty); set => SetValue(SelectedItemsProperty, value); }

        public static readonly BindableProperty StackListProperty = BindableProperty.Create(nameof(StackList), typeof(ObservableCollection<StackLayout>), typeof(MultipleSelect), null, BindingMode.TwoWay);
        public ObservableCollection<StackLayout> StackList { get => (ObservableCollection<StackLayout>)GetValue(SelectedItemsProperty); set => SetValue(SelectedItemsProperty, value); }

        //public ListView listView;

        //public event EventHandler<OnCloseEventArgs> OnClose;

        /// <summary>
        /// listviewPopup is control that show popup has listview multiselect when focus entry
        ///     
        /// </summary>
        private MyListViewPopup listviewPopup;

        public MultipleSelect()
        {
            InitializeComponent();           
            this.BindingContext = this;
            this.SelectedItems = new ObservableCollection<string>();
            entry.SetBinding(Entry.PlaceholderProperty, new Binding("Placeholder") { Source = this });
            //entry.SetBinding(Entry.TextProperty, new Binding("SelectedItems")
            //{
            //    Source = this,
            //    Converter = new ItemsSelectedConverter()
            //});

            listviewPopup = new MyListViewPopup();

            listviewPopup.OnConfirmClicked += ListviewPopup_OnConfirmClicked;
            listviewPopup.OnCloseClicked += ListviewPopup_OnCloseClicked;
            listviewPopup.OnTappedItem += ListviewPopup_OnTappedItem;          
        }

        void ListviewPopup_OnConfirmClicked(object sender, EventArgs e)
        {
            ItemsSource = (ObservableCollection<OptionSet>)listviewPopup.ItemsSource;
            PopupNavigation.Instance.PopAsync();
        }

        void ListviewPopup_OnCloseClicked(object sender, EventArgs e)
        {
            ////// using OnCloseClicked default of MyListViewPopup
        }

        void ListviewPopup_OnTappedItem(object sender, ItemTappedEventArgs e)
        {
        }
        /// <summary>
        ///             Show Mutil Selected Item
        /// </summary>
        private void setValueMultiEntry()
        {
            FlexLayout.Children.Clear();
            SelectedItems.Clear();
            foreach(var i in ItemsSource.Where(x => x.Selected))
            {
                entry.IsVisible = false;
                SelectedItems.Add(i.Val);
                StackLayout stack = new StackLayout() { Padding = 1, BackgroundColor = Color.Transparent };
                Frame frame = new Frame() { BackgroundColor = Color.DarkBlue, Padding = 5, CornerRadius = 5, HasShadow = false};
                Label label = new Label() { Text = i.Label, TextColor = Color.White };
                stack.Children.Add(frame);
                frame.Content = label;
                FlexLayout.Children.Add(stack);
            }
            if(SelectedItems.Count == 0) { entry.IsVisible = true; }
        }


        //public void CloseModal(object sender, EventArgs e)
        //{
        //    entry.Unfocus();
        //    ModalContentView.IsVisible = false;
        //    if (OnClose != null)
        //    {
        //        OnClose.Invoke(this, new OnCloseEventArgs()
        //        {
        //            Item = "noi dung cua tham so tra ve"
        //        });
        //    }
        //}

        //public void Confirm(object sender, EventArgs e)
        //{
        //    this.SelectedItems.Clear();
        //    this.SelectedItems = new ObservableCollection<OptionSet>(this.ItemsSource.Where(x => x.Selected));
        //    ModalContentView.IsVisible = false;
        //}

        //public void Tap(object sender, EventArgs e)
        //{
        //    StackLayout stackLayout = (StackLayout)sender;
        //    TapGestureRecognizer tapGes = stackLayout.GestureRecognizers[0] as TapGestureRecognizer;
        //    OptionSet item = tapGes.CommandParameter as OptionSet;
        //    item.Selected = !item.Selected;
        //}

        private async void Entry_Focus(object sender, EventArgs e)
        {
            entry.Unfocus();
            LoadingHelper.Show();
            /////// ****** ANH PHONG comment ************
            /// 
            /////// using tmp to copy ONLY VALUE of itemsource into listviewPopup ItemsSource
            ///  If listviewPopup is closed, nothing happen
            ///  If listviewPopup is confirmed, ItemsSource of this will be modified
            ObservableCollection<OptionSet> tmp = new ObservableCollection<OptionSet>();
            foreach(var i in ItemsSource)
            {
                tmp.Add(new OptionSet() { Label = i.Label, Selected = i.Selected, Title = i.Title, Val = i.Val});
            }
            listviewPopup.ItemsSource = tmp;
            listviewPopup.Placeholder = Placeholder;
            await PopupNavigation.Instance.PushAsync(listviewPopup);
            LoadingHelper.Hide();
            //using Rg.Plugins.Popup
            //ModalContentView.IsVisible = true;
            ////stack.IsVisible = true;

            //if (true)
            //{
            //    ModalContentView.BackgroundColor = Color.FromRgba(0, 0, 0, 0.6);
            //    var stackLayout = new StackLayout();
            //    ModalContentView.Content = stackLayout;

            //    stackLayout.BackgroundColor = Color.White;
            //    stackLayout.Children.Add(GetHeader());
            //    stackLayout.Children.Add(GetListView());

            //    Button btnConfirm = new Button()
            //    {
            //        VerticalOptions = LayoutOptions.End,
            //        Text = "Xác nhận",
            //    };
            //    btnConfirm.Clicked += Confirm;
            //    stackLayout.Children.Add(btnConfirm);

            //    first = false;
            //}
        }

        //public StackLayout GetHeader()
        //{
        //    var stackLayout = new StackLayout();
        //    stackLayout.Orientation = StackOrientation.Horizontal;
        //    stackLayout.Padding = 10;
        //    stackLayout.BackgroundColor = Color.FromHex("#BFC5C6");

        //    Label titleLabel = new Label();
        //    titleLabel.SetBinding(Label.TextProperty, new Binding("Placeholder") { Source = this });
        //    titleLabel.HorizontalOptions = LayoutOptions.StartAndExpand;
        //    titleLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
        //    titleLabel.FontSize = 18;
        //    stackLayout.Children.Add(titleLabel);

        //    var closeBtn = new ImageButton()
        //    {
        //        BackgroundColor = Color.White,
        //        Source = "close.png",
        //        Padding = 5,
        //        Aspect = Aspect.AspectFit,
        //        HeightRequest = 32,
        //        WidthRequest = 32
        //    };
        //    closeBtn.Clicked += CloseModal;
        //    stackLayout.Children.Add(closeBtn);

        //    return stackLayout;
        //}

        //public StackLayout GetListView()
        //{
        //    var stackLayout = new StackLayout()
        //    {
        //        Padding = new Thickness(10, 0, 10, 0)
        //    };


        //    listView = new ListView(cachingStrategy: ListViewCachingStrategy.RecycleElement)
        //    {
        //        HasUnevenRows = true,
        //    };

        //    listView.SetBinding(ListView.ItemsSourceProperty, new Binding("ItemsSource")
        //    {
        //        Source = this
        //    });


        //    listView.ItemTemplate = new DataTemplate(() =>
        //     {
        //         var itemLayout = new StackLayout()
        //         {
        //             Padding = new Thickness(10, 10, 10, 10),
        //             BackgroundColor = Color.White,
        //             Orientation = StackOrientation.Horizontal,
        //             VerticalOptions = LayoutOptions.Center
        //         };

        //         //var tapGes = new TapGestureRecognizer()
        //         //{
        //         //    NumberOfTapsRequired = 1,
        //         //};
        //         //tapGes.Tapped += Tap;
        //         //tapGes.SetBinding(TapGestureRecognizer.CommandParameterProperty, new Binding("."));
        //         //itemLayout.GestureRecognizers.Add(tapGes);


        //         var itemLabel = new Label()
        //         {
        //             FontSize = 17,
        //             TextColor = Color.FromHex("#333333"),
        //             HorizontalOptions = LayoutOptions.StartAndExpand
        //         };
        //         itemLabel.SetBinding(Label.TextProperty, new Binding("Label"));
        //         itemLayout.Children.Add(itemLabel);

        //         RadCheckBox checkbox = new RadCheckBox()
        //         {
        //             HorizontalOptions = LayoutOptions.End,
        //             //BackgroundColor = Color.FromHex("#eeeeee"),
        //             CheckedColor = Color.FromHex("#666666"),

        //         };
        //         checkbox.SetBinding(RadCheckBox.IsCheckedProperty, new Binding("Selected"));
        //         itemLayout.Children.Add(checkbox);

        //         return new ViewCell { View = itemLayout };
        //     });

        //    listView.ItemTapped += (sender, e) =>
        //    {
        //        if (e.Item != null)
        //        {
        //            OptionSet item = (OptionSet)e.Item;
        //            item.Selected = !item.Selected;
        //            listView.SelectedItem = null;
        //        }
        //    };
        //    stackLayout.Children.Add(listView);
        //    return stackLayout;
        //}

       

        public void addSelectedItem(string ItemId)        {            var tmplst = new ObservableCollection<OptionSet>();            foreach (var i in ItemsSource)            {                if (i.Val == ItemId)                {                    i.Selected = true;                }                tmplst.Add(i);            }            this.ItemsSource = tmplst;        }        public void removeSelectedItem(string ItemId)        {            var tmplst = new ObservableCollection<OptionSet>();            foreach (var i in ItemsSource)            {                if (i.Val == ItemId)                {                    i.Selected = false;                }                tmplst.Add(i);            }            this.ItemsSource = tmplst;        }

        public void Clear()
        {
            FlexLayout.Children.Clear();     
            foreach(var i in ItemsSource)
            {
                i.Selected = false;
            }
            SelectedItems.Clear();
            entry.IsVisible = true;           
        }
    }

    public class OnCloseEventArgs : EventArgs
    {
        public object Item { get; set; }
    }


    public class ItemsSelectedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //return string.Join(", ", ((ObservableCollection<OptionSet>)value).Where(x => x.Selected).Select(x => x.Label).ToArray());
                return "df";
            }
            catch
            {
                return "ahihi";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "la sao";
        }
    }
}