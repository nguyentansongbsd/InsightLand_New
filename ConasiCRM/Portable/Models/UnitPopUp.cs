using System;
using System.Collections.Generic;
using System.Text;

namespace ConasiCRM.Portable.Models
{
    public class UnitPopUp
    {
        // fulloption
        public static List<PopupItem> OnlyView = new List<PopupItem>()
            {
                new PopupItem("Xem thông tin căn hộ",PopupItemValue.View),
                new PopupItem("Đóng",PopupItemValue.Close)
            };
        public static List<PopupItem> Queuing = new List<PopupItem>()
            {
                new PopupItem("Xem thông tin căn hộ",PopupItemValue.View),
                new PopupItem("Tạo đặt chỗ",PopupItemValue.Queuing),
                new PopupItem("Đóng",PopupItemValue.Close)
            };
        public static List<PopupItem> Full = new List<PopupItem>()
            {
                new PopupItem("Xem thông tin căn hộ",PopupItemValue.View),
                new PopupItem("Tạo đặt chỗ",PopupItemValue.Queuing),
                new PopupItem("Danh sách đặt chỗ",PopupItemValue.ViewQueueList),
                new PopupItem("Đóng",PopupItemValue.Close)
            };
    }


    public enum PopupItemValue
    {
        View,
        Queuing,
        ViewQueueList,
        Reservation,
        Close
    };
    public class PopupItem
    {
        public string Name { get; set; }
        public PopupItemValue Value { get; set; }

        public PopupItem(string name, PopupItemValue value)
        {
            Name = name;
            Value = value;
        }
    }
}
