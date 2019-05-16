using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zupan.CodeReview.Domains.Core;
using Zupan.CodeReview.Dtos.Core;
using Zupan.CodeReview.Library.Security;
using Zupan.CodeReview.Services.Interfaces.Core;
using Zupan.CodeReview.UnitOfWork;

namespace Zupan.CodeReview.Services.Core
{
    public class AuthorizationService : IAuthorizationService
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public IMapper Mapper { get; set; }

        public AuthorizationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        public UsersDto GetUserByLoginAndPassword(string login, string password)
        {
            var user = UnitOfWork.GetCoreRepository<Users>().GetSingle(x => x.Login == login);

            var passwordDecrypted = Encryption.Decrypt(user.Password);

            if (login == user.Login && passwordDecrypted == password)
            {
                return Mapper.Map<Users, UsersDto>(user);
            }
            return null;
        }

        public UsersDto GetUserById(int id)
        {
            var user = UnitOfWork.GetCoreRepository<Users>().GetOnlyIncludedProperties(u => u.Id == id)
                .FirstOrDefault();
            var userProfile = UnitOfWork.GetCoreRepository<UserProfiles>().GetOnlyIncludedProperties(u => u.UserId == user.Id, u => u.Profile).FirstOrDefault();
            var userDto = Mapper.Map<Users, UsersDto>(user);
            if (userProfile != null)
            {
                userDto.Name = userProfile.Profile.Name;
            }
            return userDto;
        }
    }
}