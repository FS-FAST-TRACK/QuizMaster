using AutoMapper;
using Moq;
using QuizMaster.API.Quiz.Controllers;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Profiles;
using QuizMaster.API.Quiz.Tests.Services;

namespace QuizMaster.API.Quiz.Tests
{
	public class QuestionCategoryControllerTests
	{
		[Fact]
		public async Task Get_GetCatories_MustReturnNotEmpty()
		{
			// Arrange
			var quizTestDataRepository = new QuizTestDataRepository();
			var mapperConfiguration = new MapperConfiguration(
				cfg => cfg.AddProfile<CategoryProfile>());
			var mapper = new Mapper(mapperConfiguration);
			var questionCategoryController = new QuestionCategoryController(quizTestDataRepository, mapper);

			// Act
			var result =await  questionCategoryController.Get();

			// Assert
			Assert.NotNull(result.Result);
		}
	}
}
