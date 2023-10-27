 namespace QuizMaster.API.Quiz.Models.ValidationModel
{
	public class ValidationModel : IValidationModel
	{
		public bool IsValid { get => Error == ""; }
		public string Error { get; set; } = "";
	}
}
