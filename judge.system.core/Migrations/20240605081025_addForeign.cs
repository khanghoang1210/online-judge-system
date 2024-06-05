using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace judge.system.core.Migrations
{
    /// <inheritdoc />
    public partial class addForeign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "refresh_token",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refresh_token_account_UserId",
                table: "refresh_token");

            migrationBuilder.DropIndex(
                name: "IX_refresh_token_UserId",
                table: "refresh_token");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "refresh_token",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
