using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Models.ValidationModel;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuizMaster.API.Quiz.Controllers
{
	[Route("api/question")]
	[ApiController]
	public class QuestionController : ControllerBase
	{
		private readonly IQuizRepository _quizRepository;
		private readonly IMapper _mapper;

		public QuestionController(IQuizRepository quizRepository, IMapper mapper)
		{
			_quizRepository = quizRepository;
			_mapper = mapper;
		}

		// GET: api/question
		[HttpGet]
		public async Task<ActionResult<IEnumerable<QuestionDto>>> Get()
		{
			// Get all active questions asynchronously
			var questions = await _quizRepository.GetAllQuestionsAsync();
			return Ok(_mapper.Map<IEnumerable<QuestionDto>>(questions));
		}

		// GET api/question/5
		[HttpGet("{id}", Name = "GetQuestion")]
		public async Task<ActionResult<QuestionDto>> Get(int id)
		{
			// Get Question asynchronously
			var question = await _quizRepository.GetQuestionAsync(id);

			// Return not found if question doesn't exist or no longer active
			if (question == null || !question.ActiveData) return NotFound(new ResponseDto { Type = "Error", Message = $"Question with id {id} not found." });

			return Ok(_mapper.Map<QuestionDto>(question));

		}

		// POST api/question
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] QuestionCreateDto question)
		{
			// validate model
			if (!ModelState.IsValid)
			{
				return ReturnModelStateErrors();
			}

			// Check if question statement already exist
			var questionFromRepo = await _quizRepository.GetQuestionAsync(question.QStatement , question.QDifficultyId, question.QTypeId, question.QCategoryId);

			if (questionFromRepo != null && questionFromRepo.ActiveData)
			{
				return ReturnQuestionAlreadyExist();
			}

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

			bool isSuccess;

			// If question is not null and not active, we set active to true and update the question
			if (questionFromRepo != null && !questionFromRepo.ActiveData)
			{
				questionFromRepo.ActiveData = true;
				isSuccess = _quizRepository.UpdateQuestion(questionFromRepo);
			}
			// else, we create new question
			else
			{
				questionFromRepo = _mapper.Map<Question>(question);
				isSuccess = await _quizRepository.AddQuestionAsync(questionFromRepo);
			}


			// Check if update or create is not success 
			if (!isSuccess)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to create question." });

			}

			await _quizRepository.SaveChangesAsync();
			return CreatedAtRoute("GetQuestion", new { id = questionFromRepo.Id }, _mapper.Map<QuestionDto>(questionFromRepo));
		}

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

			var questionForPatch = new QuestionCreateDto();

			patch.ApplyTo(questionForPatch);

			// Validate model of question
			if (!TryValidateModel(questionForPatch))
			{
				return ReturnModelStateErrors();
			}

			// Check if question description already exist
			if (await _quizRepository.GetQuestionAsync(questionForPatch.QStatement, questionForPatch.QDifficultyId, questionForPatch.QTypeId, questionForPatch.QCategoryId) != null)
			{
				return ReturnQuestionAlreadyExist();
			}

			_mapper.Map(questionForPatch, questionFromRepo);

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
			if (category == null || !category.ActiveData)
			{
				validationModel.Error += "Category is not found. ";
			}

			if (difficulty == null || !difficulty.ActiveData)
			{
				validationModel.Error += "Difficulty is not found. ";

				
			}
			if (type == null || !type.ActiveData)
			{
				validationModel.Error += "Type is not found. ";
			}
			return validationModel;
		}

	}
}
