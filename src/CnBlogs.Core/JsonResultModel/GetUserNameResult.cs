using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.JsonResultModel
{
    public class GetUserNameResult
    {
        public bool IsSuccess { get; set; }

        public bool IsValidEmail { get; set; }
        public bool IsVaildCaptchaCode { get; set; }

        public string EmailErrorMessage { get; set; }
        public string CaptchaCodeErrorMessage { get; set; }
    }
}
