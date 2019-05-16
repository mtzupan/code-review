using System;
using System.Collections.Generic;
using System.Text;

namespace Zupan.CodeReview.Domains.Core
{
    public class Users : BaseEntity
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}