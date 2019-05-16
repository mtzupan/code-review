using System;
using System.Collections.Generic;
using System.Text;

namespace Zupan.CodeReview.Domains.Core
{
    public class Machines : BaseEntity
    {
        public string Name { get; set; }
        public string AssetNumber { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Serial { get; set; }
    }
}