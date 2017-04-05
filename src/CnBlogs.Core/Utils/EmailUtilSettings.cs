using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Utils
{
    public class EmailUtilSettings : IEmailUtilSettings
    {
        public string Host { get; private set; }        // smtp 服务器名称
        public string Sender { get; private set; }      // 发送人邮箱
        public string Password { get; private set; }    // 发送人邮箱密码

        public EmailUtilSettings()
        {
            var configration = new ConfigurationBuilder().AddJsonFile("appGlobal.json").Build();
            Host = configration["EmailUtil.Host"];
            Sender = configration["EmailUtil.Sender"];
            Password = configration["EmailUtil.Password"];
        }
    }
}
