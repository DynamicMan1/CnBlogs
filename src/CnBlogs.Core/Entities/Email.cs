using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Entities
{
    public class Email
    {
        public int Id { get; set; }                     // Id，主键，在数据库中自增
        public int UserId { get; set; }                 // 关联的用户Id
        public DateTime CreateTime { get; set; }        // 记录创建时间
        public string PrivateKeyJson { get; set; }      // 用户发送邮件对话时的私钥（JSON格式）
        public string PublicKeyJson { get; set; }       // 用户发送邮件对话时的公钥（JSON格式）
        public int ActionType { get; set; }             // 邮件所用于的用户动作，0为注册，1为重置密码
    }
}
