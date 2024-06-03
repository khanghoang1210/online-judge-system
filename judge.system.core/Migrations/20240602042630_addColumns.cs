using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace judge.system.core.Migrations
{
    /// <inheritdoc />
    public partial class addColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "submission",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "submission",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_submission_UserId",
                table: "submission",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_submission_account_UserId",
                table: "submission",
                column: "UserId",
                principalTable: "account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_submission_account_UserId",
                table: "submission");

            migrationBuilder.DropIndex(
                name: "IX_submission_UserId",
                table: "submission");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "submission");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "submission");
        }
    }
}
