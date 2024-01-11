using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizMaster.API.Monitoring.Migrations
{
    /// <inheritdoc />
    public partial class addusername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "UserAuditTrails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "UserAuditTrails");
        }
    }
}
