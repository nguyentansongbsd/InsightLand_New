using System;
namespace ConasiCRM.Portable.IServices
{
    public interface IToastMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}
