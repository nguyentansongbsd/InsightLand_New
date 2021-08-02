using System;
using System.Threading.Tasks;

namespace ConasiCRM.Portable.IServices
{
    public interface INumImeiService
    {
        Task<string> GetImei();
    }
}
