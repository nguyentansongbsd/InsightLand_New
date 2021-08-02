using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms.Xaml;

namespace ConasiCRM.Portable.Controls
{
    public partial class MyNewEntry : ContentView
    {
        public MyNewEntry()
        {
            InitializeComponent();

            Text_entry.BindingContext = this;
            clear_entry_button.BindingContext = this;

            //MainContainer.GestureRecognizers.Add(new TapGestureRecognizer()
            //{
            //    Command = new Command(tapped)
            //});

            Text_entry.Focused += Text_Entry_Focused;

            clear_entry_button.Clicked += (senser, arg) => { this.SendClearClicked(); };
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(MyNewEntry), null, BindingMode.TwoWay);
        public static readonly BindableProperty PlaceHolderProperty = BindableProperty.Create(nameof(PlaceHolder), typeof(string), typeof(MyNewEntry), null, BindingMode.TwoWay);
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(Int32), typeof(MyNewEntry), 16, BindingMode.TwoWay);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(MyNewEntry), Color.Black, BindingMode.OneWay);
        public static readonly BindableProperty HasClearButtonProperty = BindableProperty.Create(nameof(HasClearButton), typeof(bool), typeof(MyNewEntry), true, BindingMode.OneWay);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string PlaceHolder
        {
            get { return (string)GetValue(PlaceHolderProperty); }
            set { SetValue(PlaceHolderProperty, value); }
        }

        public Int32 FontSize
        {
            get { return (Int32)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public bool HasClearButton
        {
            get { return (bool)GetValue(HasClearButtonProperty); }
            set { SetValue(HasClearButtonProperty, value); }
        }


        /// <summary>  
        /// Handle chackox tapped action  
        /// </summary>  
        void tapped()
        {
            this.SendOnClicked();
        }

        void Text_Entry_Focused(object sender, FocusEventArgs e)
        {
            Text_entry.Focus();
            Text_entry.Unfocus();
            this.SendOnClicked();
        }

        public event EventHandler OnClicked;
        public void SendOnClicked()
        {
            EventHandler eventHandler = this.OnClicked;
            eventHandler?.Invoke((object)this, EventArgs.Empty);
        }

        public event EventHandler ClearClicked;
        public void SendClearClicked()
        {
            EventHandler eventHandler = this.ClearClicked;
            eventHandler?.Invoke((object)this, EventArgs.Empty);
        }
    }
}
