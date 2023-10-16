using QuizMaster.Common.Library.Entities;

namespace QuizMaster.Common.Library.Roles
{
	public static class RolesData
	{

		public static UserRole Admin = new UserRole
		{
			Id = 1,
			UserRoleDesc = "Admin"
		};

		public static UserRole User = new UserRole
		{
			Id = 2,
			UserRoleDesc = "User"
		};

	}

}
