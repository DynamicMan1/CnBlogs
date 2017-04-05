using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Entities
{
    public class User
    {
        public int Id { get; set; }                     // Id，主键，在数据库中自增
        public string Email { get; set; }               // 邮箱
        public string UserName { get; set; }            // 登陆用户名，在表中唯一
        public string DisplayName { get; set; }         // 显示名称
        public string Password { get; set; }            // 密码
        public DateTime CreateTime { get; set; }        // 创建时间
        public DateTime LastModifiedTime { get; set; }  // 上次修改时间
        public DateTime LastLoginTime { get; set; }     // 上次登陆时间
        public int LoginTimes { get; set; }             // 登陆次数
        public bool IsActivate { get; set; }            // 激活状态，0为未进行验证，1为正常
        public bool IsBlogApply { get; set; }           // 博客申请状态，0为不可用，1为可用
    }
}
