using Microsoft.AspNetCore.Mvc;

namespace QuizMaster.API.Gateway.Helper
{
    public class QuizMasterAuthorizationAttribute : TypeFilterAttribute
    {
        public QuizMasterAuthorizationAttribute() : base(typeof(QuizMasterAuthorizationFilter)) { }
    }
}
