using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSE_Hankers.Models.IRepositories
{
    public interface IUserRepository
    {
        ApplicationUser Update(ApplicationUser user);
    }
}
