using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QcResponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QcForms_QcTypes_QcTypeId",
                table: "QcForms");

            migrationBuilder.AddColumn<int>(
                name: "OutsourceId",
                table: "QcResponses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderId",
                table: "QcResponses",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "QcTypeId",
                table: "QcForms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Outsource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outsource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Outsource_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Outsource_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrder_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrder_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "QcForms",
                keyColumn: "Id",
                keyValue: 1,
                column: "QcTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "QcForms",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "QcCategoryId", "QcTypeId" },
                values: new object[] { 1, 2 });

            migrationBuilder.InsertData(
                table: "QcForms",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "IsActive", "QcCategoryId", "QcTypeId", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, 1, 3, null, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, 2, 3, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_QcResponses_OutsourceId",
                table: "QcResponses",
                column: "OutsourceId");

            migrationBuilder.CreateIndex(
                name: "IX_QcResponses_PurchaseOrderId",
                table: "QcResponses",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Outsource_CreatedById",
                table: "Outsource",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Outsource_UpdatedById",
                table: "Outsource",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_CreatedById",
                table: "PurchaseOrder",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_UpdatedById",
                table: "PurchaseOrder",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_QcForms_QcTypes_QcTypeId",
                table: "QcForms",
                column: "QcTypeId",
                principalTable: "QcTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QcResponses_Outsource_OutsourceId",
                table: "QcResponses",
                column: "OutsourceId",
                principalTable: "Outsource",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QcResponses_PurchaseOrder_PurchaseOrderId",
                table: "QcResponses",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrder",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QcForms_QcTypes_QcTypeId",
                table: "QcForms");

            migrationBuilder.DropForeignKey(
                name: "FK_QcResponses_Outsource_OutsourceId",
                table: "QcResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_QcResponses_PurchaseOrder_PurchaseOrderId",
                table: "QcResponses");

            migrationBuilder.DropTable(
                name: "Outsource");

            migrationBuilder.DropTable(
                name: "PurchaseOrder");

            migrationBuilder.DropIndex(
                name: "IX_QcResponses_OutsourceId",
                table: "QcResponses");

            migrationBuilder.DropIndex(
                name: "IX_QcResponses_PurchaseOrderId",
                table: "QcResponses");

            migrationBuilder.DeleteData(
                table: "QcForms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "QcForms",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "OutsourceId",
                table: "QcResponses");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderId",
                table: "QcResponses");

            migrationBuilder.AlterColumn<int>(
                name: "QcTypeId",
                table: "QcForms",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "QcForms",
                keyColumn: "Id",
                keyValue: 1,
                column: "QcTypeId",
                value: null);

            migrationBuilder.UpdateData(
                table: "QcForms",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "QcCategoryId", "QcTypeId" },
                values: new object[] { 2, null });

            migrationBuilder.AddForeignKey(
                name: "FK_QcForms_QcTypes_QcTypeId",
                table: "QcForms",
                column: "QcTypeId",
                principalTable: "QcTypes",
                principalColumn: "Id");
        }
    }
}
