using AutoMapper;
using Azure;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Authentication.Models;
using QuizMaster.API.Authentication.Proto;
using QuizMaster.API.Gateway.Configuration;
using QuizMaster.API.Gateway.Helper;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.Quiz.SeedData;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Models;

namespace QuizMaster.API.Gateway.Controllers
{

    [ApiController]
    [Route("gateway/api/question/category")]
    public class QuizCategoryGatewayController : Controller
    {
        private readonly GrpcChannel _channel;
        private readonly QuizCategoryService.QuizCategoryServiceClient _channelClient;
        private readonly GrpcChannel _authChannel;
        private readonly AuthService.AuthServiceClient _authChannelClient;
        private readonly IMapper _mapper;

        public QuizCategoryGatewayController(IMapper mapper, IOptions<GrpcServerConfiguration> options)
        {
            _channel = GrpcChannel.ForAddress(options.Value.Quiz_Category_Service);
            _channelClient = new QuizCategoryService.QuizCategoryServiceClient(_channel);
            _authChannel = GrpcChannel.ForAddress(options.Value.Authentication_Service);
            _authChannelClient = new AuthService.AuthServiceClient(_authChannel);
            _mapper = mapper;
        }

        /// <summary>
        /// Get all the category
        /// </summary>
        /// <returns>Task<IActionResult></returns>
        [HttpGet("get_all_category")]
        public async Task<IActionResult> GetAllCategory()
        {
            // create the request
            var request = new Empty();

            // get all the categories from the service
            var response = _channelClient.GetAllQuizCatagory(request);

            var categories = new List<GetAllQuizCatagoryResponse>();

            // iterate through the response stream
            while (await response.ResponseStream.MoveNext())
            {
                categories.Add(response.ResponseStream.Current);
            }

            return Ok(categories);
        }

        /// <summary>
        /// Get the category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task<IActionResult></returns>
        [HttpGet("get_category/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            // create the request
            var request = new GetCategoryByIdRequest
            {
                Id = id
            };

            // get the category from the service
            var response = await _channelClient.GetCategoryByIdAsync(request);

            // if category not found, return not found
            if (response.CategoryNotFound != null)
            {
                return NotFound(response.CategoryNotFound);
            }

            return Ok(response.GetCategoryByIdReply);
        }

        /// <summary>
        /// Create category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>Task<IActionResult></returns>
        [QuizMasterAuthorization]
        [HttpPost("create_category")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto category)
        {
            var info = await ValidateUserTokenAndGetInfo();

            if (info == null)
            {
                return NotFound(new { Message = "Invalid user information in the token" });
            }

            if (info == null || info.UserData == null)
            {
                return NotFound(new { Message = "Invalid user information in the token" });
            }

            var userName = info.UserData.UserName;
            var userId = info.UserData.Id;
            var userRole = info.Roles.Any(h => h.Equals("Administrator")) ? "Administrator" : "User";

            // create the request
            var request = new CreateCategoryRequest
            {
                QuizCategoryDesc = category.QCategoryDesc
            };
            var headers = new Metadata
            {
                { "username", userName ?? "unknown" },
                { "id", userId.ToString() ?? "unknown" },
                { "role", userRole }
            };

            // check if category already exist
            var response = await _channelClient.CreateCategoryAsync(request, headers);

            // if the server response is other than 200
            if (response.CreateCategoryFail != null)
            {
                // if the category already exist
                if (response.CreateCategoryFail.Type == "409")
                {
                    return StatusCode(409, new
                    {
                        error = "Resource Already Exists",
                        message = "The category you are trying to create already exists.",
                        details = "A category with the same description already exists in the system."
                    });
                }
                // if the service failed to create the category
                return BadRequest(response.CreateCategoryFail);
            }

            return Ok(response.CreatedCategoryReply);
        }

        /// <summary>
        /// Delete category
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task<IActionResult></returns>
        [QuizMasterAuthorization]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var info = await ValidateUserTokenAndGetInfo();

            if (info == null)
            {
                return NotFound(new { Message = "Invalid user information in the token" });
            }

            if (info == null || info.UserData == null)
            {
                return NotFound(new { Message = "Invalid user information in the token" });
            }

            var userName = info.UserData.UserName;
            var userId = info.UserData.Id;
            var userRole = info.Roles.Any(h => h.Equals("Administrator")) ? "Administrator" : "User";

            var headers = new Metadata
            {
                { "username", userName ?? "unknown" },
                { "id", userId.ToString() ?? "unknown" },
                { "role", userRole }
            };

            // create the request to check if category exist
            var request = new DeleteCategoryRequest
            {
                Id = id
            };

            // call the service to check if category exist
            var response = await _channelClient.DeleteCategoryAsync(request, headers);

            // if category does not exist
            if (response.Response == 404)
            {
                return NotFound(new
                {
                    error = "Resource Not Found",
                    message = "The category you are trying to delete does not exist.",
                    details = "A category with the same id does not exist in the system."
                });
            }

            // if the service failed to delete the category
            else if (response.Response == 500)
            {
                return StatusCode(500, new
                {
                    error = "Internal Server Error",
                    message = "Failed to delete category.",
                });
            }

            return NoContent();
        }

