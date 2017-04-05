using CnBlogs.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.ViewModels
{
    public class ManagementCenterViewModel
    {
        public IEnumerable<User> Users { get; set; }
    }
}
