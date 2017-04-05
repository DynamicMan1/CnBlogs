using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CnBlogs.Core.Services;
using CnBlogs.Core.ViewModels;
using CnBlogs.Core.Entities;
using CnBlogs.Core.JsonResultModel;
using CnBlogs.Core.Utils;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using CnBlogs.Core.Filters;

namespace CnBlogs.Core.Controllers
{
    public class ManagementController : Controller
    {
        private readonly IUserService userService;
        private readonly IBlogApplyService blogApplyService;
        private readonly IEmailUtil emailUtil;
        private readonly IDataProtectorUtil dataProtectorUtil;

        public ManagementController(IUserService userService, IBlogApplyService blogApplyService, IDataProtectorUtil dataProtectorUtil,IEmailUtil emailUtil)
        {
            this.userService = userService;
            this.blogApplyService = blogApplyService;
            this.emailUtil = emailUtil;
            this.dataProtectorUtil = dataProtectorUtil;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string password)
        {
            if(password == null || password.Equals(string.Empty))
            {
                return new JsonResult(new { IsSuccess = false, Message = "请填写密码！" });
            }
            // 解密password
            dataProtectorUtil.PrivateKeyJson = HttpContext.Session.GetString("PrivateKeyJson");
            dataProtectorUtil.PublicKeyJson = HttpContext.Session.GetString("PublicKeyJson");
            password = dataProtectorUtil.DecryptString(password);

            var configration = new ConfigurationBuilder().AddJsonFile("appGlobal.json").Build();
            var truePassword = configration["ManagerPassword"];
            if (password.Equals(truePassword))
            {
                HttpContext.Session.SetString("IsManagerLogin", true.ToString());
                return new JsonResult(new { IsSuccess = true });
            }
            else
            {
                return new JsonResult(new { IsSuccess = false, Message = "密码错误！" });
            }
        }

        [ServiceFilter(typeof(ManagerActionFilter))]
        public IActionResult Center()
        {
            var model = new ManagementCenterViewModel();
            model.Users = userService.GetAll();
            return View(model);
        }

        [ServiceFilter(typeof(ManagerActionFilter))]
        public IActionResult BlogApply(bool isRead = true)
        {
            IEnumerable<BlogApply> blogApplies = null;
            if (isRead)
                blogApplies = blogApplyService.GetAllNoReaderBlogApplies();
            else
                blogApplies = blogApplyService.GetAllBlogApply();
            ViewData["isRead"] = isRead;
            return View(blogApplies);
        }

        [ServiceFilter(typeof(ManagerActionFilter))]
        public IActionResult UserDetail(int id)
        {
            User user = userService.GetUserById(id);
            return View(user);
        }

        [ServiceFilter(typeof(ManagerActionFilter))]
        public IActionResult SubmitBlogApply(int id)
        {
            var result = new ManageBlogApplyResult();
            var blogApply = blogApplyService.SelectById(id);
            if(blogApply == null)
            {
                result.IsSuccess = false;
                result.Message = "该Id参数不存在！";
            }
            else
            {
                if(!blogApply.IsRead)
                {
                    blogApplyService.HadRead(id);
                    var user = userService.GetUserById(blogApply.UserId);
                    if (!user.IsBlogApply)
                    {
                        userService.UpdateBlogApply(blogApply.UserId, true);
                        result.IsSuccess = true;
                        result.Message = "操作成功！";
                        // 向用户发送邮件
                        SendEmailIfSubmit(id);
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "该用户的申请已被同意！";
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "该次申请已被处理过！";
                }
            }
            return new JsonResult(result);
        }

        private void SendEmailIfSubmit(int id)
        {
            User user = userService.GetUserById(id);
            string subject = $"博客园博客申请通知-{user.DisplayName}";
            string message = "<p>抱歉！您的博客没有被批准。</p>";
            emailUtil.Send(user.Email, subject, message);
        }

        [ServiceFilter(typeof(ManagerActionFilter))]
        public IActionResult RefuseBlogApply(int id)
        {

            var result = new ManageBlogApplyResult();
            var blogApply = blogApplyService.SelectById(id);
            if (blogApply == null)
            {
                result.IsSuccess = false;
                result.Message = "该Id参数不存在！";
            }
            else
            {
                if (!blogApply.IsRead)
                {
                    blogApplyService.HadRead(id);
                    // 向用户发送邮件
                    SendEmailIfRefuse(id);
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "该次申请已被处理过！";
                }
            }
            return new JsonResult(result);
        }

        private void SendEmailIfRefuse(int id)
        {
            User user = userService.GetUserById(id);
            string subject = $"博客园博客申请通知-{user.DisplayName}";
            string message = "<p>您的博客申请已经被批准！</p>";
            emailUtil.Send(user.Email, subject, message);
        }
    }
}
