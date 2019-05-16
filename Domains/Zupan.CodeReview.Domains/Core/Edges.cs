using System;
using System.Collections.Generic;
using System.Text;

namespace Zupan.CodeReview.Domains.Core
{
    public class Edges : BaseEntity
    {
        public string EdgeGuid { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Serial { get; set; }
        public string MacAddress1 { get; set; }
        public string MacAddress2 { get; set; }
        public string IpAddress1 { get; set; }
        public string IpAddress2 { get; set; }
    }
}
