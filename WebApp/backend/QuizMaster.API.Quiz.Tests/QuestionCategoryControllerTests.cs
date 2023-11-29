using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using QuizMaster.API.Quiz.Controllers;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Profiles;
using QuizMaster.API.Quiz.Tests.Services;
using System.Collections.Generic;

namespace QuizMaster.API.Quiz.Tests
{
	public class QuestionCategoryControllerTests
	{
		[Fact]
		public async Task Get_GetCategories_MustReturnNotEmpty()
		{
			// Arrange
			var quizTestDataRepository = new QuizTestDataRepository();
			var mapperConfiguration = new MapperConfiguration(
				cfg => cfg.AddProfile<CategoryProfile>());
			var mapper = new Mapper(mapperConfiguration);
			var questionCategoryController = new QuestionCategoryController(quizTestDataRepository, mapper);

			// Act
			var result =await  questionCategoryController.Get(new ResourceParameters.CategoryResourceParameter
			{

			});

			// Assert
			Assert.NotNull(result.Result);
			var actionResult = Assert.IsAssignableFrom<ActionResult<IEnumerable<CategoryDto>>>(result);
			var objResult = Assert.IsAssignableFrom <OkObjectResult>(result.Result);
			var categories = Assert.IsAssignableFrom<IEnumerable<CategoryDto>>(objResult.Value);
			Assert.NotEmpty(categories);
		}
	}
}
