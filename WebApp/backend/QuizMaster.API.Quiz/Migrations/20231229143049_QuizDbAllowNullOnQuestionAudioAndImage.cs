using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizMaster.API.Quiz.Migrations
{
    /// <inheritdoc />
    public partial class QuizDbAllowNullOnQuestionAudioAndImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "QImage",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "QAudio",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 109, DateTimeKind.Local).AddTicks(8625));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 109, DateTimeKind.Local).AddTicks(9030));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 109, DateTimeKind.Local).AddTicks(9033));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 109, DateTimeKind.Local).AddTicks(9033));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 109, DateTimeKind.Local).AddTicks(9034));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 109, DateTimeKind.Local).AddTicks(9037));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 109, DateTimeKind.Local).AddTicks(9037));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 109, DateTimeKind.Local).AddTicks(9038));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 109, DateTimeKind.Local).AddTicks(5379));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 109, DateTimeKind.Local).AddTicks(5885));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 109, DateTimeKind.Local).AddTicks(5930));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 1 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 111, DateTimeKind.Local).AddTicks(2568));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 1, 2 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 111, DateTimeKind.Local).AddTicks(2761));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 2 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 111, DateTimeKind.Local).AddTicks(2762));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 3 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 111, DateTimeKind.Local).AddTicks(2763));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 4 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 111, DateTimeKind.Local).AddTicks(2764));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 1, 5 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 111, DateTimeKind.Local).AddTicks(2765));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 1, 6 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 111, DateTimeKind.Local).AddTicks(2766));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 3, 7 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 111, DateTimeKind.Local).AddTicks(2766));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 4, 8 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 111, DateTimeKind.Local).AddTicks(2867));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 5, 9 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 111, DateTimeKind.Local).AddTicks(2869));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 111, DateTimeKind.Local).AddTicks(267));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 111, DateTimeKind.Local).AddTicks(582));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 111, DateTimeKind.Local).AddTicks(584));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 111, DateTimeKind.Local).AddTicks(585));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 111, DateTimeKind.Local).AddTicks(585));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 111, DateTimeKind.Local).AddTicks(640));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 111, DateTimeKind.Local).AddTicks(642));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 111, DateTimeKind.Local).AddTicks(643));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 9,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 111, DateTimeKind.Local).AddTicks(643));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 110, DateTimeKind.Local).AddTicks(5627));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 110, DateTimeKind.Local).AddTicks(7833));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 110, DateTimeKind.Local).AddTicks(7994));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 106, DateTimeKind.Local).AddTicks(9305));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 108, DateTimeKind.Local).AddTicks(3424));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 108, DateTimeKind.Local).AddTicks(3446));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 108, DateTimeKind.Local).AddTicks(3447));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 108, DateTimeKind.Local).AddTicks(3448));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 22, 30, 49, 108, DateTimeKind.Local).AddTicks(3449));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "QImage",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QAudio",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(4646));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(4945));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(4947));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(4947));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(4948));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(4952));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(4952));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(4953));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(2163));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(2555));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(2576));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 1 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(11));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 1, 2 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(158));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 2 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(159));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 3 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(160));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 4 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(160));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 1, 5 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(162));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 1, 6 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(162));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 3, 7 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(163));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 4, 8 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(238));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 5, 9 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(240));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(8353));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(8581));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(8583));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(8583));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(8584));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(8586));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(8586));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(8587));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 9,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(8588));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(4186));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(6443));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(6572));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 920, DateTimeKind.Local).AddTicks(3812));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 921, DateTimeKind.Local).AddTicks(4192));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 921, DateTimeKind.Local).AddTicks(4197));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 921, DateTimeKind.Local).AddTicks(4198));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 921, DateTimeKind.Local).AddTicks(4198));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 12, 29, 20, 29, 14, 921, DateTimeKind.Local).AddTicks(4199));
        }
    }
}
