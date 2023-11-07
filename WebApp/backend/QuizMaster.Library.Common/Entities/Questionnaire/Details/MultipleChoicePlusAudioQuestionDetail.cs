using QuizMaster.Library.Common.Entities.Questionnaire.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Library.Common.Entities.Questionnaire.Details
{
	public class MultipleChoicePlusAudioQuestionDetail
	{
		public IEnumerable<Option> Choices { get; set; }
		public string TextToAudio { get; set; }
		public string Language { get; set; }

	}
}
