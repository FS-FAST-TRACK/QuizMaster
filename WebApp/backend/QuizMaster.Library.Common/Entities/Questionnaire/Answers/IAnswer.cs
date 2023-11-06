using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Library.Common.Entities.Questionnaire.Answers
{
	public interface IAnswer<T>
	{
		T Answer { get; set;}
	}
}
