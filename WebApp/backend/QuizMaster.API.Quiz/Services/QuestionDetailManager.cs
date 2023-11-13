using QuizMaster.API.Quiz.Models;
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
				questionDetailCreateDto.ToList().ForEach(async questionDetailCreateDto =>
				{


					var detail = new Detail()
					{
						DetailDesc = questionDetailCreateDto.DetailDesc
					};
					await _quizRepository.AddDetailAsync(detail);
					
					questionDetailCreateDto.DetailTypeIds.ToList().ForEach(async dTypeId =>
					{
						var questionDetail = new QuestionDetail();
						questionDetail.Detail = detail;
						questionDetail.Question = question;
						questionDetail.QuestionDetailTypeId = dTypeId;
						questionDetails.Add(questionDetail);
					});
				});

				await _quizRepository.AddQuestionDetailsAsync(questionDetails); 
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
