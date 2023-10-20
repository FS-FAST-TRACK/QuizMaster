using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Authentication.Configuration;
using QuizMaster.API.Authentication.Models;
using QuizMaster.API.Authentication.Services.Temp;
using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Utilities;
using System.Text.Json;

namespace QuizMaster.API.Authentication.Services.Auth
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly IRepository repository;
        private readonly AppSettings appSettings;
        public AuthenticationServices(IRepository repository, IOptions<AppSettings> options)
        {
            this.repository = repository;
            appSettings = options.Value;
        }

        public AuthResponse Authenticate(AuthRequest authRequest)
        {
            UserAccount userAccount = repository.GetUserByUsername(authRequest.Username);

            if (userAccount.Id == -1) { userAccount = repository.GetUserByEmail(authRequest.Email); }
            if (userAccount.Id == -1) { return new() { Token = null }; };

            // intialize password hasher
            PasswordHasher<UserAccount> hasher = new();

            // check if password is correct
            var passwordVerification = hasher.VerifyHashedPassword(userAccount, userAccount.PasswordHash, authRequest.Password);
            if (PasswordVerificationResult.Success != passwordVerification) { return new() { Token = null }; };

            // attributes to store in the JWT token
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            // Create an auth store and save it in the token
            AuthStore authStore = new(userAccount, repository.GetRoles(userAccount.Id), DateTime.Now, appSettings.IntExpireHour);

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
    }
}
