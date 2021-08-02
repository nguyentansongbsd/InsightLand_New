using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Telerik.XamarinForms.Primitives.CheckBox;
using Xamarin.Forms.Xaml;


namespace ConasiCRM.Portable.Controls
{
    public partial class MyNewCheckBox : ContentView
    {
        public MyNewCheckBox()
        {
            InitializeComponent();

            MainContainer.BindingContext = this;
            checkBox.BindingContext = this;
            title.BindingContext = this;

            MainContainer.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(tapped)
            });

        }

        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(MyNewCheckBox), false, BindingMode.TwoWay, propertyChanged: checkedPropertyChanged);
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(MyNewCheckBox), "", BindingMode.OneWay);
        public static readonly BindableProperty CheckedChangedCommandProperty = BindableProperty.Create(nameof(CheckedChangedCommand), typeof(Command), typeof(MyNewCheckBox), null, BindingMode.OneWay);
        public static readonly BindableProperty LabelStyleProperty = BindableProperty.Create(nameof(LabelStyle), typeof(Style), typeof(MyNewCheckBox), null, BindingMode.OneWay);
        public static readonly BindableProperty UncheckedColorProperty = BindableProperty.Create(nameof(UncheckedColor), typeof(Color), typeof(MyNewCheckBox), Color.Black, BindingMode.OneWay);
        public static readonly BindableProperty CheckedColorProperty = BindableProperty.Create(nameof(CheckedColor), typeof(Color), typeof(MyNewCheckBox), Color.DarkSlateGray, BindingMode.OneWay);
        public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor), typeof(Color), typeof(MyNewCheckBox), Color.Black, BindingMode.OneWay);
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create(nameof(Spacing), typeof(double), typeof(MyNewCheckBox), 1.0, BindingMode.OneWay);

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public Command CheckedChangedCommand
        {
            get { return (Command)GetValue(CheckedChangedCommandProperty); }
            set { SetValue(CheckedChangedCommandProperty, value); }
        }

        public Style LabelStyle
        {
            get { return (Style)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        public Label ControlLabel
        {
            get { return title; }
        }

        public Color UncheckedColor
        {
            get { return (Color)GetValue(UncheckedColorProperty); }
            set { SetValue(UncheckedColorProperty, value); }
        }

        public Color CheckedColor
        {
            get { return (Color)GetValue(CheckedColorProperty); }
            set { SetValue(CheckedColorProperty, value); }
        }
        
        public Color TitleColor
        {
            get { return (Color)GetValue(TitleColorProperty); }
            set { SetValue(TitleColorProperty, value); }
        }

        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        static void checkedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((MyNewCheckBox)bindable).ApplyCheckedState();

        }

        /// <summary>  
        /// Handle chackox tapped action  
        /// </summary>  
        void tapped()
        {
            IsChecked = !IsChecked;
            ApplyCheckedState();
            this.SendChangeChecked();
        }

        /// <summary>  
        /// Reflect the checked event change on the UI  
        /// with a small animation  
        /// </summary>  
        /// <param name="isChecked"></param>  
        ///   
        void ApplyCheckedState()
        {
            checkBox.IsChecked = IsChecked;
        }

        public event EventHandler changeChecked;
        public void SendChangeChecked()
        {
            EventHandler eventHandler = this.changeChecked;
            eventHandler?.Invoke((object)this, EventArgs.Empty);
        }
    }
}
