using PlanningApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningApi.App_Start.Filters
{
    public class ModelStateErrorFilter: IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var JsonErrors = Errors.ErrorBuilder(context.ModelState);
                context.Result = new BadRequestObjectResult(new { errors = JsonErrors });
            }
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do something after the action executes
        }
    }
}
