using System;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.IServices
{
    public interface IUrlEnCodeSevice
    {
        Task<string> GetUrlEnCode(string url);
    }
}
