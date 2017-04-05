using CnBlogs.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Repository
{
    public interface IBlogApplyRepository
    {
        void Insert(BlogApply blogApply);
        void DeleteById(int id);
        IEnumerable<BlogApply> SelectAll();
        BlogApply SelectById(int id);
        IEnumerable<BlogApply> SelectAllNoReader();
        void UpdateIsRead(int id, bool isRead);
        IEnumerable<BlogApply> SelectByUserId(int userId);
    }
}
