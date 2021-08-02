using ConasiCRM.Portable.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.ViewModels
{
    public class LookUpViewModel<T> : BaseViewModel where T : class
    {
        public ObservableCollection<T> Data { get; set; }
        public T SelectedItem { get; set; }
        public string EntityName { get; set; }
        public string FetchXml { get; set; }
        private int _page = 1;
        public int Page
        {
            get
            {
                return _page;
            }
            set
            {
                if (this._page != value)
                {
                    this._page = value;
                    OnPropertyChanged("Page");
                }
            }
        }
        public ICRMService<T> _service;


        public LookUpViewModel(string entityName, ICRMService<T> service)
        {
            this.EntityName = entityName;
            _service = service;
            Data = new ObservableCollection<T>();
        }

        public async Task LoadData()
        {
            string xml = string.Format(FetchXml, this._page);
            var data = await _service.RetrieveMultiple(EntityName, xml);
            foreach (var item in data)
            {
                Data.Add(item);
            }
            this.IsBusy = false;
        }
    }
}
