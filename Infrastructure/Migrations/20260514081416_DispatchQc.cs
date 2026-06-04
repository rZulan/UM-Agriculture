using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DispatchQc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QcResponses_DispatchId",
                table: "QcResponses");

            migrationBuilder.CreateIndex(
                name: "IX_QcResponses_DispatchId",
                table: "QcResponses",
                column: "DispatchId",
                unique: true,
                filter: "[DispatchId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_QcResponses_DispatchId",
                table: "QcResponses");

            migrationBuilder.CreateIndex(
                name: "IX_QcResponses_DispatchId",
                table: "QcResponses",
                column: "DispatchId");
        }
    }
}
