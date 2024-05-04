using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using judge.system.core.Models;

#nullable disable

namespace judge.system.core.Migrations
{
    /// <inheritdoc />
    public partial class addTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "problem",
                columns: table => new
                {
                    ProblemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    TimeLimit = table.Column<float>(type: "real", nullable: false),
                    MemoryLimit = table.Column<int>(type: "integer", nullable: false),
                    TestCases = table.Column<List<TestCase>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_problem", x => x.ProblemId);
                });

            migrationBuilder.CreateTable(
                name: "submission",
                columns: table => new
                {
                    SubmissionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsAccepted = table.Column<bool>(type: "boolean", nullable: false),
                    NumCasesPassed = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_submission", x => x.SubmissionId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "problem");

            migrationBuilder.DropTable(
                name: "submission");
        }
    }
}
