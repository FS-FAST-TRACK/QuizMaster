using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizMaster.API.Account.Proto;
using QuizMaster.Library.Common.Entities.Accounts;

namespace QuizMaster.API.Account.Service
{
    public class InformationService : AccountService.AccountServiceBase
    {
        private readonly UserManager<UserAccount> _userManager;

        public InformationService(UserManager<UserAccount> userManager)
        {
            _userManager = userManager;
        }

        public override async Task<RegisterResponseOrUserNotFound> Register(RegisterRequest request, ServerCallContext context)
        {
            var success = new RegisterResponse();
            var error = new UserNotFound();
            var response = new RegisterResponseOrUserNotFound();

            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                error.Code = "404";
                error.Message = "User not found";

                response.UserNotFound = error;
            }
            else
            {
                success.Id = user.Id;
                success.LastName = user.LastName != null ? user.LastName : "";
                success.FirstName = user.FirstName != null ? user.FirstName : "";
                success.Email = user.Email;
                success.UserName = user.UserName;
                success.ActiveData = user.ActiveData;
                success.DateCreated = user.DateCreated.ToString();
                success.DateUpdated = user.DateUpdated != null ? user.DateUpdated.ToString() : "";
                success.UpdatedByUser = user.UpdatedByUser != null ? user.UpdatedByUser.ToString() : "";

                response.RegisterResponse = success;
            }

            return await Task.FromResult(response);
        }

        public override async Task GetAllUsers(Empty request, IServerStreamWriter<AllUserReply> responseStream, ServerCallContext context)
        {
            var reply = new AllUserReply();
            foreach (var user in _userManager.Users.ToArray())
            {
                reply.Id = user.Id;
                reply.LastName = user.LastName != null ? user.LastName : "";
                reply.FirstName = user.FirstName != null ? user.FirstName : "";
                reply.Email = user.Email;
                reply.UserName = user.UserName;
                reply.ActiveData = user.ActiveData;
                reply.DateCreated = user.DateCreated.ToString();
                reply.DateUpdated = user.DateUpdated != null ? user.DateUpdated.ToString() : "";
                reply.UpdatedByUser = user.UpdatedByUser != null ? user.UpdatedByUser.ToString() : "";

                await responseStream.WriteAsync(reply);
            }
        }

    }
}
