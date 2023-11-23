using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QuizMaster.API.Account.Proto;
using QuizMaster.API.Monitoring.Proto;
using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Helpers;

namespace QuizMaster.API.Account.Service
{
    public class InformationService : AccountService.AccountServiceBase
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly IMapper _mapper;
        private readonly AuditService.AuditServiceClient _auditServiceClient;

        public InformationService(UserManager<UserAccount> userManager, IMapper mapper, AuditService.AuditServiceClient auditServiceClient)
        {
            _userManager = userManager;
            _mapper = mapper;
            _auditServiceClient = auditServiceClient;
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

            // Capture the new values before updating the user
            var newValues = new Dictionary<string, string>
            {
                { "Id", userAccount.Id.ToString() },
                { "Email", userAccount.Email! },
                { "FirstName", userAccount.FirstName! },
                { "LastName", userAccount.LastName! },
                { "UserName", userAccount.UserName }
            };
            var userRole = await _userManager.GetRolesAsync(userAccount);
            var auditRegistrationEvent = new RegistrationEvent
            {
                UserId = userAccount.Id,
                Action = "User Registration",
                Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                Details = "User account successfully created",
                Userrole = userRole.FirstOrDefault(),
                OldValues = "",
                NewValues = JsonConvert.SerializeObject(newValues),

            };

            var auditRequest = new LogRegistrationEventRequest
            {
                Event = auditRegistrationEvent
            };

            try
            {
                // Make the gRPC call to log the registration event
                _auditServiceClient.LogRegistrationEvent(auditRequest);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the gRPC call and log them
                //logger.LogError("Error while logging registration event: {ErrorMessage}", ex.Message);
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
            var newValues = new Dictionary<string, string>
            {
                { "Id", user.Id.ToString() },
                { "Email", user.Email! },
                { "UserName", user.UserName }
            };
            var userRole = await _userManager.GetRolesAsync(user);

            var auditPartialRegistrationEvent = new PartialRegistrationEvent
            {
                UserId = user.Id,
                Action = "User Partial Registration",
                Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                Details = "User account partially created",
                Userrole = userRole.FirstOrDefault(),
                OldValues = "",
                NewValues = JsonConvert.SerializeObject(newValues),
            };

            var auditRequest = new LogPartialRegistrationEventRequest
            {
                Event = auditPartialRegistrationEvent,
            };
            try
            {
                // Make the gRPC call to log the registration event
                _auditServiceClient.LogPartialRegistrationEvent(auditRequest);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the gRPC call and log them
                Console.WriteLine(ex.Message);
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

            // Capture the old values before updating the user
            var oldValues = new Dictionary<string, string>
            {
                { "Id", user.Id.ToString() },
                { "FirstName", user.FirstName! },
                { "LastName", user.LastName! },
                { "Email", user.Email! },
                { "UserName", user.UserName }
            };

            // Capture the details of the user being deleted, including who deleted it
            var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
            var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            user.ActiveData = false;
            user.DateUpdated = DateTime.Now;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                var deleteEvent = new DeleteEvent
                {
                    UserId = int.Parse(userId!),
                    Action = "User Deletion",
                    Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                    Details = $"User account deleted by: {userNameClaim}",
                    Userrole = userRoles,
                    OldValues = JsonConvert.SerializeObject(oldValues),
                    NewValues = ""
                };

                var deleteRequest = new LogDeleteEventRequest
                {
                    Event = deleteEvent
                };

                try
                {
                    // Make the gRPC call to log the delete event
                    _auditServiceClient.LogDeleteEvent(deleteRequest);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during the gRPC call and log them
                    // logger.LogError("Error while logging delete event: {ErrorMessage}", ex.Message);
                }
            }
            else
            {
                reply.StatusCode = 500;
            }
            return await Task.FromResult(reply);
        }

        public override async Task<UpdateAccountReply> UpdateAccount(UpdateAccountRequest request, ServerCallContext context)
        {
            var reply = new UpdateAccountReply { StatusCode = 203 };

            var user = JsonConvert.DeserializeObject<UserAccount>(request.Account);

            // Find the existing user to capture old values
            var existingUser = await _userManager.FindByIdAsync(user!.Id.ToString());

            if (existingUser == null)
            {
                reply.StatusCode = 404;
                return await Task.FromResult(reply);
            }

            // Capture the old values before updating the user
            var oldValues = JsonConvert.SerializeObject(existingUser);

            try
            {
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    // Capture the details of the user being updated, including who updated it
                    var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
                    var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
                    var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

                    var newValues = JsonConvert.SerializeObject(user);

                    var updateEvent = new UpdateEvent
                    {
                        UserId = int.Parse(userId!),
                        Action = "User Update",
                        Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                        Details = $"User account updated by: {userNameClaim}",
                        Userrole = userRoles,
                        OldValues = oldValues,
                        NewValues = newValues
                    };

                    var updateRequest = new LogUpdateEventRequest
                    {
                        Event = updateEvent
                    };

                    // Make the gRPC call to log the update event
                    _auditServiceClient.LogUpdateEvent(updateRequest);
                }
                else
                {
                    reply.StatusCode = 500;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the update
                reply.StatusCode = 500;
            }

            return await Task.FromResult(reply);
        }

        public override async Task<SetAccountAdminResponse> SetAdminAccount(SetAccountAdminRequest request, ServerCallContext context)
        {
            var reply = new SetAccountAdminResponse();

            var userAccount = await _userManager.FindByNameAsync(request.Username);

            if (userAccount == null)
            {
                reply.Code = 404;
                reply.Message = "User not found";
                return await Task.FromResult(reply);
            }

            IList<string> roles = await _userManager.GetRolesAsync(userAccount);

            // Capture the old values before updating the user's roles
            var oldValues = new Dictionary<string, string>
            {
                { "UserId", userAccount.Id.ToString() },
                { "Roles", string.Join(",", roles) }
            };

            try
            {
                if (roles.Contains("Administrator"))
                {
                    if (request.SetAdmin)
                    {
                        reply.Code = 200;
                        reply.Message = "This user is already an administrator";
                        return await Task.FromResult(reply);
                    }
                    else
                    {
                        var result = await _userManager.RemoveFromRoleAsync(userAccount, "Administrator");
                        if (result.Succeeded)
                        {
                            // Capture the new values after removing the role
                            var newValues = new Dictionary<string, string>
                            {
                                { "UserId", userAccount.Id.ToString() },
                                { "Roles", string.Join(",", await _userManager.GetRolesAsync(userAccount)) }
                            };

                            reply.Code = 200;
                            reply.Message = "User was removed from admin role";

                            // Log the set admin event
                            LogSetAdminEvent(false, oldValues, newValues, context);
                            return await Task.FromResult(reply);
                        }
                        else
                        {
                            reply.Code = 500;
                            reply.Message = "Failed to update user role";
                            return await Task.FromResult(reply);
                        }
                    }
                }

                if (request.SetAdmin)
                {
                    var result = await _userManager.AddToRoleAsync(userAccount, "Administrator");
                    if (result.Succeeded)
                    {
                        // Capture the new values after adding the role
                        var newValues = new Dictionary<string, string>
                        {
                            { "UserId", userAccount.Id.ToString() },
                            { "Roles", string.Join(",", await _userManager.GetRolesAsync(userAccount)) }
                        };

                        reply.Code = 200;
                        reply.Message = "User was set to admin role";

                        // Log the set admin event
                        LogSetAdminEvent(true, oldValues, newValues, context);
                        return await Task.FromResult(reply);
                    }
                    else
                    {
                        reply.Code = 500;
                        reply.Message = "Failed to update user role";
                        return await Task.FromResult(reply);
                    }
                }
                else
                {
                    reply.Code = 200;
                    reply.Message = "This user is not an administrator";
                    return await Task.FromResult(reply);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the update
                reply.Code = 500;
                reply.Message = "Failed to update user role";
                return await Task.FromResult(reply);
            }
        }

        // Helper method to log the SetAdminEvent
        private void LogSetAdminEvent(bool setAdmin, Dictionary<string, string> oldValues, Dictionary<string, string> newValues, ServerCallContext context)
        {
            var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
            var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;
            var setAdminEvent = new SetAdminEvent
            {
                UserId = int.Parse(userId!),
                Action = setAdmin ? "Set Admin" : "Remove Admin",
                Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                Details = setAdmin ? "User set to admin role" : "User removed from admin role",
                Userrole = userRoles,
                OldValues = JsonConvert.SerializeObject(oldValues),
                NewValues = JsonConvert.SerializeObject(newValues)
            };

            var setAdminRequest = new LogSetAdminEventRequest
            {
                Event = setAdminEvent
            };

            try
            {
                // Make the gRPC call to log the set admin event
                _auditServiceClient.LogSetAdminEvent(setAdminRequest);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the gRPC call and log them
                Console.WriteLine(ex.Message);
            }
        }

    }
}
