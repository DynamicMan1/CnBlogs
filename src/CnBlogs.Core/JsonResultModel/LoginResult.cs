using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.JsonResultModel
{
    public class LoginResult
    {
        public bool IsSuccess { get; set; }
        public string message { get; set; }

        public bool Success { get; set; }
        public bool IsValidUserName { get; set; }
        public bool IsValidPassword { get; set; }

        public string UserNameErrorMessage { get; set; }
        public string PasswordErrorMessage { get; set; }
    }
}
