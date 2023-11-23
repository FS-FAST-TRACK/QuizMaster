using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Authentication.Configuration;
using QuizMaster.API.Authentication.Models;
using QuizMaster.API.Authentication.Services.Temp;
using QuizMaster.API.Authentication.Services.Worker;
using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Models;
using QuizMaster.Library.Common.Utilities;
using System.Text.Json;

namespace QuizMaster.API.Authentication.Services.Auth
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly ILogger<AuthenticationServices> _logger;
        private readonly AppSettings appSettings;
        private readonly RabbitMqUserWorker rabbitMqUserWorker;
        public AuthenticationServices(IOptions<AppSettings> options, RabbitMqUserWorker rabbitMqUserWorker, ILogger<AuthenticationServices> _logger)
        {
            appSettings = options.Value;
            this.rabbitMqUserWorker = rabbitMqUserWorker;
            this._logger = _logger;
        }

        public async Task<AuthResponse> Authenticate(AuthRequest authRequest)
        {
            RabbitMQ_AccountPayload retrieveUserInformation = new() { Account = new UserAccount{ Id = -1 }, Roles = new List<string>() };
            int tries = 1;
            while(retrieveUserInformation.Account.Id == -1 && (tries++ < 15))
            {
                retrieveUserInformation = await rabbitMqUserWorker.RequestUserCredentials(new Library.Common.Models.Services.AuthRequest { Username = authRequest.Username, Email = authRequest.Email, Password = authRequest.Password });
            }
            /*
            UserAccount userAccount = repository.GetUserByUsername(authRequest.Username);

            if (userAccount.Id == -1) { userAccount = repository.GetUserByEmail(authRequest.Email); }
            if (userAccount.Id == -1) { return new() { Token = null }; };
            */
            if (retrieveUserInformation.Account.Id == -1) { return new() { Token = null }; };

            // attributes to store in the JWT token
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            // Create an auth store and save it in the token
            AuthStore authStore = new(retrieveUserInformation.Account, retrieveUserInformation.Roles, DateTime.Now, appSettings.IntExpireHour);

            var authStoreJson = JsonConvert.SerializeObject(authStore);
            keyValuePairs.Add("token", authStoreJson);

            // generate the token
            string jwtToken = JWTHelper.GenerateJsonWebToken(appSettings.JWTSecret, keyValuePairs);

            return new() { Token = jwtToken };
        }

        /// <summary>
        /// Validated token will always return an AuthStore object which
        /// contains the <see cref="UserAccount"/>, IssuedDate, ValidUntil and Roles
        /// </summary>
        /// <param name="token"></param>
        /// <returns><see cref="AuthStore"/></returns>
        public AuthStore? Validate(string token)
        {
            return ValidateToken(appSettings.JWTSecret, token);
        }

        /// <summary>
        /// Validated token will always return an AuthStore object which
        /// contains the <see cref="UserAccount"/>, IssuedDate, ValidUntil and Roles
        /// </summary>
        /// <param name="token"></param>
        /// <returns><see cref="AuthStore"/></returns>
        public static AuthStore? ValidateToken(string secret, string token)
        {
            IDictionary<string, string> keyValuePairs = JWTHelper.DecodeJsonWebToken(secret,token);

            if(!keyValuePairs.TryGetValue("token", out var authStoreJson)) { return null; }

            return JsonConvert.DeserializeObject<AuthStore>(authStoreJson);
        }

        public async Task<ResponseDto> UpdateRole(AuthRequest authRequest, bool SetAdmin)
        {
            RabbitMQ_AccountPayload retrieveUserInformation = new() { Account = new UserAccount { Id = -1 }, Roles = new List<string>() };
            int tries = 1;

            // flush data
            for(int it = 0; it < 3; it++, await rabbitMqUserWorker.RequestUserCredentials(new Library.Common.Models.Services.AuthRequest { Type = "RoleUpdate", Username = authRequest.Username, Email = authRequest.Email, Password = authRequest.Password, IsAdmin = SetAdmin }));

            // retrieve from message bus
            while (retrieveUserInformation.Account.Id == -1 && (tries++ < 5))
            {
                retrieveUserInformation = await rabbitMqUserWorker.RequestUserCredentials(new Library.Common.Models.Services.AuthRequest { Type = "RoleUpdate", Username = authRequest.Username, Email = authRequest.Email, Password = authRequest.Password, IsAdmin= SetAdmin });
            }

            _logger.LogInformation("New Data: " + JsonConvert.SerializeObject(retrieveUserInformation));
            
            // if not found
            if(retrieveUserInformation.Account.Id == -1)
            {
                return new ResponseDto { Message = $"Failed to update user admin credentials, not found in database... Please try again", Type = "Role Update" };
            }

            if (retrieveUserInformation.Roles.Contains("Administrator"))
            {
                return new ResponseDto { Message = $"User[{retrieveUserInformation.Account.UserName}] is now an admin", Type = "Role Update" };
            }
            return new ResponseDto { Message = $"User[{retrieveUserInformation.Account.UserName}] was revoked from admin", Type = "Role Update" };
        }
    }
}
