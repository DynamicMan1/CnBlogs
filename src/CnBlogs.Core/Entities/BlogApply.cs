using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Entities
{
    public class BlogApply
    {
        public int Id { get; set; }                     // Id，主键，在数据库中自增
        public int UserId { get; set; }                 // 用户Id，外键关联User表中的Id字段
        public string Reason { get; set; }              // 申请理由
        public string RealName { get; set; }            // 真实姓名
        public string Position { get; set; }            // 职位
        public string Unit { get; set; }                // 单位
        public string Interest { get; set; }            // 技术兴趣
        public bool IsRead { get; set; }                // 是否查看
        public DateTime LastModifiedTime { get; set; }  // 最后修改时间
        public DateTime CreateTime { get; set; }        // 记录创建时间
    }
}
