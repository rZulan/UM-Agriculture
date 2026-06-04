using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Dispatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dispatch_Farms_FarmId",
                table: "Dispatch");

            migrationBuilder.DropForeignKey(
                name: "FK_Dispatch_Products_ProductId",
                table: "Dispatch");

            migrationBuilder.DropForeignKey(
                name: "FK_Dispatch_Users_CheckedById",
                table: "Dispatch");

            migrationBuilder.DropForeignKey(
                name: "FK_Dispatch_Users_CreatedById",
                table: "Dispatch");

            migrationBuilder.DropForeignKey(
                name: "FK_Dispatch_Users_PreparedById",
                table: "Dispatch");

            migrationBuilder.DropForeignKey(
                name: "FK_Dispatch_Users_ReturnedById",
                table: "Dispatch");

            migrationBuilder.DropForeignKey(
                name: "FK_Dispatch_Users_UpdatedById",
                table: "Dispatch");

            migrationBuilder.DropForeignKey(
                name: "FK_QcResponses_Dispatch_DispatchId",
                table: "QcResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dispatch",
                table: "Dispatch");

            migrationBuilder.RenameTable(
                name: "Dispatch",
                newName: "Dispatches");

            migrationBuilder.RenameIndex(
                name: "IX_Dispatch_UpdatedById",
                table: "Dispatches",
                newName: "IX_Dispatches_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Dispatch_ReturnedById",
                table: "Dispatches",
                newName: "IX_Dispatches_ReturnedById");

            migrationBuilder.RenameIndex(
                name: "IX_Dispatch_ProductId",
                table: "Dispatches",
                newName: "IX_Dispatches_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Dispatch_PreparedById",
                table: "Dispatches",
                newName: "IX_Dispatches_PreparedById");

            migrationBuilder.RenameIndex(
                name: "IX_Dispatch_FarmId",
                table: "Dispatches",
                newName: "IX_Dispatches_FarmId");

            migrationBuilder.RenameIndex(
                name: "IX_Dispatch_CreatedById",
                table: "Dispatches",
                newName: "IX_Dispatches_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Dispatch_CheckedById",
                table: "Dispatches",
                newName: "IX_Dispatches_CheckedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dispatches",
                table: "Dispatches",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispatches_Farms_FarmId",
                table: "Dispatches",
                column: "FarmId",
                principalTable: "Farms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dispatches_Products_ProductId",
                table: "Dispatches",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dispatches_Users_CheckedById",
                table: "Dispatches",
                column: "CheckedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dispatches_Users_CreatedById",
                table: "Dispatches",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dispatches_Users_PreparedById",
                table: "Dispatches",
                column: "PreparedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dispatches_Users_ReturnedById",
                table: "Dispatches",
                column: "ReturnedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dispatches_Users_UpdatedById",
                table: "Dispatches",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QcResponses_Dispatches_DispatchId",
                table: "QcResponses",
                column: "DispatchId",
                principalTable: "Dispatches",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dispatches_Farms_FarmId",
                table: "Dispatches");

            migrationBuilder.DropForeignKey(
                name: "FK_Dispatches_Products_ProductId",
                table: "Dispatches");

            migrationBuilder.DropForeignKey(
                name: "FK_Dispatches_Users_CheckedById",
                table: "Dispatches");

            migrationBuilder.DropForeignKey(
                name: "FK_Dispatches_Users_CreatedById",
                table: "Dispatches");

            migrationBuilder.DropForeignKey(
                name: "FK_Dispatches_Users_PreparedById",
                table: "Dispatches");

            migrationBuilder.DropForeignKey(
                name: "FK_Dispatches_Users_ReturnedById",
                table: "Dispatches");

            migrationBuilder.DropForeignKey(
                name: "FK_Dispatches_Users_UpdatedById",
                table: "Dispatches");

            migrationBuilder.DropForeignKey(
                name: "FK_QcResponses_Dispatches_DispatchId",
                table: "QcResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dispatches",
                table: "Dispatches");

            migrationBuilder.RenameTable(
                name: "Dispatches",
                newName: "Dispatch");

            migrationBuilder.RenameIndex(
                name: "IX_Dispatches_UpdatedById",
                table: "Dispatch",
                newName: "IX_Dispatch_UpdatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Dispatches_ReturnedById",
                table: "Dispatch",
                newName: "IX_Dispatch_ReturnedById");

            migrationBuilder.RenameIndex(
                name: "IX_Dispatches_ProductId",
                table: "Dispatch",
                newName: "IX_Dispatch_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Dispatches_PreparedById",
                table: "Dispatch",
                newName: "IX_Dispatch_PreparedById");

            migrationBuilder.RenameIndex(
                name: "IX_Dispatches_FarmId",
                table: "Dispatch",
                newName: "IX_Dispatch_FarmId");

            migrationBuilder.RenameIndex(
                name: "IX_Dispatches_CreatedById",
                table: "Dispatch",
                newName: "IX_Dispatch_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Dispatches_CheckedById",
                table: "Dispatch",
                newName: "IX_Dispatch_CheckedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dispatch",
                table: "Dispatch",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispatch_Farms_FarmId",
                table: "Dispatch",
                column: "FarmId",
                principalTable: "Farms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dispatch_Products_ProductId",
                table: "Dispatch",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dispatch_Users_CheckedById",
                table: "Dispatch",
                column: "CheckedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispatch_Users_CreatedById",
                table: "Dispatch",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispatch_Users_PreparedById",
                table: "Dispatch",
                column: "PreparedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispatch_Users_ReturnedById",
                table: "Dispatch",
                column: "ReturnedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispatch_Users_UpdatedById",
                table: "Dispatch",
                column: "UpdatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QcResponses_Dispatch_DispatchId",
                table: "QcResponses",
                column: "DispatchId",
                principalTable: "Dispatch",
                principalColumn: "Id");
        }
    }
}
