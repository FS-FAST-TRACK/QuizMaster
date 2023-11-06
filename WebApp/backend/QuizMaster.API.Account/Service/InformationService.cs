using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using QuizMaster.API.Account.Proto;
using QuizMaster.Library.Common.Entities.Accounts;

namespace QuizMaster.API.Account.Service
{
    public class InformationService : AccountService.AccountServiceBase
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly IMapper _mapper;

        public InformationService(UserManager<UserAccount> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public override async Task<AccountOrNotFound> GetAccountById(GetAccountByIdRequest request, ServerCallContext context)
        {
            var success = new GetAccountByIdReply();
            var response = new AccountOrNotFound();

            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            if (user == null)
            {
                response.UserNotFound = new UserNotFound() { Code = "404", Message = "User not found" };
            }
            else
            {
                success.Account = JsonConvert.SerializeObject(user);
                response.GetAccountByIdReply = success;
            }

            return await Task.FromResult(response);
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="request"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns>Task</returns>
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

        /// <summary>
        /// Check if username is still available
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>Task<CheckUserNameResponse></returns>
        public override async Task<CheckUserNameResponse> CheckUserName(CheckUserNameRequest request, ServerCallContext context)
        {
            var reply = new CheckUserNameResponse();
            var user = _userManager.FindByNameAsync(request.Username).Result;
            if (user != null)
            {
                reply.IsAvailable = false;
            }
            else
            {
                reply.IsAvailable = true;
            }
            return await Task.FromResult(reply);
        }

		/// <summary>
		/// Check if username is still available
		/// </summary>
		/// <param name="request"></param>
		/// <param name="context"></param>
		/// <returns>Task<CheckUserNameResponse></returns>
		public override async Task<CheckEmailResponse> CheckEmail(CheckEmailRequest request, ServerCallContext context)
		{
			var reply = new CheckEmailResponse();
			var user = _userManager.FindByEmailAsync(request.Email).Result;
			if (user != null)
			{
				reply.IsAvailable = false;
			}
			else
			{
				reply.IsAvailable = true;
			}
			return await Task.FromResult(reply);
		}

		/// <summary>
		/// Create account
		/// </summary>
		/// <param name="request"></param>
		/// <param name="context"></param>
		/// <returns>Task<CreateAccountReply> </returns>
		public override async Task<CreateAccountReply> CreateAccount(CreateAccountRequest request, ServerCallContext context)
        {
            var reply = new CreateAccountReply() { Type = "Success", Message = "Successfully created user" };

            var userAccount = _mapper.Map<UserAccount>(request);

            var result = await _userManager.CreateAsync(userAccount, request.Password);

            if (!result.Succeeded)
            {
                reply.Type = "Error";
                reply.Message = "Failed to create user.";
            }
            else
            {
                await _userManager.AddToRoleAsync(userAccount, "user");
            }

            return await Task.FromResult(reply);
        }

        public override async Task<CreateAccountReply> CreateAccountPartial(CreateAccountPartialRquest request, ServerCallContext context)
        {
            var reply = new CreateAccountReply() { Type = "Success", Message = "Successfully created user" };

            var user = new UserAccount()
            {
                Email = request.Email,
                UserName = request.UserName,
            };

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                reply.Type = "Error";
                reply.Message = "Failed to create user.";
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "user");
            }

            return await Task.FromResult(reply);
        }

        public override async Task<DeleteAccountReply> DeleteAccount(DeleteAccountRequest request, ServerCallContext context)
        {
            var reply = new DeleteAccountReply { StatusCode = 203 };
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                reply.StatusCode = 404;
                return await Task.FromResult(reply);
            }

            user.ActiveData = false;
            user.DateUpdated = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                reply.StatusCode = 500;
            }
            return await Task.FromResult(reply);
        }

        public override async Task<UpdateAccountReply> UpdateAccount(UpdateAccountRequest request, ServerCallContext context)
        {
            var reply = new UpdateAccountReply { StatusCode = 203 };

            var user = JsonConvert.DeserializeObject<UserAccount>(request.Account);

            try
            {
                var result = await _userManager.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                reply.StatusCode = 500;
            }

            return await Task.FromResult(reply);
        }
    }
}
