using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Gateway.Filters;

namespace QuizMaster.API.Gateway.Attributes
{
    public class QuizMasterAdminAuthorizationAttribute : TypeFilterAttribute
    {
        public QuizMasterAdminAuthorizationAttribute() : base(typeof(QuizMasterAdminAuthorizationFilter)) { }
    }
}
