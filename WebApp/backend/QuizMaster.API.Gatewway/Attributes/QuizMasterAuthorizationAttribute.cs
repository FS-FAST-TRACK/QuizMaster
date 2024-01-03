using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Gateway.Filters;

namespace QuizMaster.API.Gateway.Attributes
{
    public class QuizMasterAuthorizationAttribute : TypeFilterAttribute
    {
        public QuizMasterAuthorizationAttribute() : base(typeof(QuizMasterAuthorizationFilter)) { }
    }
}