        /// <summary>
        /// Update category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patch"></param>
        /// <returns>Task<IActionResult></returns>
        [QuizMasterAuthorization]
        [HttpPatch("update_category/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, JsonPatchDocument<CategoryCreateDto> patch)
        {
            var info = await ValidateUserTokenAndGetInfo();

            if (info == null)
            {
                return NotFound(new { Message = "Invalid user information in the token" });
            }

            if (info == null || info.UserData == null)
            {
                return NotFound(new { Message = "Invalid user information in the token" });
            }

            var userName = info.UserData.UserName;
            var userId = info.UserData.Id;
            var userRole = info.Roles.Any(h => h.Equals("Administrator")) ? "Administrator" : "User";
            var headers = new Metadata
            {
                { "username", userName ?? "unknown" },
                { "id", userId.ToString() ?? "unknown" },
                { "role", userRole }
            };
            // create the request to check if category exist
            var request = new CheckCategoryOrActiveRequest
            {
                Id = id
            };

            // call service to check if category exist or is it still active
            var result = await _channelClient.CheckCategoryOrActiveAsync(request);
            var categoryFromRepo = result.CheckCategoryOrActiveFail == null ?
                JsonConvert.DeserializeObject<QuestionCategory>(result.CheckCategoryOrActiveReply.Category)
                : null;

            // if category does not exist or is not active
            if (result.CheckCategoryOrActiveFail != null || !categoryFromRepo.ActiveData)
            {
                return ReturnCategoryDoesNotExist(id);
            }

            // create the CategoryCreateDto to update category
            var categoryForPatch = new CategoryCreateDto();
            patch.ApplyTo(categoryForPatch);

            if(!TryValidateModel(categoryForPatch))
            {
                return ReturnModelStateErrors();
            }   

            // create the request to check if category already exist
            var checkCategory = new CheckCategoryNameRequest 
            { QuizCategoryDesc = categoryForPatch.QCategoryDesc };

            // call service if category already exist
            var checkCategoryResult = await _channelClient.CheckCategoryNameAsync(checkCategory);

            // if category already exist
            if(checkCategoryResult.Status == "200")
            {
                return ReturnCategoryAlreadyExist();
            }

            // map the CategoryCreateDto to QuestionCategory
            _mapper.Map(categoryForPatch, categoryFromRepo);

            // create request to update category
            var update = new UpdateCategoryRequest
            {
                Category = JsonConvert.SerializeObject(categoryFromRepo),
            };

            // call service to update category
            var updateResult = await _channelClient.UpdateCategoryAsync(update,headers);

            // if service failed to update category
            if(updateResult.StatusCode == 500)
            {
                return StatusCode(500, new
                {
                    error = "Internal Server Error",
                    message = "Failed to update category.",
                });
            }  

            return Ok(new { id = updateResult.Id, questionCategoryDesc = updateResult.QuizCategoryDesc });
        }

        // return if category does not exist
        private ActionResult ReturnCategoryDoesNotExist(int id)
        {
            return BadRequest(new ResponseDto
            {
                Type = "Error",
                Message = $"Category with id {id} doesn't exist."
            });

        }
        // return if model state is invalid
        private ActionResult ReturnModelStateErrors()
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
        // return if category already exist
        private ActionResult ReturnCategoryAlreadyExist()
        {
            return BadRequest(new ResponseDto
            {
                Type = "Error",
                Message = "Category already exist."
            });
        }
        private async Task<AuthStore> ValidateUserTokenAndGetInfo()
        {
            string token = GetUserToken();

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var info = await GetAuthStoreInfo(token);

            if (info == null || info.UserData == null)
            {
                return null;
            }

            return info;
        }

        private string GetUserToken()
        {
            var tokenClaim = User.Claims.FirstOrDefault(e => e.Type == "token");

            if (tokenClaim != null)
            {
                return tokenClaim.Value;
            }

            try
            {
                return HttpContext.Request.Headers.Authorization.ToString().Split(" ")[1];
            }
            catch
            {
                return null;
            }
        }

        private async Task<AuthStore> GetAuthStoreInfo(string token)
        {
            var requestValidation = new ValidationRequest()
            {
                Token = token
            };

            var authStore = await _authChannelClient.ValidateAuthenticationAsync(requestValidation);

            return !string.IsNullOrEmpty(authStore?.AuthStore) ? JsonConvert.DeserializeObject<AuthStore>(authStore.AuthStore) : null;
        }

    }
}
