namespace QuizMaster.API.Quiz.Models.ValidationModel
{
	public interface IValidationModel
	{
		bool IsValid { get; }
		string Error { get; set; }
	}
}
