﻿using QuizMaster.Library.Common.Entities.Questionnaire;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.API.Quiz.Models
{
	public class QuestionDto
	{
		public int Id { get; set; }
		public string QStatement { get; set; }

		public string QImage { get; set; } = "null";

		public string QAudio { get; set; } = "null";

		public int QTime { get; set; }

		public int QDifficultyId { get; set; }
		public QuestionDifficulty QDifficulty { get; set; } = default!;

        public int QCategoryId { get; set; }

		public int QTypeId { get; set; }
		public IEnumerable<QuestionDetailDto> Details { get; set; }

	}
}
