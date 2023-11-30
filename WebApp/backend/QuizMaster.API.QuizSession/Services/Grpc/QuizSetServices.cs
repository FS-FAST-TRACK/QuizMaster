using Grpc.Core;
using Newtonsoft.Json;
using QuizMaster.API.QuizSession.DbContexts;
using QuizMaster.API.QuizSession.Protos;
using QuizMaster.Library.Common.Entities.Rooms;
using QuizMaster.Library.Common.Models.QuizSession;

namespace QuizMaster.API.QuizSession.Services.Grpc
{
    public class QuizSetServices : QuizSetService.QuizSetServiceBase
    {
        private readonly QuizSessionDbContext _quizSetManager;

        public QuizSetServices(QuizSessionDbContext quizSetManager)
        {
            _quizSetManager = quizSetManager;
        }
        public override async Task<QuizSetMessage> AddQuizSet(QuizSetRequest request, ServerCallContext context)
        {
            var reply = new QuizSetMessage();

            var quizSet = JsonConvert.DeserializeObject<SetDTO>(request.QuizSet)!;

            var checkQuestion = await CheckForQuestionIdAsync(quizSet.questions);

            try 
            {
                if (checkQuestion != -1)
                {
                    reply.Code = 404;
                    reply.Message = $"Question with id {checkQuestion} does not exist";

                    return await Task.FromResult(reply);
                }

                var set = new Set { QSetName = quizSet.QSetName, QSetDesc = quizSet.QSetDesc };
                await _quizSetManager.Sets.AddAsync(set);
                await _quizSetManager.SaveChangesAsync();

                foreach (var questionID in quizSet.questions)
                {
                    var questionSet = new QuestionSet { QuestionId = questionID, SetId = set.Id };
                    await _quizSetManager.QuestionSets.AddAsync(questionSet);
                }

                await _quizSetManager.SaveChangesAsync();

                reply.Code = 200;
                reply.Data = JsonConvert.SerializeObject(set);

                return await Task.FromResult(reply);
            }
            catch (Exception ex) 
            {
                reply.Code =500;
                reply.Message = ex.Message;
                return await Task.FromResult(reply);
            }

            

        }

        private async Task<int> CheckForQuestionIdAsync(IEnumerable<int> questionIds)
        {
            var questionIdsArray = _quizSetManager.Questions.Select(x => x.Id).ToArray();

            var tasks = questionIds.Select(async id =>
            {
                if (!questionIdsArray.Contains(id))
                {
                    return id;
                }
                return -1;
            });

            var results = await Task.WhenAll(tasks);

            // You might want to customize this logic based on your requirements
            // For example, you might return the first non-existing ID, or a list of all non-existing IDs
            return results.FirstOrDefault(result => result != -1);
        }
    }
}
