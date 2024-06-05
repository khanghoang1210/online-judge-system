using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace judge.system.core.Migrations
{
    /// <inheritdoc />
    public partial class change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_token_account_UserId",
                table: "refresh_token");

            migrationBuilder.DropIndex(
                name: "IX_refresh_token_UserId",
                table: "refresh_token");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_refresh_token_UserId",
                table: "refresh_token",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_refresh_token_account_UserId",
                table: "refresh_token",
                column: "UserId",
                principalTable: "account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
