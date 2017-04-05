using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Utils
{
    public interface IEmailUtilSettings
    {
        string Host { get; }        // smtp 服务器名称
        string Sender { get; }      // 发送人邮箱
        string Password { get; }    // 发送人邮箱密码
    }
}
