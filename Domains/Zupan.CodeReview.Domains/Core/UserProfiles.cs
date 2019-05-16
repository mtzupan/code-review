using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Zupan.CodeReview.Domains.Core
{
    public class UserProfiles : BaseEntity
    {
        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
        public int UserId { get; set; }

        [ForeignKey("ProfileId")]
        public virtual Profiles Profile { get; set; }
        public int ProfileId { get; set; }
    }
}
