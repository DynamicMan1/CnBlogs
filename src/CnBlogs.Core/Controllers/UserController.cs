using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CnBlogs.Core.Utils;
using CnBlogs.Core.ViewModels;
using CnBlogs.Core.JsonResultModel;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using CnBlogs.Core.Services;
using CnBlogs.Core.Entities;
using CnBlogs.Core.Filters;

namespace CnBlogs.Core.Controllers
{
    public class UserController : Controller
    {
        private readonly IEmailUtil emailUtil;
        private readonly ICaptchaCodeUtil captchaCodeUtil;
        private readonly IInputValidatorUtil inputValidatorUtil;
        private readonly IDataProtectorUtil dataProtectorUtil;
        private readonly IUserService userService;
        private readonly ICookieSettings cookieSettings;

        public UserController(IEmailUtil emailUtil,
            ICaptchaCodeUtil captchaCodeUtil,
            IInputValidatorUtil inputValidatorUtil,
            IDataProtectorUtil dataProtectorUtil,
            IUserService userService,
            ICookieSettings cookieSettings)
        {
            this.emailUtil = emailUtil;
            this.captchaCodeUtil = captchaCodeUtil;
            this.inputValidatorUtil = inputValidatorUtil;
            this.dataProtectorUtil = dataProtectorUtil;
            this.userService = userService;
            this.cookieSettings = cookieSettings;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var result = new LoginResult();
            result.IsValidUserName = true;
            result.IsValidPassword = true;
            if (model.UserName == null || model.UserName.Equals(string.Empty))
            {
                result.UserNameErrorMessage = "登陆用户名不可为空！";
                result.IsValidUserName = false;
            }
            if(model.Password == null || model.Password.Equals(string.Empty))
            {
                result.PasswordErrorMessage = "密码不可为空！";
                result.IsValidPassword = false;
            }
            if (result.IsValidUserName && result.IsValidPassword)
            {
                // 使用非对称解密算法，获取用户登录名和密码
                dataProtectorUtil.PrivateKeyJson = HttpContext.Session.GetString("PrivateKeyJson");
                dataProtectorUtil.PublicKeyJson = HttpContext.Session.GetString("PublicKeyJson");
                model.UserName = dataProtectorUtil.DecryptString(model.UserName);
                model.Password = dataProtectorUtil.DecryptString(model.Password);

                User user = userService.GetUserByUserName(model.UserName);

                if (user == null)
                {
                    result.IsValidUserName = false;
                    result.UserNameErrorMessage = "该登陆用户名不存在！";
                }
                else
                {
                    result.IsValidUserName = true;
                    if (model.Password.Equals(user.Password))
                    {
                        if (!user.IsActivate)
                        {
                            result.IsSuccess = false;
                            result.message = "该用户没有被激活！";
                            return new JsonResult(result);
                        }
                        // 登陆信息正确，将用户信息写入Session
                        HttpContext.Session.SetString("User", Newtonsoft.Json.JsonConvert.SerializeObject(user));
                        result.IsValidPassword = true;
                        result.IsSuccess = true;
                        userService.AddLoginTimes(user.Id);
                        // 如果用户选择下次自动登陆，则将用户密码和用户登录名使用公钥加密后写入Cookie中
                        // Cookie加密使用本机上设置的密钥，不用现在生成
                        if (model.IsRemember)
                        {
                            dataProtectorUtil.PublicKeyJson = cookieSettings.CookiePublicKeyJson;
                            dataProtectorUtil.PrivateKeyJson = cookieSettings.CookiePrivateKeyJson;
                            HttpContext.Response.Cookies.Append("UserName", dataProtectorUtil.EncryptString(user.DisplayName), new CookieOptions() { Expires = DateTime.Now.AddDays(cookieSettings.ExpiresValue) });
                            HttpContext.Response.Cookies.Append("Password", dataProtectorUtil.EncryptString(user.Password), new CookieOptions() { Expires = DateTime.Now.AddDays(cookieSettings.ExpiresValue) });
                        }
                        return new JsonResult(result);
                    }
                    else
                    {
                        result.IsValidPassword = false;
                        result.PasswordErrorMessage = "密码不正确！";
                    }
                }
            }
            result.IsSuccess = false;
            return new JsonResult(result);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            // 使用RSA解密算法，获取用户登录名和密码
            dataProtectorUtil.PrivateKeyJson = HttpContext.Session.GetString("PrivateKeyJson");
            dataProtectorUtil.PublicKeyJson = HttpContext.Session.GetString("PublicKeyJson");
            if (model.UserName != null && !model.UserName.Equals(string.Empty))
                model.UserName = dataProtectorUtil.DecryptString(model.UserName.Trim());
            if (model.Password != null && !model.Password.Equals(string.Empty))
                model.Password = dataProtectorUtil.DecryptString(model.Password.Trim());
            if (model.ConfirmPassword != null && !model.ConfirmPassword.Equals(string.Empty))
                model.ConfirmPassword = dataProtectorUtil.DecryptString(model.ConfirmPassword.Trim());

            // 验证输入数据是否合法
            RegisterResult result = ValidateRegisterInput(model);

            

            // 所有数据均合法将对用户数据进行永久化
            if (result.IsSuccess)
                userService.Register(model, HttpContext.Session.GetString("PublicKeyJson"), HttpContext.Session.GetString("PrivateKeyJson"), HttpContext.Request.Host.ToString());
            return new JsonResult(result);
        }

