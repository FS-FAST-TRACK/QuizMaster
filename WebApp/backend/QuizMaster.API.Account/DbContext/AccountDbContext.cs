using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Entities.Roles;

namespace QuizMaster.API.Account.DbContext
{
	public class AccountDbContext : IdentityDbContext<UserAccount, UserRole, int>
	{


		public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options) {
			ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);


			// Seed Roles
			modelBuilder.Entity<UserRole>()
				.HasData(Library.Common.Entities.Details.UserRoles.Admin, Library.Common.Entities.Details.UserRoles.User);

			// Seed claim for each role
			modelBuilder.Entity<IdentityRoleClaim<int>>()
				.HasData(new
					{
						Id = 1,
						RoleId = 1,
						ClaimType = "Role",
						ClaimValue = "Admin"
					}, new
					{
						Id = 2,
						RoleId = 2,
						ClaimType = "Role",
						ClaimValue = "User"
					});

			// Seed user table with admin user
			PasswordHasher<UserAccount> hasher = new PasswordHasher<UserAccount>();
			
			var seedAdmin = new UserAccount
			{
				Id = 1,
				UserName = "Admin",
				Email = "admin@gmail.com",
				SecurityStamp = Guid.NewGuid().ToString(),
			};

			var hashedPassword = hasher.HashPassword(seedAdmin, "P@ssw0rd");
			seedAdmin.PasswordHash = hashedPassword;

			modelBuilder.Entity<UserAccount>().HasData(
				seedAdmin
				);

			// Assign admin role to seeded admin user
			modelBuilder.Entity<IdentityUserRole<int>>()
				.HasData(new IdentityUserRole<int>
				{
					RoleId = 1,
					UserId = 1,
				});


		}
	}
}
