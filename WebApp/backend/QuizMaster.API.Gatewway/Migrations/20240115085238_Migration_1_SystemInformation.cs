using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizMaster.API.Gateway.Migrations
{
    /// <inheritdoc />
    public partial class Migration_1_SystemInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemAboutData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Version = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Web_link = table.Column<string>(type: "TEXT", nullable: false),
                    Mobile_link = table.Column<string>(type: "TEXT", nullable: false),
                    Ios_link = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemAboutData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemContactData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Contact = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemContactData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemReachingContacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Lastname = table.Column<string>(type: "TEXT", nullable: false),
                    Firstname = table.Column<string>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemReachingContacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    StarRating = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemReviews", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SystemAboutData",
                columns: new[] { "Id", "Description", "Ios_link", "Mobile_link", "Version", "Web_link" },
                values: new object[] { 1, "Lorem ipsum dolor sit amet consectetur. Pulvinar porta egestas molestie purus faucibus neque malesuada lectus. Lacus auctor sit felis sed ultrices nullam sapien ornare justo. Proin adipiscing viverra vestibulum arcu sit. Suscipit bibendum ullamcorper ut et dolor quisque nulla et.", "", "", "1.0.0", "" });

            migrationBuilder.InsertData(
                table: "SystemContactData",
                columns: new[] { "Id", "Contact", "Email" },
                values: new object[] { 1, "09205195701", "admin.quizmaster@gmail.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemAboutData");

            migrationBuilder.DropTable(
                name: "SystemContactData");

            migrationBuilder.DropTable(
                name: "SystemReachingContacts");

            migrationBuilder.DropTable(
                name: "SystemReviews");
        }
    }
}
