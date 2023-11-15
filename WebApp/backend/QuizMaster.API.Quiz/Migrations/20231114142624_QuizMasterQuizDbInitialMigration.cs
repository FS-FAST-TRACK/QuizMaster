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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Questions_Difficulties_QDifficultyId",
                        column: x => x.QDifficultyId,
                        principalTable: "Difficulties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Questions_Types_QTypeId",
                        column: x => x.QTypeId,
                        principalTable: "Types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionDetails",
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
                    table.PrimaryKey("PK_QuestionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionDetails_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true),
                    QuestionDetailId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DetailTypes_QuestionDetails_QuestionDetailId",
                        column: x => x.QuestionDetailId,
                        principalTable: "QuestionDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuestionDetailTypes",
                columns: table => new
                {
                    QuestionDetailId = table.Column<int>(type: "int", nullable: false),
                    DetailTypeId = table.Column<int>(type: "int", nullable: false),
                    ActiveData = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionDetailTypes", x => new { x.QuestionDetailId, x.DetailTypeId });
                    table.ForeignKey(
                        name: "FK_QuestionDetailTypes_DetailTypes_DetailTypeId",
                        column: x => x.DetailTypeId,
                        principalTable: "DetailTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionDetailTypes_QuestionDetails_QuestionDetailId",
                        column: x => x.QuestionDetailId,
                        principalTable: "QuestionDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ActiveData", "CreatedByUserId", "DateCreated", "DateUpdated", "QCategoryDesc", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 782, DateTimeKind.Local).AddTicks(4541), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Science", null },
                    { 2, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 782, DateTimeKind.Local).AddTicks(5806), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Movies", null },
                    { 3, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 782, DateTimeKind.Local).AddTicks(5812), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Animals", null },
                    { 4, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 782, DateTimeKind.Local).AddTicks(5813), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Places", null },
                    { 5, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 782, DateTimeKind.Local).AddTicks(5814), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "People", null },
                    { 6, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 782, DateTimeKind.Local).AddTicks(5826), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System Operations and Maintenance", null },
                    { 7, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 782, DateTimeKind.Local).AddTicks(5827), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Data Structures", null },
                    { 8, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 782, DateTimeKind.Local).AddTicks(5828), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Algorithms", null }
                });

            migrationBuilder.InsertData(
                table: "DetailTypes",
                columns: new[] { "Id", "ActiveData", "CreatedByUserId", "DTypeDesc", "DateCreated", "DateUpdated", "QuestionDetailId", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, false, 0, "Answer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 2, false, 0, "Option", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 3, false, 0, "Minimum", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 4, false, 0, "Maximum", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 5, false, 0, "Interval", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 6, false, 0, "Margin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 7, false, 0, "TextToAudio", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 8, false, 0, "Language", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null }
                });

            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "ActiveData", "CreatedByUserId", "DateCreated", "DateUpdated", "QDifficultyDesc", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 781, DateTimeKind.Local).AddTicks(5905), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Easy", null },
                    { 2, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 781, DateTimeKind.Local).AddTicks(7628), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Average", null },
                    { 3, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 781, DateTimeKind.Local).AddTicks(7771), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Difficult", null }
                });

            migrationBuilder.InsertData(
                table: "Types",
                columns: new[] { "Id", "ActiveData", "CreatedByUserId", "DateCreated", "DateUpdated", "QDetailRequired", "QTypeDesc", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 776, DateTimeKind.Local).AddTicks(5234), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Multiple Choice", null },
                    { 2, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 778, DateTimeKind.Local).AddTicks(7491), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Multiple Choice + Audio", null },
                    { 3, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 778, DateTimeKind.Local).AddTicks(7522), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "True or False", null },
                    { 4, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 778, DateTimeKind.Local).AddTicks(7524), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Type Answer", null },
                    { 5, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 778, DateTimeKind.Local).AddTicks(7526), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Slider", null },
                    { 6, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 778, DateTimeKind.Local).AddTicks(7527), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Puzzle", null }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "ActiveData", "CreatedByUserId", "DateCreated", "DateUpdated", "QAudio", "QCategoryId", "QDifficultyId", "QImage", "QStatement", "QTime", "QTypeId", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, true, 0, new DateTime(2023, 11, 14, 22, 26, 23, 783, DateTimeKind.Local).AddTicks(9253), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 1, "", "What is the primary gas in Earth's atmosphere?", 30, 1, null },
                    { 2, true, 0, new DateTime(2023, 11, 14, 22, 26, 23, 784, DateTimeKind.Local).AddTicks(8287), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 2, "", "True or False: Earth's moon is larger than Pluto.", 20, 3, null },
                    { 3, true, 0, new DateTime(2023, 11, 14, 22, 26, 23, 784, DateTimeKind.Local).AddTicks(9447), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", 1, 2, "", "What is the number divisible by 3 and 7, anb between 10 and 50?", 25, 5, null }
                });

            migrationBuilder.InsertData(
                table: "QuestionDetails",
                columns: new[] { "Id", "ActiveData", "CreatedByUserId", "DateCreated", "DateUpdated", "QDetailDesc", "QuestionId", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 785, DateTimeKind.Local).AddTicks(6520), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Oxygen", 1, null },
                    { 2, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 785, DateTimeKind.Local).AddTicks(7828), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nitrogen", 1, null },
                    { 3, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 785, DateTimeKind.Local).AddTicks(7833), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Carbon Dioxide", 1, null },
                    { 4, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 785, DateTimeKind.Local).AddTicks(7834), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hydrogen", 1, null },
                    { 5, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 785, DateTimeKind.Local).AddTicks(7835), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "True", 2, null },
                    { 6, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 785, DateTimeKind.Local).AddTicks(7842), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "42", 3, null },
                    { 7, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 785, DateTimeKind.Local).AddTicks(7843), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "10", 3, null },
                    { 8, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 785, DateTimeKind.Local).AddTicks(7844), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "50", 3, null },
                    { 9, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 785, DateTimeKind.Local).AddTicks(7845), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", 3, null }
                });

            migrationBuilder.InsertData(
                table: "QuestionDetailTypes",
                columns: new[] { "DetailTypeId", "QuestionDetailId", "ActiveData", "CreatedByUserId", "DateCreated", "DateUpdated", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 2, 1, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(3760), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 1, 2, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(4559), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, 2, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(4563), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, 3, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(4695), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, 4, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(4697), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 1, 5, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(4704), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 1, 6, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(4705), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, 7, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(4705), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4, 8, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(5087), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 5, 9, true, 1, new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(5096), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetailTypes_QuestionDetailId",
                table: "DetailTypes",
                column: "QuestionDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDetails_QuestionId",
                table: "QuestionDetails",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDetailTypes_DetailTypeId",
                table: "QuestionDetailTypes",
                column: "DetailTypeId");

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
                name: "QuestionDetailTypes");

            migrationBuilder.DropTable(
                name: "DetailTypes");

            migrationBuilder.DropTable(
                name: "QuestionDetails");

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
