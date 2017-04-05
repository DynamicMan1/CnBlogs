using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.ViewModels
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }            // 应该为用户名的指纹（对用户数据进行保护）
        public string DisplayName { get; set; }
        public string Password { get; set; }            // 应该为密码的指纹（用用户密码进行保护）
        public string ConfirmPassword { get; set; }
        public string CaptchaCode { get; set; }
    }
}
