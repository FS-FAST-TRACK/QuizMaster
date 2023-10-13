using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.Account.Api.Models;
using QuizMaster.Account.Api.Repositories;
using QuizMaster.Common.Library.Entities;
using QuizMaster.Common.Library.Roles;
using QuizMaster.Common.Library.Utility;
using QuizMaster.Library.Common.Utility;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuizMaster.Account.Api.Controllers
{
	[Route("api/account")]
	[ApiController]
	[Authorize]
	public class AccountController : ControllerBase
	{
		private readonly IQuizMasterRepository _quizMasterRepository;
		private readonly IConfiguration _configuration;
		private readonly IMapper _mapper;


		public AccountController(IQuizMasterRepository quizMasterRepository, IConfiguration configuration, IMapper mapper)
		{
			_quizMasterRepository = quizMasterRepository;
			_configuration = configuration;
			_mapper = mapper;
		}

		[Route("login", Name = "Login")]
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody] UserForLoginDto userCredentials)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState.ValidationState);
			try
			{
				var user = await _quizMasterRepository.GetUserAsync(userCredentials.UserName);

				if (user == null)
				{
					return BadRequest(new ResponseDto { Status = "Error", Message = $"User doesn't exist" });

				}

				if (user.Password != Hash.GetHashSha256(userCredentials.Password))
				{
					return BadRequest(new ResponseDto { Status = "Error", Message = $"Invalid Credentials" });

				}


				var authClaims = new Dictionary<string, string>
				{
					{ "id", user.Id.ToString() },
					{ "username", user.UserName },
					{ "firstName", user.FirstName },
					{ "lastName", user.LastName },
					{ "email", user.EmailAddress },
					{ "role", user.UserRole.UserRoleDesc },
				};

				var secretKey = _configuration["JWT:Secret"]!;
				var token = JWTHelper.GenerateJsonWebToken(secretKey, authClaims);
	
				return Ok(new
				{
					token,
				});
			}
			catch (Exception ex)
			{

				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}


		}


		[Route("signup")]
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Register([FromBody] UserForRegisterDto newUser)
		{
			try
			{
				var userFromRepo = await _quizMasterRepository.IsUserExistsByUserNameAsync(newUser.UserName);
				if (userFromRepo)
				{
					return BadRequest(new ResponseDto { Status = "Error", Message = $"Username {newUser.UserName} is already taken." });
				}

				var user = _mapper.Map<UserAccount>(newUser);

				user.DateCreated = DateTime.UtcNow;
				user.Password = Hash.GetHashSha256(newUser.Password);

				await _quizMasterRepository.AddUserAsync(user);



				await _quizMasterRepository.SaveChangesAsync();

				return CreatedAtRoute("Login", _mapper.Map<UserForLoginDto>(newUser));
			}
			catch (Exception ex)
			{

				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}


		}

		[HttpPut("{id}")]
		[Authorize]
		public async Task<ActionResult<UserDto>> UpdateUserProfile(int id, [FromBody] UserForUpdateDto user)
		{
			try
			{
				var updaterUserNameClaim = User.Claims.Where((claim) => claim.Type == "username").FirstOrDefault();

				if (updaterUserNameClaim == null)
				{
					return BadRequest(new ResponseDto
					{
						Status = "Error",
						Message = "Claim for username is not found"
					});
				}

				var updaterUserName = updaterUserNameClaim.Value;

				var updaterUser = await _quizMasterRepository.GetUserAsync(updaterUserName);

				if (updaterUser == null)
				{
					return BadRequest(new ResponseDto
					{
						Status = "Error",
						Message = "Username of updater is not found."
					});
				}


				var userFromRepo = await _quizMasterRepository.GetUserAsync(id);

				if (userFromRepo == null)
				{
					return BadRequest(new ResponseDto
					{
						Status = "Error",
						Message = "User doesn't exist."
					});
				}


				// Prevent other users (with only user role) from updating other user profile
				// Users (with user role) are forbidden to update other user profile
				// User can only update their own profile, admins can update their profile as well as other profiles
				if (updaterUser.UserRole.UserRoleDesc != "Admin" && updaterUserName != userFromRepo.UserName)
				{
					return Forbid();
				}
				_mapper.Map(user, userFromRepo);
				var isUserExist = await _quizMasterRepository.IsUserExistsAsync(userFromRepo);
				if (await _quizMasterRepository.IsUserExistsAsync(userFromRepo))
				{
					return BadRequest(new ResponseDto { Status = "Error", Message = $"Username {user.UserName} is already taken." });
				}

				userFromRepo.UpdatedByUser = updaterUser;

				userFromRepo.DateUpdated = DateTime.UtcNow;


				_quizMasterRepository.UpdateUser(userFromRepo);

				await _quizMasterRepository.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}


			return NoContent();

		}

		[Route("upgrade-to-admin/{id}")]
		[HttpPut]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpgradeUserRole(int id)
		{
			var updaterUserNameClaim = User.Claims.Where((claim) => claim.Type == "username").FirstOrDefault();

			if (updaterUserNameClaim == null)
			{
				return BadRequest(new ResponseDto
				{
					Status = "Error",
					Message = "Claim for username is not found"
				});
			}

			var updaterUserName = updaterUserNameClaim.Value;

			var updaterUser = await _quizMasterRepository.GetUserAsync(updaterUserName);

			if (updaterUser == null)
			{
				return BadRequest(new ResponseDto
				{
					Status = "Error",
					Message = "Username of updater is not found."
				});
			}

			var userFromRepo = await _quizMasterRepository.GetUserAsync(id);

			if (userFromRepo == null)
			{
				return BadRequest(new ResponseDto
				{
					Status = "Error",
					Message = "User doesn't exist."
				});
			}

			if (userFromRepo.UserRole.UserRoleDesc == "Admin")
			{
				return BadRequest(new ResponseDto
				{
					Status = "Error",
					Message = "User is already admin."
				});
			}

			userFromRepo.UserRole = RolesData.Admin;

			userFromRepo.UpdatedByUser = updaterUser;
			userFromRepo.DateUpdated = DateTime.UtcNow;

			_quizMasterRepository.UpdateUser(userFromRepo);

			return Ok();
		}




		[Authorize(Roles = "Admin, User")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Detete(int id)
		{

			var userFromRepo = await _quizMasterRepository.GetUserAsync(id);

			if (userFromRepo == null)
			{
				return BadRequest(new ResponseDto
				{
					Status = "Error",
					Message = "User doesn't exist"
				});
			}

			userFromRepo.ActiveData = false;
			userFromRepo.DateUpdated = DateTime.Now;

			_quizMasterRepository.UpdateUser(userFromRepo);
			await _quizMasterRepository.SaveChangesAsync();

			return NoContent();
		}




		/// <summary>
		/// Generates a JSON Web Token (JWT) with the provided authentication claims.
		/// </summary>
		/// <param name="authClaims">A list of claims representing the user's identity and additional information.</param>
		/// <returns>A JWT containing the specified claims, signed with a symmetric key, and configured with issuer, audience, and expiration.</returns>
		//private JwtSecurityToken GetToken(List<Claim> authClaims)
		//{
		//	var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));

		//	var token = new JwtSecurityToken(
		//		issuer: _configuration["JWT:ValidIssuer"],
		//		audience: _configuration["JWT:ValidAudience"],
		//		expires: DateTime.Now.AddHours(24),
		//		claims: authClaims,
		//		signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
		//		);

		//	return token;
		//}
	}
}
