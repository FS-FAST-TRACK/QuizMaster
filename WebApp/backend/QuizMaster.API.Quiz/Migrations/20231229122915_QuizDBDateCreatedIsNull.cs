using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizMaster.API.Quiz.Migrations
{
    /// <inheritdoc />
    public partial class QuizDBDateCreatedIsNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Types",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Questions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "QuestionDetailTypes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "QuestionDetails",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Difficulties",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "DetailTypes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Categories",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(4646), null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(4945), null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(4947), null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(4947), null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(4948), null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(4952), null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(4952), null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(4953), null });

            migrationBuilder.UpdateData(
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateUpdated",
                value: null);

            migrationBuilder.UpdateData(
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateUpdated",
                value: null);

            migrationBuilder.UpdateData(
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateUpdated",
                value: null);

            migrationBuilder.UpdateData(
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateUpdated",
                value: null);

            migrationBuilder.UpdateData(
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateUpdated",
                value: null);

            migrationBuilder.UpdateData(
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateUpdated",
                value: null);

            migrationBuilder.UpdateData(
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateUpdated",
                value: null);

            migrationBuilder.UpdateData(
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateUpdated",
                value: null);

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(2163), null });

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(2555), null });

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 922, DateTimeKind.Local).AddTicks(2576), null });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 1 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(11), null });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 1, 2 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(158), null });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 2 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(159), null });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 3 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(160), null });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 4 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(160), null });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 1, 5 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(162), null });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 1, 6 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(162), null });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 3, 7 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(163), null });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 4, 8 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(238), null });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 5, 9 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 924, DateTimeKind.Local).AddTicks(240), null });

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(8353), null });

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(8581), null });

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(8583), null });

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(8583), null });

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(8584), null });

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(8586), null });

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(8586), null });

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(8587), null });

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(8588), null });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(4186), null });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(6443), null });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 923, DateTimeKind.Local).AddTicks(6572), null });

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 920, DateTimeKind.Local).AddTicks(3812), null });

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 921, DateTimeKind.Local).AddTicks(4192), null });

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 921, DateTimeKind.Local).AddTicks(4197), null });

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 921, DateTimeKind.Local).AddTicks(4198), null });

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 921, DateTimeKind.Local).AddTicks(4198), null });

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 29, 20, 29, 14, 921, DateTimeKind.Local).AddTicks(4199), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Types",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Questions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "QuestionDetailTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "QuestionDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Difficulties",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "DetailTypes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUpdated",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(4429), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(5465), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(5470), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(5471), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(5472), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(5478), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(5479), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(5480), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateUpdated",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateUpdated",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateUpdated",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateUpdated",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateUpdated",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "DateUpdated",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "DateUpdated",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "DetailTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "DateUpdated",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 828, DateTimeKind.Local).AddTicks(8961), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(205), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 829, DateTimeKind.Local).AddTicks(272), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 1 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(9818), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 1, 2 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 832, DateTimeKind.Local).AddTicks(403), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 2 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 832, DateTimeKind.Local).AddTicks(423), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 3 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 832, DateTimeKind.Local).AddTicks(424), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 2, 4 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 832, DateTimeKind.Local).AddTicks(425), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 1, 5 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 832, DateTimeKind.Local).AddTicks(429), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 1, 6 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 832, DateTimeKind.Local).AddTicks(429), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 3, 7 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 832, DateTimeKind.Local).AddTicks(430), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 4, 8 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 832, DateTimeKind.Local).AddTicks(700), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "QuestionDetailTypes",
                keyColumns: new[] { "DetailTypeId", "QuestionDetailId" },
                keyValues: new object[] { 5, 9 },
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 832, DateTimeKind.Local).AddTicks(706), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(5400), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(6239), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(6244), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(6245), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(6246), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(6250), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(6251), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(6252), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "QuestionDetails",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(6253), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 830, DateTimeKind.Local).AddTicks(5744), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(739), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 831, DateTimeKind.Local).AddTicks(1181), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 688, DateTimeKind.Local).AddTicks(3), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 827, DateTimeKind.Local).AddTicks(2880), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 827, DateTimeKind.Local).AddTicks(2893), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 827, DateTimeKind.Local).AddTicks(2895), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 827, DateTimeKind.Local).AddTicks(2896), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Types",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DateCreated", "DateUpdated" },
                values: new object[] { new DateTime(2023, 12, 12, 15, 51, 26, 827, DateTimeKind.Local).AddTicks(2897), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
