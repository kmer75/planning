using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Model;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public User GetUser(string userId)
        {
            var user = 
                this._context.Users.SingleOrDefault(u => u.Id.Equals(userId));
            return user;
        }

    }
}
