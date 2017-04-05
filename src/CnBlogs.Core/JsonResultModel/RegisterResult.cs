using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.JsonResultModel
{
    public class RegisterResult
    {
        public bool IsSuccess { get; set; }

        public bool IsValidEmail { get; set; }
        public bool IsValidUserName { get; set; }
        public bool IsValidDisplayName { get; set; }
        public bool IsValidPassword { get; set; }
        public bool IsValidConfirmPassword { get; set; }
        public bool IsValidCaptchaCode { get; set; }

        public string EmailErrorMessage { get; set; }
        public string UserNameErrorMessage { get; set; }
        public string DisplayNameErrorMessage { get; set; }
        public string PasswordErrorMessage { get; set; }
        public string ConfirmPasswordErrorMessage { get; set; }
        public string CaptchaCodeErrorMessage { get; set; }
    }
}
