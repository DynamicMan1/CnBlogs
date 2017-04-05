using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.JsonResultModel
{
    public class ResetPasswordResult
    {
        public bool IsSuccess { get; set; }

        public bool IsValidUserName { get; set; }
        public bool IsValidEmail { get; set; }
        public bool IsValidCaptchaCode { get; set; }

        public string UserNameErrorMessage { get; set; }
        public string EmailErrorMessage { get; set; }
        public string CaptchaCodeErrorMessage { get; set; }
    }
}
