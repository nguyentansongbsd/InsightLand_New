using System;
using System.Globalization;

namespace ConasiCRM.Portable.Models
{
    public class CallLogModel
    {
        public string CallName { get; set; }
        public string CallNumber { get; set; }
        public string CallDuration { get; set; }
        public string CallDurationFormat
        {
            get
            {
                var intDuration = Convert.ToInt32(CallDuration);
                TimeSpan time = TimeSpan.FromSeconds(intDuration);

                //here backslash is must to tell that colon is
                //not the part of format, it just a character that we want in output
                return time.ToString(@"hh\:mm\:ss");
            }
        }
        public long CallDateTick { get; set; }
        public DateTime CallDate
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(this.CallDateTick).ToLocalTime();
            }
        }

        public string CallType { get; set; }
        public string CallTypeImage { get
            {
                switch (CallType)
                {
                    case "Incoming":return "icon_incoming_call";
                    case "Missed": return "icon_missed_call";
                    case "Outgoing":return "icon_outgoing_call";
                    case "Rejected":return "icon_rejected_call";
                    default: return null;
                }
            } }

        public string CallTitle { get { if (CallName == null) { return CallNumber; } else { return CallName + " (" + CallNumber + ")"; } } }
        public string CallDetail { get { if (CallType == "Incoming" || CallType == "Outgoing") { return CallDate.ToString() + " - " + CallDurationFormat; } else { return CallDate.ToString(); } } }
        //public string CallDescription { get => $"{CallDate.ToString("g", CultureInfo.CreateSpecificCulture("es-mx"))} - {CallType} | Duration: {CallDurationFormat}"; }
    }
}
