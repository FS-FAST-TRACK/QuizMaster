

using Grpc.Core;
using Newtonsoft.Json;
using QuizMaster.API.Authentication.Models;
using QuizMaster.API.Authentication.Proto;
using QuizMaster.API.Authentication.Services.Auth;

namespace QuizMaster.API.Authentication.Services.GRPC
{
    public class Service : AuthService.AuthServiceBase
    {
        private readonly IAuthenticationServices _authenticationServices;

        public Service(IAuthenticationServices authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }

        public override async Task<AuthenticationReply> GetAuthentication(AuthenticationRequest request, ServerCallContext context)
        {
            var requestModel = new AuthRequest()
            {
                Username = request.Username,
                Email = request.Email,
                Password = request.Password,
            };

            var tokenHolder = await _authenticationServices.Authenticate(requestModel);

            try 
            {
                var reply = new AuthenticationReply()
                { Token = tokenHolder.Token != null ? tokenHolder.Token : ""};

                return await Task.FromResult(reply);
            }
            catch(Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public override Task<ValidationReply> ValidateAuthentication(ValidationRequest request, ServerCallContext context)
        {
            var reply = new ValidationReply();
            var authStore = _authenticationServices.Validate(request.Token);

            if(authStore == null)
            {
                reply.AuthStore = "";
            }
            else
            {
                reply.AuthStore = JsonConvert.SerializeObject(authStore);
            }
            return Task.FromResult(reply);
        }

        public override async Task<SetAdminReply> SetAdmin(SetAdminRequest request, ServerCallContext context)
        {
            var reply = new SetAdminReply();
            // todo harold
            //var response = await _authenticationServices.UpdateRole(new AuthRequest { Username = request.Username }, isAdmin=true or false);
            //reply.Response = JsonConvert.SerializeObject(response);

            return await Task.FromResult(reply);
        }
    }
}
