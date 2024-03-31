using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MKopa.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmsMessages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    SmsText = table.Column<string>(type: "TEXT", nullable: false),
                    CountryCode = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SmsServiceProviders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CountryCode = table.Column<string>(type: "TEXT", nullable: false),
                    IsPrimary = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmsServiceProviders", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SmsMessages",
                columns: new[] { "Id", "CountryCode", "CreatedDate", "ModifiedDate", "PhoneNumber", "SmsText", "Status" },
                values: new object[] { "be2b2b50-f839-4d11-ab98-ee5ea6562314", "90", new DateTimeOffset(new DateTime(2024, 3, 28, 8, 20, 27, 237, DateTimeKind.Unspecified).AddTicks(6400), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2024, 3, 29, 8, 15, 27, 237, DateTimeKind.Unspecified).AddTicks(6409), new TimeSpan(0, 0, 0, 0, 0)), "555555555", "Hello world", 1 });

            migrationBuilder.InsertData(
                table: "SmsServiceProviders",
                columns: new[] { "Id", "CountryCode", "IsPrimary", "Name" },
                values: new object[,]
                {
                    { "3e1a033d-e821-4b6a-acef-9c51f77ec6a4", "90", false, "Provider B" },
                    { "bf3b76bf-dd12-4e31-889f-6dddbb35b23e", "90", true, "Provider A" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmsMessages");

            migrationBuilder.DropTable(
                name: "SmsServiceProviders");
        }
    }
}
