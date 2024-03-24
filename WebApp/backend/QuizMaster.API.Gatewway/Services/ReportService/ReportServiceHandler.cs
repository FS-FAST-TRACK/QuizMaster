using QuizMaster.API.Gateway.Models.Report;

namespace QuizMaster.API.Gateway.Services.ReportService
{
    public class ReportServiceHandler
    {
        private readonly List<ParticipantAnswerReport> ParticipantAnswerReports;
        
        public ReportServiceHandler()
        {
            ParticipantAnswerReports = new();
        }

        public void SaveParticipantAnswer(ParticipantAnswerReport report)
        {
            ParticipantAnswerReports.Add(report);
        }

        public ParticipantAnswerReport? GetParticipantAnswerReport(string participantName, string SessionId)
        {
            return ParticipantAnswerReports.Where(p => p.ParticipantName.Equals(participantName) && p.SessionId.Equals(SessionId)).FirstOrDefault();
        }

        public ParticipantAnswerReport? GetParticipantAnswerReport(string ParticipantName, string SessionId, int QuestionId)
        {
            return ParticipantAnswerReports.Where(p => p.ParticipantName.Equals(ParticipantName) && p.SessionId.Equals(SessionId) && p.QuestionId == QuestionId).FirstOrDefault();
        }

        public void ClearParticipantAnswerReports(string SessionId)
        {
            ParticipantAnswerReports.RemoveAll(p => p.SessionId.Equals(SessionId));
        }

        public IEnumerable<ParticipantAnswerReport> GetParticipantAnswerReports(string SessionId)
        {
            return ParticipantAnswerReports.Where(p => p.SessionId.Equals(SessionId));
        }
    }
}
