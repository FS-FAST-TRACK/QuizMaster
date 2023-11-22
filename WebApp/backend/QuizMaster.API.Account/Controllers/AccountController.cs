using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizMaster.API.Account.Models;
using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Entities.Roles;
using QuizMaster.Library.Common.Models;
using QuizMaster.Library.Common.Models.Services;

namespace QuizMaster.API.Account.Controllers
{
	[ApiController]
	[Route("api/account")]
	public class AccountController : ControllerBase
	{

		private readonly ILogger<AccountController> _logger;
		private readonly UserManager<UserAccount> _userManager;
		private readonly RoleManager<UserRole> _roleManager;
		private readonly IMapper _mapper;

		public AccountController(ILogger<AccountController> logger, UserManager<UserAccount> userManager, RoleManager<UserRole> roleManager, IMapper mapper)
		{
			_logger = logger;
			_userManager = userManager;
			_roleManager = roleManager;
			_mapper = mapper;
		}

		[HttpGet(Name = "GetUsers")]
		public async Task<ActionResult<IEnumerable<AccountDto>>> Get()
		{
			var users = await _userManager.Users.ToListAsync();

			// Return the users
			return Ok(_mapper.Map<IEnumerable<AccountDto>>(users));
		}

		[HttpGet("{id}", Name = "GetUser")]
		public async Task<ActionResult<AccountDto>> Get(int id)
		{
			// Get user by Id
			var user = await _userManager.FindByIdAsync(id.ToString());

			// user null guard
			if (user == null)
			{
				return ReturnUserDoesNotExist();
			}

			// return converted user 
			return Ok(_mapper.Map<AccountDto>(user));
		}

		[Route("create")]
		[HttpPost]
		public async Task<IActionResult> Create(AccountCreateDto account)
		{
			// Validate ModelState
			if (!ModelState.IsValid)
			{
				return ReturnModelStateErrors();

			}

			// Check if username is already taken
			if (await _userManager.FindByNameAsync(account.UserName) != null)
			{
				return ReturnUserNameAlreadyExist();
			}

			// Check if email is already taken
			if (await _userManager.FindByEmailAsync(account.Email) != null)
			{
				return ReturnEmailAlreadyExist();
			}

			// Convert AccountCreateDto to UserAccount
			var user = _mapper.Map<UserAccount>(account);

			// Create new user
			var result = await _userManager.CreateAsync(user, account.Password);

			// Check if create succeed 
			if (!result.Succeeded)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to create user." });
			}

			// Add user as default role
			await _userManager.AddToRoleAsync(user, "user");

			// Return success
			return Ok(new ResponseDto { Type = "Success", Message = "Successfully created User", });
		}

		[Route("create-partial")]
		[HttpPost]
		public async Task<IActionResult> CreatePartial(AccountCreatePartialDto account)
		{
			// Validate ModelState
			if (!ModelState.IsValid)
			{
				return ReturnModelStateErrors();
			}

			// Check if username is already taken
			if (await _userManager.FindByNameAsync(account.UserName) != null)
			{
				return ReturnUserNameAlreadyExist();
			}

			// Check if email is already taken
			if (await _userManager.FindByEmailAsync(account.Email) != null)
			{
				return ReturnEmailAlreadyExist();
			}

			// Convert AccountCreateDto to UserAccount
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
				return ReturnUserDoesNotExist();
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
				return ReturnUserDoesNotExist();
			}

			patch.ApplyTo(userFromDb);

			if (!ModelState.IsValid)
			{
				return ReturnModelStateErrors();
			}


			var result = await _userManager.UpdateAsync(userFromDb);

			if (!result.Succeeded)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to delete user." });
			}

			return NoContent();

		}

        [HttpPost]
        [Route("set_admin/{Username}")]
        public async Task<IActionResult> SetAdmin(string Username, [FromQuery] bool SetAdmin = false)
        {
            var userAccount = await _userManager.FindByNameAsync(Username);

			if (userAccount == null)
			{
				return NotFound(new {Message = "User not found"});
			}

			IList<string> roles = await _userManager.GetRolesAsync(userAccount);

			if (roles.Contains("Administrator"))
			{
				if(SetAdmin)
				{
					return Ok(new { Message = "This user is already an administrator" });
				}
				else
				{
					var result = await _userManager.RemoveFromRoleAsync(userAccount, "Administrator");
					if (result.Succeeded)
					{
                        return Ok(new { Message = "User was removed from admin role" });
					}
					else
					{
						return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Failed to update user role" });
					}
				}
			}
            if (SetAdmin)
            {
                var result = await _userManager.AddToRoleAsync(userAccount, "Administrator");
                if (result.Succeeded)
                {
                    return Ok(new { Message = "User was set to admin role" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Failed to update user role" });
                }
            }
            else
            {
                return Ok(new { Message = "This user is not an administrator" });
            }
        }

        private IActionResult ReturnModelStateErrors()
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

		private ActionResult ReturnUserDoesNotExist()
		{
			return BadRequest(new ResponseDto
			{
				Type = "Error",
				Message = "User doesn't exist."
			});

		}

		private ActionResult ReturnUserNameAlreadyExist()
		{
			return BadRequest(new ResponseDto
			{
				Type = "Error",
				Message = "UserName already exist."
			});
		}

		private ActionResult ReturnEmailAlreadyExist()
		{
			return BadRequest(new ResponseDto
			{
				Type = "Error",
				Message = "Email already exist."
			});
		}

	}
}