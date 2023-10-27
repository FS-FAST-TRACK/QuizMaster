namespace QuizMaster.API.Quiz.Models.Questions
{
	public interface IQuestion<T> 
	{
		int Id { get; set; }
		string Statement { get; set; }
		int Time { get; set; }
		string? Audio { get; set; }
		string? Video { get; set; }
		string? Image { get; set; }
		int DifficultyId { get; set; }
		int CategoryId { get; set; }
		int TypeId { get; set; }
		T Answer {get;set;}
	}
}
