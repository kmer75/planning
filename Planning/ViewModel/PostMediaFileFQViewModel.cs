using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningApi.ViewModel
{
    public class PostMediaFileFQViewModel
    {
        public IFormFile File { get; set;}
        public string reference { get; set; }
    }
}
