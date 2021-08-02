using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Android.Content;
using Android.Database;
using Android.Provider;
using ConasiCRM.Android;
using ConasiCRM.Portable.Controls;
using ConasiCRM.Portable.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(AccessService))]
namespace ConasiCRM.Android
{
    public class AccessService : IAccessService
    {
        public ObservableCollection<CallLogModel> GetCallLogs()
        {
            var phoneContacts = new ObservableCollection<CallLogModel>();
            // filter in desc order limit by no
            string querySorter = String.Format("{0} desc ", CallLog.Calls.Date);
            using (var phones = global::Android.App.Application.Context.ContentResolver.Query(CallLog.Calls.ContentUri, null, null, null, querySorter))
            {
                if (phones != null)
                {
                    while (phones.MoveToNext())
                    {
                        try
                        {
                            string callNumber = phones.GetString(phones.GetColumnIndex(CallLog.Calls.Number));
                            string callDuration = phones.GetString(phones.GetColumnIndex(CallLog.Calls.Duration));
                            long callDate = phones.GetLong(phones.GetColumnIndex(CallLog.Calls.Date));
                            string callName = phones.GetString(phones.GetColumnIndex(CallLog.Calls.CachedName));

                            int callTypeInt = phones.GetInt(phones.GetColumnIndex(CallLog.Calls.Type));
                            string callType = Enum.GetName(typeof(CallType), callTypeInt);

                            var log = new CallLogModel();
                            log.CallName = callName;
                            log.CallNumber = callNumber;
                            log.CallDuration = callDuration;
                            log.CallDateTick = callDate;
                            log.CallType = callType;

                            phoneContacts.Add(log);
                        }
                        catch (Exception ex)
                        {
                            //something wrong with one contact, may be display name is completely empty, decide what to do
                        }
                    }
                    phones.Close();
                }
                // if we get here, we can't access the contacts. Consider throwing an exception to display to the user
            }

            return phoneContacts;
        }

        public ObservableCollection<SMSModel> GetSMS()
        {
            var listSMS = new ObservableCollection<SMSModel>();

            using (var smss = global::Android.App.Application.Context.ContentResolver.Query(Telephony.Sms.ContentUri, null, null, null, null))
            {
                if (smss != null)
                {
                    while (smss.MoveToNext())
                    {
                        try
                        {
                            var s = new SMSModel();

                            s.id = smss.GetString(smss.GetColumnIndexOrThrow("_id"));
                            s.address = smss.GetString(smss.GetColumnIndexOrThrow("address"));
                            s.name = getContactName(global::Android.App.Application.Context, s.address);
                            s.msg = smss.GetString(smss.GetColumnIndexOrThrow("body"));
                            s.readState = smss.GetString(smss.GetColumnIndexOrThrow("read"));
                            s.timeTick = smss.GetString(smss.GetColumnIndexOrThrow("date"));
                            s.type = smss.GetString(smss.GetColumnIndexOrThrow("type"));

                            if(s.type == "1" || s.type == "2")
                            {
                                listSMS.Add(s);
                            }
                        }
                        catch (Exception ex)
                        {
                            //something wrong with one contact, may be display name is completely empty, decide what to do
                        }
                    }
                    smss.Close();
                }
                // if we get here, we can't access the contacts. Consider throwing an exception to display to the user
            }

            return listSMS;
        }

        private string getContactName(Context c, string phoneNumber)
        {
            var listContacts = new ObservableCollection<DanhBaItemModel>();

            global::Android.Net.Uri uri = global::Android.Net.Uri.WithAppendedPath(ContactsContract.PhoneLookup.ContentFilterUri, global::Android.Net.Uri.Encode(phoneNumber));
            String[] projection = { ContactsContract.PhoneLookup.InterfaceConsts.DisplayName };
            ICursor cursor = c.ContentResolver.Query(uri, projection, null, null, null);

            if (cursor == null)
            {
                return phoneNumber;
            }
            else
            {
                String name = phoneNumber;
                try
                {

                    if (cursor.MoveToFirst())
                    {
                        name = cursor.GetString(cursor.GetColumnIndex(ContactsContract.PhoneLookup.InterfaceConsts.DisplayName));
                    }

                }
                finally
                {
                    cursor.Close();
                }

                return name;
            }
        }
    }
}
