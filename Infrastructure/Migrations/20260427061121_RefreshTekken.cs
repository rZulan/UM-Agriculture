using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTekken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "RefreshTokens",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "RefreshTokens",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "RefreshTokens",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_CreatedById",
                table: "RefreshTokens",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UpdatedById",
                table: "RefreshTokens",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_CreatedById",
                table: "RefreshTokens",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UpdatedById",
                table: "RefreshTokens",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_CreatedById",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_UpdatedById",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_CreatedById",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_UpdatedById",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "RefreshTokens");
        }
    }
}
