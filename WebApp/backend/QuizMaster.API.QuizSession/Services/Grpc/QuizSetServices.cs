using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuizMaster.API.QuizSession.DbContexts;
using QuizMaster.API.QuizSession.Protos;
using QuizMaster.Library.Common.Entities.Rooms;
using QuizMaster.Library.Common.Models.QuizSession;

namespace QuizMaster.API.QuizSession.Services.Grpc
{

    /* Potential Issues 
     * - Should not accept set name if name already exist
     */
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

            var checkQuestion =  CheckForQuistionId(quizSet.questions);

            try 
            {
                if (checkQuestion != -1)
                {
                    reply.Code = 404;
                    reply.Message = $"Question with id {checkQuestion} does not exist";

                    return await Task.FromResult(reply);
                } 

                var set = new Set { QSetName = quizSet.QSetName, QSetDesc = quizSet.QSetDesc, ActiveData = true };
                await _quizSetManager.Sets.AddAsync(set);
                await _quizSetManager.SaveChangesAsync();

                foreach (var questionID in quizSet.questions)
                {
                    var questionSet = new QuestionSet { QuestionId = questionID, SetId = set.Id, ActiveData=true };
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

        public override async Task<QuizSetMessage> GetAllQuizSet(QuizSetEmpty request, ServerCallContext context)
        {
            var reply = new QuizSetMessage();
            try 
            {
                var data = await _quizSetManager.Sets.Where(x => x.ActiveData == true).ToArrayAsync();

                reply.Code = 200;
                reply.Data = JsonConvert.SerializeObject(data);

                return await Task.FromResult(reply);
            }
            catch(Exception ex) 
            {
                reply.Code=500;
                reply.Message = ex.Message;

                return await Task.FromResult(reply);
            }
           
        }

        public override  async Task<QuizSetMessage> GetQuizSet(GetQuizSetRequest request, ServerCallContext context)
        {
            var reply = new QuizSetMessage();
            try
            {
                var data = await _quizSetManager.Sets.FirstOrDefaultAsync(x => x.Id == request.Id);
                if(data == null)
                {
                    reply.Code = 404;
                    reply.Message = $"Quiz Set with id of {request.Id} does not exist";
                    return await Task.FromResult(reply); 
                }

                reply.Code = 200;
                reply.Data= JsonConvert.SerializeObject(data);

                return await Task.FromResult(reply);

            }
            catch(Exception ex) 
            {
                reply.Code = 500;
                reply.Message= ex.Message;

                return await Task.FromResult(reply);
            }
        }

        public override async Task<QuizSetMessage> DeleteQuizSet(GetQuizSetRequest request, ServerCallContext context)
        {
            var reply = new QuizSetMessage();
            try
            {
                var data = await _quizSetManager.Sets.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (data == null)
                {
                    reply.Code = 404;
                    reply.Message = $"Quiz Set with id of {request.Id} does not exist";
                    return await Task.FromResult(reply);
                }

                data.ActiveData = false;
                await _quizSetManager.SaveChangesAsync();

                reply.Code = 200;
                reply.Message = "Successfully deleted the question set";

                return await Task.FromResult(reply);

            }
            catch(Exception ex)
            {
                reply.Code = 500;
                reply.Message = ex.Message;

                return await Task.FromResult(reply);
            }
        }

        public override async Task<QuizSetMessage> UpdateQuizSet(QuizSetRequest request, ServerCallContext context)
        {
            var reply = new QuizSetMessage();
            var id = request.Id;
            var setDTO = JsonConvert.DeserializeObject<SetDTO>(request.QuizSet)!;

            try
            {
                // Get the set by id if it exist
                var data = await _quizSetManager.Sets.FirstOrDefaultAsync(x => x.Id == request.Id);

                if (data == null)
                {
                    // Return not found
                    reply.Code = 404;
                    reply.Message = $"Quiz Set with id of {request.Id} does not exist";
                    return await Task.FromResult(reply);
                }

                // Check if the questions exist
                var checkQuestion = CheckForQuistionId(setDTO.questions);

                if (checkQuestion != -1)
                {
                    // If at lest 1 question ID does not exist it will return not found
                    reply.Code = 404;
                    reply.Message = $"Question with id {checkQuestion} does not exist";

                    return await Task.FromResult(reply);
                }

                // Update the changes
                data.QSetName = setDTO.QSetName;
                data.QSetDesc = setDTO.QSetDesc;
                await _quizSetManager.SaveChangesAsync();

                // Get the question in the set
                var questionSetsToRemove = _quizSetManager.QuestionSets
                    .Where(x => x.SetId == data.Id)
                    .ToList();

                // Delete the questions
                if (questionSetsToRemove.Count > 0)
                {
                    _quizSetManager.QuestionSets.RemoveRange(questionSetsToRemove);
                    await _quizSetManager.SaveChangesAsync();
                }

                // Add a new quest set with updated questin array
                foreach (var questionID in setDTO.questions)
                {
                    var questionSet = new QuestionSet { QuestionId = questionID, SetId = data.Id };
                    await _quizSetManager.QuestionSets.AddAsync(questionSet);
                }

                await _quizSetManager.SaveChangesAsync();

                reply.Code = 200;
                reply.Data = JsonConvert.SerializeObject(data);

                return await Task.FromResult(reply);

            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Message = ex.Message;

                return await Task.FromResult(reply);
            }

        }

        public override async Task<QuizSetMessage> GetAllQuestionSet(QuizSetEmpty request, ServerCallContext context)
        {
            var reply = new QuizSetMessage();
            try
            {
                //var data = await _quizSetManager.QuestionSets.Where(x => x.ActiveData == true).ToArrayAsync();
                var data = await _quizSetManager.QuestionSets.ToArrayAsync();

                reply.Code = 200;
                reply.Data = JsonConvert.SerializeObject(data);

                return await Task.FromResult(reply);
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Message = ex.Message;

                return await Task.FromResult(reply);
            }
        }

        public override async Task<QuizSetMessage> GetQuestionSet(GetQuizSetRequest request, ServerCallContext context)
        {
            var reply = new QuizSetMessage();
            try
            {
                var data = await _quizSetManager.QuestionSets.Where(x => x.SetId == request.Id).ToArrayAsync();
                if (data == null)
                {
                    reply.Code = 404;
                    reply.Message = $"Quiz Set with id of {request.Id} does not exist";
                    return await Task.FromResult(reply);
                }

                reply.Code = 200;
                reply.Data = JsonConvert.SerializeObject(data);

                return await Task.FromResult(reply);

            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Message = ex.Message;

                return await Task.FromResult(reply);
            }
        }

        private int CheckForQuistionId(IEnumerable<int> quistionID)
        {
            var quistions = _quizSetManager.Questions.Where(x => x.ActiveData).Select(x => x.Id).ToArray();
            foreach (var id in quistionID)
            {
                if (!quistions.Contains(id))
                { return id; }
            }
            return -1;
        }
    }
}
