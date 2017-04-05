using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.JsonResultModel
{
    public class BlogApplyResult
    {
        public bool IsSuccess { get; set; }

        public bool IsValidReason { get; set; }
        public bool IsVaildRealName { get; set; }
        public bool IsVaildPosition { get; set; }
        public bool IsVaildUnit { get; set; }
        public bool IsVaildInterest { get; set; }

        public string ReasonErrorMessage { get; set; }
        public string RealNameErrorMessage { get; set; }
        public string PositionErrorMessage { get; set; }
        public string UnitErrorMessage { get; set; }
        public string InterestErrorMessage { get; set; }
    }
}
