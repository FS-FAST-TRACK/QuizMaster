﻿namespace QuizMaster.Library.Common.Models.Accounts
{
    public class ChangePasswordDTO
    {
        public string CurrentPassword { get; set;  } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
