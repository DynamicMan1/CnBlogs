using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.ViewModels
{
    public class ResetPasswordViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string CaptchaCode { get; set; }
    }
}
