using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizMaster.API.Quiz.Migrations
{
    /// <inheritdoc />
    public partial class QuizDbAllowNulQImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(4429));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(5465));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(5470));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(5471));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(5472));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(5478));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(5479));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(5480));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 828, DateTimeKind.Local).AddTicks(8961));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(205));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(272));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 1 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(9818));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 1, 2 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 832, DateTimeKind.Local).AddTicks(403));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 2 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 832, DateTimeKind.Local).AddTicks(423));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 3 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 832, DateTimeKind.Local).AddTicks(424));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 4 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 832, DateTimeKind.Local).AddTicks(425));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 1, 5 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 832, DateTimeKind.Local).AddTicks(429));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 1, 6 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 832, DateTimeKind.Local).AddTicks(429));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 3, 7 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 832, DateTimeKind.Local).AddTicks(430));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 4, 8 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 832, DateTimeKind.Local).AddTicks(700));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 5, 9 },
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 832, DateTimeKind.Local).AddTicks(706));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(5400));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(6239));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(6244));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(6245));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(6246));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(6250));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(6251));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(6252));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 9,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(6253));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 830, DateTimeKind.Local).AddTicks(5744));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(739));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(1181));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 688, DateTimeKind.Local).AddTicks(3));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 827, DateTimeKind.Local).AddTicks(2880));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 827, DateTimeKind.Local).AddTicks(2893));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 827, DateTimeKind.Local).AddTicks(2895));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 827, DateTimeKind.Local).AddTicks(2896));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 12, 12, 15, 51, 26, 827, DateTimeKind.Local).AddTicks(2897));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 782, DateTimeKind.Local).AddTicks(4541));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 782, DateTimeKind.Local).AddTicks(5806));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 782, DateTimeKind.Local).AddTicks(5812));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 782, DateTimeKind.Local).AddTicks(5813));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 782, DateTimeKind.Local).AddTicks(5814));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 782, DateTimeKind.Local).AddTicks(5826));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 782, DateTimeKind.Local).AddTicks(5827));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 782, DateTimeKind.Local).AddTicks(5828));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 781, DateTimeKind.Local).AddTicks(5905));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 781, DateTimeKind.Local).AddTicks(7628));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 781, DateTimeKind.Local).AddTicks(7771));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 1 },
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(3760));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 1, 2 },
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(4559));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 2 },
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(4563));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 3 },
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(4695));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 4 },
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(4697));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 1, 5 },
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(4704));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 1, 6 },
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(4705));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 3, 7 },
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(4705));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 4, 8 },
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(5087));

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 5, 9 },
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 786, DateTimeKind.Local).AddTicks(5096));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 785, DateTimeKind.Local).AddTicks(6520));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 785, DateTimeKind.Local).AddTicks(7828));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 785, DateTimeKind.Local).AddTicks(7833));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 785, DateTimeKind.Local).AddTicks(7834));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 785, DateTimeKind.Local).AddTicks(7835));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 785, DateTimeKind.Local).AddTicks(7842));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 785, DateTimeKind.Local).AddTicks(7843));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 785, DateTimeKind.Local).AddTicks(7844));

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 9,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 785, DateTimeKind.Local).AddTicks(7845));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 783, DateTimeKind.Local).AddTicks(9253));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 784, DateTimeKind.Local).AddTicks(8287));

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 784, DateTimeKind.Local).AddTicks(9447));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 776, DateTimeKind.Local).AddTicks(5234));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 778, DateTimeKind.Local).AddTicks(7491));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 778, DateTimeKind.Local).AddTicks(7522));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 778, DateTimeKind.Local).AddTicks(7524));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 778, DateTimeKind.Local).AddTicks(7526));

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2023, 11, 14, 22, 26, 23, 778, DateTimeKind.Local).AddTicks(7527));
        }
    }
}
