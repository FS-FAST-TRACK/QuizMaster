namespace QuizMaster.API.Quiz.Models
{
	public class Option
	{
		string Id { get; set; }
		public string Type { get; set; }
		public string Value { get; set; }
		public Option(string id, string type, string value) {
			Id = id;
			Type = type;
			Value = value;
		}
	}
}
