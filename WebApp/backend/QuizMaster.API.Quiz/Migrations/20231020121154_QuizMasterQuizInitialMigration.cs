using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuizMaster.API.Quiz.Migrations
{
    /// <inheritdoc />
    public partial class QuizMasterQuizInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QCategoryDesc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActiveData = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Difficulties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QDifficultyDesc = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ActiveData = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Difficulties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QTypeDesc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    QDetailRequired = table.Column<bool>(type: "bit", nullable: false),
                    ActiveData = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QStatement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QAudio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QDifficultyId = table.Column<int>(type: "int", nullable: false),
                    QCategoryId = table.Column<int>(type: "int", nullable: false),
                    QTypeId = table.Column<int>(type: "int", nullable: false),
                    ActiveData = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Categories_QCategoryId",
                        column: x => x.QCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Questions_Difficulties_QDifficultyId",
                        column: x => x.QDifficultyId,
                        principalTable: "Difficulties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Questions_Types_QTypeId",
                        column: x => x.QTypeId,
                        principalTable: "Types",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Details",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QDetailDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    ActiveData = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Details", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Details_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ActiveData", "CreatedByUserId", "DateCreated", "DateUpdated", "QCategoryDesc", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, true, 1, new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(6521), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Science", null },
                    { 2, true, 1, new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(7462), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Movies", null },
                    { 3, true, 1, new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(7466), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Animals", null },
                    { 4, true, 1, new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(7468), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Places", null },
                    { 5, true, 1, new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(7469), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "People", null },
                    { 6, true, 1, new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(7472), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System Operations and Maintenance", null },
                    { 7, true, 1, new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(7473), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Data Structures", null },
                    { 8, true, 1, new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(7474), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Algorithms", null }
                });

            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "ActiveData", "CreatedByUserId", "DateCreated", "DateUpdated", "QDifficultyDesc", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, true, 1, new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(867), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Easy", null },
                    { 2, true, 1, new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(1876), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Average", null },
                    { 3, true, 1, new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(1881), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Difficult", null }
                });

            migrationBuilder.InsertData(
                table: "Types",
                columns: new[] { "Id", "ActiveData", "CreatedByUserId", "DateCreated", "DateUpdated", "QDetailRequired", "QTypeDesc", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, true, 1, new DateTime(2023, 10, 20, 20, 11, 54, 403, DateTimeKind.Local).AddTicks(3591), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Multiple Choice", null },
                    { 2, true, 1, new DateTime(2023, 10, 20, 20, 11, 54, 404, DateTimeKind.Local).AddTicks(6907), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Multiple Choice + Audio", null },
                    { 3, true, 1, new DateTime(2023, 10, 20, 20, 11, 54, 404, DateTimeKind.Local).AddTicks(6976), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "True or False", null },
                    { 4, true, 1, new DateTime(2023, 10, 20, 20, 11, 54, 404, DateTimeKind.Local).AddTicks(6982), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Type Answer", null },
                    { 5, true, 1, new DateTime(2023, 10, 20, 20, 11, 54, 404, DateTimeKind.Local).AddTicks(6983), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Slider", null },
                    { 6, true, 1, new DateTime(2023, 10, 20, 20, 11, 54, 404, DateTimeKind.Local).AddTicks(6989), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Puzzle", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Details_QuestionId",
                table: "Details",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QCategoryId",
                table: "Questions",
                column: "QCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QDifficultyId",
                table: "Questions",
                column: "QDifficultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QTypeId",
                table: "Questions",
                column: "QTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Details");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Difficulties");

            migrationBuilder.DropTable(
                name: "Types");
        }
    }
}
