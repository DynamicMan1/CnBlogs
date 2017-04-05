using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Utils
{
    public interface ICaptchaCodeUtil
    {
        MemoryStream Create(out string code, int numbers = 6);
    }
}
