using ConasiCRM.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class Item : BaseViewModel
    {
        public Item(string name)
        {
            this.Name = name;
            this.Children = new ObservableCollection<Item>();
        }

        public string Name { get; set; }
        public IList<Item> Children { get; set; }
    }
}
