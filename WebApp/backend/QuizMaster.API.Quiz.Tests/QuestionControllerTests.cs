using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using QuizMaster.API.Quiz.Controllers;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Profiles;
using QuizMaster.API.Quiz.ResourceParameters;
using QuizMaster.API.Quiz.Tests.Services;
using System.Text.Json;

namespace QuizMaster.API.Quiz.Tests
{
	public class QuestionControllerTests
	{
		[Fact]
		public async Task Get_Questions_MustReturnEmpty()
		{
			// Arrange
			var quizTestDataRepository = new QuizTestDataRepository();
			var mapperConfiguration = new MapperConfiguration(
				cfg => {
					cfg.AddProfile<QuestionProfile>();
					cfg.AddProfile<DetailProfile>();
					});
			var mapper = new Mapper(mapperConfiguration);
		
			var questionController = new QuestionController(quizTestDataRepository, mapper);

			questionController.ControllerContext.HttpContext = new DefaultHttpContext();
			
			var resourceParameter = new QuestionResourceParameter();

			// Act
			var result = await questionController.Get(resourceParameter);

			// Assert
			Assert.NotNull(result.Result);
			var actionResult = Assert.IsAssignableFrom<ActionResult<IEnumerable<QuestionDto>>>(result);
			var objResult = Assert.IsAssignableFrom<OkObjectResult>(result.Result);
			var questions = Assert.IsAssignableFrom<IEnumerable<QuestionDto>>(objResult.Value);
			Assert.Empty(questions);
		}
	}
}
