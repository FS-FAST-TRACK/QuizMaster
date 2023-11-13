using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuizMaster.API.Quiz.Migrations
{
    /// <inheritdoc />
    public partial class QuizMasterQuizDbInitialMigration : Migration
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
                name: "Details",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DetailDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActiveData = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Details", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DetailTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DTypeDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActiveData = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailTypes", x => x.Id);
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
                name: "QuestionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    DetailId = table.Column<int>(type: "int", nullable: false),
                    QuestionDetailTypeId = table.Column<int>(type: "int", nullable: false),
                    ActiveData = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionDetails_DetailTypes_QuestionDetailTypeId",
                        column: x => x.QuestionDetailTypeId,
                        principalTable: "DetailTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionDetails_Details_DetailId",
                        column: x => x.DetailId,
                        principalTable: "Details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionDetails_Questions_QuestionId",
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
                    { 1, true, 1, new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(289), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Science", null },
                    { 2, true, 1, new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(1659), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Movies", null },
                    { 3, true, 1, new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(1664), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Animals", null },
                    { 4, true, 1, new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(1666), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Places", null },
                    { 5, true, 1, new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(1667), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "People", null },
                    { 6, true, 1, new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(1675), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System Operations and Maintenance", null },
                    { 7, true, 1, new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(1676), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Data Structures", null },
                    { 8, true, 1, new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(1677), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Algorithms", null }
                });

            migrationBuilder.InsertData(
                table: "DetailTypes",
                columns: new[] { "Id", "ActiveData", "CreatedByUserId", "DTypeDesc", "DateCreated", "DateUpdated", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, true, 1, "Answer", new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(5944), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, true, 1, "Option", new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(6715), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, true, 1, "Minimum", new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(6719), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4, true, 1, "Maximum", new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(6720), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 5, true, 1, "Interval", new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(6720), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 6, true, 1, "Margin", new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(6721), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 7, true, 1, "TextToAudio", new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(6722), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 8, true, 1, "Language", new DateTime(2023, 11, 13, 23, 1, 56, 334, DateTimeKind.Local).AddTicks(6723), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "ActiveData", "CreatedByUserId", "DateCreated", "DateUpdated", "QDifficultyDesc", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, true, 1, new DateTime(2023, 11, 13, 23, 1, 56, 333, DateTimeKind.Local).AddTicks(2307), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Easy", null },
                    { 2, true, 1, new DateTime(2023, 11, 13, 23, 1, 56, 333, DateTimeKind.Local).AddTicks(3675), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Average", null },
                    { 3, true, 1, new DateTime(2023, 11, 13, 23, 1, 56, 333, DateTimeKind.Local).AddTicks(3751), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Difficult", null }
                });

            migrationBuilder.InsertData(
                table: "Types",
                columns: new[] { "Id", "ActiveData", "CreatedByUserId", "DateCreated", "DateUpdated", "QDetailRequired", "QTypeDesc", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, true, 1, new DateTime(2023, 11, 13, 23, 1, 56, 329, DateTimeKind.Local).AddTicks(6012), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Multiple Choice", null },
                    { 2, true, 1, new DateTime(2023, 11, 13, 23, 1, 56, 330, DateTimeKind.Local).AddTicks(8490), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Multiple Choice + Audio", null },
                    { 3, true, 1, new DateTime(2023, 11, 13, 23, 1, 56, 330, DateTimeKind.Local).AddTicks(8512), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "True or False", null },
                    { 4, true, 1, new DateTime(2023, 11, 13, 23, 1, 56, 330, DateTimeKind.Local).AddTicks(8514), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Type Answer", null },
                    { 5, true, 1, new DateTime(2023, 11, 13, 23, 1, 56, 330, DateTimeKind.Local).AddTicks(8515), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Slider", null },
                    { 6, true, 1, new DateTime(2023, 11, 13, 23, 1, 56, 330, DateTimeKind.Local).AddTicks(8516), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Puzzle", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDetails_DetailId",
                table: "QuestionDetails",
                column: "DetailId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDetails_QuestionDetailTypeId",
                table: "QuestionDetails",
                column: "QuestionDetailTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDetails_QuestionId",
                table: "QuestionDetails",
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
                name: "QuestionDetails");

            migrationBuilder.DropTable(
                name: "DetailTypes");

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
