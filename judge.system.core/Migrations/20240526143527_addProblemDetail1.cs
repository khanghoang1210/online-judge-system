using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using judge.system.core.Models;

#nullable disable

namespace judge.system.core.Migrations
{
    /// <inheritdoc />
    public partial class addProblemDetail1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "problem_detail",
                columns: table => new
                {
                    ProblemDetailId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProblemId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    TimeLimit = table.Column<float>(type: "real", nullable: false),
                    MemoryLimit = table.Column<int>(type: "integer", nullable: false),
                    TestCases = table.Column<List<TestCase>>(type: "jsonb", nullable: false),
                    Hint = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_problem_detail", x => x.ProblemDetailId);
                    table.ForeignKey(
                        name: "FK_problem_detail_problem_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "problem",
                        principalColumn: "ProblemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_problem_detail_ProblemId",
                table: "problem_detail",
                column: "ProblemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "problem_detail");
        }
    }
}
