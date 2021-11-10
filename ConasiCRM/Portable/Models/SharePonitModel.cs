using System;
using System.Collections.Generic;

namespace ConasiCRM.Portable.Models
{
    public class SharePonitModel
    {
        public String odataMetadata { get; set; }
        public List<Value> value { get; set; }
    }

    public class Value
    {
        public String odataType { get; set; }

        public String odataId { get; set; }

        public String odataEditLink { get; set; }

        public String checkInComment { get; set; }

        public int checkOutType { get; set; }

        public String contentTag { get; set; }

        public int customizedPageStatus { get; set; }

        public String eTag { get; set; }

        public bool exists { get; set; }

        public bool irmEnabled { get; set; }

        public String length { get; set; }

        public int level { get; set; }

        public Object linkingUri { get; set; }

        public String linkingUrl { get; set; }

        public int majorVersion { get; set; }

        public int minorVersion { get; set; }

        public String name { get; set; }

        public String serverRelativeUrl { get; set; }

        public DateTime timeCreated { get; set; }

        public DateTime timeLastModified { get; set; }

        public Object title { get; set; }

        public int uIVersion { get; set; }

        public String uIVersionLabel { get; set; }

        public String uniqueId { get; set; }
    }
}
