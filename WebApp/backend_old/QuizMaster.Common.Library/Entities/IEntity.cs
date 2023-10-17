

using System;

namespace QuizMaster.Common.Library.Entities
{
	public interface IEntity
	{
		int Id { get; set; }

		DateTime DateCreated { get; set; }

		DateTime DateUpdated { get; set; }

	}
}
