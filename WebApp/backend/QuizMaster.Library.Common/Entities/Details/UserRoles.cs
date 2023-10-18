using QuizMaster.Library.Common.Entities.Roles;

namespace QuizMaster.Library.Common.Entities.Details
{


	public static class UserRoles
	{
		public static UserRole Admin = new UserRole()
		{
			Id = 1,
			UserRoleDesc = "Admin",
			Name = "Administrator",
			NormalizedName = "ADMINISTRATOR"
		};

		public static UserRole User = new UserRole()
		{
			Id = 2,
			UserRoleDesc = "User",
			Name = "User",
			NormalizedName = "USER"
		};

	}
}
