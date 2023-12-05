using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.ResourceParameters;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Models;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuizMaster.API.Quiz.Controllers
{
	[Route("api/question/difficulty")]
	[ApiController]
	public class QuestionDifficultyController : ControllerBase
	{

		private readonly IQuizRepository _quizRepository;
		private readonly IMapper _mapper;

		public QuestionDifficultyController(IQuizRepository quizRepository, IMapper mapper)
		{
			_quizRepository = quizRepository;
			_mapper = mapper;
		}

		// GET: api/question/difficulty
		[HttpGet]
		public async Task<ActionResult<IEnumerable<DifficultyDto>>> Get([FromQuery] DifficultyResourceParameter resourceParameter)
		{
            if (resourceParameter.IsGetAll)
            {
                var difficultiesFromDb = await _quizRepository.GetAllDifficultiesAsync();

                return Ok(_mapper.Map<IEnumerable<DifficultyDto>>(difficultiesFromDb));
            }
            // Get all active difficulties asynchronously
            var difficulties = await _quizRepository.GetAllDifficultiesAsync(resourceParameter);

            var paginationMetadata = new Dictionary<string, object?>
            {
                    { "totalCount", difficulties.TotalCount },
                    { "pageSize", difficulties.PageSize },
                    { "currentPage", difficulties.CurrentPage },
                    { "totalPages", difficulties.TotalPages },
                    { "previousPageLink", difficulties.HasPrevious ?
                        Url.Link("GetQuestions", resourceParameter.GetObject("prev"))
                    : null },
                { "nextPageLink", difficulties.HasNext ?
                        Url.Link("GetQuestions", resourceParameter.GetObject("next"))
                        : null }
                };

            Response.Headers.Add("X-Pagination",
                   JsonSerializer.Serialize(paginationMetadata));

            Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");

            return Ok(difficulties);
        }

		// GET api/question/difficulty/5
		[HttpGet("{id}", Name = "GetDifficulty")]
		public async Task<ActionResult<DifficultyDto>> Get(int id)
		{
			// Get Difficulty asynchronously
			var difficulty = await _quizRepository.GetDifficultyAsync(id);

			// Return not found if difficulty doesn't exist or no longer active
			if (difficulty == null || !difficulty.ActiveData) return NotFound(new ResponseDto { Type = "Error", Message = $"Difficulty with id {id} not found." });

			return Ok(_mapper.Map<DifficultyDto>(difficulty));

		}

		// POST api/question/difficulty
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] DifficultyCreateDto difficulty)
		{
			// validate model
			if (!ModelState.IsValid)
			{
				return ReturnModelStateErrors();
			}

			// Check if difficulty description already exist
			var difficultyFromRepo = await _quizRepository.GetDifficultyAsync(difficulty.QDifficultyDesc);

			if (difficultyFromRepo != null && difficultyFromRepo.ActiveData)
			{
				return ReturnDifficultyAlreadyExist();
			}

			bool isSuccess;

			// If difficulty is not null and not active, we set active to true and update the difficulty
			if (difficultyFromRepo != null && !difficultyFromRepo.ActiveData)
			{
				difficultyFromRepo.ActiveData = true;
				isSuccess = _quizRepository.UpdateDifficulty(difficultyFromRepo);
			}
			// else, we create new difficulty
			else
			{
				difficultyFromRepo = _mapper.Map<QuestionDifficulty>(difficulty);
				isSuccess = await _quizRepository.AddDifficultyAsync(difficultyFromRepo);
			}


			// Check if update or create is not access 
			if (!isSuccess)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to create difficulty." });

			}

			await _quizRepository.SaveChangesAsync();
			return CreatedAtRoute("GetDifficulty", new { id = difficultyFromRepo.Id }, _mapper.Map<DifficultyDto>(difficultyFromRepo));
		}

		// PATCH api/question/difficulty/5
		[HttpPatch("{id}")]
		public async Task<ActionResult<DifficultyDto>> Put(int id, JsonPatchDocument<DifficultyCreateDto> patch)
		{

			var difficultyFromRepo = await _quizRepository.GetDifficultyAsync(id);

			// Checks if difficulty does not exist or (if it exist) checks if difficulty is active
			if (difficultyFromRepo == null || !difficultyFromRepo.ActiveData)
			{
				return ReturnDifficultyDoesNotExist(id);
			}

			var difficultyForPatch = new DifficultyCreateDto();

			patch.ApplyTo(difficultyForPatch);

			// Validate model of difficulty
			if (!TryValidateModel(difficultyForPatch))
			{
				return ReturnModelStateErrors();
			}

			// Check if difficulty description already exist
			if (await _quizRepository.GetDifficultyAsync(difficultyForPatch.QDifficultyDesc) != null)
			{
				return ReturnDifficultyAlreadyExist();
			}

			_mapper.Map(difficultyForPatch, difficultyFromRepo);

			var isSuccess = _quizRepository.UpdateDifficulty(difficultyFromRepo);

			// Check if update is success
			if (!isSuccess)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to update difficulty." });
			}

			// Save changes and return created difficulty
			await _quizRepository.SaveChangesAsync();
			return CreatedAtRoute("GetDifficulty", new { id = difficultyFromRepo.Id }, _mapper.Map<DifficultyDto>(difficultyFromRepo));



		}

		// DELETE api/question/difficulty/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var difficultyFromDb = await _quizRepository.GetDifficultyAsync(id);

			// Checks if difficulty does not exist or (if it exist) checks if difficulty is active
			if (difficultyFromDb == null || !difficultyFromDb.ActiveData)
			{
				return ReturnDifficultyDoesNotExist(id);
			}

			// change active to false 
			difficultyFromDb.ActiveData = false;
			var isSuccess = _quizRepository.UpdateDifficulty(difficultyFromDb);

			// Check if update is success
			if (!isSuccess)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to update difficulty." });

			}

			// Save changes and return created difficulty
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

		private ActionResult ReturnDifficultyDoesNotExist(int id)
		{
			return BadRequest(new ResponseDto
			{
				Type = "Error",
				Message = $"Difficulty with id {id} doesn't exist."
			});

		}

		private ActionResult ReturnDifficultyAlreadyExist()
		{
			return BadRequest(new ResponseDto
			{
				Type = "Error",
				Message = "Difficulty already exist."
			});
		}
	}
}
