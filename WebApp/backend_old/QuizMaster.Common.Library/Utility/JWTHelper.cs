using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;

namespace QuizMaster.Library.Common.Utility
{
	/*
     * By Jayharron, do some checks before using this helper class
     */
	public class JWTHelper
	{

		/*
         * Helper method, for generating JsonWebToken
         * 
         * install Nuget System.IdentityModel.Tokens.Jwt
         */

		public static string GenerateJsonWebToken(string secretKey, IDictionary<string, string> payload)
		{
			// instantiate the jwt security token
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(secretKey);

			// make the properties of the payload to be in the claim identity, parsing payload to the token
			List<Claim> claims = new List<Claim>();
			payload.ToList().ForEach(p => { claims.Add(new Claim(p.Key, p.Value)); });

			var tokenDescriptor = new SecurityTokenDescriptor
			{

				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}

		/*
         * Helper method that will decode JWToken back to payload
         */
		public static IDictionary<string, string> DecodeJsonWebToken(string secretKey, string jsonWebToken)
		{
			// instantiate the jwt security token
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(secretKey);

			tokenHandler.ValidateToken(jsonWebToken, new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = false,
				ValidateAudience = false,
				// set clock skew to zero so token expire exactly at token expiration time
				ClockSkew = TimeSpan.Zero
			}, out SecurityToken validatedToken);

			var jwtToken = (JwtSecurityToken)validatedToken;
			Dictionary<string, string> payload = new Dictionary<string, string>();

			jwtToken.Claims.ToList().ForEach(c => payload.Add(c.Type, c.Value));

			return payload;
		}
	}
}