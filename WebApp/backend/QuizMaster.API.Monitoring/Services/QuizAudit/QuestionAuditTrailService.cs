using QuizMaster.API.Monitoring.DataAccess.Quiz;
using QuizMaster.Library.Common.Entities.Audits;
using System;

namespace QuizMaster.API.Monitoring.Services.QuizAudit
{
    public class QuestionAuditTrailService : IQuestionAuditTrailService
    {
        private readonly IQuestionAuditTrailRepository _questionAuditTrailRepository;
        public QuestionAuditTrailService(IQuestionAuditTrailRepository questionAuditTrailRepository)
        {
            _questionAuditTrailRepository = questionAuditTrailRepository;
        }

     
        public async Task<IEnumerable<QuestionAuditTrail>> GetAllQuestionAuditTrailsAsync()
        {
            try
            {
                return await _questionAuditTrailRepository.GetAllQuestionAuditTrailsAsync();
            }
            catch (Exception ex)
            {
                // You can handle and log the exception here
                throw new Exception("Failed to retrieve question audit trails.", ex);
            }
        }

        public async Task<QuestionAuditTrail> GetQuestionAuditTrailByIdAsync(int id)
        {
            try
            {
                return await _questionAuditTrailRepository.GetQuestionAuditTrailByIdAsync(id);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trail by ID.", ex);
            }
        }

        public async Task<IEnumerable<QuestionAuditTrail>> GetQuestionAuditTrailsByActionAsync(string action)
        {
            try
            {
                return await _questionAuditTrailRepository.GetQuestionAuditTrailsByActionAsync(action);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails by action.", ex);
            }
        }


        public async Task<IEnumerable<QuestionAuditTrail>> GetQuestionAuditTrailsByUsernameAsync(string username)
        {
            try
            {
                return await _questionAuditTrailRepository.GetQuestionAuditTrailsByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails by username.", ex);
            }
        }

        public async Task<IEnumerable<QuestionAuditTrail>> GetQuestionAuditTrailsByUserRoleAsync(string userRole)
        {
            try
            {
                return await _questionAuditTrailRepository.GetQuestionAuditTrailsByUserRoleAsync(userRole);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails by user Role.", ex);
            }
        }

        public async Task<IEnumerable<QuestionAuditTrail>> GetQuestionAuditTrailsWithinDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                return await _questionAuditTrailRepository.GetQuestionAuditTrailsWithinDateRangeAsync(startDate, endDate);
            }
            catch (Exception ex)
            {
                // Handle and log the exception
                throw new Exception("Failed to retrieve audit trails within a date range.", ex);
            }
        }
    }
}
