using Microsoft.AspNetCore.Identity;
using System;

namespace Repositories.Model
{
    public class User : IdentityUser, IModelDefaultProperties
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Sex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
    }    
}
