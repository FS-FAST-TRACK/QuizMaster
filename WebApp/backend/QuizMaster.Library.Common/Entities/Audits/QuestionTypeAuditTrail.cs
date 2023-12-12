﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Library.Common.Entities.Audits
{
    public class QuestionTypeAuditTrail
    {
        public int QuestionTypeAuditTrailId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Details { get; set; } = string.Empty;
        public string UserRole { get; set; } = string.Empty;

        public string OldValues { get; set; } = string.Empty;
        public string NewValues { get; set; } = string.Empty;
    }
}
