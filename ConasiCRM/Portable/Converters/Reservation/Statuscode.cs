using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Converters.Reservation
{
    public class Statuscode
    {
        public static string Format(int value)
        {
            switch (value)
            {
                case 1: return "In Progress";
                case 100000007: return "Quotation";
                case 100000000: return "Reservation";
                case 100000006: return "Collected";
                case 100000010: return "Đã ký phiếu cọc";
                case 2: return "In Progress";
                case 3: return "Deposited";
                case 100000002: return "Pending Cancel Deposit";
                case 100000004: return "Signed RF";
                case 100000009: return "Expired";
                case 4: return "Won";
                case 100000001: return "Terminated";
                case 100000003: return "Reject";
                case 5: return "Lost";
                case 6: return "Canceled";
                case 7: return "Revised";
                case 100000005: return "Expired of signing RF";
                case 100000008: return "Expired Quotation";
                default:
                    return "";
            }
        }
    }
}
