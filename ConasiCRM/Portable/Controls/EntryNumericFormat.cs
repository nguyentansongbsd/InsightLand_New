using System;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Controls
{
    /// <summary>
    ///     Entry numeric format.
    ///         Control that is used to format a numeric input (ex: 100000000 ==> 100.000.000)
    ///         Use this control, use can define MaxValue, Min Value, Negative or NonNegative of numeric input
    ///         Control extend Entry control of Xamarin.Forms, so it can be used like a normal entry
    ///         This control was created by Anh Phong
    /// </summary>
    public class EntryNumericFormat : MainEntry
    {
        public EntryNumericFormat()
        {
            Keyboard = Keyboard.Numeric;
            TextChanged += EntryNumericFormat_TextChanged;
            Unfocused += EntryNumericFormat_Unfocused;
        }

        public static readonly BindableProperty NumericTextProperty = BindableProperty.Create(nameof(NumericText), typeof(decimal?), typeof(EntryNumericFormat), null, BindingMode.TwoWay, propertyChanged: TextFormatPropertyChanged);
        public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(nameof(MaxValue), typeof(decimal?), typeof(EntryNumericFormat), null, BindingMode.TwoWay);
        public static readonly BindableProperty MinValueProperty = BindableProperty.Create(nameof(MinValue), typeof(decimal?), typeof(EntryNumericFormat), null, BindingMode.TwoWay);
        public static readonly BindableProperty IsNotNegativeProperty = BindableProperty.Create(nameof(IsNotNegative), typeof(bool), typeof(EntryNumericFormat), false, BindingMode.TwoWay);

        /// <summary>
        ///     NumericText is a bindable variable to binding numeric text. BindableProperty of this is NumericTextProperty
        /// </summary>
        /// <value>The numeric text.</value>
        public decimal? NumericText
        {
            get { return (decimal?)GetValue(NumericTextProperty); }
            set { SetValue(NumericTextProperty, value); this.renderFormat(value); }
        }
        /// <summary>
        ///     MaxValue is a bindable variable to binding max value of numeric input. BindableProperty of this is
        ///         MaxValueProperty.
        ///     If MaxValue is defined, the value was inputed in text entry that greater than MaxValue will be denied (Not 
        ///         show warning, only denied input).
        /// </summary>
        /// <value>The max value.</value>
        public decimal? MaxValue
        {
            get { return (decimal?)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
        /// <summary>
        ///     MinValue is a bindable variable to binding min value of numeric input. BindableProperty of this is
        ///         MinValueProperty.
        ///     If MinValue is defined, the value was inputed in text entry that less than MinValue will be denied (Not 
        ///         show warning, only denied input).
        /// </summary>
        /// <value>The minimum value.</value>
        public decimal? MinValue
        {
            get { return (decimal?)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        /// <summary>
        ///     IsNotNegative is a bindable variable to binding the value that define numeric input is nonNegative or not.
        ///         BindableProperty of this is IsNotNegativeProperty.
        ///     Default, it is fale, if it true, text input of entry cannot be negative numeric.
        /// </summary>
        /// <value><c>true</c> if is not negative; otherwise, <c>false</c>.</value>
        public bool IsNotNegative
        {
            get { return (bool)GetValue(IsNotNegativeProperty); }
            set { SetValue(IsNotNegativeProperty, value); }
        }

        /// <summary>
        ///     Listen property changed of NumericText to call renderFormat function
        /// </summary>
        /// <param name="bindable">Bindable.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void TextFormatPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (EntryNumericFormat)bindable;
            control.renderFormat((decimal?)newValue);
        }

        /// <summary>
        ///     RenderFormat function()
        ///         When value of TextFormat changed (numeric), text of entry will be rerendered.
        /// </summary>
        /// <param name="value">Value.</param>
        private void renderFormat(decimal? value)
        {
            if (value.HasValue) { this.Text = string.Format("{0:#,0.#}", value); }
        }


        /// <summary>
        ///     Entry Text changed ==> parse Text to numeric, if has exception, let numeric text to null.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void EntryNumericFormat_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    NumericText = null;
                }
                else
                {
                    decimal num = 0;
                    if (IsNotNegative)
                    {
                        num = decimal.Parse(e.NewTextValue.Replace(",", "").Replace(".", "").Replace("-", ""));
                    }
                    else
                    {
                        num = decimal.Parse(e.NewTextValue.Replace(",", "").Replace(".", ""));
                    }
                    if (MaxValue.HasValue)
                    {
                        if(num > MaxValue.Value) { num = NumericText.Value; }
                    }
                    if (MinValue.HasValue)
                    {
                        if(num < MinValue.Value) { num = NumericText.Value; }
                    }
                    if (num != 0)
                    {
                        NumericText = num;
                    }
                }
            }
            catch (Exception ex)
            {
                NumericText = null;
                if (!IsNotNegative)
                {
                    if (e.NewTextValue == "-") { this.Text = e.NewTextValue; }
                    else { this.Text = ""; }
                }
                else
                {
                    this.Text = "";
                }
            }


        }


        /// <summary>
        ///     Parse text to numeric to format after finish input
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void EntryNumericFormat_Unfocused(object sender, FocusEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Text))
                {
                    NumericText = null;
                }
                else
                {
                    decimal num = 0;
                    if (IsNotNegative)
                    {
                        num = decimal.Parse(Text.Replace(",", "").Replace(".", "").Replace("-", ""));
                    }
                    else
                    {
                        num = decimal.Parse(Text.Replace(",", "").Replace(".", ""));
                    }
                    if (MaxValue.HasValue)
                    {
                        if (num > MaxValue.Value) { num = NumericText.Value; }
                    }
                    if (MinValue.HasValue)
                    {
                        if (num < MinValue.Value) { num = NumericText.Value; }
                    }
                    NumericText = num;
                }
            }
            catch (Exception ex)
            {
                NumericText = null;
            }
        }    
    }
}
