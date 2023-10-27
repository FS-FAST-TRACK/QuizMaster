using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Library.Common.Entities.Questionnaire.Options
{
	public class Option
	{
		public string? Id { get; set; }
		[Required]
		public string Type { get; set; }
		[Required]
		public string Value { get; set; }
		public Option(string id, string type, string value)
		{
			Id = id;
			Type = type;
			Value = value;
		}
	}
}
