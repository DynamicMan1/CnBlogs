using CnBlogs.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Repository
{
    public interface IEmailRepository
    {
        void Insert(Email email);
        void DeleteById(int id);
        IEnumerable<Email> SelectAll();
        Email SelectById(int id);
        Email SelectLastByUserId(int userId, int actionType);
    }
}
