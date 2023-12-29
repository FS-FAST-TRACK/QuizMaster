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
using QuizMaster.API.Quiz.ResourceParameters;
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
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            _channel = GrpcChannel.ForAddress(options.Value.Quiz_Category_Service, new GrpcChannelOptions { HttpHandler = handler });
            _channelClient = new QuizCategoryService.QuizCategoryServiceClient(_channel);
            _authChannel = GrpcChannel.ForAddress(options.Value.Authentication_Service, new GrpcChannelOptions { HttpHandler = handler });
            _authChannelClient = new AuthService.AuthServiceClient(_authChannel);
            _mapper = mapper;
        }

        /// <summary>
        /// Get all the category
        /// </summary>
        /// <returns>Task<IActionResult></returns>
        [HttpGet("get_all_category")]
        public async Task<IActionResult> GetAllCategory([FromQuery] CategoryResourceParameter resourceParameter)
        {
            // Proccess Input
            var request = new CategoryRequest()
            {
                Content = JsonConvert.SerializeObject(resourceParameter),
                Type = "getAllCategoryRequest",
            };


            // Process Logic
            if (resourceParameter.IsGetAll)
            {
                var response = await _channelClient.GetAllQuizCatagoryAsync(request);
                var categories = JsonConvert.DeserializeObject<IEnumerable<QuestionCategory>>(response.Content);

                return Ok(_mapper.Map<IEnumerable<CategoryDto>>(categories));
            }

            var pagedResponse = await _channelClient.GetCategoriesAsync(request);

            // Process output

            try
            {
                var pagedCategories= JsonConvert.DeserializeObject<Tuple<IEnumerable<CategoryDto>, Dictionary<string, object?>>>(pagedResponse.Content);
                Response.Headers.Add("X-Pagination",
                       JsonConvert.SerializeObject(pagedCategories!.Item2));

                Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");

                return Ok(pagedCategories.Item1);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        /// <summary>
        /// Get the category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task<IActionResult></returns>
        [HttpGet("get_category/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            // Process Input
            var request = new CategoryRequest
            {
                Id = id,
            };

            // Process Logic
            var response = await _channelClient.GetCategoryByIdAsync(request);

            // Process Output
            if (response.Code == 404)
            {
                return NotFound("Category not found");
            }
            return Ok(_mapper.Map<CategoryDto>(JsonConvert.DeserializeObject<QuestionCategory>(response.Content)));
            
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
            var headers = new Metadata
            {
                { "username", userName ?? "unknown" },
                { "id", userId.ToString() ?? "unknown" },
                { "role", userRole }
            };
            // Process Input
            var request = new CategoryRequest
            {
                Content = JsonConvert.SerializeObject(category),
                Type = "categoryCreateDto"
            };

            // Process Logic
            var response = await _channelClient.CreateCategoryAsync(request, headers);

            // Process Output
            if (response.Code == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.Content);
            }
            var createDifficulty = JsonConvert.DeserializeObject<QuestionCategory>(response.Content);

            return Ok(_mapper.Map<CategoryDto>(createDifficulty));
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

            // Process Input
            var request = new CategoryRequest() { Id  = id };
            
            // Process Logic
            var resposnse = await _channelClient.DeleteCategoryAsync(request, headers);

            // Process Output
            if (resposnse.Code == 404)
            {
                return ReturnCategoryDoesNotExist(id);
            }
            if (resposnse.Code == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to delete category." });
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

            // Process Input
            var request = new CategoryRequest()
            {
                Id = id,
                Content = JsonConvert.SerializeObject( patch)
            };

            // Process Logic
            var response = await _channelClient.UpdateCategoryAsync(request, headers);

            // Process Output
            if (response.Code == 404)
            {
                return ReturnCategoryDoesNotExist(id);
            }

            if (response.Code == 409)
            {
                return ReturnCategoryAlreadyExist();
            }

            if (response.Code == 500)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to update category." });
            }

            var updateCategory = JsonConvert.DeserializeObject<QuestionCategory>(response.Content);

            return Ok(_mapper.Map<CategoryDto>(updateCategory));

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
