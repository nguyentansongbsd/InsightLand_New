using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Helper;
using ConasiCRM.Portable.IServices;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable.Settings;
using ConasiCRM.Portable.ViewModels;
using FFImageLoading;
using FFImageLoading.Config;
using FFImageLoading.Forms;
using FormsVideoLibrary;
using Newtonsoft.Json;
using Stormlion.PhotoBrowser;
using Xamarin.CommunityToolkit.Core;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace ConasiCRM.Portable
{
    public class AuthenticatedHttpImageClientHandler : HttpClientHandler
    {
        private readonly string _getToken;

        public AuthenticatedHttpImageClientHandler(string getToken)
        {
            if (getToken == null) throw new ArgumentNullException("getToken");
            _getToken = getToken;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Authorization", "Bearer " + _getToken);
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }

    public partial class BlankPage : ContentPage
    {

        public BlankPage()
        {

            InitializeComponent();
        }
    }
}
