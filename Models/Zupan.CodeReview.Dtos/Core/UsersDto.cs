using System;
using System.Collections.Generic;
using System.Text;

namespace Zupan.CodeReview.Dtos.Core
{
    public class UsersDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
