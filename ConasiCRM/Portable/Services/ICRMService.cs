using ConasiCRM.Portable.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.Services
{
    public interface ICRMService<T> where T : class
    {
        Task<List<T>> RetrieveMultiple(string EntityName, string FetchXml);
        Task<T> Retrieve(string Entity, Guid ID, string[] columns = null);

        Task<CrmApiResponse> Update(string EntityName, Guid Id, object Content, int Mode);
        Task<List<TResult>> DynamicRetrieve<TResult>(string EntityName, string FetchXml) where TResult : class;

        Task<CrmApiResponse> SetNullLookupField(string EntityName, Guid Id, string FieldName);
    }
}
