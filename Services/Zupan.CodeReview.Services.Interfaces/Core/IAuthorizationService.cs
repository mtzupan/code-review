using System;
using System.Collections.Generic;
using System.Text;
using Zupan.CodeReview.Dtos.Core;

namespace Zupan.CodeReview.Services.Interfaces.Core
{
    public interface IAuthorizationService
    {
        UsersDto GetUserByLoginAndPassword(string login, string password);

        UsersDto GetUserById(int id);
    }
}
