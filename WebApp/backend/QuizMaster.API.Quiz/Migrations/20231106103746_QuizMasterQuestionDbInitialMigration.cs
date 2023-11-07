using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuizMaster.API.Quiz.Migrations
{
    /// <inheritdoc />
    public partial class QuizMasterQuestionDbInitialMigration : Migration
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
                    QTime = table.Column<int>(type: "int", nullable: false),
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Questions_Difficulties_QDifficultyId",
                        column: x => x.QDifficultyId,
                        principalTable: "Difficulties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Questions_Types_QTypeId",
                        column: x => x.QTypeId,
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ActiveData", "CreatedByUserId", "DateCreated", "DateUpdated", "QCategoryDesc", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, true, 1, new DateTime(2023, 11, 6, 18, 37, 46, 229, DateTimeKind.Local).AddTicks(9746), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Science", null },
                    { 2, true, 1, new DateTime(2023, 11, 6, 18, 37, 46, 230, DateTimeKind.Local).AddTicks(786), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Movies", null },
                    { 3, true, 1, new DateTime(2023, 11, 6, 18, 37, 46, 230, DateTimeKind.Local).AddTicks(791), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Animals", null },
                    { 4, true, 1, new DateTime(2023, 11, 6, 18, 37, 46, 230, DateTimeKind.Local).AddTicks(792), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Places", null },
                    { 5, true, 1, new DateTime(2023, 11, 6, 18, 37, 46, 230, DateTimeKind.Local).AddTicks(793), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "People", null },
                    { 6, true, 1, new DateTime(2023, 11, 6, 18, 37, 46, 230, DateTimeKind.Local).AddTicks(796), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System Operations and Maintenance", null },
                    { 7, true, 1, new DateTime(2023, 11, 6, 18, 37, 46, 230, DateTimeKind.Local).AddTicks(797), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Data Structures", null },
                    { 8, true, 1, new DateTime(2023, 11, 6, 18, 37, 46, 230, DateTimeKind.Local).AddTicks(798), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Algorithms", null }
                });

            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "ActiveData", "CreatedByUserId", "DateCreated", "DateUpdated", "QDifficultyDesc", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, true, 1, new DateTime(2023, 11, 6, 18, 37, 46, 229, DateTimeKind.Local).AddTicks(3545), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Easy", null },
                    { 2, true, 1, new DateTime(2023, 11, 6, 18, 37, 46, 229, DateTimeKind.Local).AddTicks(4645), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Average", null },
                    { 3, true, 1, new DateTime(2023, 11, 6, 18, 37, 46, 229, DateTimeKind.Local).AddTicks(4814), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Difficult", null }
                });

            migrationBuilder.InsertData(
                table: "Types",
                columns: new[] { "Id", "ActiveData", "CreatedByUserId", "DateCreated", "DateUpdated", "QDetailRequired", "QTypeDesc", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, true, 1, new DateTime(2023, 11, 6, 18, 37, 46, 226, DateTimeKind.Local).AddTicks(4385), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Multiple Choice", null },
                    { 2, true, 1, new DateTime(2023, 11, 6, 18, 37, 46, 227, DateTimeKind.Local).AddTicks(7069), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Multiple Choice + Audio", null },
                    { 3, true, 1, new DateTime(2023, 11, 6, 18, 37, 46, 227, DateTimeKind.Local).AddTicks(7083), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "True or False", null },
                    { 4, true, 1, new DateTime(2023, 11, 6, 18, 37, 46, 227, DateTimeKind.Local).AddTicks(7084), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Type Answer", null },
                    { 5, true, 1, new DateTime(2023, 11, 6, 18, 37, 46, 227, DateTimeKind.Local).AddTicks(7085), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Slider", null },
                    { 6, true, 1, new DateTime(2023, 11, 6, 18, 37, 46, 227, DateTimeKind.Local).AddTicks(7086), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Puzzle", null }
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
