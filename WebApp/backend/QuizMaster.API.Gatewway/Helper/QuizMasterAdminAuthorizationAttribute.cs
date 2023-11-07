using Microsoft.AspNetCore.Mvc;

namespace QuizMaster.API.Gateway.Helper
{
    public class QuizMasterAdminAuthorizationAttribute: TypeFilterAttribute
    {
        public QuizMasterAdminAuthorizationAttribute() : base(typeof(QuizMasterAdminAuthorizationFilter)) { }
    }
}
