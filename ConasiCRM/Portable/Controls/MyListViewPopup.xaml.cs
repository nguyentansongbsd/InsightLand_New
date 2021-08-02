using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ConasiCRM.Portable.Models;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Controls
{

    /// <summary>
    ///     MyListViewPopup is a control that use to content popup with ListView inside, 
    ///     That popup was used for Lookup field, Multiselec field, ...
    ///     This control extend PopupPage of Rg.Plugins.Popups Nuget
    ///     This control was created by AnhPhong
    /// </summary>

    public partial class MyListViewPopup : PopupPage ///// PopupPage Model is Template of Rg.Plugins.Popups Nuget that use to content popup
    {
        /// <summary>
        ///     PlaceHolder is Title of popup
        /// </summary>
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(MyListViewPopup), null, BindingMode.Default);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }

        /// <summary>
        ///     ItemsSource of ListView in Popup
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(MyListViewPopup), null, BindingMode.Default);
        public IEnumerable ItemsSource { get => (IEnumerable)GetValue(ItemsSourceProperty); set { SetValue(ItemsSourceProperty, value); } }

        public MyListViewPopup(IEnumerable ItemSource, string Title)
        {
            InitializeComponent();

            listView.ItemsSource = ItemSource;
            title.Text = Title;
        }

        public MyListViewPopup()
        {
            InitializeComponent();

            listView.BindingContext = this;
            title.BindingContext = this;
        }


        /// <summary>
        ///     Item_Tapped Event of listview of popup
        /// 
        ///         In this envetn, we send a custom event Item_Tapped 
        ///         with 2 parameter (object)sender and (ItemTappedEventArgs)e
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Item_Tapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            this.sendOnTappedItem(sender, e);

            OptionSet item = e.Item as OptionSet;
            item.Selected = !item.Selected;
        }
        public event EventHandler<Xamarin.Forms.ItemTappedEventArgs> OnTappedItem;
        private void sendOnTappedItem(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            EventHandler< Xamarin.Forms.ItemTappedEventArgs> eventHandler = this.OnTappedItem;
            eventHandler?.Invoke(sender, e);
        }


        /// <summary>
        ///     Confirm_Clicked Event of button Confirm of Popup
        /// 
        ///         In this envetn, we send a custom event OnConfirmClciked 
        ///         with 2 parameter (object)sender and (Empty EventArgs)e
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Confirm_Clicked(object sender, System.EventArgs e)
        {
            this.sendOnConfirmClicked();
        }
        public event EventHandler OnConfirmClicked;
        private void sendOnConfirmClicked()
        {
            EventHandler eventHandler = this.OnConfirmClicked;
            eventHandler?.Invoke((object)this, EventArgs.Empty);
        }


        /// <summary>
        ///     Close_clicked Event of Button Close on top-right of popup
        /// 
        ///         In this envetn, we send a custom event OnCloseClicked 
        ///         with 2 parameter (object)sender and (Empty EventArgs)e
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        async void Close_clicked(object sender, System.EventArgs e)
        {
            this.sendOnCloseClicked();
            await PopupNavigation.Instance.PopAsync();
        }
        public event EventHandler OnCloseClicked;
        private void sendOnCloseClicked()
        {
            EventHandler eventHandler = this.OnCloseClicked;
            eventHandler?.Invoke((object)this, EventArgs.Empty);
        }
    }
}
