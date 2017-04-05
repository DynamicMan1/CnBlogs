using CnBlogs.Core.Entities;
using CnBlogs.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Services
{
    public class BlogApplyService : IBlogApplyService
    {
        private readonly IBlogApplyRepository blogApplyRepository;
        private readonly IUserRepository userRepository;

        public BlogApplyService(IBlogApplyRepository blogApplyRepository, IUserRepository userRepository)
        {
            this.blogApplyRepository = blogApplyRepository;
            this.userRepository = userRepository;
        }

        public IEnumerable<User> GetAllApplingUsers()
        {
            HashSet<User> result = null;
            var blogApplies = blogApplyRepository.SelectAllNoReader();
            if(blogApplies.Count<BlogApply>() > 0)
            {
                result = new HashSet<User>();
                foreach (var ba in blogApplies)
                {
                    User user = userRepository.SelectById(ba.UserId);
                    result.Add(user);
                }
            }
            if(result != null)
            {
                return result.ToArray();
            }
            return result;
        }

        public IEnumerable<BlogApply> GetAllNoReaderBlogApplies()
        {
            return blogApplyRepository.SelectAllNoReader();
        }

        public IEnumerable<BlogApply> GetAllBlogApply()
        {
            return blogApplyRepository.SelectAll();
        }

        public BlogApply SelectById(int id)
        {
            return blogApplyRepository.SelectById(id);
        }

        public void HadRead(int id)
        {
            blogApplyRepository.UpdateIsRead(id, true);
        }
    }
}
