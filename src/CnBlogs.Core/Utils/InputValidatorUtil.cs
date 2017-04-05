using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CnBlogs.Core.Utils
{
    public class InputValidatorUtil : IInputValidatorUtil
    {
        private readonly string EmailRegex = @"^(\w[-\w.+]*@([A-Za-z0-9][-A-Za-z0-9]+\.)+[A-Za-z]{2,14})$";
        private readonly string DisplayNameRegex = @"^([A-Za-z0-9_\-\u4e00-\u9fa5]{4,30})$";
        private readonly string UserNameRegex = @"^([A-Za-z0-9_\-\u4e00-\u9fa5]{4,30})$";
        private readonly string PasswordRegex = @"^([A-Za-z0-9_\-\u4e00-\u9fa5]{4,30})$";
        private readonly string RealNameRegex = @"^([\u4e00-\u9fa5]{1,20}|[a-zA-Z\.\s]{1,20})$";
        private readonly string ReasonRegex = @"^(.{10,2000})$";

        public bool ValidateEmail(string email, out string message)
        {
            if (email == null || email.Equals(string.Empty))
            {
                message = "邮箱不可为空！";
                return false;
            }
            else if (Regex.IsMatch(email, EmailRegex))
            {
                message = "";
                return true;
            }
            message = "邮箱不合法！";
            return false;
        }

        public bool ValidateDisplayName(string displayName, out string message)
        {
            if (displayName == null || displayName.Equals(string.Empty))
            {
                message = "显示名称不可为空！";
                return false;
            }
            else if (Regex.IsMatch(displayName, DisplayNameRegex))
            {
                message = "";
                return true;
            }
            message = "显示名称不合法！";
            return false;
        }

        public bool ValidateUserName(string userName, out string message)
        {
            if (userName == null || userName.Equals(string.Empty))
            {
                message = "登录用户名不可为空！";
                return false;
            }
            else if (Regex.IsMatch(userName, UserNameRegex))
            {
                message = "";
                return true;
            }       
            message = "登录用户名不合法！";
            return false;
        }

        public bool ValidatePassword(string password, out string message)
        {
            if (password == null || password.Equals(string.Empty))
            {
                message = "密码不可为空！";
                return false;
            }
            else if (Regex.IsMatch(password, PasswordRegex))
            {
                message = "";
                return true;
            }        
            message = "密码不合法！";
            return false;
        }

        public bool ValidateConfirmPassword(string confirmPassword, string password, out string message)
        {
            if (password == null || password.Equals(string.Empty))
            {
                message = "请先输入密码！";
                return false;
            }
            else if(confirmPassword == null || confirmPassword.Equals(string.Empty))
            {
                message = "确认密码不可为空！";
                return false;
            }
            else if (confirmPassword.Equals(password))
            {
                message = "";
                return true;
            }
            message = "密码与确认密码输入不一致！";
            return false;
        }

        public bool ValidateCaptchaCode(string captchaCode, string validCaptchaCode, out string message)
        {
            
            if(captchaCode == null || captchaCode.Equals(string.Empty))
            {
                message = "验证码不可为空！";
                return false;
            }
            else if(validCaptchaCode == null || captchaCode.Equals(string.Empty))
            {
                message = "验证码已经过期！";
                return false;
            }
            else if (captchaCode.ToUpper().Equals(validCaptchaCode.ToUpper())) {
                message = "";
                return true;
            }
            message = "验证码输入错误！";
            return false;
        }

        public bool ValidateReason(string reason, out string message)
        {
            if (reason == null || reason.Equals(string.Empty))
            {
                message = "申请理由不可为空！";
                return false;
            }
            else
            {
                if(!Regex.IsMatch(reason, ReasonRegex))
                {
                    message = "申请理由不符合规则！";
                    return false;
                }
            }
            message = "";
            return true;
        }

        public bool ValidateRealName(string realName, out string message)
        {
            if (realName != null && !realName.Equals(string.Empty))
            {
                if(Regex.IsMatch(realName, RealNameRegex))
                {
                    message = "";
                    return true;
                }
                message = "真实姓名不符合规范！";
                return false;
            }
            message = "";
            return true;
        }
    }
}
