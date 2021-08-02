using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Controls
{
    class MyNewDatePicker : DatePicker
    {
        public const string NullableDatePropertyName = "NullableDate";


        public static readonly BindableProperty NullableDateProperty = BindableProperty.Create(nameof(NullableDate), typeof(DateTime?), typeof(MyNewDatePicker), null, BindingMode.TwoWay, propertyChanged: OnNullableDateChanged);

        static void OnNullableDateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (((DateTime?)newValue).HasValue)
            {
                (bindable as MyNewDatePicker).Date = ((DateTime?)newValue).Value;
                (bindable as MyNewDatePicker).Format = "dd/MM/yyyy";
            }
            else
            {
                (bindable as MyNewDatePicker).Format = "\n\n";
            }
        }


        /// <summary>
        /// Datumswert welches null Werte akzeptiert
        /// </summary>
        //public DateTime? NullableDate
        //{
        //    get
        //    {
        //        return (DateTime?)this.GetValue(NullableDateProperty);
        //    }
        //    set
        //    {
        //        this.SetValue(NullableDateProperty, value);
        //    }
        //}
        public DateTime? NullableDate
        {
            get { return (DateTime?)GetValue(NullableDateProperty); }
            set
            {
                SetValue(NullableDateProperty, value);
                //if (value != null)
                //{
                //    Date = ((DateTime?)GetValue(NullableDateProperty)).Value;
                //    this.Format = "dd/MM/yyyy";
                //}
                //else
                //{
                //    this.Format = "\n\n";
                //}
            }
        }
        /// <summary>
        /// Der Name der <c>NullText</c> Property
        /// </summary>
        public const string NullTextPropertyName = "NullText";
        /// <summary>
        /// Die BindableProperty
        /// </summary>
        public static readonly BindableProperty NullTextProperty = BindableProperty.Create<MyNewDatePicker, string>(i => i.NullText, default(string), BindingMode.TwoWay);
        /// <summary>
        /// Der Text der angezeigt wird wenn <c>NullableDate</c> keinen Wert hat
        /// </summary>
        public string NullText
        {
            get
            {
                return (string)this.GetValue(NullTextProperty);
            }
            set
            {
                this.SetValue(NullTextProperty, value);
            }
        }
        /// <summary>
        /// Der Name der <c>DisplayBorder</c> Property
        /// </summary>
        public const string DisplayBorderPropertyName = "DisplayBorder";
        /// <summary>
        /// Die BindableProperty
        /// </summary>
        public static readonly BindableProperty DisplayBorderProperty = BindableProperty.Create<MyNewDatePicker, bool>(i => i.DisplayBorder, default(bool), BindingMode.TwoWay);
        /// <summary>
        /// Gibt an ob eine Umrandung angezeigt werden soll oder nicht
        /// </summary>
        public bool DisplayBorder
        {
            get
            {
                return (bool)this.GetValue(DisplayBorderProperty);
            }
            set
            {
                this.SetValue(DisplayBorderProperty, value);
            }
        }

        /// <summary>
        /// Erstellt eine neue Instanz von <c>CustomDatePicker</c>
        /// </summary>
        public MyNewDatePicker()
        {
            this.DateSelected += CustomDatePicker_DateSelected;

            if (this.NullableDate.HasValue)
            {
                this.Format = "dd/MM/yyyy";
            }
            else
            {
                this.Format = " \n\n";
            }

            this.FontSize = 15;
        }
        /// <summary>
        /// Wird gefeuert wenn ein neues Datum selektiert wurde
        /// </summary>
        /// <param name="sender">Der Sender</param>
        /// <param name="e">Event Argumente</param>
        void CustomDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            this.Format = "dd/MM/yyyy";
            this.NullableDate = new DateTime(
            e.NewDate.Year,
            e.NewDate.Month,
            e.NewDate.Day,
            this.NullableDate.HasValue ? this.NullableDate.Value.Hour : 0,
            this.NullableDate.HasValue ? this.NullableDate.Value.Minute : 0,
            this.NullableDate.HasValue ? this.NullableDate.Value.Second : 0);
            this.SendDateChanged();
        }
        /// <summary>
        /// Gefeuert wenn sich <c>NullableDate</c> ändert
        /// </summary>
        /// <param name="obj">Der Sender</param>
        /// <param name="oldValue">Der alte Wert</param>
        /// <param name="newValue">Der neue Wert</param>
        private static void NullableDateChanged(BindableObject obj, DateTime? oldValue, DateTime? newValue)
        {
            var customDatePicker = obj as MyNewDatePicker;

            if (customDatePicker != null)
            {
                if (newValue.HasValue)
                {
                    customDatePicker.Date = newValue.Value;
                }
                else
                {
                    customDatePicker.Format = "\n\n";
                }
            }
        }

        public event EventHandler DateChanged;
        public void SendDateChanged()
        {
            EventHandler eventHandler = this.DateChanged;
            eventHandler?.Invoke((object)this, EventArgs.Empty);
        }
    }
}