        private RegisterResult ValidateRegisterInput(RegisterViewModel model)
        {
            RegisterResult result = new RegisterResult();
            string outBuff = "";
            result.IsValidEmail = inputValidatorUtil.ValidateEmail(model.Email, out outBuff);
            result.EmailErrorMessage = outBuff;
            if(result.IsValidEmail)
            {
                User user = userService.GetUserByEmail(model.Email);
                if (user != null)
                {
                    result.IsValidEmail = false;
                    result.EmailErrorMessage = "邮箱已被占用！";
                }
            }
            result.IsValidUserName = inputValidatorUtil.ValidateUserName(model.UserName, out outBuff);
            result.UserNameErrorMessage = outBuff;
            if (result.IsValidUserName)
            {
                User user = userService.GetUserByUserName(model.UserName);
                if (user != null)
                {
                    result.IsValidUserName = false;
                    result.UserNameErrorMessage = "登陆用户名已被占用！";
                }
            }
            result.IsValidDisplayName = inputValidatorUtil.ValidateDisplayName(model.DisplayName, out outBuff);
            result.DisplayNameErrorMessage = outBuff;
            result.IsValidPassword = inputValidatorUtil.ValidatePassword(model.Password, out outBuff);
            result.PasswordErrorMessage = outBuff;
            result.IsValidCaptchaCode = inputValidatorUtil.ValidateCaptchaCode(model.CaptchaCode, HttpContext.Session.GetString("CaptchaCode"), out outBuff);
            result.CaptchaCodeErrorMessage = outBuff;
            result.IsValidConfirmPassword = inputValidatorUtil.ValidateConfirmPassword(model.ConfirmPassword, model.Password, out outBuff);
            result.ConfirmPasswordErrorMessage = outBuff;
            // 判断邮件是否已被其他用户使用
            User userByEmail = userService.GetUserByEmail(model.Email);
            if (userByEmail != null)
            {
                result.IsValidEmail = false;
                result.EmailErrorMessage = "该邮箱已被占用！";
                result.IsSuccess = false;
            }
            User userByUserName = userService.GetUserByEmail(model.Email);
            if (userByUserName != null)
            {
                result.IsValidUserName = false;
                result.UserNameErrorMessage = "该登陆用户名已被占用！";
                result.IsSuccess = false;
            }
            if (result.IsValidEmail && result.IsValidUserName && result.IsValidDisplayName && result.IsValidPassword && result.IsValidCaptchaCode && result.IsValidConfirmPassword)
                result.IsSuccess = true;
            else
                result.IsSuccess = false;
            return result;
        }

