using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Protos;

namespace QuizMaster.API.Gateway.Controllers
{

    [ApiController]
    [Route("gateway/api/question/category")]
    public class QuizCategoryGatewayController : Controller
    {
        private readonly GrpcChannel _channel;
        private readonly QuizCategoryService.QuizCategoryServiceClient _channelClient;

        public QuizCategoryGatewayController()
        {
            _channel = GrpcChannel.ForAddress("https://localhost:7228");
            _channelClient = new QuizCategoryService.QuizCategoryServiceClient(_channel);
        }

        [HttpGet("get_all_category")]
        public async Task<IActionResult> GetAllCategory()
        {
            var request = new Empty();

            var response = _channelClient.GetAllQuizCatagory(request);
            var categories = new List<GetAllQuizCatagoryResponse>();
            while (await response.ResponseStream.MoveNext())
            {
                categories.Add(response.ResponseStream.Current);
            }

            return Ok(categories);
        }

        [HttpGet("get_category/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var request = new GetCategoryByIdRequest
            {
                Id = id
            };

            var response = await _channelClient.GetCategoryByIdAsync(request);

            if(response.CategoryNotFound != null)
            {
                return NotFound(response.CategoryNotFound);
            }

            return Ok(response.GetCategoryByIdReply);
        }

        [HttpPost("create_category")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto category)
        {
            var request = new CreateCategoryRequest
            {
                QuizCategoryDesc = category.QCategoryDesc
            };

            var response = await _channelClient.CreateCategoryAsync(request);
            if(response.CreateCategoryFail != null)
            {
                if(response.CreateCategoryFail.Type == "409")
                {
                    return StatusCode(409, new
                    {
                        error = "Resource Already Exists",
                        message = "The category you are trying to create already exists.",
                        details = "A category with the same description already exists in the system."
                    });
                }
                return BadRequest(response.CreateCategoryFail);
            }

            return Ok(response.CreatedCategoryReply);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var request = new DeleteCategoryRequest
            {
                Id = id
            };

            var response = await _channelClient.DeleteCategoryAsync(request);
            if(response.Response == 404)
            {
                return NotFound(new
                {
                    error = "Resource Not Found",
                    message = "The category you are trying to delete does not exist.",
                    details = "A category with the same id does not exist in the system."
                });
            }   
            else if(response.Response == 500)
            {
                return StatusCode(500, new
                {
                    error = "Internal Server Error",
                    message = "Failed to delete category.",
                });
            }

            return NoContent();
        }
    }
}
