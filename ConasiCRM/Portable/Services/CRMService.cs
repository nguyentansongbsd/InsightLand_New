using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ConasiCRM.Portable.Models;
using ConasiCRM.Portable;
using ConasiCRM.Portable.Helper;

namespace ConasiCRM.Portable.Services
{
    public class CRMService<T> : ICRMService<T> where T : class
    {
        public async Task<List<TResult>> DynamicRetrieve<TResult>(string EntityName, string FetchXml) where TResult : class
        {
            string Token = App.Current.Properties["Token"] as string;
            var client = BsdHttpClient.Instance();
            //using (var client = new HttpClient())
            //{
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://bsddemo07112018.api.crm.dynamics.com/api/data/v9.1/{EntityName}?fetchXml={FetchXml}");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                var api_Response = JsonConvert.DeserializeObject<RetrieveMultipleApiResponse<TResult>>(body);
                return api_Response.value;
            }
            //}
            return null;
        }

        public async Task<T> Retrieve(string EntityName, Guid ID, string[] columns = null)
        {
            string Token = App.Current.Properties["Token"] as string;
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://bsddemo07112018.api.crm.dynamics.com/api/data/v9.1/{EntityName}({ID})");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<T>(body);
                    return data;
                }

            }
            return null;
        }

        public async Task<List<T>> RetrieveMultiple(string EntityName, string FetchXml)
        {

            using (var client = new HttpClient())
            {
                try
                {
                    string Token = App.Current.Properties["Token"] as string;
                    var request = new HttpRequestMessage(HttpMethod.Get, $"https://bsddemo07112018.api.crm.dynamics.com/api/data/v9.1/{EntityName}?fetchXml={FetchXml}");

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.SendAsync(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var body = await response.Content.ReadAsStringAsync();
                        var api_Response = JsonConvert.DeserializeObject<RetrieveMultipleApiResponse<T>>(body);
                        return api_Response.value;
                    }
                }
                catch (Exception ex)
                {
                    string mess = ex.Message;
                }
            }
            return null;
        }

        public async Task<CrmApiResponse> SetNullLookupField(string EntityName, Guid Id, string FieldName)
        {
            using (var client = new HttpClient())
            {
                string Token = App.Current.Properties["Token"] as string;
                var request = new HttpRequestMessage(HttpMethod.Delete, $"https://bsddemo07112018.api.crm.dynamics.com/api/data/v9.1/{EntityName}({Id})/{FieldName}/$ref");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return new CrmApiResponse()
                    {
                        IsSuccess = true
                    };
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var api_Response = JsonConvert.DeserializeObject<ErrorResponse>(body);
                    return new CrmApiResponse()
                    {
                        IsSuccess = false,
                        ErrorResponse = api_Response
                    };
                }
            }

        }

        public async Task<CrmApiResponse> Update(string EntityName, Guid Id, object formContent, int Mode)
        {
            string Token = App.Current.Properties["Token"] as string;
            using (var client = new HttpClient())
            {
                string Url = $"https://bsddemo07112018.api.crm.dynamics.com/api/data/v9.1/{EntityName}";
                if (Mode == 2)
                {
                    Url += "(" + Id.ToString() + ")";
                }
                string objContent = JsonConvert.SerializeObject(formContent);
                HttpContent content = new StringContent(objContent, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response;

                if (Mode == 1)
                {
                    response = await client.PostAsync(Url, content);
                }
                else
                {
                    var method = new HttpMethod("PATCH");
                    var request = new HttpRequestMessage(method, Url);
                    request.Content = content;
                    response = await client.SendAsync(request);
                }

                CrmApiResponse res = new CrmApiResponse();

                if (response.IsSuccessStatusCode)
                {
                    res.IsSuccess = true;
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    var api_Response = JsonConvert.DeserializeObject<ErrorResponse>(body);
                    res.ErrorResponse = api_Response;
                }
                return res;
            }
        }
    }
}
