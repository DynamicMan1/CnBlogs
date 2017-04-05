using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Utils
{
    public interface IInputValidatorUtil
    {
        bool ValidateEmail(string email, out string message);
        bool ValidateDisplayName(string displayName, out string message);
        bool ValidateUserName(string userName, out string message);
        bool ValidatePassword(string password, out string message);
        bool ValidateConfirmPassword(string confirmPassword, string password, out string message);
        bool ValidateCaptchaCode(string captchaCode, string validCaptchaCode, out string message);
        bool ValidateReason(string reason, out string message);
        bool ValidateRealName(string realName, out string message);
    }
}
