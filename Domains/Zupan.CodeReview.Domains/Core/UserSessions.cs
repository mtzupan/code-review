using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Zupan.CodeReview.Domains.Core
{
    public class UserSessions : BaseEntity
    {
        [ForeignKey("UserId")]
        public virtual Users User { get; set; }

        public string BrowserName { get; set; }
        public string IpAddress { get; set; }
        public int UserId { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime LogoutDate { get; set; }
    }
}
