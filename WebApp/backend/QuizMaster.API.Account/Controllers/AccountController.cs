using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Entities.Roles;
using QuizMaster.Library.Common.Models.Account;
using QuizMaster.Library.Common.Models.Response;

namespace QuizMaster.API.Account.Controllers
{
	[ApiController]
	[Route("api/account")]
	public class AccountController : ControllerBase
	{

		private readonly ILogger<AccountController> _logger;
		private readonly UserManager<UserAccount> _userManager;
		private readonly RoleManager<UserRole> _roleManager;

		public AccountController(ILogger<AccountController> logger, UserManager<UserAccount> userManager, RoleManager<UserRole> roleManager)
		{
			_logger = logger;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		[HttpGet(Name = "GetUsers")]
		public async Task<ActionResult<IEnumerable<AccountDto>>> Get()
		{
			var users = await _userManager.Users.ToListAsync();
			IEnumerable<AccountDto> accountDtos = users.Select(user => new AccountDto
			{
				Id = user.Id,
				ActiveData = user.ActiveData,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				UserName = user.UserName,
				DateCreated = user.DateCreated,
				DateUpdated = user.DateUpdated,
				UpdatedByUser = user.UpdatedByUser,
			});
			return Ok(accountDtos);
		}

		[HttpGet("{id}", Name = "GetUser")]
		public async Task<ActionResult<AccountDto>> Get(int id)
		{
			var user = await _userManager.FindByIdAsync(id.ToString());
			if (user == null)
			{
				return BadRequest(new ResponseDto
				{
					Type = "Error",
					Message = "User doesn't exist."
				});
			}
			return Ok(new AccountDto
			{
				Id = user.Id,
				ActiveData = user.ActiveData,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				UserName = user.UserName,
				DateCreated = user.DateCreated,
				DateUpdated = user.DateUpdated,
				UpdatedByUser = user.UpdatedByUser,
			});
		}

		[Route("create")]
		[HttpPost]
		public async Task<IActionResult> Create(AccountCreateDto account)
		{
			if (!ModelState.IsValid)
			{
				var errorList = ModelState.Values
									.SelectMany(v => v.Errors)
									.Select(e => e.ErrorMessage)
									.ToList();

				var errorString = string.Join(", ", errorList);

				return BadRequest(new ResponseDto
				{
					Type = "Error",
					Message = errorString
				});

			}

			if (await _userManager.FindByNameAsync(account.UserName) != null)
			{
				return BadRequest(new ResponseDto
				{
					Type = "Error",
					Message = "UserName already exist."
				});
			}

			UserAccount user = new UserAccount()
			{
				FirstName = account.FirstName,
				LastName = account.LastName,
				Email = account.Email,
				SecurityStamp = Guid.NewGuid().ToString(),
				UserName = account.UserName,
			};


			var result = await _userManager.CreateAsync(user, account.Password);

			if (!result.Succeeded)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to create user." });
			}

			await _userManager.AddToRoleAsync(user, "user");

			return Ok(new ResponseDto { Type = "Success", Message = "Successfully created User", });
		}

		[Route("create-partial")]
		[HttpPost]
		public async Task<IActionResult> CreatePartial(AccountCreatePartialDto account)
		{
			if (!ModelState.IsValid)
			{
				var errorList = ModelState.Values
									.SelectMany(v => v.Errors)
									.Select(e => e.ErrorMessage)
									.ToList();

				var errorString = string.Join(", ", errorList);

				return BadRequest(new ResponseDto
				{
					Type = "Error",
					Message = errorString
				});

			}

			if (await _userManager.FindByNameAsync(account.UserName) != null)
			{
				return BadRequest(new ResponseDto
				{
					Type = "Error",
					Message = "UserName already exist."
				});
			}

			UserAccount user = new UserAccount()
			{

				Email = account.Email,
				UserName = account.UserName,
			};


			var result = await _userManager.CreateAsync(user);

			if (!result.Succeeded)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to create user." });
			}

			await _userManager.AddToRoleAsync(user, "user");

			return Ok(new ResponseDto { Type = "Success", Message = "Successfully created User", });
		}


		[HttpDelete("delete/{id}")]
		public async Task<IActionResult> Delete(int id)
		{

			var user = await _userManager.FindByIdAsync(id.ToString());
			if (user == null)
			{
				return BadRequest(new ResponseDto
				{
					Type = "Error",
					Message = "User doesn't exist."
				});
			}

			user.ActiveData = false;
			user.DateUpdated = DateTime.Now;

			var result = await _userManager.UpdateAsync(user);

			if (!result.Succeeded)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to delete user." });
			}

			return NoContent();
		}

		[HttpPatch("update/{id}")]
		public async Task<IActionResult> Update(int id, JsonPatchDocument<UserAccount> patch)
		{

			var userFromDb = await _userManager.FindByIdAsync(id.ToString());
			if (userFromDb == null)
			{
				return BadRequest(new ResponseDto
				{
					Type = "Error",
					Message = "User doesn't exist."
				});
			}

			patch.ApplyTo(userFromDb);

			if (!ModelState.IsValid)
			{
				var errorList = ModelState.Values
									.SelectMany(v => v.Errors)
									.Select(e => e.ErrorMessage)
									.ToList();

				var errorString = string.Join(", ", errorList);

				return BadRequest(new ResponseDto
				{
					Type = "Error",
					Message = errorString
				});

			}


			var result = await _userManager.UpdateAsync(userFromDb);

			if (!result.Succeeded)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to delete user." });
			}

			return NoContent();

		}


	}
}