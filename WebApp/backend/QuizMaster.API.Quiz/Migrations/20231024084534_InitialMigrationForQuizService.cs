using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizMaster.API.Quiz.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationForQuizService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 10, 24, 16, 45, 33, 455, DateTimeKind.Local).AddTicks(6800));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 10, 24, 16, 45, 33, 455, DateTimeKind.Local).AddTicks(9305));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 10, 24, 16, 45, 33, 455, DateTimeKind.Local).AddTicks(9317));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 10, 24, 16, 45, 33, 455, DateTimeKind.Local).AddTicks(9320));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 10, 24, 16, 45, 33, 455, DateTimeKind.Local).AddTicks(9322));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 10, 24, 16, 45, 33, 455, DateTimeKind.Local).AddTicks(9330));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2023, 10, 24, 16, 45, 33, 455, DateTimeKind.Local).AddTicks(9333));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateCreated",
                value: new DateTime(2023, 10, 24, 16, 45, 33, 455, DateTimeKind.Local).AddTicks(9335));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 10, 24, 16, 45, 33, 454, DateTimeKind.Local).AddTicks(3567));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 10, 24, 16, 45, 33, 454, DateTimeKind.Local).AddTicks(6524));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 10, 24, 16, 45, 33, 454, DateTimeKind.Local).AddTicks(6534));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 10, 24, 16, 45, 33, 448, DateTimeKind.Local).AddTicks(188));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 10, 24, 16, 45, 33, 450, DateTimeKind.Local).AddTicks(3226));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 10, 24, 16, 45, 33, 450, DateTimeKind.Local).AddTicks(3387));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 10, 24, 16, 45, 33, 450, DateTimeKind.Local).AddTicks(3393));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 10, 24, 16, 45, 33, 450, DateTimeKind.Local).AddTicks(3395));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 10, 24, 16, 45, 33, 450, DateTimeKind.Local).AddTicks(3413));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(6521));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(7462));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(7466));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(7468));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(7469));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(7472));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(7473));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateCreated",
                value: new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(7474));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(867));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(1876));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 10, 20, 20, 11, 54, 406, DateTimeKind.Local).AddTicks(1881));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 10, 20, 20, 11, 54, 403, DateTimeKind.Local).AddTicks(3591));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 10, 20, 20, 11, 54, 404, DateTimeKind.Local).AddTicks(6907));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 10, 20, 20, 11, 54, 404, DateTimeKind.Local).AddTicks(6976));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 10, 20, 20, 11, 54, 404, DateTimeKind.Local).AddTicks(6982));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 10, 20, 20, 11, 54, 404, DateTimeKind.Local).AddTicks(6983));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 10, 20, 20, 11, 54, 404, DateTimeKind.Local).AddTicks(6989));
        }
    }
}
