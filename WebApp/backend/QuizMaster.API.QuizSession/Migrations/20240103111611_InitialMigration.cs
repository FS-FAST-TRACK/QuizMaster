using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuizMaster.API.QuizSession.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QCategoryDesc = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ActiveData = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Difficulties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QDifficultyDesc = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    ActiveData = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Difficulties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuizParticipants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QParticipantDesc = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    QRoomId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false),
                    QStartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    QEndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    QStatus = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActiveData = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizParticipants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuizRoomDatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QRoomId = table.Column<int>(type: "INTEGER", nullable: false),
                    SetQuizRoomJSON = table.Column<string>(type: "TEXT", nullable: false),
                    ParticipantsJSON = table.Column<string>(type: "TEXT", nullable: false),
                    SessionId = table.Column<string>(type: "TEXT", nullable: false),
                    StartedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndedDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HostId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActiveData = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizRoomDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuizRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QRoomDesc = table.Column<string>(type: "TEXT", nullable: false),
                    QRoomPin = table.Column<int>(type: "INTEGER", nullable: false),
                    RoomOptions = table.Column<string>(type: "TEXT", nullable: false),
                    ActiveData = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizRooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QSetName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    QSetDesc = table.Column<string>(type: "TEXT", nullable: false),
                    ActiveData = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QTypeDesc = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    QDetailRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                    ActiveData = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SetQuizRooms",
                columns: table => new
                {
                    QSetId = table.Column<int>(type: "INTEGER", nullable: false),
                    QRoomId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActiveData = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetQuizRooms", x => new { x.QSetId, x.QRoomId });
                    table.ForeignKey(
                        name: "FK_SetQuizRooms_QuizRooms_QRoomId",
                        column: x => x.QRoomId,
                        principalTable: "QuizRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SetQuizRooms_Sets_QSetId",
                        column: x => x.QSetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QStatement = table.Column<string>(type: "TEXT", nullable: false),
                    QImage = table.Column<string>(type: "TEXT", nullable: true),
                    QAudio = table.Column<string>(type: "TEXT", nullable: true),
                    QTime = table.Column<int>(type: "INTEGER", nullable: false),
                    QDifficultyId = table.Column<int>(type: "INTEGER", nullable: false),
                    QCategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    QTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActiveData = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "INTEGER", nullable: true)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QDetailDesc = table.Column<string>(type: "TEXT", nullable: false),
                    QuestionId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActiveData = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "INTEGER", nullable: true)
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
                name: "QuestionSets",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "INTEGER", nullable: false),
                    SetId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActiveData = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionSets", x => new { x.QuestionId, x.SetId });
                    table.ForeignKey(
                        name: "FK_QuestionSets_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionSets_Sets_SetId",
                        column: x => x.SetId,
                        principalTable: "Sets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DetailTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DTypeDesc = table.Column<string>(type: "TEXT", nullable: false),
                    ActiveData = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    QuestionDetailId = table.Column<int>(type: "INTEGER", nullable: true)
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
                    QuestionDetailId = table.Column<int>(type: "INTEGER", nullable: false),
                    DetailTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActiveData = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "INTEGER", nullable: true)
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
                    { 1, true, 1, new DateTime(2024, 1, 3, 19, 16, 11, 583, DateTimeKind.Local).AddTicks(6204), null, "Science", null },
                    { 2, true, 1, new DateTime(2024, 1, 3, 19, 16, 11, 583, DateTimeKind.Local).AddTicks(6681), null, "Movies", null },
                    { 3, true, 1, new DateTime(2024, 1, 3, 19, 16, 11, 583, DateTimeKind.Local).AddTicks(6684), null, "Animals", null },
                    { 4, true, 1, new DateTime(2024, 1, 3, 19, 16, 11, 583, DateTimeKind.Local).AddTicks(6685), null, "Places", null },
                    { 5, true, 1, new DateTime(2024, 1, 3, 19, 16, 11, 583, DateTimeKind.Local).AddTicks(6686), null, "People", null },
                    { 6, true, 1, new DateTime(2024, 1, 3, 19, 16, 11, 583, DateTimeKind.Local).AddTicks(6715), null, "System Operations and Maintenance", null },
                    { 7, true, 1, new DateTime(2024, 1, 3, 19, 16, 11, 583, DateTimeKind.Local).AddTicks(6716), null, "Data Structures", null },
                    { 8, true, 1, new DateTime(2024, 1, 3, 19, 16, 11, 583, DateTimeKind.Local).AddTicks(6716), null, "Algorithms", null }
                });

            migrationBuilder.InsertData(
                table: "DetailTypes",
                columns: new[] { "Id", "ActiveData", "CreatedByUserId", "DTypeDesc", "DateCreated", "DateUpdated", "QuestionDetailId", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, false, 0, "Answer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null },
                    { 2, false, 0, "Option", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null },
                    { 3, false, 0, "Minimum", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null },
                    { 4, false, 0, "Maximum", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null },
                    { 5, false, 0, "Interval", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null },
                    { 6, false, 0, "Margin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null },
                    { 7, false, 0, "TextToAudio", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null },
                    { 8, false, 0, "Language", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "ActiveData", "CreatedByUserId", "DateCreated", "DateUpdated", "QDifficultyDesc", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, true, 1, new DateTime(2024, 1, 3, 19, 16, 11, 583, DateTimeKind.Local).AddTicks(2621), null, "Easy", null },
                    { 2, true, 1, new DateTime(2024, 1, 3, 19, 16, 11, 583, DateTimeKind.Local).AddTicks(3218), null, "Average", null },
                    { 3, true, 1, new DateTime(2024, 1, 3, 19, 16, 11, 583, DateTimeKind.Local).AddTicks(3291), null, "Difficult", null }
                });

            migrationBuilder.InsertData(
                table: "Types",
                columns: new[] { "Id", "ActiveData", "CreatedByUserId", "DateCreated", "DateUpdated", "QDetailRequired", "QTypeDesc", "UpdatedByUserId" },
                values: new object[,]
                {
                    { 1, true, 1, new DateTime(2024, 1, 3, 19, 16, 11, 580, DateTimeKind.Local).AddTicks(9897), null, true, "Multiple Choice", null },
                    { 2, true, 1, new DateTime(2024, 1, 3, 19, 16, 11, 582, DateTimeKind.Local).AddTicks(1628), null, true, "Multiple Choice + Audio", null },
                    { 3, true, 1, new DateTime(2024, 1, 3, 19, 16, 11, 582, DateTimeKind.Local).AddTicks(1635), null, false, "True or False", null },
                    { 4, true, 1, new DateTime(2024, 1, 3, 19, 16, 11, 582, DateTimeKind.Local).AddTicks(1636), null, true, "Type Answer", null },
                    { 5, true, 1, new DateTime(2024, 1, 3, 19, 16, 11, 582, DateTimeKind.Local).AddTicks(1637), null, true, "Slider", null },
                    { 6, true, 1, new DateTime(2024, 1, 3, 19, 16, 11, 582, DateTimeKind.Local).AddTicks(1638), null, true, "Puzzle", null }
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

            migrationBuilder.CreateIndex(
                name: "IX_QuestionSets_SetId",
                table: "QuestionSets",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_SetQuizRooms_QRoomId",
                table: "SetQuizRooms",
                column: "QRoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionDetailTypes");

            migrationBuilder.DropTable(
                name: "QuestionSets");

            migrationBuilder.DropTable(
                name: "QuizParticipants");

            migrationBuilder.DropTable(
                name: "QuizRoomDatas");

            migrationBuilder.DropTable(
                name: "SetQuizRooms");

            migrationBuilder.DropTable(
                name: "DetailTypes");

            migrationBuilder.DropTable(
                name: "QuizRooms");

            migrationBuilder.DropTable(
                name: "Sets");

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
