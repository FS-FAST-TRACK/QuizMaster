using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.ResourceParameters;
using QuizMaster.API.Quiz.SeedData;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Models;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuizMaster.API.Quiz.Controllers
{
	[Route("api/question/category")]
	[ApiController]
	public class QuestionCategoryController : ControllerBase
	{
		private readonly IQuizRepository _quizRepository;
		private readonly IMapper _mapper;

		public QuestionCategoryController(IQuizRepository quizRepository, IMapper mapper)
		{
			_quizRepository = quizRepository;
			_mapper = mapper;
		}

		// GET: api/question/category
		[HttpGet]
		public async Task<ActionResult<IEnumerable<CategoryDto>>> Get([FromQuery] CategoryResourceParameter resourceParameter)
		{
			if (resourceParameter.IsGetAll)
			{
				var categoriesFromDb = await _quizRepository.GetAllCategoriesAsync();

				return Ok(_mapper.Map<IEnumerable<CategoryDto>>(categoriesFromDb));
			}
			// Get all active categories asynchronously
			var categories = await _quizRepository.GetAllCategoriesAsync(resourceParameter);

			var paginationMetadata = new Dictionary<string, object?>
			{
					{ "totalCount", categories.TotalCount },
					{ "pageSize", categories.PageSize },
					{ "currentPage", categories.CurrentPage },
					{ "totalPages", categories.TotalPages },
					{ "previousPageLink", categories.HasPrevious ?
						Url.Link("GetQuestions", resourceParameter.GetObject("prev"))
					: null },
					{ "nextPageLink", categories.HasNext ?
						Url.Link("GetQuestions", resourceParameter.GetObject("next"))
						: null }
				};

			Response.Headers.Add("X-Pagination",
				   JsonSerializer.Serialize(paginationMetadata));

			Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");

			return Ok(categories);
		}

		// GET api/question/category/5
		[HttpGet("{id}", Name = "GetCategory")]
		public async Task<ActionResult<CategoryDto>> Get(int id)
		{
			// Get Category asynchronously
			var category = await _quizRepository.GetCategoryAsync(id);

			// Return not found if category doesn't exist or no longer active
			if (category == null || !category.ActiveData) return NotFound(new ResponseDto { Type = "Error", Message = $"Category with id {id} not found." });

			return Ok(_mapper.Map<CategoryDto>(category));

		}

		// POST api/question/category
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CategoryCreateDto category)
		{
			// validate model
			if (!ModelState.IsValid)
			{
				return ReturnModelStateErrors();
			}

			// Check if category description already exist
			var categoryFromRepo = await _quizRepository.GetCategoryAsync(category.QCategoryDesc);

			if (categoryFromRepo != null && categoryFromRepo.ActiveData)
			{
				return ReturnCategoryAlreadyExist();
			}

			bool isSuccess;

			// If category is not null and not active, we set active to true and update the category
			if (categoryFromRepo != null && !categoryFromRepo.ActiveData)
			{
				categoryFromRepo.ActiveData = true;
				isSuccess = _quizRepository.UpdateCategory(categoryFromRepo);
			}
			// else, we create new category
			else
			{
				categoryFromRepo = _mapper.Map<QuestionCategory>(category);
				isSuccess = await _quizRepository.AddCategoryAsync(categoryFromRepo);
			}


			// Check if update or create is not access 
			if (!isSuccess)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to create category." });

			}

			await _quizRepository.SaveChangesAsync();
			return CreatedAtRoute("GetCategory", new { id = categoryFromRepo.Id }, _mapper.Map<CategoryDto>(categoryFromRepo));
		}

		// PATCH api/question/category/5
		[HttpPatch("{id}")]
		public async Task<ActionResult<CategoryDto>> Put(int id, JsonPatchDocument<CategoryCreateDto> patch)
		{

			var categoryFromRepo = await _quizRepository.GetCategoryAsync(id);

			// Checks if category does not exist or (if it exist) checks if category is active
			if (categoryFromRepo == null || !categoryFromRepo.ActiveData)
			{
				return ReturnCategoryDoesNotExist(id);
			}

			var categoryForPatch = new CategoryCreateDto();

			patch.ApplyTo(categoryForPatch);

			// Validate model of category
			if (!TryValidateModel(categoryForPatch))
			{
				return ReturnModelStateErrors();
			}

			// Check if category description already exist
			if (await _quizRepository.GetCategoryAsync(categoryForPatch.QCategoryDesc) != null)
			{
				return ReturnCategoryAlreadyExist();
			}

			_mapper.Map(categoryForPatch, categoryFromRepo);

			var isSuccess = _quizRepository.UpdateCategory(categoryFromRepo);

			// Check if update is success
			if (!isSuccess)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to update category." });
			}

			// Save changes and return created category
			await _quizRepository.SaveChangesAsync();
			return CreatedAtRoute("GetCategory", new { id = categoryFromRepo.Id }, _mapper.Map<CategoryDto>(categoryFromRepo));



		}

		// DELETE api/question/category/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var categoryFromDb = await _quizRepository.GetCategoryAsync(id);

			// Checks if category does not exist or (if it exist) checks if category is active
			if (categoryFromDb == null || !categoryFromDb.ActiveData)
			{
				return ReturnCategoryDoesNotExist(id);
			}

			// change active to false 
			categoryFromDb.ActiveData = false;
			var isSuccess = _quizRepository.UpdateCategory(categoryFromDb);

			// Check if update is success
			if (!isSuccess)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { Type = "Error", Message = "Failed to update category." });

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

		private ActionResult ReturnCategoryDoesNotExist(int id)
		{
			return BadRequest(new ResponseDto
			{
				Type = "Error",
				Message = $"Category with id {id} doesn't exist."
			});

		}

		private ActionResult ReturnCategoryAlreadyExist()
		{
			return BadRequest(new ResponseDto
			{
				Type = "Error",
				Message = "Category already exist."
			});
		}
	}
}
