namespace QuizMaster.API.QuizSession.Models
{
    public class SessionDTO
    {
        List<int> Sets { get; set; } = new();
        List<string> Options { get; set; } = new();
    }
}
