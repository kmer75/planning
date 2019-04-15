using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PlanningApi.Auth;
using PlanningApi.Helpers;
using Repositories.Repositories;
using Repositories.Model;
using AutoMapper;
using PlanningApi.DTO;

namespace PlanningApi.Services
{
    public class UserService : IUserService
    {
        #region proprietes

        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        #endregion

        #region constructor

        public UserService(UserManager<User> userManager, IUserRepository userRepository, IMapper mapper)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        #endregion


        #region methodes

        /// <summary>
        /// Find User by his ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public UserDto FindByIdAsync(string Id)
        {
            var entireUser = this._userManager.FindByIdAsync(Id).Result;
            var user = _mapper.Map<UserDto>(entireUser);
            return user;
        }

        /// <summary>
        /// Find User by his username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public UserDto FindByNameAsync(string username)
        {
            var entireUser = this._userManager.FindByNameAsync(username).Result;
            var user = _mapper.Map<UserDto>(entireUser);
            return user;
        }

        /// <summary>
        /// Get user by Id with all his context
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserDto GetUser(string userId)
        {
            var entireUser = this._userManager.FindByIdAsync(userId).Result;
            var user = _mapper.Map<UserDto>(entireUser);
            return user;
        }


        public bool IsUserNameExist(string username)
        {
            var user = this._userManager.FindByNameAsync(username).Result;
            return user != null;
        }

        #endregion

    }
    }
