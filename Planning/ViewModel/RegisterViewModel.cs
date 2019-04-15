using Repositories.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningApi.ViewModel
{
    public class RegisterViewModel
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Sex { get; set; }
        public string UserName { get; set; }
        public string DateOfBirth { get; set; }
    }
    
}
