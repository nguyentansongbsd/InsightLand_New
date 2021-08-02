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
    public partial class BsdModal : ContentView
    {

        public static readonly BindableProperty HeaderTextProperty = BindableProperty.Create(nameof(HeaderText), typeof(string), typeof(BsdModal), null, BindingMode.TwoWay);
        public static readonly BindableProperty IsLoadingProperty = BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(BsdModal), false, BindingMode.TwoWay);
        public static readonly BindableProperty IsShowProperty = BindableProperty.Create(nameof(IsShow), typeof(bool), typeof(BsdModal), false, BindingMode.TwoWay, propertyChanged: IsShow_PropertyChanged);
        public static readonly BindableProperty ModalContentProperty = BindableProperty.Create(nameof(ModalContent), typeof(object), typeof(BsdModal), null, BindingMode.TwoWay, propertyChanged: Content_PropertyChanged);

        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        public bool IsShow
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public object ModalContent
        {
            get
            {
                var type = ModalContentProperty.GetType();
                return (object)GetValue(ModalContentProperty);
            }
            set
            {
                var type = ModalContentProperty.GetType();
                SetValue(ModalContentProperty, value);
            }
        }

        public BsdModal()
        {
            InitializeComponent();
            this.BindingContext = this;
            this.BackgroundColor = Color.FromRgba(0, 0, 0, 0.6);
        }

        private void BtnCloseModal_Clicked(object sender, EventArgs e)
        {
            this.IsVisible = false;
        }

        public static void Content_PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            BsdModal control = (BsdModal)bindable;

            control.mainStackLayout.Children.Add((StackLayout)newValue);
        }

        public static void IsShow_PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            BsdModal control = (BsdModal)bindable;

            if (newValue != null)
            {
                control.IsVisible = (bool)newValue;
            }
            else
            {
                control.IsVisible = false;
            }
        }
    }
}