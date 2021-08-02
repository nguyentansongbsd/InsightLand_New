using ConasiCRM.Portable.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BsdLookUp : ContentView
    {
        #region Select Item
        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem),
                typeof(Models.LookUp),
                typeof(BsdLookUp),
                null,
                BindingMode.TwoWay);
        public Models.LookUp SelectedItem { get => (Models.LookUp)GetValue(SelectedItemProperty); set => SetValue(SelectedItemProperty, value); }
        #endregion

        #region Place Holder
        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create(nameof(Placeholder),
                typeof(string),
                typeof(BsdLookUp),
                null,
                BindingMode.TwoWay);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }
        #endregion

        #region IsEnable
        public static readonly BindableProperty IsEnableProperty =
            BindableProperty.Create(nameof(IsEnable),
                typeof(bool),
                typeof(BsdLookUp),
                true,
                BindingMode.TwoWay, propertyChanged: OnIsEnableChanged);
        public bool IsEnable { get => (bool)GetValue(IsEnableProperty); set => SetValue(IsEnableProperty, value); }
        #endregion

        public static readonly BindableProperty HasClearButtonProperty = 
            BindableProperty.Create(nameof(HasClearButton),
            typeof(bool),
            typeof(BsdLookUp),
            true,
            BindingMode.Default,propertyChanged: OnHasClearButtonChanged);
        public Boolean HasClearButton { get => (bool)GetValue(HasClearButtonProperty); set => SetValue(HasClearButtonProperty, value); }

        public BsdLookUp()
        {
            InitializeComponent();
            entry.SetBinding(Entry.TextProperty, new Binding("SelectedItem.Name") { Source = this });
            entry.SetBinding(Entry.PlaceholderProperty, new Binding("Placeholder") { Source = this });
            btnClear.SetBinding(Button.IsVisibleProperty, new Binding("SelectedItem") { Source = this, Converter = new ObjToBoolConverter() });
        }

        public event EventHandler OpenClicked;

        private void ClearLookUp_Clicked(object sender, EventArgs e)
        {
            this.SelectedItem = null;
        }

        private void OnTapped(object sender, EventArgs e)
        {
            if (!IsEnable) return;
            OpenClicked.Invoke(this, EventArgs.Empty);
        }


        static void OnIsEnableChanged(BindableObject bindable, object oldValue, object newValue)
        {
            BsdLookUp bsdLookUp = (BsdLookUp)bindable;
            if (bsdLookUp != null)
            {
                if (bsdLookUp.IsEnable)
                {
                    bsdLookUp.btnClear.SetBinding(Button.IsVisibleProperty, new Binding("SelectedItem") { Source = bsdLookUp, Converter = new ObjToBoolConverter() });
                }
                else
                {
                    //bsdLookUp.btnClear.SetBinding(Button.IsVisibleProperty, new Binding("SelectedItem") { Source = bsdLookUp, Converter = new ObjToBoolConverter() });
                    bsdLookUp.btnClear.IsVisible = false;
                }
            }
        }

        static void OnHasClearButtonChanged(BindableObject bindable, object oldValue, object newValue)
        {
            BsdLookUp bsdLookUp = (BsdLookUp)bindable;
            if ((bool)newValue)
            {
                bsdLookUp.btnClear.SetBinding(Button.IsVisibleProperty,new Binding("SelectedItem") { Source = bsdLookUp, Converter = new ObjToBoolConverter() });
            }
            else {
                bsdLookUp.btnClear.SetBinding(Button.IsVisibleProperty, new Binding("HasClearButton") { Source = bsdLookUp } );
            }
        }
    }
}