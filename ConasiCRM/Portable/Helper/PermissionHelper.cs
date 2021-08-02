using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Helper
{
	public static class PermissionHelper
	{
		public static async Task<PermissionStatus> CheckPermissions(Permission permission)
		{
			var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
			bool request = false;
			if (permissionStatus == PermissionStatus.Denied)
			{
				if (Device.RuntimePlatform == Device.iOS)
				{

					var title = $"{permission} Permission";
					var question = "Ứng dụng cần được cấp quyền để sử dụng chức năng này. Vui lòng vào cài đặt và cấp quyền cho ứng dụng.";
					var positive = "Đi đến cài đặt";
					var negative = "Để sau";
					var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
					if (task == null)
						return permissionStatus;

					var result = await task;
					if (result)
					{
						CrossPermissions.Current.OpenAppSettings();
					}

					return permissionStatus;
				}

				request = true;

			}

			if (request || permissionStatus != PermissionStatus.Granted)
			{
				var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(permission);
				
				if (!newStatus.ContainsKey(permission))
				{
					return permissionStatus;					
				}

				permissionStatus = newStatus[permission];

				if (newStatus[permission] != PermissionStatus.Granted)
				{
					permissionStatus = newStatus[permission];
                    var title = $"{permission} Permission";
                    var question = "Ứng dụng cần được cấp quyền để sử dụng chức năng này. Vui lòng vào cài đặt và cấp quyền cho ứng dụng.";
                    var positive = "Đi đến cài đặt";
                    var negative = "Để sau";
                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
					if (task == null)
						return permissionStatus;

					var result = await task;
					if (result)
					{
						CrossPermissions.Current.OpenAppSettings();
					}
					return permissionStatus;
				}
			}

			return permissionStatus;
		}
	}
}
