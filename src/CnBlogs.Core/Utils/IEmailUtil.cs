using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Utils
{
    public interface IEmailUtil
    {
       void Send(string email, string subject, string message);
    }
}