        public IActionResult RedirectToEmail(string email)
        {
            if(email == null || email.Equals(string.Empty))
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            var emailHost = "email." + email.Split('@')[1];
            return View("RedirectToEmail", emailHost);
        }

        public IActionResult ActivateUser(string code, string email)
        {
            string result;
            if (code == null || code.Equals(string.Empty) || email == null || email.Equals(string.Empty))
                result = "参数错误！";
            code = code.Trim();
            email = email.Trim();
            userService.ActivateUser(code, email, out result);
            return View("ActivateUser", result);
        }

        public IActionResult GetCaptchaCode()
        {
            string code;
            byte[] bytes = captchaCodeUtil.Create(out code).ToArray();
            // 用Session记录验证码信息
            HttpContext.Session.SetString("CaptchaCode", code);
            return File(bytes, @"image/png");
        }

        public IActionResult GetPublicKey()
        {
            if (HttpContext.Session.GetString("PublicKeyJson") == null || HttpContext.Session.GetString("PrivateKeyJson") == null)
            {
                HttpContext.Session.SetString("PublicKeyJson", dataProtectorUtil.PublicKeyJson);
                HttpContext.Session.SetString("PrivateKeyJson", dataProtectorUtil.PrivateKeyJson);
            }
            var rsaParamsPublic = Newtonsoft.Json.JsonConvert.DeserializeObject<RSAParameters>(HttpContext.Session.GetString("PublicKeyJson"));
            return new JsonResult(rsaParamsPublic);
        }

        public IActionResult GetUserName()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetUserName(GetUserNameViewModel model)
        {
            // 进行输入验证
            GetUserNameResult result = ValidateGetUserNameInput(model);

            if(result.IsValidEmail && result.IsVaildCaptchaCode)
            {
                model.CaptchaCode = model.CaptchaCode.Trim();
                model.Email = model.Email.Trim();
                if (model.CaptchaCode.ToUpper().Equals(HttpContext.Session.GetString("CaptchaCode").ToUpper()))
                {
                    result.IsVaildCaptchaCode = true;
                    result.IsValidEmail = true;
                    result.IsSuccess = true;
                    // 向用户邮箱发送邮件
                    string message = "";
                    if(userService.SendUserNameToEmail(model.Email, out message))
                    {
                        result.IsValidEmail = true;
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsValidEmail = false;
                        result.EmailErrorMessage = "该邮箱不存在！";
                    }
                }
                else
                {
                    result.IsVaildCaptchaCode = false;
                    result.CaptchaCodeErrorMessage = "验证码不正确！";
                }
            }
            return new JsonResult(result);
        }

        private GetUserNameResult ValidateGetUserNameInput(GetUserNameViewModel model)
        {
            GetUserNameResult result = new GetUserNameResult();
            result.IsVaildCaptchaCode = true;
            result.IsValidEmail = true;
            if (model.Email == null || model.Email.Equals(string.Empty))
            {
                result.IsValidEmail = false;
                result.EmailErrorMessage = "邮箱不能为空！";
            }
            else
            {
                string buff = "";
                if (!inputValidatorUtil.ValidateEmail(model.Email, out buff))
                {
                    result.IsValidEmail = false;
                    result.EmailErrorMessage = "邮箱不符合规范！";
                }
            }
            if (model.CaptchaCode == null || model.CaptchaCode.Equals(string.Empty))
            {
                result.IsVaildCaptchaCode = false;
                result.CaptchaCodeErrorMessage = "验证码不能为空！";
            }
            return result;  
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            ResetPasswordResult result = ValidateResetPasswordResult(model);
            if (result.IsSuccess)
            {
                string buff = "";
                if(userService.SendResetPasswordEmail(model.Email, dataProtectorUtil.PublicKeyJson, dataProtectorUtil.PrivateKeyJson, HttpContext.Request.Host.ToString(), out buff))
                    return new JsonResult(result);
                else
                {
                    result.IsSuccess = false;
                    result.EmailErrorMessage = buff;
                }
            }
            return new JsonResult(result);
        }

