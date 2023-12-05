using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Models.ValidationModel;
using QuizMaster.API.Quiz.ResourceParameters;
using QuizMaster.API.Quiz.Services;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.Library.Common.Entities.Interfaces;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Models;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuizMaster.API.Quiz.Controllers
{
	[Route("api/question")]
	[ApiController]
	public class QuestionController : ControllerBase
	{
		private readonly IQuizRepository _quizRepository;
		private readonly IQuestionDetailManager _questionDetailManager;
		private readonly IMapper _mapper;


		public QuestionController(IQuizRepository quizRepository, IQuestionDetailManager questionDetailManager, IMapper mapper)
		{
			_quizRepository = quizRepository;
			_questionDetailManager = questionDetailManager;
			_mapper = mapper;
		}

		#region Get All Questions
		// GET: api/question
		[HttpGet(Name = "GetQuestions")]
		public async Task<ActionResult<IEnumerable<QuestionDto>>> Get([FromQuery] QuestionResourceParameter
			resourceParameter)
		{
			// Get all active questions asynchronously
			var questions = await _quizRepository.GetAllQuestionsAsync(resourceParameter);

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
				   JsonSerializer.Serialize(paginationMetadata));

			Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");

			return Ok(_mapper.Map<IEnumerable<QuestionDto>>(questions));
		}
		#endregion

		#region Get Question
		// GET api/question/5
		[HttpGet("{id}", Name = "GetQuestion")]
		public async Task<ActionResult<QuestionDto>> Get(int id)
		{
			// Get Question asynchronously
			var question = await _quizRepository.GetQuestionAsync(id);

			// Return not found if question doesn't exist or no longer active
			if (question == null || !question.ActiveData) return NotFound(new ResponseDto { Type = "Error", Message = $"Question with id {id} not found." });

			var questionDto = _mapper.Map<QuestionDto>(question);


			return Ok(questionDto);

		}
		#endregion

		#region Post Question
		// POST api/question
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] QuestionCreateDto question)
		{
			// validate model
			if (!ModelState.IsValid)
			{
				return ReturnModelStateErrors();
			}

			// Custom Validation
			var validationResult = question.IsValid();
			if (!validationResult.IsValid)
			{
				return BadRequest(
					new ResponseDto { Type = "Error", Message = validationResult.Error }
				);
			}

			// Check if question statement with associated category, difficulty, and type already exist
			var questionFromRepo = await _quizRepository.GetQuestionAsync(question.QStatement, question.QDifficultyId, question.QTypeId, question.QCategoryId);

			// Return question already exist error
			if (questionFromRepo != null && questionFromRepo.ActiveData)
			{
				return ReturnQuestionAlreadyExist();
			}

			bool isSuccess;

			// If question is not null and not active, we set active to true and update the question
			if (questionFromRepo != null && !questionFromRepo.ActiveData)
			{
				questionFromRepo.ActiveData = true;
				isSuccess = _quizRepository.UpdateQuestion(questionFromRepo);
			}
			else
			// else, we create new question
			{

				// Get category, difficulty, and type
				var category = await _quizRepository.GetCategoryAsync(question.QCategoryId);
				var difficulty = await _quizRepository.GetDifficultyAsync(question.QDifficultyId);
				var type = await _quizRepository.GetTypeAsync(question.QTypeId);

				// Guard if category, difficulty, and type is not found
				var result = ValidateCategoryDifficultyType(category, difficulty, type);
				if (!result.IsValid)
				{
					return BadRequest(new ResponseDto
					{
						Type = "Error",
						Message = result.Error
					});
				}


				questionFromRepo = _mapper.Map<Question>(question);



				// Assign category, difficulty, and type
				questionFromRepo.QCategory = category!;
				questionFromRepo.QDifficulty = difficulty!;
				questionFromRepo.QType = type!;

				var isQuestionAddedSuccessfully = await _quizRepository.AddQuestionAsync(questionFromRepo);
				var isDetailAddedSuccessfully = true;

				if (isQuestionAddedSuccessfully)
				{
					isDetailAddedSuccessfully = await _questionDetailManager.AddQuestionDetailAsync(questionFromRepo, question.questionDetailCreateDtos);
				}

				isSuccess = isDetailAddedSuccessfully && isQuestionAddedSuccessfully;
			}

			// Check if update or create is not success 
			if (!isSuccess)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "error", Message = "failed to create question." });

			}

			await _quizRepository.SaveChangesAsync();

			var questionDto = _mapper.Map<QuestionDto>(questionFromRepo);

			return CreatedAtRoute("GetQuestion", new { id = questionFromRepo.Id }, questionDto);
		}
		#endregion

		#region Patch Question
		// PATCH api/question/5
		[HttpPatch("{id}")]
		public async Task<ActionResult<QuestionDto>> Put(int id, JsonPatchDocument<QuestionCreateDto> patch)
		{

			var questionFromRepo = await _quizRepository.GetQuestionAsync(id);

			// Checks if question does not exist or (if it exist) checks if question is active
			if (questionFromRepo == null || !questionFromRepo.ActiveData)
			{
				return ReturnQuestionDoesNotExist(id);
			}

			// Patch the changes into the question from repo
			var questionForPatch = _mapper.Map<QuestionCreateDto>(questionFromRepo);
			patch.ApplyTo(questionForPatch);
			_mapper.Map(questionForPatch, questionFromRepo);

			// Validate model of question
			if (!TryValidateModel(questionFromRepo))
			{
				return ReturnModelStateErrors();
			}

			// Get category, difficulty, and type
			var category = await _quizRepository.GetCategoryAsync(questionFromRepo.QCategoryId);
			var difficulty = await _quizRepository.GetDifficultyAsync(questionFromRepo.QDifficultyId);
			var type = await _quizRepository.GetTypeAsync(questionFromRepo.QTypeId);


			// Guard if category, difficulty, and type is not found
			var result = ValidateCategoryDifficultyType(category, difficulty, type);
			if (!result.IsValid)
			{
				return BadRequest(new ResponseDto
				{
					Type = "Error",
					Message = result.Error
				});
			}


			// Check if question description already exist
			if (await _quizRepository.GetQuestionAsync(questionFromRepo.QStatement, questionFromRepo.QDifficultyId, questionFromRepo.QTypeId, questionFromRepo.QCategoryId) != null)
			{
				return ReturnQuestionAlreadyExist();
			}

			var isSuccess = _quizRepository.UpdateQuestion(questionFromRepo);

			// Check if update is success
			if (!isSuccess)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to update question." });
			}

			// Save changes and return created question
			await _quizRepository.SaveChangesAsync();
			return CreatedAtRoute("GetQuestion", new { id = questionFromRepo.Id }, _mapper.Map<QuestionDto>(questionFromRepo));

		}
		#endregion

		#region Delete question
		// DELETE api/question/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var questionFromDb = await _quizRepository.GetQuestionAsync(id);

			// Checks if question does not exist or (if it exist) checks if question is active
			if (questionFromDb == null || !questionFromDb.ActiveData)
			{
				return ReturnQuestionDoesNotExist(id);
			}

			// change active to false 
			questionFromDb.ActiveData = false;
			var isSuccess = _quizRepository.UpdateQuestion(questionFromDb);

			// Check if update is success
			if (!isSuccess)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to delete question." });

			}

			// Save changes and return created question
			await _quizRepository.SaveChangesAsync();

			return NoContent();
		}
		#endregion

		#region Controller Utilities
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

		private ActionResult ReturnQuestionDoesNotExist(int id)
		{
			return BadRequest(new ResponseDto
			{
				Type = "Error",
				Message = $"Question with id {id} doesn't exist."
			});

		}

		private ActionResult ReturnQuestionAlreadyExist()
		{
			return BadRequest(new ResponseDto
			{
				Type = "Error",
				Message = "Question already exist."
			});
		}
		private ValidationModel ValidateCategoryDifficultyType(QuestionCategory? category, QuestionDifficulty? difficulty, QuestionType? type)
		{
			var validationModel = new ValidationModel();

			validationModel.Error += ValidateItem(category, "Category");
			validationModel.Error += ValidateItem(difficulty, "Difficulty");
			validationModel.Error += ValidateItem(type, "Type");

			return validationModel;
		}

		private string ValidateItem(IEntity? item, string itemName)
		{
			return item == null || !item.ActiveData ? $"{itemName} is not found. " : "";
		}
		#endregion

	}
}
