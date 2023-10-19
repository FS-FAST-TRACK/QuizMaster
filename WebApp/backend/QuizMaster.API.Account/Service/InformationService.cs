using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using QuizMaster.API.Account.Proto;
using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Models.Account;
using QuizMaster.Library.Common.Models.Response;

namespace QuizMaster.API.Account.Service
{
    public class InformationService : AccountService.AccountServiceBase
    {
        private readonly UserManager<UserAccount> _userManager;

        public InformationService(UserManager<UserAccount> userManager)
        {
            _userManager = userManager;
        }

        public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            var reply = new RegisterResponse();
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            //if (user == null)
            //{
            //    return BadRequest(new ResponseDto
            //    {
            //        Type = "Error",
            //        Message = "User doesn't exist."
            //    });
            //}

            reply.Id = user.Id;
            reply.LastName = user.LastName;
            reply.FirstName = user.FirstName;
            reply.Email = user.Email;
            reply.UserName = user.UserName;
            reply.ActiveData = user.ActiveData;
            reply.DateCreated = user.DateCreated.ToString();
            reply.DateUpdated = user.DateUpdated.ToString();
            reply.UpdatedByUser = user.UpdatedByUser !=null ? user.UpdatedByUser.ToString() : "";

            return await Task.FromResult(reply);

            //return Ok(new AccountDto
            //{
            //    Id = user.Id,
            //    ActiveData = user.ActiveData,
            //    FirstName = user.FirstName,
            //    LastName = user.LastName,
            //    Email = user.Email,
            //    UserName = user.UserName,
            //    DateCreated = user.DateCreated,
            //    DateUpdated = user.DateUpdated,
            //    UpdatedByUser = user.UpdatedByUser,
            //});
        }
    }
}
