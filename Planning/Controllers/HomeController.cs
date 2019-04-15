using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningApi.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// redirect to the API documention of the project
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            string pathBase = HttpContext.Request.PathBase;
            var url = String.Format("{0}/swagger", pathBase);
            return Redirect(url);
        }
    }
}
