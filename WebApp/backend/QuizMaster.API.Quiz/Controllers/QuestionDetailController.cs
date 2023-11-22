using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.SeedData;
using QuizMaster.API.Quiz.Services;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuizMaster.API.Quiz.Controllers
{
	[Route("api/question/{questionId}/question-detail")]
	[ApiController]
	public class QuestionDetailController : ControllerBase
	{

		// _quizRepository interacts directly to the dbContext
		// _questionDetailManager manages the adding and updating of questionDetail
		private readonly IQuizRepository _quizRepository;
		private readonly IQuestionDetailManager _questionDetailManager;
		private readonly IMapper _mapper;

		public QuestionDetailController(IQuizRepository quizRepository, IQuestionDetailManager questionDetailManager, IMapper mapper)
		{
			_quizRepository = quizRepository;
			_questionDetailManager = questionDetailManager;
			_mapper = mapper;
		}

		// GET: api/<QuestionDetailController>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<QuestionDetailDto>>> Get(int questionId)
		{
			var questionDetails = await _quizRepository.GetQuestionDetailsAsync(questionId);
			var questionDetailsDto = _mapper.Map<IEnumerable<QuestionDetailDto>>(questionDetails);
			return Ok(questionDetailsDto);
		}

		// GET api/<QuestionDetailController>/5
		[HttpGet("{id}", Name = "GetQuestionDetail")]
		public async Task<ActionResult<QuestionDetailDto>> Get(int questionId, int id)
		{
			var questionDetail = await _quizRepository.GetQuestionDetailAsync(questionId, id);
			if (questionDetail == null || !questionDetail.ActiveData) {
				return NotFound(new ResponseDto
				{
					Type = "Not Found",
					Message = "QuestionDetail not found."
				});
			}
			return _mapper.Map<QuestionDetailDto>(questionDetail);
		}


		// POST api/<QuestionDetailController>
		[HttpPost]
		public async Task<IActionResult> Post(int questionId, QuestionDetailCreateDto questionDetailCreateDto)
		{
			// validate model
			if (!ModelState.IsValid)
			{
				return ReturnModelStateErrors();
			}

			// Check if question exist or not
			var question = await _quizRepository.GetQuestionAsync(questionId);
			if (question == null)
			{
				return BadRequest(new ResponseDto
				{
					Type = "Error",
					Message = "Question is not found."
				});
			}

			// Map the questionDetailCreateDto to QuestionDetail
			var questionDetail = _mapper.Map<QuestionDetail>(questionDetailCreateDto);

			// Get detailTypes of the questionDetail is using
			var detailTypes = await _quizRepository.GetDetailTypesAsync(questionDetailCreateDto.DetailTypes);

			// Set the detailTypes of the questionDetail
			questionDetail.DetailTypes = detailTypes;

			// Add the questionDetail to the Db
			var success = await _questionDetailManager.AddQuestionDetailAsync(question, questionDetail);

			// Check if addQuestionDetail is a success
			if (!success)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to create question detail." });
			}

			// Save Changes to Db
			await _quizRepository.SaveChangesAsync();

			// Return the created question Detail in the form of QuestionDetailDto
			questionDetail.DetailTypes = detailTypes;
			var questionDetailDto = _mapper.Map<QuestionDetailDto>(questionDetail);

			return CreatedAtRoute("GetQuestionDetail", new { questionId , id = questionDetailDto.Id }, questionDetailDto);
		}

		// PUT api/<QuestionDetailController>/5
		[HttpPatch("{id}")]
		public async Task<ActionResult<CategoryDto>> Patch(int questionId, int id, [FromBody] JsonPatchDocument<QuestionDetailCreateDto> patch)
		{
			var question = await _quizRepository.GetQuestionAsync(questionId);

			// Checks if question does not exist or (if it exist) checks if question is active
			if (question == null || !question.ActiveData)
			{
				return BadRequest(new ResponseDto
				{
					Type = "Error",
					Message = "Question is not found."
				});
			}
			
			// Get QuestionDetail with associated questionID
			var questionDetailFromRepo = await  _quizRepository.GetQuestionDetailAsync(questionId, id);

			// Guard if questionDetail doesn't exist or deleted
			if (questionDetailFromRepo == null || !questionDetailFromRepo.ActiveData) {
				return BadRequest(new ResponseDto
				{
					Type = "Error",
					Message = "Question Detail is not found."
				});
			}

			// Patch the changes into the question from repo
			var questionDetailToPatch = _mapper.Map<QuestionDetailCreateDto>(questionDetailFromRepo);
			patch.ApplyTo(questionDetailToPatch);
			_mapper.Map(questionDetailToPatch, questionDetailFromRepo);

			// Checks if DetailTypes is updated or not
			if(questionDetailToPatch.DetailTypes != null)
			{
				var detailTypes = await _quizRepository.GetDetailTypesAsync(questionDetailToPatch.DetailTypes);
				questionDetailFromRepo.DetailTypes = detailTypes;
			}

			// Validate model of question
			if (!TryValidateModel(questionDetailFromRepo))
			{
				return ReturnModelStateErrors();
			}


			var success = await _questionDetailManager.UpdateQuestionDetailAsync(question, questionDetailFromRepo);

			// Check if updateQuestionDetail is a success
			if (!success)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to create question detail." });
			}

			// Save Changes to Db
			await _quizRepository.SaveChangesAsync();

			// Return the created question Detail in the form of QuestionDetailDto
			var questionDetailDto = _mapper.Map<QuestionDetailDto>(questionDetailFromRepo);

			return CreatedAtRoute("GetQuestionDetail", new { questionId, id = questionDetailDto.Id }, questionDetailDto);
		}

		// DELETE api/<QuestionDetailController>/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int questionId, int id)
		{
			var questionDetailFromDb = await _quizRepository.GetQuestionDetailAsync(questionId,id);

			// Checks if category does not exist or (if it exist) checks if category is active
			if (questionDetailFromDb == null || !questionDetailFromDb.ActiveData)
			{
				return BadRequest(new ResponseDto
				{
					Type = "Error",
					Message = "QuestionDetail does not exist"
				});
			}

			// change active to false 
			questionDetailFromDb.ActiveData = false;
			var isSuccess = _quizRepository.UpdateQuestionDetail(questionDetailFromDb);

			// Check if update is success
			if (!isSuccess)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = $"Failed to update question detail for question with id {questionId}." });

			}

			// Save changes and return created category
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
	}
}
