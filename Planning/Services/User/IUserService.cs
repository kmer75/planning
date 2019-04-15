using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Repositories.Repositories;
using Repositories.Model;
using PlanningApi.DTO;

namespace PlanningApi.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Find User by his ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        UserDto FindByIdAsync(string Id);
        /// <summary>
        /// Find User by his username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        UserDto FindByNameAsync(string username);
        /// <summary>
        /// Get user by Id with all his context
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserDto GetUser(string userId);
        /// <summary>
        /// Is user exist
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool IsUserNameExist(string username);

    }
}
