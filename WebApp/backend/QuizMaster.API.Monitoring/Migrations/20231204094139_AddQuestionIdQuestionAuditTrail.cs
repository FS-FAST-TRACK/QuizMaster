using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizMaster.API.Monitoring.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionIdQuestionAuditTrail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "QuestionAuditTrails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "QuestionAuditTrails");
        }
    }
}
