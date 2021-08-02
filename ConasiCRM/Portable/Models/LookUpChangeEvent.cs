using System;
namespace ConasiCRM.Portable.Models
{
    public class LookUpChangeEvent : EventArgs
    {
        public object Item { get; set; }
    }
}