        private ResetPasswordResult ValidateResetPasswordResult(ResetPasswordViewModel model)
        {
            ResetPasswordResult result = new ResetPasswordResult();
            string buff = "";
            result.IsValidUserName = inputValidatorUtil.ValidateUserName(model.UserName, out buff);
            result.UserNameErrorMessage = buff;
            result.IsValidEmail = inputValidatorUtil.ValidateEmail(model.Email, out buff);
            result.EmailErrorMessage = buff;
            result.IsValidCaptchaCode = inputValidatorUtil.ValidateCaptchaCode(model.CaptchaCode, HttpContext.Session.GetString("CaptchaCode"), out buff);
            result.CaptchaCodeErrorMessage = buff;
            if (result.IsValidEmail && result.IsValidUserName && result.IsValidCaptchaCode)
                result.IsSuccess = true;
            else
                result.IsSuccess = false;
            return result;
        }

        public IActionResult ResetPasswordAction(string email, string code)
        {
            string result;
            if (code == null || code.Equals(string.Empty) || email == null || email.Equals(string.Empty))
                result = "参数错误！";
            code = code.Trim();
            email = email.Trim();
            if(userService.ValidateResetPasswordCode(code, email, out result))
            {
                var model = new ResetPasswordActionViewModel();
                model.Code = code;
                model.Email = email;
                return View("ResetPasswordAction", model);
            }
            return View("ActionResult", result);
        }

        [HttpPost]
        public IActionResult ResetPasswordAction(string email, string code, string newPassword)
        {
            // 使用非对称解密算法，获取用户登录名和密码的指纹
            dataProtectorUtil.PrivateKeyJson = HttpContext.Session.GetString("PrivateKeyJson");
            dataProtectorUtil.PublicKeyJson = HttpContext.Session.GetString("PublicKeyJson");
            newPassword = dataProtectorUtil.DecryptString(newPassword);

            string result;
            if (code == null || code.Equals(string.Empty) || email == null || email.Equals(string.Empty))
                result = "参数错误！";
            code = code.Trim();
            email = email.Trim();
            if (userService.ValidateResetPasswordCode(code, email, out result))
            {
                User user = userService.GetUserByEmail(email);
                // 更新用户的密码
                userService.UpdatePassword(user.Id, newPassword);
                return View("ActionResult", result);
            }
            return View("ActionResult", result);
        }

        public IActionResult BlogApply()
        {
            if (HttpContext.Session.GetString("User") == null || HttpContext.Session.GetString("User").Equals(string.Empty))
            {
                return View("BlogApplyInfo", 0);
            }
            User user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            // 用户已经开通博客功能
            if (user.IsBlogApply)
            {
                return View("BlogApplyInfo", 1);
            }
            if (userService.IsHadBlogApply(user.Id))
            {
                return View(true);
            }
            return View(false);
        }

        // 一下Action需要先登录才有权限访问
        [ServiceFilter(typeof(LoginActionFilter))]
        public IActionResult Center()
        {
            return View();
        }

        [ServiceFilter(typeof(LoginActionFilter))]
        [HttpPost]
        public IActionResult BlogApply(BlogApplyViewModel model)
        {
            BlogApplyResult result = ValidateBlogApplyInput(model);
            User user = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            if (result.IsSuccess)
                userService.CreateBlogApply(user.Id, model);
            return new JsonResult(result);
        }

        private BlogApplyResult ValidateBlogApplyInput(BlogApplyViewModel model)
        {
            BlogApplyResult result = new BlogApplyResult();
            string buff;
            result.IsValidReason = inputValidatorUtil.ValidateReason(model.Reason, out buff);
            result.ReasonErrorMessage = buff;
            result.IsVaildRealName = inputValidatorUtil.ValidateRealName(model.RealName, out buff);
            result.RealNameErrorMessage = buff;
            result.IsVaildPosition = true;
            result.IsVaildUnit = true;
            result.IsVaildInterest = true;
            if (result.IsValidReason && result.IsVaildRealName && result.IsVaildPosition && result.IsVaildUnit && result.IsVaildInterest)
                result.IsSuccess = true;
            else
                result.IsSuccess = false;
            return result;
        }
    }
}
