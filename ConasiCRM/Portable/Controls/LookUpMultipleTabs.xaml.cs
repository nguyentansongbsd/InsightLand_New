using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Extended;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LookUpMultipleTabs : Grid
    {
        public Func<Task> PreOpenAsync;
        public Action PreOpen;

        public event EventHandler<LookUpChangeEvent> SelectedItemChange;

        public static readonly BindableProperty ListListViewProperty = BindableProperty.Create(nameof(ListListView), typeof(List<ObservableCollection<OptionSet>>), typeof(LookUpMultipleTabs), null, BindingMode.TwoWay);
        public List<ObservableCollection<OptionSet>> ListListView { get => ( List<ObservableCollection<OptionSet>>)GetValue(ListListViewProperty); set { SetValue(ListListViewProperty, value); } }              

        public static readonly BindableProperty TabsProperty = BindableProperty.Create(nameof(Tabs), typeof(List<FloatButtonItem>), typeof(LookUpMultipleTabs), null, BindingMode.TwoWay);
        public List<FloatButtonItem> Tabs { get => (List<FloatButtonItem>)GetValue(TabsProperty); set { SetValue(TabsProperty, value); } }

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(LookUpMultipleTabs), null, BindingMode.TwoWay);
        public object SelectedItem { get => (object)GetValue(SelectedItemProperty); set { SetValue(SelectedItemProperty, value); } }

        public static readonly BindableProperty NameDipslayProperty = BindableProperty.Create(nameof(SelectedItem), typeof(string), typeof(LookUpMultipleTabs), null, BindingMode.TwoWay, propertyChanged: DisplayNameChang);
        public string NameDisplay { get => (string)GetValue(NameDipslayProperty); set { SetValue(NameDipslayProperty, value); } }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(LookUpMultipleTabs), null, BindingMode.TwoWay);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }

        private LookUpView _lookUpView;
        public CenterModal CenterModal { get; set; }

        public bool FocusSearchBarOnTap = false;

        private Grid gridMain;
        public bool PreOpenOneTime { get; set; } = true;
        private List<RadBorder> ListRadBorderTab { get; set; }
        private List<Label> ListLabelTab { get; set; }
        private int indexTab { get; set; }

        private StackLayout stackFooter = new StackLayout();
        private OptionSet oldItem { get; set; }
        public LookUpMultipleTabs()
        {
            InitializeComponent();
            this.Entry.BindingContext = this;
            this.Entry.SetBinding(EntryNoneBorder.PlaceholderProperty, "Placeholder");
            this.BtnClear.SetBinding(Button.IsVisibleProperty, new Binding("SelectedItem") { Source = this, Converter = new Converters.NullToHideConverter() });
        }

        public void Clear_Clicked(object sender, EventArgs e)
        {
            this.SelectedItem = null;
            SelectedItemChange?.Invoke(this, new LookUpChangeEvent());
        }
        public void HideClearButton()
        {
            BtnClear.IsVisible = false;
        }
        public async void OpenLookUp_Tapped(object sender, EventArgs e)
        {
            await OpenModal();
        }

        private static void DisplayNameChang(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;
            LookUpMultipleTabs control = (LookUpMultipleTabs)bindable;
            control.Entry.SetBinding(EntryNoneBorder.TextProperty, "SelectedItem." + newValue);
        }
        public async Task OpenModal()
        {
            if (PreOpenAsync != null)
            {
                await PreOpenAsync();
                if (PreOpenOneTime)
                {
                    PreOpenAsync = null;
                }
            }
            if (PreOpen != null)
            {
                PreOpen.Invoke();
                if (PreOpenOneTime)
                {
                    PreOpen = null;
                }
            }

            if (this.ListListView == null || this.ListListView.Count == 0 || this.CenterModal == null || Tabs == null) return;

            if (_lookUpView == null)
            {
                _lookUpView = new LookUpView();
                 _lookUpView.SetList(ListListView[0].Cast<object>().ToList(), NameDisplay);
                _lookUpView.lookUpListView.ItemTapped += async (lookUpSender, lookUpTapEvent) =>
                {
                    if (this.SelectedItem != lookUpTapEvent.Item)
                    {
                        this.SelectedItem = lookUpTapEvent.Item;
                        SelectedItemChange?.Invoke(this, new LookUpChangeEvent());
                    }
                    await CenterModal.Hide();
                };

                _lookUpView.lookUpListView.ItemAppearing += LookUpListView_ItemAppearing;
                if (Device.RuntimePlatform == Device.Android)
                {
                    SetUpFooterLayout();
                }


                Grid tabs = SetUpTabs();
                gridMain = new Grid();
                gridMain.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                gridMain.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                Grid.SetRow(tabs, 0);
                gridMain.Children.Add(tabs);

                gridMain.Children.Add(_lookUpView);
                Grid.SetRow(_lookUpView, 1);
                indexTab = 0;
                IndexTab(indexTab);
            }
            else
            {
                _lookUpView.SetList(ListListView[indexTab].Cast<object>().ToList(), NameDisplay);
            }

            CenterModal.Title = Placeholder;
            CenterModal.Body = gridMain;
            await CenterModal.Show();

            if (FocusSearchBarOnTap)
            {
                _lookUpView.FocusSearchBarOnTap();
            }
        }

        private void LookUpListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if(e.Item != null && e.Item != oldItem)
            {
                if(e.ItemIndex == ListListView[indexTab].Count-1)
                {
                    oldItem = e.Item as OptionSet;
                    stackFooter.IsVisible = true;
                    Tabs[indexTab].OnClickeEvent?.Invoke(this, EventArgs.Empty);
                    stackFooter.IsVisible = false;
                    _lookUpView.SetList(ListListView[indexTab].Cast<object>().ToList(), NameDisplay);
                }
            }
        }

        public Grid SetUpTabs()
        {
            Grid grid = new Grid();
            if (Tabs != null)
            {
                ListLabelTab = new List<Label>();
                ListRadBorderTab = new List<RadBorder>();
                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                for (int i = 0; i < Tabs.Count; i++)
                {
                    RadBorder rd = new RadBorder();
                    rd.Style = (Style)Application.Current.Resources["rabBorder_Tab"];
                    Label lb = new Label();
                    lb.Style = (Style)Application.Current.Resources["Lb_Tab"];
                    lb.Text = Tabs[i].Text;
                    rd.Content = lb;
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += Tab_Tapped;
                    lb.GestureRecognizers.Add(tapGestureRecognizer);
                    ListLabelTab.Add(lb);
                    ListRadBorderTab.Add(rd);

                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                    grid.Children.Add(rd);
                    Grid.SetColumn(rd, i);
                    Grid.SetRow(rd, 0);
                }
                BoxView boxView = new BoxView();
                boxView.HeightRequest = 1;
                boxView.BackgroundColor = Color.FromHex("F1F1F1");
                boxView.VerticalOptions = LayoutOptions.EndAndExpand;
                grid.Children.Add(boxView);
                Grid.SetColumn(boxView, 0);
                Grid.SetRow(boxView, 0);
                Grid.SetColumnSpan(boxView, Tabs.Count);
            }
            return grid;
        }
        private void Tab_Tapped(object sender, EventArgs e)
        {
            var button = sender as Label;
            indexTab = ListRadBorderTab.IndexOf(ListRadBorderTab.FirstOrDefault(x => x.Children.Last() == button));
            IndexTab(indexTab);
        }

        private void IndexTab(int index)
        {
            if (ListRadBorderTab != null && ListRadBorderTab.Count > 0)
            {
                for (int i = 0; i < ListRadBorderTab.Count; i++)
                {
                    if (i == index)
                    {
                        VisualStateManager.GoToState(ListRadBorderTab[i], "Selected");
                        VisualStateManager.GoToState(ListLabelTab[i], "Selected");
                        if (ListListView != null && ListListView.Count > 0 )
                        {
                            if (ListListView[indexTab] != null && ListListView[indexTab].Count <= 0)
                            {
                                LoadingHelper.Show();
                                Tabs[indexTab].OnClickeEvent?.Invoke(this, EventArgs.Empty);
                                LoadingHelper.Hide();
                            }
                            _lookUpView.SetList(ListListView[indexTab].Cast<object>().ToList(), NameDisplay);
                        }
                    }
                    else
                    {
                        VisualStateManager.GoToState(ListRadBorderTab[i], "Normal");
                        VisualStateManager.GoToState(ListLabelTab[i], "Normal");
                    }
                }
            }
        }

        public void SetUpFooterLayout()
        {
            stackFooter.Padding = new Thickness(0, 0, 0, 64);

            ActivityIndicator activityIndicator = new ActivityIndicator()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                WidthRequest = 30,
                IsRunning = true,
                Margin = new Thickness(0, 5, 0, 0),
                Color = Color.FromHex("#333333")
            };

            stackFooter.Children.Add(activityIndicator);

            _lookUpView.lookUpListView.Footer = stackFooter;
        }   
    }
}