using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Filters
{
    public class ManagerActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            ;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string flag = context.HttpContext.Session.GetString("IsManagerLogin");
            if (flag == null || flag.Equals(string.Empty))
            {
                context.Result = new RedirectResult("/Management/Login");
            }
        }
    }
}
