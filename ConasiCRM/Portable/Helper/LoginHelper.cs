using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ConasiCRM.Portable.Config;
using ConasiCRM.Portable.Models;
using Newtonsoft.Json;

namespace ConasiCRM.Portable.Helper
{
    public class LoginHelper
    {
        public static async Task<HttpResponseMessage> Login()
        {
            var client = BsdHttpClient.Instance();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://login.microsoftonline.com/common/oauth2/token");//OrgConfig.LinkLogin
            var formContent = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("resource", OrgConfig.Resource),
                        //new KeyValuePair<string, string>("client_id", OrgConfig.ClientId),
                        //new KeyValuePair<string, string>("client_secret", OrgConfig.ClientSecret),
                        //new KeyValuePair<string, string>("grant_type", "client_credentials")
                        new KeyValuePair<string, string>("client_id", "2ad88395-b77d-4561-9441-d0e40824f9bc"),
                        new KeyValuePair<string, string>("username", OrgConfig.UserName),
                        new KeyValuePair<string, string>("password", OrgConfig.Password),
                        new KeyValuePair<string, string>("grant_type", "password"),

                    });
            request.Content = formContent;
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                GetTokenResponse tokenData = JsonConvert.DeserializeObject<GetTokenResponse>(body);
            }

            return response;
        }
    }
}
