using Entities.DataTransferObjects.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAuthRepository
    {
        string Authentication(UserAuthDto authUser, string role);

    }
}
