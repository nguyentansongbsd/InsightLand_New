using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Essentials;

namespace ConasiCRM.Portable.Settings
{
    public class UserLogged
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static string Password
        {
            get => AppSettings.GetValueOrDefault(nameof(Password), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Password), value);
        }
        public static string User
        {
            get => AppSettings.GetValueOrDefault(nameof(User), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(User), value);
        }
        public static bool IsLogged
        {
            get => AppSettings.GetValueOrDefault(nameof(IsLogged), false);
            set => AppSettings.AddOrUpdateValue(nameof(IsLogged), value);
        }
    }
}
