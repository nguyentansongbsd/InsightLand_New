using ConasiCRM.Portable.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ConasiCRM.Portable.Models
{
    public class LookUpConfig
    {
        public string FetchXml { get; set; }
        public string PropertyName { get; set; }
        public string Keyword { get; set; }
        public string EntityName { get; set; }
        public ListView ListView { get; set; }
        public ObservableCollection<LookUp> LookUpData { get; set; }
        public int LookUpPage { get; set; }

        public string LookUpTitle { get; set; }

        public LookUpConfig()
        {
            LookUpData = new ObservableCollection<LookUp>();
            LookUpPage = 1;
        }


        public async Task FetchData()
        {
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<LookUp>>(EntityName, string.Format(FetchXml, LookUpPage, Keyword ?? ""));
            var data = result.value;
            var count = data.Count;
            for (int i = 0; i < count; i++)
            {
                LookUpData.Add(data[i]);
            }
        }

    }
}
