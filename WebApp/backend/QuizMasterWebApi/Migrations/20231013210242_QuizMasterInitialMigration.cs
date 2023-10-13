using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuizMaster.Account.API.Migrations
{
    /// <inheritdoc />
    public partial class QuizMasterInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserRoleDesc = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserRoleId = table.Column<int>(type: "int", nullable: false),
                    ActiveData = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAccounts_UserAccounts_UpdatedByUserId",
                        column: x => x.UpdatedByUserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserAccounts_UserRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QCategoryDesc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActiveData = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionCategories_UserAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionDifficulties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QDifficultyDesc = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ActiveData = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionDifficulties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionDifficulties_UserAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QTypeDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QDetailRequired = table.Column<bool>(type: "bit", nullable: false),
                    ActiveData = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionTypes_UserAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    QAudio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QDifficultyId = table.Column<int>(type: "int", nullable: true),
                    QCategoryId = table.Column<int>(type: "int", nullable: true),
                    QTypeId = table.Column<int>(type: "int", nullable: true),
                    ActiveData = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_QuestionCategories_QCategoryId",
                        column: x => x.QCategoryId,
                        principalTable: "QuestionCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Questions_QuestionDifficulties_QDifficultyId",
                        column: x => x.QDifficultyId,
                        principalTable: "QuestionDifficulties",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Questions_QuestionTypes_QTypeId",
                        column: x => x.QTypeId,
                        principalTable: "QuestionTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Questions_UserAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccounts",
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
                    QId = table.Column<int>(type: "int", nullable: true),
                    ActiveData = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionDetails_Questions_QId",
                        column: x => x.QId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuestionDetails_UserAccounts_UserId",
                        column: x => x.UserId,
                        principalTable: "UserAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "UserRoleDesc" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "UserAccounts",
                columns: new[] { "Id", "ActiveData", "DateCreated", "DateUpdated", "EmailAddress", "FirstName", "LastName", "Password", "UpdatedByUserId", "UserName", "UserRoleId" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2023, 10, 14, 5, 2, 42, 63, DateTimeKind.Local).AddTicks(1057), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@fullscale.io", "admin", "admin", "b03ddf3ca2e714a6548e7495e2a03f5e824eaac9837cd7f159c67b90fb4b7342", null, "admin", 1 },
                    { 2, true, new DateTime(2023, 10, 14, 5, 2, 42, 63, DateTimeKind.Local).AddTicks(1107), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user@fullscale.io", "user", "user", "b03ddf3ca2e714a6548e7495e2a03f5e824eaac9837cd7f159c67b90fb4b7342", null, "user", 2 }
                });

            migrationBuilder.InsertData(
                table: "QuestionCategories",
                columns: new[] { "Id", "ActiveData", "DateCreated", "DateUpdated", "QCategoryDesc", "UserId" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2023, 10, 14, 5, 2, 42, 63, DateTimeKind.Local).AddTicks(1343), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Science", 2 },
                    { 2, true, new DateTime(2023, 10, 14, 5, 2, 42, 63, DateTimeKind.Local).AddTicks(1345), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Movies", 2 },
                    { 3, true, new DateTime(2023, 10, 14, 5, 2, 42, 63, DateTimeKind.Local).AddTicks(1346), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Animals", 2 },
                    { 4, true, new DateTime(2023, 10, 14, 5, 2, 42, 63, DateTimeKind.Local).AddTicks(1347), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Places", 2 },
                    { 5, true, new DateTime(2023, 10, 14, 5, 2, 42, 63, DateTimeKind.Local).AddTicks(1348), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "People", 2 },
                    { 6, true, new DateTime(2023, 10, 14, 5, 2, 42, 63, DateTimeKind.Local).AddTicks(1349), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System Operations and Maintenance", 2 },
                    { 7, true, new DateTime(2023, 10, 14, 5, 2, 42, 63, DateTimeKind.Local).AddTicks(1350), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Data Structures", 2 },
                    { 8, true, new DateTime(2023, 10, 14, 5, 2, 42, 63, DateTimeKind.Local).AddTicks(1351), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Algorithms", 2 }
                });

            migrationBuilder.InsertData(
                table: "QuestionDifficulties",
                columns: new[] { "Id", "ActiveData", "DateCreated", "DateUpdated", "QDifficultyDesc", "UserId" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2023, 10, 14, 5, 2, 42, 63, DateTimeKind.Local).AddTicks(1324), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Easy", 2 },
                    { 2, true, new DateTime(2023, 10, 14, 5, 2, 42, 63, DateTimeKind.Local).AddTicks(1326), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Average", 2 },
                    { 3, true, new DateTime(2023, 10, 14, 5, 2, 42, 63, DateTimeKind.Local).AddTicks(1328), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Difficult", 2 }
                });

            migrationBuilder.InsertData(
                table: "QuestionTypes",
                columns: new[] { "Id", "ActiveData", "DateCreated", "DateUpdated", "QDetailRequired", "QTypeDesc", "UserId" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2023, 10, 14, 5, 2, 42, 63, DateTimeKind.Local).AddTicks(1368), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Multiple Choice", 2 },
                    { 2, true, new DateTime(2023, 10, 14, 5, 2, 42, 63, DateTimeKind.Local).AddTicks(1369), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Multiple Choice + Audio", 2 },
                    { 3, true, new DateTime(2023, 10, 14, 5, 2, 42, 63, DateTimeKind.Local).AddTicks(1371), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "True or False", 2 },
                    { 4, true, new DateTime(2023, 10, 14, 5, 2, 42, 63, DateTimeKind.Local).AddTicks(1372), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Type Answer", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionCategories_UserId",
                table: "QuestionCategories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDetails_QId",
                table: "QuestionDetails",
                column: "QId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDetails_UserId",
                table: "QuestionDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDifficulties_UserId",
                table: "QuestionDifficulties",
                column: "UserId");

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
                name: "IX_Questions_UserId",
                table: "Questions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTypes_UserId",
                table: "QuestionTypes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_UpdatedByUserId",
                table: "UserAccounts",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_UserRoleId",
                table: "UserAccounts",
                column: "UserRoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionDetails");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "QuestionCategories");

            migrationBuilder.DropTable(
                name: "QuestionDifficulties");

            migrationBuilder.DropTable(
                name: "QuestionTypes");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "UserRoles");
        }
    }
}
