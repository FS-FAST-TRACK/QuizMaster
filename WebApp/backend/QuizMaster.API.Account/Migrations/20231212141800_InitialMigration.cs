using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizMaster.API.Account.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7bb9a586-eaf9-4003-a967-d59e6080baea");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "e81908e7-b968-44ed-b959-afb46e36ef0c");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "DateCreated", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e0c1e9b3-f2a9-4449-8246-c3512b52d739", new DateTime(2023, 12, 12, 14, 18, 0, 827, DateTimeKind.Utc).AddTicks(4812), "AQAAAAEAACcQAAAAENOVn6zyRk1v8ClGsGZgBABLNKdVXnVuHGwGRrwvFSszGaZBNWNmnC7GtGNm+ksZMw==", "0f8334f6-1abe-4240-a377-208623afe882" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "599476f9-5b63-4c59-b662-6cf749e3fe67");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "7f89f3b2-569d-4f38-b0f2-297fe007d11d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "DateCreated", "PasswordHash", "SecurityStamp" },
                values: new object[] { "624f1bad-0ff0-41db-b236-672fcff42fd2", new DateTime(2023, 10, 19, 9, 19, 41, 677, DateTimeKind.Utc).AddTicks(1076), "AQAAAAEAACcQAAAAEMXYJbdGVE5UBm2FnIGB2U0zr2zBru8HMiU612F+6wfiG4U+FPKx04mwEnpsZJZ5WA==", "1cb9a652-7a89-4197-b7df-deb5bded7205" });
        }
    }
}
