using CnBlogs.Core.Entities;
using CnBlogs.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Services
{
    public interface IUserService
    {
        void Register(RegisterViewModel registerViewModel, string publicKeyJson, string privateKeyJson, string host);
        bool ActivateUser(string code, string email, out string message);
        User GetUserByUserName(string userName);
        User GetUserByEmail(string email);
        IEnumerable<User> GetAll();
        bool SendUserNameToEmail(string email, out string message);
        bool SendResetPasswordEmail(string email, string publicKeyJson, string privateKeyJson, string host, out string message);
        bool ValidateResetPasswordCode(string code, string email, out string message);
        void UpdatePassword(int id, string password);
        void CreateBlogApply(int userId, BlogApplyViewModel model);
        bool IsHadBlogApply(int userId);
        void AddLoginTimes(int id);
        User GetUserById(int id);
        void UpdateBlogApply(int id, bool isBlogApply);
    }
}
