using CnBlogs.Core.Entities;
using CnBlogs.Core.Repository;
using CnBlogs.Core.Utils;
using CnBlogs.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CnBlogs.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IEmailRepository emailRepository;
        private readonly IEmailUtil emailUtil;
        private readonly IDataProtectorUtil dataProtectorUtil;
        private readonly IBlogApplyRepository blogApplyRepository;

        public UserService(IUserRepository userRepository, 
            IEmailRepository emailRepository, 
            IEmailUtil emailUtil, 
            IDataProtectorUtil dataProtectorUtil,
            IBlogApplyRepository blogApplyRepository)
        {
            this.userRepository = userRepository;
            this.emailRepository = emailRepository;
            this.emailUtil = emailUtil;
            this.dataProtectorUtil = dataProtectorUtil;
            this.blogApplyRepository = blogApplyRepository;
        }

        public void Register(RegisterViewModel registerViewModel, string publicKeyJson, string privateKeyJson, string host)
        {
            User user = new User();
            user.Email = registerViewModel.Email;
            user.UserName = registerViewModel.UserName;
            user.DisplayName = registerViewModel.DisplayName;
            user.Password = registerViewModel.Password;
            user.CreateTime = DateTime.Now;
            user.LastModifiedTime = DateTime.Now;
            user.LastLoginTime = DateTime.Now;
            user.LoginTimes = 0;
            user.IsActivate = false;    
            userRepository.Insert(user);
            User gotUser = userRepository.SelectByUserName(registerViewModel.UserName);
            // 生成验证码
            dataProtectorUtil.PublicKeyJson = publicKeyJson;
            // 生成激活码，激活码由UserName字段和Password字段合并加密而成
            var code = dataProtectorUtil.EncryptString(gotUser.UserName + gotUser.Password);
            // 解决get不能传“+”的问题
            code = code.Replace("+", "_");
            // 发送验证邮件并记录
            var subject = $"博客园帐户激活邮件-{gotUser.DisplayName}";
            var message = new StringBuilder();
            message.Append("<div>您好！<br/><br/>");
            message.Append("感谢您在博客园注册账号！<br/><br/>");
            message.Append("账户需要激活才能使用，赶紧激活成为博客园正式的一员吧：");
            message.Append("点击下面的链接立即激活账户（或将网址复制到浏览器中打开）：<br/><br/>");
            message.Append($"<a href='http://{host}/User/ActivateUser?code={code}&email={gotUser.Email}'>");
            message.Append($"http://{host}/User/ActivateUser?code={code}&email={gotUser.Email}");
            message.Append("</a></div>");
            emailUtil.Send(gotUser.Email, subject, message.ToString());
            emailRepository.Insert(new Email()
            {
                UserId = gotUser.Id,
                CreateTime = DateTime.Now,
                PublicKeyJson = publicKeyJson,
                PrivateKeyJson = privateKeyJson,
                ActionType = 0                      // 0，表示邮件用于注册
            });
        }

        public bool ActivateUser(string code, string emailAddress, out string message)
        {
            // 解决get不能传“+”的问题
            code = code.Replace("_", "+");
            var user = userRepository.SelectByEmail(emailAddress);
            if (user == null)
            {
                message = "用户不存在！";
                return false;
            }

            var email = emailRepository.SelectLastByUserId(user.Id, 0);

            // 使用RSA解密算法，解密code
            dataProtectorUtil.PrivateKeyJson = email.PrivateKeyJson;
            string deCode = "";
            try
            {
                deCode = dataProtectorUtil.DecryptString(code);
            }
            catch (FormatException)
            {
                message = "参数错误！";
                return false;
            }
            if (user.IsActivate)
            {
                message = "已经激活过了！";
                return false;
            }
            if (deCode.Equals(user.UserName + user.Password))
                userRepository.UpdateStatus(user.Id, 1);  // 1状态表示已经激活，且可以正常使用的状态
            else
            {
                message = "激活码错误！";
                return false;
            }
            message = "激活成功！";
            return true;
        }

        public User GetUserByUserName(string userName)
        {
            return userRepository.SelectByUserName(userName);
        }

        public User GetUserByEmail(string email)
        {
            return userRepository.SelectByEmail(email);
        }

        public IEnumerable<User> GetAll()
        {
            return userRepository.SelectAll();
        }

        public bool SendUserNameToEmail(string email, out string message)
        {
            User user = userRepository.SelectByEmail(email);
            if (user == null)
            {
                message = "该邮箱不存在！";
                return false;
            }
            var subject = $"博客园找回登陆用户名邮件-{user.DisplayName}";
            var content = new StringBuilder();
            content.Append($"<div>您好！{user.DisplayName}<br/><br/>");
            content.Append($"您的登陆用户名为：<b>{user.UserName}</b></div>");
            emailUtil.Send(user.Email, subject, content.ToString());
            message = "我们向您的登陆用户名发送到了您的邮箱中！";
            return true;
        }

        public bool SendResetPasswordEmail(string email, string publicKeyJson, string privateKeyJson, string host, out string message)
        {
            User user = userRepository.SelectByEmail(email);
            if (user == null)
            {
                message = "该邮箱不存在！";
                return false;
            }
            // 生成验证码
            dataProtectorUtil.PublicKeyJson = publicKeyJson;
            // 生成激活码，激活码由UserName字段和Password字段合并加密而成
            var code = dataProtectorUtil.EncryptString(user.UserName + user.Password);
            // 解决get不能传“+”的问题
            code = code.Replace("+", "_");
            // 发送验证邮件并记录
            var subject = $"博客园重置密码邮件-{user.DisplayName}";
            var content = new StringBuilder();
            content.Append($"<div>您好！{user.DisplayName}<br/><br/>");
            content.Append("点击下面的链接根据提醒重置密码（或将网址复制到浏览器中打开）：<br/><br/>");
            content.Append($"<a href='http://{host}/User/ResetPasswordAction?code={code}&email={user.Email}'>");
            content.Append($"http://{host}/User/ResetPasswordAction?code={code}&email={user.Email}");
            content.Append("</a></div>");
            emailUtil.Send(user.Email, subject, content.ToString());
            message = "我们向您的邮箱发送了一封邮件，根据邮件中的提示重置密码！";
            emailRepository.Insert(new Email()
            {
                UserId = user.Id,
                CreateTime = DateTime.Now,
                PublicKeyJson = publicKeyJson,
                PrivateKeyJson = privateKeyJson,
                ActionType = 1                      // 1，表示邮件用于重置密码
            });
            return true;
        }

        public bool ValidateResetPasswordCode(string code, string emailAddress, out string message)
        {
            // 解决get不能传“+”的问题
            code = code.Replace("_", "+");
            var user = userRepository.SelectByEmail(emailAddress);
            if (user == null)
            {
                message = "用户不存在！";
                return false;
            }

            var email = emailRepository.SelectLastByUserId(user.Id, 1);     // 1，表示邮件用于重置密码
            // 使用RSA解密算法，解密code
            dataProtectorUtil.PrivateKeyJson = email.PrivateKeyJson;
            string deCode = "";
            try
            {
                deCode = dataProtectorUtil.DecryptString(code);
            }
            catch (FormatException)
            {
                message = "参数错误！";
                return false;
            }
            if (deCode.Equals(user.UserName + user.Password))
            {
                message = "激活码正确！";
                return true;
            }
            message = "激活码错误！";
            return false;         
        }

        public void UpdatePassword(int id, string password)
        {
            userRepository.UpdatePassword(id, password);
        }

        public void CreateBlogApply(int userId, BlogApplyViewModel model)
        {
            BlogApply blogApply = new BlogApply();
            blogApply.UserId = userId;
            blogApply.RealName = model.RealName;
            blogApply.Reason = model.Reason;
            blogApply.Position = model.Position;
            blogApply.Unit = model.Unit;
            blogApply.Interest = model.Interest;
            blogApply.LastModifiedTime = DateTime.Now;
            blogApply.CreateTime = DateTime.Now;
            blogApply.IsRead = false;
            blogApplyRepository.Insert(blogApply);
        }

        public bool IsHadBlogApply(int userId)
        {
            IEnumerable<BlogApply> blogApplies =  blogApplyRepository.SelectByUserId(userId);
            if (blogApplies != null)
                return true;
            else
                return false;
        }

        public void AddLoginTimes(int id)
        {
            userRepository.AddLoginTimes(id);
        }

        public User GetUserById(int id)
        {
            return userRepository.SelectById(id);
        }

        public void UpdateBlogApply(int id, bool isBlogApply)
        {
            userRepository.UpdateBlogApply(id, isBlogApply);
        }
    }
}
