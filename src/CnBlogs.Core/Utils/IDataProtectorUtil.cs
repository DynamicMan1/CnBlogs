using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Utils
{
    public interface IDataProtectorUtil
    {
        string PublicKeyJson { get; set; }
        string PrivateKeyJson { get; set; }
        string EncryptString(string str);
        string DecryptString(string str);
        string GetFingerprint(string str);
    }
}
