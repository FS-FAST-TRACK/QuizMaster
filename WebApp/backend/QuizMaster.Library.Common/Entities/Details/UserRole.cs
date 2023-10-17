using System.Runtime.Serialization;

namespace QuizMaster.Library.Common.Entities.Details
{
    public enum UserRole
    {
        [EnumMember(Value = "User")]
        USER,
        [EnumMember(Value = "Administrator")]
        ADMIN,
    }
}
