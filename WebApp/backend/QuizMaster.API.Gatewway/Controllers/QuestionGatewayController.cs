﻿using AutoMapper;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Gateway.Configuration;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.Quiz.ResourceParameters;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Helpers.Quiz;
using System.Threading.Channels;

namespace QuizMaster.API.Gateway.Controllers
{
    [Route("api/gateway/question")]
    public class QuestionGatewayController : Controller
    {
        private readonly IMapper _mapper;
        private readonly GrpcChannel _channel;
        private readonly QuestionServices.QuestionServicesClient _channelClient;

        public QuestionGatewayController(IMapper mapper, IOptions<GrpcServerConfiguration> options)
        {
            _mapper = mapper;
            _channel = GrpcChannel.ForAddress(options.Value.Quiz_Category_Service);
            _channelClient = new QuestionServices.QuestionServicesClient(_channel);
            _mapper = mapper;
        }

        [HttpGet("get_questions")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestions([FromQuery] QuestionResourceParameter resourceParameter)
        {
            var request = new QuestionRequest
            {
                Parameter = JsonConvert.SerializeObject(resourceParameter)
            };

            var response = await _channelClient.GetQuestionAsync(request);

            var questions = JsonConvert.DeserializeObject<PagedList<Question>>(response.Questions);

            var paginationMetadata = new Dictionary<string, object?>
                {
                    { "totalCount", questions.TotalCount },
                    { "pageSize", questions.PageSize },
                    { "currentPage", questions.CurrentPage },
                    { "totalPages", questions.TotalPages },
                    { "previousPageLink", questions.HasPrevious ?
                        Url.Link("GetQuestions", resourceParameter.GetObject("prev"))
                        : null },
                    { "nextPageLink", questions.HasNext ?
                        Url.Link("GetQuestions", resourceParameter.GetObject("next"))
                        : null }
                };

            Response.Headers.Add("X-Pagination",
                   System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

            Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");

            return Ok(_mapper.Map<IEnumerable<QuestionDto>>(questions));
        }
    }
}