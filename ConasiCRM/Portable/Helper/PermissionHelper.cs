using ConasiCRM.Portable.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace ConasiCRM.Portable.Helper
{
	public static class PermissionHelper
	{
		//public static async Task<PermissionStatus> CheckPermissions(Permission permission)
		//{
		//	var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
		//	bool request = false;
		//	if (permissionStatus == PermissionStatus.Denied)
		//	{
		//		if (Device.RuntimePlatform == Device.iOS)
		//		{

		//			var title = $"{permission} Permission";
		//			var question = "Ứng dụng cần được cấp quyền để sử dụng chức năng này. Vui lòng vào cài đặt và cấp quyền cho ứng dụng.";
		//			var positive = "Đi đến cài đặt";
		//			var negative = "Để sau";
		//			var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
		//			if (task == null)
		//				return permissionStatus;

		//			var result = await task;
		//			if (result)
		//			{
		//				CrossPermissions.Current.OpenAppSettings();
		//			}

		//			return permissionStatus;
		//		}

		//		request = true;

		//	}

		//	if (request || permissionStatus != PermissionStatus.Granted)
		//	{
		//		var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(permission);
				
		//		if (!newStatus.ContainsKey(permission))
		//		{
		//			return permissionStatus;					
		//		}

		//		permissionStatus = newStatus[permission];

		//		if (newStatus[permission] != PermissionStatus.Granted)
		//		{
		//			permissionStatus = newStatus[permission];
  //                  var title = $"{permission} Permission";
  //                  var question = "Ứng dụng cần được cấp quyền để sử dụng chức năng này. Vui lòng vào cài đặt và cấp quyền cho ứng dụng.";
  //                  var positive = "Đi đến cài đặt";
  //                  var negative = "Để sau";
  //                  var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
		//			if (task == null)
		//				return permissionStatus;

		//			var result = await task;
		//			if (result)
		//			{
		//				CrossPermissions.Current.OpenAppSettings();
		//			}
		//			return permissionStatus;
		//		}
		//	}

		//	return permissionStatus;
		//}

        public async static Task<PermissionStatus> RequestPhotosPermission()
        {
            if (!Plugin.Media.CrossMedia.Current.IsPickPhotoSupported)
            {
                await Shell.Current.DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return PermissionStatus.Unknown;
            }


            PermissionStatus photoStatus;

            if (Device.RuntimePlatform == Device.Android)
            {
                photoStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            }
            else
            {
                photoStatus = await Permissions.CheckStatusAsync<Permissions.Photos>();
            }

            PermissionStatus firstPermission = photoStatus; // chi su dung tren ios. 

            if (photoStatus != PermissionStatus.Granted)
            {
                if (Device.iOS == Device.RuntimePlatform)
                {
                    photoStatus = await RequestPermission<Permissions.Photos>("Thư Viện", "Insight Land cần quyền truy cập vào thư viện", firstPermission);
                }
                else
                {
                    photoStatus = await RequestPermission<Permissions.StorageRead>("Thư Viện", "Insight Land cần quyền truy cập vào thư viện", firstPermission);
                }
            }
            return photoStatus;
        }

        public async static Task<PermissionStatus> RequestCameraPermission()
        {
            if (!MediaPicker.IsCaptureSupported)
            {
                await Shell.Current.DisplayAlert("No Camera", ":( No camera available.", "OK");
                return PermissionStatus.Unknown;
            }

            var cameraStatus = await CheckStatusAsync<Camera>();
            PermissionStatus firstPermission = cameraStatus; // chi su dung tren ios. 
            if (cameraStatus != PermissionStatus.Granted)
            {
                cameraStatus = await PermissionHelper.RequestPermission<Camera>("Quyền truy cập máy ảnh", "Insight Land cần quyền truy cập vào máy ảnh", firstPermission);
            }

            if (Device.RuntimePlatform == Device.Android)
            {
                if (cameraStatus == PermissionStatus.Granted)
                {
                    cameraStatus = await Permissions.CheckStatusAsync<StorageRead>();

                    if (cameraStatus != PermissionStatus.Granted)
                    {
                        cameraStatus = await RequestPermission<Permissions.StorageRead>("Thư Viện", "Insight Land cần quyền truy cập vào thư viện", firstPermission);
                    }
                }
            }
            return cameraStatus;
        }

        public static async Task<PermissionStatus> RequestPermission<T>(string title, string Description, PermissionStatus firstPermission)
          where T : BasePlatformPermission
          , new()
        {
            PermissionStatus status;
            if (Device.RuntimePlatform == Device.iOS)
            {
                status = await Permissions.RequestAsync<T>();
                if (status != PermissionStatus.Granted)
                {
                    if (firstPermission != PermissionStatus.Unknown)
                    {
                        var task = Application.Current?.MainPage?.DisplayAlert(title, Description, "Cài đặt" , "Để sau");
                        bool result = await task;
                        if (result)
                        {
                            DependencyService.Get<IOpenAppSettings>().Open();
                        }
                    }
                }
            }
            else
            {
                status = await Permissions.RequestAsync<T>();
            }
            return status;
        }
    }
}
