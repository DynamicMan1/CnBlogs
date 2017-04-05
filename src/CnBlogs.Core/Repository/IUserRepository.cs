using CnBlogs.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Repository
{
    public interface IUserRepository
    {
        void Insert(User user);
        void DeleteById(int id);
        IEnumerable<User> SelectAll();
        User SelectById(int id);
        User SelectByUserName(string userName);
        User SelectByEmail(string email);
        void UpdateStatus(int id, int status);
        void UpdatePassword(int id, string password);
        void UpdateLoginTimes(int id, int times);
        void AddLoginTimes(int id);
        void UpdateBlogApply(int id, bool isBlogApply);
    }
}
