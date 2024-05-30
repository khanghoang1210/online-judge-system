using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace judge.system.core.Migrations
{
    /// <inheritdoc />
    public partial class addForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tag_problem_ProblemId",
                table: "tag");

            migrationBuilder.DropIndex(
                name: "IX_tag_ProblemId",
                table: "tag");

            migrationBuilder.DropColumn(
                name: "ProblemId",
                table: "tag");

            migrationBuilder.CreateTable(
                name: "problem_tag",
                columns: table => new
                {
                    ProblemId = table.Column<int>(type: "integer", nullable: false),
                    TagId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_problem_tag", x => new { x.ProblemId, x.TagId });
                    table.ForeignKey(
                        name: "FK_problem_tag_problem_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "problem",
                        principalColumn: "ProblemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_problem_tag_tag_TagId",
                        column: x => x.TagId,
                        principalTable: "tag",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_problem_tag_TagId",
                table: "problem_tag",
                column: "TagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "problem_tag");

            migrationBuilder.AddColumn<int>(
                name: "ProblemId",
                table: "tag",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tag_ProblemId",
                table: "tag",
                column: "ProblemId");

            migrationBuilder.AddForeignKey(
                name: "FK_tag_problem_ProblemId",
                table: "tag",
                column: "ProblemId",
                principalTable: "problem",
                principalColumn: "ProblemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
