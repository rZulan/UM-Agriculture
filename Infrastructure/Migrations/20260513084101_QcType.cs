using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QcType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DispatchId",
                table: "QcResponses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QcTypeId",
                table: "QcForms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Dispatch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantityOut = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QuantityReturn = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HarvestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    FarmId = table.Column<int>(type: "int", nullable: false),
                    PreparedById = table.Column<int>(type: "int", nullable: true),
                    CheckedById = table.Column<int>(type: "int", nullable: true),
                    ReturnedById = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dispatch_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dispatch_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dispatch_Users_CheckedById",
                        column: x => x.CheckedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Dispatch_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Dispatch_Users_PreparedById",
                        column: x => x.PreparedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Dispatch_Users_ReturnedById",
                        column: x => x.ReturnedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Dispatch_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QcTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QcTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QcTypes_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QcTypes_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                column: "QcTypeId",
                value: null);

            migrationBuilder.InsertData(
                table: "QcTypes",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "IsActive", "Name", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Dispatch", null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Outside Purchase", null, null },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Import PO", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_QcResponses_DispatchId",
                table: "QcResponses",
                column: "DispatchId");

            migrationBuilder.CreateIndex(
                name: "IX_QcForms_QcTypeId",
                table: "QcForms",
                column: "QcTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Dispatch_CheckedById",
                table: "Dispatch",
                column: "CheckedById");

            migrationBuilder.CreateIndex(
                name: "IX_Dispatch_CreatedById",
                table: "Dispatch",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Dispatch_FarmId",
                table: "Dispatch",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_Dispatch_PreparedById",
                table: "Dispatch",
                column: "PreparedById");

            migrationBuilder.CreateIndex(
                name: "IX_Dispatch_ProductId",
                table: "Dispatch",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Dispatch_ReturnedById",
                table: "Dispatch",
                column: "ReturnedById");

            migrationBuilder.CreateIndex(
                name: "IX_Dispatch_UpdatedById",
                table: "Dispatch",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QcTypes_CreatedById",
                table: "QcTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QcTypes_UpdatedById",
                table: "QcTypes",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_QcForms_QcTypes_QcTypeId",
                table: "QcForms",
                column: "QcTypeId",
                principalTable: "QcTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QcResponses_Dispatch_DispatchId",
                table: "QcResponses",
                column: "DispatchId",
                principalTable: "Dispatch",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QcForms_QcTypes_QcTypeId",
                table: "QcForms");

            migrationBuilder.DropForeignKey(
                name: "FK_QcResponses_Dispatch_DispatchId",
                table: "QcResponses");

            migrationBuilder.DropTable(
                name: "Dispatch");

            migrationBuilder.DropTable(
                name: "QcTypes");

            migrationBuilder.DropIndex(
                name: "IX_QcResponses_DispatchId",
                table: "QcResponses");

            migrationBuilder.DropIndex(
                name: "IX_QcForms_QcTypeId",
                table: "QcForms");

            migrationBuilder.DropColumn(
                name: "DispatchId",
                table: "QcResponses");

            migrationBuilder.DropColumn(
                name: "QcTypeId",
                table: "QcForms");
        }
    }
}
