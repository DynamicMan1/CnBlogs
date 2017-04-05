using CnBlogs.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Services
{
    public interface IBlogApplyService
    {
        IEnumerable<User> GetAllApplingUsers();
        IEnumerable<BlogApply> GetAllNoReaderBlogApplies();
        IEnumerable<BlogApply> GetAllBlogApply();
        BlogApply SelectById(int id);
        void HadRead(int id);
    }
}
