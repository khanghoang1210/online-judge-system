using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using judge.system.core.Models;

#nullable disable

namespace judge.system.core.Migrations
{
    /// <inheritdoc />
    public partial class changeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemoryLimit",
                table: "problem");

            migrationBuilder.DropColumn(
                name: "TestCases",
                table: "problem");

            migrationBuilder.DropColumn(
                name: "TimeLimit",
                table: "problem");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "problem",
                newName: "TitleSlug");

            migrationBuilder.AddColumn<string>(
                name: "Difficulty",
                table: "problem",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<List<string>>(
                name: "TagId",
                table: "problem",
                type: "text[]",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    JwtId = table.Column<string>(type: "text", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "problem");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "problem");

            migrationBuilder.RenameColumn(
                name: "TitleSlug",
                table: "problem",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "MemoryLimit",
                table: "problem",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<List<TestCase>>(
                name: "TestCases",
                table: "problem",
                type: "jsonb",
                nullable: false);

            migrationBuilder.AddColumn<float>(
                name: "TimeLimit",
                table: "problem",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
