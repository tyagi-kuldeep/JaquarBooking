using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastrucure.Migrations
{
    public partial class MasterState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JB_MasterState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JB_MasterState", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AuthRole",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "8af1b710-f348-4e51-b8a6-47c2f38e850b");

            migrationBuilder.UpdateData(
                table: "AuthRole",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "b45bcfb1-d72b-44f2-8659-ee8e9ec7ecfc");

            migrationBuilder.UpdateData(
                table: "AuthUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedOnUtc", "Dob", "PasswordHash", "UpdatedOnUtc" },
                values: new object[] { "263d6dba-748e-4ba0-b650-9f48377530d3", new DateTime(2022, 12, 29, 13, 4, 57, 287, DateTimeKind.Utc).AddTicks(7406), new DateTime(2022, 12, 29, 18, 34, 57, 287, DateTimeKind.Local).AddTicks(7376), "AQAAAAEAACcQAAAAEEN5KGpVwyb8Ko6hMpYFjruab31620NzMY37ikqrJhHDy5c+r/NOSu2kwpcl6vHOXQ==", new DateTime(2022, 12, 29, 13, 4, 57, 287, DateTimeKind.Utc).AddTicks(7407) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JB_MasterState");

            migrationBuilder.UpdateData(
                table: "AuthRole",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "879c905a-a8ce-44d5-8107-60bbc72e3a74");

            migrationBuilder.UpdateData(
                table: "AuthRole",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "52dd4c91-f862-4ddd-820c-3b0aec37a3a0");

            migrationBuilder.UpdateData(
                table: "AuthUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedOnUtc", "Dob", "PasswordHash", "UpdatedOnUtc" },
                values: new object[] { "ba5029ab-465b-4ce0-b5fb-2bab2ddcbb98", new DateTime(2022, 12, 29, 12, 41, 51, 323, DateTimeKind.Utc).AddTicks(4941), new DateTime(2022, 12, 29, 18, 11, 51, 323, DateTimeKind.Local).AddTicks(4896), "AQAAAAEAACcQAAAAEOYDppHy8ehoH7aN6HKUUCbgauRbWWhh7cY4tMMcZ5FxzUjwHr8zoNdsKgaY0STQVw==", new DateTime(2022, 12, 29, 12, 41, 51, 323, DateTimeKind.Utc).AddTicks(4950) });
        }
    }
}
