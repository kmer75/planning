using Repositories.Model;
using System.Collections.Generic;

namespace Repositories.Repositories
{
    public interface IUserRepository
    {
        User GetUser(string userId);
    }
}