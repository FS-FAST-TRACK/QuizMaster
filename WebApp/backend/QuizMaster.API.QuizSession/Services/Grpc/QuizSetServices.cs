using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QuizMaster.API.Monitoring.Proto;
using QuizMaster.API.QuizSession.DbContexts;
using QuizMaster.API.QuizSession.Protos;
using QuizMaster.Library.Common.Entities.Rooms;
using QuizMaster.Library.Common.Helpers;
using QuizMaster.Library.Common.Models.QuizSession;

namespace QuizMaster.API.QuizSession.Services.Grpc
{

    /* Potential Issues 
     * - Should not accept set name if name already exist
     */
    public class QuizSetServices : QuizSetService.QuizSetServiceBase
    {
        private readonly QuizSessionDbContext _quizSetManager;
        private readonly QuizAuditService.QuizAuditServiceClient _quizAuditServiceClient;

        public QuizSetServices(QuizSessionDbContext quizSetManager, QuizAuditService.QuizAuditServiceClient quizAuditServiceClient)
        {
            _quizSetManager = quizSetManager;
            _quizAuditServiceClient = quizAuditServiceClient;
        }

        public override async Task<QuizSetMessage> AddQuizSet(QuizSetRequest request, ServerCallContext context)
        {
            var reply = new QuizSetMessage();

            var quizSet = JsonConvert.DeserializeObject<SetDTO>(request.QuizSet)!;

            var checkQuestion = CheckForQuistionId(quizSet.questions);

            try
            {
                if (checkQuestion != -1)
                {
                    reply.Code = 404;
                    reply.Message = $"Question with id {checkQuestion} does not exist";

                    return await Task.FromResult(reply);
                }

                var setWithNameExists = _quizSetManager.Sets.Where(s => s.QSetName.ToLower() == quizSet.QSetName.ToLower()).FirstOrDefault();

                if (setWithNameExists != null)
                {
                    reply.Code = 409;
                    reply.Message = $"There is a QuestionSet of name '{quizSet.QSetName}' exists in the database.";

                    return await Task.FromResult(reply);
                }

                var set = new Set { QSetName = quizSet.QSetName, QSetDesc = quizSet.QSetDesc, ActiveData = true, DateCreated = DateTime.Now };
                await _quizSetManager.Sets.AddAsync(set);
                await _quizSetManager.SaveChangesAsync();

                foreach (var questionID in quizSet.questions)
                {
                    var questionSet = new QuestionSet { QuestionId = questionID, SetId = set.Id, ActiveData = true };
                    await _quizSetManager.QuestionSets.AddAsync(questionSet);
                }

                await _quizSetManager.SaveChangesAsync();

                // Capture the details of the user adding the quiz set
                var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
                var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
                var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

                // Construct the add quiz set event
                var createQuizSetEvent = new CreateQuizSetEvent
                {
                    UserId = int.Parse(userId!),
                    Username = userNameClaim,
                    Action = "Create Quiz Set",
                    Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                    Details = $"Quiz Set created by: {userNameClaim}",
                    Userrole = userRoles,
                    OldValues = "",
                    NewValues = JsonConvert.SerializeObject(set),
                };

                var logRequest = new LogCreateQuizSetEventRequest
                {
                    Event = createQuizSetEvent
                };

                try
                {
                    // Make the gRPC call to log the add quiz set event
                    _quizAuditServiceClient.LogCreateQuizSetEvent(logRequest);
                }
                catch (Exception ex)
                {
                    reply.Code = 500;
                    reply.Message = ex.Message;
                    return await Task.FromResult(reply);
                }

                reply.Code = 200;
                reply.Data = JsonConvert.SerializeObject(set);

                return await Task.FromResult(reply);
            }
            catch (Exception ex)
            {
                reply.Code = 500;
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

                // Capture the details of the user deleting the quiz set
                var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
                var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
                var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

                // Capture the old quiz set state
                var oldQuizSet = new Set
                {
                    Id = data.Id,
                    QSetName = data.QSetName,
                    QSetDesc = data.QSetDesc,
                    DateCreated = data.DateCreated,
                };

                data.ActiveData = false;
                await _quizSetManager.SaveChangesAsync();

                // Construct the delete quiz set event
                var deleteQuizSetEvent = new DeleteQuizSetEvent
                {
                    UserId = int.Parse(userId!),
                    Username = userNameClaim,
                    Action = "Delete Quiz Set",
                    Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                    Details = $"Quiz Set deleted by: {userNameClaim}",
                    Userrole = userRoles,
                    OldValues = JsonConvert.SerializeObject(oldQuizSet),
                    NewValues = "",
                };

                var logRequest = new LogDeleteQuizSetEventRequest
                {
                    Event = deleteQuizSetEvent
                };

                try
                {
                    // Make the gRPC call to log the delete quiz set event
                    _quizAuditServiceClient.LogDeleteQuizSetEvent(logRequest);
                }
                catch (Exception ex)
                {
                    reply.Code = 500;
                    reply.Message = ex.Message;
                    return await Task.FromResult(reply);
                }

                reply.Code = 200;
                reply.Message = "Successfully deleted the question set";

                return await Task.FromResult(reply);

            }
            catch (Exception ex)
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
                // Get the set by id if it exists
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
                    // If at least 1 question ID does not exist, it will return not found
                    reply.Code = 404;
                    reply.Message = $"Question with id {checkQuestion} does not exist";

                    return await Task.FromResult(reply);
                }

                // Capture the details of the user updating the quiz set
                var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
                var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
                var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

                // Capture the old quiz set state
                var oldQuizSet = new Set
                {
                    Id = data.Id,
                    QSetName = data.QSetName,
                    QSetDesc = data.QSetDesc,
                    DateCreated = data.DateCreated,
                };

                // Update the changes
                data.QSetName = setDTO.QSetName;
                data.QSetDesc = setDTO.QSetDesc;
                data.DateUpdated = DateTime.Now;
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

                // Add a new quest set with updated question array
                foreach (var questionID in setDTO.questions)
                {
                    var questionSet = new QuestionSet { QuestionId = questionID, SetId = data.Id };
                    await _quizSetManager.QuestionSets.AddAsync(questionSet);
                }

                await _quizSetManager.SaveChangesAsync();

                // Construct the update quiz set event
                var updateQuizSetEvent = new UpdateQuizSetEvent
                {
                    UserId = int.Parse(userId!),
                    Username = userNameClaim,
                    Action = "Update Quiz Set",
                    Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                    Details = $"Quiz Set updated by: {userNameClaim}",
                    Userrole = userRoles,
                    OldValues = JsonConvert.SerializeObject(oldQuizSet),
                    NewValues = JsonConvert.SerializeObject(data),
                };

                var logRequest = new LogUpdateQuizSetEventRequest
                {
                    Event = updateQuizSetEvent
                };

                try
                {
                    // Make the gRPC call to log the update quiz set event
                    _quizAuditServiceClient.LogUpdateQuizSetEvent(logRequest);
                }
                catch (Exception ex)
                {
                    reply.Code = 500;
                    reply.Message = ex.Message;
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
