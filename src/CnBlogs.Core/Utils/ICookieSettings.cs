using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Utils
{
    public interface ICookieSettings
    {
        string CookiePublicKeyJson { get; }
        string CookiePrivateKeyJson { get; }

        double ExpiresValue { get; set; }
    }
}
