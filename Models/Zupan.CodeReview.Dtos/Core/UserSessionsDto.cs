using System;
using System.Collections.Generic;
using System.Text;

namespace Zupan.CodeReview.Dtos.Core
{
    public class UserSessionsDto
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public UsersDto User { get; set; }
    }
}
