using QuizMaster.Library.Common.Entities.Questionnaire.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Library.Common.Entities.Questionnaire.Answers
{
	public class PuzzleAnswer : IAnswer<IEnumerable<Option>>
	{
		public IEnumerable<Option> Answer { get ; set ; }
	}
}
