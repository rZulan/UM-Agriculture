using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QualityCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QcAnswerTypes",
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
                    table.PrimaryKey("PK_QcAnswerTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QcAnswerTypes_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QcAnswerTypes_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QcCategories",
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
                    table.PrimaryKey("PK_QcCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QcCategories_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QcCategories_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QcForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QcCategoryId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QcForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QcForms_QcCategories_QcCategoryId",
                        column: x => x.QcCategoryId,
                        principalTable: "QcCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QcForms_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QcForms_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QcResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QcFormId = table.Column<int>(type: "int", nullable: false),
                    ResponderId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QcResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QcResponses_QcForms_QcFormId",
                        column: x => x.QcFormId,
                        principalTable: "QcForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QcResponses_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QcResponses_Users_ResponderId",
                        column: x => x.ResponderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QcResponses_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QcSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    QcFormId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QcSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QcSections_QcForms_QcFormId",
                        column: x => x.QcFormId,
                        principalTable: "QcForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QcSections_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QcSections_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QcQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    QcSectionId = table.Column<int>(type: "int", nullable: false),
                    QcAnswerTypeId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QcQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QcQuestions_QcAnswerTypes_QcAnswerTypeId",
                        column: x => x.QcAnswerTypeId,
                        principalTable: "QcAnswerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QcQuestions_QcSections_QcSectionId",
                        column: x => x.QcSectionId,
                        principalTable: "QcSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QcQuestions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QcQuestions_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QcAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QcResponseId = table.Column<int>(type: "int", nullable: false),
                    QcQuestionId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    UpdatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QcAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QcAnswers_QcQuestions_QcQuestionId",
                        column: x => x.QcQuestionId,
                        principalTable: "QcQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QcAnswers_QcResponses_QcResponseId",
                        column: x => x.QcResponseId,
                        principalTable: "QcResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QcAnswers_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QcAnswers_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "QcAnswerTypes",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "IsActive", "Name", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Inputtext", null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Yes/No", null, null }
                });

            migrationBuilder.InsertData(
                table: "QcCategories",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "IsActive", "Name", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Horticulture", null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, "Aquaculture", null, null }
                });

            migrationBuilder.InsertData(
                table: "QcForms",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "IsActive", "QcCategoryId", "UpdatedAt", "UpdatedById" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, 1, null, null },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, 2, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_QcAnswers_CreatedById",
                table: "QcAnswers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QcAnswers_QcQuestionId",
                table: "QcAnswers",
                column: "QcQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QcAnswers_QcResponseId",
                table: "QcAnswers",
                column: "QcResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_QcAnswers_UpdatedById",
                table: "QcAnswers",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QcAnswerTypes_CreatedById",
                table: "QcAnswerTypes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QcAnswerTypes_UpdatedById",
                table: "QcAnswerTypes",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QcCategories_CreatedById",
                table: "QcCategories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QcCategories_UpdatedById",
                table: "QcCategories",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QcForms_CreatedById",
                table: "QcForms",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QcForms_QcCategoryId",
                table: "QcForms",
                column: "QcCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_QcForms_UpdatedById",
                table: "QcForms",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QcQuestions_CreatedById",
                table: "QcQuestions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QcQuestions_QcAnswerTypeId",
                table: "QcQuestions",
                column: "QcAnswerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QcQuestions_QcSectionId",
                table: "QcQuestions",
                column: "QcSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_QcQuestions_UpdatedById",
                table: "QcQuestions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QcResponses_CreatedById",
                table: "QcResponses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QcResponses_QcFormId",
                table: "QcResponses",
                column: "QcFormId");

            migrationBuilder.CreateIndex(
                name: "IX_QcResponses_ResponderId",
                table: "QcResponses",
                column: "ResponderId");

            migrationBuilder.CreateIndex(
                name: "IX_QcResponses_UpdatedById",
                table: "QcResponses",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QcSections_CreatedById",
                table: "QcSections",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_QcSections_QcFormId",
                table: "QcSections",
                column: "QcFormId");

            migrationBuilder.CreateIndex(
                name: "IX_QcSections_UpdatedById",
                table: "QcSections",
                column: "UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QcAnswers");

            migrationBuilder.DropTable(
                name: "QcQuestions");

            migrationBuilder.DropTable(
                name: "QcResponses");

            migrationBuilder.DropTable(
                name: "QcAnswerTypes");

            migrationBuilder.DropTable(
                name: "QcSections");

            migrationBuilder.DropTable(
                name: "QcForms");

            migrationBuilder.DropTable(
                name: "QcCategories");
        }
    }
}
