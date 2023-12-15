using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizMaster.API.QuizSession.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuizParticipants_QuizRooms_QRoomId",
                table: "QuizParticipants");

            migrationBuilder.DropIndex(
                name: "IX_QuizParticipants_QRoomId",
                table: "QuizParticipants");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 13, 19, 16, 12, 308, DateTimeKind.Local).AddTicks(7725));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 13, 19, 16, 12, 308, DateTimeKind.Local).AddTicks(8335));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 13, 19, 16, 12, 308, DateTimeKind.Local).AddTicks(8338));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 12, 13, 19, 16, 12, 308, DateTimeKind.Local).AddTicks(8339));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 12, 13, 19, 16, 12, 308, DateTimeKind.Local).AddTicks(8340));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 12, 13, 19, 16, 12, 308, DateTimeKind.Local).AddTicks(8343));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2023, 12, 13, 19, 16, 12, 308, DateTimeKind.Local).AddTicks(8344));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateCreated",
                value: new DateTime(2023, 12, 13, 19, 16, 12, 308, DateTimeKind.Local).AddTicks(8345));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 13, 19, 16, 12, 308, DateTimeKind.Local).AddTicks(4268));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 13, 19, 16, 12, 308, DateTimeKind.Local).AddTicks(4828));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 13, 19, 16, 12, 308, DateTimeKind.Local).AddTicks(4865));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 13, 19, 16, 12, 306, DateTimeKind.Local).AddTicks(2585));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 13, 19, 16, 12, 307, DateTimeKind.Local).AddTicks(4792));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 13, 19, 16, 12, 307, DateTimeKind.Local).AddTicks(4799));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 12, 13, 19, 16, 12, 307, DateTimeKind.Local).AddTicks(4800));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 12, 13, 19, 16, 12, 307, DateTimeKind.Local).AddTicks(4801));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 12, 13, 19, 16, 12, 307, DateTimeKind.Local).AddTicks(4802));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 22, 53, 23, 170, DateTimeKind.Local).AddTicks(2075));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 22, 53, 23, 170, DateTimeKind.Local).AddTicks(2605));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 22, 53, 23, 170, DateTimeKind.Local).AddTicks(2608));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 22, 53, 23, 170, DateTimeKind.Local).AddTicks(2609));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 22, 53, 23, 170, DateTimeKind.Local).AddTicks(2609));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 22, 53, 23, 170, DateTimeKind.Local).AddTicks(2613));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 22, 53, 23, 170, DateTimeKind.Local).AddTicks(2614));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 22, 53, 23, 170, DateTimeKind.Local).AddTicks(2615));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 22, 53, 23, 169, DateTimeKind.Local).AddTicks(8386));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 22, 53, 23, 169, DateTimeKind.Local).AddTicks(8960));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 22, 53, 23, 169, DateTimeKind.Local).AddTicks(9031));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 22, 53, 23, 167, DateTimeKind.Local).AddTicks(1647));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 22, 53, 23, 168, DateTimeKind.Local).AddTicks(8199));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 22, 53, 23, 168, DateTimeKind.Local).AddTicks(8209));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 22, 53, 23, 168, DateTimeKind.Local).AddTicks(8210));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 22, 53, 23, 168, DateTimeKind.Local).AddTicks(8211));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 22, 53, 23, 168, DateTimeKind.Local).AddTicks(8212));

            migrationBuilder.CreateIndex(
                name: "IX_QuizParticipants_QRoomId",
                table: "QuizParticipants",
                column: "QRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizParticipants_QuizRooms_QRoomId",
                table: "QuizParticipants",
                column: "QRoomId",
                principalTable: "QuizRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
