using AutoMapper;
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
        private readonly IMapper _mapper;

        public InformationService(UserManager<UserAccount> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Get account by id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>Task<RegisterResponseOrUserNotFound></returns>
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
        /// Create account
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>Task<CreateAccountReply> </returns>
        public override async Task<CreateAccountReply> CreateAccount(CreateAccountRequest request, ServerCallContext context)
        {
            var reply = new CreateAccountReply() {Type="Success", Message="Successfully created user" };

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
    }
}
