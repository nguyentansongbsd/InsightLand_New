﻿using System;
using Android.Content;
using views= Android.Views;
using ConasiCRM.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ConasiCRM.Portable.Controls;

[assembly: ExportRenderer(typeof(LookUpEntry), typeof(LookUpEntryRenderer))]
namespace ConasiCRM.Droid.Renderers
{
    public class LookUpEntryRenderer : EntryRenderer
    {
        private LookUpEntry _lookUpEntry;
        public LookUpEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                _lookUpEntry = e.NewElement as LookUpEntry;
            }
            if (Control != null)
            {
                FormsEditText editText = Control;
                editText.SetBackgroundResource(Resource.Drawable.bg_main_entry);
                editText.SetPadding(20, 0, 15, 0);
                editText.SetFocusable(views.ViewFocusability.NotFocusable);
                editText.FocusableInTouchMode = false;
                editText.SetCursorVisible(false);
                //editText.Touch += EditText_Touch;
            }
        }

        //private void EditText_Touch(object sender, TouchEventArgs e)
        //{
        //    if (e.Event.Action == Android.Views.MotionEventActions.Up)
        //    {

        //    }
        //}
    }
}
