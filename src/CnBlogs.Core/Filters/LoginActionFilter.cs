using CnBlogs.Core.Entities;
using CnBlogs.Core.Services;
using CnBlogs.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Filters
{
    public class LoginActionFilter : IActionFilter
    {
        private readonly IUserService userService;
        private readonly IDataProtectorUtil dataProtectorUtil;
        private readonly ICookieSettings cookieSettings;

        public LoginActionFilter(IUserService userService, IDataProtectorUtil dataProtectorUtil, ICookieSettings cookieSettings)
        {
            this.userService = userService;
            this.dataProtectorUtil = dataProtectorUtil;
            this.cookieSettings = cookieSettings;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            ;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string user = context.HttpContext.Session.GetString("User");
            if(user == null || user.Equals(string.Empty))
            {
                if (context.HttpContext.Request.Cookies["UserName"] != null && context.HttpContext.Request.Cookies["UserName"] != string.Empty && context.HttpContext.Request.Cookies["Password"] != null && context.HttpContext.Request.Cookies["Password"] != string.Empty)
                {
                    dataProtectorUtil.PrivateKeyJson = cookieSettings.CookiePrivateKeyJson;
                    User userObj = userService.GetUserByUserName(dataProtectorUtil.DecryptString(context.HttpContext.Request.Cookies["UserName"]));
                    if (userObj.Password.Equals(dataProtectorUtil.DecryptString(context.HttpContext.Request.Cookies["Password"])))
                    {
                        context.HttpContext.Session.SetString("User", Newtonsoft.Json.JsonConvert.SerializeObject(userObj));
                        userService.AddLoginTimes(userObj.Id);
                    }
                    else
                        context.Result = new RedirectResult("/User/Login");
                }
                else
                {
                    context.Result = new RedirectResult("/User/Login");
                }
            }       
        }
    }
}
