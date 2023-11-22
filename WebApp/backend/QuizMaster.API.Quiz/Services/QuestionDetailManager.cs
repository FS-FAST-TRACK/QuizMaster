using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.SeedData;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.Services
{
	public class QuestionDetailManager : IQuestionDetailManager
	{
		private readonly IQuizRepository _quizRepository;
		private readonly ILogger<QuestionDetailManager> _logger;

		public QuestionDetailManager(IQuizRepository quizRepository, ILogger<QuestionDetailManager> logger)
		{
			_quizRepository = quizRepository;
			_logger = logger;
		}

		public async Task<bool> AddQuestionDetailAsync(Question question, IEnumerable<QuestionDetailCreateDto> questionDetailCreateDtos)
		{
			try
			{
				// Initialized variable for questionDetails and questionDetailTypes
				var questionDetailTypes = new List<QuestionDetailType>();

				// Get the questionDetails from the question
				questionDetailCreateDtos.ToList().ForEach(questionDetailCreateDto =>
				{
					var detail = new QuestionDetail()
					{
						QDetailDesc = questionDetailCreateDto.QDetailDesc,
						Question = question
					};

					// Get the questionDetailTypes for questionDetail
					questionDetailCreateDto.DetailTypes.ToList().ForEach(dType =>
					{
						var questionDetailType = new QuestionDetailType();
						// below code will automatically add the questionDetail
						questionDetailType.QuestionDetail = detail;
						questionDetailType.DetailTypeId = DetailTypes.keyValuePairs[dType];
						questionDetailTypes.Add(questionDetailType);
					});
				});


				// Add questionDetailTypes
				await _quizRepository.AddQuestionDetailTypesAsync(questionDetailTypes);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Failed Adding Question Details", ex);
				return false;
			}



		}

		public async Task<bool> AddQuestionDetailAsync(Question question, QuestionDetail questionDetail)
		{
			try
			{
				var questionDetailTypes = new List<QuestionDetailType>();
				questionDetail.DetailTypes.ToList().ForEach(dT =>
				{
					questionDetailTypes.Add(new QuestionDetailType
					{
						QuestionDetail = questionDetail,
						DetailTypeId = dT.Id,
					});
				});

				await _quizRepository.AddQuestionDetailTypesAsync(questionDetailTypes);

				questionDetail.Question = question;
				await _quizRepository.AddQuestionDetailAsync(questionDetail);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Failed Adding Question Details", ex);
				return false;
			}
		}

		public async Task<bool> AddQuestionDetailAsync(Question question, QuestionDetailCreateDto questionDetailCreateDto)
		{
			try
			{

				// Initialized variable questionDetailTypes
				var questionDetailTypes = new List<QuestionDetailType>();

				// Get the questionDetails from the question

				var detail = new QuestionDetail()
				{
					QDetailDesc = questionDetailCreateDto.QDetailDesc,
					Question = question
				};

				// Get the questionDetailTypes questionDetail
				questionDetailCreateDto.DetailTypes.ToList().ForEach(dType =>
				{
					var questionDetailType = new QuestionDetailType();
					questionDetailType.QuestionDetail = detail;
					questionDetailType.DetailTypeId = DetailTypes.keyValuePairs[dType];
					questionDetailTypes.Add(questionDetailType);
				});



				// Add questionDetail and questionDetailTypes
				await _quizRepository.AddQuestionDetailAsync(detail);
				await _quizRepository.AddQuestionDetailTypesAsync(questionDetailTypes);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Failed Adding Question Details", ex);
				return false;
			}
		}

		public async Task<bool> UpdateQuestionDetailAsync(Question question, QuestionDetail questionDetail)
		{

			try
			{
				// Initialized variable questionDetailTypes
				var questionDetailTypes = new List<QuestionDetailType>();

				// Get the questionDetailTypes questionDetail
				questionDetail.DetailTypes.ToList().ForEach(dType =>
				{
					var questionDetailType = new QuestionDetailType();
					questionDetailType.QuestionDetail = questionDetail;
					questionDetailType.DetailTypeId = dType.Id;
					questionDetailTypes.Add(questionDetailType);
				});

				// update the question detail
				_quizRepository.UpdateQuestionDetail(questionDetail);

				if(questionDetailTypes.Count > 0)
				{
					await _quizRepository.RemoveQuestionDetailTypesOfQuestionDetailByIdAsync(questionDetail.Id);
				}

				// Add questionDetail and questionDetailTypes
				await _quizRepository.AddQuestionDetailTypesAsync(questionDetailTypes);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Failed Adding Question Details", ex);
				return false;
			}
		}

	}
}
