using AutoMapper;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuizMaster.API.Quiz.Controllers
{
	[Route("api/question/type")]
	[ApiController]
	public class QuestionTypeController : ControllerBase
	{

		private readonly IQuizRepository _quizRepository;
		private readonly IMapper _mapper;

		public QuestionTypeController(IQuizRepository quizRepository, IMapper mapper)
		{
			_quizRepository = quizRepository;
			_mapper = mapper;
		}

		// GET: api/question/type
		[HttpGet]
		public async Task<ActionResult<IEnumerable<TypeDto>>> Get()
		{
			// Get all active types asynchronously
			var types = await _quizRepository.GetAllTypesAsync();
			return Ok(_mapper.Map<IEnumerable<TypeDto>>(types));
		}

		// GET api/question/type/5
		[HttpGet("{id}", Name = "GetType")]
		public async Task<ActionResult<TypeDto>> Get(int id)
		{
			// Get Type asynchronously
			var type = await _quizRepository.GetTypeAsync(id);

			// Return not found if type doesn't exist or no longer active
			if (type == null || !type.ActiveData) return NotFound(new ResponseDto { Type = "Error", Message = $"Type with id {id} not found." });

			return Ok(_mapper.Map<TypeDto>(type));

		}

		// POST api/question/type
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] TypeCreateDto type)
		{
			// validate model
			if (!ModelState.IsValid)
			{
				return ReturnModelStateErrors();
			}

			// Check if type description already exist
			var typeFromRepo = await _quizRepository.GetTypeAsync(type.QTypeDesc);

			if (typeFromRepo != null && typeFromRepo.ActiveData)
			{
				return ReturnTypeAlreadyExist();
			}

			bool isSuccess;

			// If type is not null and not active, we set active to true and update the type
			if (typeFromRepo != null && !typeFromRepo.ActiveData)
			{
				typeFromRepo.ActiveData = true;
				isSuccess = _quizRepository.UpdateType(typeFromRepo);
			}
			// else, we create new type
			else
			{
				typeFromRepo = _mapper.Map<QuestionType>(type);
				isSuccess = await _quizRepository.AddTypeAsync(typeFromRepo);
			}


			// Check if update or create is not access 
			if (!isSuccess)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to create type." });

			}

			await _quizRepository.SaveChangesAsync();
			return CreatedAtRoute("GetType", new { id = typeFromRepo.Id }, _mapper.Map<TypeDto>(typeFromRepo));
		}

		// PATCH api/question/type/5
		[HttpPatch("{id}")]
		public async Task<ActionResult<TypeDto>> Put(int id, JsonPatchDocument<TypeCreateDto> patch)
		{

			var typeFromRepo = await _quizRepository.GetTypeAsync(id);

			// Checks if type does not exist or (if it exist) checks if type is active
			if (typeFromRepo == null || !typeFromRepo.ActiveData)
			{
				return ReturnTypeDoesNotExist(id);
			}

			var typeForPatch = new TypeCreateDto();

			// update the type to the current type in our db
			_mapper.Map(typeFromRepo, typeForPatch);

			patch.ApplyTo(typeForPatch);

			// Validate model of type
			if (!TryValidateModel(typeForPatch))
			{
				return ReturnModelStateErrors();
			}

			// Check if type description already exist
			if (await _quizRepository.GetTypeAsync(typeForPatch.QTypeDesc) != null && id != typeFromRepo.Id)
			{
				return ReturnTypeAlreadyExist();
			}

			_mapper.Map(typeForPatch, typeFromRepo);

			var isSuccess = _quizRepository.UpdateType(typeFromRepo);

			// Check if update is success
			if (!isSuccess)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to update type." });
			}

			// Save changes and return created type
			await _quizRepository.SaveChangesAsync();
			return CreatedAtRoute("GetType", new { id = typeFromRepo.Id }, _mapper.Map<TypeDto>(typeFromRepo));



		}

		// DELETE api/question/type/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var typeFromDb = await _quizRepository.GetTypeAsync(id);

			// Checks if type does not exist or (if it exist) checks if type is active
			if (typeFromDb == null || !typeFromDb.ActiveData)
			{
				return ReturnTypeDoesNotExist(id);
			}

			// change active to false 
			typeFromDb.ActiveData = false;
			var isSuccess = _quizRepository.UpdateType(typeFromDb);

			// Check if update is success
			if (!isSuccess)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to update type." });

			}

			// Save changes and return created type
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

		private ActionResult ReturnTypeDoesNotExist(int id)
		{
			return BadRequest(new ResponseDto
			{
				Type = "Error",
				Message = $"Type with id {id} doesn't exist."
			});

		}

		private ActionResult ReturnTypeAlreadyExist()
		{
			return BadRequest(new ResponseDto
			{
				Type = "Error",
				Message = "Type already exist."
			});
		}
	}
}
