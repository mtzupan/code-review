using System;
using System.Collections.Generic;
using System.Text;

namespace Zupan.CodeReview.Domains.Core
{
    public class DistributedServices : BaseEntity
    {
        public string Name { get; set; }
        public string Endpoint { get; set; }
    }
}