using QuizMaster.API.Quiz.Models;using QuizMaster.API.Quiz.SeedData;
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

		public async Task<bool> AddQuestionDetail(Question question, IEnumerable<QuestionDetailCreateDto> questionDetailCreateDto)
		{
			try
			{
				var questionDetails = new List<QuestionDetail>();
				var questionDetailTypes = new List<QuestionDetailType>();
				questionDetailCreateDto.ToList().ForEach(async questionDetailCreateDto =>
				{
					var detail = new QuestionDetail()
					{
						QDetailDesc = questionDetailCreateDto.QDetailDesc,
						Question = question
					};
					
					
					questionDetailCreateDto.DetailTypes.ToList().ForEach( dType =>
					{
						var questionDetailType = new QuestionDetailType();
						questionDetailType.QuestionDetail = detail;
						questionDetailType.DetailTypeId = DetailTypes.keyValuePairs[dType];
						questionDetailTypes.Add(questionDetailType);						
					});
				});

				await _quizRepository.AddQuestionDetailsAsync(questionDetails);
				await _quizRepository.AddQuestionDetailTypesAsync(questionDetailTypes);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError("Failed Adding Question Details", ex);
				return false;
			}



		}

		Task<bool> IQuestionDetailManager.AddQuestionDetail(Question question, QuestionDetail questionDetail)
		{

			throw new NotImplementedException();
		}

		Task<bool> IQuestionDetailManager.AddQuestionDetail(Question question, QuestionDetailCreateDto questionDetailCreateDto)
		{

			throw new NotImplementedException();
		}
	}
}
