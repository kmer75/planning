using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningApi.ViewModel
{
    //[Validator(typeof(LoginViewModelValidator))]
    public class LoginViewModel
    {
        public string Username { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }
 }
    

}
