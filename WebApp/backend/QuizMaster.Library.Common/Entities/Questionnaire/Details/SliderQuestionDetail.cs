using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Library.Common.Entities.Questionnaire.Details
{
	public class SliderQuestionDetail
	{
		[Required]
		public int Minimum { get; set; }
		[Required]
		public int Maximum { get; set; }
		[Required]
		public int Increment { get; set; }

	}
}
